using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Notifications;

public class SpellSystem : Aspect, IObserve {

	public void Awake () {
		this.AddObserver (OnPeformPlayCard, Global.PerformNotification<PlayCardAction> (), container);
		this.AddObserver (OnPrepareCastSpell, Global.PrepareNotification<CastSpellAction> (), container);
	}

	public void Destroy () {
		this.RemoveObserver (OnPeformPlayCard, Global.PerformNotification<PlayCardAction> (), container);
		this.RemoveObserver (OnPrepareCastSpell, Global.PrepareNotification<CastSpellAction> (), container);
	}

	void OnPeformPlayCard (object sender, object args) {
		var action = args as PlayCardAction;
		var spell = action.card as Spell;
		if (spell != null) {
			var reaction = new CastSpellAction (spell);
			container.AddReaction (reaction);
		}
	}

	void OnPrepareCastSpell (object sender, object args) {
		var action = args as CastSpellAction;
		var ability = action.spell.GetAspect<Ability> ();
		var reaction = new AbilityAction (ability);
		container.AddReaction (reaction);
	}
}
