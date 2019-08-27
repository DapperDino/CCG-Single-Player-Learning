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
	}

	void Start () {
		Temp_SetupSinglePlayer ();
		container.ChangeState<PlayerIdleState> ();
	}

	void Update () {
		actionSystem.Update ();
	}

	void Temp_SetupSinglePlayer() {
		var match = container.GetMatch ();
		match.players [0].mode = ControlModes.Local;
		match.players [1].mode = ControlModes.Computer;
	}
}