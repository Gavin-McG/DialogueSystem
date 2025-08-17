using System;
using DialogueSystem.Default.Runtime;
using DialogueSystem.Editor;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Default.Editor
{
    [Serializable, UseWithContext(typeof(SequentialRedirectNode), typeof(RandomRedirectNode))]
    public class ValueCompConditionalNode : ConditionalNode<ValueCompConditional> {}
}