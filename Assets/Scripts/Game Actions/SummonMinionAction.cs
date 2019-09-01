using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMinionAction : GameAction {
	public Minion minion;

	public SummonMinionAction(Minion minion) {
		this.minion = minion;
	}
}