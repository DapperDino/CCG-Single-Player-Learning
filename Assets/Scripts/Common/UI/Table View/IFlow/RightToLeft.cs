using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TheLiquidFire.DataTypes;

namespace TheLiquidFire.UI
{
	public class RightToLeft : IFlow 
	{
		ScrollRect ScrollRect;
		ISpacer Spacer;
		int ViewWidth;

		public RightToLeft (ScrollRect scrollRect, ISpacer spacer)
		{
			ScrollRect = scrollRect;
			Spacer = spacer;
			RectTransform rt = scrollRect.transform as RectTransform;
			ViewWidth = Mathf.CeilToInt(rt.sizeDelta.x);
		}

		public void ConfigureCell (TableViewCell cell, int index)
		{
			RectTransform rt = cell.transform as RectTransform;
			rt.anchorMin = new Vector2(1, 0);
			rt.anchorMax = new Vector2(1, 1);
			rt.pivot = new Vector2(1, 0);

			int width = Spacer.GetSize(index);
			rt.sizeDelta = new Vector2( width, 0 );

			cell.SetShowAnchors(TextAnchor.LowerRight, TextAnchor.LowerRight);
			cell.SetHideAnchors(TextAnchor.UpperRight, TextAnchor.LowerRight);
		}

		public Point GetVisibleCellRange ()
		{
			int startY = Mathf.RoundToInt(ScrollRect.content.anchoredPosition.x);
			int endY = startY + ViewWidth;
			return Spacer.GetVisibleCellRange(startY, endY);
		}

		public Vector2 GetCellOffset (int index)
		{
			return GetCellOffset(index, 0);
		}

		public Vector2 GetCellOffset (int index, int padding)
		{
			int position = Spacer.GetPosition(index) + padding;
			return new Vector2(-position, 0);
		}
	}
}
