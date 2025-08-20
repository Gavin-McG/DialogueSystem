using System;
using System.Collections.Generic;
using System.Reflection;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class GeneralEventTestNode : Node, IDataNode<DSEventReference>
    {
        private const string EventOptionName = "eventObject";
        private const string TypeOptionName = "type";
        private const string ValuePortName = "value";
        
        private enum EventTypes {
            String,
            Int,
            Float,
            Bool,
            Vector2,
            Vector3,
            Vector4,
            GameObject,
            AudioClip,
        }
        
        private readonly Dictionary<EventTypes, Type> _typeDict = new Dictionary<EventTypes, Type>()
        {
            { EventTypes.String, typeof(string)},
            { EventTypes.Int, typeof(int)},
            { EventTypes.Float, typeof(float)},
            { EventTypes.Bool, typeof(bool)},
            { EventTypes.Vector2, typeof(Vector2)},
            { EventTypes.Vector3, typeof(Vector3)},
            { EventTypes.Vector4, typeof(Vector4)},
            { EventTypes.GameObject, typeof(GameObject)},
            { EventTypes.AudioClip, typeof(AudioClip)},
        };
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption(EventOptionName, typeof(DSEventObject), "Event");
            context.AddNodeOption(TypeOptionName, typeof(EventTypes), "Type");
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            EventTypes type = DialogueGraphUtility.GetOptionValueOrDefault<EventTypes>(this, TypeOptionName);
            Type eventType = _typeDict[type];
            if (eventType != null) context.AddInputPort(ValuePortName)
                .WithDataType(eventType)
                .WithDisplayName("Value")
                .Delayed()
                .Build();
            
            DialogueGraphUtility.DefineEventInputPort(context);
        }

        public DSEventReference GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            DSEventObject eventObject = DialogueGraphUtility.GetOptionValueOrDefault<DSEventObject>(this, EventOptionName);
            EventTypes typeOption = DialogueGraphUtility.GetOptionValueOrDefault<EventTypes>(this, TypeOptionName);
            Type selectedType = _typeDict[typeOption];

            if (eventObject == null)
            {
                return null;
            }

            // Build generic type DSEventCaller<T>
            Type eventType = typeof(DSEvent<>).MakeGenericType(selectedType);
            Type callerType = typeof(DSEventCaller<>).MakeGenericType(selectedType);

            // Ensure eventObject is of that type
            if (!eventType.IsInstanceOfType(eventObject))
                return null;

            // Get the value using reflection (generic method call)
            MethodInfo getPortValue = typeof(DialogueGraphUtility)
                .GetMethod(nameof(DialogueGraphUtility.GetPortValueOrDefault))
                .MakeGenericMethod(selectedType);

            object value = getPortValue.Invoke(null, new object[] { this, ValuePortName });

            // Create new instance of DSEventCaller<T>
            object newCaller = Activator.CreateInstance(callerType);

            // Copy over dialogueEvent and value fields
            callerType.GetField("dialogueEvent")?.SetValue(newCaller, eventObject);
            callerType.GetField("value")?.SetValue(newCaller, value);

            return (DSEventReference)newCaller;
        }
    }
}