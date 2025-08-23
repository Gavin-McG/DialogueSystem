using System;
using UnityEngine;
using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// Type of <see cref="RedirectNode"/> which evaluates all conditions and randomizes output by assigned weight.
    /// </summary>
    [Serializable]
    public sealed class RandomRedirectNode : RedirectNode
    {
        public override bool UsesWeight => true;

        public override ScriptableObject CreateDialogueObject()
        {
            var redirect = ScriptableObject.CreateInstance<RandomRedirect>();
            redirect.name = "Random Redirect Dialogue";
            
            return redirect;
        }
    }
    
}