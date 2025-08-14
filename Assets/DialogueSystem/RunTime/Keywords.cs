namespace DialogueSystem.Runtime
{
    public static class Keywords
    {
        public enum DefineRule { IsDefined, IsNotDefined }

        public static bool EvaluateKeyword(DefineRule rule, string keyword, DialogueManager manager)
        {
            switch (rule)
            {
                case DefineRule.IsDefined: return manager.IsKeywordDefined(keyword);
                case DefineRule.IsNotDefined: return !manager.IsKeywordDefined(keyword);
                default: return false;
            }
        }
    }
}