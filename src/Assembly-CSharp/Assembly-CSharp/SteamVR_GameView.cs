using System;
using UnityEngine;

// Token: 0x0200029D RID: 669
[ExecuteInEditMode]
public class SteamVR_GameView : MonoBehaviour
{
	// Token: 0x06001941 RID: 6465 RVA: 0x000E5617 File Offset: 0x000E3A17
	private void Awake()
	{
		Debug.Log("SteamVR_GameView is deprecated in Unity 5.4 - REMOVING");
		global::UnityEngine.Object.DestroyImmediate(this);
	}
}
