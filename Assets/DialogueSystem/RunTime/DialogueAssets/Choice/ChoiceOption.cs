using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Base class for Scriptable Object representing a Choice Dialogue
    /// </summary>
    public abstract class ChoiceOption : Option
    {
        [HideInDialogueGraph, SerializeReference] public OptionParams optionParams;
    }
}
