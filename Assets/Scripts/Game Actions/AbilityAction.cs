using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAction : GameAction {
	public Ability ability;

	public AbilityAction (Ability ability) {
		this.ability = ability;
	}
}
