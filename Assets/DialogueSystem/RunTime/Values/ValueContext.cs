using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Primary class for storing and managing values
    /// </summary>
    public class ValueContext : IValueContext
    {
        private readonly Dictionary<ValueScope, Dictionary<ValueSO, object>> _values = new();
        
        private static readonly Dictionary<string, ValueSO> ValueNames = new();
        
        private IEnumerable<ValueScope> LocalScopes => Enum.GetValues(typeof(ValueScope))
            .Cast<ValueScope>()
            .Where(x => x != ValueScope.Global);
        
        public ValueContext()
        {
            //initialize dictionary for each non-global scope
            foreach (ValueScope scope in LocalScopes)
            {
                _values.Add(scope, new Dictionary<ValueSO, object>());
            }
        }

        public void DefineValue(ValueSO valueSO, object value, ValueScope scope = ValueScope.Manager)
        {
            if (scope == ValueScope.Global)
                //define in global scope
                GlobalValueStore.Instance.Define(valueSO, value);
            else
                //define in local scope
                _values[scope][valueSO] = value;

            //Add value name to dict
            if (ValueNames.TryGetValue(valueSO.valueName, out var currentSO))
            {
                if (currentSO == valueSO) return;
                
                Debug.LogError($"Attempted to define value {valueSO.name} using name {valueSO.valueName}." +
                               $"value {currentSO.name} has already been defined with the same name. " +
                               $"valueName queries will default to older value.");
            }
            
            ValueNames[valueSO.valueName] = valueSO;
        }

        public void UndefineValue(ValueSO valueSO, ValueScope scope = ValueScope.Manager)
        {
            //remove from global if necessary
            if (scope == ValueScope.Global)
                GlobalValueStore.Instance.Undefine(valueSO);
        
            //remove from all scopes <= to the removed scope
            foreach (var kvp in _values)
            {
                if (kvp.Key <= scope)
                {
                    kvp.Value.Remove(valueSO);
                }
            }
        }

        public bool IsValueDefined(ValueSO valueSO)
        {
            //check global scope
            if (GlobalValueStore.Instance.IsDefined(valueSO)) return true;

            //check local scopes
            foreach (var kvp in _values)
            {
                if (kvp.Value.ContainsKey(valueSO)) return true;
            }
            
            //valueName not found
            return false;
        }

        public object GetValue(ValueSO valueSO)
        {
            //check local scopes
            foreach (var scope in LocalScopes)
            {
                if (_values[scope].TryGetValue(valueSO, out var value)) return value;
            }
            
            //check global scope
            if (GlobalValueStore.Instance.IsDefined(valueSO)) 
                return GlobalValueStore.Instance.GetValue(valueSO);
            
            //valueName not found
            return null;
        }
        
        public T GetValue<T>(ValueSO valueSO)
        {
            bool foundLowerScope = GlobalValueStore.Instance.IsDefined(valueSO);
            Type lowestType = null;
            
            // check local scopes
            foreach (var scope in LocalScopes)
            {
                if (!_values[scope].TryGetValue(valueSO, out var value)) 
                    continue;

                // Record the actual type the first time we encounter this value name
                if (lowestType == null)
                    lowestType = value?.GetType();

                if (value is T tValue)
                {
                    if (foundLowerScope)
                    {
                        Debug.LogWarning(
                            $"Lowest scope of \"{valueSO.name}\" contained value of type {lowestType?.FullName ?? "null"} " +
                            $"but requested type is {typeof(T).FullName}. Returning higher scope value instead."
                        );
                    }
                    return tValue;
                }

                // Mark that we found the name, but type did not match
                foundLowerScope = true;
            }

            // check global scope
            if (GlobalValueStore.Instance.IsDefined<T>(valueSO))
            {
                if (foundLowerScope)
                {
                    Debug.LogWarning(
                        $"Lowest scope of \"{valueSO.name}\" contained value of type {lowestType?.FullName ?? "null"} " +
                        $"but requested type is {typeof(T).FullName}. Returning global scope value instead."
                    );
                }
                return GlobalValueStore.Instance.GetValue<T>(valueSO);
            }

            // correct type of valueName not found
            if (foundLowerScope)
            {
                Debug.LogWarning(
                    $"Value \"{valueSO.name}\" was found in lowest scope as type {lowestType?.FullName ?? "null"}, " +
                    $"but requested type is {typeof(T).FullName}. Returning default value."
                );
            }

            return default;
        }

        public object GetValue(string valueName)
        {
            if (ValueNames.TryGetValue(valueName, out var valueSO))
                return GetValue(valueSO);
            
            Debug.LogWarning(
                $"No value of name {valueName} has been defined. Make sure the valueName matches that of a defined ValueSO. " +
                $"Returning default value."
            );
            return null;
        }

        public T GetValue<T>(string valueName)
        {
            if (ValueNames.TryGetValue(valueName, out var valueSO))
                return GetValue<T>(valueSO);
            
            Debug.LogWarning(
                $"No value of name {valueName} has been defined. Make sure the valueName matches that of a defined ValueSO. " +
                $"Returning default value."
            );
            return default;
        }

        public ValueScope GetValueScope(ValueSO valueSO)
        {
            if (GlobalValueStore.Instance.IsDefined(valueSO))
                return ValueScope.Global;

            foreach (var kvp in _values)
            {
                if (kvp.Value.ContainsKey(valueSO)) return kvp.Key;
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