using UnityEditor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Post-processor for dialogue assets which is used to assign the proper icons
    /// </summary>
    [InitializeOnLoad]
    public static class DialogueSOIconInitializer
    {
        private static Texture2D valueSOIcon => Resources.Load<Texture2D>("DialogueValueTexture");
        private static Texture2D valueHolderIcon => Resources.Load<Texture2D>("DialogueValueHolderTexture");
        private static Texture2D dsEventIcon => Resources.Load<Texture2D>("DialogueEventTexture");

        static DialogueSOIconInitializer()
        {
            // Runs when the editor loads
            EditorApplication.projectChanged += AssignAllIcons;
            AssignAllIcons();
        }

        private static void AssignAllIcons()
        {
            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);
                if (obj == null) continue;

                if (obj is DSEventBase && dsEventIcon != null)
                    EditorGUIUtility.SetIconForObject(obj, dsEventIcon);
            }
        }
    }
}