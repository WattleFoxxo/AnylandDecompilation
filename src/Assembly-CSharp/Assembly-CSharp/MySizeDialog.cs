using System;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class MySizeDialog : Dialog
{
	// Token: 0x06000AEB RID: 2795 RVA: 0x00056934 File Offset: 0x00054D34
	public void Start()
	{
		float? num = MySizeDialog.lastEnteredMyScale;
		if (num == null && PlayerPrefs.HasKey("lastEnteredMyScaleFloat"))
		{
			MySizeDialog.lastEnteredMyScale = new float?(PlayerPrefs.GetFloat("lastEnteredMyScaleFloat", 100f));
		}
		this.weAreResized = Managers.personManager.WeAreResized();
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		this.AddButtons();
		base.UpdateScale();
		if (!this.weAreResized)
		{
			this.HandleSetMyScale();
		}
	}

	// Token: 0x06000AEC RID: 2796 RVA: 0x000569C8 File Offset: 0x00054DC8
	private void AddButtons()
	{
		float num = Managers.personManager.GetOurScale() * 100f;
		string text = "My size: " + num + "%";
		base.AddButton("setMyScale", null, text, "ButtonCompactNoIcon", 0, -150, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		if (!this.weAreResized)
		{
			base.AddCheckboxHelpButton("setMyScale_help", -150);
		}
		if (this.weAreResized && Managers.areaManager.weAreEditorOfCurrentArea && Managers.personManager.ourPerson.hasEditTools)
		{
			GameObject gameObject = base.AddButton("toggleEditArea", null, "Change things", "ButtonCompact", 0, 150, "editArea", Our.mode == EditModes.Area, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			gameObject.transform.Find("IconQuad").localScale *= 0.8f;
		}
		if (num >= 2f)
		{
			base.AddBackButton();
		}
	}

	// Token: 0x06000AED RID: 2797 RVA: 0x00056B00 File Offset: 0x00054F00
	private void HandleSetMyScale()
	{
		if (Our.mode == EditModes.Body)
		{
			Our.SetPreviousMode();
		}
		float? num = MySizeDialog.lastEnteredMyScale;
		string text = ((num == null) ? string.Empty : MySizeDialog.lastEnteredMyScale.ToString());
		Managers.dialogManager.GetFloatInput(text, this.GetScalePercentInfo(), delegate(float? nullablePercent)
		{
			if (nullablePercent != null)
			{
				float num2 = nullablePercent.Value;
				if (num2 >= 1f && num2 <= 2500f)
				{
					MySizeDialog.lastEnteredMyScale = new float?(num2);
					PlayerPrefs.SetFloat("lastEnteredMyScaleFloat", num2);
				}
				if (Managers.areaManager.weAreEditorOfCurrentArea || Managers.areaManager.rights.anyPersonSize == true)
				{
					num2 = Mathf.Clamp(num2, 1f, 2500f);
				}
				else
				{
					num2 = Mathf.Clamp(num2, 1f, 150f);
				}
				Managers.personManager.ApplyAndCachePhotonRigScale(num2 * 0.01f, false);
			}
			base.CloseDialog();
		}, false, false);
	}

	// Token: 0x06000AEE RID: 2798 RVA: 0x00056B6C File Offset: 0x00054F6C
	private string GetScalePercentInfo()
	{
		return string.Concat(new string[]
		{
			1.ToString(),
			"% to ",
			150.ToString(),
			"%/ ",
			2500.ToString(),
			"% as editor."
		});
	}

	// Token: 0x06000AEF RID: 2799 RVA: 0x00056BDA File Offset: 0x00054FDA
	private void Minimize()
	{
		base.SwitchTo(DialogType.Start, string.Empty);
	}

	// Token: 0x06000AF0 RID: 2800 RVA: 0x00056BEC File Offset: 0x00054FEC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "setMyScale"))
			{
				if (!(contextName == "toggleEditArea"))
				{
					if (!(contextName == "setMyScale_help"))
					{
						if (!(contextName == "back"))
						{
							if (contextName == "close")
							{
								base.CloseDialog();
							}
						}
						else
						{
							base.SwitchTo(DialogType.Settings, string.Empty);
						}
					}
					else
					{
						base.ToggleHelpLabel("Sets your size, like 10% to be smaller, or 200% to be twice as big. For certain sizes you need to be editor, or the area needs to use the  \"Allow any person size\" command. Limits: " + this.GetScalePercentInfo(), -700, 1f, 50, 0.7f);
					}
				}
				else
				{
					Our.ToggleMode(EditModes.Area);
					base.Invoke("Minimize", 0.15f);
				}
			}
			else
			{
				this.HandleSetMyScale();
			}
		}
	}

	// Token: 0x0400084A RID: 2122
	private bool weAreResized;

	// Token: 0x0400084B RID: 2123
	public static float? lastEnteredMyScale;

	// Token: 0x0400084C RID: 2124
	private const string lastEnteredMyScaleKey = "lastEnteredMyScaleFloat";
}
