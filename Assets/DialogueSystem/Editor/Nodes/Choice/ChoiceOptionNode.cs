using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Generic Base Class for Choice Option Nodes. 
    /// </summary>
    /// <typeparam name="TOptionParams">Type of <see cref="OptionParams"/> to be used by the node</typeparam>
    /// <typeparam name="TChoiceOption">Type of <see cref="ChoiceOption"/> to be made by the node</typeparam>
    public abstract class ChoiceOptionNode<TOptionParams, TChoiceOption> : BlockNode, IDialogueTraceNode
        where TOptionParams : OptionParams
        where TChoiceOption : ChoiceOption
    {
        protected sealed override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<TChoiceOption>(context);
            DialogueGraphUtility.DefineFieldOptions<TOptionParams>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
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
