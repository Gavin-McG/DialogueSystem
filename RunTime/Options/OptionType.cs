using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class for Conditional and Choice options. Provides the method used for each to evaluate their conditions
    /// </summary>
    [Serializable, TabName("Option")]
    public abstract class OptionType : ICustomNodeStyle
    {
        public Color BackgroundColor => new Color(0.2f,0.3f,0.2f);
        public Color BorderColor => new Color(0.3f,0.4f,0.3f);
        
        /// <summary>
        /// Returns true if the condition is passed and the option should be considered by its respective Redirect
        /// </summary>
        public abstract bool EvaluateCondition(AdvanceContext advanceContext, DialogueManager manager);
    }
}