using System.Collections;
using DialogueSystem.Default.Editor.Params;
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
        [SerializeField] private ProfileUIManager profileUIManager;

        private bool dialogueEnabled = false;
        private DialogueSettings currentSettings;
        private DialogueParams currentParams;
        private DefaultBaseParams baseParams;
        private DefaultChoiceParams choiceParams;
        private float timeStarted;

        private void OnEnable()
        {
            dialogueManager.beginDialogueEvent.AddListener(BeginDialogue);
            
            mainTextUI.completedText.AddListener(BeginChoiceTimer);
            
            timeLimitUI.timeLimitExpired.AddListener(TimeExpired);
            
            choiceUIManager.AddChoiceListener(ChoicePressed);
            choiceUIManager.AddContinueListener(ContinuePressed);
        }

        private void OnDisable()
        {
            dialogueManager.beginDialogueEvent.RemoveListener(BeginDialogue);
            
            mainTextUI.completedText.RemoveListener(BeginChoiceTimer);

            timeLimitUI.timeLimitExpired.RemoveListener(TimeExpired);
            
            choiceUIManager.RemoveChoiceListener(ChoicePressed);
            choiceUIManager.RemoveContinueListener(ContinuePressed);
        }

        private void BeginDialogue(DialogueSettings dialogueSettings)
        {
            currentSettings = dialogueSettings;
            DisplayDialogue(dialogueManager.GetNextDialogue());
        }

        private void DisplayDialogue(DialogueParams dialogueParams)
        {
            if (dialogueParams == null)
            {
                HideDialogue();
                return;
            }
            
            dialogueEnabled = true;
            dialogueUI.SetActive(true);
            
            currentParams = dialogueParams;
            baseParams = dialogueParams.GetBaseParams<DefaultBaseParams>();
            choiceParams = dialogueParams.GetChoiceParams<DefaultChoiceParams>();

            mainTextUI.SetText(currentParams.baseParams.text);
            timeStarted = Time.time;
            EndTimeLimit();

            if (baseParams.profile)
            {
                profileUIManager.IntroduceProfile(baseParams.profile);
            }
    
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
                if (choiceParams.hasTimeLimit)
                {
                    timeLimitUI.StartTimer(choiceParams.timeLimitDuration);
                }
            }
        }

        private void ChoicePressed(int index)
        {
            EndTimeLimit();
            DisplayDialogue(dialogueManager.GetNextDialogue(new AdvanceDialogueContext()
            {
                choice = index,
                inputDelay = Time.time - timeStarted,
                timedOut = false
            }));
        }

        private void ContinuePressed()
        {
            if (mainTextUI.textState == MainTextUI.TextState.Scrolling)
            {
                mainTextUI.CompleteText();
            }
            else if (mainTextUI.textState == MainTextUI.TextState.Completed)
            {
                DisplayDialogue(dialogueManager.GetNextDialogue(new AdvanceDialogueContext()
                {
                    choice = -1,
                    inputDelay = Time.time - timeStarted,
                    timedOut = false
                }));
            }
        }

        private void TimeExpired()
        {
            DisplayDialogue(dialogueManager.GetNextDialogue(new AdvanceDialogueContext()
            {
                choice = 0,
                inputDelay = choiceParams.timeLimitDuration,
                timedOut = true,
            }));
        }

        private void HideDialogue()
        {
            dialogueEnabled = false;
            dialogueUI.SetActive(false);
            EndTimeLimit();
        }

        private void EndTimeLimit()
        {
            timeLimitUI.Disable();
        }
    }
}
