using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    [Serializable]
    [UseWithGraph(typeof(DialogueGraph))]
    public class RandomRedirectNode : RedirectNode
    {
        public override bool UseWeight => true;

        public override ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<RandomRedirectObject>();
            _asset.name = "RandomRedirect";
            return _asset;
        }
    }
}