using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(TabGroup), true)]
public class TabGroupDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        var root = new VisualElement();
        root.style.width = 300;
        root.style.alignSelf = new StyleEnum<Align>(Align.Center);
        var obj = property.boxedValue;

        if (obj == null)
        {
            root.Add(new Label("Null TabGroup (create an object)"));
            return root;
        }

        Type type = obj.GetType();

        // Create the TabView
        var tabView = new TabView();
        root.Add(tabView);

        // Get all instance fields of the TabGroup<T...>
        FieldInfo[] fields = GetTabFields(type);

        foreach (var field in fields)
        {
            // Name for the tab
            string tabName = GetTabName(field.FieldType);

            // Create tab
            var tab = new Tab( tabName );
            tabView.Add(tab);

            // Visual container inside the tab
            var container = new VisualElement();
            container.style.marginLeft = 4;
            container.style.marginTop = 4;

            tab.contentContainer.Add(container);

            // Create a property field for the Serializable field
            SerializedProperty childProp = property.FindPropertyRelative(field.Name);

            if (childProp != null)
            {
                var pf = new PropertyField(childProp);
                pf.Bind(property.serializedObject);
                container.Add(pf);
            }
            else
            {
                container.Add(new Label($"Field '{field.Name}' not found"));
            }
        }

        return root;
    }
    
    private static FieldInfo[] GetTabFields(Type type) =>
        type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(f => !f.IsStatic && f.DeclaringType == type)
            .ToArray();


    private string GetTabName(Type t)
    {
        var attr = t.GetCustomAttribute<TabNameAttribute>();
        return attr != null ? attr.tabName : t.Name;
    }
}