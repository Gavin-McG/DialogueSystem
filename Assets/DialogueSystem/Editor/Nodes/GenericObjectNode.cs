using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    
    public abstract class GenericObjectNode<T> : Node, IDialogueReferenceNode where T : DialogueObject
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldPorts<T>(context);
        }

        public virtual DialogueObject CreateDialogueObject()
        {
            var obj = ScriptableObject.CreateInstance<T>();

            DialogueGraphUtility.AssignFromFieldOptions(this, ref obj);

            return obj;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var obj = DialogueGraphUtility.GetObjectFromNode<T>(this, dialogueDict);
            
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref obj);
        }
    }

}