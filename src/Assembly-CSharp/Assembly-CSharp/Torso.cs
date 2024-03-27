using System;
using UnityEngine;

// Token: 0x020000EB RID: 235
public class Torso : MonoBehaviour
{
	// Token: 0x060007D4 RID: 2004 RVA: 0x0002B5A1 File Offset: 0x000299A1
	private void Update()
	{
		this.AdjustBasedOnHeadCore();
	}

	// Token: 0x060007D5 RID: 2005 RVA: 0x0002B5AC File Offset: 0x000299AC
	private void AdjustBasedOnHeadCore()
	{
		if (this.headCore != null)
		{
			base.transform.position = this.headCore.transform.position;
			float num = 1f;
			float num2 = Mathf.Abs(base.transform.localEulerAngles.y - this.headCore.transform.localEulerAngles.y);
			if (num2 > 45f)
			{
				num += (num2 - 45f) / 50f;
			}
			float num3 = Mathf.LerpAngle(base.transform.localEulerAngles.y, this.headCore.transform.localEulerAngles.y, num * Time.deltaTime);
			base.transform.localEulerAngles = new Vector3(0f, num3, 0f);
		}
	}

	// Token: 0x040005BF RID: 1471
	public GameObject headCore;
}
