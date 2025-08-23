using System.ComponentModel;
using WolverineSoft.DialogueSystem.Runtime;

namespace WolverineSoft.DialogueSystem.Default.Runtime
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