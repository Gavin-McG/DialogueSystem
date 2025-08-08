using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    [UseWithContext(typeof(ChoiceDialogueNode))]
    public class ChoiceOptionNode : BlockNode, IDialogueTraceNode
    {
        private const string PromptOptionName = "prompt";
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption<string>(PromptOptionName, "Prompt",
                tooltip: "Text corresponding to the response option", 
                defaultValue: "Response");
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeOutputPort(context);
        }

        public DialogueObject CreateDialogueObject()
        {
            var option = ScriptableObject.CreateInstance<ChoiceOption>();
            option.name = "Choice Option";
            
            option.prompt = DialogueGraphUtility.GetOptionValueOrDefault<string>(this, PromptOptionName);
            
            return option;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var option = DialogueGraphUtility.GetObject<ChoiceOption>(this, dialogueDict);
            var optionObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            
            option.nextDialogue = optionObject;
            option.events = DialogueGraphUtility.GetEvents(this, dialogueDict);
        }
    }

}
