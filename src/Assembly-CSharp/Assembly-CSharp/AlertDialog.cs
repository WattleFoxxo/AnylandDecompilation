using System;
using UnityEngine;

// Token: 0x020000FB RID: 251
public class AlertDialog : Dialog
{
	// Token: 0x060008D8 RID: 2264 RVA: 0x00033B4D File Offset: 0x00031F4D
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		this.fundament = base.AddFundament();
		this.FlipHapticPulse();
	}

	// Token: 0x060008D9 RID: 2265 RVA: 0x00033B70 File Offset: 0x00031F70
	public void SetInfo(string _text)
	{
		this.mode = AlertDialog.Modes.Info;
		this.text = _text;
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00033B80 File Offset: 0x00031F80
	public void SetError(string _text, bool showDefaultIntroText, bool allowClosingInsteadOfRestarting = false)
	{
		this.mode = AlertDialog.Modes.Error;
		this.allowClosingInsteadOfRestarting = allowClosingInsteadOfRestarting;
		if (showDefaultIntroText)
		{
			this.text += "Oops, an error occurred: ";
		}
		this.text += _text;
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x00033BC0 File Offset: 0x00031FC0
	public void SetPing(global::Ping _ping)
	{
		this.ping = _ping;
		this.mode = AlertDialog.Modes.Ping;
		this.text = this.ping.originPersonName + " pinged you from " + this.ping.originAreaName;
		this.originAreaName = this.ping.originAreaName;
	}

	// Token: 0x060008DC RID: 2268 RVA: 0x00033C12 File Offset: 0x00032012
	public void SetNewPersonBorn(string originPersonName, string _originAreaName)
	{
		this.mode = AlertDialog.Modes.NewPersonBorn;
		this.text = originPersonName + " is born into the universe";
		this.originAreaName = _originAreaName;
	}

	// Token: 0x060008DD RID: 2269 RVA: 0x00033C34 File Offset: 0x00032034
	private void AddTextIfNeeded()
	{
		if (!this.didAddText)
		{
			this.didAddText = true;
			this.text = this.text.ToUpper();
			int num = ((this.align != TextAlignment.Left) ? 0 : (-450));
			int num2 = -300;
			if (this.autoAddNewlines)
			{
				string[] stringInParts = this.text.GetStringInParts(38, -1);
				foreach (string text in stringInParts)
				{
					string text2 = text;
					int num3 = num;
					int num4 = num2;
					float num5 = this.textSizeFactor;
					TextColor textColor = this.textColor;
					base.AddLabel(text2, num3, num4, num5, false, textColor, false, this.align, -1, 1f, false, TextAnchor.MiddleLeft);
					num2 += 60;
				}
			}
			else
			{
				string text2 = this.text;
				int num4 = num;
				int num3 = num2;
				float num5 = this.textSizeFactor;
				TextColor textColor = this.textColor;
				base.AddLabel(text2, num4, num3, num5, false, textColor, false, this.align, -1, 1f, false, TextAnchor.MiddleLeft);
			}
			switch (this.mode)
			{
			case AlertDialog.Modes.Info:
				if (this.maxBuzzes == -1)
				{
					this.maxBuzzes = 3;
				}
				if (this.showOkButton)
				{
					base.AddButton("close", null, "     ok", "ButtonCompact", 0, 410, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
				else
				{
					base.AddCloseButton();
				}
				break;
			case AlertDialog.Modes.Error:
				Our.SetMode(EditModes.None, false);
				base.SetFundamentColor(new Color(1f, 0.7f, 0.7f));
				base.AddButton("restart", null, "restart", "Button", 0, 220, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				if (this.allowClosingInsteadOfRestarting)
				{
					base.AddCloseButton();
				}
				break;
			case AlertDialog.Modes.Ping:
				this.maxBuzzes = 3;
				base.AddCloseButton();
				Managers.soundManager.Play("ping", this.transform, 1f, false, false);
				base.AddButton("teleport", null, "go there", "Button", 0, 220, "teleportTo", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				break;
			case AlertDialog.Modes.NewPersonBorn:
				this.maxBuzzes = 2;
				base.AddCloseButton();
				Managers.soundManager.Play("newPersonBorn", this.transform, 0.2f, false, false);
				base.AddButton("teleport", null, "go there", "Button", 0, 220, "teleportTo", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				break;
			}
			base.UpdateScale();
		}
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00033F2B File Offset: 0x0003232B
	private new void Update()
	{
		this.AddTextIfNeeded();
		if (this.hapticPulseOn && this.hand != null)
		{
			this.hand.TriggerHapticPulse(600);
		}
		base.ReactToOnClick();
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x00033F68 File Offset: 0x00032368
	private void FlipHapticPulse()
	{
		bool flag = true;
		this.hapticPulseOn = !this.hapticPulseOn;
		if (this.maxBuzzes != -1)
		{
			if (this.hapticPulseOn)
			{
				this.buzzCount++;
			}
			if (this.buzzCount > this.maxBuzzes - 1)
			{
				flag = false;
				this.hapticPulseOn = false;
			}
		}
		if (flag)
		{
			base.Invoke("FlipHapticPulse", 1f);
		}
	}

	// Token: 0x060008E0 RID: 2272 RVA: 0x00033FE0 File Offset: 0x000323E0
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "close"))
			{
				if (!(contextName == "teleport"))
				{
					if (contextName == "restart")
					{
						Managers.areaManager.TryTransportToAreaByNameOrUrlName(Managers.areaManager.currentAreaName, string.Empty, false);
						base.CloseDialog();
					}
				}
				else
				{
					Managers.areaManager.TryTransportToAreaByNameOrUrlName(this.originAreaName, string.Empty, false);
					base.CloseDialog();
				}
			}
			else
			{
				if (this.dialogToReturnTo == DialogType.None)
				{
					this.dialogToReturnTo = DialogType.Start;
				}
				base.SwitchTo(this.dialogToReturnTo, string.Empty);
			}
		}
	}

	// Token: 0x04000691 RID: 1681
	private bool hapticPulseOn;

	// Token: 0x04000692 RID: 1682
	private string text;

	// Token: 0x04000693 RID: 1683
	private AlertDialog.Modes mode;

	// Token: 0x04000694 RID: 1684
	private bool didAddText;

	// Token: 0x04000695 RID: 1685
	private GameObject fundament;

	// Token: 0x04000696 RID: 1686
	private global::Ping ping;

	// Token: 0x04000697 RID: 1687
	private string originAreaName;

	// Token: 0x04000698 RID: 1688
	private int buzzCount;

	// Token: 0x04000699 RID: 1689
	public int maxBuzzes = -1;

	// Token: 0x0400069A RID: 1690
	public bool showOkButton;

	// Token: 0x0400069B RID: 1691
	public bool autoAddNewlines = true;

	// Token: 0x0400069C RID: 1692
	public DialogType dialogToReturnTo = DialogType.Start;

	// Token: 0x0400069D RID: 1693
	public float textSizeFactor = 1f;

	// Token: 0x0400069E RID: 1694
	public TextColor textColor;

	// Token: 0x0400069F RID: 1695
	public TextAlignment align;

	// Token: 0x040006A0 RID: 1696
	private bool allowClosingInsteadOfRestarting;

	// Token: 0x020000FC RID: 252
	private enum Modes
	{
		// Token: 0x040006A2 RID: 1698
		Info,
		// Token: 0x040006A3 RID: 1699
		Error,
		// Token: 0x040006A4 RID: 1700
		Ping,
		// Token: 0x040006A5 RID: 1701
		NewPersonBorn
	}
}
