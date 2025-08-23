using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// static class for value-related helper functions
    /// </summary>
    public static class ValueUtility
    {
        public static float GetNumericValue(ValueSO valueSO, IValueContext context)
        {
            object value = context.GetValue(valueSO);

            switch (value)
            {
                case null: return 0f;
                case IConvertible conv: 
                    try
                    {
                        float floatValue = (float)Convert.ChangeType(conv, typeof(float));
                        return floatValue;
                    }
                    catch (Exception)
                    {
                        return 0;
                    };
                default: return 0f;
            }
        }

        public static bool CompareNumericValue(ValueComp compOperation, ValueSO valueSO, float compValue, IValueContext context)
        {
            float value = GetNumericValue(valueSO, context);

            switch (compOperation)
            {
                case ValueComp.Equal: return value == compValue;
                case ValueComp.NotEqual: return value != compValue;
                case ValueComp.GreaterThan: return value > compValue;
                case ValueComp.GreaterThanOrEqualTo: return value >= compValue;
                case ValueComp.LessThan: return value < compValue;
                case ValueComp.LessThanOrEqualTo: return value <= compValue;
                default: return false;
            }
        }

        public static bool ValueEquals<T>(ValueSO valueSO, T compValue, IValueContext context)
        {
            object value = context.GetValue(valueSO);
            
            return value.Equals(compValue);
        }
    }
}