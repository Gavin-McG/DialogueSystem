using System;
using DialogueSystem.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DialogueSystem.Default.Runtime
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


