using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace TheLiquidFire.Extensions
{
	public static class ObjectExtensions 
	{
		public static byte[] Serialize(this object obj)
		{
			var bf = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}

		public static T Deserialize<T>(this byte[] bytes) where T : class
		{
			T retvalue = null;
			var bf = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				ms.Write(bytes, 0, bytes.Length);
				ms.Seek(0, SeekOrigin.Begin);
				retvalue = (T)bf.Deserialize(ms);
			}
			return retvalue;
		}
	}
}