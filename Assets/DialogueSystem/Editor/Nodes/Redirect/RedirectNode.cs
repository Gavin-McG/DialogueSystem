using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;


namespace DialogueSystem.Editor
{
    
    public abstract class RedirectNode : ContextNode, IDialogueTraceNode
    {
        private const string DefaultPortDisplayName = "Default";

        public abstract bool UsesWeight { get; }
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
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