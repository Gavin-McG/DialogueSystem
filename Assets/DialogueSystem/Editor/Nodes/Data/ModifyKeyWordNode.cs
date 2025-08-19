using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using DialogueSystem.Runtime.Keywords;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class ModifyKeyWordNode : Node, IDataNode<KeywordEditor>
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<KeywordEditor>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context, "");
        }

        public KeywordEditor GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var newEditor = DialogueGraphUtility.AssignFromFieldOptions<KeywordEditor>(this);
            return newEditor;
        }
    }
}