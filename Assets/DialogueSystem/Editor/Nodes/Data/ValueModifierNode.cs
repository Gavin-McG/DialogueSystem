using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class ValueModifierNode : Node, IDataNode<Values.ValueEntry>
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<Values.ValueModifier>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context, "");
        }

        public Values.ValueEntry GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var valueEntry = DialogueGraphUtility.AssignFromFieldOptions<Values.ValueModifier>(this);
            return valueEntry;
        }
    }
}