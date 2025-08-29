using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WolverineSoft.DialogueSystem.Default;

namespace WolverineSoft.DialogueSystem.DefaultUI
{
    public class ProfileUI : MonoBehaviour
    {
        [SerializeField] GameObject profileUI;
        [SerializeField] Image profileImage;
        [SerializeField] TextMeshProUGUI profileText;

        public void Disable()
        {
            profileUI.SetActive(false);
        }
        
        public void SetProfile(DialogueProfile profile)
        {
            profileUI.SetActive(true);
            profileImage.sprite = profile.sprite;
            profileText.text = profile.characterName;
        }
    }
}