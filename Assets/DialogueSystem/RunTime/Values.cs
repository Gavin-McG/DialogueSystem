using System;

namespace DialogueSystem.Runtime
{
    public static class Values
    {
        public enum Operation { Equal, NotEqual, GreaterThan, GreaterThanOrEqualTo, LessThan, LessThanOrEqualTo }

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

        public static bool EvaluateValue(Operation operation, string valueName, float compValue, DialogueManager manager)
        {
            object value = manager.GetValue(valueName);
            if (value == null) return false;

            float floatValue = 0;
            if (value is IConvertible conv)
            {
                try
                {
                    floatValue = (float)Convert.ChangeType(conv, typeof(float));
                }
                catch (Exception) {return false;}
            }

            switch (operation)
            {
                case Operation.Equal: return floatValue == compValue;
                case Operation.NotEqual: return floatValue != compValue;
                case Operation.GreaterThan: return floatValue > compValue;
                case Operation.GreaterThanOrEqualTo: return floatValue >= compValue;
                case Operation.LessThan: return floatValue < compValue;
                case Operation.LessThanOrEqualTo: return floatValue <= compValue;
                default: return false;
            }
        }
    }
}