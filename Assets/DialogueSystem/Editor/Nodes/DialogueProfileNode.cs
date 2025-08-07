using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using DialogueSystem.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace DialogueSystem.Editor
{
    [Serializable]
    public class DialogueProfileNode : GenericObjectNode<DialogueProfile>
    {
        public override DialogueObject CreateDialogueObject()
        {
            var obj = base.CreateDialogueObject();
            
            if (obj is DialogueProfile profile)
                profile.name = profile.displayName + " Profile";
            
            return obj;
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            DialogueGraphUtility.DefineProfileOutputPort(context);
        }
    }
    
}