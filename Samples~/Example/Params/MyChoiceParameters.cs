using System;
using UnityEngine;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    [Serializable, TypeOption("Sample")]
    public class MyChoiceParameters : ChoiceParameters
    {
        [SerializeField] public bool isTimed;
        [SerializeField, Min(0)] public float timeLimit;
    }
}