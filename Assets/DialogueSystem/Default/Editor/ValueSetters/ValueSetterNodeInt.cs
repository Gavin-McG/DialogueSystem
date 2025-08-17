using System;
using DialogueSystem.Default.Runtime;
using DialogueSystem.Editor;
using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Default.Editor
{
    [Serializable, UseWithGraph(typeof(DialogueGraph))]
    public class ValueSetterNodeInt : ValueSetterNode<ValueSetterInt> {}
}