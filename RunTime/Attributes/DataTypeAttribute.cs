using System;
using UnityEngine;

public abstract class DataTypeAttribute : Attribute
{
    public string displayName;
    protected DataTypeAttribute(string displayName)
    {
        this.displayName = displayName;
    }
}