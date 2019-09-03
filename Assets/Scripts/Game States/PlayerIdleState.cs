using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class PlayerIdleState : BaseState {
	public const string EnterNotification = "PlayerIdleState.EnterNotification";
	public const string ExitNotification = "PlayerIdleState.ExitNotification";

	public override void Enter () {
		var mode = container.GetMatch ().CurrentPlayer.mode;
		container.GetAspect<AttackSystem> ().Refresh ();
		container.GetAspect<CardSystem> ().Refresh (mode);
		if (mode == ControlModes.Computer)
			container.GetAspect<EnemySystem> ().TakeTurn ();
		this.PostNotification (EnterNotification);
	}

	public override void Exit () {
		this.PostNotification (ExitNotification);
	}
}