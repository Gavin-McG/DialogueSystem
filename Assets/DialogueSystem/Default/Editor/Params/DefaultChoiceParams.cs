using DialogueSystem.Runtime;

namespace DialogueSystem.Default.Editor.Params
{
    public class DefaultChoiceParams : ChoiceParams
    {
        public bool hasTimeLimit;
        public float timeLimitDuration;

        public override ChoiceParams GetCopy() => new DefaultChoiceParams()
        {
            hasTimeLimit = hasTimeLimit,
            timeLimitDuration = timeLimitDuration
        };
    }
}