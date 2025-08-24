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
    public abstract class DialogueNode<TBaseParams> : Node, IDialogueTraceNode
    where TBaseParams : BaseParams
    {
        private INodeOption _textOption;
        private List<IPort> valuePorts;
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _textOption = DialogueGraphUtility.AddNodeOption(context, "Text", typeof(string));
            DialogueGraphUtility.DefineFieldOptions<TBaseParams>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            //Next/Previous Port
            DialogueGraphUtility.DefineNodeInputPort(context);
            DialogueGraphUtility.DefineNodeOutputPort(context);
            
            //Param specific ports
            DialogueGraphUtility.DefineFieldPorts<TBaseParams>(context);
            
            //value ports
            _textOption.TryGetValue(out string text);
            valuePorts = new();
            int index = 0;
            foreach (var value in TextParams.ExtractBracketContents(text))
            {
                valuePorts.Add(context.AddInputPort<ValueSO>($"value {index++}")
                    .WithDisplayName(value)
                    .Build());
            }
        }

        public ScriptableObject CreateDialogueObject()
        {
            var dialogue = ScriptableObject.CreateInstance<Dialogue>();
            dialogue.name = "Basic Dialogue";
            
            dialogue.baseParams = DialogueGraphUtility.AssignFromFieldOptions<TBaseParams>(this);
            _textOption.TryGetValue(out dialogue.baseParams.text);
            
            return dialogue;
        }
        
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var dialogue = DialogueGraphUtility.GetObject<Dialogue>(this, dialogueDict);
            var dialogueTrace = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            dialogue.nextDialogue = dialogueTrace;

            foreach (var valuePort in valuePorts)
            {
                dialogue.baseParams.values.Add(DialogueGraphUtility.GetPortValueOrDefault<ValueSO>(this, valuePort.name));
            }
            
            DialogueGraphUtility.AssignDialogueData(this, dialogue.data, dialogueDict);
            
            var baseParams = (TBaseParams)dialogue.baseParams;
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref baseParams);
        }
    }

}
