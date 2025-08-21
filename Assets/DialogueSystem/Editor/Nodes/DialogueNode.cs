using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    
    [Serializable]
    public abstract class DialogueNode<TBaseParams> : Node, IDialogueTraceNode
    where TBaseParams : BaseParams
    {
        
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<TBaseParams>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
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
