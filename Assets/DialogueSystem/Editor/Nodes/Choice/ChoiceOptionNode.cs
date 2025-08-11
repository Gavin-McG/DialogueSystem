using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    [UseWithContext(typeof(ChoiceDialogueNode))]
    public class ChoiceOptionNode : BlockNode, IDialogueTraceNode
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<OptionParams>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeOutputPort(context);
            
            DialogueGraphUtility.DefineFieldPorts<OptionParams>(context);
        }

        public ScriptableObject CreateDialogueObject()
        {
            var option = ScriptableObject.CreateInstance<ChoiceOption>();
            option.name = "Choice Option";

            option.optionParams = DialogueGraphUtility.AssignFromFieldOptions<OptionParams>(this);
            
            return option;
        }

        public void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var option = DialogueGraphUtility.GetObject<ChoiceOption>(this, dialogueDict);
            var optionObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);

            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref option.optionParams);
            
            option.nextDialogue = optionObject;
            option.events = DialogueGraphUtility.GetEvents(this, dialogueDict);
        }
    }

}
