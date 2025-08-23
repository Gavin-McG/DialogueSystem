using UnityEngine;

namespace WolverineSoft.DialogueSystem.Default.Runtime
{
    [CreateAssetMenu(fileName = "Dialogue Profile", menuName = "Dialogue System/Dialogue Profile")]
    public class DialogueProfile : ScriptableObject
    {
        private static class Styles
        {
            public const string CharacterNameTooltip =
                "Name of character. Used to identify duplicate character on other side";

            public const string DisplayNameTooltip =
                "Name to display for character";

            public const string SpriteTooltip =
                "Image to display for character";

            public const string RightSideTooltip =
                "Whether character is on right or left Side";
        }
        
        public enum Side { Right, Left }

        [Tooltip(Styles.CharacterNameTooltip)]
        public string characterName;
        
        [Tooltip(Styles.DisplayNameTooltip)]
        public string displayName;
        
        [Tooltip(Styles.SpriteTooltip)]
        public Sprite sprite;
        
        [Tooltip(Styles.RightSideTooltip)]
        public Side side;
    }
}
