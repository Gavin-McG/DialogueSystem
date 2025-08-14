using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DialogueSystem.Runtime
{
    [Serializable]
    public class BaseParams
    {
        [SerializeField] private string text;
        public DialogueProfile profile;

        public string Text => ReplaceValues(text);
        
        public static string ReplaceValues(string input)
        {
            return Regex.Replace(input, @"\{(.*?)\}", match =>
            {
                string key = match.Groups[1].Value;
                object value = DialogueManager.GetValue(key);
                return value?.ToString() ?? "";
            });
        }
    }
}