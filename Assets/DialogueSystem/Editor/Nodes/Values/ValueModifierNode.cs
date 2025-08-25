using System;
using System.Collections.Generic;
using WolverineSoft.DialogueSystem;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// Node for performing operations on existing numeric values. 
    /// </summary>
    [Serializable]
    internal class ValueModifierNode : Node, IDataNode<ValueEditor>, IErrorNode
    {
        private INodeOption _valueSOOption;
        private INodeOption _operationOption;
        private INodeOption _otherValueOption;

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _valueSOOption = DialogueGraphUtility.AddNodeOption(context, "valueSO", typeof(ValueSO));
            _operationOption = DialogueGraphUtility.AddNodeOption(context, "Operation", typeof(ValueSO.ValueOperation));
            _otherValueOption = DialogueGraphUtility.AddNodeOption(context, "Other Value", typeof(float));
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineDataInputPort(context);
        }

        public ValueEditor GetData()
        {
            var valueEntry = new ValueModifier();

            _valueSOOption.TryGetValue(out valueEntry.valueSO);
            _operationOption.TryGetValue(out valueEntry.operation);
            _otherValueOption.TryGetValue(out valueEntry.otherValue);

            return valueEntry;
        }
        
        public void DisplayErrors(GraphLogger infos)
        {
            _valueSOOption.TryGetValue(out ValueSO valueSO);
            if (valueSO==null)
                infos.LogWarning("Value should not be null", this);
        }
    }
}