using System.Collections.Generic;
using WolverineSoft.DialogueSystem;
using UnityEngine;
using Unity.GraphToolkit.Editor;


namespace WolverineSoft.DialogueSystem.Editor
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
