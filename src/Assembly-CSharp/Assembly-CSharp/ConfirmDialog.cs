using System;
using UnityEngine;

// Token: 0x02000109 RID: 265
public class ConfirmDialog : Dialog
{
	// Token: 0x0600097C RID: 2428 RVA: 0x0003D134 File Offset: 0x0003B534
	public void DoStart(string text, Action<bool> _callback)
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		this.callback = _callback;
		text = Misc.WrapWithNewlines(text, 34, -1);
		int num = ((text.Length > 100) ? (-350) : (-100));
		base.AddLabel(text, 0, num, 1f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton("close", null, "cancel", "Button", -235, 380, "cross", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.AddButton("confirm", null, "ok", "Button", 235, 380, "checkmark", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.Invoke("StopHapticPulse", 1f);
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x0003D240 File Offset: 0x0003B640
	private void StopHapticPulse()
	{
		this.hapticPulseOn = false;
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x0003D249 File Offset: 0x0003B649
	private new void Update()
	{
		if (this.hapticPulseOn && this.hand != null)
		{
			this.hand.TriggerHapticPulse(600);
		}
		base.ReactToOnClick();
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x0003D280 File Offset: 0x0003B680
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "confirm"))
			{
				if (contextName == "close")
				{
					this.callback(false);
				}
			}
			else
			{
				Managers.soundManager.Play("success", this.transform, 0.5f, false, false);
				this.callback(true);
			}
		}
	}

	// Token: 0x04000729 RID: 1833
	private Action<bool> callback;

	// Token: 0x0400072A RID: 1834
	private bool hapticPulseOn = true;
}
