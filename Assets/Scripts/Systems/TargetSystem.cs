using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;
using TheLiquidFire.Extensions;

public class TargetSystem : Aspect, IObserve {
	public void Awake () {
		this.AddObserver (OnValidatePlayCard, Global.ValidateNotification<PlayCardAction> ());
	}

	public void Destroy () {
		this.RemoveObserver (OnValidatePlayCard, Global.ValidateNotification<PlayCardAction> ());
	}

	public void AutoTarget (Card card, ControlModes mode) {
		var target = card.GetAspect<Target> ();
		if (target == null)
			return;
		var mark = mode == ControlModes.Computer ? target.preferred : target.allowed;
		var candidates = GetMarks (card, mark);
		target.selected = candidates.Count > 0 ? candidates.Random() : null;
	}

	public List<Card> GetMarks (Card source, Mark mark) {
		var marks = new List<Card> ();
		var players = GetPlayers (source, mark);
		foreach (Player player in players) {
			var cards = GetCards (source, mark, player);
			marks.AddRange (cards);
		}
		return marks;
	}

	List<Player> GetPlayers (Card source, Mark mark) {
		var dataSystem = container.GetAspect<DataSystem> ();
		var players = new List<Player> ();
		var pair = new Dictionary<Alliance, Player> () {
			{ Alliance.Ally, dataSystem.match.players[source.ownerIndex] }, 
			{ Alliance.Enemy, dataSystem.match.players[1 - source.ownerIndex] }
		};
		foreach (Alliance key in pair.Keys) {
			if (mark.alliance.Contains (key)) {
				players.Add (pair[key]);
			}
		}
		return players;
	}

	List<Card> GetCards (Card source, Mark mark, Player player) {
		var cards = new List<Card> ();
		var zones = new Zones[] { 
			Zones.Hero, 
			Zones.Weapon, 
			Zones.Deck, 
			Zones.Hand, 
			Zones.Battlefield, 
			Zones.Secrets, 
			Zones.Graveyard 
		};
		foreach (Zones zone in zones) {
			if (mark.zones.Contains (zone)) {
				cards.AddRange (player[zone]);
			}
		}
		return cards;
	}

	void OnValidatePlayCard (object sender, object args) {
		var playCardAction = sender as PlayCardAction;
		var card = playCardAction.card;
		var target = card.GetAspect<Target> ();
		if (target == null || (target.required == false && target.selected == null))
			return;
		var validator = args as Validator;
		var candidates = GetMarks (card, target.allowed);
		if (!candidates.Contains(target.selected)) {
			validator.Invalidate ();
		}
	}
}