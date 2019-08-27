using CCG.Containers;

namespace CCG.Aspects
{
    public class StateMachine : IAspect
    {
        public IContainer Container { get; set; } = null;
    }
}