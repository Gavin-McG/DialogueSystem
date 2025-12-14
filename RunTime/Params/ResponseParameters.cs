using System;
using System.Collections.Generic;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class representing the parameters used by choice options
    /// </summary>
    [Serializable, TabName("Response")]
    public class ResponseParameters
    {
        [SerializeField] public string text;
        [SerializeReference] public System.Object data;
    }
}