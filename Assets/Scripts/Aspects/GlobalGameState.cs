using CCG.Containers;

namespace CCG.Aspects
{
    public class GlobalGameState : IAspect
    {
        public IContainer Container { get; set; } = null;
    }
}