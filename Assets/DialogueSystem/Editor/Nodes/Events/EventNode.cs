using System;
using System.Collections.Generic;
using System.Reflection;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class EventNode : Node, IDataNode<DSEventReference>, IErrorNode
    {
        private const string EventOptionName = "eventObject";
        private const string ValuePortName = "value";

        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption(EventOptionName, typeof(DSEventObject), "Event");
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            // Try to get the event object
            DSEventObject eventObject = DialogueGraphUtility.GetOptionValueOrDefault<DSEventObject>(this, EventOptionName);

            if (eventObject != null)
            {
                Type valueType = GetEventGenericType(eventObject);
                if (valueType != null)
                {
                    context.AddInputPort(ValuePortName)
                        .WithDataType(valueType)
                        .WithDisplayName("Value")
                        .Delayed()
                        .Build();
                }
            }

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

            object value = getPortValue.Invoke(null, new object[] { this, ValuePortName });

            // Create caller instance
            object newCaller = Activator.CreateInstance(callerType);

            // Assign fields
            callerType.GetField("dialogueEvent")?.SetValue(newCaller, eventObject);
            callerType.GetField("value")?.SetValue(newCaller, value);

            return (DSEventReference)newCaller;
        }

        /// <summary>
        /// Gets the T from a DSEvent&lt;T&gt; instance.
        /// </summary>
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