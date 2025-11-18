using System;
using UnityEngine;

[TextDataType("Default"), Serializable]
public class DefaultTextData
{
    [SerializeField] string CharacterName;
    [SerializeField] Sprite CharacterSprite;
}