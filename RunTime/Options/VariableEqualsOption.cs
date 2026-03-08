using System.Collections.Generic;
using UnityEngine;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem
{
    [TypeOption("Variable Equals")]
    public class VariableEqualsOption : OptionType
    {
        [Tooltip("Name of the variable to be compared")]
        [SerializeField] private string variableName;
        
        [Tooltip("value to compare the variable with")]
        [SerializeField] private Variable value;
        
        public override bool EvaluateCondition(AdvanceContext advanceContext, IVariableContext variables)
        {
            if (variables.TryGetVariable(variableName, out var variable))
                return variable == value;
            return false;
        }
        
        public override IEnumerable<string> CheckVariables => new List<string> { variableName };
    }
}
