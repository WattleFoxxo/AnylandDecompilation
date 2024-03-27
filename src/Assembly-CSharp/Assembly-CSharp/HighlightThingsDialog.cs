using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class HighlightThingsDialog : Dialog
{
	// Token: 0x0600094B RID: 2379 RVA: 0x00039F5C File Offset: 0x0003835C
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		this.seesInvisiblePartsBefore = Our.seeInvisibleAsEditor;
		Our.SetSeeInvisibleAsEditor(true);
		if (!string.IsNullOrEmpty(HighlightThingsDialog.nameContainsText))
		{
			this.mode = "nameContains";
			this.page = 7;
		}
		base.AddHeadline("Highlight", -370, -460, TextColor.Default, TextAlignment.Left, false);
		base.AddDefaultPagingButtons(80, 400, "Page", false, 0, 0.85f, false);
		if (this.mode != null)
		{
			this.ShowOnlyCertainThingsInArea();
		}
		this.AddButtons();
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x0003A007 File Offset: 0x00038407
	private void AddButtons()
	{
		base.StartCoroutine(this.DoAddButtons());
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0003A018 File Offset: 0x00038418
	private IEnumerator DoAddButtons()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		this.rowOffset = 0f;
		switch (this.page)
		{
		case 1:
			this.AddShowOnlyCheckbox("newest", "Newest " + 25 + " placements", false, ExtraIcon.None);
			this.AddShowOnlyCheckbox("oldest", "Oldest " + 25 + " placements", false, ExtraIcon.None);
			this.AddCheckboxSpacing();
			this.AddShowOnlyCheckbox("mostParts", "Top " + 25 + " by part count", false, ExtraIcon.ThingParts);
			this.AddShowOnlyCheckbox("mostPartsAround", "Top " + 3 + " around by part count", false, ExtraIcon.ThingParts);
			this.AddCheckboxSpacing();
			this.AddShowOnlyCheckbox("usesBehaviorScriptVariables", "Uses variables", false, ExtraIcon.Variables);
			break;
		case 2:
			this.AddShowOnlyCheckbox("benefitsFromShowingAtDistance", "\"Show if far away\" things", false, ExtraIcon.ShowAtDistance);
			this.AddShowOnlyCheckbox("specialShowDistance", "Special \"Show at\" distance", false, ExtraIcon.ShowAtDistance);
			this.AddCheckboxSpacing();
			this.AddShowOnlyCheckbox("lights", null, false, ExtraIcon.Light);
			this.AddCheckboxSpacing();
			this.AddShowOnlyCheckbox("lockedThings", null, true, ExtraIcon.Locked);
			this.AddShowOnlyCheckbox("unlockedThings", null, true, ExtraIcon.Unlocked);
			break;
		case 3:
			this.AddShowOnlyCheckbox("clonables", null, false, ExtraIcon.Clonable);
			this.AddCheckboxSpacing();
			this.AddShowOnlyCheckbox("holdables", null, false, ExtraIcon.Holdable);
			this.AddCheckboxSpacing();
			this.AddShowOnlyCheckbox("thingsWhichSendToPlace", "\"Send to\" things", true, ExtraIcon.None);
			this.AddShowOnlyCheckbox("rightsSettingScripts", "Rights-setting scripts", false, ExtraIcon.None);
			this.AddShowOnlyCheckbox("tellAnyThings", "Tell any/ nearby scripts", false, ExtraIcon.None);
			break;
		case 4:
			this.AddShowOnlyCheckbox("peopleAttachments", "Attached to people", true, ExtraIcon.None);
			this.AddCheckboxSpacing();
			this.AddShowOnlyCheckbox("namedThings", null, true, ExtraIcon.None);
			this.AddShowOnlyCheckbox("describedThings", "Things with description", false, ExtraIcon.None);
			this.AddCheckboxSpacing();
			this.AddShowOnlyCheckbox("emitters", null, false, ExtraIcon.None);
			this.AddShowOnlyCheckbox("particleEmitters", null, false, ExtraIcon.None);
			break;
		case 5:
			this.AddShowOnlyCheckbox("videoOrSlideshowRelated", "video/ slideshow related", false, ExtraIcon.None);
			this.AddShowOnlyCheckbox("surroundSound", null, false, ExtraIcon.None);
			this.AddShowOnlyCheckbox("soundTracks", null, false, ExtraIcon.None);
			this.AddCheckboxSpacing();
			this.AddShowOnlyCheckbox("usedAsSky", null, false, ExtraIcon.UseTextureAsSky);
			break;
		case 6:
			this.AddShowOnlyCheckbox("includedSubThings", "Included Sub-Things", false, ExtraIcon.None);
			this.AddShowOnlyCheckbox("placedSubThingRelated", "Placed Sub-Things & containers", false, ExtraIcon.None);
			this.AddCheckboxSpacing();
			this.AddShowOnlyCheckbox("unwalkables", null, true, ExtraIcon.Unwalkable);
			this.AddShowOnlyCheckbox("climbables", null, true, ExtraIcon.Climbable);
			break;
		case 7:
		{
			this.AddShowOnlyCheckbox("everything", null, true, ExtraIcon.SeeInvisible);
			this.AddCheckboxSpacing();
			string text = ((!string.IsNullOrEmpty(HighlightThingsDialog.nameContainsText)) ? ("Contains \"" + Misc.Truncate(HighlightThingsDialog.nameContainsText, 14, true) + "\"") : "Name contains...");
			this.AddShowOnlyCheckbox("nameContains", text, true, ExtraIcon.None);
			break;
		}
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x0003A033 File Offset: 0x00038433
	private new void Update()
	{
		base.Update();
		base.ReactToOnClickInWrapper(this.expanderWrapper);
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0003A047 File Offset: 0x00038447
	private void SetBatchLockButtons(bool show)
	{
		if (Managers.areaManager.weAreOwnerOfCurrentArea)
		{
			if (this.expanderWrapper == null)
			{
				this.AddBatchLockButtons();
			}
			this.expanderWrapper.SetActive(show);
		}
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x0003A07C File Offset: 0x0003847C
	private void AddBatchLockButtons()
	{
		this.expanderWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.expanderWrapper);
		base.AddButton("lockAll", null, "Lock All", "ButtonCompactNoIconShortCentered", -190, -560, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.AddButton("unlockAll", null, "Unlock All", "ButtonCompactNoIconShortCentered", 190, -560, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0003A134 File Offset: 0x00038534
	private void ShowProcessing(int? amount = null)
	{
		base.HideHelpLabel();
		if (amount != null)
		{
			base.ShowHelpLabel("Processing " + amount.Value.ToString() + " placements, please wait...", 50, 0.7f, TextAlignment.Left, -700, false, false, 1f, TextColor.Default);
		}
		else
		{
			base.ShowHelpLabel("Processing, please wait...", 50, 0.7f, TextAlignment.Left, -700, false, false, 1f, TextColor.Default);
		}
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x0003A1BC File Offset: 0x000385BC
	private void SetAllPlacementsLock(bool doLock)
	{
		if (!this.isProcessingSetAll)
		{
			this.placementIdsToLock = new List<string>();
			this.placementIdsToUnlock = new List<string>();
			Component[] componentsInChildren = Managers.thingManager.placements.transform.GetComponentsInChildren(typeof(Thing), true);
			foreach (Thing thing in componentsInChildren)
			{
				if (thing.IsPlacement())
				{
					if (doLock)
					{
						if (!thing.isLocked)
						{
							this.placementIdsToLock.Add(thing.placementId);
						}
					}
					else if (thing.isLocked)
					{
						this.placementIdsToUnlock.Add(thing.placementId);
					}
				}
			}
			if (this.placementIdsToLock.Count >= 1)
			{
				this.isProcessingSetAll = true;
				this.ShowProcessing(new int?(this.placementIdsToLock.Count));
				this.SetAllPlacementsLockNext();
			}
			else if (this.placementIdsToUnlock.Count >= 1)
			{
				this.isProcessingSetAll = true;
				this.ShowProcessing(new int?(this.placementIdsToUnlock.Count));
				this.SetAllPlacementsLockNext();
			}
			else
			{
				base.HideHelpLabel();
				base.ShowHelpLabel("None found for processing.", 50, 0.7f, TextAlignment.Left, -700, false, false, 1f, TextColor.Default);
			}
		}
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x0003A310 File Offset: 0x00038710
	private void SetAllPlacementsLockNext()
	{
		if (this.placementIdsToLock.Count >= 1)
		{
			string text = this.placementIdsToLock.First<string>();
			this.placementIdsToLock.RemoveAt(0);
			Managers.areaManager.SetPlacementAttribute(text, PlacementAttribute.Locked, delegate(bool ok)
			{
				if (ok)
				{
					if (this.placementIdsToLock.Count >= 1)
					{
						this.SetAllPlacementsLockNext();
					}
					else
					{
						Managers.soundManager.Play("success", this.transform, 0.15f, false, false);
						this.isProcessingSetAll = false;
						Managers.broadcastNetworkManager.RaiseEvent_ReloadArea();
					}
				}
				else
				{
					Managers.soundManager.Play("no", this.transform, 0.15f, false, false);
					this.isProcessingSetAll = false;
				}
			});
		}
		else if (this.placementIdsToUnlock.Count >= 1)
		{
			string text2 = this.placementIdsToUnlock.First<string>();
			this.placementIdsToUnlock.RemoveAt(0);
			Managers.areaManager.ClearPlacementAttribute(text2, PlacementAttribute.Locked, delegate(bool ok)
			{
				if (ok)
				{
					if (this.placementIdsToUnlock.Count >= 1)
					{
						this.SetAllPlacementsLockNext();
					}
					else
					{
						Managers.soundManager.Play("success", this.transform, 0.15f, false, false);
						this.isProcessingSetAll = false;
						Managers.broadcastNetworkManager.RaiseEvent_ReloadArea();
					}
				}
				else
				{
					Managers.soundManager.Play("no", this.transform, 0.15f, false, false);
					this.isProcessingSetAll = false;
				}
			});
		}
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x0003A3A4 File Offset: 0x000387A4
	private void AddShowOnlyCheckbox(string thisMode, string label = null, bool editorOnly = false, ExtraIcon extraIcon = ExtraIcon.None)
	{
		if (label == null)
		{
			label = Misc.CamelCaseToSpaceSeparated(thisMode);
		}
		bool flag = this.mode == thisMode;
		string text = string.Empty;
		string text2;
		if (flag)
		{
			text2 = label;
			label = string.Concat(new object[] { text2, " (", this.foundCount, ")" });
		}
		else if (editorOnly)
		{
			text = " editor only";
		}
		TextColor textColor = TextColor.Default;
		string text3 = "showOnly";
		bool flag2 = editorOnly && !Managers.areaManager.weAreEditorOfCurrentArea;
		if (flag2)
		{
			textColor = TextColor.Gray;
			text3 = "inaccessibleShowOnly";
		}
		int num = (int)(-300f + this.rowOffset * 115f);
		text2 = text3;
		string text4 = label;
		int num2 = 0;
		int num3 = num;
		bool flag3 = flag;
		float num4 = 0.8f;
		TextColor textColor2 = textColor;
		GameObject gameObject = base.AddCheckbox(text2, thisMode, text4, num2, num3, flag3, num4, "Checkbox", textColor2, text, extraIcon);
		base.AddCheckboxHelpButton(thisMode + "_help", num);
		this.rowOffset += 1f;
	}

	// Token: 0x06000955 RID: 2389 RVA: 0x0003A4C2 File Offset: 0x000388C2
	private void AddCheckboxSpacing()
	{
		this.rowOffset += 0.4f;
	}

	// Token: 0x06000956 RID: 2390 RVA: 0x0003A4D8 File Offset: 0x000388D8
	private void ShowOnlyCertainThingsInArea()
	{
		this.ResetHighlights();
		if (this.mode == "mostParts")
		{
			this.ShowOnlyCertainThingsInArea_MostParts(25, false);
		}
		else if (this.mode == "mostPartsAround")
		{
			this.ShowOnlyCertainThingsInArea_MostParts(3, true);
		}
		else if (this.mode == "peopleAttachments")
		{
			this.ShowOnlyCertainThingsInArea_People();
		}
		else
		{
			Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Thing), true);
			int num = 0;
			foreach (Thing thing in componentsInChildren)
			{
				num++;
				bool flag = false;
				Component[] componentsInChildren2 = thing.GetComponentsInChildren(typeof(ThingPart), true);
				string text = this.mode;
				switch (text)
				{
				case "lights":
					foreach (ThingPart thingPart in componentsInChildren2)
					{
						flag = thingPart.materialType == MaterialTypes.PointLight || thingPart.materialType == MaterialTypes.SpotLight;
						if (flag)
						{
							break;
						}
					}
					break;
				case "particleEmitters":
					foreach (ThingPart thingPart2 in componentsInChildren2)
					{
						flag = thingPart2.materialType == MaterialTypes.Particles || thingPart2.materialType == MaterialTypes.ParticlesBig || thingPart2.particleSystemType != ParticleSystemType.None;
						if (flag)
						{
							break;
						}
					}
					break;
				case "videoOrSlideshowRelated":
					foreach (ThingPart thingPart3 in componentsInChildren2)
					{
						flag = thingPart3.offersScreen || thingPart3.isVideoButton || thingPart3.offersSlideshowScreen || thingPart3.isSlideshowButton;
						if (flag)
						{
							break;
						}
					}
					break;
				case "placedSubThingRelated":
					foreach (ThingPart thingPart4 in componentsInChildren2)
					{
						flag = thingPart4.placedSubThingIdsWithOriginalInfo.Count >= 1;
						if (flag)
						{
							break;
						}
					}
					break;
				case "usedAsSky":
					foreach (ThingPart thingPart5 in componentsInChildren2)
					{
						flag = thingPart5.useTextureAsSky;
						if (flag)
						{
							break;
						}
					}
					break;
				case "rightsSettingScripts":
				case "emitters":
				case "thingsWhichSendToPlace":
				case "tellAnyThings":
				case "soundTracks":
					flag = this.ContainsThisStateListener(componentsInChildren2, this.mode);
					break;
				case "usesBehaviorScriptVariables":
					flag = this.ContainsThisStateListener(componentsInChildren2, this.mode);
					break;
				case "surroundSound":
					flag = thing.hasSurroundSound;
					if (!flag)
					{
						foreach (ThingPart thingPart6 in componentsInChildren2)
						{
							flag = thingPart6.videoScreenHasSurroundSound;
							if (flag)
							{
								break;
							}
						}
					}
					if (!flag)
					{
						flag = this.ContainsThisStateListener(componentsInChildren2, this.mode);
					}
					break;
				case "newest":
					flag = num > componentsInChildren.Length - 25;
					break;
				case "oldest":
					flag = num <= 25;
					break;
				case "unwalkables":
					flag = thing.isUnwalkable;
					break;
				case "climbables":
					flag = thing.isClimbable;
					break;
				case "holdables":
					flag = thing.isHoldable;
					break;
				case "clonables":
					flag = thing.isClonable;
					break;
				case "benefitsFromShowingAtDistance":
					flag = thing.benefitsFromShowingAtDistance;
					break;
				case "everything":
					flag = true;
					break;
				case "namedThings":
					flag = thing.name != CreationHelper.thingDefaultName;
					break;
				case "describedThings":
					flag = !string.IsNullOrEmpty(thing.description);
					break;
				case "includedSubThings":
					flag = thing.IsIncludedSubThing();
					break;
				case "specialShowDistance":
				{
					float? distanceToShow = thing.distanceToShow;
					flag = distanceToShow != null;
					break;
				}
				case "lockedThings":
					flag = thing.isLocked && thing.IsPlacement();
					break;
				case "unlockedThings":
					flag = !thing.isLocked && thing.IsPlacement();
					break;
				case "nameContains":
					flag = thing.givenName.ToLower().IndexOf(HighlightThingsDialog.nameContainsText) >= 0;
					break;
				}
				if (flag)
				{
					this.Highlight(thing);
				}
			}
		}
	}

	// Token: 0x06000957 RID: 2391 RVA: 0x0003AB24 File Offset: 0x00038F24
	private bool ContainsThisStateListener(Component[] parts, string mode)
	{
		bool flag = false;
		foreach (ThingPart thingPart in parts)
		{
			foreach (ThingPartState thingPartState in thingPart.states)
			{
				foreach (StateListener stateListener in thingPartState.listeners)
				{
					switch (mode)
					{
					case "rightsSettingScripts":
						flag = stateListener.rights != null;
						break;
					case "emitters":
						flag = !string.IsNullOrEmpty(stateListener.emitId);
						break;
					case "thingsWhichSendToPlace":
						flag = !string.IsNullOrEmpty(stateListener.transportToArea) || !string.IsNullOrEmpty(stateListener.transportOntoThing);
						break;
					case "tellAnyThings":
						if (stateListener.tells != null)
						{
							foreach (KeyValuePair<TellType, string> keyValuePair in stateListener.tells)
							{
								TellType key = keyValuePair.Key;
								flag = key == TellType.Any || key == TellType.Nearby || key == TellType.FirstOfAny;
								if (flag)
								{
									break;
								}
							}
						}
						break;
					case "usesBehaviorScriptVariables":
						flag = stateListener.eventType == StateListener.EventType.OnVariableChange || stateListener.variableOperations != null;
						break;
					case "surroundSound":
						if (stateListener.sounds != null)
						{
							foreach (Sound sound in stateListener.sounds)
							{
								flag = sound.surround;
								if (flag)
								{
									break;
								}
							}
						}
						break;
					case "soundTracks":
						flag = stateListener.soundTrackData != null;
						break;
					}
					if (flag)
					{
						return flag;
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06000958 RID: 2392 RVA: 0x0003AE54 File Offset: 0x00039254
	private void ResetHighlights()
	{
		Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
		this.RemoveHighlightMaterials();
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x0003AE6B File Offset: 0x0003926B
	private void Highlight(Thing thing)
	{
		Managers.thingManager.AddOutlineHighlightMaterial(thing, false);
		thing.gameObject.SetActive(true);
		this.foundCount++;
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x0003AE94 File Offset: 0x00039294
	private void RemoveHighlightMaterials()
	{
		if (Managers.thingManager.placements != null)
		{
			Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Thing), true);
			Component[] componentsInChildren2 = Managers.personManager.People.GetComponentsInChildren(typeof(Thing), true);
			Component[] array = componentsInChildren.Union(componentsInChildren2).ToArray<Component>();
			foreach (Thing thing in array)
			{
				Managers.thingManager.RemoveOutlineHighlightMaterial(thing, false);
			}
			this.foundCount = 0;
		}
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x0003AF34 File Offset: 0x00039334
	private void ShowOnlyCertainThingsInArea_MostParts(int amountToShow, bool onlyHighlightIfObjectActive = false)
	{
		GameObject[] childrenAsArray = Misc.GetChildrenAsArray(Managers.thingManager.placements.transform);
		Thing[] array = new Thing[childrenAsArray.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = childrenAsArray[i].GetComponent<Thing>();
		}
		array = array.OrderByDescending((Thing x) => x.thingPartCount).ToArray<Thing>();
		int num = 0;
		while (num < array.Length && num < amountToShow)
		{
			if (!onlyHighlightIfObjectActive || array[num].gameObject.activeSelf)
			{
				this.Highlight(array[num]);
			}
			num++;
		}
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x0003AFE4 File Offset: 0x000393E4
	private void ShowOnlyCertainThingsInArea_People()
	{
		Component[] componentsInChildren = Managers.personManager.People.GetComponentsInChildren(typeof(Thing), true);
		foreach (Thing thing in componentsInChildren)
		{
			this.Highlight(thing);
		}
	}

	// Token: 0x0600095D RID: 2397 RVA: 0x0003B032 File Offset: 0x00039432
	private void OnDestroy()
	{
		this.ResetHighlights();
		Our.SetSeeInvisibleAsEditor(this.seesInvisiblePartsBefore);
	}

	// Token: 0x0600095E RID: 2398 RVA: 0x0003B048 File Offset: 0x00039448
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "showOnly":
			this.mode = ((!state) ? null : contextId);
			if (this.mode == "nameContains" && state)
			{
				DialogManager dialogManager = Managers.dialogManager;
				Action<string> action = delegate(string text)
				{
					if (text != null)
					{
						HighlightThingsDialog.nameContainsText = text.ToLower();
					}
					base.SwitchTo(DialogType.HighlightThings, string.Empty);
				};
				string text2 = HighlightThingsDialog.nameContainsText;
				dialogManager.GetInput(action, string.Empty, text2, 60, string.Empty, false, false, false, false, 1f, false, false, null, false);
			}
			else
			{
				HighlightThingsDialog.nameContainsText = string.Empty;
				this.AddButtons();
				if (this.mode == null)
				{
					this.ResetHighlights();
				}
				else
				{
					this.ShowOnlyCertainThingsInArea();
				}
			}
			break;
		case "inaccessibleShowOnly":
			Managers.soundManager.Play("no", this.transform, 0.2f, false, false);
			this.AddButtons();
			break;
		case "previousPage":
		{
			int num = --this.page;
			if (num < 1)
			{
				this.page = 7;
			}
			this.AddButtons();
			break;
		}
		case "lockedThings_help":
			if (this.isProcessingSetAll)
			{
				this.ShowProcessing(null);
				this.SetBatchLockButtons(false);
			}
			else
			{
				bool flag = base.ToggleHelpLabel("Highlights placements which were locked by an editor in the placement context dialog.", -700, 1f, 50, 0.7f);
				this.SetBatchLockButtons(flag);
			}
			break;
		case "unlockedThings_help":
			if (this.isProcessingSetAll)
			{
				this.ShowProcessing(null);
				this.SetBatchLockButtons(false);
			}
			else
			{
				bool flag2 = base.ToggleHelpLabel("Highlights placements which are not locked in the placement context dialog.", -700, 1f, 50, 0.7f);
				this.SetBatchLockButtons(flag2);
			}
			break;
		case "lockAll":
			this.SetAllPlacementsLock(true);
			break;
		case "unlockAll":
			this.SetAllPlacementsLock(false);
			break;
		case "newest_help":
			base.ToggleHelpLabel("Highlights the most recently placed things in the area.", -700, 1f, 50, 0.7f);
			break;
		case "oldest_help":
			base.ToggleHelpLabel("Highlights the oldest placements made in the area.", -700, 1f, 50, 0.7f);
			break;
		case "mostParts_help":
			base.ToggleHelpLabel("Highlights the Things with most Thing Parts (after merging). Higher counts can increase area lag.", -700, 1f, 50, 0.7f);
			break;
		case "mostPartsAround_help":
			base.ToggleHelpLabel("Highlights the Things with most Thing Parts (after merging) nearby where you are. Higher counts can increase area lag.", -700, 1f, 50, 0.7f);
			break;
		case "lights_help":
			base.ToggleHelpLabel("Highlights light sources in the area.", -700, 1f, 50, 0.7f);
			break;
		case "unwalkables_help":
			base.ToggleHelpLabel("Highlights Things set to be Unwalkable.", -700, 1f, 50, 0.7f);
			break;
		case "climbables_help":
			base.ToggleHelpLabel("Highlights Things set to be Climbable.", -700, 1f, 50, 0.7f);
			break;
		case "benefitsFromShowingAtDistance_help":
			base.ToggleHelpLabel("Highlights Things using the \"Show if far away too\" attribute.", -700, 1f, 50, 0.7f);
			break;
		case "clonables_help":
			base.ToggleHelpLabel("Highlights Things which the creator made clonable, to be edited by others.", -700, 1f, 50, 0.7f);
			break;
		case "holdables_help":
			base.ToggleHelpLabel("Highlights Things which can be picked up by area visitors in non-\"Change Things\" mode.", -700, 1f, 50, 0.7f);
			break;
		case "thingsWhichSendToPlace_help":
			base.ToggleHelpLabel("Highlights placements containing teleporting scripts, like \"then send nearyb...\".", -700, 1f, 50, 0.7f);
			break;
		case "rightsSettingScripts_help":
			base.ToggleHelpLabel("Highlights placements containing scripts with \"then allow/ disallow\" commands.", -700, 1f, 50, 0.7f);
			break;
		case "tellAnyThings_help":
			base.ToggleHelpLabel("Highlights placements containing \"then tell any/ nearby\" scripts.", -700, 1f, 50, 0.7f);
			break;
		case "usesBehaviorScriptVariables_help":
			base.ToggleHelpLabel("Highlights placements containing \"when is/ then is\" variables scripting (which at higher count can increase area synchronization messaging lag).", -700, 1f, 50, 0.7f);
			break;
		case "peopleAttachments_help":
			base.ToggleHelpLabel("Highlights the bodies of people.", -700, 1f, 50, 0.7f);
			break;
		case "namedThings_help":
			base.ToggleHelpLabel("Highlights Things which were given a name that's not just \"Thing\".", -700, 1f, 50, 0.7f);
			break;
		case "describedThings_help":
			base.ToggleHelpLabel("Highlights Things which were given a text description. Descriptions show in the placement context dialog.", -700, 1f, 50, 0.7f);
			break;
		case "emitters_help":
			base.ToggleHelpLabel("Highlights Things which emit something, like cannons.", -700, 1f, 50, 0.7f);
			break;
		case "particleEmitters_help":
			base.ToggleHelpLabel("Highlights placements which emit particles.", -700, 1f, 50, 0.7f);
			break;
		case "specialShowDistance_help":
			base.ToggleHelpLabel("Highlights placements which, via the placement context dialog backside, were given a special \"Show at...\" meters value, overriding their default distance showing behavior.", -700, 1f, 50, 0.7f);
			break;
		case "videoOrSlideshowRelated_help":
			base.ToggleHelpLabel("Highlights Things that contain screens for video, browsers or slideshows, or buttons which start those.", -700, 1f, 50, 0.7f);
			break;
		case "surroundSound_help":
			base.ToggleHelpLabel("Highlights Things, like screens, using the \"Surround Sound\" attribute.", -700, 1f, 50, 0.7f);
			break;
		case "soundTracks_help":
			base.ToggleHelpLabel("Highlights Things which use the \"play track\" script command to play background music for the area.", -700, 1f, 50, 0.7f);
			break;
		case "usedAsSky_help":
			base.ToggleHelpLabel("Highlights Things used to determine what the area's sky texture looks like.", -700, 1f, 50, 0.7f);
			break;
		case "includedSubThings_help":
			base.ToggleHelpLabel("Highlights Things which make use of Included Sub-Things.", -700, 1f, 50, 0.7f);
			break;
		case "placedSubThingRelated_help":
			base.ToggleHelpLabel("Highlights Things which are Placed Sub-Things, or move them (see the back of a part's Sub-Thing dialog).", -700, 1f, 50, 0.7f);
			break;
		case "everything_help":
			base.ToggleHelpLabel("Highlights every object in the area.", -700, 1f, 50, 0.7f);
			break;
		case "nameContains_help":
			base.ToggleHelpLabel("Highlights objects whose name contain a certain string specified by you, e.g. if you enter \"red\" it would show things named \"red tree\", \"fred\" and so on.", -700, 1f, 50, 0.7f);
			break;
		case "nextPage":
		{
			base.HideHelpLabel();
			int num = ++this.page;
			if (num > 7)
			{
				this.page = 1;
			}
			this.AddButtons();
			break;
		}
		case "back":
			base.SwitchTo(DialogType.Area, string.Empty);
			break;
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x0400070C RID: 1804
	private string mode;

	// Token: 0x0400070D RID: 1805
	private int page = 1;

	// Token: 0x0400070E RID: 1806
	private const int maxPages = 7;

	// Token: 0x0400070F RID: 1807
	private float rowOffset;

	// Token: 0x04000710 RID: 1808
	private const int byPartCountMax = 25;

	// Token: 0x04000711 RID: 1809
	private const int newestOrOldestMax = 25;

	// Token: 0x04000712 RID: 1810
	private const int byPartCountNearbyMax = 3;

	// Token: 0x04000713 RID: 1811
	private bool seesInvisiblePartsBefore;

	// Token: 0x04000714 RID: 1812
	private int foundCount;

	// Token: 0x04000715 RID: 1813
	private GameObject expanderWrapper;

	// Token: 0x04000716 RID: 1814
	public static string nameContainsText = string.Empty;

	// Token: 0x04000717 RID: 1815
	private List<string> placementIdsToLock;

	// Token: 0x04000718 RID: 1816
	private List<string> placementIdsToUnlock;

	// Token: 0x04000719 RID: 1817
	private bool isProcessingSetAll;
}
