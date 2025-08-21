using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
 
    public abstract class ChoiceOptionNode<TOptionParams, TChoiceOption> : BlockNode, IDialogueTraceNode
        where TOptionParams : OptionParams
        where TChoiceOption : ChoiceOption
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<TChoiceOption>(context);
            DialogueGraphUtility.DefineFieldOptions<TOptionParams>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeOutputPort(context);
            
            DialogueGraphUtility.DefineFieldPorts<TChoiceOption>(context);
            DialogueGraphUtility.DefineFieldPorts<TOptionParams>(context);
        }

        public ScriptableObject CreateDialogueObject()
        {
            var option = ScriptableObject.CreateInstance<TChoiceOption>();
            option.name = "Choice Option";

            DialogueGraphUtility.AssignFromFieldOptions<TChoiceOption>(this, ref option);
            option.optionParams = DialogueGraphUtility.AssignFromFieldOptions<TOptionParams>(this);
            
            return option;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var option = DialogueGraphUtility.GetObject<TChoiceOption>(this, dialogueDict);
            var optionObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            option.nextDialogue = optionObject;
            
            DialogueGraphUtility.AssignDialogueData(this, option.data, dialogueDict);

            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref option);
            var optionParams = (TOptionParams)option.optionParams;
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref optionParams);
        }
    }

}
