using WolverineSoft.DialogueSystem.Values;

namespace WolverineSoft.DialogueSystem.Editor
{
    using UnityEditor;
    using UnityEngine;

    public class DialogueSOPostprocessor : AssetPostprocessor
    {
        // Load textures from Resources
        private static Texture2D valueSOIcon => Resources.Load<Texture2D>("DialogueValueTexture");
        private static Texture2D valueHolderIcon => Resources.Load<Texture2D>("DialogueGraphTexture");
        private static Texture2D dsEventIcon => Resources.Load<Texture2D>("DialogueGraphTexture");

        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            // Process imported assets
            foreach (string path in importedAssets)
            {
                AssignIcons(path);
            }

            // Optionally, process moved assets too
            foreach (string path in movedAssets)
            {
                AssignIcons(path);
            }
        }

        private static void AssignIcons(string assetPath)
        {
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (obj == null) return;

            // Assign icon for ValueSO
            if (obj is ValueSO && valueSOIcon != null)
            {
                EditorGUIUtility.SetIconForObject(obj, valueSOIcon);
                EditorUtility.SetDirty(obj);
            }

            // Assign icon for ValueHolder
            if (obj is ValueHolder && valueHolderIcon != null)
            {
                EditorGUIUtility.SetIconForObject(obj, valueHolderIcon);
                EditorUtility.SetDirty(obj);
            }

            // Assign icon for DSEventObject
            if (obj is DSEventObject && dsEventIcon != null)
            {
                EditorGUIUtility.SetIconForObject(obj, dsEventIcon);
                EditorUtility.SetDirty(obj);
            }
        }
    }
}