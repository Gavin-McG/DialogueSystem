using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public abstract class DataTypeDrawer<TAttr> : PropertyDrawer
    where TAttr : DataTypeAttribute
{
    protected List<Type> types;
    protected List<string> typeNames;

    private int currentIndex = 0;
    
    protected abstract string InfoName { get; }
    
    public override abstract VisualElement CreatePropertyGUI(SerializedProperty property);


    // Load all valid types with the attribute
    protected void LoadTypes()
    {
        types = TypeCache.GetTypesWithAttribute<TAttr>()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .ToList();

        typeNames = types
            .Select(t => t.GetCustomAttribute<TAttr>().displayName)
            .ToList();
    }


    // Add dropdown for selecting the type of the SerializedReference
    protected DropdownField AddTypeDropdown(VisualElement parent, SerializedProperty serializedReferenceProp, Action onTypeChanged = null)
    {
        LoadTypes();

        currentIndex = 0;

        if (serializedReferenceProp.managedReferenceValue != null)
        {
            // Get the index of the current Data type if there is a current data instance
            var currentType = serializedReferenceProp.managedReferenceValue.GetType();
            currentIndex = types.IndexOf(currentType);
            if (currentIndex < 0) currentIndex = 0;
        }
        else if (types.Count > 0)
        {
            // Create a new instance from index 0 if current data is none
            serializedReferenceProp.managedReferenceValue = Activator.CreateInstance(types[currentIndex]);
            serializedReferenceProp.serializedObject.ApplyModifiedProperties();
        }

        // Add the dropdown field
        var dropdown = new DropdownField(InfoName+" Type", typeNames.ToList(), currentIndex);
        dropdown.tooltip = types[currentIndex].FullName;
        parent.Add(dropdown);

        dropdown.RegisterValueChangedCallback(evt =>
        {
            int index = typeNames.IndexOf(evt.newValue);
            if (index < 0) return;
            currentIndex = index;

            // Instantiate and assign
            serializedReferenceProp.managedReferenceValue = Activator.CreateInstance(types[index]);
            serializedReferenceProp.serializedObject.ApplyModifiedProperties();

            // Let inheriting drawer update UI
            onTypeChanged?.Invoke();
        });

        return dropdown;
    }


    // Add inspector for the selected managed reference object
    protected void AddTypeInspector(VisualElement parent, SerializedProperty serializedReferenceProp)
    {
        parent.Clear();

        if (serializedReferenceProp.managedReferenceValue == null)
            return;

        var field = new PropertyField(serializedReferenceProp);
        field.label = typeNames[currentIndex] + " Parameters";
        field.Bind(serializedReferenceProp.serializedObject);
        parent.Add(field);
    }
}
