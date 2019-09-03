using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAction : GameAction {
	public Card card;

	public DeathAction (Card card) {
		this.card = card;
	}
}
