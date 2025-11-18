using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class for Conditional and Choice options. Provides the method used for each to evaluate their conditions
    /// </summary>
    public abstract class OptionType
    {
        /// <summary>
        /// Returns true if the condition is passed and the option should be considered by its respective Redirect
        /// </summary>
        public abstract bool EvaluateCondition(AdvanceParams advanceParams, DialogueManager manager);
    }
}