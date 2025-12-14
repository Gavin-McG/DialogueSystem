
namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Interface for DialogueTrace classes which are meant to provide an output to AdvanceDialogue
    /// Currently limited to <see cref="TextObject"/> and <see cref="ChoiceObject"/>
    /// </summary>
    public interface IDialogueOutput
    {
        public DialogueInfo GetDialogueDetails(AdvanceContext advanceContext, DialogueManager manager);
    }

}
