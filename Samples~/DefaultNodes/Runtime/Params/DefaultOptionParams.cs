using System.ComponentModel;
using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    public class DefaultOptionParams : OptionParams
    {
        public DefaultOptionParams() {}

        protected DefaultOptionParams(DefaultOptionParams other) : base(other) { }

        public override OptionParams Clone() => new DefaultOptionParams(this);
    }
}