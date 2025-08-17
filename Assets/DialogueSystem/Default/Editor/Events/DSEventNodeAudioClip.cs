using System;
using DialogueSystem.Editor;
using DialogueSystem.Default.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Default.Editor
{
    [Serializable, UseWithGraph(typeof(DialogueGraph))]
    public class DSEventNodeAudioClip : DSEventNode<AudioClip, DSEventAudioClip> {}
}
