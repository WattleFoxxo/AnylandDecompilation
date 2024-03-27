using System;
using UnityEngine;

namespace Steamworks
{
	// Token: 0x020002BB RID: 699
	public static class CallbackDispatcher
	{
		// Token: 0x06000C3C RID: 3132 RVA: 0x0000BB9D File Offset: 0x00009D9D
		public static void ExceptionHandler(Exception e)
		{
			Debug.LogException(e);
		}
	}
}
