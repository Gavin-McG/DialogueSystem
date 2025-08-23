using System.ComponentModel;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    public class DefaultOptionParams : OptionParams
    {
        public bool grayOut;
        
        public override OptionParams Clone() => new DefaultOptionParams()
        {
            text = new string(text),
        };
    }
}