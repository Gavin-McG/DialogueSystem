using TMPro;
using UnityEngine;
using UnityEngine.Events;
using WolverineSoft.DialogueSystem.Default;

namespace WolverineSoft.DialogueSystem.Example
{
    /// <summary>
    /// Manager Core dialogue UI Components
    /// </summary>
    public class DialogueUI : MonoBehaviour
    {
        [HideInInspector] public UnityEvent<AdvanceContext> AdvanceDialogue = new();
        
        public enum DialogueState { Scrolling, Completed, Stalling, Hidden }
        
        [SerializeField] private ProfileUI profileUI;
        [SerializeField] private MainTextUI mainTextUI;
        [SerializeField] private ChoiceUI choiceUI;
        [SerializeField] private TimeLimitUI timeLimitUI;

        private DialogueInfo currentInfo;
        private DialogueState state = DialogueState.Scrolling;

        private void OnEnable()
        {
            mainTextUI.CompletedText.AddListener(CompleteText);
            choiceUI.ChoicePressed.AddListener(SelectChoice);
            choiceUI.ContinuePressed.AddListener(Continue);
            timeLimitUI.TimeLimitExpired.AddListener(TimeLimitExpired);
        }

        private void OnDisable()
        {
            mainTextUI.CompletedText.RemoveListener(CompleteText);
            choiceUI.ChoicePressed.RemoveListener(SelectChoice);
            choiceUI.ContinuePressed.RemoveListener(Continue);
            timeLimitUI.TimeLimitExpired.RemoveListener(TimeLimitExpired);
        }
        
        //Dialogue Methods

        public void ShowTextDialogue(DialogueInfo info)
        {
            gameObject.SetActive(true);
            state = DialogueState.Scrolling;
            currentInfo = info;
            
            var textParams = info.GetTextParams<MyTextParameters>();
            profileUI.UpdateProfile(textParams);
            
            mainTextUI.UpdateText(info.text);
            
            choiceUI.SetContinue();
        }

        public void ShowChoiceDialogue(DialogueInfo info)
        {
            gameObject.SetActive(true);
            state = DialogueState.Scrolling;
            currentInfo = info;
            
            var textParams = info.GetTextParams<MyTextParameters>();
            profileUI.UpdateProfile(textParams);
            
            mainTextUI.UpdateText(info.text);
            
            choiceUI.SetContinue();
        }

        public void ShowStallDialogue(DialogueInfo info)
        {
            gameObject.SetActive(false);
            state = DialogueState.Stalling;
            currentInfo = info;
        }
        
        public void EndDialogue()
        {
            gameObject.SetActive(false);
            state = DialogueState.Stalling;
            currentInfo = null;
        }
        
        //Listener Methods

        private void CompleteText()
        {
            state = DialogueState.Completed;
            
            var choiceParams = currentInfo.GetChoiceParams<MyChoiceParameters>();
            if (choiceParams != null && choiceParams.isTimed)
            {
                choiceUI.SetChoices(currentInfo.responses);
                timeLimitUI.StartTimer(choiceParams.timeLimit);
            }
        }

        private void SelectChoice(int index)
        {
            timeLimitUI.StopTimer();
            AdvanceDialogue.Invoke(new AdvanceContext(index));
        }

        private void Continue()
        {
            if (state == DialogueState.Scrolling)
            {
                mainTextUI.CompleteText();
                return;
            }
            
            AdvanceDialogue.Invoke(new AdvanceContext(-1));
        }

        private void TimeLimitExpired()
        {
            AdvanceDialogue.Invoke(new AdvanceContext(-1));
        }
    }
}