using System;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    public abstract class ChoiceOptionNode : BlockNode, IDialogueTraceNode
    {
        public abstract ScriptableObject CreateDialogueObject();
        public abstract void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict);
    }
 
    public abstract class ChoiceOptionNode<T> : ChoiceOptionNode
        where T : ChoiceOption
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<T>(context);
            DialogueGraphUtility.DefineFieldOptions<OptionParams>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeOutputPort(context);
            
            DialogueGraphUtility.DefineFieldPorts<T>(context);
            DialogueGraphUtility.DefineFieldPorts<OptionParams>(context);
        }

        public override ScriptableObject CreateDialogueObject()
        {
            var option = ScriptableObject.CreateInstance<T>();
            option.name = "Choice Option";

            DialogueGraphUtility.AssignFromFieldOptions<T>(this, ref option);
            option.optionParams = DialogueGraphUtility.AssignFromFieldOptions<OptionParams>(this);
            
            return option;
        }

        public override void AssignObjectReferences(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict)
        {
            var option = DialogueGraphUtility.GetObject<T>(this, dialogueDict);
            var optionObject = DialogueGraphUtility.GetConnectedTrace(this, dialogueDict);
            option.nextDialogue = optionObject;
            
            DialogueGraphUtility.AssignKeywordAndEventReferences(this, option, dialogueDict);

            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref option);
            DialogueGraphUtility.AssignFromFieldPorts(this, dialogueDict, ref option.optionParams);
        }
    }

}
