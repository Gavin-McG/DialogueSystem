using System.Collections.Generic;
using DialogueSystem.Runtime;
using UnityEngine;
using Unity.GraphToolkit.Editor;


namespace DialogueSystem.Editor
{

    public interface IDialogueObjectNode
    {
        public DialogueObject CreateDialogueObject();

    }

}
