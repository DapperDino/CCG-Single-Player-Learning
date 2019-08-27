using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TheLiquidFire.DataTypes;

namespace TheLiquidFire.UI
{
	public class BottomToTop : IFlow 
	{
		ScrollRect ScrollRect;
		ISpacer Spacer;
		int ViewHeight;

		public BottomToTop (ScrollRect scrollRect, ISpacer spacer)
		{
			ScrollRect = scrollRect;
			Spacer = spacer;
			RectTransform rt = scrollRect.transform as RectTransform;
			ViewHeight = Mathf.CeilToInt(rt.sizeDelta.y);
		}

		public void ConfigureCell (TableViewCell cell, int index)
		{
			RectTransform rt = cell.transform as RectTransform;
			rt.anchorMin = new Vector2(0, 0);
			rt.anchorMax = new Vector2(1, 0);
			rt.pivot = new Vector2(0, 0);

			int height = Spacer.GetSize(index);
			rt.sizeDelta = new Vector2( 0, height );

			cell.SetShowAnchors(TextAnchor.LowerLeft, TextAnchor.LowerLeft);
			cell.SetHideAnchors(TextAnchor.LowerLeft, TextAnchor.LowerRight);
		}

		public Point GetVisibleCellRange ()
		{
			int startY = -Mathf.RoundToInt(ScrollRect.content.anchoredPosition.y);
			int endY = startY + ViewHeight;
			return Spacer.GetVisibleCellRange(startY, endY);
		}

		public Vector2 GetCellOffset (int index)
		{
			return GetCellOffset(index, 0);
		}

		public Vector2 GetCellOffset (int index, int padding)
		{
			int position = Spacer.GetPosition(index) + padding;
			return new Vector2(0, position);
		}
	}
}