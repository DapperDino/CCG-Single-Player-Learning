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
			for (int i = 0; i < Player.maxDeck; ++i) {
				var card = new Minion ();
				card.name = "Card " + i.ToString();
				card.cost = Random.Range (1, 10);
				card.maxHitPoints = card.hitPoints = Random.Range (1, card.cost);
				card.attack = card.cost - card.hitPoints;
				card.ownerIndex = p.index;
				p [Zones.Deck].Add (card);
			}

			var hero = new Hero ();
			hero.hitPoints = hero.maxHitPoints = 30;
			p.hero.Add (hero);
		}
	}
}