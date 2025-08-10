using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{

    public abstract class ConditionalNode : BlockNode, IDialogueTraceNode
    {
        public abstract DialogueObject CreateDialogueObject();
        public abstract void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict);
    }

    public abstract class ConditionalNode<T> : ConditionalNode
    where T : ConditionalOption
    {
        private const string WeightOptionName = "Weight";
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineFieldPorts<T>(context);
            DialogueGraphUtility.DefineNodeOutputPort(context);
        }

        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            if (contextNode == null) Debug.Log("No ref");
            
            if (contextNode is RedirectNode redirectNode && redirectNode.UsesWeight)
            {
                context.AddNodeOption<float>(WeightOptionName, "Weight",
                    tooltip: "Percent probablility for this option to be chosen on evaluation",
                    defaultValue: 0.5f);
            }
            
            DialogueGraphUtility.DefineFieldOptions<T>(context);
        }

        public override DialogueObject CreateDialogueObject()
        {
            var option = ScriptableObject.CreateInstance<T>();
            option.name = "Conditional Option";
            
            DialogueGraphUtility.AssignFromFieldOptions(this, ref option);
        
            return option;
        }

        public override void AssignObjectReferences(Dictionary<IDialogueObjectNode, DialogueObject> dialogueDict)
        {
            var option = DialogueGraphUtility.GetObject<T>(this, dialogueDict);
            var nextTrace = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref option);
            
            option.nextDialogue = nextTrace;
            option.events = DialogueGraphUtility.GetEvents(this, dialogueDict);
        }

        protected T GetOptionValueOrDefault<T>(string optionName)
        {
            return DialogueGraphUtility.GetOptionValueOrDefault<T>(this, optionName);
        }
    }

}
