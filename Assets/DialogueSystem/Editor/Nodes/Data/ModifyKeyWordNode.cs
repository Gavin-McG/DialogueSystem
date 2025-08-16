using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class ModifyKeyWordNode : Node, IDataNode<Keywords.KeywordEntry>
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<Keywords.KeywordEntry>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context, "");
        }

        public Keywords.KeywordEntry GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var newEntry = DialogueGraphUtility.AssignFromFieldOptions<Keywords.KeywordEntry>(this);
            return newEntry;
        }
    }
}