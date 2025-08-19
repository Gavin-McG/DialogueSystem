using System;

namespace DialogueSystem.Runtime.Keywords
{
    public static class KeywordUtility
    {
        
        public static bool EvaluateKeyword(KeywordDefineRule rule, string keywordName, IKeywordContext context)
        {
            switch (rule)
            {
                case KeywordDefineRule.IsDefined: return context.IsKeywordDefined(keywordName);
                case KeywordDefineRule.IsNotDefined: return !context.IsKeywordDefined(keywordName);
                default: return false;
            }
        }
    }
}