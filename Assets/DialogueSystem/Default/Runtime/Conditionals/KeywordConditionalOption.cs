using System;
using DialogueSystem.Runtime;

namespace DialogueSystem.Default.Runtime
{
    public class KeywordConditionalOption : ConditionalOption
    {
        public Keywords.DefineRule rule;
        public string keyword;

        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return Keywords.EvaluateKeyword(rule, keyword, manager);
        }
    }
}