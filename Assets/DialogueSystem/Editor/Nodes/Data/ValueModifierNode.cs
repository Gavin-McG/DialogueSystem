using System;
using System.Collections.Generic;
using WolverineSoft.DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Runtime.Values;

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
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<ValueModifier>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineDataInputPort(context);
        }

        public ValueEditor GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var valueEntry = DialogueGraphUtility.AssignFromFieldOptions<ValueModifier>(this);
            return valueEntry;
        }
    }
}