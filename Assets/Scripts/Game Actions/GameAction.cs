using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class GameAction {
	#region Fields & Properties
	public readonly int id;
	public Player player { get; set; }
	public int priority { get; set; }
	public int orderOfPlay { get; set; }
	public bool isCanceled { get; protected set; }
	public Phase prepare { get; protected set; }
	public Phase perform { get; protected set; }
	public Phase cancel { get; protected set; }
	#endregion

	#region Constructor
	public GameAction() {
		id = Global.GenerateID (this.GetType ());
		prepare = new Phase (this, OnPrepareKeyFrame);
		perform = new Phase (this, OnPerformKeyFrame);
		cancel = new Phase (this, OnCancelKeyFrame);
	}
	#endregion

	#region Public
	public virtual void Cancel () {
		isCanceled = true;
	}
	#endregion

	#region Protected
	protected virtual void OnPrepareKeyFrame (IContainer game) {
		var notificationName = Global.PrepareNotification (this.GetType ());
		game.PostNotification (notificationName, this);
	}

	protected virtual void OnPerformKeyFrame (IContainer game) {
		var notificationName = Global.PerformNotification (this.GetType ());
		game.PostNotification (notificationName, this);
	}

	protected virtual void OnCancelKeyFrame (IContainer game) {
		var notificationName = Global.CancelNotification (this.GetType ());
		game.PostNotification (notificationName, this);
	}
	#endregion
}