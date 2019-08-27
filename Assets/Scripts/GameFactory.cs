using CCG.Aspects;
using CCG.Containers;

namespace CCG
{
    public static class GameFactory
    {
        public static Container Create()
        {
            var game = new Container();

            game.AddAspect<ActionSystem>();
            game.AddAspect<DataSystem>();
            game.AddAspect<MatchSystem>();

            game.AddAspect<StateMachine>();
            game.AddAspect<GlobalGameState>();

            return game;
        }
    }
}
