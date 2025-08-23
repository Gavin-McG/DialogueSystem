using System;
using System.ComponentModel;
using WolverineSoft.DialogueSystem.Runtime;
using WolverineSoft.DialogueSystem.Runtime.Keywords;

namespace WolverineSoft.DialogueSystem.Default.Runtime
{
    public class KeywordConditional : Option
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