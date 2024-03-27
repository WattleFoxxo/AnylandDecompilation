using System;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x02000134 RID: 308
public class StartDialog : Dialog
{
	// Token: 0x06000B96 RID: 2966 RVA: 0x00061164 File Offset: 0x0005F564
	public void Start()
	{
		this.testTapPulse = false;
		base.Init(base.gameObject, true, false, false);
		this.side = ((!(this.transform.parent.name == "HandCoreLeft")) ? Side.Right : Side.Left);
		if (Universe.features.ringMenu)
		{
			base.AddButton("startMain", null, null, "ButtonSmall", 0, 0, "start", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		if (Our.mode == EditModes.Area)
		{
			Our.UpdateModeMarkers();
		}
		base.ApplyAppropriateLayerForDesktopStream();
		base.UpdateScale();
		Our.dialogToGoBackTo = DialogType.None;
		base.DisableCollisionsBetweenPersonBodyAndCharacterController();
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x00061224 File Offset: 0x0005F624
	private new void Update()
	{
		if (Our.mode != EditModes.Inventory)
		{
			this.HandleTapMeGuidance();
			base.ReactToOnClick();
		}
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00061240 File Offset: 0x0005F640
	private void HandleTapMeGuidance()
	{
		if (this.hand != null && this.side == Side.Left && XRDevice.isPresent && !CrossDevice.desktopMode)
		{
			bool flag = !this.didHandleShowTapMe && Time.realtimeSinceStartup >= 15f && Managers.achievementManager != null && !Managers.achievementManager.DidAchieve(Achievement.TappedRing);
			if (this.testTapPulse)
			{
				flag = !this.didHandleShowTapMe;
			}
			if (flag)
			{
				this.didHandleShowTapMe = true;
				for (int i = 1; i <= 2; i++)
				{
					bool flag2 = i == 2;
					TextColor textColor = ((!flag2) ? TextColor.Gray : TextColor.White);
					string text = "tap";
					int num = -30;
					int num2 = -20;
					float num3 = 0.8f;
					TextColor textColor2 = textColor;
					TextMesh textMesh = base.AddLabel(text, num, num2, num3, false, textColor2, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
					Vector3 localPosition = textMesh.transform.localPosition;
					localPosition = new Vector3(localPosition.x, localPosition.y + 0.01f, localPosition.z);
					textMesh.transform.localPosition = localPosition;
					if (flag2)
					{
						this.tapLabelBright = textMesh;
					}
					else
					{
						this.tapLabel = textMesh;
						this.tapLabel.gameObject.SetActive(false);
					}
				}
				this.FlipHapticPulse();
			}
			if (this.hapticPulseOn)
			{
				this.hand.TriggerHapticPulse(600);
			}
		}
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x000613C4 File Offset: 0x0005F7C4
	private void FlipHapticPulse()
	{
		this.hapticPulseOn = !this.hapticPulseOn;
		if (this.hapticPulseOn)
		{
			this.tapLabel.gameObject.SetActive(false);
			this.tapLabelBright.gameObject.SetActive(true);
		}
		else
		{
			this.tapLabel.gameObject.SetActive(true);
			this.tapLabelBright.gameObject.SetActive(false);
		}
		base.Invoke("FlipHapticPulse", 1f);
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x00061444 File Offset: 0x0005F844
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		Our.SetPreferentialHandSide(this.hand);
		if (contextName != null)
		{
			if (contextName == "startMain")
			{
				Managers.achievementManager.RegisterAchievement(Achievement.TappedRing);
				this.OpenAppropriateDialog();
			}
		}
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00061484 File Offset: 0x0005F884
	public void OpenAppropriateDialog()
	{
		if (Our.GetCurrentNonStartDialog() == null)
		{
			if (Managers.personManager.WeAreResized())
			{
				base.SwitchTo(DialogType.MySize, string.Empty);
			}
			else
			{
				EditModes mode = Our.mode;
				if (mode != EditModes.Environment)
				{
					if (mode != EditModes.Thing)
					{
						DialogType dialogType = DialogType.None;
						if (dialogType != DialogType.None)
						{
							base.SwitchTo(dialogType, string.Empty);
						}
						else
						{
							base.SwitchTo(DialogType.Main, string.Empty);
						}
					}
					else if (this.hand.GetHeldObjectName() == "VertexMover" || this.hand.otherHandScript.GetHeldObjectName() == "VertexMover")
					{
						base.SwitchTo(DialogType.VertexMover, string.Empty);
					}
					else
					{
						CreateDialog.wasLastMinimized = false;
						base.SwitchTo(DialogType.Create, string.Empty);
					}
				}
				else
				{
					base.SwitchTo(DialogType.Area, string.Empty);
				}
			}
		}
	}

	// Token: 0x040008C5 RID: 2245
	private bool testTapPulse;

	// Token: 0x040008C6 RID: 2246
	private bool didHandleShowTapMe;

	// Token: 0x040008C7 RID: 2247
	private Side side;

	// Token: 0x040008C8 RID: 2248
	private bool hapticPulseOn;

	// Token: 0x040008C9 RID: 2249
	private TextMesh tapLabel;

	// Token: 0x040008CA RID: 2250
	private TextMesh tapLabelBright;
}
