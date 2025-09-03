using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    public class ResponseTimeOption : Option
    {
        public enum ComparisonMode
        {
            LessThan,
            GreaterThan
        }

        public ComparisonMode mode;
        public float time;

        public override bool EvaluateCondition(AdvanceContext context, DialogueManager manager)
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
