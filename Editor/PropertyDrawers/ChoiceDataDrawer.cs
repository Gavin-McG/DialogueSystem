using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using WolverineSoft.DialogueSystem;
using WolverineSoft.DialogueSystem.Editor;

[CustomPropertyDrawer(typeof(ChoiceData))]
public class ChoiceDataDrawer : DataTypeDrawer<ChoiceDataTypeAttribute>
{
    protected override string InfoName => "Choice";
    
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement root = new VisualElement();
        root.style.width = 300;
        
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