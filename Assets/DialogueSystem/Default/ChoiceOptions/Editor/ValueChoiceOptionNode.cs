using System;
using DialogueSystem.Editor;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Default.ChoiceOptions.Editor
{
    [Serializable, UseWithContext(typeof(ChoiceDialogueNode))]
    public class ValueChoiceOptionNode : ChoiceOptionNode<ValueChoiceOption> {}
}