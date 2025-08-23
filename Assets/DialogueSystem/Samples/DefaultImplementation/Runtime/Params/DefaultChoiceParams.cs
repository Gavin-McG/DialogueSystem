using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Default.Runtime
{
    public class DefaultChoiceParams : ChoiceParams
    {
        public bool hasTimeLimit;
        public float timeLimitDuration;

        public override ChoiceParams Clone() => new DefaultChoiceParams()
        {
            hasTimeLimit = hasTimeLimit,
            timeLimitDuration = timeLimitDuration
        };
    }
}