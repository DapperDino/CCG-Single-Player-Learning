using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public interface ITargetSelector : IAspect {
	List<Card> SelectTargets (IContainer game);
}
