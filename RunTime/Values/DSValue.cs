using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// Scriptable Object to identify distinct values. Values previously identified by string, but was changed for
    /// easier refactoring and preventing typo-related issues
    /// </summary>
    [CreateAssetMenu(menuName = "Dialogue System/Dialogue Value")]
    public class DSValue : ScriptableObject
    {
        [SerializeField, Delayed] public string valueName;
        
        [SerializeReference] private SerializedValueBase _globalValue;
        private Dictionary<string, Dictionary<ValueScope, object>> _localValues = new();

        public enum ValueScope { Dialogue = 0, Manager = 1, Global = 2 }
        public enum ValueComp { Equal, NotEqual, GreaterThan, GreaterThanOrEqualTo, LessThan, LessThanOrEqualTo }
        public enum ValueOperation { Add, Subtract, Multiply, Divide }
        
        private IEnumerable<ValueScope> LocalScopes => Enum.GetValues(typeof(ValueScope))
            .Cast<ValueScope>()
            .Where(x => x != ValueScope.Global);
        
        private object GlobalValue
        {
            get => _globalValue?.GetValue();
            set
            {
                if (value == null)
                {
                    _globalValue = null;
                    return;
                }

                var wrapperType = typeof(SerializedValue<>).MakeGenericType(value.GetType());
                _globalValue = (SerializedValueBase)System.Activator.CreateInstance(wrapperType, value);
            }
        }

        //ensures valueName is not empty
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(valueName))
                valueName = name;
        }

        /// <summary>
        /// Retrieve the lowest context value stored for context
        /// </summary>
        public object GetValue(IValueContext context)
        {
            //Check local scopes
            if (_localValues.TryGetValue(context.ContextName, out var contextValues))
            {
                foreach (var scope in LocalScopes)
                {
                    if (contextValues.TryGetValue(scope, out var value))
                        return value;
                }
            }
            
            //return global scope
            return GlobalValue;
        }
        
        /// <summary>
        /// Attempts to retrieve a value from the specific context
        /// </summary>
        public bool TryGetValue(IValueContext context, out float value)
        {
            object obj = GetValue(context);
            
            switch (obj)
            {
                case null: value = 0f; return false;
                case IConvertible conv: 
                    try {
                        value = (float)Convert.ChangeType(conv, typeof(float));
                        return true;
                    } catch (Exception) {
                        value = 0f; return false;
                    };
                default: value = 0f; return false;
            }
        }
        
        /// <summary>
        /// Attempts to retrieve a value from the specific scope and context
        /// </summary>
        public bool TryGetValue(IValueContext context, ValueScope scope, out object value)
        {
            if (scope == ValueScope.Global)
            {
                value = GlobalValue;
                return true;
            }
            
            //Check local scopes
            if (_localValues.TryGetValue(context.ContextName, out var contextValues) &&
                contextValues.TryGetValue(scope, out value))
            {
                return true;
            }
            
            //return global scope
            value = null;
            return false;
        }
        
        /// <summary>
        /// Attempts to retrieve a value from the specific context of type T
        /// </summary>
        public bool TryGetValue<T>(IValueContext context, out T value)
        {
            var obj = GetValue(context);
            if (obj is T tObj)
            {
                value = tObj;
                return true;
            }
            value = default;
            return false;
        }
        
        /// <summary>
        /// Attempts to retrieve a value from the specific scope and context of type T
        /// </summary>
        public bool TryGetValue<T>(IValueContext context, ValueScope scope, out T value)
        {
            if (TryGetValue(context, scope, out var obj) && obj is T tObj)
            {
                value = tObj;
                return true;
            }
            value = default;
            return false;
        }
        
        /// <summary>
        /// Sets value for the given context and scope
        /// </summary>
        public void SetValue(IValueContext context, ValueScope scope, object value)
        {
            //set global value
            if (scope == ValueScope.Global)
            {
                GlobalValue = value;
                return;
            }
            
            //add new context if necessary
            if (!_localValues.ContainsKey(context.ContextName))
            {
                _localValues.Add(context.ContextName, new Dictionary<ValueScope, object>());
            }
            
            //set local value
            _localValues[context.ContextName][scope] = value;
        }
        
        /// <summary>
        /// Gets the lowest scope which a value is stored for the provided context. Global if no context values are stored
        /// </summary>
        public ValueScope GetValueScope(IValueContext context)
        {
            //check local scopes for stored value
            if (_localValues.TryGetValue(context.ContextName, out var contextValues))
            {
                foreach (var scope in LocalScopes)
                {
                    if (contextValues.ContainsKey(scope))
                        return scope;
                }
            }
            
            //default to global scope
            return ValueScope.Global;
        }
        
        /// <summary>
        /// CLear all associated vaalues of the given context below the provided scope.
        /// </summary>
        public void ClearScope(IValueContext context, ValueScope scope)
        {
            //warning for global clear
            if (scope == ValueScope.Global)
            {
                GlobalValue = null;
            }

            _localValues ??= new();

            //clear local scopes
            if (_localValues.TryGetValue(context.ContextName, out var contextValues))
            {
                foreach (var localScope in LocalScopes)
                {
                    if (localScope <= scope)
                        contextValues.Remove(localScope);
                }
            }
        }
        
        
        //modifiers / comparisons
        public bool TryCompareValue(IValueContext context, ValueComp compOperation, float compValue, out bool result)
        {
            if (TryGetValue(context, out var value))
            {
                //evaluate operation
                switch (compOperation)
                {
                    case ValueComp.Equal: result = value == compValue; break;
                    case ValueComp.NotEqual: result = value != compValue; break;
                    case ValueComp.GreaterThan: result = value > compValue; break;
                    case ValueComp.GreaterThanOrEqualTo: result = value >= compValue; break;
                    case ValueComp.LessThan: result = value < compValue; break;
                    case ValueComp.LessThanOrEqualTo: result = value <= compValue; break;
                    default: result = false; break;
                }

                return true;
            }
            
            //value wasnt numeric type
            result = false;
            return false;
        }
        
        /// <summary>
        /// Check if the currently stored value matches compValue
        /// </summary>
        public bool ValueEquals<T>(IValueContext context, T compValue)
        {
            return TryGetValue<T>(context, out var value) && value.Equals(compValue);
        }
        
        /// <summary>
        /// Apply a mathematical operation to the stored value of the current lowest scope
        /// </summary>
        public bool TryOperateValue(IValueContext context, ValueOperation operation, float otherValue)
        {
            if (TryGetValue(context, out float value))
            {
                float newValue;
                switch (operation)
                {
                    case ValueOperation.Add: newValue = value + otherValue; break;
                    case ValueOperation.Subtract: newValue = value - otherValue; break;
                    case ValueOperation.Multiply: newValue = value * otherValue; break;
                    case ValueOperation.Divide: newValue = value / otherValue; break;
                    default: newValue = 0; break;
                }
                
                var scope = GetValueScope(context);
                SetValue(context, scope, newValue);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a simple list of all stored values with associated context/scope
        /// </summary>
        public SavedDSValue GetData()
        {
            SavedDSValue entry = new SavedDSValue();
            entry.ValueId = valueName;

            if (_globalValue != null)
            {
                entry.Instances.Add(new ValueInstance()
                {
                    value = _globalValue,
                    contextName = "Global",
                    scope = ValueScope.Global
                });
            }

            foreach (var contextKVP in _localValues)
            {
                foreach (var scopeKVP in contextKVP.Value)
                {
                    var value = scopeKVP.Value;
                    if (value == null) continue;

                    var wrapperType = typeof(SerializedValue<>).MakeGenericType(value.GetType());
                    var valueWrapper = (SerializedValueBase)System.Activator.CreateInstance(wrapperType, value);

                    entry.Instances.Add(new ValueInstance()
                    {
                        value = valueWrapper,
                        contextName = contextKVP.Key,
                        scope = scopeKVP.Key
                    });
                }
            }

            return entry;
        }

        /// <summary>
        /// Restore the values from a saved List (must have maintained polymorphism if used in a save/serialization system)
        /// </summary>
        public void RestoreFromData(SavedDSValue entry)
        {
            if (entry.ValueId != valueName)
            {
                Debug.LogWarning("Cannot restore value from save because ValueId does not match name. " + "" +
                                 $"ValueId of save is \"{entry.ValueId}\", valueName of DSValue is \"{valueName}\"");
                return;
            }
            
            if (entry?.Instances == null)
            {
                Debug.LogWarning("RestoreValues called with null values collection.");
                return;
            }

            _localValues.Clear();
            _globalValue = null;

            foreach (var instance in entry.Instances)
            {
                if (instance == null || instance.value == null)
                    continue;

                if (instance.scope == ValueScope.Global)
                {
                    if (instance.contextName != "Global")
                    {
                        Debug.LogWarning(
                            $"Global value must use contextName='Global' and scope=ValueScope.Global. " +
                            $"Got context='{instance.contextName}', scope='{instance.scope}'. Resetting global to null."
                        );
                        _globalValue = null;
                        continue;
                    }

                    _globalValue = instance.value;
                }
                else
                {
                    if (!_localValues.TryGetValue(instance.contextName, out var contextValues))
                    {
                        contextValues = new Dictionary<ValueScope, object>();
                        _localValues[instance.contextName] = contextValues;
                    }

                    contextValues[instance.scope] = instance.value.GetValue();
                }
            }
        }
        
        public string GetSaveData()
        {
            var data = GetData();
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
            var json = JsonConvert.SerializeObject(data, settings);
            return json;
        }

        public void RestoreFromSaveData(string saveData)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented
                };
                var data = JsonConvert.DeserializeObject<SavedDSValue>(saveData, settings);
                RestoreFromData(data);
            }
            catch (JsonReaderException e)
            {
                Debug.LogWarning($"Could not restore value {valueName} due to JSON read error.");
            }
        }
    }
}