using System;
using System.ComponentModel;

namespace DialogueSystem.Runtime.Keywords
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Class representing a specific modification to a IKeywordContext
    /// </summary>
    [Serializable]
    public class KeywordEditor
    {
        [DefaultValue("MyKeyword")] 
        public string keyword;
        
        [DefaultValue(KeywordScope.Dialogue)] 
        public KeywordScope scope;
        
        [DefaultValue(KeywordOperation.Add)]
        public KeywordOperation operation;
        
        public void Apply(IKeywordContext context)
        {
            switch (operation)
            {
                case KeywordOperation.Add: context.DefineKeyword(keyword, scope); break;
                case KeywordOperation.Remove: context.UndefineKeyword(keyword, scope); break;
                case KeywordOperation.RemoveAll: context.ClearKeywords(scope); break;
                default: break;
            }
        }
    }
}