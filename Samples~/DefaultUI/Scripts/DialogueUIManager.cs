using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WolverineSoft.DialogueSystem.Default;
using WolverineSoft.DialogueSystem;


namespace WolverineSoft.DialogueSystem.DefaultUI
{
    using MyParams = WolverineSoft.DialogueSystem.DialogueParams<DefaultBaseParams, DefaultChoiceParams, DefaultOptionParams>;
    
    public class DialogueUIManager : MonoBehaviour
    {

        [SerializeField] private DialogueManager dialogueManager;

        [Header("UI References")] [SerializeField]
        private GameObject dialogueUI;
        [SerializeField] private MainTextUI mainTextUI;
        [SerializeField] private ChoiceUIManager choiceUIManager;
        [SerializeField] private TimeLimitUI timeLimitUI;
        [SerializeField] private ProfileUIManager profileUIManager;
        
        private DefaultDialogueSettings currentSettings;
        private MyParams currentParams;
        
        private float timeStarted;

        private void OnEnable()
        {
            dialogueManager.StartedDialogue.AddListener(BeginDialogue);
            
            mainTextUI.completedText.AddListener(BeginChoiceTimer);
            
            timeLimitUI.timeLimitExpired.AddListener(TimeExpired);
            
            choiceUIManager.AddChoiceListener(ChoicePressed);
            choiceUIManager.AddContinueListener(ContinuePressed);
        }

        private void OnDisable()
        {
            dialogueManager.StartedDialogue.RemoveListener(BeginDialogue);
            
            mainTextUI.completedText.RemoveListener(BeginChoiceTimer);

            timeLimitUI.timeLimitExpired.RemoveListener(TimeExpired);
            
            choiceUIManager.RemoveChoiceListener(ChoicePressed);
            choiceUIManager.RemoveContinueListener(ContinuePressed);
        }

        private void BeginDialogue()
        {
            currentSettings = dialogueManager.GetSettings<DefaultDialogueSettings>();
            DisplayDialogue(dialogueManager.AdvanceDialogue<DefaultBaseParams, DefaultChoiceParams, DefaultOptionParams>());
        }

        private void DisplayDialogue(MyParams dialogueParams)
        {
            if (dialogueParams == null)
            {
                HideDialogue();
                return;
            }
            
            dialogueUI.SetActive(true);
            
            currentParams = dialogueParams;

            mainTextUI.SetText(currentParams.BaseParams.text);
            timeStarted = Time.time;
            EndTimeLimit();

            if (currentParams.BaseParams.profile)
            {
                profileUIManager.IntroduceProfile(currentParams.BaseParams.profile);
            }
    
            switch (currentParams.dialogueType)
            {
                case DialogueType.Basic: DisplayBasicDialogue(); break;
                case DialogueType.Choice: DisplayChoiceDialogue(); break;
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
            if (currentParams.dialogueType == DialogueType.Choice)
            {
                choiceUIManager.SetChoiceButtons(currentParams.Options);
                if (currentParams.ChoiceParams.hasTimeLimit)
                {
                    timeLimitUI.StartTimer(currentParams.ChoiceParams.timeLimitDuration);
                }
            }
        }

        private void ChoicePressed(int index)
        {
            EndTimeLimit();
            DisplayDialogue(dialogueManager.AdvanceDialogue<DefaultBaseParams, DefaultChoiceParams, DefaultOptionParams>(new DefaultAdvanceParams(
                inputDelay: Time.time - timeStarted, 
                choice: index, 
                timedOut: false
            )));
        }

        private void ContinuePressed()
        {
            if (mainTextUI.textState == MainTextUI.TextState.Scrolling)
            {
                mainTextUI.CompleteText();
            }
            else if (mainTextUI.textState == MainTextUI.TextState.Completed)
            {
                DisplayDialogue(dialogueManager.AdvanceDialogue<DefaultBaseParams, DefaultChoiceParams, DefaultOptionParams>(new DefaultAdvanceParams(
                    inputDelay: Time.time - timeStarted, 
                    choice: 0, 
                    timedOut: false
                )));
            }
        }

        private void TimeExpired()
        {
            DisplayDialogue(dialogueManager.AdvanceDialogue<DefaultBaseParams, DefaultChoiceParams, DefaultOptionParams>(new DefaultAdvanceParams(
                inputDelay: currentParams.ChoiceParams.timeLimitDuration, 
                choice: 0, 
                timedOut: true
            )));
        }

        private void HideDialogue()
        {
            dialogueUI.SetActive(false);
            EndTimeLimit();
        }

        private void EndTimeLimit()
        {
            timeLimitUI.Disable();
        }
    }
}
