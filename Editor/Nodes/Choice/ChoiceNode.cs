using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Generic Base Class for Choice Dialogue Nodes. 
    /// </summary>
    public abstract class ChoiceNode<TBaseParams, TChoiceParams, TOptionParams> : OptionContext
    where TBaseParams : BaseParams
    where TChoiceParams : ChoiceParams
    where TOptionParams : OptionParams
    {
        protected sealed override string NextPortName => "TimeOut";
        public sealed override bool UsesWeight => false;

        private INodeOption _textOption;
        private readonly List<IPort> _valuePorts = new();
        private ChoiceDialogue _dialogue;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _textOption = DialogueGraphUtility.AddNodeOption(context, "Text", typeof(string));
            
            // Define options for BaseParams and choiceParams
            DialogueGraphUtility.AddTypeOptions<TBaseParams>(context);
            DialogueGraphUtility.AddTypeOptions<TChoiceParams>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DefineNextAndPrevious(context);
            
            // Define ports for BaseParams and choiceParams
            DialogueGraphUtility.AddTypePorts<TBaseParams>(context);
            DialogueGraphUtility.AddTypePorts<TChoiceParams>(context);
            
            // Create input ports for each {bracket} in the text
            _textOption.TryGetValue(out string text);
            int index = 0;
            foreach (var value in TextParams.ExtractBracketContents(text))
            {
                _valuePorts.Add(context.AddInputPort<DSValue>($"value {index++}")
                    .WithDisplayName(value)
                    .Build());
            }
        }

        public sealed override ScriptableObject CreateDialogueObject()
        {
            // Create dialogue asset
            _dialogue = ScriptableObject.CreateInstance<ChoiceDialogue>();
            _dialogue.name = "Choice Dialogue";
            
            return _dialogue;
        }
        
        public sealed override void AssignObjectReferences()
        {
            // Assign default next dialogue
            var timeOutObject = DialogueGraphUtility.GetTrace(_nextPort);
            _dialogue.defaultDialogue = timeOutObject;
            
            // Assign events & valueEditors
            DialogueGraphUtility.AssignDialogueData(_dialogue.data, _nextPort);
            
            // Assign BaseParams
            var baseParams = Activator.CreateInstance<TBaseParams>();
            DialogueGraphUtility.AssignFromNode(this, ref baseParams);
            _dialogue.baseParams = baseParams;
            
            // Assign other BaseParam values
            _textOption.TryGetValue(out _dialogue.baseParams.text);
            foreach (var valuePort in _valuePorts)
            {
                _dialogue.baseParams.values.Add(DialogueGraphUtility.GetPortValueOrDefault<DSValue>(this, valuePort.name));
            }
            
            // Assign ChoiceParams
            var choiceParams = Activator.CreateInstance<TChoiceParams>();
            DialogueGraphUtility.AssignFromNode(this, ref choiceParams);
            _dialogue.choiceParams = choiceParams;
            
            // Assign Options
            _dialogue.options = Options;
        }

        public sealed override DialogueTrace GetInputData() => _dialogue;
    }

}
