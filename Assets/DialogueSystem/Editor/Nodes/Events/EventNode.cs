using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem;

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
        private INodeOption _eventOption;
        private INodeOption _valueOption;

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _eventOption = DialogueGraphUtility.AddNodeOption(context, "Event", typeof(DSEventObject));
            
            // Add option based on Type parameter of Event
            _eventOption.TryGetValue(out DSEventObject eventObject);
            if (eventObject != null)
            {
                Type valueType = GetEventGenericType(eventObject);
                if (valueType != null)
                {
                    _valueOption = DialogueGraphUtility.AddNodeOption(context, "Value", valueType);
                }
            }
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineDataInputPort(context);
        }

        public DSEventReference GetData()
        {
            _eventOption.TryGetValue(out DSEventObject eventObject);
            if (eventObject == null)
                return null;
            
            // Return non-typed event reference if the Event has no type parameter
            Type selectedType = GetEventGenericType(eventObject);
            if (selectedType == null)
                return GetNonTypedReference(eventObject);
            
            return GetTypedReference(eventObject, selectedType);
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
            MethodInfo getPortValue = typeof(INodeOption)
                .GetMethod(nameof(INodeOption.TryGetValue))
                .MakeGenericMethod(selectedType);
            
            var args = new object[] { null };
            getPortValue.Invoke(_valueOption, args);
            object value = args[0];

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
            _eventOption.TryGetValue(out DSEventObject eventObject);
            if (eventObject == null)
                infos.LogWarning("EventNode must have a Event Object Assigned", this);
            
        }
    }
}