using UnityEngine;
using System.Collections;
using TheLiquidFire.DataTypes;

namespace TheLiquidFire.UI
{
	public class UniformSpacer : ISpacer
	{
		public int TotalSize
		{
			get
			{
				return CellCount * CellSize;
			}
		}

		int CellSize;
		int CellCount;

		public UniformSpacer (int cellSize, int cellCount)
		{
			CellSize = cellSize;
			CellCount = cellCount;
		}

		public int GetSize (int index)
		{
			return CellSize;
		}

		public int GetPosition (int index)
		{
			return index * CellSize;	
		}

		public void Insert (int index)
		{
			CellCount++;
		}

		public void Remove (int index)
		{
			CellCount--;
		}

		public Point GetVisibleCellRange (int screenStart, int screenEnd)
		{
			Point range = new Point(int.MaxValue, int.MinValue);
			if (CellCount == 0)
				return range;

			range.x = Mathf.Max( (screenStart / CellSize), 0 );
			range.y = Mathf.Min( (screenEnd / CellSize), CellCount - 1 );
			return range;
		}
	}
}