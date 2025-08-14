using System;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class ModifyKeyWordNode : Node
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<DialogueTrace.KeywordEntry>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context, "");
        }

        public DialogueTrace.KeywordEntry GetEntry()
        {
            var newEntry = DialogueGraphUtility.AssignFromFieldOptions<DialogueTrace.KeywordEntry>(this);
            return newEntry;
        }
    }
}