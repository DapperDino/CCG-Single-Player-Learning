using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TheLiquidFire.AspectContainer;

public class StateMachineTests {

	private class FooState : BaseState {
		public bool calledEnter;
		public bool calledExit;
		public bool isLocked;

		public override void Enter () {
			calledEnter = true;
		}

		public override void Exit () {
			calledExit = true;
		}

		public override bool CanTransition (IState other) {
			return !isLocked;
		}
	}

	private class BarState : BaseState {
		
	}

	IContainer container;
	StateMachine stateMachine;


	[SetUp]
	public void TestSetup() {
		container = new Container ();
		stateMachine = container.AddAspect<StateMachine> ();
	}

	[Test]
	public void CanTransitionToExistingState() {
		var fooState = container.AddAspect<FooState> ();
		stateMachine.ChangeState<FooState> ();
		Assert.AreSame (stateMachine.currentState, fooState);
	}

	[Test]
	public void TransitionCanAddsStateIfNeeded() {
		stateMachine.ChangeState<FooState> ();
		Assert.IsTrue (stateMachine.currentState is FooState);
	}

	[Test]
	public void TransitionCallsEnterOnNewState() {
		stateMachine.ChangeState<FooState> ();
		var state = stateMachine.currentState as FooState;
		Assert.IsTrue (state.calledEnter);
	}

	[Test]
	public void TransitionCallsExitOnOldState() {
		stateMachine.ChangeState<FooState> ();
		var state = stateMachine.currentState as FooState;
		stateMachine.ChangeState<BarState> ();
		Assert.IsTrue (state.calledExit);
	}

	[Test]
	public void StateMachineTracksPreviousState() {
		stateMachine.ChangeState<FooState> ();
		stateMachine.ChangeState<BarState> ();
		Assert.IsNotNull (stateMachine.previousState);
	}

	[Test]
	public void CanTransitionCanBlockStateChange() {
		var fooState = container.AddAspect<FooState> ();
		stateMachine.ChangeState<FooState> ();
		fooState.isLocked = true;
		stateMachine.ChangeState<BarState> ();
		Assert.AreSame (stateMachine.currentState, fooState);
	}

	[Test]
	public void IgnoresTransitionToSameState() {
		var fooState = container.AddAspect<FooState> ();
		stateMachine.ChangeState<FooState> ();
		fooState.calledEnter = false;
		stateMachine.ChangeState<FooState> ();
		Assert.IsFalse (fooState.calledEnter);
	}
}
