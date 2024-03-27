using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x020002D2 RID: 722
public static class YoutubePlayerExtensions
{
	// Token: 0x06001AF3 RID: 6899 RVA: 0x000F4218 File Offset: 0x000F2618
	public static T[] Splice<T>(this T[] source, int start)
	{
		List<T> list = source.ToList<T>();
		List<T> list2 = list.Skip(start).ToList<T>();
		return list2.ToArray<T>();
	}
}
