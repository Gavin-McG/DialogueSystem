using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    [Serializable]
    public class TextHolder
    {
        [SerializeField] public string text;
    }

    [Serializable, TextHolder(80, 270)]
    public class TextTextHolder : TextHolder { }
    [Serializable, TextHolder(80, 300)]
    public class ChoiceTextHolder : TextHolder { }
    [Serializable, TextHolder(35, 270)]
    public class OptionTextHolder : TextHolder { }

}