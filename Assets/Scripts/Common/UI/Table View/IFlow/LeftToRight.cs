using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TheLiquidFire.DataTypes;

namespace TheLiquidFire.UI
{
	public class LeftToRight : IFlow 
	{
		ScrollRect ScrollRect;
		ISpacer Spacer;
		int ViewWidth;

		public LeftToRight (ScrollRect scrollRect, ISpacer spacer)
		{
			ScrollRect = scrollRect;
			Spacer = spacer;
			RectTransform rt = scrollRect.transform as RectTransform;
			ViewWidth = Mathf.CeilToInt(rt.sizeDelta.x);
		}

		public void ConfigureCell (TableViewCell cell, int index)
		{
			RectTransform rt = cell.transform as RectTransform;
			rt.anchorMin = new Vector2(0, 0);
			rt.anchorMax = new Vector2(0, 1);
			rt.pivot = new Vector2(0, 0);

			int width = Spacer.GetSize(index);
			rt.sizeDelta = new Vector2( width, 0 );

			cell.SetShowAnchors(TextAnchor.LowerLeft, TextAnchor.LowerLeft);
			cell.SetHideAnchors(TextAnchor.UpperLeft, TextAnchor.LowerLeft);
		}

		public Point GetVisibleCellRange ()
		{
			int startY = -Mathf.RoundToInt(ScrollRect.content.anchoredPosition.x);
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
			return new Vector2(position, 0);
		}
	}
}