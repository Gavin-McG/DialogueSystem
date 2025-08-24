using System.ComponentModel;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    public class DefaultOptionParams : OptionParams
    {
        public bool grayOut;
        
        public DefaultOptionParams() {}

        protected DefaultOptionParams(DefaultOptionParams other) : base(other)
        {
            grayOut = other.grayOut;
        }

        public override OptionParams Clone() => new DefaultOptionParams(this);
    }
}