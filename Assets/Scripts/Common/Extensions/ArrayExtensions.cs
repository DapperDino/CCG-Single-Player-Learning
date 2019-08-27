using UnityEngine;
using System;
using System.Collections;

namespace TheLiquidFire.Extensions
{
	public static class ArrayExtensions 
	{
		public static T Random<T> (this T[] array)
		{
			int index = UnityEngine.Random.Range(0, array.Length);
			return array[index];
		}

		public static T First<T> (this T[] array)
		{
			return array[0];
		}

		public static T Last<T> (this T[] array)
		{
			return array[ array.Length - 1 ];
		}

		public static bool Matches<T> (this T[] array1, T[] array2)
		{
			if (array1 == null || array2 == null)
				return false;
			if (array1.Length != array2.Length)
				return false;

			for (int i = 0; i < array1.Length; ++i)
			{
				if (!array1[i].Equals(array2[i]))
					return false;
			}

			return true;
		}
	}
}