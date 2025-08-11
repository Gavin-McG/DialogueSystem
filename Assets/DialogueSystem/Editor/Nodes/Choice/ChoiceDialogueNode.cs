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
        private const string TimeOutPortDisplayName = "TimeOut";
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<BaseParams>(context);
            DialogueGraphUtility.DefineFieldOptions<ChoiceParams>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context);
            DialogueGraphUtility.DefineFieldPorts<BaseParams>(context);
            DialogueGraphUtility.DefineFieldPorts<ChoiceParams>(context);

            DialogueGraphUtility.DefineNodeOutputPort(context, TimeOutPortDisplayName);
        }

        public DialogueObject CreateDialogueObject()
        {
            var dialogue = ScriptableObject.CreateInstance<ChoiceDialogue>();
            dialogue.name = "Choice Dialogue";
            
            dialogue.baseParams = DialogueGraphUtility.AssignFromFieldOptions<BaseParams>(this);
            dialogue.choiceParams = DialogueGraphUtility.AssignFromFieldOptions<ChoiceParams>(this);
            
            return dialogue;
        }
        
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogue = DialogueGraphUtility.GetObject<ChoiceDialogue>(this, dialogueDict);
            var timeOutObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref dialogue.baseParams);
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref dialogue.choiceParams);
            
            dialogue.defaultDialogue = timeOutObject;
            dialogue.events = DialogueGraphUtility.GetEvents(this, dialogueDict);

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
