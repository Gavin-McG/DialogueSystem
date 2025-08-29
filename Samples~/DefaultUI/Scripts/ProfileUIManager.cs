using WolverineSoft.DialogueSystem;
using UnityEngine;
using WolverineSoft.DialogueSystem.Default;

namespace WolverineSoft.DialogueSystem.DefaultUI
{
    public class ProfileUIManager : MonoBehaviour
    {
        [SerializeField] ProfileUI profileUI;

        private DialogueProfile currentProfile;

        public void IntroduceProfile(DialogueProfile profile)
        {
            profileUI.SetProfile(profile);
            currentProfile = profile;
        }
    }
}