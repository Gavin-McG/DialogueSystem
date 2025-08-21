using Unity.GraphToolkit.Editor;

namespace DialogueSystem.Editor
{
    public interface IErrorNode
    {
        public void DisplayErrors(GraphLogger infos);
    }
}