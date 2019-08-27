using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public static class GameFactory
{

    public static Container Create()
    {
        Container game = new Container();

        // Add Systems
        game.AddAspect<ActionSystem>();
        game.AddAspect<DataSystem>();
        game.AddAspect<DestructableSystem>();
        game.AddAspect<MatchSystem>();
        game.AddAspect<PlayerSystem>();
        game.AddAspect<VictorySystem>();
        game.AddAspect<CardSystem>();
        game.AddAspect<MinionSystem>();

        // Add Other
        game.AddAspect<StateMachine>();
        game.AddAspect<GlobalGameState>();

        return game;
    }
}