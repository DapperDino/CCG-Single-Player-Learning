using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAction : GameAction {
	public List<IDestructable> targets;
	public int amount;

	public DamageAction(IDestructable target, int amount) {
		targets = new List<IDestructable> (1);
		targets.Add (target);
		this.amount = amount;
	}

	public DamageAction(List<IDestructable> targets, int amount) {
		this.targets = targets;
		this.amount = amount;
	}
}