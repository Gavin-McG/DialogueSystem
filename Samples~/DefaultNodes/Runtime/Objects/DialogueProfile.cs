using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Default
{
    [DialogueValueType]
    [CreateAssetMenu(fileName = "Dialogue Profile", menuName = "Dialogue System/Dialogue Profile")]
    public class DialogueProfile : ScriptableObject
    {
        private static class Styles
        {
            public const string CharacterNameTooltip =
                "Name of character.";

            public const string SpriteTooltip =
                "Image to display for character";
        }
        
        [Tooltip(Styles.CharacterNameTooltip)]
        public string characterName;
        
        [Tooltip(Styles.SpriteTooltip)]
        public Sprite sprite;
    }
}
