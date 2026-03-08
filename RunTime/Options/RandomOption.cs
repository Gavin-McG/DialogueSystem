using UnityEngine;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem
{
    [TypeOption("Random Chance")]
    public class RandomOption : OptionType
    {
        [SerializeField] private float chance;
        
        public override bool EvaluateCondition(AdvanceContext advanceContext, IVariableContext variables)
        {
            return Random.Range(0f, 1f) < chance;
        }
    }
}
