
namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Interface for nodes that provide data from an output port.
    /// </summary>
    /// <typeparam name="T">Type that the node will provide</typeparam>
    public interface IOutputDataNode<out T>
    {
        /// <summary>
        /// method that is used to retrieve the data that the IInputDataNode provides
        /// </summary>
        public T GetOutputData();
    }
}