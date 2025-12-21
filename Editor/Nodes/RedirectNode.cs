using System;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    public abstract class RedirectNode : OptionContextNode
    {
        protected RedirectObject _asset;
        private IPort _nextPort;

        public override bool UseText => false;
        public override bool UseResponseParameters => false;

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.AddPreviousPort(context);
            _nextPort = DialogueGraphUtility.AddNextPort(context, "Default");
        }
        
        public override void AssignObjectReferences()
        {
            //Get Next Dialogue
            _asset.defaultDialogue = DialogueGraphUtility.GetTrace(_nextPort);
            
            //Get Options
            _asset.options = blockNodes
                .OfType<OptionNode>()
                .Select(n => n.GetData())
                .OfType<OptionObject>()
                .ToList();
        }

        public override DialogueObject GetData()
        {
            return _asset;
        }
    }
}
