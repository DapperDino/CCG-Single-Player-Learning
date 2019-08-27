using CCG.Containers;

namespace CCG.Aspects
{
    public class DataSystem : Aspect
    {
        public Match Match { get; } = new Match();
    }

    public static class DataSystemExtensions
    {
        public static Match GetMatch(this IContainer game)
        {
            var dataSystem = game.GetAspect<DataSystem>();
            return dataSystem.Match;
        }
    }
}
