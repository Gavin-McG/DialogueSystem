using System;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class ChoiceParams
    {
        public bool hasTimeLimit;
        public float timeLimitDuration;


        public ChoiceParams()
        {
            hasTimeLimit = false;
            timeLimitDuration = 0.0f;
        }
        
        public ChoiceParams(ChoiceParams copyObj)
        {
            hasTimeLimit = copyObj.hasTimeLimit;
            timeLimitDuration = copyObj.timeLimitDuration;
        }
    }
}