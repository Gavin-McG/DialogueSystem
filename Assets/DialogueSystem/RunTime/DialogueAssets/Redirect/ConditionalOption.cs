using UnityEngine;

namespace DialogueSystem.Runtime
{
    /// <author>Gavin McGinness</author>
    /// <date>2025-08-21</date>
    
    /// <summary>
    /// abstract class representing a conditionalOption for a <see cref="Redirect"/>
    /// </summary>
    public abstract class ConditionalOption : Option
    {
        //weight parameter used for randomized redirects
        [HideInDialogueGraph] public float weight = 1;
    }

}
