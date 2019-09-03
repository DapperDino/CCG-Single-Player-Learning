using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;
using TheLiquidFire.Extensions;

public class RandomTarget : Aspect, ITargetSelector {
	public Mark mark;
	public int count = 1;

	public List<Card> SelectTargets (IContainer game) {
		var result = new List<Card> ();
		var system = game.GetAspect<TargetSystem> ();
		var card = (container as Ability).card;
		var marks = system.GetMarks (card, mark);
		if (marks.Count == 0)
			return result;
		for (int i = 0; i < count; ++i) {
			result.Add (marks.Random ());
		}
		return result;
	}
}
