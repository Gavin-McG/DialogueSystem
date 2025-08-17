using DialogueSystem.Runtime;

namespace DialogueSystem.Default.Runtime
{
    public class KeywordOption : ChoiceOption
    {
        public string keyword;
        public Keywords.DefineRule rule;
        
        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return Keywords.EvaluateKeyword(rule, keyword, manager); 
        }
    }
}