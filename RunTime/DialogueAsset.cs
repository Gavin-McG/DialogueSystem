using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// ScriptableObject representing the main asset of a DialogueGraph and the first Trace when starting a dialogue
    /// </summary>
    public sealed class DialogueAsset : ScriptableObject
    {
        [SerializeField] public List<DialogueStart> startPoints;
        [SerializeField] public SettingsData settingsData;

        public DialogueObject GetStartDialogue(string startName)
        {
            return startPoints
                .FirstOrDefault(ds => ds.startName==startName)
                ?.startDialogue;
        }
    }

}
