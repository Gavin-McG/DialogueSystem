using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class representing the Settings for an overall interaction.
    /// Provided via the <see cref="DialogueManager"/>
    /// </summary>
    [Serializable, TabName("Dialogue")]
    public abstract class DialogueParameters : ParameterBase, ICustomNodeStyle
    {
        public Color BackgroundColor => new Color(0.4f,0.4f,0.4f);
        public Color BorderColor => new Color(0.5f,0.5f,0.5f);
    }
}