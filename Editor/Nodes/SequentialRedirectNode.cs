using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    [Serializable]
    [UseWithGraph(typeof(DialogueGraph))]
    public class SequentialRedirectNode : RedirectNode
    {
        public override bool UseWeight => false;

        public override ScriptableObject CreateDialogueObject()
        {
            _asset = ScriptableObject.CreateInstance<SequentialRedirectObject>();
            _asset.name = "SequentialRedirect";
            return _asset;
        }
    }
}