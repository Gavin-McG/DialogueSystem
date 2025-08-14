using System;
using DialogueSystem.Editor;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Default.Conditionals.Editor
{
    [Serializable, UseWithContext(typeof(RedirectNode))]
    public class KeywordConditionalNode : ConditionalNode<KeywordConditionalOption> {}
}