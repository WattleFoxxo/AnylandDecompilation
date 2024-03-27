using System;
using UnityEngine;

// Token: 0x02000292 RID: 658
[ExecuteInEditMode]
public class SteamVR_CameraMask : MonoBehaviour
{
	// Token: 0x060018F0 RID: 6384 RVA: 0x000E327B File Offset: 0x000E167B
	private void Awake()
	{
		Debug.Log("SteamVR_CameraMask is deprecated in Unity 5.4 - REMOVING");
		global::UnityEngine.Object.DestroyImmediate(this);
	}
}
