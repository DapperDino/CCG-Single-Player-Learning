using UnityEngine;
using System.Collections;

namespace TheLiquidFire.UI
{
	public interface IContainer
	{
		IFlow Flow { get; }
		void AutoSize ();
	}
}