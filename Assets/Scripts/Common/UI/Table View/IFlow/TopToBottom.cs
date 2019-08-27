using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TheLiquidFire.DataTypes;

namespace TheLiquidFire.UI
{
	public class TopToBottom : IFlow 
	{
		ScrollRect ScrollRect;
		ISpacer Spacer;
		int ViewHeight;

		public TopToBottom (ScrollRect scrollRect, ISpacer spacer)
		{
			ScrollRect = scrollRect;
			Spacer = spacer;
			RectTransform rt = scrollRect.transform as RectTransform;
			ViewHeight = Mathf.CeilToInt(rt.sizeDelta.y);
		}

		public void ConfigureCell (TableViewCell cell, int index)
		{
			RectTransform rt = cell.transform as RectTransform;
			rt.anchorMin = new Vector2(0, 1);
			rt.anchorMax = new Vector2(1, 1);
			rt.pivot = new Vector2(0, 1);

			int height = Spacer.GetSize(index);
			rt.sizeDelta = new Vector2( 0, height );

			cell.SetShowAnchors(TextAnchor.UpperLeft, TextAnchor.UpperLeft);
			cell.SetHideAnchors(TextAnchor.UpperLeft, TextAnchor.UpperRight);
		}

		public Point GetVisibleCellRange ()
		{
			int startY = Mathf.RoundToInt(ScrollRect.content.anchoredPosition.y);
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
			return new Vector2(0, -position);
		}
	}
}