using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using DialogueSystem.Runtime.Values;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public abstract class CustomValueSetterNode<T> : Node, IDataNode<ValueEditor>
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<ValueSetter<T>>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context, "");
        }

        public ValueEditor GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var valueEntry = DialogueGraphUtility.AssignFromFieldOptions<ValueSetter<T>>(this);
            return valueEntry;
        }
    }
}