using UnityEngine;

namespace DialogueSystem.Runtime
{

    public abstract class ConditionalOption : Option
    {
        [HideInDialogueGraph] public float weight = 1;
    }

}
