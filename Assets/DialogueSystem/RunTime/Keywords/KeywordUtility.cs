using System;

namespace WolverineSoft.DialogueSystem.Runtime.Keywords
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// static class for keyword-related helper functions
    /// </summary>
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