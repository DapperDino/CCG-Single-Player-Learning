using CCG.Containers;

namespace CCG.Aspects
{
    public interface IAspect
    {
        IContainer Container { get; set; }
    }
}