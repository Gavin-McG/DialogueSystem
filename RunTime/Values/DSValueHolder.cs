using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Values
{
    /// <summary>
    /// ScriptableObject for performing operations on multiple/all DSValue objects.
    /// Primarily useful for clearing value scopes and save system integration.
    /// </summary>
    [CreateAssetMenu(fileName = "Values", menuName = "Dialogue System/Value Holder")]
    public class DSValueHolder : ScriptableObject
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

        public SavedDSValueHolder GetData()
        {
            var data = new SavedDSValueHolder();
            foreach (var value in values)
            {
                data.Values.Add(value.GetData());
            }
            return data;
        }

        public void RestoreFromData(SavedDSValueHolder saved)
        {
            foreach (var entry in saved.Values)
            {
                DSValue value = values.FirstOrDefault(v => v.valueName == entry.ValueId);
                if (value != null)
                {
                    value.RestoreFromData(entry);
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
                var data = JsonConvert.DeserializeObject<SavedDSValueHolder>(saveData, settings);
                RestoreFromData(data);
            }
            catch (JsonReaderException e)
            {
                Debug.LogWarning($"Could not restore valueHolder {name} due to JSON read error.");
            }
        }
    }
}