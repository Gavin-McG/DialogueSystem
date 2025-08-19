namespace DialogueSystem.Runtime.Values
{
    public interface IValueContext
    {
        public void DefineValue(string valueName, object value, ValueScope scope = ValueScope.Manager);
        public void UndefineValue(string valueName, ValueScope scope = ValueScope.Manager);
        public bool IsValueDefined(string valueName);
        public object GetValue(string valueName);
        public T GetValue<T>(string valueName);
        public ValueScope GetValueScope(string valueName);
        public void ClearValues(ValueScope scope);
    }
}