using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public class Ability : Container, IAspect {
	public IContainer container { get; set; }
	public Card card { get { return container as Card; } }
	public string actionName { get; set; }
	public object userInfo { get; set; }
}