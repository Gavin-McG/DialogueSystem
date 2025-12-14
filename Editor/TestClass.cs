using System;
using UnityEngine;
using WolverineSoft.DialogueSystem;

[TabName("Test")]
public abstract class TestClass : ICustomNodeStyle
{
    public Color BackgroundColor => new Color(0.3f, 0.2f, 0.2f);
    public Color BorderColor => new Color(0.6f, 0.4f, 0.4f);
}

[Serializable, TypeOption("Child 1", 0)]
public class Child1 : TestClass
{
    public string value1;
    public float value2;
}

[Serializable, TypeOption("Child 2", -1)]
public class Child2 : TestClass
{
    public int value1;
    [Range(0, 100)] public int value2;
}