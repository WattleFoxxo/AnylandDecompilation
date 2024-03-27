using System;
using UnityEngine;

// Token: 0x020000E7 RID: 231
public class HeadLight : MonoBehaviour
{
	// Token: 0x060007BC RID: 1980 RVA: 0x0002AF10 File Offset: 0x00029310
	private void Start()
	{
		this.light = base.GetComponent<Light>();
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x0002AF20 File Offset: 0x00029320
	private void Update()
	{
		base.transform.position = this.headCore.position;
		base.transform.Translate(0.15f * Vector3.up, Space.World);
		base.transform.Translate(0.25f * this.headCore.forward, Space.World);
		float num = (this.sunUpSphere.position.y - this.sunUpSphere.parent.position.y) * 20f;
		float num2 = 0.5f;
		if (num < 0f && Our.mode != EditModes.None)
		{
			num2 += Mathf.Abs(num) * 0.35f;
		}
		this.light.intensity = num2;
	}

	// Token: 0x040005B5 RID: 1461
	public Transform headCore;

	// Token: 0x040005B6 RID: 1462
	public Transform sunUpSphere;

	// Token: 0x040005B7 RID: 1463
	private Light light;
}
