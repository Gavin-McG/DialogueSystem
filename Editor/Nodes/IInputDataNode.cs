using System.Collections.Generic;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
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