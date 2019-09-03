using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class ActionSystemTests {

	private class TestAction: GameAction {
		public bool didPrepare;
		public bool didPerform;
	}

	private class NotificationMarks {
		public bool sequenceBegin;
		public bool sequenceEnd;
		public bool complete;
		public bool prepare;
		public bool perform;
		public bool deathReaper;
	}

	private class TestSystem: Aspect {
		public const int rootActionOrder = 0;
		public const int depthCheckPriority = 1;
		public const int depthReactionOrder = int.MinValue;
		public NotificationMarks actionMarks = new NotificationMarks ();
		public NotificationMarks reactionMarks = new NotificationMarks ();
		public List<TestAction> reactions = new List<TestAction> ();
		public bool loopedDeath;
		public bool depthFirst;

		public void Setup() {
			this.AddObserver (OnSequenceBegin, ActionSystem.beginSequenceNotification);
			this.AddObserver (OnSequenceEnd, ActionSystem.endSequenceNotification);
			this.AddObserver (OnComplete, ActionSystem.completeNotification);
			this.AddObserver (OnPrepare, Global.PrepareNotification<TestAction> ());
			this.AddObserver (OnPerform, Global.PerformNotification<TestAction> ());
			this.AddObserver (OnDeath, ActionSystem.deathReaperNotification);
		}

		public void TearDown() {
			this.RemoveObserver (OnSequenceBegin, ActionSystem.beginSequenceNotification);
			this.RemoveObserver (OnSequenceEnd, ActionSystem.endSequenceNotification);
			this.RemoveObserver (OnComplete, ActionSystem.completeNotification);
			this.RemoveObserver (OnPrepare, Global.PrepareNotification<TestAction> ());
			this.RemoveObserver (OnPerform, Global.PerformNotification<TestAction> ());
			this.RemoveObserver (OnDeath, ActionSystem.deathReaperNotification);
		}

		void OnSequenceBegin (object sender, object args) {
			var action = args as TestAction;
			var marks = action.orderOfPlay == rootActionOrder ? actionMarks : reactionMarks;
			marks.sequenceBegin = true;

			action.prepare.viewer = testViewer;
			action.perform.viewer = testViewer;
		}

		void OnSequenceEnd (object sender, object args) {
			var action = args as TestAction;
			var marks = action.orderOfPlay == rootActionOrder ? actionMarks : reactionMarks;
			marks.sequenceEnd = true;
		}

		void OnComplete (object sender, object args) {
			actionMarks.complete = true;
		}

		void OnPrepare (object sender, object args) {
			var action = args as TestAction;
			var marks = action.orderOfPlay == rootActionOrder ? actionMarks : reactionMarks;
			marks.prepare = true;
			action.didPrepare = true;
		}

		void OnPerform (object sender, object args) {
			var action = args as TestAction;
			var marks = action.orderOfPlay == rootActionOrder ? actionMarks : reactionMarks;
			marks.perform = true;
			action.didPerform = true;

			if (action.orderOfPlay != rootActionOrder) {
				reactions.Add (action);
			} else {
				AddReactions ((IContainer)sender);
			}
			if (action.priority == depthCheckPriority) {
				var reaction = new TestAction ();
				reaction.orderOfPlay = depthReactionOrder;
				((IContainer)sender).GetAspect<ActionSystem> ().AddReaction (reaction);
			}
			if (action.orderOfPlay == depthReactionOrder) {
				// Expect '2'.  The trigger action and child action, 
				// Any greater value indicates that a sibling action of the trigger action responded first
				depthFirst = (reactions.Count == 2); 
			}
		}

		void OnDeath (object sender, object args) {
			var action = args as TestAction;
			var marks = action.orderOfPlay == rootActionOrder ? actionMarks : reactionMarks;

			if (actionMarks.deathReaper == false) {
				var reaction = new TestAction ();
				reaction.orderOfPlay = int.MaxValue;
				((ActionSystem)sender).AddReaction (reaction);
			} else {
				loopedDeath = true;
			}

			marks.deathReaper = true;
		}

		IEnumerator testViewer (IContainer game, GameAction action) {
			yield return null;
			yield return true;
			yield return null;
		}

		void AddReactions (IContainer game) {
			for (int i = 0; i < 5; ++i) {
				var reaction = new TestAction ();
				reaction.orderOfPlay = UnityEngine.Random.Range (1, 100);
				if (i == 2)
					reaction.priority = depthCheckPriority;
				game.GetAspect<ActionSystem> ().AddReaction (reaction);
			}
		}
	}

	IContainer game;
	ActionSystem actionSystem;
	TestSystem testSystem;

	[SetUp]
	public void TestSetup() {
		NotificationCenter.instance.Clean ();
		game = new Container ();
		actionSystem = game.AddAspect<ActionSystem> ();
		testSystem = game.AddAspect<TestSystem> ();
		testSystem.Setup ();
	}

	[TearDown]
	public void TestTearDown() {
		testSystem.TearDown ();
	}

	void RunToCompletion() {
		var timeOut = 0;
		while (actionSystem.IsActive && timeOut < 1000) {
			timeOut++;
			actionSystem.Update ();
		}
	}

	[Test]
	public void testActionSystemTracksActiveState() {
		actionSystem.Perform (new TestAction ());
		Assert.IsTrue (actionSystem.IsActive);
		RunToCompletion ();
		Assert.IsFalse (actionSystem.IsActive);
	}

	[Test]
	public void testActionNotifications() {
		actionSystem.Perform (new TestAction ());
		RunToCompletion ();
		var m = testSystem.actionMarks;
		var result = m.sequenceBegin &&
		             m.sequenceEnd &&
		             m.complete &&
		             m.prepare &&
		             m.perform &&
		             m.deathReaper;
		Assert.IsTrue (result);
	}

	[Test]
	public void testReactionNotifications() {
		actionSystem.Perform (new TestAction ());
		RunToCompletion ();
		var m = testSystem.reactionMarks;
		var result = m.sequenceBegin && 
			m.sequenceEnd && 
			m.prepare && 
			m.perform && 
			!m.complete && 
			!m.deathReaper;
		Assert.IsTrue (result);
	}

	[Test]
	public void testReactionsAreSorted() {
		// Test by priority first, then order of play
		actionSystem.Perform (new TestAction ());
		RunToCompletion ();
		int priority = int.MaxValue;
		int orderOfPlay = int.MinValue;
		for (int i = 0; i < testSystem.reactions.Count; ++i) {			
			var reaction = testSystem.reactions [i];
//			Debug.Log (string.Format("p: {0} o: {1}", reaction.priority, reaction.orderOfPlay));
			Assert.LessOrEqual (reaction.priority, priority);
			if (reaction.priority != priority) {
				priority = reaction.priority;
				orderOfPlay = int.MinValue;
			}
			Assert.GreaterOrEqual (reaction.orderOfPlay, orderOfPlay);
			orderOfPlay = reaction.orderOfPlay;
		}
	}

	[Test]
	public void testCancelAction() {
		var action = new TestAction ();
		action.Cancel ();
		actionSystem.Perform (action);
		RunToCompletion ();
		Assert.IsFalse (action.didPrepare);
		Assert.IsFalse (action.didPerform);
	}

	[Test]
	public void testLoopableDeathPhase() {
		actionSystem.Perform (new TestAction ());
		RunToCompletion ();
		Assert.IsTrue (testSystem.loopedDeath);
	}

	[Test]
	public void testDepthFirstReactions() {
		actionSystem.Perform (new TestAction ());
		RunToCompletion ();
		Assert.IsTrue (testSystem.depthFirst);
	}

	[Test]
	public void testActionTypesHaveUniqueIDs() {
		var id1 = new GameAction ().id;
		var id2 = new TestAction ().id;
		Assert.AreNotEqual (id1, id2);
	}
}