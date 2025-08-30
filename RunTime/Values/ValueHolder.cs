using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// ScriptableObject for performing operations on multiple/all DSValue objects.
    /// Primarily useful for clearing value scopes and save system integration.
    /// </summary>
    [CreateAssetMenu(fileName = "Values", menuName = "Dialogue System/Value Holder")]
    public class ValueHolder : ScriptableObject
    {
        [SerializeField] private List<DSValue> values = new();

        public IReadOnlyList<DSValue> Values => values;

        public void OnValidate()
        {
            Dictionary<string, DSValue> nameDict = new();
            foreach (DSValue value in values)
            {
                if (nameDict.ContainsKey(value.valueName))
                    Debug.LogWarning($"Multiple DSValues within this valueHolder contain the same name. " +
                                     $"{nameDict[value.valueName]} and {value} both have the name \"{value.valueName}\". " +
                                     $"This will interfere with the saving/reloading process");
                else
                    nameDict[value.valueName] = value;
            }
        }

        public void ClearScope(IValueContext context, DSValue.ValueScope scope)
        {
            foreach (DSValue value in values)
            {
                value?.ClearScope(context, scope);
            }
        }
        
        // internal method to refresh list
        public void SetValues(List<DSValue> newValues) => values = newValues;

        public SavedValueHolder GetSaveData()
        {
            var saved = new SavedValueHolder();
            foreach (var value in values)
            {
                saved.Values.Add(new SavedValueEntry
                {
                    ValueId = value.valueName, // or some stable identifier
                    Instances = value.GetAllValues().ToList()
                });
            }
            return saved;
        }

        public void RestoreFromSave(SavedValueHolder saved)
        {
            foreach (var entry in saved.Values)
            {
                DSValue value = values.FirstOrDefault(v => v.valueName == entry.ValueId);
                if (value != null)
                {
                    value.RestoreValues(entry.Instances);
                }
            }
        }
    }
}