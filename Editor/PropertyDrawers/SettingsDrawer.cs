using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using WolverineSoft.DialogueSystem;
using WolverineSoft.DialogueSystem.Editor;

[CustomPropertyDrawer(typeof(SettingsData))]
public class SettingsDrawer : DataTypeDrawer<DialogueSettingsTypeAttribute>
{
    protected override string InfoName => "Settings";
    
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement root = new VisualElement();
        root.style.width = 300;
        
        var textProp = property.FindPropertyRelative("description");
        var textField = new TextField();
        textField.bindingPath = textProp.propertyPath;
        textField.multiline = true;
        textField.style.height = 60;
        textField.verticalScrollerVisibility = ScrollerVisibility.Auto;
        root.Add(textField);
        
                
        var dataProp = property.FindPropertyRelative("data");
        var inspectorContainer = new VisualElement();
        inspectorContainer.style.backgroundColor = Color.black * 0.3f;

        LoadTypes();

        AddTypeDropdown(root, dataProp, () =>
        {
            AddTypeInspector(inspectorContainer, dataProp);
        });

        AddTypeInspector(inspectorContainer, dataProp);

        root.Add(inspectorContainer);
        return root;
    }
}