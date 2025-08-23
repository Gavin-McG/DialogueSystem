using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Node used to define where/how events are called in a graph.
    /// Defines a port based on the type parameter T of the assigned DSEvent 
    /// </summary>
    [Serializable]
    internal class EventNode : Node, IDataNode<DSEventReference>, IErrorNode
    {
        private const string EventOptionName = "eventObject";
        private const string ValueOptionName = "value";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            var eventOption = DialogueGraphUtility.AddNodeOption(context, EventOptionName, typeof(DSEventObject), "Event");
            
            //add option for event value
            eventOption.TryGetValue(out DSEventObject eventObject);
            if (eventObject != null)
            {
                Type valueType = GetEventGenericType(eventObject);
                if (valueType != null)
                {
                    DialogueGraphUtility.AddNodeOption(context, ValueOptionName, valueType, "Value");
                }
            }
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineDataInputPort(context);
        }

        public DSEventReference GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            DSEventObject eventObject = DialogueGraphUtility.GetOptionValueOrDefault<DSEventObject>(this, EventOptionName);
            if (eventObject == null)
                return null;

            Type selectedType = GetEventGenericType(eventObject);
            if (selectedType == null)
                return GetNonTypedReference(eventObject);
            else
            {
                return GetTypedReference(eventObject, selectedType);
            }
        }

        public DSEventReference GetNonTypedReference(DSEventObject eventObject)
        {
            return new DSEventCaller()
            {
                dialogueEvent = (DSEvent)eventObject
            };
        }

        public DSEventReference GetTypedReference(DSEventObject eventObject, Type selectedType)
        {
            // Build generic type DSEventCaller<T>
            Type callerType = typeof(DSEventCaller<>).MakeGenericType(selectedType);

            // Get the port value dynamically
            MethodInfo getPortValue = typeof(DialogueGraphUtility)
                .GetMethod(nameof(DialogueGraphUtility.GetPortValueOrDefault))
                .MakeGenericMethod(selectedType);

            object value = getPortValue.Invoke(null, new object[] { this, ValueOptionName });

            // Create caller instance
            object newCaller = Activator.CreateInstance(callerType);

            // Assign fields
            callerType.GetField("dialogueEvent")?.SetValue(newCaller, eventObject);
            callerType.GetField("value")?.SetValue(newCaller, value);

            return (DSEventReference)newCaller;
        }
        
        private static Type GetEventGenericType(DSEventObject eventObject)
        {
            Type type = eventObject.GetType();

            // Walk up inheritance chain until we find DSEvent<T>
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(DSEvent<>))
                {
                    return type.GetGenericArguments()[0];
                }
                type = type.BaseType;
            }

            return null;
        }

        public void DisplayErrors(GraphLogger infos)
        {
            var eventObject = DialogueGraphUtility.GetOptionValueOrDefault<DSEventObject>(this, EventOptionName);
            if (eventObject == null)
                infos.LogWarning("EventNode must have a Event Object Assigned", this);
            
        }
    }
}