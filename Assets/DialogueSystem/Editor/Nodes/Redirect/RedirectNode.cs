using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using WolverineSoft.DialogueSystem;


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
        private Redirect _redirect;

        public abstract bool UsesWeight { get; }
        
        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context);
            DialogueGraphUtility.DefineNodeOutputPort(context, DefaultPortDisplayName);
        }
        
        public abstract Redirect CreateRedirectObject();

        public ScriptableObject CreateDialogueObject()
        {
            _redirect = CreateRedirectObject();
            return _redirect;
        }
        
        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            // Set Default nextDialogue for redirect
            var defaultObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            _redirect.defaultDialogue = defaultObject;
            
            // Assign Events and ValueEditors
            DialogueGraphUtility.AssignDialogueData(this, _redirect.data);

            // Assign Options
            var optionNodes = blockNodes.ToList();
            _redirect.options = new List<Option>();
            foreach (var optionNode in optionNodes)
            {
                var choiceObject = DialogueGraphUtility.GetObjectFromNode<Option>(
                    optionNode , dialogueDict);
                _redirect.options.Add(choiceObject);
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