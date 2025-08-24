using WolverineSoft.DialogueSystem;

namespace WolverineSoft.DialogueSystem.Default
{
    public class DefaultBaseParams : BaseParams
    {
        public DialogueProfile profile;
        
        public DefaultBaseParams() {}
        
        protected DefaultBaseParams(DefaultBaseParams other) : base(other)
        {
            profile = other.profile;
        }

        public override BaseParams Clone() => new DefaultBaseParams(this);
    }
}