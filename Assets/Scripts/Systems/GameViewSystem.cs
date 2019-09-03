using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public class GameViewSystem : MonoBehaviour, IAspect {

	public IContainer container { 
		get {
			if (_container == null) {
				_container = GameFactory.Create ();
				_container.AddAspect (this);
			}
			return _container;
		}
		set {
			_container = value;
		}
	}
	IContainer _container;

	ActionSystem actionSystem;

	void Awake () {
		container.Awake ();
		actionSystem = container.GetAspect<ActionSystem> ();
		Temp_SetupSinglePlayer ();
	}

	void Start () {
		container.ChangeState<PlayerIdleState> ();
	}

	void Update () {
		actionSystem.Update ();
	}

	void Temp_SetupSinglePlayer() {
		var match = container.GetMatch ();
		match.players [0].mode = ControlModes.Local;
		match.players [1].mode = ControlModes.Computer;

		foreach (Player p in match.players) {
			var deck = DeckFactory.Create ();
			foreach (Card card in deck) {
				card.ownerIndex = p.index;
			}
			p [Zones.Deck].AddRange (deck);

			var hero = new Hero ();
			hero.hitPoints = hero.maxHitPoints = 30;
			hero.allowedAttacks = 1;
			hero.ownerIndex = p.index;
			hero.zone = Zones.Hero;
			p.hero.Add (hero);
		}
	}
}