using System;
using System.Collections.Generic;
using System.Linq;

namespace DialogueSystem.Runtime.Keywords
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Primary class for storing and managing keywords
    /// </summary>
    public class KeywordContext : IKeywordContext
    {
        private readonly Dictionary<KeywordScope, HashSet<string>> _keywords = new();
        
        private IEnumerable<KeywordScope> LocalScopes => Enum.GetValues(typeof(KeywordScope))
            .Cast<KeywordScope>()
            .Where(x => x != KeywordScope.Global);

        public KeywordContext()
        {
            //initialize hashSet for each non-global scope
            foreach (KeywordScope scope in LocalScopes)
            {
                _keywords.Add(scope, new HashSet<string>());
            }
        }

        public void DefineKeyword(string keyword, KeywordScope scope)
        {
            if (scope == KeywordScope.Global)
                //define in global scope
                GlobalKeywordStore.Instance.Define(keyword);
            else
                //define in local scope
                _keywords[scope].Add(keyword);
        }

        public void UndefineKeyword(string keyword, KeywordScope scope)
        {
            //remove from global if necessary
            if (scope == KeywordScope.Global)
                GlobalKeywordStore.Instance.Undefine(keyword);
            
            //remove from all scopes <= to the removed scope
            foreach (var kvp in _keywords)
            {
                if (kvp.Key <= scope)
                {
                    kvp.Value.Remove(keyword);
                }
            }
        }

        public bool IsKeywordDefined(string keyword)
        {
            //check global scope
            if (GlobalKeywordStore.Instance.IsDefined(keyword)) return true;

            //check local scopes
            foreach (var kvp in _keywords)
            {
                if (kvp.Value.Contains(keyword)) return true;
            }
            
            //keyword not found
            return false;
        }

        public void ClearKeywords(KeywordScope scope)
        {
            //clear global if scope is global
            //TODO - assign all other KeyWordContexts to clear when global is cleared
            if (scope == KeywordScope.Global)
                GlobalKeywordStore.Instance.Clear();
            
            //clear all scopes <= to the cleared scope
            foreach (var kvp in _keywords)
            {
                if (kvp.Key <= scope)
                {
                    kvp.Value.Clear();
                }
            }
        }
    }
}