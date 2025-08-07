using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class ChoiceDialogueNode : ContextNode, IDialogueTraceNode
    {
        private const string TextOptionName = "text";
        private const string HasTimeLimitOptionName = "hasTimeLimit";
        private const string TimeLimitDurationOptionName = "timeLimitDuration";
        private const string TimeOutPortDisplayName = "TimeOut";
        
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            context.AddNodeOption<string>(TextOptionName, "Text",
                tooltip: "Display text for this dialogue",
                defaultValue: "Text");
            
            context.AddNodeOption<bool>(HasTimeLimitOptionName, "Has Time Limit",
                tooltip: "Determines whether a Time limit is present for the option.",
                defaultValue: false);
            
            context.AddNodeOption<float>(TimeLimitDurationOptionName, "Time Limit Duration",
                tooltip: "Amount of Time the user has to make a choice",
                defaultValue: 10f);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context);
            DialogueGraphUtility.DefineProfileInputPort(context);

            DialogueGraphUtility.DefineNodeOutputPort(context, TimeOutPortDisplayName);
            DialogueGraphUtility.DefineEventOutputPort(context);
        }

        public DialogueObject CreateDialogueObject()
        {
            var dialogue = ScriptableObject.CreateInstance<ChoiceDialogue>();
            dialogue.name = "Choice Dialogue";
            
            dialogue.text = 
                DialogueGraphUtility.GetOptionValueOrDefault<string>(this, TextOptionName);
            dialogue.hasTimeLimit = 
                DialogueGraphUtility.GetOptionValueOrDefault<bool>(this, HasTimeLimitOptionName);
            dialogue.timeLimitDuration = 
                DialogueGraphUtility.GetOptionValueOrDefault<float>(this, TimeLimitDurationOptionName);
            
            return dialogue;
        }
        
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogue = DialogueGraphUtility.GetObject<ChoiceDialogue>(this, dialogueDict);
            var timeOutObject = DialogueGraphUtility.GetConnectedDialogue(this, dialogueDict);
            dialogue.defaultDialogue = timeOutObject;
            dialogue.profile = DialogueGraphUtility.GetProfileValueOrNull(this, dialogueDict);
            dialogue.events = DialogueGraphUtility.GetEvents(this);

            var optionNodes = blockNodes.ToList();
            dialogue.options = new List<ChoiceOption>();
            foreach (var optionNode in optionNodes)
            {
                var choiceObject = DialogueGraphUtility.GetObjectFromNode<ChoiceOption>(
                    optionNode , dialogueDict);
                dialogue.options.Add(choiceObject);
            }
        }
    }

}
