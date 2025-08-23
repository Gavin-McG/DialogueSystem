using UnityEngine;

namespace WolverineSoft.DialogueSystem.Values
{
    ///<author>Gavin McGinness</author>
    /// <date>2025-08-23</date>
    
    /// <summary>
    /// Scriptable Object to identify distinct values. Values previously identified by string, but was changed for
    /// easier refactoring and preventing typo-related issues
    /// </summary>
    [CreateAssetMenu(menuName = "Dialogue System/ValueSO")]
    public class ValueSO : ScriptableObject
    {
        public string valueName = "MyValue";
    }
}