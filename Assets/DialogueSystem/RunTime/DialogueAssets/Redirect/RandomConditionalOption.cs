using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class RandomConditionalOption : ConditionalOption
    {
        public float chance = 0.5f;

        public override bool EvaluateCondition(AdvanceDialogueContext context)
        {
            return Random.Range(0f, 1f) < chance;
        }
    }

}
