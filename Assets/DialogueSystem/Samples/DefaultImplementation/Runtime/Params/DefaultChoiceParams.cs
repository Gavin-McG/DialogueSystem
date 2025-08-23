using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
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