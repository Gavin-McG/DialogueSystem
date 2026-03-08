using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Editor {
    
    [CustomPropertyDrawer(typeof(Variable))]
    public class VariableDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            Foldout root = new Foldout();
            root.text = property.displayName;
            root.value = true;

            root.style.paddingLeft = 4;

            var typeProp = property.FindPropertyRelative("type");
            var typeField = new PropertyField(typeProp);
            root.Add(typeField);

            var valueContainer = new VisualElement();
            root.Add(valueContainer);

            void RefreshValueField()
            {
                valueContainer.Clear();

                var typeValue = (VariableType)typeProp.enumValueIndex;

                SerializedProperty valueProp = typeValue switch
                {
                    VariableType.String => property.FindPropertyRelative("_stringValue"),
                    VariableType.Float  => property.FindPropertyRelative("_floatValue"),
                    VariableType.Int    => property.FindPropertyRelative("_intValue"),
                    VariableType.Bool   => property.FindPropertyRelative("_boolValue"),
                    _ => null
                };

                if (valueProp == null) return;
                
                var valueField = new PropertyField(valueProp);
                valueField.BindProperty(property.serializedObject);
                
                valueContainer.Add(valueField);
            }

            // Initial draw
            RefreshValueField();

            // React to enum changes
            typeField.RegisterValueChangeCallback(_ =>
            {
                property.serializedObject.Update();
                RefreshValueField();
            });

            return root;
        }
    }

}