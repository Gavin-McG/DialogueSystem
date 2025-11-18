using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class representing the Settings for an overall interaction.
    /// Provided via the <see cref="DialogueManager"/>
    /// </summary>
    [Serializable]
    public class SettingsData
    {
        [SerializeField] public string description;
        [SerializeReference] private System.Object data;
    }
}