using System;
using UnityEngine;

namespace DialogueSystem.Runtime.Values
{
    public static class ValueUtility
    {
        public static float GetNumericValue(string valueName, IValueContext context)
        {
            object value = context.GetValue(valueName);

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

        public static bool CompareNumericValue(ValueComp compOperation, string valueName, float compValue, IValueContext context)
        {
            float value = GetNumericValue(valueName, context);

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

        public static bool ValueEquals<T>(string valueName, T compValue, IValueContext context)
        {
            object value = context.GetValue(valueName);
            
            return value.Equals(compValue);
        }
    }
}