using System;
using Unity.GraphToolkit.Editor;
using WolverineSoft.DialogueSystem.Default.Runtime;
using WolverineSoft.DialogueSystem.Editor;

namespace WolverineSoft.DialogueSystem.Default.Editor
{
    [Serializable, UseWithContext(typeof(DefaultChoiceNode))]
    public class KeywordOptionNode : ChoiceOptionNode<DefaultOptionParams, KeywordOption> {}
}