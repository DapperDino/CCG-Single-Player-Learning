using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardAction : GameAction {
	public Card card;

	public PlayCardAction(Card card) {
		this.card = card;
	}
}
