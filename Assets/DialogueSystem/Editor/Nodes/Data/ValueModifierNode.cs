using System;
using System.Collections.Generic;
using WolverineSoft.DialogueSystem;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Node for performing operations on existing numeric values. 
    /// </summary>
    [Serializable]
    internal class ValueModifierNode : Node, IDataNode<ValueEditor>
    {
        private const string ValueSOOptionName = "valueSO";
        private const string OperationOptionName = "operation";
        private const string OtherValueOptionName = "otherValue";
        
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            DialogueGraphUtility.AddNodeOption(context, ValueSOOptionName, typeof(ValueSO), "ValueSO");
            DialogueGraphUtility.AddNodeOption(context, OperationOptionName, typeof(ValueOperation), "Operation");
            DialogueGraphUtility.AddNodeOption(context, OtherValueOptionName, typeof(float), "Other Value");
            
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineDataInputPort(context);
        }

        public ValueEditor GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var valueEntry = new ValueModifier();
            valueEntry.valueSO = DialogueGraphUtility.GetOptionValueOrDefault<ValueSO>(this, ValueSOOptionName);
            valueEntry.operation = DialogueGraphUtility.GetOptionValueOrDefault<ValueOperation>(this, OperationOptionName);
            valueEntry.otherValue = DialogueGraphUtility.GetOptionValueOrDefault<float>(this, OtherValueOptionName);
            
            return valueEntry;
        }
    }
}