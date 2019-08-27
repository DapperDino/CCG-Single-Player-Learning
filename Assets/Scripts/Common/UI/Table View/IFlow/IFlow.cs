using UnityEngine;
using System.Collections;
using TheLiquidFire.DataTypes;

namespace TheLiquidFire.UI
{
	public interface IFlow
	{
		void ConfigureCell (TableViewCell cell, int index);
		Point GetVisibleCellRange ();
		Vector2 GetCellOffset (int index);
		Vector2 GetCellOffset (int index, int padding);
	}
}