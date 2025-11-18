using System;

[ChoiceDataType("Default"), Serializable]
public class DefaultChoiceData
{
    public bool hasTimeLimit = false;
    public float timeLimitDuration = 5;
}