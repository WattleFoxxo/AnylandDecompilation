using System;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class GetEditToolsDialog : Dialog
{
	// Token: 0x06000A02 RID: 2562 RVA: 0x000446B0 File Offset: 0x00042AB0
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		int num = 20;
		TextColor textColor = TextColor.Default;
		if (Managers.personManager.ourPerson.hasEditToolsPermanently)
		{
			this.introText = "You have a Creation Tools life-time pass";
			textColor = TextColor.Gold;
		}
		else if (Managers.personManager.ourPerson.hasEditTools && !Managers.personManager.ourPerson.isInEditToolsTrial)
		{
			this.introText = "You unlocked the Creation Tools";
			textColor = TextColor.Gold;
		}
		else
		{
			if (this.introText == null)
			{
				this.introText = "We are two indie devs and this is our labor of love. Whether you pick the free or paid version, thanks for being here!";
			}
			if (!Managers.personManager.ourPerson.isInEditToolsTrial)
			{
				this.trialButton = base.AddButton("getEditToolsTrial", null, "Get Creation Tools for a month!\nFree + Extendable", "ButtonBig", 0, num, "createThingTrial", false, 0.825f, TextColor.Gray, 1f, 0.004f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				num += 190;
				Managers.achievementManager.RegisterAchievement(Achievement.SawEditToolsTrialButton);
			}
			this.buyButton = base.AddButton("getEditToolsPermanently", null, "Get Creation Tools for a year!\nSupport Anyland for $9", "ButtonBig", 0, num, "createThing", false, 0.825f, TextColor.Gold, 1f, 0.004f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		string text = this.introText;
		int num2 = 0;
		int num3 = -270;
		float num4 = 0.8f;
		TextColor textColor2 = textColor;
		this.introTextMesh = base.AddLabel(text, num2, num3, num4, false, textColor2, false, TextAlignment.Center, 45, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0004483C File Offset: 0x00042C3C
	private void HandleStartedOrContinuedEditToolsTrial()
	{
		Managers.personManager.ourPerson.hasEditTools = true;
		Managers.personManager.ourPerson.isInEditToolsTrial = true;
		Managers.personManager.ourPerson.wasEditToolsTrialEverActivated = true;
		Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
		base.SwitchTo(this.dialogToForwardToAfterHasEditTools, string.Empty);
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x000448A8 File Offset: 0x00042CA8
	private void RequestTakingLongTime()
	{
		if (this != null && base.gameObject != null)
		{
			string text = "This takes longer than usual. Is your internet ok? Do you have any antivirus which may interfere with the connection?";
			base.AddLabel(text, 0, -110, 0.85f, false, TextColor.Red, false, TextAlignment.Center, 40, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x000448F8 File Offset: 0x00042CF8
	private void HandlePreCall()
	{
		base.Invoke("RequestTakingLongTime", 5f);
		if (this.introTextMesh != null)
		{
			global::UnityEngine.Object.Destroy(this.introTextMesh.gameObject);
		}
		global::UnityEngine.Object.Destroy(this.buyButton);
		global::UnityEngine.Object.Destroy(this.trialButton);
	}

	// Token: 0x06000A06 RID: 2566 RVA: 0x0004494C File Offset: 0x00042D4C
	private void OnDestroy()
	{
		base.CancelInvoke();
	}

	// Token: 0x06000A07 RID: 2567 RVA: 0x00044954 File Offset: 0x00042D54
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "getEditToolsTrial"))
			{
				if (!(contextName == "getEditToolsPermanently"))
				{
					if (contextName == "back")
					{
						base.SwitchTo(DialogType.Main, string.Empty);
					}
				}
				else
				{
					bool desktopMode = CrossDevice.desktopMode;
					if (desktopMode)
					{
						Managers.dialogManager.ShowInfo("Sorry, you need to be in VR mode for this (using e.g. Vive or Oculus).", true, true, 0, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
					}
					else
					{
						this.HandlePreCall();
						Managers.achievementManager.RegisterAchievement(Achievement.ClickedBuyCreationTools);
						Managers.extrasManager.StartEditToolsPurchase(delegate(ResponseError responseError)
						{
							base.CancelInvoke();
							if (responseError == null)
							{
								base.CloseDialog();
							}
							else
							{
								Managers.errorManager.ShowCriticalHaltError(responseError.GetPublicErrorMessage(), true, false, true, false, false);
							}
						});
					}
				}
			}
			else
			{
				this.HandlePreCall();
				Managers.extrasManager.StartEditToolsTrial(delegate(ResponseError responseError)
				{
					base.CancelInvoke();
					if (responseError == null)
					{
						Managers.achievementManager.RegisterAchievement(Achievement.ClickedEditToolsTrialButton);
						this.HandleStartedOrContinuedEditToolsTrial();
					}
					else
					{
						Managers.errorManager.ShowCriticalHaltError(responseError.GetPublicErrorMessage(), true, false, true, false, false);
					}
				});
			}
		}
	}

	// Token: 0x0400077D RID: 1917
	public DialogType dialogToForwardToAfterHasEditTools = DialogType.Create;

	// Token: 0x0400077E RID: 1918
	public string introText;

	// Token: 0x0400077F RID: 1919
	private GameObject buyButton;

	// Token: 0x04000780 RID: 1920
	private GameObject trialButton;

	// Token: 0x04000781 RID: 1921
	private TextMesh introTextMesh;
}
