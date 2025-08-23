using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    public class ResponseTimeConditional : Option
    {
        public enum ComparisonMode
        {
            LessThan,
            GreaterThan
        }

        public ComparisonMode mode;
        public float time;

        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            switch (mode)
            {
                case ComparisonMode.LessThan: return context.inputDelay < time;
                case ComparisonMode.GreaterThan: return context.inputDelay >= time;
                default: return false;
            }
        }
    }
}
