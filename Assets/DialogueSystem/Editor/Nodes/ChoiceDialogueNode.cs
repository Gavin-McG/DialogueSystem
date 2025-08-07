using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class ChoiceDialogueNode : ContextNode, IDialogueReferenceNode
    {
        private const string TimeOutPortDisplayName = "TimeOut";
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<DialogueBaseParams>(context);
            DialogueGraphUtility.DefineFieldOptions<DialogueChoiceParams>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context);
            DialogueGraphUtility.DefineFieldPorts<DialogueBaseParams>(context);
            DialogueGraphUtility.DefineFieldPorts<DialogueChoiceParams>(context);

            DialogueGraphUtility.DefineNodeOutputPort(context, TimeOutPortDisplayName);
            DialogueGraphUtility.DefineEventOutputPort(context);
        }

        public DialogueObject CreateDialogueObject()
        {
            var dialogue = ScriptableObject.CreateInstance<ChoiceDialogue>();
            dialogue.name = "Choice Dialogue";
            
            dialogue.baseParams ??= new DialogueBaseParams();
            dialogue.choiceParams ??= new DialogueChoiceParams();
            
            DialogueGraphUtility.AssignFromFieldOptions(this, ref dialogue.baseParams);
            DialogueGraphUtility.AssignFromFieldOptions(this, ref dialogue.choiceParams);
            
            return dialogue;
        }
        
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogue = DialogueGraphUtility.GetObject<ChoiceDialogue>(this, dialogueDict);
            var timeOutObject = DialogueGraphUtility.GetConnectedDialogue(this, dialogueDict);
            
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref dialogue.baseParams);
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref dialogue.choiceParams);
            
            dialogue.defaultDialogue = timeOutObject;
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
