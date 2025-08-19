using System.ComponentModel;
using DialogueSystem.Runtime;

namespace DialogueSystem.Default.Runtime
{
    public class DefaultOptionParams : OptionParams
    {
        [DefaultValue("Response")]

        public override OptionParams Clone() => new DefaultOptionParams()
        {
            text = new string(text),
        };
    }
}