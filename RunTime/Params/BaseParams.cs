using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-24</date>
    
    /// <summary>
    /// Base class representing the Base parameters shared by choice and regular dialogue
    /// </summary>
    [Serializable]
    public abstract class BaseParams : TextParams
    {
        public BaseParams() {}
        
        protected BaseParams(BaseParams other) : base(other) {}
        
        public abstract BaseParams Clone();
    }
}