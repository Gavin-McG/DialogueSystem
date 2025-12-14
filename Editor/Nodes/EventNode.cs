using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Node used to define where/how events are called in a graph.
    /// Defines a port based on the type parameter T of the assigned DSEvent 
    /// </summary>
    [Serializable]
    [UseWithGraph(typeof(DialogueGraph))]
    internal class EventNode : Node, IDialogueNode
    {
        private EventObject _asset;
        private INodeOption _eventOption;
        private INodeOption _valueOption;
        private IPort _nextPort;
        
        private static Type GetDSEventValueType(Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DSEvent<>))
                    return type.GetGenericArguments()[0];

                type = type.BaseType;
            }

            return null;
        }

        private Type GetSelectedType()
        {
            if (_eventOption == null) return null;
            _eventOption.TryGetValue(out DSEventBase currentEvent);
            
            //DSEvent has no value type
            if (currentEvent is DSEvent)
                return null;
            
            //Get DSEvent<T> template type
            return GetDSEventValueType(currentEvent.GetType());
        }
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _eventOption = context.AddOption<DSEventBase>("Event").WithDisplayName("").Build();

            Type valueType = GetSelectedType();
            if (valueType != null)
                _valueOption = context.AddOption("value", valueType).Build();
            else
                _valueOption = null;
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddPreviousPort(context);
            _nextPort = DialogueGraphUtility.AddNextPort(context);
        }
  
        //-------------------------------------------
        //          DialogueNode Methods
        //-------------------------------------------

        public ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<EventObject>();
            return _asset;
        }
        
        public void AssignObjectReferences()
        {
            //Get Next Dialogue
            _asset.nextDialogue = DialogueGraphUtility.GetTrace(_nextPort);
            
            //Assign EventReference
            var valueType = GetSelectedType();
            if (valueType != null)
            {
                // Create EventCaller<T>
                Type callerType = typeof(EventCaller<>).MakeGenericType(valueType);
                var caller = (EventReference)Activator.CreateInstance(callerType);

                // Assign dialogueEvent
                _eventOption.TryGetValue(out DSEventBase dialogueEvent);
                callerType
                    .GetField("dialogueEvent")
                    ?.SetValue(caller, dialogueEvent);

                // Assign value
                if (_valueOption != null)
                {
                    _valueOption.TryGetValue(out object value);
                    callerType
                        .GetField("value")
                        ?.SetValue(caller, value);
                }

                _asset.eventCaller = caller;
            }
            else
            {
                var caller = new EventCaller();
                _eventOption.TryGetValue(out caller.dialogueEvent);
                _asset.eventCaller = caller;
            }
                
        }

        public DialogueObject GetData()
        {
            return _asset;
        }
    }
}