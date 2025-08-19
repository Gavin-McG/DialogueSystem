using System.Collections.Generic;

namespace DialogueSystem.Runtime.Keywords
{
    public class GlobalKeywordStore
    {
        private static GlobalKeywordStore _instance;
        public static GlobalKeywordStore Instance => _instance ??= new GlobalKeywordStore();
        
        private readonly HashSet<string> _globalKeywords = new();
        
        public void Define(string keyword) => _globalKeywords.Add(keyword);
        public void Undefine(string keyword) => _globalKeywords.Remove(keyword);
        public bool IsDefined(string keyword) => _globalKeywords.Contains(keyword);
        public void Clear() => _globalKeywords.Clear();
    }
}