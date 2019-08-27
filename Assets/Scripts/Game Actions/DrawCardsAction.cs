using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardsAction : GameAction {
	public int amount;
	public List<Card> cards;

	public DrawCardsAction(Player player, int amount) {
		this.player = player;
		this.amount = amount;
	}
}