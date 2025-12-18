using System.Collections.Generic;
using UnityEngine;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem
{
    [TypeOption("Variable Random")]
    public class RandomVariableOption : OptionType
    {
        [Tooltip("Float Variable to use for chance value.")]
        [SerializeField] private string variableName;
        
        public override bool EvaluateCondition(AdvanceContext advanceContext, IVariableContext variables)
        {
            if (variables.TryGetVariable(variableName, out var chanceVariable))
                return Random.Range(0f, 1f) < chanceVariable.GetFloat();
            return false;
        }

        public override IEnumerable<string> CheckVariables => new List<string> { variableName };
    }
}
