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
            DialogueGraphUtility.DefineFieldOptions<BaseParams>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context);
            DialogueGraphUtility.DefineNodeOutputPort(context);

            DialogueGraphUtility.DefineFieldPorts<BaseParams>(context);
        }

        public ScriptableObject CreateDialogueObject()
        {
            var dialogue = ScriptableObject.CreateInstance<BasicDialogue>();
            dialogue.name = "Basic Dialogue";
            
            dialogue.baseParams = DialogueGraphUtility.AssignFromFieldOptions<BaseParams>(this);
            
            return dialogue;
        }
        
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var dialogue = DialogueGraphUtility.GetObject<BasicDialogue>(this, dialogueDict);
            var dialogueTrace = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            dialogue.nextDialogue = dialogueTrace;
            
            DialogueGraphUtility.AssignKeywordAndEventReferences(this, dialogue, dialogueDict);
            
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref dialogue.baseParams);
        }
    }

}
