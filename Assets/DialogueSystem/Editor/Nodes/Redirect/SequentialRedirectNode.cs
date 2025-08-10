using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace  DialogueSystem.Editor
{
    
    [Serializable]
    public class SequentialRedirectNode : RedirectNode
    {
        public override bool UsesWeight => false;
        
        public override DialogueObject CreateDialogueObject()
        {
            var redirect = ScriptableObject.CreateInstance<SequentialRedirect>();
            redirect.name = "Sequential Redirect Dialogue";
            
            return redirect;
        }
    }
    
}


