using System.Collections.Generic;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Values
{
    [CreateAssetMenu(fileName = "Values", menuName = "Dialogue System/Value Holder")]
    public class ValueHolder : ScriptableObject
    {
        [SerializeField] private List<ValueSO> values = new();

        public IReadOnlyList<ValueSO> Values => values;
        
        public void ClearScope(IValueContext context, ValueSO.ValueScope scope)
        {
            foreach (ValueSO value in values)
            {
                value.ClearScope(context, scope);
            }
        }
        
        // internal method to refresh list
        public void SetValues(List<ValueSO> newValues) => values = newValues;
    }
}