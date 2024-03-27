using System;
using UnityEngine;

// Token: 0x020001AA RID: 426
public class ChangeSizeOverTime : MonoBehaviour
{
	// Token: 0x06000D33 RID: 3379 RVA: 0x00076938 File Offset: 0x00074D38
	private void Update()
	{
		Vector3 localScale = base.transform.localScale;
		float num = this.growth * Time.deltaTime;
		localScale.x = Mathf.Clamp(localScale.x + num, this.minSize, this.maxSize);
		localScale.y = Mathf.Clamp(localScale.y + num, this.minSize, this.maxSize);
		localScale.z = Mathf.Clamp(localScale.z + num, this.minSize, this.maxSize);
		base.transform.localScale = localScale;
	}

	// Token: 0x04000ED1 RID: 3793
	public float minSize = 0.01f;

	// Token: 0x04000ED2 RID: 3794
	public float maxSize = 1000f;

	// Token: 0x04000ED3 RID: 3795
	public float growth;
}
