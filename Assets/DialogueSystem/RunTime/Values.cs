using System;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    public static class Values
    {
        public enum CompOperation { Equal, NotEqual, GreaterThan, GreaterThanOrEqualTo, LessThan, LessThanOrEqualTo }

        [Serializable]
        public abstract class ValueEntry
        {
            public abstract void SetValue(DialogueManager manager);
        }

        [Serializable]
        public class ValueEntry<T> : ValueEntry
        {
            public string valueName;
            public T value;

            public override void SetValue(DialogueManager manager)
            {
                manager.SetValue(valueName, value);
            }
        }

        public static float GetNumericValue(string valueName, DialogueManager manager)
        {
            object value = manager.GetValue(valueName);
            if (value == null)
            {
                Debug.LogWarning($"No value of name {valueName} found; Setting default value of 0");
                manager.SetValue(valueName, 0);
                return 0;
            }

            if (value is IConvertible conv)
            {
                try
                {
                    float floatValue = (float)Convert.ChangeType(conv, typeof(float));
                    return floatValue;
                }
                catch (Exception)
                {
                    Debug.LogWarning($"Could not convert IConvertible value {valueName} to float; Setting default value of 0");
                    manager.SetValue(valueName, 0);
                    return 0;
                }
            }
            
            Debug.LogWarning($"value {valueName} is not of valid numeric type; Setting default value of 0");
            manager.SetValue(valueName, 0);
            return 0;
        }

        public static bool CompareNumericValue(CompOperation compOperation, string valueName, float compValue, DialogueManager manager)
        {
            float value = GetNumericValue(valueName, manager);

            switch (compOperation)
            {
                case CompOperation.Equal: return value == compValue;
                case CompOperation.NotEqual: return value != compValue;
                case CompOperation.GreaterThan: return value > compValue;
                case CompOperation.GreaterThanOrEqualTo: return value >= compValue;
                case CompOperation.LessThan: return value < compValue;
                case CompOperation.LessThanOrEqualTo: return value <= compValue;
                default: return false;
            }
        }

        public static bool ValueEquals<T>(string valueName, T compValue, DialogueManager manager)
        {
            object value = manager.GetValue(valueName);
            
            return value.Equals(compValue);
        }
    }
}