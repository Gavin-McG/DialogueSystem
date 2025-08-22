using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DialogueSystem.Runtime.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Primary class for storing and managing values
    /// </summary>
    public class ValueContext : IValueContext
    {
        private readonly Dictionary<ValueScope, Dictionary<string, object>> _values = new();
        
        private IEnumerable<ValueScope> LocalScopes => Enum.GetValues(typeof(ValueScope))
            .Cast<ValueScope>()
            .Where(x => x != ValueScope.Global);
        
        public ValueContext()
        {
            //initialize dictionary for each non-global scope
            foreach (ValueScope scope in LocalScopes)
            {
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
            //check local scopes
            foreach (var scope in LocalScopes)
            {
                if (_values[scope].TryGetValue(valueName, out var value)) return value;
            }
            
            //check global scope
            if (GlobalValueStore.Instance.IsDefined(valueName)) 
                return GlobalValueStore.Instance.GetValue(valueName);
            
            //valueName not found
            return null;
        }
        
        public T GetValue<T>(string valueName)
        {
            bool foundLowerScope = GlobalValueStore.Instance.IsDefined(valueName);
            Type lowestType = null;
            
            // check local scopes
            foreach (var scope in LocalScopes)
            {
                if (!_values[scope].TryGetValue(valueName, out var value)) 
                    continue;

                // Record the actual type the first time we encounter this value name
                if (lowestType == null)
                    lowestType = value?.GetType();

                if (value is T tValue)
                {
                    if (foundLowerScope)
                    {
                        Debug.LogWarning(
                            $"Lowest scope of \"{valueName}\" contained value of type {lowestType?.FullName ?? "null"} " +
                            $"but requested type is {typeof(T).FullName}. Returning higher scope value instead."
                        );
                    }
                    return tValue;
                }

                // Mark that we found the name, but type did not match
                foundLowerScope = true;
            }

            // check global scope
            if (GlobalValueStore.Instance.IsDefined<T>(valueName))
            {
                if (foundLowerScope)
                {
                    Debug.LogWarning(
                        $"Lowest scope of \"{valueName}\" contained value of type {lowestType?.FullName ?? "null"} " +
                        $"but requested type is {typeof(T).FullName}. Returning global scope value instead."
                    );
                }
                return GlobalValueStore.Instance.GetValue<T>(valueName);
            }

            // correct type of valueName not found
            if (foundLowerScope)
            {
                Debug.LogWarning(
                    $"Value \"{valueName}\" was found in lowest scope as type {lowestType?.FullName ?? "null"}, " +
                    $"but requested type is {typeof(T).FullName}. Returning default value."
                );
            }

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