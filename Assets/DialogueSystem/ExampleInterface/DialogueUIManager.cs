using System.Collections;
using DialogueSystem.Runtime;
using UnityEngine;

namespace DialogueSystem.ExampleInterface
{
    public class DialogueUIManager : MonoBehaviour
    {

        [SerializeField] private DialogueManager dialogueManager;

        [Header("UI References")] [SerializeField]
        private GameObject dialogueUI;
        [SerializeField] private MainTextUI mainTextUI;
        [SerializeField] private ChoiceUIManager choiceUIManager;
        [SerializeField] private TimeLimitUI timeLimitUI;

        private bool dialogueEnabled = false;
        private DialogueParams currentParams;
        private float timeStarted;

        private void OnEnable()
        {
            dialogueManager.displayDialogue.AddListener(DisplayDialogue);
            dialogueManager.endDialogue.AddListener(HideDialogue);
            
            mainTextUI.completedText.AddListener(BeginChoiceTimer);
            
            timeLimitUI.timeLimitExpired.AddListener(TimeExpired);
            
            choiceUIManager.AddChoiceListener(ChoicePressed);
            choiceUIManager.AddContinueListener(ContinuePressed);
        }

        private void OnDisable()
        {
            dialogueManager.displayDialogue.RemoveListener(DisplayDialogue);
            dialogueManager.endDialogue.RemoveListener(HideDialogue);
            
            mainTextUI.completedText.RemoveListener(BeginChoiceTimer);

            timeLimitUI.timeLimitExpired.RemoveListener(TimeExpired);
            
            choiceUIManager.RemoveChoiceListener(ChoicePressed);
            choiceUIManager.RemoveContinueListener(ContinuePressed);
        }

        private void DisplayDialogue(DialogueParams dialogueParams)
        {
            dialogueEnabled = true;
            dialogueUI.SetActive(true);
            currentParams = dialogueParams;

            mainTextUI.SetText(currentParams.baseParams.text);
            timeStarted = Time.time;
            EndTimeLimit();
    
            switch (currentParams.dialogueType)
            {
                case DialogueParams.DialogueType.Basic: DisplayBasicDialogue(); break;
                case DialogueParams.DialogueType.Choice: DisplayChoiceDialogue(); break;
                default: break;
            }
        }

        private void DisplayBasicDialogue()
        {
            choiceUIManager.SetContinueButton(currentParams);
        }

        private void DisplayChoiceDialogue()
        {
            choiceUIManager.SetContinueButton(currentParams);
        }

        private void BeginChoiceTimer()
        {
            if (currentParams.dialogueType == DialogueParams.DialogueType.Choice)
            {
                choiceUIManager.SetChoiceButtons(currentParams);
                if (currentParams.choiceParams.hasTimeLimit)
                {
                    timeLimitUI.StartTimer(currentParams.choiceParams.timeLimitDuration);
                }
            }
        }

        private void ChoicePressed(int index)
        {
            EndTimeLimit();
            dialogueManager.advanceDialogue.Invoke(new AdvanceDialogueContext()
            {
                choice = index,
                inputDelay = Time.time - timeStarted,
                timedOut = false
            });
        }

        private void ContinuePressed()
        {
            if (mainTextUI.textState == MainTextUI.TextState.Scrolling)
            {
                mainTextUI.CompleteText();
            }
            else if (mainTextUI.textState == MainTextUI.TextState.Completed)
            {
                dialogueManager.advanceDialogue.Invoke(new AdvanceDialogueContext()
                {
                    choice = -1,
                    inputDelay = Time.time - timeStarted,
                    timedOut = false
                });
            }
        }

        private void TimeExpired()
        {
            dialogueManager.advanceDialogue.Invoke(new AdvanceDialogueContext()
            {
                choice = 0,
                inputDelay = currentParams.choiceParams.timeLimitDuration,
                timedOut = true,
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

        private void EndTimeLimit()
        {
            timeLimitUI.Disable();
        }
    }
}
