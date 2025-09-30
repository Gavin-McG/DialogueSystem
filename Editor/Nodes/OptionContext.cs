using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Base class for all Dialogue ContextNodes which utilize Options.
    /// Used for UseWithContext on options.
    /// </summary>
    public abstract class OptionContext : ContextNode, IDialogueTraceNode, IInputDataNode<DialogueTrace>
    {
        protected IPort _nextPort;
        
        /// <summary>
        /// Field to define the name of the default output port
        /// </summary>
        protected abstract string NextPortName { get; }
        
        /// <summary>
        /// Whether the weight property of options should be shown for this context
        /// </summary>
        public abstract bool UsesWeight { get; }
        
        /// <summary>
        /// Get the Options assigned to this context node
        /// </summary>
        protected List<Option> Options => blockNodes
            .OfType<IInputDataNode<Option>>()
            .Select(n => n.GetInputData())
            .ToList();
        
        // define previous and next ports
        protected void DefineNextAndPrevious(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddPreviousPort(context);
            _nextPort = DialogueGraphUtility.AddNextPort(context, NextPortName);
        }
        
        // interface functions
        public abstract ScriptableObject CreateDialogueObject();
        public abstract void AssignObjectReferences();
        public abstract DialogueTrace GetInputData();
        
        public void DisplayErrors(GraphLogger infos)
        {
            DialogueGraphUtility.MultipleOutputsCheck(this, infos);
            DialogueGraphUtility.CheckPreviousConnection((INode)this, infos);
            
            DialogueGraphUtility.HasOptionsCheck(this, infos);
        }
    }
}