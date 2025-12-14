
namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Interface for nodes that provide data from a port.
    /// </summary>
    /// <typeparam name="T">Type that the node will provide</typeparam>
    public interface IDataNode<out T>
    {
        /// <summary>
        /// method that is used to retrieve the data that the node provides
        /// </summary>
        public T GetData();
    }
}