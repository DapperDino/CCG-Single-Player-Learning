using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class DeckFactory {
	public static List<Card> Create () {
		List<Card> deck = new List<Card> ();
		Func<Card>[] builder = new Func<Card>[] { 
			Card1, Card2, Card3, Card4, Card5, 
			Card6, Card7, Card8, Card9, Card10, 
			Card11, Card12, Card13, Card14, Card15
		};
		foreach (Func<Card> func in builder) {
			deck.Add(func());
			deck.Add(func());
		}
		return deck;
	}

	static Card Card1 () {
		var card = CreateCard<Spell> ("Shoots A Lot", "3 damage to random enemies.", 1);
		var ability = AddAbility (card, typeof(DamageAction).Name, 1);
		var targetSelector = new RandomTarget ();
		targetSelector.mark = new Mark (Alliance.Enemy, Zones.Active);
		targetSelector.count = 3;
		ability.AddAspect<ITargetSelector> (targetSelector);
		return card;
	}

	static Card Card2 () {
		return CreateMinion ("Grunt 1", string.Empty, 1, 2, 1);
	}

	static Card Card3 () {
		var card = CreateCard<Spell> ("Wide Boom", "1 damage to all enemy minions.", 2);
		var ability = AddAbility (card, typeof(DamageAction).Name, 1);
		var targetSelector = new AllTarget ();
		targetSelector.mark = new Mark (Alliance.Enemy, Zones.Battlefield);
		ability.AddAspect<ITargetSelector> (targetSelector);
		return card;
	}

	static Card Card4 () {
		return CreateMinion ("Grunt 2", string.Empty, 2, 3, 2);
	}

	static Card Card5 () {
		var card = CreateMinion ("Rich Grunt", "Draw a card when summoned.", 2, 1, 1);
		AddAbility (card, typeof(DrawCardsAction).Name, 1);
		return card;
	}

	static Card Card6 () {
		return CreateMinion ("Grunt 3", string.Empty, 2, 2, 3);
	}

	static Card Card7 () {
		var card = CreateCard<Spell> ("Card Lovin'", "Draw 2 cards", 3);
		AddAbility (card, typeof(DrawCardsAction).Name, 2);
		return card;
	}

	static Card Card8 () {
		var card = CreateMinion ("Grunt 4", "Taunt", 3, 2, 2);
		card.AddAspect<Taunt> ();
		return card;
	}

	static Card Card9 () {
		var card = CreateMinion ("Grunt 5", "Taunt", 3, 1, 3);
		card.AddAspect<Taunt> ();
		return card;
	}
		
	static Card Card10 () {
		var card = CreateCard<Spell> ("Focus Beam", "6 damage", 4);
		var ability = AddAbility (card, typeof(DamageAction).Name, 6);
		ability.AddAspect<ITargetSelector> (new ManualTarget());
		var target = card.AddAspect<Target> ();
		target.allowed = new Mark (Alliance.Any, Zones.Active);
		target.preferred = new Mark (Alliance.Enemy, Zones.Active);
		return card;
	}

	static Card Card11 () {
		return CreateMinion ("Grunt 6", string.Empty, 4, 2, 7);
	}

	static Card Card12 () {
		var card = CreateMinion ("Grunt 7", "Taunt", 5, 2, 7);
		card.AddAspect<Taunt> ();
		return card;
	}

	static Card Card13 () {
		var card = CreateMinion ("Grunt 8", "Taunt", 4, 3, 5);
		card.AddAspect<Taunt> ();
		return card;
	}

	static Card Card14 () {
		var card = CreateMinion ("Grunt 9", "3 Damage to Opponent", 5, 4, 4);
		var ability = AddAbility (card, typeof(DamageAction).Name, 3);
		var targetSelector = new AllTarget ();
		targetSelector.mark = new Mark (Alliance.Enemy, Zones.Hero);
		ability.AddAspect<ITargetSelector> (targetSelector);
		return card;
	}

	static Card Card15 () {
		return CreateMinion ("Big Grunt", string.Empty, 6, 6, 7);
	}

	static T CreateCard<T> (string name, string text, int cost) where T : Card, new() {
		var card = new T();
		card.name = name;
		card.text = text;
		card.cost = cost;
		return card;
	}

	static Minion CreateMinion (string name, string text, int cost, int attack, int hitPoints) {
		var card = CreateCard<Minion> (name, text, cost);
		card.attack = attack;
		card.hitPoints = card.maxHitPoints = hitPoints;
		card.allowedAttacks = 1;
		return card;
	}

	static Ability AddAbility (Card card, string actionName, object userInfo) {
		var ability = card.AddAspect<Ability> ();
		ability.actionName = actionName;
		ability.userInfo = userInfo;
		return ability;
	}
}
