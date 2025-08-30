
namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Interface for nodes that provide data from an input Port
    /// </summary>
    /// <typeparam name="T">Type that the node will provide</typeparam>
    public interface IInputDataNode<out T>
    {
        /// <summary>
        /// method that is used to retrieve the data that the IInputDataNode provides
        /// </summary>
        public T GetInputData();
    }
}