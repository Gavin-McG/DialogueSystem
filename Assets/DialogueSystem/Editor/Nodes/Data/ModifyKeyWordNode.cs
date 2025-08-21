using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using DialogueSystem.Runtime.Keywords;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Node to add/remove/removeall keyword(s). 
    /// </summary>
    [Serializable]
    internal class ModifyKeyWordNode : Node, IDataNode<KeywordEditor>
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<KeywordEditor>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineDataInputPort(context);
        }

        public KeywordEditor GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var newEditor = DialogueGraphUtility.AssignFromFieldOptions<KeywordEditor>(this);
            return newEditor;
        }
    }
}