using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public abstract class SetValueNode<T> : Node, IDialogueObjectNode, IDataNode<ValueSetter>
        where T : ValueSetter
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context, "");
        }

        public ScriptableObject CreateDialogueObject()
        {
            T valueSetter = ScriptableObject.CreateInstance<T>();
            DialogueGraphUtility.AssignFromFieldOptions(this, ref valueSetter);
            return valueSetter;
        }

        public ValueSetter GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            T setter = DialogueGraphUtility.GetObjectFromNode<T>(this, dialogueDict);
            return setter;
        }
    }
}