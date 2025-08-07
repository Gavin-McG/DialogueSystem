using System;
using DialogueSystem.Runtime;
using UnityEngine;

namespace DialogueSystem.Editor
{
    
    [Serializable]
    public class RandomRedirectNode : RedirectNode
    {
        public override bool UsesWeight => true;

        public override DialogueObject CreateDialogueObject()
        {
            var redirect = ScriptableObject.CreateInstance<RandomRedirect>();
            redirect.name = "Random Redirect Dialogue";
            
            return redirect;
        }
    }
    
}