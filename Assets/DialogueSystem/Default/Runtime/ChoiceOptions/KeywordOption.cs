using DialogueSystem.Runtime;
using DialogueSystem.Runtime.Keywords;

namespace DialogueSystem.Default.Runtime
{
    public class KeywordOption : ChoiceOption
    {
        public string keyword;
        public KeywordDefineRule rule;
        
        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return KeywordUtility.EvaluateKeyword(rule, keyword, manager); 
        }
    }
}