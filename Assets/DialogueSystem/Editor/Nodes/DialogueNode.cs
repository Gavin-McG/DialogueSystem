using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;
using WolverineSoft.DialogueSystem;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// Generic Base Class for Non-choice dialogue.
    /// </summary>
    /// <typeparam name="TBaseParams">Type of <see cref="BaseParams"/> to be used by the node</typeparam>
    [Serializable]
    public abstract class DialogueNode<TBaseParams> : Node, IDialogueTraceNode, IInputDataNode<DialogueTrace>
    where TBaseParams : BaseParams
    {
        private INodeOption _textOption;
        private IPort _nextPort;
        private readonly List<IPort> _valuePorts = new();
        private Dialogue _dialogue;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _textOption = DialogueGraphUtility.AddNodeOption(context, "Text", typeof(string));
            DialogueGraphUtility.DefineFieldOptions<TBaseParams>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            // Next/Previous Port
            DialogueGraphUtility.AddPreviousPort(context);
            _nextPort = DialogueGraphUtility.AddNextPort(context);
            
            // Param specific ports
            DialogueGraphUtility.DefineFieldPorts<TBaseParams>(context);
            
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
            _dialogue = ScriptableObject.CreateInstance<Dialogue>();
            _dialogue.name = "Basic Dialogue";
            
            // Assign dialogue fields from options
            _dialogue.baseParams = DialogueGraphUtility.AssignFromFieldOptions<TBaseParams>(this);
            _textOption.TryGetValue(out _dialogue.baseParams.text);
            
            return _dialogue;
        }
        
        public void AssignObjectReferences()
        {
            // Assign next dialogue
            var dialogueTrace = DialogueGraphUtility.GetTrace(_nextPort);
            _dialogue.nextDialogue = dialogueTrace;

            // Assign valueSOs
            foreach (var valuePort in _valuePorts)
            {
                _dialogue.baseParams.values.Add(DialogueGraphUtility.GetPortValueOrDefault<ValueSO>(this, valuePort.name));
            }
            
            // Assign events & valueEditors
            DialogueGraphUtility.AssignDialogueData(_dialogue.data, _nextPort);
            
            // assign BaseParams from ports
            var baseParams = (TBaseParams)_dialogue.baseParams;
            DialogueGraphUtility.AssignFromFieldPorts(this, ref baseParams);
        }
        
        public DialogueTrace GetInputData() => _dialogue;
    }

}
