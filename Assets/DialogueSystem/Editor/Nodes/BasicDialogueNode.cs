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
        private const string TextOptionName = "text";
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption<string>(TextOptionName, "Text",
                tooltip: "Display text for this dialogue",
                defaultValue: "Text");
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context);
            DialogueGraphUtility.DefineProfileInputPort(context);
            
            DialogueGraphUtility.DefineNodeOutputPort(context);
            DialogueGraphUtility.DefineEventOutputPort(context);
        }

        public DialogueObject CreateDialogueObject()
        {
            var dialogue = ScriptableObject.CreateInstance<BasicDialogue>();
            dialogue.name = "Basic Dialogue";

            dialogue.text = DialogueGraphUtility.GetOptionValueOrDefault<string>(this, TextOptionName);
            
            return dialogue;
        }
        
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogue = DialogueGraphUtility.GetObject<BasicDialogue>(this, dialogueDict);
            var dialogueTrace = DialogueGraphUtility.GetConnectedDialogue(this, dialogueDict);
            
            dialogue.nextDialogue = dialogueTrace;
            dialogue.profile = DialogueGraphUtility.GetProfileValueOrNull(this, dialogueDict);
            dialogue.events = DialogueGraphUtility.GetEvents(this);
        }
    }

}
