using System;

[AttributeUsage(AttributeTargets.Class)]
public class TabNameAttribute : Attribute
{
    public string tabName;

    public TabNameAttribute(string tabName)
    {
        this.tabName = tabName;
    }
}