using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000126 RID: 294
public class SettingsDialog : Dialog
{
	// Token: 0x06000B13 RID: 2835 RVA: 0x000589B0 File Offset: 0x00056DB0
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		this.AddAttributes();
		base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
		this.AddMoreButtons();
		base.AddButton("editTools", null, "Creation Tools...", "ButtonCompactNoIcon", 0, 0, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, true, false);
		this.AddBacksideControlsInfo();
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x00058A48 File Offset: 0x00056E48
	private void AddAttributes()
	{
		this.RemoveAttributes();
		int num = -300;
		int num2 = 0;
		if (this.currentPage == 1)
		{
			this.attributeButtons.Add(base.AddCheckbox("isFindable", null, "I'm findable", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.Findable), 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Findable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("isFindable_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("canEditFly", null, "Fly", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.Fly), 1f, "Checkbox", TextColor.Default, "as editor", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("canEditFly_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("seeInvisibleAsEditor", null, "See invisible", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.SeeInvisible), 0.8f, "Checkbox", TextColor.Default, "as editor", ExtraIcon.SeeInvisible));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("seeInvisibleAsEditor_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("touchUncollidableAsEditor", null, "Touch uncollidable", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.TouchUncollidable), 0.8f, "Checkbox", TextColor.Default, "as editor", ExtraIcon.TouchUncollidable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("touchUncollidableAsEditor_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("doOptimizeSpeed", null, "Lower quality graphics", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.LowerGraphicsQuality), 0.9f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("doOptimizeSpeed_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 2)
		{
			this.attributeButtons.Add(base.AddCheckbox("useSmoothMovement", null, "Smooth walk motion", 0, num + num2 * 115, Our.useSmoothMovement, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("useSmoothMovement_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("useSmoothRiding", null, "Smooth riding on things", 0, num + num2 * 115, Our.useSmoothRiding, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("useSmoothRiding_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("allThingsClonable", null, "All my creations are clonable", 0, num + num2 * 115, Managers.personManager.ourPerson.allThingsClonable, 0.75f, "Checkbox", TextColor.Default, null, ExtraIcon.Clonable));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("allThingsClonable_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("onlyFriendsCanPingUs", null, "Only get pings from friends", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.OnlyFriendsCanPingUs), 0.9f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("onlyFriendsCanPingUs_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("stopPingsAndAlerts", null, "Stop all pings & alerts", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.StopAlerts), 0.9f, "Checkbox", TextColor.Default, null, ExtraIcon.StopPingsAndAlerts));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("stopPingsAndAlerts_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 3)
		{
			this.attributeButtons.Add(base.AddCheckbox("findOptimizations", null, "Find Optimizations", 0, num + num2 * 115, Managers.optimizationManager.findOptimizations, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Body));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("findOptimizations_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("showGridLines", null, "Show grid lines", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.ShowGrid), 1f, "Checkbox", TextColor.Default, null, ExtraIcon.Grid));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("showGridLines_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("snapThingsToGrid", null, "Snap things to grid", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.SnapThingsToGrid), 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("snapThingsToGrid_help", num + num2 * 115));
			num2++;
			num += 40;
			string text = "Grid size: " + Math.Round((double)Our.gridSize, 4) + "m";
			this.attributeButtons.Add(base.AddButton("setGridSize", null, text, "ButtonCompactNoIcon", 0, num + num2 * 115, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("setGridSize_help", num + num2 * 115));
			num2++;
		}
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x00058FE0 File Offset: 0x000573E0
	private void AddBacksideControlsInfo()
	{
		string text = CrossDevice.GetVrModelString();
		if (string.IsNullOrEmpty(text))
		{
			text = "None";
		}
		string text2 = ((CrossDevice.type != global::DeviceType.Other) ? Misc.CamelCaseToSpaceSeparated(CrossDevice.type.ToString()) : "Non-VR");
		base.AddLabel("Detected VR Identifier: " + Environment.NewLine + text, 0, 200, 0.7f, true, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddLabel("Triggered Controls Mode: " + Environment.NewLine + text2, 0, 320, 0.7f, true, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x00059090 File Offset: 0x00057490
	private void AddMoreButtons()
	{
		base.MinifyMoreButton(base.AddButton("mySize", null, "My Size", "ButtonCompactNoIconShortCentered", -355, 400, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
		base.MinifyMoreButton(base.AddButton("more", null, "More", "ButtonCompactNoIconShortCentered", 355, 400, null, false, 1f, TextColor.Gray, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x0005912C File Offset: 0x0005752C
	private void RemoveAttributes()
	{
		foreach (GameObject gameObject in this.attributeButtons)
		{
			global::UnityEngine.Object.Destroy(gameObject);
		}
		this.attributeButtons = new List<GameObject>();
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x00059194 File Offset: 0x00057594
	public override void RecreateInterfaceAfterSettingsChangeIfNeeded()
	{
		this.AddAttributes();
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0005919C File Offset: 0x0005759C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "previousPage":
		{
			base.HideHelpLabel();
			int num = --this.currentPage;
			if (num < 1)
			{
				this.currentPage = 3;
			}
			this.AddAttributes();
			break;
		}
		case "nextPage":
		{
			base.HideHelpLabel();
			int num = ++this.currentPage;
			if (num > 3)
			{
				this.currentPage = 1;
			}
			this.AddAttributes();
			break;
		}
		case "isFindable":
			Managers.settingManager.SetState(Setting.Findable, state, false);
			break;
		case "doOptimizeSpeed":
			Managers.settingManager.SetState(Setting.LowerGraphicsQuality, state, false);
			break;
		case "canEditFly":
			Managers.settingManager.SetState(Setting.Fly, state, false);
			break;
		case "seeInvisibleAsEditor":
			Managers.settingManager.SetState(Setting.SeeInvisible, state, false);
			break;
		case "touchUncollidableAsEditor":
			Managers.settingManager.SetState(Setting.TouchUncollidable, state, false);
			break;
		case "useSmoothMovement":
			if (state && !SettingsDialog.confirmedSmoothMovementWarningThisSession)
			{
				string text = "Please note turning this on may make you motion sick";
				base.SwitchToConfirmDialog(text, delegate(bool didConfirm)
				{
					if (didConfirm)
					{
						SettingsDialog.confirmedSmoothMovementWarningThisSession = true;
						Our.SetUseSmoothMovement(state);
					}
					GameObject gameObject = this.SwitchTo(DialogType.Settings, string.Empty);
					SettingsDialog component = gameObject.GetComponent<SettingsDialog>();
					component.currentPage = 2;
				});
			}
			else
			{
				Our.SetUseSmoothMovement(state);
			}
			break;
		case "useSmoothRiding":
			if (state && !SettingsDialog.confirmedSmoothRidingWarningThisSession)
			{
				string text2 = "Please note turning this on may make you motion sick when riding on things";
				base.SwitchToConfirmDialog(text2, delegate(bool didConfirm)
				{
					if (didConfirm)
					{
						SettingsDialog.confirmedSmoothRidingWarningThisSession = true;
						Our.SetUseSmoothRiding(state);
					}
					GameObject gameObject2 = this.SwitchTo(DialogType.Settings, string.Empty);
					SettingsDialog component2 = gameObject2.GetComponent<SettingsDialog>();
					component2.currentPage = 2;
				});
			}
			else
			{
				Our.SetUseSmoothRiding(state);
			}
			break;
		case "findOptimizations":
			Managers.optimizationManager.SetFindOptimizations(state);
			Managers.personManager.AddOrTimeExtendNameTagsForAllOthers((!state) ? 10f : 120f);
			if (!Managers.settingManager.GetState(Setting.SeeInvisible))
			{
				Managers.settingManager.SetState(Setting.SeeInvisible, true, false);
			}
			break;
		case "allThingsClonable":
			Managers.personManager.DoSetAllThingsClonable(state);
			break;
		case "showGridLines":
			Managers.settingManager.SetState(Setting.ShowGrid, state, false);
			break;
		case "snapThingsToGrid":
			Managers.settingManager.SetState(Setting.SnapThingsToGrid, state, false);
			break;
		case "setGridSize":
		{
			string text3 = Our.gridSize.ToString();
			string text4 = "Default: " + 1f.ToString() + ".0";
			Managers.dialogManager.GetFloatInput(text3, text4, delegate(float? value)
			{
				if (value != null && value > 0f && value <= 1000f)
				{
					Our.SetGridSize(value.Value);
				}
				this.SwitchTo(DialogType.Settings, string.Empty);
			}, false, false);
			break;
		}
		case "isFindable_help":
			base.ToggleHelpLabel("When findable, others see your location in their friends list (unless the area's private) and you may appear on in.anyland.com", -700, 1f, 50, 0.7f);
			break;
		case "findOptimizations_help":
			base.ToggleHelpLabel("Adds unmerged Body Thing Part count & script messages per second to name tags, as high counts can add lag (context-laser someone to make name tags reappear). Also emits a red \"-\" at high-message sending Things.", -700, 1f, 50, 0.7f);
			break;
		case "canEditFly_help":
			base.ToggleHelpLabel("Lets you move into air when creating or changing things", -700, 1f, 50, 0.7f);
			break;
		case "seeInvisibleAsEditor_help":
			base.ToggleHelpLabel("Lets you see objects marked as \"invisible\" even when not creating", -700, 1f, 50, 0.7f);
			break;
		case "touchUncollidableAsEditor_help":
			base.ToggleHelpLabel("Lets you touch objects marked as \"uncollidable\" even when not creating", -700, 1f, 50, 0.7f);
			break;
		case "doOptimizeSpeed_help":
			base.ToggleHelpLabel("If you get graphics speed issues, you can try this to disable shadows, some lights & more", -700, 1f, 50, 0.7f);
			break;
		case "useSmoothMovement_help":
			base.ToggleHelpLabel("Smoothly moves when teleport-walking. Please note this may make you motion sick", -700, 1f, 50, 0.7f);
			break;
		case "useSmoothRiding_help":
			base.ToggleHelpLabel("When walking onto moving sub-things, you will move with them smoothly instead of in an interval. Please note this may make you motion sick", -700, 1f, 50, 0.7f);
			break;
		case "allThingsClonable_help":
			base.ToggleHelpLabel("Allows others to clone-edit your creations even if \"Clonable\" wasn't set (except items you set to \"Never clonable\"). This can be toggled on & off anytime, and doesn't make areas clonable. Credit to you is given when context-lasering the clone.", -700, 1f, 50, 0.7f);
			break;
		case "showGridLines_help":
			base.ToggleHelpLabel("Shows grid lines at each 1 meter, with indicators for half meters.", -700, 1f, 50, 0.7f);
			break;
		case "snapThingsToGrid_help":
			Managers.browserManager.OpenGuideBrowser("lHSEnGrk-dI", null);
			break;
		case "setGridSize_help":
			base.ToggleHelpLabel("Sets a grid size in meters for the \"Show Grid Lines\" and \"Snap to Grid\" options, e.g. \"2.5\". Default is 1m, maximum is " + 1000f + "m.", -700, 1f, 50, 0.7f);
			break;
		case "onlyFriendsCanPingUs":
			Managers.settingManager.SetState(Setting.OnlyFriendsCanPingUs, state, false);
			if (state)
			{
				Managers.settingManager.SetState(Setting.StopAlerts, false, false);
				this.AddAttributes();
			}
			break;
		case "onlyFriendsCanPingUs_help":
			base.ToggleHelpLabel("Stops receiving pings from people you didn't friend. (Non-friends can still sent off pings to you, you just won't get them.)", -700, 1f, 50, 0.7f);
			break;
		case "stopPingsAndAlerts":
			Managers.settingManager.SetState(Setting.StopAlerts, state, false);
			if (state)
			{
				Managers.settingManager.SetState(Setting.OnlyFriendsCanPingUs, false, false);
				this.AddAttributes();
			}
			break;
		case "stopPingsAndAlerts_help":
			base.ToggleHelpLabel("Stops receiving any pings or newborn alerts. (People can still sent off pings to you, you just won't get them.)", -700, 1f, 50, 0.7f);
			break;
		case "mySize":
			base.SwitchTo(DialogType.MySize, string.Empty);
			break;
		case "more":
			base.SwitchTo(DialogType.SettingsMore, string.Empty);
			break;
		case "editTools":
			base.SwitchTo(DialogType.GetEditTools, string.Empty);
			break;
		case "close":
			Our.dialogToGoBackTo = DialogType.None;
			if (Our.mode == EditModes.Body)
			{
				Our.SetPreviousMode();
			}
			base.CloseDialog();
			break;
		case "back":
			if (Our.mode == EditModes.Thing)
			{
				base.SwitchTo(DialogType.Create, string.Empty);
			}
			else
			{
				base.SwitchTo(DialogType.OwnProfile, string.Empty);
			}
			break;
		}
	}

	// Token: 0x04000869 RID: 2153
	public int currentPage = 1;

	// Token: 0x0400086A RID: 2154
	private const int maxPages = 3;

	// Token: 0x0400086B RID: 2155
	private const int pageContainingSmoothOptions = 2;

	// Token: 0x0400086C RID: 2156
	private List<GameObject> attributeButtons = new List<GameObject>();

	// Token: 0x0400086D RID: 2157
	public static bool confirmedSmoothMovementWarningThisSession;

	// Token: 0x0400086E RID: 2158
	public static bool confirmedSmoothRidingWarningThisSession;
}
