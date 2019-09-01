using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour {
	public DeckView deck;
	public HandView hand;
	public TableView table;
	public HeroView hero;
	public GameObject cardPrefab;
	public Player player { get; private set; }

	public void SetPlayer (Player player) {
		this.player = player;
		var heroCard = player.hero [0] as Hero;
		hero.SetHero (heroCard);
	}

	public GameObject GetMatch (Card card) {
		switch (card.zone) {
		case Zones.Battlefield:
			return table.GetMatch (card);
		case Zones.Hero:
			return hero.gameObject;
		default:
			Debug.Log ("No Implementation for zone");
			return null;
		}
	}
}