using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class representing the Settings for an overall interaction.
    /// Provided via the <see cref="DialogueManager"/>
    /// </summary>
    public class DialogueSettings
    {
        [Multiline, Tooltip("(Optional) description of dialogue purpose/contents")]
        public string DialogueDescription;
    }
}