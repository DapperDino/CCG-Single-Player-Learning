namespace TheLiquidFire.AspectContainer
{
    public interface IAspect
    {
        IContainer container { get; set; }
    }

    public class Aspect : IAspect
    {
        public IContainer container { get; set; }
    }
}