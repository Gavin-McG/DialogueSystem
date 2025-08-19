using System;
using System.ComponentModel;
using DialogueSystem.Runtime;
using DialogueSystem.Runtime.Keywords;

namespace DialogueSystem.Default.Runtime
{
    public class KeywordConditional : ConditionalOption
    {
        [DefaultValue("MyKeyword")]
        public string keyword;
        
        [DefaultValue(KeywordDefineRule.IsDefined)]
        public KeywordDefineRule rule;

        public override bool EvaluateCondition(AdvanceDialogueContext context, DialogueManager manager)
        {
            return KeywordUtility.EvaluateKeyword(rule, keyword, manager);
        }
    }
}