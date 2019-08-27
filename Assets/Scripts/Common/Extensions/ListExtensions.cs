using UnityEngine;
using System.Collections.Generic;

namespace TheLiquidFire.Extensions
{
	public static class ListExtensions
	{
		public static T Random<T> (this List<T> list)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			return list[index];
		}

		public static T First<T> (this List<T> list)
		{
			return list[0];
		}

		public static T Last<T> (this List<T> list)
		{
			return list[ list.Count - 1 ];
		}
	}
}