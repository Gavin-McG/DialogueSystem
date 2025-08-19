using DialogueSystem.Runtime;

namespace DialogueSystem.Default.Editor.Params
{
    public class DefaultBaseParams : BaseParams
    {
        public DialogueProfile profile;

        public override BaseParams GetCopy() => new DefaultBaseParams()
        {
            text = new string(text),
            profile = profile,
        };
    }
}