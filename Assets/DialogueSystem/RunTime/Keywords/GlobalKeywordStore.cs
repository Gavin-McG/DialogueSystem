using System.Collections.Generic;

namespace DialogueSystem.Runtime.Keywords
{
    public class GlobalKeywordStore
    {
        private static GlobalKeywordStore _instance;
        public static GlobalKeywordStore Instance => _instance ??= new GlobalKeywordStore();
        
        private readonly HashSet<string> globalKeywords = new();
        
        public void Define(string keyword) => globalKeywords.Add(keyword);
        public void Undefine(string keyword) => globalKeywords.Remove(keyword);
        public bool IsDefined(string keyword) => globalKeywords.Contains(keyword);
        public void Clear() => globalKeywords.Clear();
    }
}