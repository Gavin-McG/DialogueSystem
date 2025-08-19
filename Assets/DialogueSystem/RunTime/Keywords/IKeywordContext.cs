namespace DialogueSystem.Runtime.Keywords
{
    public interface IKeywordContext
    {
        public void DefineKeyword(string keyword, KeywordScope scope);
        public void UndefineKeyword(string keyword, KeywordScope scope);
        public bool IsKeywordDefined(string keyword);
        public void ClearKeywords(KeywordScope scope);
    }
}