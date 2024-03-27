using System;
using DaikonForge.VoIP;
using UnityEngine;

// Token: 0x02000124 RID: 292
public class OwnProfileDialog : Dialog
{
	// Token: 0x06000AF4 RID: 2804 RVA: 0x00056D94 File Offset: 0x00055194
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		if (!Universe.features.ownProfileDialog)
		{
			Our.dialogToGoBackTo = DialogType.None;
			base.CloseDialog();
			return;
		}
		if (!CrossDevice.desktopMode && !Managers.personManager.ourPerson.amplifySpeech)
		{
			this.attachThingsText = base.AddLabel("You can now attach things to yourself", 0, 200, 0.7f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		GameObject gameObject = base.AddButton("settings", null, null, "ButtonSmall", -400, 420, "attributes", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.MinifyButton(gameObject, 1f, 1f, 0.55f, false);
		base.AddBrush();
		base.AddMirror();
		Person ourPerson = Managers.personManager.ourPerson;
		base.AddPersonInfo(ourPerson, ourPerson.screenName, ourPerson.statusText, ourPerson.ageInDays, true, true, false);
		this.microphone = Managers.personManager.ourPerson.Head.GetComponent<MicrophoneInputDevice>();
		this.AddMicrophoneInterface();
		base.AddTopCreationsOfPerson(Managers.personManager.ourPerson.userId);
		base.Invoke("SetBodyMode", 0.05f);
		GameObject gameObject2 = base.AddButton("desktopCamera", null, null, "ButtonSmall", 400, 420, "camera", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.MinifyButton(gameObject2, 1f, 1f, 0.55f, false);
		this.AddCreateToolsStateInfo();
		base.AddGiftsButton();
		this.AddBacksideButtons();
		Managers.achievementManager.RegisterAchievement(Achievement.OpenedOwnProfile);
	}

	// Token: 0x06000AF5 RID: 2805 RVA: 0x00056F68 File Offset: 0x00055368
	private void AddBacksideButtons()
	{
		this.backsideWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.backsideWrapper);
		int num = -300;
		this.memorizeBodyPositionsLabel = base.AddLabel(string.Empty, 0, -455, 0.8f, false, TextColor.Green, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddCheckbox("memorizeBodyPositions", null, "Snap body parts", 0, num, Our.attachmentPointsMemory.HasMemorized(), 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		num += 115;
		base.AddCheckbox("toggleExtraMirrors", null, "Extra mirrors", 0, num, Our.showExtraMirrors, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		base.RotateBacksideWrapper();
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000AF6 RID: 2806 RVA: 0x00057024 File Offset: 0x00055424
	private void AddCreateToolsStateInfo()
	{
		Person ourPerson = Managers.personManager.ourPerson;
		if (ourPerson.hasEditTools)
		{
			string text = string.Empty;
			TextColor textColor = TextColor.Default;
			if (ourPerson.hasEditToolsPermanently)
			{
				text = "You have a Creation Tools life-time pass";
				textColor = TextColor.Gold;
			}
			else if (!ourPerson.isInEditToolsTrial)
			{
				text = "You unlocked the Creation Tools";
				textColor = TextColor.Gold;
			}
			if (text != string.Empty)
			{
				base.AddLabel(text, 0, 410, 0.8f, true, textColor, false, TextAlignment.Center, -1, 1f, false, TextAnchor.LowerCenter);
			}
		}
	}

	// Token: 0x06000AF7 RID: 2807 RVA: 0x000570AE File Offset: 0x000554AE
	private void SetBodyMode()
	{
		Our.SetMode(EditModes.Body, false);
	}

	// Token: 0x06000AF8 RID: 2808 RVA: 0x000570B8 File Offset: 0x000554B8
	private void AddMicrophoneInterface()
	{
		if (!Managers.settingManager.GetState(Setting.Microphone))
		{
			this.disableMuteMeButton = base.AddCheckbox("disableMuteMe", null, "Mute me", 9, 60, true, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
		}
		else
		{
			if (Managers.personManager.ourPerson.amplifySpeech)
			{
				base.AddCheckbox("toggleAmplifySpeech", null, "Amplify speech", 9, 200, true, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None);
			}
			base.AddMicrophoneIndicators(20);
			base.AddButton("microphone", null, "Microphone...", "ButtonCompactNoIconShortCentered", 0, 65, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
	}

	// Token: 0x06000AF9 RID: 2809 RVA: 0x00057181 File Offset: 0x00055581
	private void OnDestroy()
	{
		base.RemoveMirror();
		Managers.personManager.ShowOurSecondaryDots(false);
	}

	// Token: 0x06000AFA RID: 2810 RVA: 0x00057194 File Offset: 0x00055594
	private new void Update()
	{
		base.UpdateMicrophoneIndicator(this.microphone);
		base.Update();
	}

	// Token: 0x06000AFB RID: 2811 RVA: 0x000571A8 File Offset: 0x000555A8
	public void Close()
	{
		base.RemoveMirror();
		Our.dialogToGoBackTo = DialogType.None;
		Our.SetPreviousMode();
		base.CloseDialog();
	}

	// Token: 0x06000AFC RID: 2812 RVA: 0x000571C1 File Offset: 0x000555C1
	public override void RecreateInterfaceAfterSettingsChangeIfNeeded()
	{
		if (this.disableMuteMeButton != null)
		{
			this.DisableMuteMe();
			this.disableMuteMeButton = null;
		}
	}

	// Token: 0x06000AFD RID: 2813 RVA: 0x000571E4 File Offset: 0x000555E4
	private void DisableMuteMe()
	{
		if (Managers.personManager.ourPerson.isSoftBanned)
		{
			Managers.soundManager.Play("no", this.transform, 1f, false, false);
			Managers.dialogManager.SwitchToNewDialog(DialogType.Softban, null, string.Empty);
		}
		else
		{
			Managers.settingManager.SetState(Setting.Microphone, true, false);
			base.SwitchTo(DialogType.OwnProfile, string.Empty);
		}
	}

	// Token: 0x06000AFE RID: 2814 RVA: 0x00057254 File Offset: 0x00055654
	public void AddOrUpdatePersonStats()
	{
		base.AddOrUpdatePersonStats(Managers.personManager.ourPerson);
	}

	// Token: 0x06000AFF RID: 2815 RVA: 0x00057268 File Offset: 0x00055668
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "editMyName":
		{
			string text3 = Managers.personManager.ourPerson.photonPlayer.name;
			string text2 = string.Empty;
			if (text3 == "unnamed")
			{
				text3 = string.Empty;
				text2 = "your name";
			}
			text3 = text3.Trim();
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (!string.IsNullOrEmpty(text))
				{
					Managers.personManager.DoChangeName(text);
				}
				base.SwitchTo(DialogType.OwnProfile, string.Empty);
			}, contextName, (!(text3 == "unnamed")) ? text3 : string.Empty, 30, text2, false, false, false, false, 1f, true, false, null, false);
			break;
		}
		case "editMyStatus":
			if (Managers.personManager.ourPerson.isSoftBanned)
			{
				Managers.soundManager.Play("no", this.transform, 1f, false, false);
				Managers.dialogManager.SwitchToNewDialog(DialogType.Softban, null, string.Empty);
			}
			else
			{
				Managers.dialogManager.GetInput(delegate(string text)
				{
					if (text != null)
					{
						Managers.personManager.DoChangeStatusText(text);
					}
					base.SwitchTo(DialogType.OwnProfile, string.Empty);
				}, contextName, Managers.personManager.ourPerson.statusText, 120, "status text (you can use [areaname], [board: ..])", true, true, false, false, 1f, false, false, null, false);
			}
			break;
		case "settings":
			base.SwitchTo(DialogType.Settings, string.Empty);
			break;
		case "back":
			if (Our.dialogToGoBackTo != DialogType.None)
			{
				DialogType dialogToGoBackTo = Our.dialogToGoBackTo;
				Our.dialogToGoBackTo = DialogType.None;
				base.SwitchTo(dialogToGoBackTo, string.Empty);
			}
			else
			{
				Our.SetPreviousMode();
				base.SwitchTo(DialogType.Main, string.Empty);
			}
			break;
		case "desktopCamera":
			Our.dialogToGoBackTo = DialogType.None;
			Our.SetPreviousMode();
			base.SwitchTo(DialogType.DesktopCamera, string.Empty);
			break;
		case "gifts":
			Our.dialogToGoBackTo = DialogType.None;
			Our.SetPreviousMode();
			Our.personIdOfInterest = Managers.personManager.ourPerson.userId;
			base.SwitchTo(DialogType.Gifts, string.Empty);
			break;
		case "findAreasByCreatorId":
			FindAreasDialog.findByCreatorId = Managers.personManager.ourPerson.userId;
			FindAreasDialog.findByCreatorId_nameForReference = contextId;
			base.SwitchTo(DialogType.FindAreas, string.Empty);
			break;
		case "disableMuteMe":
			this.DisableMuteMe();
			break;
		case "toggleAmplifySpeech":
			Managers.personManager.DoAmplifySpeech(state, false);
			this.Close();
			base.SwitchTo(DialogType.OwnProfile, string.Empty);
			break;
		case "microphone":
			base.SwitchTo(DialogType.Microphone, string.Empty);
			break;
		case "toggleExtraMirrors":
			Our.SetShowExtraMirrors(state);
			base.UpdateMirror();
			break;
		case "memorizeBodyPositions":
			if (state)
			{
				Our.attachmentPointsMemory.Memorize();
				this.memorizeBodyPositionsLabel.text = ("✔ Memorized" + Environment.NewLine + "current").ToUpper();
			}
			else
			{
				Our.attachmentPointsMemory.Clear();
				this.memorizeBodyPositionsLabel.text = string.Empty;
			}
			break;
		case "close":
			this.Close();
			break;
		}
	}

	// Token: 0x0400084D RID: 2125
	private MicrophoneInputDevice microphone;

	// Token: 0x0400084E RID: 2126
	private TextMesh attachThingsText;

	// Token: 0x0400084F RID: 2127
	private const int middleLabelY = 200;

	// Token: 0x04000850 RID: 2128
	private GameObject disableMuteMeButton;

	// Token: 0x04000851 RID: 2129
	private TextMesh memorizeBodyPositionsLabel;
}
