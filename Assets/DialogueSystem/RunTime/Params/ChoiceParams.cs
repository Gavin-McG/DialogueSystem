using System;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class ChoiceParams
    {
        public bool hasTimeLimit;
        public float timeLimitDuration;
    }
}