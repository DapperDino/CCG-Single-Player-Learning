using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public class AllTarget : Aspect, ITargetSelector {
	public Mark mark;

	public List<Card> SelectTargets (IContainer game) {
		var result = new List<Card> ();
		var system = game.GetAspect<TargetSystem> ();
		var card = (container as Ability).card;
		var marks = system.GetMarks (card, mark);
		result.AddRange (marks);
		return result;
	}
}
