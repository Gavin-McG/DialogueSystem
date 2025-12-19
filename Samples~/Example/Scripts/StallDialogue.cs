using System.Collections;
using UnityEngine;
using WolverineSoft.DialogueSystem.Default;

namespace WolverineSoft.DialogueSystem.Example
{
    /// <summary>
    /// Receive Stall Events and Resume Dialogue After waiting
    /// </summary>
    public class StallDialogue : MonoBehaviour
    {
        [SerializeField] private DSEventFloat stallEvent;
        [SerializeField] private DialogueUIManager dialogue;

        private void OnEnable()
        {
            stallEvent.AddListener(Stall);
        }

        private void OnDisable()
        {
            stallEvent.RemoveListener(Stall);
        }

        private void Stall(float time)
        {
            StartCoroutine(StallRoutine(time));
        }

        IEnumerator StallRoutine(float time)
        {
            yield return new WaitForSeconds(time);
            
            dialogue.AdvanceDialogue(new AdvanceContext(-1));
        }
    }
}

