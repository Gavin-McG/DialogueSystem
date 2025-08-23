using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    public class DefaultBaseParams : BaseParams
    {
        public DialogueProfile profile;

        public override BaseParams Clone() => new DefaultBaseParams()
        {
            text = new string(text),
            profile = profile,
        };
    }
}