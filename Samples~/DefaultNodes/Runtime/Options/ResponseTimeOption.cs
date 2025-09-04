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

        public override bool EvaluateCondition(AdvanceParams advanceParams, DialogueManager manager)
        {
            if (advanceParams is not DefaultAdvanceParams defaultParams) return false;
            
            switch (mode)
            {
                case ComparisonMode.LessThan: return defaultParams.inputDelay < time;
                case ComparisonMode.GreaterThan: return defaultParams.inputDelay >= time;
                default: return false;
            }
        }
    }
}
