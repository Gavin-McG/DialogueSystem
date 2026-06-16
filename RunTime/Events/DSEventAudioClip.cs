using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Built in implementation for AudioClip events
    /// </summary>
    [CreateAssetMenu(fileName = "DialogueEvent", menuName = "Dialogue System/Events/Dialogue Event (AudioClip)")]
    public class DSEventAudioClip : DSEvent<AudioClip> {}
}
