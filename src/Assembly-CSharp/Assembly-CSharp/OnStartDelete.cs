using System;
using UnityEngine;

// Token: 0x020000BF RID: 191
public class OnStartDelete : MonoBehaviour
{
	// Token: 0x0600065B RID: 1627 RVA: 0x0001DE0C File Offset: 0x0001C20C
	private void Start()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
	}
}
