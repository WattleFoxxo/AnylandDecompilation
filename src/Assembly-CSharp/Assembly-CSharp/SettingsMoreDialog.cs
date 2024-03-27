using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000127 RID: 295
public class SettingsMoreDialog : Dialog
{
	// Token: 0x06000B1C RID: 2844 RVA: 0x00059AA4 File Offset: 0x00057EA4
	public void Start()
	{
		this.cachePath = Application.persistentDataPath + "/cache";
		this.ourHead = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
		base.Init(base.gameObject, false, false, true);
		base.AddFundament();
		base.AddBackButton();
		base.AddSideHeadline("More");
		this.AddAttributes();
		base.AddDefaultPagingButtons(80, 410, "Page", false, 0, 0.85f, false);
	}

	// Token: 0x06000B1D RID: 2845 RVA: 0x00059B23 File Offset: 0x00057F23
	public void SwitchToPageContainingQualitySettings()
	{
		this.currentPage = 2;
	}

	// Token: 0x06000B1E RID: 2846 RVA: 0x00059B2C File Offset: 0x00057F2C
	private void AddAttributes()
	{
		this.RemoveAttributes();
		int num = -420;
		int num2 = 0;
		if (this.currentPage == 1)
		{
			this.attributeButtons.Add(base.AddCheckbox("useHelperLights", null, "Helper lights", 0, num + num2 * 115, Our.useHelperLights, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("useHelperLights_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("muteVideos", null, "Mute videos", 0, num + num2 * 115, Managers.videoManager.personalVolumeFactor == 0f, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("muteVideos_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("doSnapThingAngles", null, "Snap thing angles", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.SnapThingAngles), 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("doSnapThingAngles_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("doSnapThingPosition", null, "Snap thing positions", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.SnapThingPosition), 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("doSnapThingPosition_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("disableAllThingSnapping", null, "Disable all snapping", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.IgnoreThingSnapping), 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("disableAllThingSnapping_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("suppressBehaviorScriptsAsEditor", null, "Stop scripts", 0, num + num2 * 115, Our.suppressBehaviorScriptsAsEditor, 1f, "Checkbox", TextColor.Default, "as editor", ExtraIcon.StopScripts));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("suppressBehaviorScriptsAsEditor_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 2)
		{
			this.attributeButtons.Add(base.AddCheckbox("extraEffectsEvenInVR", null, "Extra effects in VR", 0, num + num2 * 115, Managers.settingManager.GetState(Setting.ExtraEffectsInVr), 1f, "Checkbox", TextColor.Default, "may lag", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("extraEffectsEvenInVR_help", num + num2 * 115));
			num2++;
			Transform transform = this.ourHead.transform.Find("FPSDisplay");
			this.attributeButtons.Add(base.AddCheckbox("showFPS", null, "Show Frame Rate", 0, num + num2 * 115, transform != null, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("showFPS_help", num + num2 * 115));
			num2++;
			num += 10;
			this.attributeButtons.Add(base.AddLabel("Nearby Limits", 0, num + num2 * 115, 0.85f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft).gameObject);
			num += 110;
			string text = string.Empty;
			text = string.Empty;
			TextColor textColor = TextColor.Default;
			int? maxThingsAroundOverride = Managers.optimizationManager.maxThingsAroundOverride;
			if (maxThingsAroundOverride != null)
			{
				string text2 = ": ";
				int? maxThingsAroundOverride2 = Managers.optimizationManager.maxThingsAroundOverride;
				text = text2 + maxThingsAroundOverride2.Value.ToString();
				textColor = TextColor.Blue;
			}
			string text3 = text;
			text = string.Concat(new object[] { text3, " (default: ", 250, ")" });
			List<GameObject> list = this.attributeButtons;
			text3 = "maxThingsAroundOverride";
			string text4 = null;
			string text5 = "Things" + text;
			string text6 = "ButtonCompactNoIcon";
			int num3 = 0;
			int num4 = num + num2 * 115;
			TextColor textColor2 = textColor;
			list.Add(base.AddButton(text3, text4, text5, text6, num3, num4, null, false, 1f, textColor2, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("maxThingsAroundOverride_help", num + num2 * 115));
			num2++;
			num += -20;
			text = string.Empty;
			textColor = TextColor.Default;
			int? maxLightsAroundOverride = Managers.optimizationManager.maxLightsAroundOverride;
			if (maxLightsAroundOverride != null)
			{
				string text7 = ": ";
				int? maxLightsAroundOverride2 = Managers.optimizationManager.maxLightsAroundOverride;
				text = text7 + maxLightsAroundOverride2.Value.ToString();
				textColor = TextColor.Blue;
			}
			text6 = text;
			text = string.Concat(new object[] { text6, " (default: ", 3, ")" });
			List<GameObject> list2 = this.attributeButtons;
			text6 = "maxLightsAroundOverride";
			text5 = null;
			text4 = "Lights" + text;
			text3 = "ButtonCompactNoIcon";
			num3 = 0;
			int num5 = num + num2 * 115;
			textColor2 = textColor;
			list2.Add(base.AddButton(text6, text5, text4, text3, num3, num5, null, false, 1f, textColor2, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("maxLightsAroundOverride_help", num + num2 * 115));
			num2++;
			num += -20;
			text = string.Empty;
			textColor = TextColor.Default;
			string text8 = "default";
			int? maxLightsToThrowShadowsOverride = Managers.optimizationManager.maxLightsToThrowShadowsOverride;
			if (maxLightsToThrowShadowsOverride != null)
			{
				string text9 = ": ";
				int? maxLightsToThrowShadowsOverride2 = Managers.optimizationManager.maxLightsToThrowShadowsOverride;
				text = text9 + maxLightsToThrowShadowsOverride2.Value.ToString();
				textColor = TextColor.Blue;
				text8 = "def.";
			}
			text3 = text;
			text = string.Concat(new object[] { text3, " (", text8, ": ", 1, ")" });
			List<GameObject> list3 = this.attributeButtons;
			text3 = "maxLightsToThrowShadowsOverride";
			text4 = null;
			text5 = "Shade Lights" + text;
			text6 = "ButtonCompactNoIcon";
			num3 = 0;
			int num6 = num + num2 * 115;
			textColor2 = textColor;
			list3.Add(base.AddButton(text3, text4, text5, text6, num3, num6, null, false, 1f, textColor2, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("maxLightsToThrowShadowsOverride_help", num + num2 * 115));
			num2++;
			this.AddLimitWarnings(num - 30 + num2 * 115);
		}
		else if (this.currentPage == 3)
		{
			this.attributeButtons.Add(base.AddCheckbox("teleportLaserAutoTargetsGround", null, "Walk laser auto-targets ground", 0, num + num2 * 115, Our.teleportLaserAutoTargetsGround, 0.8f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("teleportLaserAutoTargetsGround_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("disableThingMerging", null, "Disable Thing merging", 0, num + num2 * 115, !Managers.thingManager.mergeThings, 0.85f, "Checkbox", TextColor.Default, "slow", ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("disableThingMerging_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("weAreInvisibleWhereAllowed", null, "I'm invisible", 0, num + num2 * 115, Managers.personManager.ourPerson.isInvisibleWhereAllowed, 0.85f, "Checkbox", TextColor.Default, "where allowed", ExtraIcon.WeAreInvisible));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("weAreInvisibleWhereAllowed_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("lockLegMovement", null, "Lock Legs", 0, num + num2 * 115, Our.lockLegMovement, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("lockLegMovement_help", num + num2 * 115));
			num2++;
			num += 40;
			this.attributeButtons.Add(base.AddCheckbox("createThingsInDesktopMode", null, "Create in Desktop Mode", 0, num + num2 * 115, Our.createThingsInDesktopMode, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("createThingsInDesktopMode_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 4)
		{
			this.attributeButtons.Add(base.AddCheckbox("useWindowedApp", null, "Windowed mode", 0, num + num2 * 115, Universe.useWindowedApp, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("useWindowedApp_help", num + num2 * 115));
			num2++;
			if (CrossDevice.type == global::DeviceType.OculusTouch)
			{
				this.attributeButtons.Add(base.AddCheckbox("oculusTouchLegacyMode", null, "Legacy Controls", 0, num + num2 * 115, CrossDevice.oculusTouchLegacyMode, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
				this.attributeButtons.Add(base.AddCheckboxHelpButton("oculusTouchLegacyMode_help", num + num2 * 115));
				num2++;
			}
			this.attributeButtons.Add(base.AddCheckbox("demoMode", null, "Demo mode", 0, num + num2 * 115, Universe.demoMode, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("demoMode_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddCheckbox("contextHighlightPlacements", null, "Highlight lasered Things", 0, num + num2 * 115, Our.contextHighlightPlacements, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("contextHighlightPlacements_help", num + num2 * 115));
			num2++;
			num += 40;
			Transform transform2 = this.ourHead.transform.Find("LogDisplay");
			this.attributeButtons.Add(base.AddCheckbox("showErrors", null, "Show Errors", 0, num + num2 * 115, transform2 != null, 1f, "Checkbox", TextColor.Default, null, ExtraIcon.None));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("showErrors_help", num + num2 * 115));
			num2++;
		}
		else if (this.currentPage == 5)
		{
			this.attributeButtons.Add(base.AddButton("clearCache", null, "Clear cache", "ButtonCompact", 0, num + num2 * 115, "clear", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("clearCache_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddButton("lockFeatures", null, "Lock features...", "ButtonCompactNoIcon", 0, num + num2 * 115, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("lockFeatures_help", num + num2 * 115));
			num2++;
			this.attributeButtons.Add(base.AddButton("resetGuidance", null, "Reset guidance", "ButtonCompactNoIcon", 0, num + num2 * 115, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("resetGuidance_help", num + num2 * 115));
			num2++;
			num += 115;
			this.attributeButtons.Add(base.AddButton("resetLegs", null, "Reset legs", "ButtonCompactNoIcon", 0, num + num2 * 115, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false));
			this.attributeButtons.Add(base.AddCheckboxHelpButton("resetLegs_help", num + num2 * 115));
			num2++;
		}
	}

	// Token: 0x06000B1F RID: 2847 RVA: 0x0005A778 File Offset: 0x00058B78
	private void RemoveAttributes()
	{
		foreach (GameObject gameObject in this.attributeButtons)
		{
			global::UnityEngine.Object.Destroy(gameObject);
		}
		this.attributeButtons = new List<GameObject>();
	}

	// Token: 0x06000B20 RID: 2848 RVA: 0x0005A7E0 File Offset: 0x00058BE0
	private void Minimize()
	{
		base.SwitchTo(DialogType.Start, string.Empty);
	}

	// Token: 0x06000B21 RID: 2849 RVA: 0x0005A7F0 File Offset: 0x00058BF0
	private void AddLimitWarnings(int y)
	{
		List<string> list = new List<string>();
		if (Managers.optimizationManager.maxLightsAround > Managers.optimizationManager.maxThingsAround)
		{
			list.Add("Lights limit exceeds things limit.");
		}
		if (Managers.optimizationManager.maxLightsToThrowShadows > Managers.optimizationManager.maxLightsAround)
		{
			list.Add("Shade light limit exceeds lights limit.");
		}
		if (list.Count >= 1)
		{
			string text = string.Join(Environment.NewLine, list.ToArray());
			this.attributeButtons.Add(base.AddLabel(text, 0, y, 0.75f, false, TextColor.Red, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft).gameObject);
			Managers.soundManager.Play("no", this.transform, 0.25f, false, false);
		}
	}

	// Token: 0x06000B22 RID: 2850 RVA: 0x0005A8B0 File Offset: 0x00058CB0
	private void ClearLogFiles()
	{
		string[] files = Directory.GetFiles(Application.persistentDataPath, "*log*.txt");
		foreach (string text in files)
		{
			File.Delete(text);
		}
	}

	// Token: 0x06000B23 RID: 2851 RVA: 0x0005A8F0 File Offset: 0x00058CF0
	private void SetExtraEffectsEvenInVRIfAppropriate(bool state)
	{
		if (state && !SettingsMoreDialog.confirmedExtraEffectsEvenInVR)
		{
			string text = "Please note this may lag a lot (and edge shade can currently sometimes cause an unnatural glare)";
			base.SwitchToConfirmDialog(text, delegate(bool didConfirm)
			{
				if (didConfirm)
				{
					SettingsMoreDialog.confirmedExtraEffectsEvenInVR = true;
					Managers.optimizationManager.SetExtraEffectsEvenInVR(state, false);
				}
				GameObject gameObject = this.SwitchTo(DialogType.SettingsMore, string.Empty);
				SettingsMoreDialog component = gameObject.GetComponent<SettingsMoreDialog>();
				component.currentPage = 2;
				component.dialogToGoBackTo = this.dialogToGoBackTo;
			});
		}
		else
		{
			Managers.settingManager.SetState(Setting.ExtraEffectsInVr, state, false);
		}
	}

	// Token: 0x06000B24 RID: 2852 RVA: 0x0005A957 File Offset: 0x00058D57
	public override void RecreateInterfaceAfterSettingsChangeIfNeeded()
	{
		this.AddAttributes();
	}

	// Token: 0x06000B25 RID: 2853 RVA: 0x0005A960 File Offset: 0x00058D60
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
				this.currentPage = 5;
			}
			this.AddAttributes();
			break;
		}
		case "nextPage":
		{
			base.HideHelpLabel();
			int num = ++this.currentPage;
			if (num > 5)
			{
				this.currentPage = 1;
			}
			this.AddAttributes();
			break;
		}
		case "teleportLaserAutoTargetsGround":
			Our.SetTeleportLaserAutoTargetsGround(state);
			break;
		case "suppressBehaviorScriptsAsEditor":
			Our.SetSuppressBehaviorScriptsAsEditor(state);
			break;
		case "doSnapThingAngles":
			Managers.settingManager.SetState(Setting.SnapThingAngles, state, false);
			break;
		case "doSnapThingPosition":
			Managers.settingManager.SetState(Setting.SnapThingPosition, state, false);
			break;
		case "disableAllThingSnapping":
			Managers.settingManager.SetState(Setting.IgnoreThingSnapping, state, false);
			break;
		case "createThingsInDesktopMode":
			Our.SetCreateThingsInDesktopMode(state);
			break;
		case "lockLegMovement":
			Our.SetLockLegMovement(state);
			break;
		case "disableThingMerging":
			if (state)
			{
				string text = "Please note this mode slows down everything and may make you motion sick. It should only be used for temporarily working around glitched items.";
				base.SwitchToConfirmDialog(text, delegate(bool didConfirm)
				{
					if (didConfirm)
					{
						Managers.thingManager.mergeThings = !state;
						this.RemoveAttributes();
						Managers.areaManager.ReloadCurrentArea();
					}
					else
					{
						this.SwitchTo(DialogType.SettingsMore, string.Empty);
					}
				});
			}
			else
			{
				Managers.thingManager.mergeThings = !state;
				this.RemoveAttributes();
				Managers.areaManager.ReloadCurrentArea();
			}
			break;
		case "muteVideos":
			Managers.videoManager.personalVolumeFactor = ((Managers.videoManager.personalVolumeFactor != 1f) ? 1f : 0f);
			Managers.videoManager.UpdateVolume();
			break;
		case "useWindowedApp":
			Universe.SetUseWindowedApp(state);
			break;
		case "demoMode":
			Universe.demoMode = state;
			break;
		case "useHelperLights":
		{
			Our.useHelperLights = !Our.useHelperLights;
			Transform transform = Managers.personManager.ourPerson.Rig.transform.Find("HeadLight");
			if (transform != null)
			{
				transform.gameObject.SetActive(Our.useHelperLights);
			}
			break;
		}
		case "contextHighlightPlacements":
			Our.SetContextHighlightPlacements(state);
			break;
		case "dynamicHands":
			Our.SetDynamicHands(state);
			break;
		case "showErrors":
			Managers.errorManager.ShowInWorldErrorLog(state);
			if (state)
			{
				this.ShowFPSDisplay(false);
			}
			break;
		case "showFPS":
			this.ShowFPSDisplay(state);
			if (state)
			{
				Managers.errorManager.ShowInWorldErrorLog(false);
			}
			break;
		case "clearCache":
		{
			try
			{
				this.ClearLogFiles();
			}
			catch (Exception ex)
			{
				Log.Debug("There was an issue with clearing log files");
				Managers.soundManager.Play("no", null, 1f, false, false);
			}
			Managers.browserManager.DestroyAllBrowsers();
			string[] array = new string[] { "thingdefs", "images", "browser" };
			foreach (string text2 in array)
			{
				string text3 = this.cachePath + "/" + text2;
				if (Directory.Exists(text3))
				{
					try
					{
						Directory.Delete(text3, true);
					}
					catch (Exception ex2)
					{
						string text4 = string.Concat(new string[] { "Oops, unable to delete folder \"", text2, "\" in \"", this.cachePath, "\". It may be used by a process." });
						base.ToggleHelpLabel(text4, -700, 1f, 50, 0.7f);
						Managers.soundManager.Play("no", null, 1f, false, false);
						return;
					}
				}
			}
			Managers.soundManager.Play("success", this.transform, 1f, false, false);
			base.ToggleHelpLabel("✔  Cleared", -700, 1f, 50, 0.7f);
			Managers.soundManager.Play("success", this.transform, 1f, false, false);
			break;
		}
		case "lockFeatures":
		{
			if (Our.mode == EditModes.Body)
			{
				Our.SetPreviousMode();
			}
			Misc.OpenWindowsExplorerAtPath(Application.persistentDataPath);
			string text5 = "You can lock down certain features, like creating or the ring menu, by editing the \"features.json\" file in the folder that just opened on desktop. Set features you want to disable to false, then restart. This can be useful for e.g. demoing to a group. (You can also delete the file to have it reset on next start.)";
			text5 = Misc.WrapWithNewlines(text5, 45, -1);
			Managers.dialogManager.ShowInfo(text5, false, false, 0, DialogType.Start, 0.85f, false, TextColor.Default, TextAlignment.Left);
			break;
		}
		case "oculusTouchLegacyMode":
			CrossDevice.SetOculusTouchLegacyMode(state);
			break;
		case "extraEffectsEvenInVR":
			this.SetExtraEffectsEvenInVRIfAppropriate(state);
			break;
		case "maxThingsAroundOverride":
		{
			if (Managers.settingManager.GetState(Setting.LowerGraphicsQuality))
			{
				Managers.settingManager.SetState(Setting.LowerGraphicsQuality, false, false);
			}
			string text6 = "max things around (default: " + 250 + ")";
			Managers.dialogManager.GetFloatInput(string.Empty, text6, delegate(float? floatValue)
			{
				int? num2 = ((floatValue == null) ? null : ((floatValue == null) ? null : new int?((int)floatValue.Value)));
				if (num2 == null || num2 < 0 || num2 == 250)
				{
					Managers.optimizationManager.maxThingsAroundOverride = null;
				}
				else
				{
					Managers.optimizationManager.maxThingsAroundOverride = num2;
				}
				Managers.optimizationManager.UpdateSettings();
				Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
				GameObject gameObject = this.SwitchTo(DialogType.SettingsMore, string.Empty);
				SettingsMoreDialog component = gameObject.GetComponent<SettingsMoreDialog>();
				component.currentPage = 2;
				component.dialogToGoBackTo = this.dialogToGoBackTo;
			}, false, false);
			break;
		}
		case "maxLightsAroundOverride":
		{
			if (Managers.settingManager.GetState(Setting.LowerGraphicsQuality))
			{
				Managers.settingManager.SetState(Setting.LowerGraphicsQuality, false, false);
			}
			string text7 = "max lights around (default: " + 3 + ")";
			Managers.dialogManager.GetFloatInput(string.Empty, text7, delegate(float? floatValue)
			{
				int? num3 = ((floatValue == null) ? null : ((floatValue == null) ? null : new int?((int)floatValue.Value)));
				if (num3 == null || num3 < 0 || num3 == 3)
				{
					Managers.optimizationManager.maxLightsAroundOverride = null;
				}
				else
				{
					Managers.optimizationManager.maxLightsAroundOverride = num3;
				}
				Managers.optimizationManager.UpdateSettings();
				Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
				GameObject gameObject2 = this.SwitchTo(DialogType.SettingsMore, string.Empty);
				SettingsMoreDialog component2 = gameObject2.GetComponent<SettingsMoreDialog>();
				component2.currentPage = 2;
				component2.dialogToGoBackTo = this.dialogToGoBackTo;
			}, false, false);
			break;
		}
		case "maxLightsToThrowShadowsOverride":
		{
			if (Managers.settingManager.GetState(Setting.LowerGraphicsQuality))
			{
				Managers.settingManager.SetState(Setting.LowerGraphicsQuality, false, false);
			}
			string text8 = "max lights with shadows (default: " + 1 + ")";
			Managers.dialogManager.GetFloatInput(string.Empty, text8, delegate(float? floatValue)
			{
				int? num4 = ((floatValue == null) ? null : ((floatValue == null) ? null : new int?((int)floatValue.Value)));
				if (num4 == null || num4 < 0 || num4 == 1)
				{
					Managers.optimizationManager.maxLightsToThrowShadowsOverride = null;
				}
				else
				{
					Managers.optimizationManager.maxLightsToThrowShadowsOverride = num4;
				}
				Managers.optimizationManager.UpdateSettings();
				Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
				GameObject gameObject3 = this.SwitchTo(DialogType.SettingsMore, string.Empty);
				SettingsMoreDialog component3 = gameObject3.GetComponent<SettingsMoreDialog>();
				component3.currentPage = 2;
				component3.dialogToGoBackTo = this.dialogToGoBackTo;
			}, false, false);
			break;
		}
		case "teleportLaserAutoTargetsGround_help":
			base.ToggleHelpLabel("When directing the teleport laser forward and nothing is hit, it will automatically try to target the ground. This can be useful for certain seating positions or when your hands are on the table.", -700, 1f, 50, 0.7f);
			break;
		case "maxThingsAroundOverride_help":
			base.ToggleHelpLabel("Overrides the normal maximum amount of closest Things to show. Increasing this may create lag. Note some Things may always show even when exceeding limits (like because of their large size, or attributes).", -700, 1f, 50, 0.7f);
			break;
		case "maxLightsAroundOverride_help":
			base.ToggleHelpLabel("Overrides the normal maximum amount of closest lights that shine around you at the same time. This may create lag. Note lights may still be hidden by the placements distance optimizer. In desktop mode, the light count is +2.", -700, 1f, 50, 0.7f);
			break;
		case "maxLightsToThrowShadowsOverride_help":
			base.ToggleHelpLabel("Overrides the normal maximum amount of closest lights that throw shadows around you at the same time. This may create lag. Note lights may still be hidden by the placements distance optimizer. In desktop mode, the shade light count is +1.", -700, 1f, 50, 0.7f);
			break;
		case "suppressBehaviorScriptsAsEditor_help":
			base.ToggleHelpLabel("Suppresses thing part commands if you're an editor of the area. You can also enable this across sessions by setting the Locked Features \"scriptsAsEditor\" value to false.", -700, 1f, 50, 0.7f);
			break;
		case "doSnapThingAngles_help":
			base.ToggleHelpLabel("Snaps the angles of all things when you move them, even if a thing wasn't set to angle-snap", -700, 1f, 50, 0.7f);
			break;
		case "doSnapThingPosition_help":
			base.ToggleHelpLabel("Snaps the position of all things to align with where you started dragging them, even if a thing wasn't set to position-snap", -700, 1f, 50, 0.7f);
			break;
		case "disableAllThingSnapping_help":
			base.ToggleHelpLabel("Overrrides individual thing snapping settings to not snap when moved", -700, 1f, 50, 0.7f);
			break;
		case "disableThingMerging_help":
			base.ToggleHelpLabel("Temporarily keep all thing parts in areas unmerged. This slows down everything but can help you adjust otherwise untouchable placements", -700, 1f, 50, 0.7f);
			break;
		case "contextHighlightPlacements_help":
			base.ToggleHelpLabel("When context-lasering Placements, shows a highlight color around them for better recognition", -700, 1f, 50, 0.7f);
			break;
		case "dynamicHands_help":
			base.ToggleHelpLabel("When enabled, parts of the hand move independently. On Oculus Touch and Valve Index Controllers, this individually controls the index finger and the grab part.", -700, 1f, 50, 0.7f);
			break;
		case "weAreInvisibleWhereAllowed":
			if (Our.mode == EditModes.Body)
			{
				Our.SetPreviousMode();
			}
			Managers.personManager.CachePhotonIsInvisibleWhereAllowed(state);
			Managers.areaManager.ReloadCurrentArea();
			break;
		case "resetGuidance":
			Managers.achievementManager.ClearAchievementsLocallyAndTemporarilyForTesting();
			base.ToggleHelpLabel("✔  Reset", -700, 1f, 50, 0.7f);
			Managers.soundManager.Play("success", this.transform, 1f, false, false);
			break;
		case "resetLegs":
			Managers.personManager.ourPerson.ResetLegsPositionRotationToUniversalDefault(null);
			Managers.soundManager.Play("success", this.transform, 0.25f, false, false);
			Managers.personManager.SaveOurLegAttachmentPointPositions();
			break;
		case "weAreInvisibleWhereAllowed_help":
			base.ToggleHelpLabel("Makes you invisible to others if the area uses an \"allow invisibility\" placement command", -700, 1f, 50, 0.7f);
			break;
		case "resetLegs_help":
			base.ToggleHelpLabel("Resets the position & rotation of your leg spheres/ attachments to their universal starting default. Also see the \"reset legs\" commands.", -700, 1f, 50, 0.7f);
			break;
		case "muteVideos_help":
			base.ToggleHelpLabel("Turns off the sound of all videos for this session, but just for you", -700, 1f, 50, 0.7f);
			break;
		case "useHelperLights_help":
			base.ToggleHelpLabel("Activates a head lamp so you can see even if the area is completely dark", -700, 1f, 50, 0.7f);
			break;
		case "showErrors_help":
			base.ToggleHelpLabel("Displays possible app errors in front of you and copes them to the clipboard. You normally don't need this, but for instance can use it to tell us more about an error.", -700, 1f, 50, 0.7f);
			break;
		case "showFPS_help":
			base.ToggleHelpLabel("Displays the FPS (Frames per Second) to analyze lag. A value of 90 is 100% smooth while lower values may indicate optimization need in the area, or a higher-powered PC.", -700, 1f, 50, 0.7f);
			break;
		case "clearCache_help":
			base.ToggleHelpLabel("Clears your local things cache at " + this.cachePath, -700, 1f, 70, 0.6f);
			break;
		case "lockFeatures_help":
			base.ToggleHelpLabel("Disables certain features, useful for e.g. demoing to a group.", -700, 1f, 50, 0.7f);
			break;
		case "extraEffectsEvenInVR_help":
			base.ToggleHelpLabel("Edge shade & bloom are by default only active in desktop mode, but with this setting they show in VR too. (Depending on the area and computer, this may lag a lot.)", -700, 1f, 50, 0.7f);
			break;
		case "createThingsInDesktopMode_help":
			base.ToggleHelpLabel("Adds \"Create Thing\" and \"Change Things\" buttons in desktop mode too, e.g. for scripting, testing, and moving things around.", -700, 1f, 50, 0.7f);
			break;
		case "lockLegMovement_help":
			base.ToggleHelpLabel("Will only let you move the leg attachment spheres when in Me mode. Useful if you want a finetuned and static position for the leg spheres.", -700, 1f, 50, 0.7f);
			break;
		case "useWindowedApp_help":
			base.ToggleHelpLabel("Switches Anyland on desktop from fullscreen to windowed, and remembers the setting for next startup.", -700, 1f, 50, 0.7f);
			break;
		case "demoMode_help":
			base.ToggleHelpLabel("Useful for letting others try creating at a specific spot, this disables moving, home button, context-lasering placements, the area dialog and forums. When going to another area, demo mode automatically ends.", -700, 1f, 50, 0.7f);
			break;
		case "oculusTouchLegacyMode_help":
			base.ToggleHelpLabel("[Oculus Legacy] X/A: Teleport, Y/B: Context, Thumbstick-Up: Teleport, Thumbstick-press: Delete. [Default] X/A: Context, Y/B: Delete, Thumbstick-Up/ Thumbstick-Press: Teleport.", -700, 1f, 50, 0.7f);
			break;
		case "resetGuidance_help":
			base.ToggleHelpLabel("Temporarily clears all personal achievements so you can test again what guidance the dialogs offer when one e.g. never created something, never clicked on help and so on. Restart Anyland to load your actual achievements again.", -700, 1f, 50, 0.7f);
			break;
		case "back":
			base.SwitchTo(this.dialogToGoBackTo, string.Empty);
			break;
		}
	}

	// Token: 0x06000B26 RID: 2854 RVA: 0x0005B704 File Offset: 0x00059B04
	private void ShowFPSDisplay(bool doShow)
	{
		GameObject @object = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
		if (doShow)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/FPSDisplay", typeof(GameObject))) as GameObject;
			gameObject.transform.parent = @object.transform;
		}
		else
		{
			Transform transform = @object.transform.Find("FPSDisplay");
			if (transform != null)
			{
				global::UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x04000870 RID: 2160
	public int currentPage = 1;

	// Token: 0x04000871 RID: 2161
	private const int maxPages = 5;

	// Token: 0x04000872 RID: 2162
	private const int pageContainingQualitySettings = 2;

	// Token: 0x04000873 RID: 2163
	private List<GameObject> attributeButtons = new List<GameObject>();

	// Token: 0x04000874 RID: 2164
	private string cachePath;

	// Token: 0x04000875 RID: 2165
	private GameObject ourHead;

	// Token: 0x04000876 RID: 2166
	public static bool confirmedExtraEffectsEvenInVR;

	// Token: 0x04000877 RID: 2167
	public DialogType dialogToGoBackTo = DialogType.Settings;
}
