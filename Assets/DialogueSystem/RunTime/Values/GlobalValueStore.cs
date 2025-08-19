using System.Collections.Generic;

namespace DialogueSystem.Runtime.Values
{
    public class GlobalValueStore
    {
        private static GlobalValueStore _instance;
        public static GlobalValueStore Instance => _instance ??= new GlobalValueStore();
        
        private readonly Dictionary<string, object> _globalValues = new();
        
        public void Define(string valueName, object value) => _globalValues.Add(valueName, value);
        public void Undefine(string valueName) => _globalValues.Remove(valueName);
        public bool IsDefined(string valueName) => _globalValues.ContainsKey(valueName);
        public bool IsDefined<T>(string valueName) => _globalValues.ContainsKey(valueName) && _globalValues[valueName] is T;
        public object GetValue(string valueName) => _globalValues.GetValueOrDefault(valueName);
        public T GetValue<T>(string valueName)
        {
            _globalValues.TryGetValue(valueName, out var value);
            return value is T tValue ? tValue : default;
        }
        public void Clear() => _globalValues.Clear();
    }
}