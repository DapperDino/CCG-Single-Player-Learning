using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLiquidFire.AspectContainer
{
	public interface IAspect {
		IContainer container { get; set; }
	}

	public class Aspect : IAspect {
		public IContainer container { get; set; }
	}
}