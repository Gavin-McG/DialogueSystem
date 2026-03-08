using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using WolverineSoft.DialogueSystem;
using WolverineSoft.DialogueSystem.Editor;

namespace WolverineSoft.DialogueSystem.Editor
{
    [CustomPropertyDrawer(typeof(GenericValueHolder), true)]
    public class GenericValueHolderDrawer : PropertyDrawer
    {
        private string GetTypeDisplayName(Type t)
        {
            var displayNameAttribute = t.GetAttributes<TypeOptionAttribute>().FirstOrDefault();
            string displayName = displayNameAttribute?.displayName ?? t.Name;
            
            return displayName;
        }

        private string GetTabDisplayName(Type t)
        {
            var displayNameAttribute = t.GetAttributes<TabNameAttribute>().FirstOrDefault();
            string displayName = displayNameAttribute?.tabName ?? t.Name;
            
            return displayName;
        }
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            
            // Get the template variable T for the ValueHolder
            Type holderType = fieldInfo.FieldType;
            
            // Get all field of ValueHolder
            var fields = holderType
                .GetFields(System.Reflection.BindingFlags.Instance |
                           System.Reflection.BindingFlags.Public |
                           System.Reflection.BindingFlags.NonPublic)
                .ToList();

            // Add a tab for every field
            List<(Type, VisualElement)> tabs = new();
            foreach (var field in fields)
            {
                var fieldProperty = property.FindPropertyRelative(field.Name);
                if (fieldProperty == null)
                    continue;

                var type = field.FieldType;
                var newTab = GetFieldTab(type, fieldProperty);
                if (newTab != null) tabs.Add((type, newTab));
            }

            if (tabs.Count > 1)
            {
                var tabView = new TabView();
                root.Add(tabView);
                
                //Add tabs to tab view
                foreach (var tab in tabs)
                {
                    var tabContainer = new Tab(GetTabDisplayName(tab.Item1));
                    tabContainer.Add(tab.Item2);
                    tabView.Add(tabContainer);
                }
            }
            else
            {
                foreach (var tab in tabs)
                    root.Add(tab.Item2);
            }
            
            return root;
        }

        private VisualElement GetFieldTab(Type type, SerializedProperty valueProperty)
        {
            // Initialize lists of derived Types, sorted by Attribute order
            var types = TypeCache.GetTypesDerivedFrom(type).ToList();
            if (types.Count == 0) return null;
            
            types.Sort((t1, t2) =>
            {
                int order1 = t1.GetAttributes<TypeOptionAttribute>().FirstOrDefault()?.order ?? 0;
                int order2 = t2.GetAttributes<TypeOptionAttribute>().FirstOrDefault()?.order ?? 0;
                return order1.CompareTo(order2);
            });
            var typeNames = types.Select(type => GetTypeDisplayName(type)).ToList();
            
            // Assign the current value if it is null and type is available
            var currentValue = valueProperty.managedReferenceValue;
            if (currentValue == null && types.Count != 0)
            {
                currentValue = Activator.CreateInstance(types.First());
                valueProperty.managedReferenceValue = currentValue;
                valueProperty.serializedObject.ApplyModifiedProperties();
            }
            
            // Evaluate current type
            Type currentType = currentValue?.GetType();
            int currentIndex = types.IndexOf(currentType);
            
            // Create root & type inspector
            var tab = new VisualElement();
            var inspectorContainer = new VisualElement();

            //Create type dropdown if multiple available types exist
            if (types.Count > 1)
            {
                var typeDropdown = new DropdownField(typeNames, currentIndex);
                
                // Callback for Dropdown
                typeDropdown.RegisterValueChangedCallback(evt =>
                {
                    // Get the selected type
                    int index = typeNames.IndexOf(evt.newValue);
                    Type selectedType = types[index];
                
                    // Assign the value to an instance of the selected type
                    valueProperty.managedReferenceValue = Activator.CreateInstance(selectedType);
                    valueProperty.serializedObject.ApplyModifiedProperties();
                
                    AddValueInspector(valueProperty, inspectorContainer);
                });
                
                tab.Add(typeDropdown);
            }
            
            //Add inspector container after dropdown
            tab.Add(inspectorContainer);
            
            //Set Root styling based on interface
            tab.style.width = 275;
            if (valueProperty.managedReferenceValue is ICustomNodeStyle nodeStyle)
            {
                tab.style.backgroundColor = nodeStyle.BackgroundColor;
                SetElementPadding(tab, 1f);
                SetElementBorder(tab, 0f, nodeStyle.BorderColor, 0f);
            }
            
            AddValueInspector(valueProperty, inspectorContainer);
            
            return tab;
        }

        private void AddValueInspector(SerializedProperty property, VisualElement parent)
        {
            parent.Clear();

            // Add the PropertyField for the selected type
            var propField = new PropertyField(property, "Parameters");
            propField.Bind(property.serializedObject);
            parent.Add(propField);
        }

        private static void SetElementPadding(VisualElement element, float padding)
        {
            element.style.paddingTop = padding;
            element.style.paddingBottom = padding;
            element.style.paddingLeft = padding;
            element.style.paddingRight = padding;
        }

        private static void SetElementBorder(VisualElement element, float thickness, Color color, float radius = 0)
        {
            //Set thicknesses
            element.style.borderTopWidth = thickness;
            element.style.borderBottomWidth = thickness;
            element.style.borderLeftWidth = thickness;
            element.style.borderRightWidth = thickness;
            
            //Set colors
            element.style.borderTopColor = color;
            element.style.borderBottomColor = color;
            element.style.borderLeftColor = color;
            element.style.borderRightColor = color;
            
            //Set radius
            element.style.borderTopLeftRadius = radius;
            element.style.borderTopRightRadius = radius;
            element.style.borderBottomLeftRadius = radius;
            element.style.borderBottomRightRadius = radius;
        }
    }
}