using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    
    [Serializable]
    public class BasicDialogueNode : Node, IDialogueTraceNode
    {
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<DialogueBaseParams>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context);
            DialogueGraphUtility.DefineFieldPorts<DialogueBaseParams>(context);
            
            DialogueGraphUtility.DefineNodeOutputPort(context);
        }

        public DialogueObject CreateDialogueObject()
        {
            var dialogue = ScriptableObject.CreateInstance<BasicDialogue>();
            dialogue.name = "Basic Dialogue";
            
            dialogue.baseParams ??= new DialogueBaseParams();

            DialogueGraphUtility.AssignFromFieldOptions(this, ref dialogue.baseParams);
            
            return dialogue;
        }
        
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogue = DialogueGraphUtility.GetObject<BasicDialogue>(this, dialogueDict);
            var dialogueTrace = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref dialogue.baseParams);
            
            dialogue.nextDialogue = dialogueTrace;
            dialogue.events = DialogueGraphUtility.GetEvents(this);
        }
    }

}
