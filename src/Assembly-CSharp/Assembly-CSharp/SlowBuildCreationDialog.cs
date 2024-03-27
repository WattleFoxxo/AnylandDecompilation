using System;
using UnityEngine;

// Token: 0x02000138 RID: 312
public class SlowBuildCreationDialog : Dialog
{
	// Token: 0x06000BC7 RID: 3015 RVA: 0x00063AA0 File Offset: 0x00061EA0
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		base.AddHeadline("Build Animation", -420, -460, TextColor.Default, TextAlignment.Left, false);
		this.AddSettings();
		base.AddButton("start", null, "Start", "Button", 0, 380, "play", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		if (Managers.areaManager.rights.slowBuildCreation == false)
		{
			base.AddLabel("✔  Editor-only here", 0, -290, 0.9f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x00063B78 File Offset: 0x00061F78
	private void AddSettings()
	{
		TextColor textColor = ((SlowBuildCreationDialog.startDelaySeconds != 1f) ? TextColor.Blue : TextColor.Default);
		string text = "startDelaySeconds";
		string text2 = null;
		string text3 = "Start delay: " + SlowBuildCreationDialog.startDelaySeconds + "s";
		string text4 = "ButtonCompactNoIcon";
		int num = 0;
		int num2 = -100;
		TextColor textColor2 = textColor;
		base.AddButton(text, text2, text3, text4, num, num2, null, false, 1f, textColor2, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		textColor = ((SlowBuildCreationDialog.secondsPerPart != 1f) ? TextColor.Blue : TextColor.Default);
		text4 = "secondsPerPart";
		text3 = null;
		text2 = "Time per part: " + SlowBuildCreationDialog.secondsPerPart + "s";
		text = "ButtonCompactNoIcon";
		num2 = 0;
		num = 60;
		textColor2 = textColor;
		base.AddButton(text4, text3, text2, text, num2, num, null, false, 1f, textColor2, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x00063C80 File Offset: 0x00062080
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "startDelaySeconds"))
			{
				if (!(contextName == "secondsPerPart"))
				{
					if (!(contextName == "start"))
					{
						if (contextName == "close")
						{
							base.CloseDialog();
						}
					}
					else
					{
						this.thing.gameObject.AddComponent<SlowBuildCreation>();
						base.CloseDialog();
					}
				}
				else
				{
					string text = "seconds (default: " + 1f.ToString() + ")";
					Managers.dialogManager.GetFloatInput(SlowBuildCreationDialog.secondsPerPart.ToString(), text, delegate(float? value)
					{
						if (value == null || value <= 0f)
						{
							value = new float?(1f);
						}
						SlowBuildCreationDialog.secondsPerPart = value.Value;
						GameObject gameObject = base.SwitchTo(DialogType.SlowBuildCreation, string.Empty);
						SlowBuildCreationDialog component = gameObject.GetComponent<SlowBuildCreationDialog>();
						component.thing = this.thing;
					}, false, false);
				}
			}
			else
			{
				string text2 = "seconds (default: " + 1f.ToString() + ")";
				Managers.dialogManager.GetFloatInput(SlowBuildCreationDialog.startDelaySeconds.ToString(), text2, delegate(float? value)
				{
					if (value == null || value <= 0f)
					{
						value = new float?(1f);
					}
					SlowBuildCreationDialog.startDelaySeconds = value.Value;
					GameObject gameObject2 = base.SwitchTo(DialogType.SlowBuildCreation, string.Empty);
					SlowBuildCreationDialog component2 = gameObject2.GetComponent<SlowBuildCreationDialog>();
					component2.thing = this.thing;
				}, false, false);
			}
		}
	}

	// Token: 0x040008F1 RID: 2289
	public Thing thing;

	// Token: 0x040008F2 RID: 2290
	private const float startDelaySecondsDefault = 1f;

	// Token: 0x040008F3 RID: 2291
	private const float secondsPerPartDefault = 1f;

	// Token: 0x040008F4 RID: 2292
	public static float startDelaySeconds = 1f;

	// Token: 0x040008F5 RID: 2293
	public static float secondsPerPart = 1f;
}
