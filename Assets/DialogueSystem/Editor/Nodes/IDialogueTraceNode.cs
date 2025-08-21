using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Editor
{
    public interface IDialogueTraceNode : IDialogueReferenceNode, IErrorNode
    {
        void IErrorNode.DisplayErrors(GraphLogger infos)
        {
            DialogueGraphUtility.MultipleOutputsCheck((INode)this, infos);
            DialogueGraphUtility.CheckPreviousConnection((INode)this, infos);
        }
    }
}