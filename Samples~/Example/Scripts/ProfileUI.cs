using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WolverineSoft.DialogueSystem.Example
{
    /// <summary>
    /// Manage Displayed Profile of Dialogue
    /// </summary>
    public class ProfileUI : MonoBehaviour
    {
        [SerializeField] private Image profileImage;
        [SerializeField] private GameObject nameContainer;
        [SerializeField] private TextMeshProUGUI nameText;
        
        public void UpdateProfile(MyTextParameters parameters)
        {
            //Hide profile if no textParameters provided
            if (parameters == null)
            {
                HideProfile();
                return;
            }
            
            //null profile means no change
            if (parameters.profile == null) return;

            //Hide Profile if sprite is null
            if (parameters.profile.sprite == null)
            {
                HideProfile();
                return;
            }
            
            ShowProfile(parameters.profile);
        }

        private void ShowProfile(DialogueProfile profile)
        {
            profileImage.enabled = true;
            nameContainer.SetActive(true);

            profileImage.sprite = profile.sprite;
            nameText.text = profile.characterName;
        }

        public void HideProfile()
        {
            profileImage.enabled = false;
            nameContainer.SetActive(false);
        }
    }
}