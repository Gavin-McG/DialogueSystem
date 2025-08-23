using System;
using DialogueSystem.Editor;
using DialogueSystem.Default.Runtime;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Default.Editor
{
    [Serializable, UseWithContext(typeof(DefaultChoiceNode))]
    public class BasicOptionNode : ChoiceOptionNode<DefaultOptionParams, BasicOption> {}
}