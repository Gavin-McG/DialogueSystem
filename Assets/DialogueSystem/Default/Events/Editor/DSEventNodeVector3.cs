using System;
using DialogueSystem.Editor;
using Unity.GraphToolkit.Editor;
using UnityEngine;

[Serializable, UseWithGraph(typeof(DialogueGraph))]
public class DSEventNodeVector3 : DSEventNode<Vector3, DSEventVector3, DSEventVector3Reference> {}
