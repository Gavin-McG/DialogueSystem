using System.Collections.Generic;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Singleton class used to store the current global values
    /// </summary>
    internal class GlobalValueStore
    {
        private static GlobalValueStore _instance;
        public static GlobalValueStore Instance => _instance ??= new GlobalValueStore();
        
        private readonly Dictionary<ValueSO, object> _globalValues = new();
        
        public void Define(ValueSO valueSO, object value) => _globalValues.Add(valueSO, value);
        public void Undefine(ValueSO valueSO) => _globalValues.Remove(valueSO);
        public bool IsDefined(ValueSO valueSO) => _globalValues.ContainsKey(valueSO);
        public bool IsDefined<T>(ValueSO valueSO) => _globalValues.ContainsKey(valueSO) && _globalValues[valueSO] is T;
        public object GetValue(ValueSO valueSO) => _globalValues.GetValueOrDefault(valueSO);
        public T GetValue<T>(ValueSO valueSO)
        {
            _globalValues.TryGetValue(valueSO, out var value);
            return value is T tValue ? tValue : default;
        }
        public void Clear() => _globalValues.Clear();
    }
}