using System;
using System.Collections.Generic;

namespace DaikonForge.VoIP
{
	// Token: 0x02000021 RID: 33
	public class TempArray<T>
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x000051D0 File Offset: 0x000035D0
		public static T[] Obtain(int length)
		{
			Queue<T[]> queue = TempArray<T>.getPool(length);
			if (queue.Count > 0)
			{
				return queue.Dequeue();
			}
			return new T[length];
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000051FD File Offset: 0x000035FD
		public static void Release(T[] array)
		{
			TempArray<T>.pool[array.Length].Enqueue(array);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005212 File Offset: 0x00003612
		private static Queue<T[]> getPool(int length)
		{
			if (!TempArray<T>.pool.ContainsKey(length))
			{
				TempArray<T>.pool[length] = new Queue<T[]>();
			}
			return TempArray<T>.pool[length];
		}

		// Token: 0x04000071 RID: 113
		private static Dictionary<int, Queue<T[]>> pool = new Dictionary<int, Queue<T[]>>();
	}
}
