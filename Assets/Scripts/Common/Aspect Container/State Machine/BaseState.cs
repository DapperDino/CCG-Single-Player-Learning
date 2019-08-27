using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLiquidFire.AspectContainer
{
	public interface IState : IAspect {
		void Enter ();
		bool CanTransition (IState other);
		void Exit ();
	}

	public abstract class BaseState : Aspect, IState {
		public virtual void Enter () {}
		public virtual bool CanTransition (IState other) { return true; }
		public virtual void Exit () {}
	}
}