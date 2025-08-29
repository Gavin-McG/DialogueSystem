using System.Collections.Generic;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Values
{
    [CreateAssetMenu(fileName = "Values", menuName = "Dialogue System/Value Holder")]
    public class ValueHolder : ScriptableObject
    {
        [SerializeField] private List<DSValue> values = new();

        public IReadOnlyList<DSValue> Values => values;
        
        public void ClearScope(IValueContext context, DSValue.ValueScope scope)
        {
            foreach (DSValue value in values)
            {
                value?.ClearScope(context, scope);
            }
        }
        
        // internal method to refresh list
        public void SetValues(List<DSValue> newValues) => values = newValues;
    }
}