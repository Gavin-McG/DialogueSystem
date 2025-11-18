using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class representing the parameters used by choice options
    /// </summary>
    [Serializable, TabName("Response")]
    public class ResponseData
    {
        [SerializeReference] public System.Object data;
    }
}