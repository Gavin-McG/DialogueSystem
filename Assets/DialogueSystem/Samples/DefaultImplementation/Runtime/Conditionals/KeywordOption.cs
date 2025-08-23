using System;
using System.ComponentModel;
using WolverineSoft.DialogueSystem;
using WolverineSoft.DialogueSystem.Keywords;

namespace WolverineSoft.DialogueSystem.Default
{
    public class KeywordOption : Option
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