using System;
using UnityEngine;
using WolverineSoft.DialogueSystem;
using Random = UnityEngine.Random;

namespace WolverineSoft.DialogueSystem.Default
{
    public class RandomOption : Option
    {
        public float chance = 0.5f;

        public override bool EvaluateCondition(AdvanceContext context, DialogueManager manager)
        {
            return Random.Range(0f, 1f) < chance;
        }
    }
}


