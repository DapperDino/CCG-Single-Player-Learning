using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TheLiquidFire.Notifications;
using TheLiquidFire.AspectContainer;

public abstract class BattlefieldCardView : MonoBehaviour {
	public Image avatar;
	public Text attack;
	public Text health;
	public Sprite inactive;
	public Sprite active;
	protected bool isActive;

	public abstract Card card { get; }

	void OnEnable () {
		this.AddObserver (OnPlayerIdleEnter, PlayerIdleState.EnterNotification);
		this.AddObserver (OnPlayerIdleExit, PlayerIdleState.ExitNotification);
		this.AddObserver (OnPerformDamageAction, Global.PerformNotification<DamageAction> ());
	}

	void OnDisable () {
		this.RemoveObserver (OnPlayerIdleEnter, PlayerIdleState.EnterNotification);
		this.RemoveObserver (OnPlayerIdleExit, PlayerIdleState.ExitNotification);
		this.RemoveObserver (OnPerformDamageAction, Global.PerformNotification<DamageAction> ());
	}

	void OnPlayerIdleEnter (object sender, object args) {
		var container = (sender as PlayerIdleState).container;
		isActive = container.GetAspect<AttackSystem> ().validAttackers.Contains (card);
		Refresh ();
	}

	void OnPlayerIdleExit (object sender, object args) {
		isActive = false;
	}

	protected abstract void Refresh ();

	void OnPerformDamageAction (object sender, object args) {
		var action = args as DamageAction;
		if (action.targets.Contains (card as IDestructable)) {
			Refresh ();
		}
	}
}
