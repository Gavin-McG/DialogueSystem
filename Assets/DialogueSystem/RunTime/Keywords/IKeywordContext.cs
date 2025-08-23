namespace WolverineSoft.DialogueSystem.Keywords
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Interface for classes which can store and manage keywords
    /// </summary>
    public interface IKeywordContext
    {
        /// <summary>
        /// Defines a keyword in the given scope.
        /// </summary>
        public void DefineKeyword(string keyword, KeywordScope scope);

        /// <summary>
        /// Removes a keyword definition from the given scope.
        /// </summary>
        public void UndefineKeyword(string keyword, KeywordScope scope);

        /// <summary>
        /// Checks if a keyword is currently defined in any scope.
        /// </summary>
        public bool IsKeywordDefined(string keyword);

        /// <summary>
        /// Clears all keywords defined at or below the given scope.
        /// </summary>
        public void ClearKeywords(KeywordScope scope);
    }
}