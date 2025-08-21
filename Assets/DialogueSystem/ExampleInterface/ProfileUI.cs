using DialogueSystem.Default.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem.ExampleInterface
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
            profileText.text = profile.displayName;
        }
    }
}