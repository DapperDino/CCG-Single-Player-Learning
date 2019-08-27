using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TheLiquidFire.UI
{
	public abstract class BaseTransition : MonoBehaviour {
		public abstract void Show (Action didShow = null);
		public abstract void Hide (Action didHide = null);
	}
}