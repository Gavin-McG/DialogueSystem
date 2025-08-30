using System;
using UnityEngine;

namespace  WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Type of <see cref="RedirectNode"/> which evaluates until it finds a condition that passes.
    /// </summary>
    [Serializable]
    public sealed class SequentialRedirectNode : RedirectNode
    {
        public override bool UsesWeight => false;
        
        public override Redirect CreateRedirectObject()
        {
            var redirect = ScriptableObject.CreateInstance<SequentialRedirect>();
            redirect.name = "Sequential Redirect Dialogue";
            
            return redirect;
        }
    }
    
}


