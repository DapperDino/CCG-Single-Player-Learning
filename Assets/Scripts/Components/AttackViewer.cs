using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.Notifications;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Animation;

public class AttackViewer : MonoBehaviour {

	void OnEnable () {
		this.AddObserver (OnValidateAttack, Global.ValidateNotification<AttackAction> ());
	}

	void OnDisable () {
		this.RemoveObserver (OnValidateAttack, Global.ValidateNotification<AttackAction> ());
	}

	void OnValidateAttack (object sender, object args) {
		var action = sender as AttackAction;
		action.perform.viewer = OnPerformAttack;
	}

	IEnumerator OnPerformAttack (IContainer game, GameAction action) {
		var attackAction = action as AttackAction;
		var board = GetComponent<BoardView> ();
		var attacker = board.GetMatch (attackAction.attacker);
		var target = board.GetMatch (attackAction.target);
		if (attacker == null || target == null)
			yield break;

		var startPos = attacker.transform.position;
		var toTarget = target.transform.position - startPos;

		var tweener = attacker.transform.MoveTo (target.transform.position + new Vector3 (0, 1, 0), 0.5f, EasingEquations.EaseInBack);
		while (tweener != null)
			yield return false;

		// Apply the attack damage here
		yield return true;

		var bounceBack = (toTarget.normalized * (toTarget.magnitude - 0.5f)) + startPos;
		tweener = attacker.transform.MoveTo (bounceBack, 0.2f, EasingEquations.EaseOutQuad);
		while (tweener != null)
			yield return false;

		tweener = attacker.transform.MoveTo (startPos, 0.25f);
		while (tweener != null)
			yield return false;
	}
}
