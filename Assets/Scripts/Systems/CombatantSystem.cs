using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class CombatantSystem : Aspect, IObserve {
	public void Awake () {
		this.AddObserver (OnFilterAttackers, AttackSystem.FilterAttackersNotification, container);
		this.AddObserver (OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction> (), container);
	}

	public void Destroy () {
		this.RemoveObserver (OnFilterAttackers, AttackSystem.FilterAttackersNotification, container);
		this.RemoveObserver (OnPerformChangeTurn, Global.PerformNotification<ChangeTurnAction> (), container);
	}

	void OnFilterAttackers (object sender, object args) {
		var candidates = args as List<Card>;
		for (int i = candidates.Count - 1; i >= 0; --i) {
			var combatant = candidates [i] as ICombatant;
			if (!CanAttack(combatant)) {
				candidates.RemoveAt (i);
			}
		}
	}

	void OnPerformChangeTurn (object sender, object args) {
		var action = args as ChangeTurnAction;
		var player = container.GetMatch ().players [action.targetPlayerIndex];
		var active = container.GetAspect<AttackSystem> ().GetActive (player);
		foreach (Card card in active) {
			var combatant = card as ICombatant;
			if (combatant == null)
				continue;
			combatant.remainingAttacks = combatant.allowedAttacks;
		}
	}

	bool CanAttack (ICombatant combatant) {
		return combatant != null && combatant.attack > 0 && combatant.remainingAttacks > 0;
	}
}