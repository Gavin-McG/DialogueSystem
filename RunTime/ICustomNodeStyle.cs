using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem
{
    public interface ICustomNodeStyle
    {
        public Color BackgroundColor { get; }
        public Color BorderColor { get; }
    }
}