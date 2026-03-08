using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WolverineSoft.DialogueSystem.Editor
{
    [CustomPropertyDrawer(typeof(TextHolder), true)]
    public class TextHolderDrawer : PropertyDrawer
    {
        private const int defaultWidth = 250;
        private const int defaultHeight = 80;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            
            //get TextHolderAttribute info
            var holderType = property.boxedValue.GetType();
            var holderAttribute = holderType.GetAttribute<TextHolderAttribute>();
            int width = holderAttribute?.width ?? defaultWidth;
            int height = holderAttribute?.height ?? defaultHeight;
            
            //set -1 w/h to defaults
            width = width==-1 ? defaultWidth : width;
            height = height==-1 ? defaultHeight : height;
            
            // Create text field
            var textProp = property.FindPropertyRelative("text");
            var textField = new TextField();
            textField.bindingPath = textProp.propertyPath;
            
            //Configure Text field behavior
            textField.multiline = true;
            textField.verticalScrollerVisibility = ScrollerVisibility.Auto;

            //Text field Style
            textField.style.width = width;
            textField.style.minHeight = height;
            textField.style.whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal);
            textField.Q<VisualElement>("unity-text-input").style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            textField.Q<VisualElement>("unity-text-input").style.backgroundColor = new StyleColor(new Color(0.1f,0.1f,0.1f));
            root.Add(textField);
            
            return root;
        }
    }
}