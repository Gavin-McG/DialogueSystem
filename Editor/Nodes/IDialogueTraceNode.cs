using Unity.GraphToolkit.Editor;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Interface for <see cref="IDialogueReferenceNode"/> nodes that are of the dialogue path that a manager is to trace
    /// Also posseses a default Information for <see cref="IErrorNode"/>
    /// </summary>
    public interface IDialogueTraceNode : IDialogueReferenceNode, IErrorNode
    {
        void IErrorNode.DisplayErrors(GraphLogger infos)
        {
            DialogueGraphUtility.MultipleOutputsCheck((INode)this, infos);
            DialogueGraphUtility.CheckPreviousConnection((INode)this, infos);
        }
    }
}