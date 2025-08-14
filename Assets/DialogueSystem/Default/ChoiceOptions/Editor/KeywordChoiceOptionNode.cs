using System;
using DialogueSystem.Editor;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Default.ChoiceOptions.Editor
{
    [Serializable, UseWithContext(typeof(ChoiceDialogueNode))]
    public class KeywordChoiceOptionNode : ChoiceOptionNode<KeywordChoiceOption> {}
}