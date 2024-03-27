using System;
using UnityEngine;

// Token: 0x020001DD RID: 477
public class JoystickLine : MonoBehaviour
{
	// Token: 0x06000EEE RID: 3822 RVA: 0x00083545 File Offset: 0x00081945
	private void Start()
	{
		this.line = base.gameObject.GetComponent<LineRenderer>();
		this.dialogFundament = base.transform.parent.parent;
	}

	// Token: 0x06000EEF RID: 3823 RVA: 0x0008356E File Offset: 0x0008196E
	private void Update()
	{
		this.line.SetPosition(0, base.transform.position);
		this.line.SetPosition(1, this.dialogFundament.position);
	}

	// Token: 0x04000FBA RID: 4026
	private LineRenderer line;

	// Token: 0x04000FBB RID: 4027
	private Transform dialogFundament;
}
