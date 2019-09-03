using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using UnityEngine;
using System;

public class DrawCardsAction : GameAction, IAbilityLoader {
	public int amount;
	public List<Card> cards;

	#region Constructors
	public DrawCardsAction() {
		
	}

	public DrawCardsAction(Player player, int amount) {
		this.player = player;
		this.amount = amount;
	}
	#endregion

	#region IAbility
	public void Load (IContainer game, Ability ability) {
		player = game.GetMatch ().players [ability.card.ownerIndex];
		amount = Convert.ToInt32 (ability.userInfo);
	}
	#endregion
}