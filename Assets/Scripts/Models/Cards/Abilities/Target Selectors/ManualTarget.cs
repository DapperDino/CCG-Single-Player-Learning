using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public class ManualTarget : Aspect, ITargetSelector {
	public List<Card> SelectTargets (IContainer game) {
		var card = (container as Ability).card;
		var target = card.GetAspect<Target> ();
		var result = new List<Card> ();
		result.Add (target.selected);
		return result;
	}
}
