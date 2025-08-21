using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace  DialogueSystem.Editor
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>

    /// <summary>
    /// Type of <see cref="RedirectNode"/> which evaluates until it finds a condition that passes.
    /// </summary>
    [Serializable]
    public sealed class SequentialRedirectNode : RedirectNode
    {
        public override bool UsesWeight => false;
        
        public override ScriptableObject CreateDialogueObject()
        {
            var redirect = ScriptableObject.CreateInstance<SequentialRedirect>();
            redirect.name = "Sequential Redirect Dialogue";
            
            return redirect;
        }
    }
    
}


