using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;


namespace DialogueSystem.Editor
{
    
    public abstract class RedirectNode : ContextNode, IDialogueReferenceNode
    {
        private const string DefaultPortDisplayName = "Default";

        public abstract bool UsesWeight { get; }
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context);

            DialogueGraphUtility.DefineNodeOutputPort(context, DefaultPortDisplayName);
            DialogueGraphUtility.DefineEventOutputPort(context);
        }
        
        public abstract DialogueObject CreateDialogueObject();

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var dialogue = DialogueGraphUtility.GetObject<Redirect>(this, dialogueDict);
            var defaultObject = DialogueGraphUtility.GetConnectedDialogue(this, dialogueDict);
            dialogue.defaultDialogue = defaultObject;
            dialogue.events = DialogueGraphUtility.GetEvents(this);

            var optionNodes = blockNodes.ToList();
            dialogue.options = new List<ConditionalOption>();
            foreach (var optionNode in optionNodes)
            {
                var choiceObject = DialogueGraphUtility.GetObjectFromNode<ConditionalOption>(
                    optionNode , dialogueDict);
                dialogue.options.Add(choiceObject);
            }
        }
    }
    
}