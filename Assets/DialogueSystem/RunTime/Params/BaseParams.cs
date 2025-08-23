using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Base class representing the Base parameters shared by choice and regular dialogue
    /// </summary>
    [Serializable]
    public abstract class BaseParams
    {
        public string text;
        
        public string Text { get => text; set => text = value; }

        public abstract BaseParams Clone();
    }
}