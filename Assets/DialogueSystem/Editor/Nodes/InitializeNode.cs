using System;
using Unity.GraphToolkit.Editor;
using UnityEditor.Experimental.GraphView;

namespace WolverineSoft.DialogueSystem.Editor
{
    [Serializable, UseWithContext(typeof(RandomRedirectNode), typeof(SequentialRedirectNode))]
    public abstract class InitializeNode : BlockNode
    {
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            if (contextNode == null)
                DialogueGraphUtility.AddNodeOption(context, "button", typeof(bool), displayName: "PRESS -->");
            else
                DefineFullOptions(context);
        }

        protected abstract void DefineFullOptions(IOptionDefinitionContext context);
    }
}