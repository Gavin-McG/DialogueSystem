using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Default.Runtime
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