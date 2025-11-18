using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class representing the parameters used by Choice Dialogue
    /// </summary>
    [Serializable, TabName("Choice")]
    public class ChoiceData
    {
        [SerializeReference] public System.Object data;
    }
}