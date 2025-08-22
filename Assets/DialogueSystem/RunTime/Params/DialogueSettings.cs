using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Setting displayed on the BeginDialogueNode. Provided via the <see cref="DialogueManager"/> beginDialogue Event
    /// </summary>
    [Serializable]
    public class DialogueSettings
    {
        [Multiline, Tooltip("(Optional) description of dialogue purpose/contents")]
        public string dialogueDescription;
    }
}