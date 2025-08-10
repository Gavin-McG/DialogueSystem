using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem.ExampleInterface
{
    public class DialogueUIManager : MonoBehaviour
    {

        [SerializeField] private DialogueManager dialogueManager;

        [Header("UI References")] [SerializeField]
        private GameObject dialogueUI;
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private ChoiceUIManager choiceUIManager;

        private bool dialogueEnabled = false;
        private Coroutine timeLimitCoroutine;
        private float timeStarted;

        private void OnEnable()
        {
            dialogueManager.displayDialogue.AddListener(DisplayDialogue);
            dialogueManager.endDialogue.AddListener(HideDialogue);
            
            choiceUIManager.AddChoiceListener(ChoicePressed);
            choiceUIManager.AddContinueListener(ContinuePressed);
        }

        private void OnDisable()
        {
            dialogueManager.displayDialogue.RemoveListener(DisplayDialogue);
            dialogueManager.endDialogue.RemoveListener(HideDialogue);
            
            choiceUIManager.RemoveChoiceListener(ChoicePressed);
            choiceUIManager.RemoveContinueListener(ContinuePressed);
        }

        private void DisplayDialogue(DialogueParams dialogueParams)
        {
            dialogueEnabled = true;
            dialogueUI.SetActive(true);

            mainText.text = dialogueParams.baseParams.text;
            timeStarted = Time.time;
            EndTimeLimit();

            switch (dialogueParams.dialogueType)
            {
                case DialogueParams.DialogueType.Basic: DisplayBasicDialogue(dialogueParams); break;
                case DialogueParams.DialogueType.Choice: DisplayChoiceDialogue(dialogueParams); break;
                default: break;
            }
        }

        private void DisplayBasicDialogue(DialogueParams dialogueParams)
        {
            choiceUIManager.SetBasicDialogue(dialogueParams);
        }

        private void DisplayChoiceDialogue(DialogueParams dialogueParams)
        {
            choiceUIManager.SetChoiceDialogue(dialogueParams);
            if (dialogueParams.choiceParams.hasTimeLimit)
            {
                timeLimitCoroutine = StartCoroutine(TimeLimitRoutine(dialogueParams.choiceParams.timeLimitDuration));
            }
        }

        private void ChoicePressed(int index)
        {
            dialogueManager.advanceDialogue.Invoke(new AdvanceDialogueContext()
            {
                choice = index,
                inputDelay = Time.time - timeStarted,
                timedOut = false
            });
        }

        private void ContinuePressed()
        {
            dialogueManager.advanceDialogue.Invoke(new AdvanceDialogueContext()
            {
                choice = -1,
                inputDelay = Time.time - timeStarted,
                timedOut = false
            });
        }

        private void HideDialogue()
        {
            dialogueEnabled = false;
            dialogueUI.SetActive(false);
            EndTimeLimit();
        }

        private void UpdateTimeLimit(float t)
        {
            Debug.Log(t.ToString() + " Completed");
        }

        IEnumerator TimeLimitRoutine(float timeLimitDuration)
        {
            float time = 0;
            while (time < timeLimitDuration)
            {
                float t = time / timeLimitDuration;

                UpdateTimeLimit(t);

                time += Time.deltaTime;
                yield return null;
            }

            dialogueManager.advanceDialogue.Invoke(new AdvanceDialogueContext()
            {
                choice = 0,
                inputDelay = timeLimitDuration,
                timedOut = true,
            });
        }

        private void EndTimeLimit()
        {
            if (timeLimitCoroutine != null)
            {
                StopCoroutine(timeLimitCoroutine);
                timeLimitCoroutine = null;
            }
        }
    }
}
