using System;
using System.Collections.Generic;
using System.Reflection;
using WolverineSoft.DialogueSystem;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Node for setting a value from a set of pre-defined existing types. 
    /// </summary>
    [Serializable]
    internal class ValueSetterNode : Node, IDataNode<ValueEditor>, IErrorNode
    {
        private const string ValueSOOptionName = "valueSO";
        private const string ValueScopeOptionName = "scope";
        private const string ValueTypeOptionName = "valueType";
        private const string ValueOptionName = "value";
        
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
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            DialogueGraphUtility.AddNodeOption(context, 
                ValueSOOptionName, typeof(ValueSO), "ValueSO");
            DialogueGraphUtility.AddNodeOption(context, 
                ValueScopeOptionName, typeof(ValueScope), "Scope", defaultValue:ValueScope.Dialogue);
            var valueTypeOption = DialogueGraphUtility.AddNodeOption(context, 
                ValueTypeOptionName, typeof(ValueTypes), "Value Type", defaultValue:ValueTypes.String);
            
            //set value option based on selected value type
            valueTypeOption.TryGetValue(out ValueTypes valueType);
            var optionType = _typeDict[valueType];
            DialogueGraphUtility.AddNodeOption(context, ValueOptionName, optionType, "Value");
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineDataInputPort(context);
        }

        public ValueEditor GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            // Get user selections from options
            ValueSO valueSO = DialogueGraphUtility.GetOptionValueOrDefault<ValueSO>(this, ValueSOOptionName);
            ValueScope scope = DialogueGraphUtility.GetOptionValueOrDefault<ValueScope>(this, ValueScopeOptionName);
            ValueTypes valueType = DialogueGraphUtility.GetOptionValueOrDefault<ValueTypes>(this, ValueTypeOptionName);

            // Resolve actual type from enum
            if (!_typeDict.TryGetValue(valueType, out Type selectedType))
                return null;

            // Get the input port value for that type dynamically
            MethodInfo getPortValue = typeof(DialogueGraphUtility)
                .GetMethod(nameof(DialogueGraphUtility.GetOptionValueOrDefault))
                .MakeGenericMethod(selectedType);

            object value = getPortValue.Invoke(null, new object[] { this, ValueOptionName });

            // Build a ValueSetter<T> for the correct type
            Type setterType = typeof(ValueSetter<>).MakeGenericType(selectedType);
            object setterInstance = Activator.CreateInstance(setterType);

            // Assign fields
            setterType.GetField("valueSO")?.SetValue(setterInstance, valueSO);
            setterType.GetField("scope")?.SetValue(setterInstance, scope);
            setterType.GetField("value")?.SetValue(setterInstance, value);

            return (ValueEditor)setterInstance;
        }

        public void DisplayErrors(GraphLogger infos)
        {
            var valueSO = DialogueGraphUtility.GetOptionValueOrDefault<ValueSO>(this, ValueSOOptionName);
            if (valueSO==null)
                infos.LogWarning("Value should not be null", this);
        }
    }
}