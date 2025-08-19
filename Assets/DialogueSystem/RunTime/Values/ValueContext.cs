using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Runtime.Values
{
    public class ValueContext : IValueContext
    {
        private readonly Dictionary<ValueScope, Dictionary<string, object>> _values = new();
        
        public ValueContext()
        {
            //initialize dictionary for each non-global scope
            foreach (ValueScope scope in Enum.GetValues(typeof(ValueScope)))
            {
                if (scope != ValueScope.Global)
                    _values.Add(scope, new Dictionary<string, object>());
            }
        }

        public void DefineValue(string valueName, object value, ValueScope scope = ValueScope.Manager)
        {
            if (scope == ValueScope.Global)
                //define in global scope
                GlobalValueStore.Instance.Define(valueName, value);
            else
                //define in local scope
                _values[scope][valueName] = value;
        }

        public void UndefineValue(string valueName, ValueScope scope = ValueScope.Manager)
        {
            //remove from global if necessary
            if (scope == ValueScope.Global)
                GlobalValueStore.Instance.Undefine(valueName);
        
            //remove from all scopes <= to the removed scope
            foreach (var kvp in _values)
            {
                if (kvp.Key <= scope)
                {
                    kvp.Value.Remove(valueName);
                }
            }
        }

        public bool IsValueDefined(string valueName)
        {
            //check global scope
            if (GlobalValueStore.Instance.IsDefined(valueName)) return true;

            //check local scopes
            foreach (var kvp in _values)
            {
                if (kvp.Value.ContainsKey(valueName)) return true;
            }
            
            //valueName not found
            return false;
        }

        public object GetValue(string valueName)
        {
            //check global scope
            if (GlobalValueStore.Instance.IsDefined(valueName)) 
                return GlobalValueStore.Instance.GetValue(valueName);
            
            //check local scopes
            foreach (var kvp in _values)
            {
                if (kvp.Value.TryGetValue(valueName, out var value)) return value;
            }
            
            //valueName not found
            return null;
        }
        
        public T GetValue<T>(string valueName)
        {
            //check global scope
            if (GlobalValueStore.Instance.IsDefined<T>(valueName)) 
                return GlobalValueStore.Instance.GetValue<T>(valueName);
            
            //check local scopes
            bool foundHigherScope = GlobalValueStore.Instance.IsDefined(valueName);
            foreach (var kvp in _values)
            {
                if (!kvp.Value.TryGetValue(valueName, out var value)) continue;
                
                if (value is T tValue)
                {
                    if (foundHigherScope)
                        Debug.LogWarning($"Returning \"{valueName}\" value of lower scope to provide correct value Type");
                    
                    return tValue;
                }
                foundHigherScope = true;
            }
            
            //correct type of valueName not found
            if (foundHigherScope)
                Debug.LogWarning($"\"{valueName}\" was of incorrect Type");
            
            return default;
        }

        public ValueScope GetValueScope(string valueName)
        {
            if (GlobalValueStore.Instance.IsDefined(valueName))
                return ValueScope.Global;

            foreach (var kvp in _values)
            {
                if (kvp.Value.ContainsKey(valueName)) return kvp.Key;
            }

            return ValueScope.Single;
        }

        public void ClearValues(ValueScope scope)
        {
            //clear global if scope is global
            //TODO - assign all other KeyWordContexts to clear when global is cleared
            if (scope == ValueScope.Global)
                GlobalValueStore.Instance.Clear();
            
            //clear all scopes <= to the cleared scope
            foreach (var kvp in _values)
            {
                if (kvp.Key <= scope)
                {
                    kvp.Value.Clear();
                }
            }
        }
    }
}