using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class representing the parameters used by Choice Dialogue
    /// </summary>
    [Serializable, TabName("Choice")]
    public abstract class ChoiceParameters : ParameterBase, ICustomNodeStyle
    {
        public Color BackgroundColor => new Color(0.3f,0.2f,0.3f);
        public Color BorderColor => new Color(0.4f,0.3f,0.4f);
    }
}