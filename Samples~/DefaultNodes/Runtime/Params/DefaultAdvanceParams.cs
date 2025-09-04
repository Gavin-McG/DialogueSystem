namespace WolverineSoft.DialogueSystem.Default
{
    public class DefaultAdvanceParams : AdvanceParams
    {
        public float inputDelay;

        public DefaultAdvanceParams(float inputDelay, int choice, bool timedOut) : base(choice, timedOut)
        {
            this.inputDelay = inputDelay;
        }
    }
}