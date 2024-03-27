using System;
using UnityEngine;

// Token: 0x020002AF RID: 687
[RequireComponent(typeof(GUIText))]
public class SteamVR_StatusText : SteamVR_Status
{
	// Token: 0x060019B6 RID: 6582 RVA: 0x000EA8B0 File Offset: 0x000E8CB0
	private void Awake()
	{
		this.text = base.GetComponent<GUIText>();
		if (this.mode == SteamVR_Status.Mode.WhileTrue || this.mode == SteamVR_Status.Mode.WhileFalse)
		{
			this.timer = this.fade * this.text.color.a;
		}
	}

	// Token: 0x060019B7 RID: 6583 RVA: 0x000EA904 File Offset: 0x000E8D04
	protected override void SetAlpha(float a)
	{
		if (a > 0f)
		{
			this.text.enabled = true;
			this.text.color = new Color(this.text.color.r, this.text.color.g, this.text.color.b, a);
		}
		else
		{
			this.text.enabled = false;
		}
	}

	// Token: 0x04001802 RID: 6146
	private GUIText text;
}
