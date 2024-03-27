using System;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class DestroyMeAfterTime : MonoBehaviour
{
	// Token: 0x06000D38 RID: 3384 RVA: 0x00076A52 File Offset: 0x00074E52
	private void Start()
	{
		base.Invoke("DestroyMe", this.seconds);
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x00076A65 File Offset: 0x00074E65
	private void DestroyMe()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04000ED6 RID: 3798
	public float seconds = 2f;
}
