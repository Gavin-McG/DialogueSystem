using System;
using DialogueSystem.Editor;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Default.Conditionals.Editor
{
    [Serializable, UseWithContext(typeof(SequentialRedirectNode), typeof(RandomRedirectNode))]
    public class RandomConditionalNode : ConditionalNode<RandomConditionalOption> {}
}
    

