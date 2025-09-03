using System;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    [Serializable]
    public class DefaultBaseParams : BaseParams
    {
        /*
        PUT PARAMETER FIELDS HERE
        */
        [DialoguePort] public DialogueProfile profile;
    }
}