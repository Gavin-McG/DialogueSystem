using System;
using System.Collections.Generic;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class representing the parameters used by choice options
    /// </summary>
    [Serializable, TabName("Response")]
    public abstract class ResponseParameters : ParameterBase, ICustomNodeStyle
    {
        public Color BackgroundColor => new Color(0.2f,0.2f,0.4f);
        public Color BorderColor => new Color(0.3f,0.3f,0.4f);    
    }
}