using System.Collections.Generic;
using DialogueSystem.Runtime;
using UnityEngine;
using Unity.GraphToolkit.Editor;


namespace DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Interface for nodes that are responsible for creating a scriptableObject instance
    /// </summary>
    public interface IDialogueObjectNode
    {
        /// <summary>
        /// Method that create and returns the node's scriptableObject
        /// </summary>
        ScriptableObject CreateDialogueObject();
    }

}
