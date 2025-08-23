using Unity.GraphToolkit.Editor;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Interface for nodes that are to log warning/errors based on certain conditions
    /// </summary>
    public interface IErrorNode
    {
        /// <summary>
        /// Method that is used to check and log warning/errors
        /// </summary>
        /// <param name="infos"></param>
        void DisplayErrors(GraphLogger infos);
    }
}