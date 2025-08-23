using System;
using Unity.GraphToolkit.Editor;
using WolverineSoft.DialogueSystem.Default.Runtime;
using WolverineSoft.DialogueSystem.Editor;

namespace WolverineSoft.DialogueSystem.Default.Editor
{
    [Serializable, UseWithContext(typeof(DefaultChoiceNode), typeof(SequentialRedirectNode), typeof(RandomRedirectNode))]
    public class KeywordConditionalNode : OptionNode<KeywordConditional> {}
}