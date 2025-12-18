using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    /// <summary>
    /// Base class for Parameter types which require value replacements
    /// </summary>
    [Serializable, TabName("Text")]
    public abstract class TextParameters : ParameterBase, ICustomNodeStyle
    {
        public Color BackgroundColor => new Color(0.3f,0.2f,0.2f);
        public Color BorderColor => new Color(0.4f,0.3f,0.3f);
    }
}