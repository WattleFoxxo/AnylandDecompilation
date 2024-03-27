using System;
using UnityEngine;

// Token: 0x0200005D RID: 93
public static class GameObjectExtensions
{
	// Token: 0x06000317 RID: 791 RVA: 0x0000C931 File Offset: 0x0000AD31
	public static bool GetActive(this GameObject target)
	{
		return target.activeInHierarchy;
	}
}
