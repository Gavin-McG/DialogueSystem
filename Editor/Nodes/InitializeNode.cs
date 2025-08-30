using Unity.GraphToolkit.Editor;

namespace WolverineSoft.DialogueSystem.Editor
{
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
                OnDefineInitializedOptions(context);
        }

        protected sealed override void OnDefinePorts(IPortDefinitionContext context)
        {
            if (contextNode != null)
                OnDefineInitializedPorts(context);
        }

        protected virtual void OnDefineInitializedOptions(IOptionDefinitionContext context) {}
        
        protected virtual void OnDefineInitializedPorts(IPortDefinitionContext context) {}
    }
}