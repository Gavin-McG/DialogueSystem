using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Generic Base Class for Choice Dialogue Nodes. 
    /// </summary>
    /// <typeparam name="TBaseParams">Type of <see cref="BaseParams"/> to be used by the node</typeparam>
    /// <typeparam name="TChoiceParams">Type of <see cref="ChoiceParams"/> to be used by the node</typeparam>
    public abstract class ChoiceNode<TBaseParams, TChoiceParams, TOptionParams> : ContextNode, IDialogueTraceNode
    where TBaseParams : BaseParams
    where TChoiceParams : ChoiceParams
    where TOptionParams : OptionParams
    {
        private const string TimeOutPortDisplayName = "TimeOut";
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldOptions<TBaseParams>(context);
            DialogueGraphUtility.DefineFieldOptions<TChoiceParams>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context);
            DialogueGraphUtility.DefineFieldPorts<TBaseParams>(context);
            DialogueGraphUtility.DefineFieldPorts<TChoiceParams>(context);

            DialogueGraphUtility.DefineNodeOutputPort(context, TimeOutPortDisplayName);
        }

        public ScriptableObject CreateDialogueObject()
        {
            var dialogue = ScriptableObject.CreateInstance<ChoiceDialogue>();
            dialogue.name = "Choice Dialogue";
            
            dialogue.baseParams = DialogueGraphUtility.AssignFromFieldOptions<TBaseParams>(this);
            dialogue.choiceParams = DialogueGraphUtility.AssignFromFieldOptions<TChoiceParams>(this);
            
            return dialogue;
        }
        
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var dialogue = DialogueGraphUtility.GetObject<ChoiceDialogue>(this, dialogueDict);
            var timeOutObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            dialogue.defaultDialogue = timeOutObject;
            
            DialogueGraphUtility.AssignDialogueData(this, dialogue.data, dialogueDict);
            
            var baseParams = (TBaseParams)dialogue.baseParams;
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref baseParams);
            var choiceParams = (TChoiceParams)dialogue.choiceParams;
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref choiceParams);
            
            var optionNodes = blockNodes.ToList();
            dialogue.options = new List<Option>();
            foreach (var optionNode in optionNodes)
            {
                var choiceObject = DialogueGraphUtility.GetObjectFromNode<Option>(
                    optionNode , dialogueDict);
                dialogue.options.Add(choiceObject);
            }
        }
        
        public void DisplayErrors(GraphLogger infos)
        {
            DialogueGraphUtility.MultipleOutputsCheck(this, infos);
            DialogueGraphUtility.CheckPreviousConnection((INode)this, infos);
            
            DialogueGraphUtility.HasOptionsCheck(this, infos);
        }
    }

}
