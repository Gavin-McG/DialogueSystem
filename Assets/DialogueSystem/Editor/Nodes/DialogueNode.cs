using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Generic Base Class for Non-choice dialogue.
    /// </summary>
    /// <typeparam name="TBaseParams">Type of <see cref="BaseParams"/> to be used by the node</typeparam>
    [Serializable]
    public abstract class DialogueNode<TBaseParams> : Node, IDialogueTraceNode
    where TBaseParams : BaseParams
    {
        
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldOptions<TBaseParams>(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context);
            DialogueGraphUtility.DefineNodeOutputPort(context);

            DialogueGraphUtility.DefineFieldPorts<TBaseParams>(context);
        }

        public ScriptableObject CreateDialogueObject()
        {
            var dialogue = ScriptableObject.CreateInstance<Dialogue>();
            dialogue.name = "Basic Dialogue";
            
            dialogue.baseParams = DialogueGraphUtility.AssignFromFieldOptions<TBaseParams>(this);
            
            return dialogue;
        }
        
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var dialogue = DialogueGraphUtility.GetObject<Dialogue>(this, dialogueDict);
            var dialogueTrace = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            dialogue.nextDialogue = dialogueTrace;
            
            DialogueGraphUtility.AssignDialogueData(this, dialogue.data, dialogueDict);
            
            var baseParams = (TBaseParams)dialogue.baseParams;
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref baseParams);
        }
    }

}
