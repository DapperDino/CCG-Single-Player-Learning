using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLiquidFire.AspectContainer
{
	public class StateMachine : Aspect {

		public IState currentState { get; private set; }
		public IState previousState { get; private set; }

		public void ChangeState<T> () where T : class, IState, new () {
			IState fromState = currentState;
			T toState = container.GetAspect<T> () ?? container.AddAspect<T> ();

			if (fromState != null) {
				if (fromState == toState || fromState.CanTransition(toState) == false)
					return;
				fromState.Exit ();
			}

			currentState = toState;
			previousState = fromState;
			toState.Enter ();
		}
	}

	public static class StateMachineExtensions {
		public static void ChangeState<T> (this IContainer game) where T : class, IState, new () {
			var stateMachine = game.GetAspect<StateMachine> ();
			stateMachine.ChangeState<T> ();
		}
	}
}