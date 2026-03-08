using System;

namespace WolverineSoft.DialogueSystem.Example
{
    [Serializable, TypeOption("Sample")]
    public class MyTextParameters : TextParameters
    {
        public DialogueProfile profile;
    }
}