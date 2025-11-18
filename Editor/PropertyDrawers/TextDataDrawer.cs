using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using WolverineSoft.DialogueSystem;
using WolverineSoft.DialogueSystem.Editor;

[CustomPropertyDrawer(typeof(TextData))]
public class TextDataDrawer : DataTypeDrawer<TextDataTypeAttribute>
{
    protected override string InfoName => "Dialogue";
    
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement root = new VisualElement();
        root.style.width = 300;
        
        var textProp = property.FindPropertyRelative("text");
        var textField = new TextField();
        textField.bindingPath = textProp.propertyPath;
        textField.multiline = true;
        textField.style.height = 100;
        textField.verticalScrollerVisibility = ScrollerVisibility.Auto;
        textField.style.whiteSpace = WhiteSpace.Normal;
        root.Add(textField);
        
        var dataProp = property.FindPropertyRelative("data");
        var inspectorContainer = new VisualElement();
        inspectorContainer.style.backgroundColor = Color.black * 0.3f;
        
        AddTypeDropdown(root, dataProp, () =>
        {
            AddTypeInspector(inspectorContainer, dataProp);
        });

        AddTypeInspector(inspectorContainer, dataProp);

        root.Add(inspectorContainer);
        return root;
    }
}