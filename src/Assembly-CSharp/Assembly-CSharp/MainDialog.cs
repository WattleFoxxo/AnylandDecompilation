using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011D RID: 285
public class MainDialog : Dialog
{
	// Token: 0x06000A7A RID: 2682 RVA: 0x0004FA28 File Offset: 0x0004DE28
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		if (Universe.features.homeButton)
		{
			this.homeButton = base.AddModelButton("HomeButton", "home", null, 0, 0, false);
			this.homeConfirmButton = base.AddButton("confirmedHome", null, "Go home?", "ButtonCompactNoIconShortCentered", -335, -425, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			this.homeConfirmButton.SetActive(false);
		}
		if (Managers.broadcastNetworkManager.inRoom)
		{
			this.AddMainButtons();
		}
		else
		{
			this.joiningAreaLabel = base.AddLabel("Joining area...", -170, -70, 1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		if (Universe.features.helpDialog)
		{
			this.helpButton = base.AddModelButton("HelpButton", "help", null, 0, 0, false);
			if (Managers.browserManager.GuideBrowserShows())
			{
				base.ApplyEmissionColor(this.helpButton.transform.Find("Shape"), true);
			}
		}
		bool flag = false;
		if (Managers.personManager.ourPerson.isSoftBanned || Managers.personManager.ourPerson.showFlagWarning)
		{
			base.AddButton("softban", null, "account info", "ButtonCompactNoIcon", 0, -430, null, false, 1f, TextColor.Red, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		else if (Universe.features.forums && !flag && !Universe.demoMode)
		{
			GameObject gameObject = base.AddModelButton("ForumsButton", "forums", null, 0, 0, false);
			this.forumInfoText = gameObject.GetComponentInChildren<TextMesh>();
		}
		if (CrossDevice.desktopMode)
		{
			base.AddTimeDisplayCompact();
		}
		else
		{
			base.AddTimeDisplay();
		}
		base.ApplyAppropriateLayerForDesktopStream();
		if (Managers.broadcastNetworkManager && Managers.broadcastNetworkManager.inPhotonDownMode)
		{
			this.AddPhotonDownInfo();
		}
		if (Managers.forumManager.GetLatestCommentDateSeen() == DateTime.MinValue)
		{
			Managers.forumManager.SetLatestCommentDateSeenToNow();
		}
		else
		{
			this.HandleNewForumPostsCheck();
		}
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0004FC84 File Offset: 0x0004E084
	private void HandleNewForumPostsCheck()
	{
		if (this.forumInfoText != null)
		{
			float num = Managers.forumManager.MinutesSinceLastNewCommentsCheck();
			if (num == -1f || num > 5f)
			{
				Managers.forumManager.lastNewForumCommentsCheck = Time.time;
				Managers.forumManager.GetFavoriteForums(delegate(string reasonFailed, List<ForumData> forumDatas)
				{
					if (this != null && string.IsNullOrEmpty(reasonFailed) && forumDatas != null && forumDatas.Count >= 1)
					{
						this.AutoFavoriteNewlyAddedForumIfNeeded(forumDatas, "Quests");
						DateTime latestCommentDateOfTheseForums = Managers.forumManager.GetLatestCommentDateOfTheseForums(forumDatas);
						DateTime latestCommentDateSeen = Managers.forumManager.GetLatestCommentDateSeen();
						if (latestCommentDateSeen < latestCommentDateOfTheseForums)
						{
							Managers.forumManager.didLastFindNewForumComments = true;
							this.SetNewForumPostsText("New");
						}
					}
				});
			}
			else if (Managers.forumManager.didLastFindNewForumComments)
			{
				this.SetNewForumPostsText("New");
			}
		}
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x0004FD08 File Offset: 0x0004E108
	private void AutoFavoriteNewlyAddedForumIfNeeded(List<ForumData> forumDatas, string forumName)
	{
		if (!Managers.forumManager.NameExistsInForumDatas(forumDatas, forumName))
		{
			string text;
			if (Managers.forumManager.defaultForumIds.TryGetValue(forumName, out text))
			{
				string prefKey = "autoFavorited_" + forumName;
				if (PlayerPrefs.GetInt(prefKey, 0) != 1)
				{
					Managers.forumManager.ToggleFavoriteForum(text, delegate(string reasonFailed, bool isFavorite)
					{
						if (isFavorite)
						{
							PlayerPrefs.SetInt(prefKey, 1);
						}
					});
				}
			}
			else
			{
				Debug.Log("AutoFavoriteForumIfNeeded didn't find forum name \"" + forumName + "\" in default list.");
			}
		}
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0004FD9C File Offset: 0x0004E19C
	private void SetNewForumPostsText(string text = "New")
	{
		Renderer component = this.forumInfoText.GetComponent<Renderer>();
		component.material.color = base.GetTextColorAsColor(TextColor.White);
		this.forumInfoText.text = text.ToUpper();
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0004FDD8 File Offset: 0x0004E1D8
	private void AddMainButtons()
	{
		this.didAddMainButtons = true;
		if (Managers.areaManager.weAreEditorOfCurrentArea && Universe.features.createThings && (!CrossDevice.desktopMode || Our.createThingsInDesktopMode))
		{
			base.AddButton("create", null, "Create Thing", "ButtonBigCreateThing", 0, -220, "createThing", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			if (Universe.features.changeThings)
			{
				bool flag = Our.mode == EditModes.Area;
				string text = "checkbox" + ((!flag) ? "Inactive" : "Active");
				GameObject gameObject = base.AddButton("toggleEditArea", null, "Change Things", "CheckboxChangeThings", 0, 0, text, flag, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				DialogPart component = gameObject.GetComponent<DialogPart>();
				component.state = flag;
			}
		}
		if (CrossDevice.desktopMode)
		{
			if (Our.createThingsInDesktopMode)
			{
				GameObject gameObject2 = base.AddBackground("KeyboardMouseHelp", true, false);
				gameObject2.transform.Translate(Vector3.up * 0.08f);
				base.AddBackground("KeyboardMouseBackHelp", true, true);
			}
			else
			{
				base.AddBackground("KeyboardMouseHelp", false, false);
				base.AddBackground("KeyboardMouseBackHelp", true, false);
			}
			this.backsideWrapper = base.GetUiWrapper();
			base.SetUiWrapper(this.backsideWrapper);
			if (Universe.features.quitButton)
			{
				base.AddButton("quit", null, "Quit", "ButtonCompactNoIconShortCentered", 0, 420, null, false, 1f, TextColor.Gray, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
			base.RotateBacksideWrapper();
			base.SetUiWrapper(base.gameObject);
		}
		else if (!Managers.areaManager.weAreEditorOfCurrentArea && Universe.features.createThings)
		{
			this.backsideWrapper = base.GetUiWrapper();
			base.SetUiWrapper(this.backsideWrapper);
			base.AddButton("create", null, "Create unplaced", "ButtonCompact", 0, 400, "createThing", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			base.RotateBacksideWrapper();
			base.SetUiWrapper(base.gameObject);
		}
		if (!Managers.personManager.ourPerson.isSoftBanned)
		{
			int num = 235;
			int num2 = ((!CrossDevice.desktopMode || Our.createThingsInDesktopMode) ? 220 : (-70));
			bool flag2 = Universe.features.friendsDialog ^ Universe.features.areasDialog;
			if (flag2)
			{
				num = 0;
			}
			if (Universe.features.friendsDialog)
			{
				base.AddButton("friends", null, "Friends", "ButtonMainDialog", -num, num2, "friends", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, true);
			}
			if (Universe.features.areasDialog)
			{
				base.AddButton("areas", null, "Areas", "ButtonMainDialog", num, num2, "areas", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, true);
			}
			if (Universe.features.areaDialog && !Universe.demoMode)
			{
				base.AddModelButton("AreaButton", "area", null, 0, 0, false);
			}
		}
		if (Universe.features.ownProfileDialog)
		{
			base.AddModelButton("MeButton", "ownProfile", null, 0, 0, false);
		}
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x000501A4 File Offset: 0x0004E5A4
	private void AddPhotonDownInfo()
	{
		base.AddButton("photonDownInfo", null, "Outage", "ButtonCompactNoIconShortCentered", 220, -430, null, false, 1f, TextColor.Red, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x000501F4 File Offset: 0x0004E5F4
	private new void Update()
	{
		if (!this.didAddMainButtons && Managers.broadcastNetworkManager.inRoom && this.joiningAreaLabel != null)
		{
			global::UnityEngine.Object.Destroy(this.joiningAreaLabel.gameObject);
			this.AddMainButtons();
		}
		this.HandleImportPaste();
		this.HandleBlinkHelpButtonGuidance();
		base.ReactToOnClick();
		base.ReactToOnClickInWrapper(this.backsideWrapper);
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x00050260 File Offset: 0x0004E660
	private void HandleImportPaste()
	{
		if (Misc.CtrlIsPressed() && Input.GetKeyDown(KeyCode.V))
		{
			Managers.thingManager.StartCreateThingViaJson(GUIUtility.systemCopyBuffer);
		}
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x00050288 File Offset: 0x0004E688
	private void HandleBlinkHelpButtonGuidance()
	{
		if (!this.didHandleBlinkHelpButtonGuidance && Managers.achievementManager.DidAchieve(Achievement.SavedThing) && !Managers.achievementManager.DidAchieve(Achievement.SawHelpVideo))
		{
			this.didHandleBlinkHelpButtonGuidance = true;
			base.Invoke("FlipHelpButtonGlow", 1f);
		}
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x000502D8 File Offset: 0x0004E6D8
	private void FlipHelpButtonGlow()
	{
		this.helpButtonGlowOn = !this.helpButtonGlowOn;
		if (this.helpButton != null)
		{
			base.ApplyEmissionColor(this.helpButton.transform.Find("Shape"), this.helpButtonGlowOn);
		}
		base.Invoke("FlipHelpButtonGlow", 1f);
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x00050338 File Offset: 0x0004E738
	private void ShowPhotonDownInfo()
	{
		string text = "Some servers seem to be down at the moment and we're in fallback mode where you temporarily can't see others, but can still explore & create. You may also want to restart at some point.";
		Managers.dialogManager.ShowError(text, false, true, 0);
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0005035C File Offset: 0x0004E75C
	private void ShowMessageInsteadOfHomeButton(string s)
	{
		this.homeButton.SetActive(false);
		base.AddLabel(s, -445, -445, 1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x00050398 File Offset: 0x0004E798
	private void ShowHomeButtonAgain()
	{
		if (this != null)
		{
			base.CancelInvoke("ShowHomeButtonAgain");
			this.homeConfirmButton.SetActive(false);
			this.homeButton.SetActive(true);
		}
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x000503CC File Offset: 0x0004E7CC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "create":
		{
			Managers.achievementManager.RegisterAchievement(Achievement.ClickedCreateOnMainDialog);
			string text;
			if (!Managers.thingManager.GetPlacementsReachedLimit(out text, null))
			{
				Our.SetMode(EditModes.Thing, false);
				base.SwitchTo(DialogType.Create, string.Empty);
			}
			else
			{
				Managers.dialogManager.ShowInfo(text, false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
			}
			break;
		}
		case "toggleEditArea":
			if (Managers.personManager.ourPerson.hasEditTools)
			{
				Our.ToggleMode(EditModes.Area);
				base.Invoke("CloseDialog", 0.15f);
				Managers.achievementManager.RegisterAchievement(Achievement.ClickedChangeThings);
			}
			else
			{
				GameObject gameObject = base.SwitchTo(DialogType.GetEditTools, string.Empty);
				GetEditToolsDialog component = gameObject.GetComponent<GetEditToolsDialog>();
				if (component != null)
				{
					component.dialogToForwardToAfterHasEditTools = DialogType.Main;
				}
			}
			break;
		case "home":
			Managers.achievementManager.RegisterAchievement(Achievement.UsedHomeButton);
			if (Universe.demoMode)
			{
				this.ShowMessageInsteadOfHomeButton("Demo mode");
				Managers.soundManager.Play("no", this.transform, 1f, false, false);
			}
			else
			{
				this.homeButton.SetActive(false);
				this.homeConfirmButton.SetActive(true);
				base.Invoke("ShowHomeButtonAgain", 3f);
			}
			break;
		case "confirmedHome":
		{
			Managers.areaManager.ClearAreaToTransportToAfterNextAreaLoad();
			bool flag = Managers.areaManager.currentAreaIsHomeArea && Managers.broadcastNetworkManager.inRoom;
			if (flag)
			{
				Managers.thingManager.ResetTriggeredOnSomeoneNewInVicinity();
				Managers.personManager.ourPerson.ResetPositionAndRotation();
				Managers.desktopManager.currentVelocity = Vector3.zero;
				Managers.behaviorScriptManager.StopSpeech();
				this.hand.FinalizeTeleport();
				Managers.soundManager.Play("success", this.transform, 1f, false, false);
				this.ShowHomeButtonAgain();
			}
			else
			{
				base.CancelInvoke("ShowHomeButtonAgain");
				base.CloseDialog();
				Managers.areaManager.LoadHomeArea();
			}
			break;
		}
		case "forums":
			base.SwitchTo(DialogType.Forums, string.Empty);
			break;
		case "area":
			base.SwitchTo(DialogType.Area, string.Empty);
			break;
		case "areas":
			base.SwitchTo(DialogType.Areas, string.Empty);
			break;
		case "friends":
			base.SwitchTo(DialogType.Friends, string.Empty);
			break;
		case "ownProfile":
			base.SwitchTo(DialogType.OwnProfile, string.Empty);
			break;
		case "softban":
			base.SwitchTo(DialogType.Softban, string.Empty);
			break;
		case "help":
			if (CrossDevice.desktopMode)
			{
				Managers.browserManager.CloseGuideBrowser();
				base.ShowDesktopHelp();
			}
			else
			{
				Managers.browserManager.ToggleGuideBrowser(null);
				base.CloseDialog();
			}
			break;
		case "photonDownInfo":
			this.ShowPhotonDownInfo();
			break;
		case "quit":
			if (Managers.desktopManager.showDialogBackside)
			{
				Misc.ExitApp();
			}
			break;
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x040007EA RID: 2026
	private GameObject homeButton;

	// Token: 0x040007EB RID: 2027
	private GameObject homeConfirmButton;

	// Token: 0x040007EC RID: 2028
	private bool didAddMainButtons;

	// Token: 0x040007ED RID: 2029
	private TextMesh joiningAreaLabel;

	// Token: 0x040007EE RID: 2030
	private bool helpButtonGlowOn;

	// Token: 0x040007EF RID: 2031
	private GameObject helpButton;

	// Token: 0x040007F0 RID: 2032
	private bool didHandleBlinkHelpButtonGuidance;

	// Token: 0x040007F1 RID: 2033
	private TextMesh forumInfoText;
}
