using System;
using DialogueSystem.Editor;
using DialogueSystem.Default.Runtime;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Default.Editor
{
    [Serializable, UseWithContext(typeof(ChoiceDialogueNode))]
    public class KeywordOptionNode : ChoiceOptionNode<KeywordOption> {}
}