using System;
using UnityEngine;

// Token: 0x020001AB RID: 427
public class CookieLight : MonoBehaviour
{
	// Token: 0x06000D35 RID: 3381 RVA: 0x000769D5 File Offset: 0x00074DD5
	private void Start()
	{
		this.light = base.GetComponent<Light>();
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x000769E4 File Offset: 0x00074DE4
	private void Update()
	{
		float num = (this.sunUpSphere.position.y - this.sunUpSphere.parent.position.y) * 20f;
		this.light.intensity = Mathf.Clamp(num, 0f, 0.75f);
	}

	// Token: 0x04000ED4 RID: 3796
	public Transform sunUpSphere;

	// Token: 0x04000ED5 RID: 3797
	private Light light;
}
