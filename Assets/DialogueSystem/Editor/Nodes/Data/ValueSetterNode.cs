using System;
using System.Collections.Generic;
using System.Reflection;
using DialogueSystem.Runtime;
using DialogueSystem.Runtime.Values;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public class ValueSetterNode : Node, IDataNode<ValueEditor>
    {
        private const string ValueNameOptionName = "valueName";
        private const string ValueScopeOptionName = "scope";
        private const string ValueTypeOptionName = "valueType";
        private const string ValuePortName = "value";
        
        private enum ValueTypes
        {
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
        
        private readonly Dictionary<ValueTypes, Type> _typeDict = new Dictionary<ValueTypes, Type>()
        {
            { ValueTypes.String, typeof(string)}, 
            { ValueTypes.Int, typeof(int)}, 
            { ValueTypes.Float, typeof(float)}, 
            { ValueTypes.Bool, typeof(bool)}, 
            { ValueTypes.Vector2, typeof(Vector2)}, 
            { ValueTypes.Vector3, typeof(Vector3)}, 
            { ValueTypes.Vector4, typeof(Vector4)}, 
            { ValueTypes.GameObject, typeof(GameObject)}, 
            { ValueTypes.AudioClip, typeof(AudioClip)},
        };
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            //DialogueGraphUtility.DefineFieldOptions<ValueSetter<T>>(context);
            
            context.AddNodeOption(ValueNameOptionName, typeof(string), "Value Name", defaultValue:"MyValue");
            context.AddNodeOption(ValueScopeOptionName, typeof(ValueScope), "Scope", defaultValue:ValueScope.Dialogue);
            context.AddNodeOption(ValueTypeOptionName, typeof(ValueTypes), "Value Type", defaultValue:ValueTypes.String);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            ValueTypes valueType = DialogueGraphUtility.GetOptionValueOrDefault<ValueTypes>(this, ValueTypeOptionName);
            Type portType = _typeDict[valueType];
            context.AddInputPort(ValuePortName)
                .WithDisplayName("Value")
                .WithDataType(portType)
                .Delayed()
                .Build();
            
            DialogueGraphUtility.DefineNodeInputPort(context, "");
        }

        public ValueEditor GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            // Get user selections from options
            string valueName = DialogueGraphUtility.GetOptionValueOrDefault<string>(this, ValueNameOptionName);
            ValueScope scope = DialogueGraphUtility.GetOptionValueOrDefault<ValueScope>(this, ValueScopeOptionName);
            ValueTypes valueType = DialogueGraphUtility.GetOptionValueOrDefault<ValueTypes>(this, ValueTypeOptionName);

            // Resolve actual type from enum
            if (!_typeDict.TryGetValue(valueType, out Type selectedType))
                return null;

            // Get the input port value for that type dynamically
            MethodInfo getPortValue = typeof(DialogueGraphUtility)
                .GetMethod(nameof(DialogueGraphUtility.GetPortValueOrDefault))
                .MakeGenericMethod(selectedType);

            object value = getPortValue.Invoke(null, new object[] { this, ValuePortName });

            // Build a ValueSetter<T> for the correct type
            Type setterType = typeof(ValueSetter<>).MakeGenericType(selectedType);
            object setterInstance = Activator.CreateInstance(setterType);

            // Assign fields
            setterType.GetField("valueName")?.SetValue(setterInstance, valueName);
            setterType.GetField("scope")?.SetValue(setterInstance, scope);
            setterType.GetField("value")?.SetValue(setterInstance, value);

            return (ValueEditor)setterInstance;
        }
    }
}