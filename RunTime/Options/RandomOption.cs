using UnityEngine;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem
{
    public class RandomOption : OptionType
    {
        [SerializeField] private float chance;
        
        public override bool EvaluateCondition(AdvanceContext advanceContext, DialogueManager manager)
        {
            return Random.Range(0f, 1f) < chance;
        }
    }
}
