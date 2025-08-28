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
    public abstract class RedirectNode : ContextNode, IDialogueTraceNode, IInputDataNode<DialogueTrace>
    {
        private const string DefaultPortDisplayName = "Default";

        private IPort _nextPort;
        private Redirect _redirect;

        public abstract bool UsesWeight { get; }
        
        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddPreviousPort(context);
            _nextPort = DialogueGraphUtility.AddNextPort(context, DefaultPortDisplayName);
        }
        
        public abstract Redirect CreateRedirectObject();

        public ScriptableObject CreateDialogueObject()
        {
            _redirect = CreateRedirectObject();
            return _redirect;
        }
        
        public void AssignObjectReferences()
        {
            // Set Default nextDialogue for redirect
            var defaultObject = DialogueGraphUtility.GetTrace(_nextPort);
            _redirect.defaultDialogue = defaultObject;
            
            // Assign Events and ValueEditors
            DialogueGraphUtility.AssignDialogueData(_redirect.data, _nextPort);

            // Assign Options
            var optionNodes = blockNodes.ToList().OfType<IInputDataNode<Option>>();
            _redirect.options = new List<Option>();
            foreach (var optionNode in optionNodes)
            {
                var choiceObject = optionNode.GetInputData();
                _redirect.options.Add(choiceObject);
            }
        }

        public DialogueTrace GetInputData() => _redirect;
        
        public void DisplayErrors(GraphLogger infos)
        {
            DialogueGraphUtility.MultipleOutputsCheck(this, infos);
            DialogueGraphUtility.CheckPreviousConnection((INode)this, infos);
            
            DialogueGraphUtility.HasOptionsCheck(this, infos);
        }
    }
    
}