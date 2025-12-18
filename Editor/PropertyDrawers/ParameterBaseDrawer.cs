using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace WolverineSoft.DialogueSystem.Editor
{
    [CustomPropertyDrawer(typeof(ParameterBase), true)]
    public class ParameterBaseDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();

            if (property.managedReferenceValue == null)
                return root;

            DrawChildren(property, root);

            return root;
        }

        private static void DrawChildren(SerializedProperty property, VisualElement parent)
        {
            SerializedProperty iterator = property.Copy();
            SerializedProperty end = iterator.GetEndProperty();

            // Move to first visible child
            iterator.NextVisible(true);

            while (!SerializedProperty.EqualContents(iterator, end))
            {
                var field = new PropertyField(iterator);
                parent.Add(field);

                iterator.NextVisible(false);
            }
        }
    }
}