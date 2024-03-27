using System;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class FadeOut : MonoBehaviour
{
	// Token: 0x06000D52 RID: 3410 RVA: 0x00077863 File Offset: 0x00075C63
	private void Start()
	{
		base.Invoke("DestroyMe", this.seconds);
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x00077876 File Offset: 0x00075C76
	private void DestroyMe()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04000EEB RID: 3819
	public float seconds = 5f;
}
