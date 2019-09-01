using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public static class GameFactory {

	public static Container Create () {
		Container game = new Container ();

		// Add Systems
		game.AddAspect<ActionSystem> ();
		game.AddAspect<AttackSystem> ();
		game.AddAspect<CardSystem> ();
		game.AddAspect<CombatantSystem> ();
		game.AddAspect<DataSystem> ();
		game.AddAspect<DestructableSystem> ();
		game.AddAspect<ManaSystem> ();
		game.AddAspect<MatchSystem> ();
		game.AddAspect<MinionSystem> ();
		game.AddAspect<PlayerSystem> ();
		game.AddAspect<VictorySystem> ();
        game.AddAspect<DeathSystem>();
        game.AddAspect<EnemySystem>();
        game.AddAspect<TauntSystem>();

		// Add Other
		game.AddAspect<StateMachine> ();
		game.AddAspect<GlobalGameState> ();

		return game;
	}
}