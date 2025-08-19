using System;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public abstract class ChoiceParams
    {
        public abstract ChoiceParams Clone();
    }
}