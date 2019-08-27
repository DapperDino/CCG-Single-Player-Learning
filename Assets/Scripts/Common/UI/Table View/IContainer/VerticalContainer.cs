using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TheLiquidFire.UI
{
	public class VerticalContainer : IContainer 
	{
		public IFlow Flow { get; private set; }
		ScrollRect ScrollRect;
		ISpacer Spacer;

		public VerticalContainer (ScrollRect scrollRect, ISpacer spacer)
		{
			ScrollRect = scrollRect;
			Spacer = spacer;

			if (ScrollRect.content.anchorMax.y > 0.5f)
				Flow = new TopToBottom(scrollRect, spacer);
			else
				Flow = new BottomToTop(scrollRect, spacer);
		}

		public void AutoSize ()
		{
			Vector2 contentSize = ScrollRect.content.sizeDelta;
			contentSize.y = Spacer.TotalSize;
			ScrollRect.content.sizeDelta = contentSize;
		}
	}
}