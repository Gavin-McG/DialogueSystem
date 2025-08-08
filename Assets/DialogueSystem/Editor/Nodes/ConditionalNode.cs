using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Editor
{

    public abstract class ConditionalNode : BlockNode, IDialogueTraceNode
    {
        private const string WeightOptionName = "Weight";
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeOutputPort(context);
            
            DialogueGraphUtility.DefineEventOutputPort(context);
        }

        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            if (contextNode is RedirectNode redirectNode && redirectNode.UsesWeight)
            {
                context.AddNodeOption<float>(WeightOptionName, "Weight",
                    tooltip: "Percent probablility for this option to be chosen on evaluation",
                    defaultValue: 0.5f);
            }
        }

        public abstract DialogueObject CreateDialogueObject();

        public virtual void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var option = DialogueGraphUtility.GetObject<ConditionalOption>(this, dialogueDict);
            var optionObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            
            option.nextDialogue = optionObject;
            option.events = DialogueGraphUtility.GetEvents(this, dialogueDict);
        }

        protected T GetOptionValueOrDefault<T>(string optionName)
        {
            return DialogueGraphUtility.GetOptionValueOrDefault<T>(this, optionName);
        }
    }

}
