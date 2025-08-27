using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Values
{
    ///<author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// Scriptable Object to identify distinct values. Values previously identified by string, but was changed for
    /// easier refactoring and preventing typo-related issues
    /// </summary>
    [CreateAssetMenu(menuName = "Dialogue System/ValueSO")]
    public class ValueSO : ScriptableObject
    {
        [SerializeReference] private SerializedValueBase _globalValue;
        private Dictionary<IValueContext, Dictionary<ValueScope, object>> _localValues = new();

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

        
        // Basic Value access / modification
        public object GetValue(IValueContext context)
        {
            //Check local scopes
            if (_localValues.TryGetValue(context, out var contextValues))
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
        
        
        public bool TryGetValue(IValueContext context, ValueScope scope, out object value)
        {
            if (scope == ValueScope.Global)
            {
                value = GlobalValue;
                return true;
            }
            
            //Check local scopes
            if (_localValues.TryGetValue(context, out var contextValues) &&
                contextValues.TryGetValue(scope, out value))
            {
                return true;
            }
            
            //return global scope
            value = null;
            return false;
        }
        
        
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
        
        
        public void SetValue(IValueContext context, ValueScope scope, object value)
        {
            //set global value
            if (scope == ValueScope.Global)
            {
                GlobalValue = value;
                return;
            }
            
            //add new context if necessary
            if (!_localValues.ContainsKey(context))
            {
                _localValues.Add(context, new Dictionary<ValueScope, object>());
            }
            
            //set local value
            _localValues[context][scope] = value;
        }
        
        
        public ValueScope GetValueScope(IValueContext context)
        {
            //check local scopes for stored value
            if (_localValues.TryGetValue(context, out var contextValues))
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
        
        
        public void ClearScope(IValueContext context, ValueScope scope)
        {
            //warning for global clear
            if (scope == ValueScope.Global)
            {
                Debug.LogWarning("Cannot clear global scope. clearing all lesser scopes");
            }

            //clear local scopes
            if (_localValues.TryGetValue(context, out var contextValues))
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
        
        
        public bool ValueEquals<T>(IValueContext context, T compValue)
        {
            return TryGetValue<T>(context, out var value) && value.Equals(compValue);
        }
        
        
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
    }
}