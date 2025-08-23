using System;
using Unity.GraphToolkit.Editor;
using UnityEditor.Experimental.GraphView;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-23</date>
    
    /// <summary>
    /// Base class for blockNodes which require access to contextNode on initialization.
    /// contextNode is node available on the first definition, which is a bug with the Graph Toolkit.
    /// A bug report for this has already been submitted
    /// Prompts user for a bool option change which forces an option/port redefinition.
    /// </summary>
    /// <remarks>
    /// If the issue of contextNode is fixed in a later version then this class can be removed
    /// </remarks>
    public abstract class InitializeNode : BlockNode
    {
        protected sealed override void OnDefineOptions(IOptionDefinitionContext context)
        {
            if (contextNode == null)
                DialogueGraphUtility.AddNodeOption(context, "button", typeof(bool), displayName: "PRESS -->");
            else
                DefineFullOptions(context);
        }

        protected virtual void DefineFullOptions(IOptionDefinitionContext context) {}
    }
}