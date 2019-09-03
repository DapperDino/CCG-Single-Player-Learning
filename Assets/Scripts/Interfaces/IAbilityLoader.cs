using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public interface IAbilityLoader {
	void Load (IContainer game, Ability ability);
}
