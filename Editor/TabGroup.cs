using System;

[Serializable]
public abstract class TabGroup {}


[Serializable]
public class TabGroup<T1, T2> : TabGroup
{
    public T1 first;
    public T2 second;
}


[Serializable]
public class TabGroup<T1, T2, T3> : TabGroup
{
    public T1 first;
    public T2 second;
    public T3 third;
}