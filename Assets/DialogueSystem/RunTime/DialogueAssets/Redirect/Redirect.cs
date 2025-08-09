using System.Collections.Generic;

namespace DialogueSystem.Runtime
{
    public abstract class Redirect : DialogueTrace
    {
        public DialogueTrace defaultDialogue;
        public List<ConditionalOption> options;
    }
}