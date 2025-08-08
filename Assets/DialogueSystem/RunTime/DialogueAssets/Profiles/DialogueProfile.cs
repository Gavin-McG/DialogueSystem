using UnityEngine;

namespace DialogueSystem.Runtime
{
    [CreateAssetMenu(fileName = "Dialogue Profile", menuName = "Dialogue System/Dialogue Profile")]
    public class DialogueProfile : DialogueObject
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

        [Tooltip(Styles.CharacterNameTooltip)]
        public string characterName;
        
        [Tooltip(Styles.DisplayNameTooltip)]
        public string displayName;
        
        [Tooltip(Styles.SpriteTooltip)]
        public Sprite sprite;
        
        [Tooltip(Styles.RightSideTooltip)]
        public bool rightSide;
        
        public Shirt shirt;
        
        public enum Mood { Happy, Sad, Mad, Horny }

        public Mood mood;
    }

    
    [System.Serializable]
    public class Shirt
    {
        public Color color;
        public int size;
        public Icon icon;
    }

    [System.Serializable]
    public struct Icon
    {
        public Color color;
        public Sprite sprite;
    }
}
