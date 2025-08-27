using System;
using UnityEngine;
using WolverineSoft.DialogueSystem.Values;

[DialogueValueType]
[Serializable]
public class Vector2Wrapper
{
    [SerializeField] public float x, y;

    public override string ToString()
    {
        return $"({x}, {y})";
    }
}