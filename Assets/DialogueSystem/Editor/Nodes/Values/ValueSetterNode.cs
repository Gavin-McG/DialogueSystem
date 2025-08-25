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
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// Node for setting a value from a set of pre-defined existing types. 
    /// </summary>
    [Serializable]
    internal class ValueSetterNode : Node, IDataNode<ValueEditor>, IErrorNode
    {
        private INodeOption _valueSOOption;
        private INodeOption _valueScopeOption;
        private INodeOption _valueTypeOption;
        private INodeOption _valueOption;
        
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

        private Type GetValueType()
        {
            if (_valueTypeOption.TryGetValue(out ValueTypes valueType) && 
                _typeDict.TryGetValue(valueType, out Type result))
            {
                return result;
            }

            return null;
        }
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _valueSOOption = DialogueGraphUtility.AddNodeOption(context, "ValueSO", typeof(ValueSO));
            _valueScopeOption = DialogueGraphUtility.AddNodeOption(context, "Scope", typeof(ValueSO.ValueScope));
            _valueTypeOption = DialogueGraphUtility.AddNodeOption(context, "Value Type", typeof(ValueTypes));
            
            //set value option based on selected value type
            Type selectedType = GetValueType();
            if (selectedType != null)
                _valueOption = DialogueGraphUtility.AddNodeOption(context, "Value", selectedType);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineDataInputPort(context);
        }

        public ValueEditor GetData()
        {
            // Get user selections from options
            _valueSOOption.TryGetValue<ValueSO>(out var valueSO);
            _valueScopeOption.TryGetValue<ValueSO.ValueScope>(out var scope);
            _valueOption.TryGetValue(out object value);

            // Build a ValueSetter<T> for the correct type
            Type selectedType = GetValueType();
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
            _valueSOOption.TryGetValue(out ValueSO valueSO);
            if (valueSO==null)
                infos.LogWarning("Value should not be null", this);
        }
    }
}