using System;
using UnityEngine;
using WolverineSoft.DialogueSystem;

[Serializable, TabName("Option")]
public class OptionData
{
    [SerializeReference] public OptionType data;
}
