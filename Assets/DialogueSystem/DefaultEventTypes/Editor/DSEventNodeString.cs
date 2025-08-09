using System;
using DialogueSystem.Editor;
using Unity.GraphToolkit.Editor;

[Serializable, UseWithGraph(typeof(DialogueGraph))]
public class DSEventNodeString : DSEventNode<string, DSEventString, DSEventStringReference> {}
    