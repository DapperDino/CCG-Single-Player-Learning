using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.DataTypes;
using TheLiquidFire.Extensions;

namespace TheLiquidFire.UI
{
	public class NonUniformSpacer : ISpacer 
	{
		public int TotalSize { get; private set; }
		List<int> Sizes;
		List<int> Positions;
		Func<int, int> SizeForIndex;
		
		public NonUniformSpacer (Func<int, int> sizeForIndex, int cellCount)
		{
			Sizes = new List<int>(cellCount);
			Positions = new List<int>(cellCount);
			SizeForIndex = sizeForIndex;

			TotalSize = 0;
			for (int i = 0; i < cellCount; ++i)
			{
				int size = SizeForIndex(i);
				Sizes.Add(size);
				Positions.Add(TotalSize);
				TotalSize += size;
			}
		}

		public int GetSize (int index)
		{
			return Sizes[index];	
		}

		public int GetPosition (int index)
		{
			return Positions[index];	
		}

		public void Insert (int index)
		{
			int size = SizeForIndex(index);
			TotalSize += size;

			if (Sizes.Count == index)
			{
				if (Positions.Count == 0)
					Positions.Add(0);
				else
					Positions.Add((Positions.Last() + Sizes.Last()));
				Sizes.Add(size);
			}
			else
			{
				Sizes.Insert(index, size);
				Positions.Insert(index, Positions[index]);

				for (int i = index + 1; i < Positions.Count; ++i)
					Positions[i] += size;
			}
		}

		public void Remove (int index)
		{
			int size = Sizes[index];
			TotalSize -= size;
			Sizes.RemoveAt(index);
			Positions.RemoveAt(index);

			for (int i = index; i < Positions.Count; ++i)
				Positions[i] -= size;
		}

		public Point GetVisibleCellRange (int screenStart, int screenEnd)
		{
			Point range = new Point(int.MaxValue, int.MinValue);
			if (Sizes.Count == 0)
				return range;

			for (int i = 0; i < Sizes.Count; ++i)
			{
				int size = Sizes[i];
				int position = Positions[i];
				bool isVisible = !(position + size < screenStart || position > screenEnd);
				if (isVisible)
				{
					range.x = Mathf.Min(range.x, i);
					range.y = i;
				}
				if (position + size > screenEnd)
					break;
			}

			return range;
		}
	}
}