using DialogueSystem.Default.Conditionals;
using DialogueSystem.Runtime;

namespace DialogueSystem.Default.ChoiceOptions
{
    public class KeywordChoiceOption : ChoiceOption
    {
        public string keyword;
        public Keywords.DefineRule rule;
        
        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return Keywords.EvaluateKeyword(rule, keyword, manager); 
        }
    }
}