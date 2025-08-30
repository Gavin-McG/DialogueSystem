using System;
using System.Collections.Generic;
using WolverineSoft.DialogueSystem;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Node for performing operations on existing numeric values. 
    /// </summary>
    [Serializable]
    internal class ValueModifierNode : Node, IInputDataNode<ValueEditor>, IErrorNode
    {
        private INodeOption _dsValueOption;
        private INodeOption _operationOption;
        private INodeOption _otherValueOption;

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _dsValueOption = DialogueGraphUtility.AddNodeOption(context, "DSValue", typeof(DSValue));
            _operationOption = DialogueGraphUtility.AddNodeOption(context, "Operation", typeof(DSValue.ValueOperation));
            _otherValueOption = DialogueGraphUtility.AddNodeOption(context, "Other Value", typeof(float));
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddDataInputPort(context);
        }

        public ValueEditor GetInputData()
        {
            var valueEntry = new ValueModifier();

            _dsValueOption.TryGetValue(out valueEntry.dsValue);
            _operationOption.TryGetValue(out valueEntry.operation);
            _otherValueOption.TryGetValue(out valueEntry.otherValue);
            
            if (valueEntry.dsValue==null) return null;

            return valueEntry;
        }
        
        public void DisplayErrors(GraphLogger infos)
        {
            _dsValueOption.TryGetValue(out DSValue dsValue);
            if (dsValue==null)
                infos.LogWarning("Value should not be null", this);
        }
    }
}