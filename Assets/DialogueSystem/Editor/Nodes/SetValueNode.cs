using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class SetValueNode : Node, IDataNode<Values.ValueEntry>
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<Values.ValueEntry>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context, "");
        }

        public Values.ValueEntry GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var newEntry = DialogueGraphUtility.AssignFromFieldOptions<Values.ValueEntry>(this);
            return newEntry;
        }
    }
}