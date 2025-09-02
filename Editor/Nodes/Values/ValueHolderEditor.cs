using WolverineSoft.DialogueSystem.Values;
using UnityEngine;
using System.Collections.Generic;

namespace WolverineSoft.DialogueSystem.Editor.Values
{
    using UnityEditor;

    /// <summary>
    /// Editor for ValueHolder. Creates a button to populate values with all <see cref="DSValue"/> currently in the project
    /// </summary>
    [CustomEditor(typeof(DSValueHolder))]
    public class ValueHolderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DSValueHolder holder = (DSValueHolder)target;

            if (GUILayout.Button("Populate with all ValueSO assets"))
            {
                string[] guids = AssetDatabase.FindAssets("t:DSValue");
                List<DSValue> allValues = new();

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    DSValue asset = AssetDatabase.LoadAssetAtPath<DSValue>(path);
                    if (asset != null)
                        allValues.Add(asset);
                }

                Undo.RecordObject(holder, "Populate ValueHolder");
                holder.SetValues(allValues);
                EditorUtility.SetDirty(holder);

                holder.OnValidate();
            }
        }
    }
}