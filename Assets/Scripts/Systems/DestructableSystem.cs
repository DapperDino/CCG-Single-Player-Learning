using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class DestructableSystem : Aspect, IObserve {
	public void Awake () {
		this.AddObserver (OnPerformDamageAction, Global.PerformNotification<DamageAction> (), container);
	}

	public void Destroy () {
		this.RemoveObserver (OnPerformDamageAction, Global.PerformNotification<DamageAction> (), container);
	}

	void OnPerformDamageAction (object sender, object args) {
		var action = args as DamageAction;
		foreach (IDestructable target in action.targets) {
			target.hitPoints -= action.amount;
		}
	}
}