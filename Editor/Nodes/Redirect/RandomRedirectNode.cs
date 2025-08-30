using System;
using UnityEngine;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <summary>
    /// Type of <see cref="RedirectNode"/> which evaluates all conditions and randomizes output by assigned weight.
    /// </summary>
    [Serializable]
    public sealed class RandomRedirectNode : RedirectNode
    {
        public override bool UsesWeight => true;

        public override Redirect CreateRedirectObject()
        {
            var redirect = ScriptableObject.CreateInstance<RandomRedirect>();
            redirect.name = "Random Redirect Dialogue";
            
            return redirect;
        }
    }
    
}