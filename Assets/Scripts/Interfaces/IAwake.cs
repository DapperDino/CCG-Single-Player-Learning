using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLiquidFire.AspectContainer;

public interface IAwake {
	void Awake();
}

public static class AwakeExtensions {
	public static void Awake (this IContainer container) {
		foreach (IAspect aspect in container.Aspects()) {
			var item = aspect as IAwake;
			if (item != null)
				item.Awake ();
		}
	}
}