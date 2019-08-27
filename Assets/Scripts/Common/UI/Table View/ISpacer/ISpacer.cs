using UnityEngine;
using System.Collections;
using TheLiquidFire.DataTypes;

namespace TheLiquidFire.UI
{
	public interface ISpacer
	{
		int TotalSize { get; }
		int GetSize (int index);
		int GetPosition (int index);
		void Insert (int index);
		void Remove (int index);
		Point GetVisibleCellRange (int screenStart, int screenEnd);
	}
}