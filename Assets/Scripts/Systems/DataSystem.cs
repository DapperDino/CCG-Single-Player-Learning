using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public class DataSystem : Aspect {
	public Match match = new Match();
}

public static class DataSystemExtensions {
	public static Match GetMatch (this IContainer game) {
		var dataSystem = game.GetAspect<DataSystem> ();
		return dataSystem.match;
	}
}