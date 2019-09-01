using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class ActionSystem : Aspect {
	#region Notifications
	public const string beginSequenceNotification = "ActionSystem.beginSequenceNotification";
	public const string endSequenceNotification = "ActionSystem.endSequenceNotification";
	public const string deathReaperNotification = "ActionSystem.deathReaperNotification";
	public const string completeNotification = "ActionSystem.completeNotification";
	#endregion

	#region Fields & Properties
	GameAction rootAction;
	IEnumerator rootSequence;
	List<GameAction> openReactions;
	public bool IsActive { get { return rootSequence != null; }}
	#endregion

	#region Public
	public void Perform (GameAction action) {
		if (IsActive) return;
		rootAction = action;
		rootSequence = Sequence (action);
	}

	public void Update () {
		if (rootSequence == null)
			return;

		if (rootSequence.MoveNext () == false) {
			rootAction = null;
			rootSequence = null;
			openReactions = null;
			this.PostNotification (completeNotification);
		}
	}

	public void AddReaction (GameAction action) {
		if (openReactions != null)
			openReactions.Add (action);
	}
	#endregion

	#region Private
	IEnumerator Sequence (GameAction action) {
		this.PostNotification (beginSequenceNotification, action);

		if (action.Validate () == false)
			action.Cancel ();

		var phase = MainPhase (action.prepare);
		while (phase.MoveNext ()) { yield return null; }

		phase = MainPhase (action.perform);
		while (phase.MoveNext ()) { yield return null; }

		phase = MainPhase (action.cancel);
		while (phase.MoveNext ()) { yield return null; }

		if (rootAction == action) {
			phase = EventPhase (deathReaperNotification, action, true);
			while (phase.MoveNext ()) { yield return null; }
		}

		this.PostNotification (endSequenceNotification, action);
	}

	IEnumerator MainPhase (Phase phase) {
		bool isActionCancelled = phase.owner.isCanceled;
		bool isCancelPhase = phase.owner.cancel == phase;
		if (isActionCancelled ^ isCancelPhase)
			yield break;

		var reactions = openReactions = new List<GameAction> ();
		var flow = phase.Flow (container);
		while (flow.MoveNext ()) { yield return null; }

		flow = ReactPhase (reactions);
		while (flow.MoveNext ()) { yield return null; }
	}

	IEnumerator ReactPhase (List<GameAction> reactions) {
		reactions.Sort (SortActions);
		foreach (GameAction reaction in reactions) {
			IEnumerator subFlow = Sequence (reaction);
			while (subFlow.MoveNext ()) {
				yield return null;
			}
		}
	}

	IEnumerator EventPhase (string notification, GameAction action, bool repeats = false) {
		List<GameAction> reactions;
		do {
			reactions = openReactions = new List<GameAction> ();
			this.PostNotification(notification, action);

			var phase = ReactPhase (reactions);
			while (phase.MoveNext ()) { yield return null; }
		} while (repeats == true && reactions.Count > 0);
	}

	int SortActions (GameAction x, GameAction y) {
		if (x.priority != y.priority) {
			return y.priority.CompareTo(x.priority);
		} else {
			return x.orderOfPlay.CompareTo(y.orderOfPlay);
		}
	}
	#endregion
}

public static class ActionSystemExtensions {
	public static void Perform (this IContainer game, GameAction action) {
		var actionSystem = game.GetAspect<ActionSystem> ();
		actionSystem.Perform (action);
	}

	public static void AddReaction (this IContainer game, GameAction action) {
		var actionSystem = game.GetAspect<ActionSystem> ();
		actionSystem.AddReaction (action);
	}
}