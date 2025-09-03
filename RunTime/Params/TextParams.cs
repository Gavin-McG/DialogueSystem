using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class for Parameter types which require value replacements
    /// </summary>
    public abstract class TextParams
    {
        private const string RegexPattern = @"\{(.*?)\}";
        
        [HideInDialogueGraph] public string text;
        [HideInDialogueGraph] public List<DSValue> values = new List<DSValue>();
        
        public static List<string> ExtractBracketContents(string text)
        {
            List<string> result = new List<string>();
            foreach (Match match in Regex.Matches(text, RegexPattern))
            {
                result.Add(match.Groups[1].Value);
            }
            return result.Distinct().ToList();
        }
        
        internal void ReplaceText(IValueContext context)
        {
            var subStrings = ExtractBracketContents(text);
            
            text = Regex.Replace(text, RegexPattern, match =>
            {
                var subString = match.Groups[1].Value;
                int index = subStrings.IndexOf(subString);
                if (index >= values.Count)
                    return match.Value; // safeguard: leave the placeholder as-is

                var valueSO = values[index];
                if (valueSO == null)
                    return string.Empty;

                var val = valueSO.GetValue(context);
                return val?.ToString() ?? string.Empty;
            });
        }
    }
}