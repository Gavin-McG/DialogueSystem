using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;


namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Base Class for Redirect Context Nodes. Redirect nodes are responsible for containing <see cref="ConditionalNode{T}"/> 
    /// </summary>
    internal abstract class RedirectNode : OptionContext
    {
        protected override string NextPortName => "Default";

        private Redirect _redirect;

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DefineNextAndPrevious(context);
        }

        public abstract Redirect CreateRedirectObject();

        public override ScriptableObject CreateDialogueObject()
        {
            _redirect = CreateRedirectObject();
            return _redirect;
        }
        
        public override void AssignObjectReferences()
        {
            // Set Default nextDialogue for redirect
            var defaultObject = DialogueGraphUtility.GetTrace(_nextPort);
            _redirect.defaultDialogue = defaultObject;
            
            // Assign Events and ValueEditors
            DialogueGraphUtility.AssignDialogueData(_redirect.data, _nextPort);

            // Assign Options
            _redirect.options = Options;
        }

        public override DialogueTrace GetInputData() => _redirect;
    }
    
}