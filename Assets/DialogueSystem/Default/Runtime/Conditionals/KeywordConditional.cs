using System;
using DialogueSystem.Runtime;
using DialogueSystem.Runtime.Keywords;

namespace DialogueSystem.Default.Runtime
{
    public class KeywordConditional : ConditionalOption
    {
        public KeywordDefineRule rule;
        public string keyword;

        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return KeywordUtility.EvaluateKeyword(rule, keyword, manager);
        }
    }
}