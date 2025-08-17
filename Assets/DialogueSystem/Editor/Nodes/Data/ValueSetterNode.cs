using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public abstract class ValueSetterNode<T> : Node, IDataNode<Values.ValueEntry>
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<Values.ValueEntry<T>>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context, "");
        }

        public Values.ValueEntry GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var valueEntry = DialogueGraphUtility.AssignFromFieldOptions<Values.ValueEntry<T>>(this);
            return valueEntry;
        }
    }
}