using System;
using DialogueSystem.Editor;
using Unity.GraphToolkit.Editor;
using UnityEngine;

[Serializable, UseWithGraph(typeof(DialogueGraph))]
public class DSEventNodeAudioClip : DSEventNode<AudioClip, DSEventAudioClip, DSEventAudioClipReference> {}
