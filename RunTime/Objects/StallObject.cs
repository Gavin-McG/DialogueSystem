
namespace WolverineSoft.DialogueSystem
{
    public class StallObject : EventObject, IDialogueOutput
    {
        public DialogueInfo GetDialogueDetails(AdvanceContext advanceContext, DialogueManager manager)
        {
            return new DialogueInfo();
        }
    }
}
