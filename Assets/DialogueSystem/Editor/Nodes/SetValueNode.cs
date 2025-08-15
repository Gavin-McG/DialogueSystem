using System;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class SetValueNode : Node
    {
        protected override void OnDefineOptions(INodeOptionDefinition context)
        {
            DialogueGraphUtility.DefineFieldOptions<Values.ValueEntry>(context);
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineNodeInputPort(context, "");
        }

        public Values.ValueEntry GetEntry()
        {
            var newEntry = DialogueGraphUtility.AssignFromFieldOptions<Values.ValueEntry>(this);
            return newEntry;
        }
    }
}