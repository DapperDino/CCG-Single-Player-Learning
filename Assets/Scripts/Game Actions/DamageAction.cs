using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using System;

public class DamageAction : GameAction, IAbilityLoader {
	public List<IDestructable> targets;
	public int amount;

	#region Constructors
	public DamageAction() {
		
	}

	public DamageAction(IDestructable target, int amount) {
		targets = new List<IDestructable> (1);
		targets.Add (target);
		this.amount = amount;
	}

	public DamageAction(List<IDestructable> targets, int amount) {
		this.targets = targets;
		this.amount = amount;
	}
	#endregion

	#region IAbility
	public void Load (IContainer game, Ability ability) {
		var targetSelector = ability.GetAspect<ITargetSelector> ();
		var cards = targetSelector.SelectTargets (game);
		targets = new List<IDestructable> ();
		foreach (Card card in cards) {
			var destructable = card as IDestructable;
			if (destructable != null)
				targets.Add (destructable);
		}
		amount = Convert.ToInt32 (ability.userInfo);
	}
	#endregion
}