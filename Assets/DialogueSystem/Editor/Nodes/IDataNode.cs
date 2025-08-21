using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Interface for nodes that provide data to an attached IDialogueTraceNode.
    /// </summary>
    /// <typeparam name="T">Type that the node will provide</typeparam>
    public interface IDataNode<out T>
    {
        /// <summary>
        /// method that is used to retrieve the data that the IDataNode provides
        /// </summary>
        public T GetData(Dictionary<IDialogueObjectNode, ScriptableObject> dialogueDict);
    }
}