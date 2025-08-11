using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class DialogueSettings
    {
        [Multiline, Tooltip("(Optional) description of dialogue purpose/contents")]
        public string dialogueDescription;
    }
}