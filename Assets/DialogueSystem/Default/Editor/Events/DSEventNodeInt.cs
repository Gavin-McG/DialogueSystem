using System;
using DialogueSystem.Editor;
using DialogueSystem.Default.Runtime;
using Unity.GraphToolkit.Editor;


namespace DialogueSystem.Default.Editor
{
    [Serializable, UseWithGraph(typeof(DialogueGraph))]
    public class DSEventNodeInt : DSEventNode<int, DSEventInt, DSEventIntReference> {}
}