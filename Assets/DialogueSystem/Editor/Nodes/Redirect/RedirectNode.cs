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
    /// Base Class for Redirect Context Nodes. Redirect nodes are responsible for containing <see cref="ConditionalNode{T}"/> 
    /// </summary>
    public abstract class RedirectNode : ContextNode, IDialogueTraceNode
    {
        private const string DefaultPortDisplayName = "Default";

        public abstract bool UsesWeight { get; }
        
        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context);

            DialogueGraphUtility.DefineNodeOutputPort(context, DefaultPortDisplayName);
        }
        
        public abstract ScriptableObject CreateDialogueObject();

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var dialogue = DialogueGraphUtility.GetObject<Redirect>(this, dialogueDict);
            var defaultObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            dialogue.defaultDialogue = defaultObject;
            
            DialogueGraphUtility.AssignDialogueData(this, dialogue.data, dialogueDict);

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