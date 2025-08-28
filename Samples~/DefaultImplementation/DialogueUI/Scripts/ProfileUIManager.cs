using WolverineSoft.DialogueSystem;
using UnityEngine;
using WolverineSoft.DialogueSystem.Default;

namespace WolverineSoft.DialogueSystem.ExampleInterface
{
    public class ProfileUIManager : MonoBehaviour
    {
        [SerializeField] ProfileUI leftProfileUI;
        [SerializeField] ProfileUI rightProfileUI;

        private DialogueProfile currentLeftProfile;
        private DialogueProfile currentRightProfile;

        public void IntroduceProfile(DialogueProfile profile)
        {
            if (profile.side == DialogueProfile.Side.Right)
            {
                rightProfileUI.SetProfile(profile);
                currentRightProfile = profile;

                if (currentLeftProfile?.characterName == profile.characterName)
                {
                    leftProfileUI.Disable();
                    currentLeftProfile = null;
                }
            }
            else
            {
                leftProfileUI.SetProfile(profile);
                currentLeftProfile = profile;
                
                if (currentRightProfile?.characterName == profile.characterName)
                {
                    rightProfileUI.Disable();
                    currentRightProfile = null;
                }
            }
        }
    }
}