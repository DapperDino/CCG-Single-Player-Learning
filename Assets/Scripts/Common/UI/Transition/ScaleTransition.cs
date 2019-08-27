using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TheLiquidFire.Animation;

namespace TheLiquidFire.UI
{
	public class ScaleTransition : BaseTransition {
		[System.Serializable]
		public class Data {
			public Vector3 scale;
			public float duration;
			public Func<float, float, float, float> equation;
		}

		public Data showData = new Data {
			scale = Vector3.one,
			duration = 0.5f,
			equation = EasingEquations.EaseOutBack
		};

		public Data hideData = new Data {
			scale = Vector3.zero,
			duration = 0.5f,
			equation = EasingEquations.EaseInBack
		};

		public override void Show (System.Action didShow) {
			transform.localScale = hideData.scale;
			StartCoroutine (Process (showData, didShow));
		}

		public override void Hide (System.Action didHide) {
			transform.localScale = showData.scale;
			StartCoroutine (Process (hideData, didHide));
		}

		IEnumerator Process (Data data, Action complete) {
			Tweener tweener = transform.ScaleTo (data.scale, data.duration, data.equation);
			while (tweener != null)
				yield return null;
			if (complete != null)
				complete();
		}
	}
}