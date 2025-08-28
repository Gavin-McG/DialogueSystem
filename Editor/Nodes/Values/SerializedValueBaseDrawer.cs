using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{ 
    
    [CustomPropertyDrawer(typeof(SerializedValueBase), true)]
    public class SerializedValueDrawer : PropertyDrawer
    {
        private static List<Type> _availableTypes;
        private static string[] _typeNames;

        static SerializedValueDrawer()
        {
            // Built-in supported types
            _availableTypes = new List<Type>
            {
                typeof(SerializedValue<string>),
                typeof(SerializedValue<int>),
                typeof(SerializedValue<float>),
                typeof(SerializedValue<bool>),
                typeof(SerializedValue<Vector2>),
                typeof(SerializedValue<Vector3>),
                typeof(SerializedValue<GameObject>),
                typeof(SerializedValue<AudioClip>),
            };

            // Add any custom types marked with [DialogueValueType]
            var customTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttributes(typeof(DialogueValueTypeAttribute), false).Any())
                .Select(t => typeof(SerializedValue<>).MakeGenericType(t));

            _availableTypes.AddRange(customTypes);

            // Dropdown labels = generic argument name (pretty print)
            _typeNames = _availableTypes
                .Select(t => t.GenericTypeArguments[0].Name)
                .ToArray();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var currentType = property.managedReferenceValue?.GetType();
            int currentIndex = Mathf.Max(0, _availableTypes.IndexOf(currentType));

            // If no type has been assigned yet, default to the first one
            if (property.managedReferenceValue == null && _availableTypes.Count > 0)
            {
                property.managedReferenceValue = Activator.CreateInstance(_availableTypes[0]);
                currentIndex = 0;
            }

            // Dropdown for selecting type
            Rect dropdownRect = new Rect(
                position.x,
                position.y,
                position.width,
                EditorGUIUtility.singleLineHeight
            );

            int newIndex = EditorGUI.Popup(dropdownRect, "Type", currentIndex, _typeNames);

            if (newIndex != currentIndex)
            {
                var newType = _availableTypes[newIndex];
                property.managedReferenceValue = Activator.CreateInstance(newType);
            }

            // Draw the "Value" field if we have a valid type
            if (property.managedReferenceValue != null)
            {
                var valueProp = property.FindPropertyRelative("Value");
                if (valueProp != null)
                {
                    Rect fieldRect = new Rect(
                        position.x,
                        position.y + EditorGUIUtility.singleLineHeight + 2,
                        position.width,
                        EditorGUI.GetPropertyHeight(valueProp, true)
                    );

                    EditorGUI.PropertyField(fieldRect, valueProp, new GUIContent("Value"), true);
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight + 4;

            if (property.managedReferenceValue != null)
            {
                var valueProp = property.FindPropertyRelative("Value");
                if (valueProp != null)
                    height += EditorGUI.GetPropertyHeight(valueProp, true) + 2;
            }

            return height;
        }
    }
}