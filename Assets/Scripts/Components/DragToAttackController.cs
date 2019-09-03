using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TheLiquidFire.AspectContainer;

public class DragToAttackController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	IContainer gameContainer;
	StateMachine gameStateMachine;
	Container container;
	StateMachine stateMachine;
	Card attacker;
	Card defender;

	void Awake () {
		gameContainer = GetComponentInParent<GameViewSystem> ().container;
		gameStateMachine = gameContainer.GetAspect<StateMachine> ();

		container = new Container ();
		stateMachine = container.AddAspect<StateMachine> ();
		container.AddAspect (new WaitingForInputState ()).owner = this;
		container.AddAspect (new FinishInputState ()).owner = this;
		container.AddAspect (new CompleteState ()).owner = this;
		stateMachine.ChangeState<WaitingForInputState> ();
	}

	#region Drag Handlers
	public void OnBeginDrag (PointerEventData eventData) {
		var handler = stateMachine.currentState as IBeginDragHandler;
		if (handler != null)
			handler.OnBeginDrag (eventData);
	}

	public void OnDrag (PointerEventData eventData) {
		var handler = stateMachine.currentState as IDragHandler;
		if (handler != null)
			handler.OnDrag (eventData);
	}

	public void OnEndDrag (PointerEventData eventData) {
		var handler = stateMachine.currentState as IEndDragHandler;
		if (handler != null)
			handler.OnEndDrag (eventData);
	}
	#endregion

	#region Controller States
	private abstract class BaseControllerState : BaseState {
		public DragToAttackController owner;
	}

	private class WaitingForInputState : BaseControllerState, IBeginDragHandler {
		public void OnBeginDrag (PointerEventData eventData) {
			if (!(owner.gameStateMachine.currentState is PlayerIdleState))
				return;

			var press = eventData.rawPointerPress;
			var view = (press != null) ? press.GetComponentInParent<BattlefieldCardView> () : null;
			if (view == null)
				return;

			owner.attacker = view.card;
			owner.gameStateMachine.ChangeState<PlayerInputState> ();
			owner.stateMachine.ChangeState<FinishInputState> ();
		}
	}

	private class FinishInputState : BaseControllerState, IEndDragHandler {
		public void OnEndDrag (PointerEventData eventData) {
			var hover = eventData.pointerCurrentRaycast.gameObject;
			var view = (hover != null) ? hover.GetComponentInParent<BattlefieldCardView> () : null;
			if (view != null)
				owner.defender = view.card;
			owner.stateMachine.ChangeState<CompleteState> ();				
		}
	}

	private class CompleteState : BaseControllerState {
		public override void Enter () {
			if (owner.attacker != null && owner.defender != null) {
				var action = new AttackAction (owner.attacker, owner.defender);
				owner.gameContainer.Perform (action);
			}

			if (!owner.gameContainer.GetAspect<ActionSystem>().IsActive)
				owner.gameStateMachine.ChangeState<PlayerIdleState> ();

			owner.attacker = owner.defender = null;
			owner.stateMachine.ChangeState<WaitingForInputState> ();
		}
	}
	#endregion
}