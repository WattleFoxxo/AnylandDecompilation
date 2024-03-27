using System;
using UnityEngine;

// Token: 0x020000ED RID: 237
public class CommandArgsProcessor : MonoBehaviour
{
	// Token: 0x060007DF RID: 2015 RVA: 0x0002B975 File Offset: 0x00029D75
	private void Awake()
	{
		if (Misc.ShouldDisableVR())
		{
			SteamVR.enabled = false;
			Log.Info("Dissabling VR", false);
		}
	}
}
