using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using DialogueSystem.Runtime.Values;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class ValueModifierNode : Node, IDataNode<ValueEditor>
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