using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem;
using WolverineSoft.DialogueSystem.Values;

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

        private INodeOption _textOption;
        private readonly List<IPort> _valuePorts = new();
        private ChoiceDialogue _dialogue;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _textOption = DialogueGraphUtility.AddNodeOption(context, "Text", typeof(string));
            
            // Define options for BaseParams and choiceParams
            DialogueGraphUtility.DefineFieldOptions<TBaseParams>(context);
            DialogueGraphUtility.DefineFieldOptions<TChoiceParams>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            // Next/Previous Port
            DialogueGraphUtility.DefineNodeInputPort(context);
            DialogueGraphUtility.DefineNodeOutputPort(context, TimeOutPortDisplayName);
            
            // Define ports for BaseParams and choiceParams
            DialogueGraphUtility.DefineFieldPorts<TBaseParams>(context);
            DialogueGraphUtility.DefineFieldPorts<TChoiceParams>(context);
            
            // Create input ports for each {bracket} in the text
            _textOption.TryGetValue(out string text);
            int index = 0;
            foreach (var value in TextParams.ExtractBracketContents(text))
            {
                _valuePorts.Add(context.AddInputPort<ValueSO>($"value {index++}")
                    .WithDisplayName(value)
                    .Build());
            }
        }

        public ScriptableObject CreateDialogueObject()
        {
            // Create dialogue asset
            _dialogue = ScriptableObject.CreateInstance<ChoiceDialogue>();
            _dialogue.name = "Choice Dialogue";
            
            // Assign dialogue fields from options
            _dialogue.baseParams = DialogueGraphUtility.AssignFromFieldOptions<TBaseParams>(this);
            _textOption.TryGetValue(out _dialogue.baseParams.text);
            _dialogue.choiceParams = DialogueGraphUtility.AssignFromFieldOptions<TChoiceParams>(this);
            
            return _dialogue;
        }
        
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            // Assign default next dialogue
            var timeOutObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            _dialogue.defaultDialogue = timeOutObject;
            
            // Assign events & valueEditors
            DialogueGraphUtility.AssignDialogueData(this, _dialogue.data);
            
            // Assign valueSOs
            foreach (var valuePort in _valuePorts)
            {
                _dialogue.baseParams.values.Add(DialogueGraphUtility.GetPortValueOrDefault<ValueSO>(this, valuePort.name));
            }
            
            // Assign BaseParams from ports
            var baseParams = (TBaseParams)_dialogue.baseParams;
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref baseParams);
            var choiceParams = (TChoiceParams)_dialogue.choiceParams;
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref choiceParams);
            
            // Assign node Options from 
            var optionNodes = blockNodes.ToList().OfType<IDataNode<Option>>();
            _dialogue.options = new List<Option>();
            foreach (var optionNode in optionNodes)
            {
                var choiceObject = optionNode.GetData();
                _dialogue.options.Add(choiceObject);
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
