using DialogueSystem.Default.Conditionals;
using DialogueSystem.Runtime;

namespace DialogueSystem.Default.ChoiceOptions
{
    public class KeywordChoiceOption : ChoiceOption
    {
        public Keywords.DefineRule rule;
        public string keyword;
        
        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return Keywords.EvaluateKeyword(rule, keyword, manager); 
        }
    }
}