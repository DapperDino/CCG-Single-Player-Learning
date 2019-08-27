using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public class GameOverState : BaseState {
	public override void Enter () {
		base.Enter ();
		Debug.Log ("Game Over");
	}
}