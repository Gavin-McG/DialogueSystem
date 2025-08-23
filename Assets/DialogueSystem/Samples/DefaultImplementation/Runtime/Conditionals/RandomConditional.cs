using System;
using UnityEngine;
using WolverineSoft.DialogueSystem.Runtime;
using Random = UnityEngine.Random;

namespace WolverineSoft.DialogueSystem.Default.Runtime
{
    public class RandomConditional : ConditionalOption
    {
        public float chance = 0.5f;

        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return Random.Range(0f, 1f) < chance;
        }
    }
}


