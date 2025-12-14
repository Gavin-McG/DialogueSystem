using System;
using Unity.GraphToolkit.Editor;

namespace WolverineSoft.DialogueSystem.Editor
{
    [Serializable]
    [UseWithGraph(typeof(DialogueGraph))]
    public class StartNode : Node, IDataNode<DialogueStart>
    {
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

        public DialogueStart GetData()
        {
            var start = new DialogueStart();
            start.startDialogue = DialogueGraphUtility.GetTrace(_nextPort);
            _startNameOption.TryGetValue(out start.startName);
            
            //Get dialogue Parameters
            _paramOption.TryGetValue(out ValueHolder<DialogueParameters> parameters);
            start.dialogueParameters = parameters.value1;
            
            return start;
        }
    }
}
