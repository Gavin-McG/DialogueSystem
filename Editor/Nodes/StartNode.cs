using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    [Serializable]
    [UseWithGraph(typeof(DialogueGraph))]
    public class StartNode : Node, IDialogueNode
    {
        private StartObject _asset;
        private IPort _nextPort;
        private INodeOption _startNameOption;
        private INodeOption _paramOption;

        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            _startNameOption = context.AddOption<string>("startName")
                .WithDisplayName("Id")
                .WithTooltip("Unique name of this start point (Leave blank for default)")
                .Build();
            
            _paramOption = context.AddOption<ValueHolder<DialogueParameters>>("dialogueParam").Build();
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            //Get Next Dialogue
            _nextPort = DialogueGraphUtility.AddNextPort(context);
        }
        
        //-------------------------------------------
        //          DialogueNode Methods
        //-------------------------------------------

        public ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<StartObject>();
            return _asset;
        }
        
        public void AssignObjectReferences()
        {
            //Get Next Dialogue
            _asset.nextDialogue = DialogueGraphUtility.GetTrace(_nextPort);
            
            //Assign name
            _startNameOption.TryGetValue(out _asset.startName);
            
            //Get Parameters
            _paramOption.TryGetValue(out ValueHolder<DialogueParameters> parameters);
            _asset.dialogueParameters = parameters.value1;
        }

        public DialogueObject GetData()
        {
            return _asset;
        }
    }
}
