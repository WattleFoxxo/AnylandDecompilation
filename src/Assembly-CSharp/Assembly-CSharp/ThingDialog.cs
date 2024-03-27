using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000140 RID: 320
public class ThingDialog : Dialog
{
	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000C04 RID: 3076 RVA: 0x000684D1 File Offset: 0x000668D1
	// (set) Token: 0x06000C05 RID: 3077 RVA: 0x000684D9 File Offset: 0x000668D9
	public bool canMoveFromFarAway { get; private set; }

	// Token: 0x06000C06 RID: 3078 RVA: 0x000684E4 File Offset: 0x000668E4
	public void Start()
	{
		this.openingTime = Time.time;
		this.weAreResized = Managers.personManager.WeAreResized();
		if (this.weAreResized)
		{
			base.gameObject.SetActive(false);
		}
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		bool showThingThumbObject = false;
		string placedSubThingMasterId = string.Empty;
		this.CreateSliderValueToScaleLookupIfNeeded();
		if (this.thing != null)
		{
			this.thingId = this.thing.thingId;
			this.thingObject = this.thing.gameObject;
		}
		else if (this.hand.lastContextInfoHit != null)
		{
			Our.thingIdOfInterest = null;
			this.thingObject = this.hand.lastContextInfoHit;
			this.thing = this.thingObject.GetComponent<Thing>();
			this.thingId = this.thing.thingId;
		}
		else if (Our.thingIdOfInterest != null)
		{
			this.thingObject = null;
			this.thing = null;
			this.thingId = Our.thingIdOfInterest;
			Our.thingIdOfInterest = null;
			showThingThumbObject = true;
		}
		placedSubThingMasterId = this.GetPlacedSubThingMasterId();
		this.allowBrowserOnThisThingType = this.thing != null && (this.thing.IsPlacement() || Managers.personManager.GetIsThisObjectOfOurPerson(this.thing.gameObject, false));
		if (this.allowBrowserOnThisThingType && this.autoForwardIfContainsBrowser)
		{
			Browser componentInChildren = this.thing.GetComponentInChildren<Browser>();
			if (componentInChildren != null && componentInChildren.allowUrlNavigation)
			{
				this.SwitchToBrowserDialog(componentInChildren);
				return;
			}
		}
		this.AddSpecialFeatureButtonIfNeeded();
		if (this.thingId != null)
		{
			if (!this.weAreResized && this.thing != null && !string.IsNullOrEmpty(this.thing.placementId))
			{
				string currentAreaId = Managers.areaManager.currentAreaId;
				Managers.thingManager.GetPlacementInfo(currentAreaId, this.thing.placementId, delegate(PlacementInfo placementInfo)
				{
					if (this == null)
					{
						return;
					}
					string text4 = ((!string.IsNullOrEmpty(placementInfo.copiedVia)) ? "paster" : "placer");
					this.AddUserInfo(text4, placementInfo.placerId, placementInfo.placerName, placementInfo.placedDaysAgo, -180, false, true, placedSubThingMasterId, placementInfo.copiedVia);
				});
			}
			if (this.thing != null && this.thing.IsIncludedSubThing())
			{
				base.AddButton("includedSubThingOf", null, "Sub-Thing of...", "ButtonCompactSmallIcon", 0, -160, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
			Managers.thingManager.GetThingInfo(this.thingId, delegate(ThingInfo thingInfo)
			{
				if (this == null)
				{
					return;
				}
				if (thingInfo != null)
				{
					this.AddInfo(thingInfo, showThingThumbObject);
					this.AddBacksideStats(placedSubThingMasterId != string.Empty, thingInfo);
					this.UpdateScale();
				}
				else
				{
					Managers.dialogManager.ShowInfo("This creation is not available anymore.", false, true, 1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
				}
			});
			if (this.thing != null && !string.IsNullOrEmpty(this.thing.description))
			{
				string text = Misc.Truncate(this.thing.description, 225, true);
				base.ShowHelpLabel(text, 50, 0.7f, TextAlignment.Left, -700, true, false, 1f, TextColor.Default);
			}
		}
		this.UpdateLockButton();
		this.UpdateInvisibleToEditorsButton();
		this.AddControllableControlButtonIfNeeded();
		this.UpdateScaleSlider();
		if (Managers.areaManager.weAreEditorOfCurrentArea && Our.mode == EditModes.Area && (!(this.thing != null) || !this.thing.IsIncludedSubThing()))
		{
			string text2 = "grip";
			if (CrossDevice.type == global::DeviceType.OculusTouch)
			{
				text2 = ((!CrossDevice.oculusTouchLegacyMode) ? "A/X" : "thumbstick");
			}
			else if (CrossDevice.type == global::DeviceType.Index)
			{
				text2 = "(A)";
			}
			bool flag = this.thingId == "000000000000000000000001";
			this.canMoveFromFarAway = !flag || Managers.personManager.ourPerson.ageInSecs > 600;
			string text3 = string.Concat(new string[]
			{
				"You can ",
				(!this.canMoveFromFarAway) ? "later" : "now",
				" grab into empty air to move this.",
				Environment.NewLine,
				"Press ",
				text2,
				" to show delete button."
			});
			base.AddLabel(text3, 430, -190, 0.7f, true, TextColor.Gray, false, TextAlignment.Left, -1, 1f, true, TextAnchor.MiddleLeft);
		}
		Managers.achievementManager.RegisterAchievement(Achievement.ContextLaseredThing);
		base.AddSeparator(0, -50, false);
		base.AddSeparator(0, 130, false);
		base.AddSeparator(0, 250, false);
		if (this.thing != null && Our.contextHighlightPlacements)
		{
			Managers.thingManager.AddOutlineHighlightMaterial(this.thing, true);
		}
	}

	// Token: 0x06000C07 RID: 3079 RVA: 0x0006899F File Offset: 0x00066D9F
	public float SecondsSinceOpened()
	{
		return Time.time - this.openingTime;
	}

	// Token: 0x06000C08 RID: 3080 RVA: 0x000689B0 File Offset: 0x00066DB0
	private void UpdateScaleSlider()
	{
		if (this.scaleSlider != null)
		{
			global::UnityEngine.Object.Destroy(this.scaleSlider.gameObject);
		}
		if (this.thing != null && !string.IsNullOrEmpty(this.thing.placementId) && !this.thing.isLocked && Managers.areaManager.weAreEditorOfCurrentArea && Universe.features.changeThings && !this.thing.movableByEveryone)
		{
			float x = this.thing.transform.localScale.x;
			this.scaleAtStart = new float?(x);
			float num = 1f;
			if (x < 1f)
			{
				num = x;
			}
			else if (x > 1f)
			{
				num = this.GetSliderValueFromScale(x);
				this.scaleAtStart = new float?(this.GetSliderScaleFromValue(num));
			}
			float? num2 = this.scaleAtStart;
			this.lastScaleSyncedToOthers = new Vector3?(Misc.GetUniformVector3(num2.Value));
			this.scaleSlider = base.AddSlider("Size", string.Empty, 0, 40, 0f, 2f, false, num, new Action<float>(this.OnScaleChange), false, 1f);
			this.OnScaleChange(num);
		}
	}

	// Token: 0x06000C09 RID: 3081 RVA: 0x00068B00 File Offset: 0x00066F00
	private void CreateSliderValueToScaleLookupIfNeeded()
	{
		if (ThingDialog.sliderValueToScale == null)
		{
			ThingDialog.sliderValueToScale = new Dictionary<float, float>();
			for (float num = 1f; num <= 2f; num += 0.001f)
			{
				float sliderScaleFromValue = this.GetSliderScaleFromValue(num);
				ThingDialog.sliderValueToScale.Add(num, sliderScaleFromValue);
			}
		}
	}

	// Token: 0x06000C0A RID: 3082 RVA: 0x00068B54 File Offset: 0x00066F54
	private float GetSliderValueFromScale(float scale)
	{
		float num = 1f;
		float? num2 = null;
		foreach (KeyValuePair<float, float> keyValuePair in ThingDialog.sliderValueToScale)
		{
			float value = keyValuePair.Value;
			float num3 = Mathf.Abs(value - scale);
			if (num2 == null || num3 < num2)
			{
				num2 = new float?(num3);
				num = keyValuePair.Key;
			}
		}
		return num;
	}

	// Token: 0x06000C0B RID: 3083 RVA: 0x00068C0C File Offset: 0x0006700C
	private float GetSliderScaleFromValue(float value)
	{
		float num = Mathf.Pow(value, 2f) * 2f;
		float num2 = Mathf.Pow(value, num);
		return Misc.ClampMax(num2, 250f);
	}

	// Token: 0x06000C0C RID: 3084 RVA: 0x00068C48 File Offset: 0x00067048
	private void OnScaleChange(float value)
	{
		if (this.thing != null)
		{
			float num = 1f;
			if (Mathf.Abs(value - 1f) <= 0.05f)
			{
				value = 1f;
			}
			if (value < 1f)
			{
				num = value;
				num = Misc.ClampMin(num, 0.05f);
				this.scaleSlider.valueSuffix = " X" + num.ToString("F" + 2);
			}
			else if (value > 1f)
			{
				num = this.GetSliderScaleFromValue(value);
				if (num < 10f)
				{
					this.scaleSlider.valueSuffix = " X" + num.ToString("F" + 1);
				}
				else
				{
					this.scaleSlider.valueSuffix = " X" + ((int)Mathf.Round(num)).ToString();
				}
			}
			else
			{
				this.scaleSlider.valueSuffix = string.Empty;
			}
			this.targetScale = new Vector3?(Misc.GetUniformVector3(num));
			base.CancelInvoke("SyncPlacementScaleToOthers");
			base.Invoke("SyncPlacementScaleToOthers", 0.25f);
		}
	}

	// Token: 0x06000C0D RID: 3085 RVA: 0x00068D90 File Offset: 0x00067190
	private void SyncPlacementScaleToOthers()
	{
		if (this.thing != null)
		{
			Vector3? vector = this.targetScale;
			if (vector != null)
			{
				Vector3? vector2 = this.lastScaleSyncedToOthers;
				bool flag = vector2 != null;
				Vector3? vector3 = this.targetScale;
				if (flag != (vector3 != null) || (vector2 != null && vector2.GetValueOrDefault() != vector3.GetValueOrDefault()))
				{
					this.lastScaleSyncedToOthers = this.targetScale;
					PersonManager personManager = Managers.personManager;
					Thing thing = this.thing;
					Vector3? vector4 = this.targetScale;
					personManager.DoUpdatePlacementScale(thing, vector4.Value);
				}
			}
		}
	}

	// Token: 0x06000C0E RID: 3086 RVA: 0x00068E3C File Offset: 0x0006723C
	public void UpdateLockButton()
	{
		global::UnityEngine.Object.Destroy(this.lockButton);
		if (this.thing != null && !string.IsNullOrEmpty(this.thing.placementId) && Managers.areaManager.WeCanChangeLocks())
		{
			this.lockButton = base.AddModelButton("Lock", "toggleLock", null, -210, 405, false);
			this.lockButton.GetComponent<DialogPart>().autoStopHighlight = false;
			base.ApplyEmissionColorToShape(this.lockButton, this.thing.isLocked);
		}
	}

	// Token: 0x06000C0F RID: 3087 RVA: 0x00068ED4 File Offset: 0x000672D4
	public void UpdateInvisibleToEditorsButton()
	{
		global::UnityEngine.Object.Destroy(this.invisibleToEditorsButton);
		if (this.thing != null && !string.IsNullOrEmpty(this.thing.placementId) && Managers.areaManager.weAreEditorOfCurrentArea && Universe.features.changeThings && !this.thing.isLocked)
		{
			this.invisibleToEditorsButton = base.AddModelButton("InvisibleToEditors", "toggleInvisibleToEditors", null, -100, 405, false);
			this.invisibleToEditorsButton.GetComponent<DialogPart>().autoStopHighlight = false;
			base.ApplyEmissionColorToShape(this.invisibleToEditorsButton, this.thing.isInvisibleToEditors);
		}
	}

	// Token: 0x06000C10 RID: 3088 RVA: 0x00068F88 File Offset: 0x00067388
	private string GetPlacedSubThingMasterId()
	{
		string empty = string.Empty;
		if (this.thing != null && this.thing.transform.parent != null && this.thing.transform.parent.CompareTag("PlacedSubThings") && this.thing.transform.parent.parent != null)
		{
			Thing component = this.thing.transform.parent.parent.GetComponent<Thing>();
			if (component != null)
			{
				empty = component.thingId;
			}
		}
		return empty;
	}

	// Token: 0x06000C11 RID: 3089 RVA: 0x00069035 File Offset: 0x00067435
	private void MakeThingClonableForLocalDebugging()
	{
	}

	// Token: 0x06000C12 RID: 3090 RVA: 0x00069038 File Offset: 0x00067438
	private void AddBacksideStats(bool isPlacedSubThing, ThingInfo info)
	{
		if (this.thing != null)
		{
			Component[] componentsInChildren = this.thing.gameObject.GetComponentsInChildren(typeof(ThingPart), true);
			string text = componentsInChildren.Length + " Thing Parts after merging";
			base.AddLabel(text, 430, -430, 0.8f, true, TextColor.Default, false, TextAlignment.Left, -1, 1f, true, TextAnchor.MiddleLeft);
			bool flag = (this.thing.isClonable || info.allCreatorsThingsClonable) && !this.thing.isNeverClonable;
			string text2 = Misc.BoolAsYesNo(flag);
			if ((flag && !this.thing.isClonable) || (!flag && (this.thing.isClonable || info.allCreatorsThingsClonable)))
			{
				text2 = "Indirectly " + text2;
			}
			string text3 = string.Empty;
			text3 = text3 + "Clonable: " + text2 + Environment.NewLine;
			text3 = text3 + "Show if far away: " + Misc.BoolAsYesNo(this.thing.benefitsFromShowingAtDistance) + Environment.NewLine;
			text3 = text3 + "Unwalkable: " + Misc.BoolAsYesNo(this.thing.isUnwalkable) + Environment.NewLine;
			text3 = text3 + "Things version: " + this.thing.version.ToString();
			base.AddLabel(text3, 430, -390, 0.8f, true, TextColor.Gray, false, TextAlignment.Left, -1, 0.9f, true, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x06000C13 RID: 3091 RVA: 0x000691C8 File Offset: 0x000675C8
	private int GetSubMeshCount(Thing thing)
	{
		int num = 0;
		Component[] componentsInChildren = thing.gameObject.GetComponentsInChildren(typeof(MeshFilter), true);
		foreach (MeshFilter meshFilter in componentsInChildren)
		{
			Mesh mesh = meshFilter.mesh;
			num += mesh.subMeshCount;
		}
		return num;
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x00069228 File Offset: 0x00067628
	private void AddSpecialFeatureButton(string name, string label, bool isSecondInRow = false)
	{
		int num = -230;
		if (isSecondInRow)
		{
			num *= -1;
		}
		this.specialFeatureButton = base.AddButton(name, null, label + "...", "ButtonCompactNoIcon", num, 300, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x0006928C File Offset: 0x0006768C
	private void AddSpecialFeatureButtonIfNeeded()
	{
		if (this.thing != null)
		{
			Component[] componentsInChildren = this.thing.gameObject.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart in componentsInChildren)
			{
				if (thingPart.offersScreen)
				{
					Browser componentInChildren = this.thing.GetComponentInChildren<Browser>();
					this.specialFeatureThingPart = thingPart;
					if (!componentInChildren || componentInChildren.allowUrlNavigation)
					{
						this.AddSpecialFeatureButton("videoControl", "Video", false);
					}
					if (Universe.features.webBrowsing)
					{
						bool? webBrowsing = Managers.areaManager.rights.webBrowsing;
						if (webBrowsing.Value && this.allowBrowserOnThisThingType)
						{
							this.addedWebButton = true;
							this.AddSpecialFeatureButton("web", "Web", true);
						}
					}
					break;
				}
				if (thingPart.isCamera)
				{
					this.specialFeatureThingPart = thingPart;
					this.AddSpecialFeatureButton("cameraControl", "Camera", false);
					break;
				}
				if (thingPart.offersSlideshowScreen)
				{
					this.specialFeatureThingPart = thingPart;
					this.AddSpecialFeatureButton("slideshowControl", "Slideshow", false);
					break;
				}
			}
		}
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x000693C8 File Offset: 0x000677C8
	private void AddControllableControlButtonIfNeeded()
	{
		if (this.thing != null && this.thing.IsControllableWithThrust() && this.thing.IsPlacement())
		{
			this.AddSpecialFeatureButton("controllableControl", "Control", false);
		}
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x00069418 File Offset: 0x00067818
	private void AddUserInfo(string label, string userId, string userName, int daysAgo, int y, bool isCloned = false, bool offerUndoIfExists = false, string placedSubThingMasterIdToAddButtonFor = "", string wasPastedFromAreaId = "")
	{
		if (userId != "000000000000000000000001")
		{
			if (isCloned)
			{
				label += "*";
			}
			base.AddLabel(label + ":", -447, y - 10, 0.95f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			string text = Misc.Truncate(userName, 21, true);
			base.AddButton("userInfo", userId, text, "ButtonCompactSmallIcon", 0, y + 20, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			int num = y + 20;
			if (placedSubThingMasterIdToAddButtonFor != string.Empty)
			{
				GameObject gameObject = base.AddButton("showPlacedSubThingMaster", placedSubThingMasterIdToAddButtonFor, null, "ButtonSmall", 390, num, "subConnections", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				base.MinifyButton(gameObject, 1f, 1f, 0.55f, false);
			}
			else if (offerUndoIfExists && this.thing.GetHasRecentUndoState() && Managers.areaManager.weAreEditorOfCurrentArea && (!(this.thing != null) || !this.thing.isLocked))
			{
				base.AddLabel("just now", 250, y + 5, 0.5f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
				this.undoButton = base.AddButton("undo", null, null, "ButtonVerySmall", 420, num, "undo", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
			else if (!string.IsNullOrEmpty(wasPastedFromAreaId))
			{
				GameObject gameObject2 = base.AddButton("areaCopyCreditsDialog", wasPastedFromAreaId, "from...", "ButtonSmall", 390, num, null, false, 0.21f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				base.MinifyButton(gameObject2, 1f, 1f, 0.55f, false);
			}
			else
			{
				string text2 = ((daysAgo <= 0) ? "today" : (daysAgo + " days ago"));
				base.AddLabel(text2, 250, y, 0.7f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			}
		}
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00069690 File Offset: 0x00067A90
	private void AddInfo(ThingInfo info, bool showThingThumbObject = false)
	{
		base.AddFlexibleSizeHeadline(info.name, -450, -460, TextColor.Default, 25);
		bool flag = info.clonedFromId != null;
		this.weAreCreator = info.creatorId == Managers.personManager.ourPerson.userId;
		this.isUnlisted = info.isUnlisted;
		GameObject gameObject = base.AddButton("tags", null, "Tag", "ButtonSmall", -387, 410, null, false, 0.35f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.MinifyButton(gameObject, 1f, 1f, 0.55f, false);
		bool flag2 = false;
		if (this.thingObject != null && this.thing != null && !string.IsNullOrEmpty(this.thing.placementId) && Managers.areaManager.weAreEditorOfCurrentArea)
		{
			bool flag3 = (this.thing.isClonable || info.allCreatorsThingsClonable) && !this.thing.isNeverClonable;
			bool flag4 = this.weAreCreator || flag3;
			bool flag5 = Our.lastTransformHandled != null && Our.lastTransformHandled.GetComponent<ThingPart>() != null;
			if (Our.mode == EditModes.Thing)
			{
				if (flag4 || flag5)
				{
					this.cloneOrAddButton = base.AddButton("addToCreation", null, "Include...", "ButtonCompact", 230, 410, "includeThing", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
			}
			else if (flag4)
			{
				this.thingThatWasClonedFromIdIfRelevant = string.Empty;
				if (Universe.features.createThings)
				{
					this.offerCloning = true;
					flag2 = true;
					if (!this.weAreCreator || info.clonedFromId != null)
					{
						this.thingThatWasClonedFromIdIfRelevant = this.thingId;
					}
				}
			}
		}
		this.AddUserInfo("creator", info.creatorId, info.creatorName, info.createdDaysAgo, -300, flag, false, string.Empty, string.Empty);
		this.AddCollectedPlacedStats(info.collectedCount, info.placedCount);
		if (flag && !this.addedWebButton)
		{
			this.clonedFromButton = base.AddButton("clonedFrom", info.clonedFromId, "*based on...", "ButtonCompactSmallIcon", 230, 300, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			flag2 = true;
		}
		if (flag2)
		{
			this.UpdateCloneOrAddButton();
		}
		if (showThingThumbObject)
		{
			Vector3 vector = new Vector3(-0.075f, 0.025f, -0.02f);
			Managers.thingManager.InstantiateThingOnDialogViaCache(ThingRequestContext.ThingDialogAddInfo, this.thingId, this.transform, vector, 0.035f, false, false, 0f, 0f, 0f, false, false);
		}
		this.AddBacksideButtons(info);
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x000699AC File Offset: 0x00067DAC
	private void AddCollectedPlacedStats(int collectedCount, int placedCount)
	{
		if (this.deleteButton == null)
		{
			string text = string.Concat(new object[]
			{
				"Collected: ",
				collectedCount,
				" time",
				(collectedCount == 1) ? string.Empty : "s"
			});
			string text2 = string.Concat(new object[]
			{
				"Placed: ",
				placedCount,
				" time",
				(placedCount == 1) ? string.Empty : "s"
			});
			this.collectedLabel = base.AddLabel(text, -440, 165, 0.85f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			this.placedLabel = base.AddLabel(text2, 440, 165, 0.85f, false, TextColor.Default, false, TextAlignment.Right, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x00069A98 File Offset: 0x00067E98
	private void AddBacksideButtons(ThingInfo info)
	{
		if (this.weAreResized || this.thing == null)
		{
			return;
		}
		Managers.thingManager.GetThingFlagStatus(this.thingId, delegate(bool isFlagged)
		{
			if (this == null)
			{
				return;
			}
			bool flag = (this.thing.isClonable || info.allCreatorsThingsClonable) && !this.thing.isNeverClonable;
			bool flag2 = this.weAreCreator || flag;
			bool flag3 = this.weAreCreator || flag;
			this.backsideWrapper = this.GetUiWrapper();
			this.SetUiWrapper(this.backsideWrapper);
			if (!this.weAreCreator)
			{
				this.flagButton = this.AddFlag("flagCreation", isFlagged, string.Empty, true, -410, 420);
			}
			if (!isFlagged && flag2)
			{
				this.exportButton = this.AddButton("export", null, string.Empty, "ButtonSmall", 400, 390, "export", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, true, TextAlignment.Left, false, false);
			}
			if (flag3)
			{
				this.hearsAndTellsButton = this.AddButton("hearsAndTells", null, string.Empty, "ButtonSmall", 0, 390, "hears", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, true, TextAlignment.Left, false, false);
			}
			if (this.thing.IsPlacement())
			{
				if (!Managers.areaManager.weAreEditorOfCurrentArea)
				{
					bool? slowBuildCreation = Managers.areaManager.rights.slowBuildCreation;
					if (!slowBuildCreation.Value)
					{
						goto IL_225;
					}
				}
				this.slowBuildCreationButton = this.AddButton("slowBuildCreation", null, "Build Animation", "ButtonCompactSmallIcon", 0, 0, "slowBuildCreation", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
			IL_225:
			if (Managers.areaManager.weAreEditorOfCurrentArea)
			{
				this.AddButton("copyPaste", null, "Copy & Paste", "ButtonCompactNoIconShortCentered", 0, 190, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				int num = -315 * ((this.handSide != Side.Right) ? 1 : (-1));
				if (!this.thing.isLocked)
				{
					this.AddButton("suppress", null, "Enabled...", "ButtonCompactNoIconShortCentered", num, 190, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
				string text = "Show at...";
				float? distanceToShow = this.thing.distanceToShow;
				if (distanceToShow != null)
				{
					text = "Show at ";
					if (this.thing.distanceToShow >= 100000f)
					{
						text += "100+km";
					}
					else if (this.thing.distanceToShow >= 10000f)
					{
						object obj = text;
						float? distanceToShow2 = this.thing.distanceToShow;
						text = obj + Mathf.Round(distanceToShow2.Value / 1000f) + "km";
					}
					else if (this.thing.distanceToShow >= 1000f)
					{
						object obj2 = text;
						float? distanceToShow3 = this.thing.distanceToShow;
						text = obj2 + Math.Round((double)(distanceToShow3.Value / 1000f), 1) + "km";
					}
					else
					{
						object obj3 = text;
						float? distanceToShow4 = this.thing.distanceToShow;
						text = obj3 + Math.Round((double)distanceToShow4.Value, 2) + "m";
					}
				}
				this.AddButton("distanceToShow", null, text, "ButtonCompactNoIconShortCentered", -num, 190, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
			this.RotateBacksideWrapper();
			this.SetUiWrapper(this.gameObject);
		});
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x00069AF2 File Offset: 0x00067EF2
	public void CloneIfWeMay()
	{
		if (this.thing != null && !this.thing.isLocked && this.offerCloning)
		{
			this.HandleEditClone();
		}
		else
		{
			this.cloneWhenWeKnowWeMay = true;
		}
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00069B34 File Offset: 0x00067F34
	private void UpdateCloneOrAddButton()
	{
		global::UnityEngine.Object.Destroy(this.cloneOrAddButton);
		if (!this.thing.isLocked && this.offerCloning)
		{
			if (this.cloneWhenWeKnowWeMay)
			{
				this.cloneWhenWeKnowWeMay = false;
				this.HandleEditClone();
			}
			else
			{
				bool flag = this.specialFeatureThingPart == null && this.clonedFromButton == null;
				int num = ((!flag) ? 410 : 365);
				string text = ((!flag) ? "ButtonCompact" : "Button");
				this.cloneOrAddButton = base.AddButton("editClone", null, "Edit", text, 230, num, "createThing", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
		}
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x00069C14 File Offset: 0x00068014
	public void ShowDeleteButtonIfNeeded()
	{
		if (this.thing != null && this.thingObject != null)
		{
			Thing myRootThing = this.thing.GetMyRootThing();
			if (Managers.areaManager.weAreEditorOfCurrentArea && myRootThing.IsPlacement())
			{
				if (!myRootThing.isLocked && (!myRootThing.isInvisibleToEditors || Our.seeInvisibleAsEditor))
				{
					global::UnityEngine.Object.Destroy(this.placedLabel);
					global::UnityEngine.Object.Destroy(this.collectedLabel);
					global::UnityEngine.Object.Destroy(this.deleteButton);
					this.deleteButton = base.AddButton("delete", null, "Delete?", "ButtonCompact", 0, 190, null, false, 1f, TextColor.Red, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
				else
				{
					Managers.soundManager.Play("no", this.transform, 0.3f, false, false);
				}
			}
		}
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00069D10 File Offset: 0x00068110
	private void CreateThingDuplicates(int amount, bool nearby = false)
	{
		for (int i = 0; i < amount; i++)
		{
			Vector3 vector = global::UnityEngine.Random.insideUnitSphere * ((!nearby) ? 50f : 10f);
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.thingObject, vector, this.thingObject.transform.localRotation);
			gameObject.transform.parent = this.debugPlacements.transform;
		}
		Managers.soundManager.Play("putDown", this.transform, 0.75f, false, false);
		this.UpdateDebugClonesLabel();
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x00069DA4 File Offset: 0x000681A4
	private void ClearThingDuplicates()
	{
		IEnumerator enumerator = this.debugPlacements.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				global::UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		Managers.soundManager.Play("delete", this.transform, 0.75f, false, false);
		base.Invoke("UpdateDebugClonesLabel", 0.01f);
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x00069E40 File Offset: 0x00068240
	private void UpdateDebugClonesLabel()
	{
		int num = 0;
		if (this.debugPlacements != null)
		{
			num = this.debugPlacements.transform.childCount;
		}
		this.debugClonesLabel.text = ("Clones made: " + num.ToString()).ToUpper();
	}

	// Token: 0x06000C21 RID: 3105 RVA: 0x00069E98 File Offset: 0x00068298
	private IEnumerator DoClone(bool setClonableAttribute)
	{
		CreationHelper.thingThatWasClonedFrom = this.thingObject;
		Thing thing = CreationHelper.thingThatWasClonedFrom.GetComponent<Thing>();
		thing.UnassignMyPlacedSubThings();
		thing.RestoreOriginalPlacement(false);
		thing.ResetStates();
		CreationHelper.thingThatWasClonedFrom.SetActive(false);
		GameObject screenThingObject = Managers.videoManager.GetCurrentScreenThingObject(this.thingObject);
		if (screenThingObject != null && screenThingObject == this.thingObject)
		{
			Managers.videoManager.StopVideo(true);
		}
		Component browser = this.thingObject.GetComponentInChildren(typeof(Browser), true);
		if (browser != null)
		{
			global::UnityEngine.Object.Destroy(browser.gameObject);
		}
		CreationHelper.thingBeingEdited = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.ThingDialogDoClone, this.thingId, delegate(GameObject returnThing)
		{
			CreationHelper.thingBeingEdited = returnThing;
			Thing component = returnThing.GetComponent<Thing>();
			component.version = 9;
			if (setClonableAttribute && component != null)
			{
				component.isClonable = true;
			}
			component.distanceToShow = thing.distanceToShow;
			CreationHelper.alreadyExceededMaxThingPartCountOnCloning = component.transform.childCount > 1000;
		}, true, false, -1, null));
		CreationHelper.thingBeingEdited.transform.parent = Managers.thingManager.placements.transform;
		CreationHelper.thingBeingEdited.transform.localPosition = CreationHelper.thingThatWasClonedFrom.transform.localPosition;
		CreationHelper.thingBeingEdited.transform.localRotation = CreationHelper.thingThatWasClonedFrom.transform.localRotation;
		Our.SetMode(EditModes.Thing, false);
		Thing thingScript = CreationHelper.thingBeingEdited.GetComponent<Thing>();
		if (thingScript != null && thingScript.containsPlacedSubThings && !this.OneOrMorePlacedSubThingsArePlaced(thingScript))
		{
			QuestionDialog.mode = QuestionDialog.Mode.RetrievePlacedSubThings;
			base.SwitchTo(DialogType.Question, string.Empty);
		}
		else
		{
			base.SwitchTo(DialogType.Create, string.Empty);
		}
		if (thingScript != null)
		{
			thingScript.MemorizeOriginalTransform(true);
			thingScript.SetLightShadows(true, null);
		}
		yield break;
	}

	// Token: 0x06000C22 RID: 3106 RVA: 0x00069EBC File Offset: 0x000682BC
	private bool OneOrMorePlacedSubThingsArePlaced(Thing thing)
	{
		bool flag = false;
		ThingPart[] componentsInChildren = thing.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			foreach (KeyValuePair<string, ThingIdPositionRotation> keyValuePair in thingPart.placedSubThingIdsWithOriginalInfo)
			{
				string key = keyValuePair.Key;
				GameObject placementById = Managers.thingManager.GetPlacementById(key, true);
				if (placementById != null)
				{
					flag = true;
					return flag;
				}
			}
		}
		return flag;
	}

	// Token: 0x06000C23 RID: 3107 RVA: 0x00069F70 File Offset: 0x00068370
	private void HandlePlacementAttributeSuccess(string text)
	{
		global::UnityEngine.Object.Destroy(this.collectedLabel);
		global::UnityEngine.Object.Destroy(this.placedLabel);
		global::UnityEngine.Object.Destroy(this.placementAttributeInfoLabel);
		this.placementAttributeInfoLabel = base.AddLabel("✔ " + text, 0, 167, 0.75f, false, TextColor.Green, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
	}

	// Token: 0x06000C24 RID: 3108 RVA: 0x00069FE8 File Offset: 0x000683E8
	private void ReopenIfWeAreResized()
	{
		if (this.weAreResized)
		{
			base.SwitchTo(DialogType.Thing, string.Empty);
		}
	}

	// Token: 0x06000C25 RID: 3109 RVA: 0x0006A004 File Offset: 0x00068404
	private void ShowHearsAndTells()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		Component[] componentsInChildren = this.thing.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			foreach (ThingPartState thingPartState in thingPart.states)
			{
				foreach (StateListener stateListener in thingPartState.listeners)
				{
					if (!string.IsNullOrEmpty(stateListener.whenData))
					{
						string text = stateListener.whenData.Trim();
						StateListener.EventType eventType = stateListener.eventType;
						switch (eventType)
						{
						case StateListener.EventType.OnTold:
						case StateListener.EventType.OnToldByNearby:
						case StateListener.EventType.OnToldByAny:
						case StateListener.EventType.OnToldByBody:
							list2.Add(text);
							break;
						default:
							if (eventType == StateListener.EventType.OnHears || eventType == StateListener.EventType.OnHearsAnywhere)
							{
								list.Add(text);
							}
							break;
						}
					}
				}
			}
		}
		string text2 = string.Empty;
		text2 += "<h2>Hears</h2>";
		if (list.Count >= 1)
		{
			list.Sort();
			string text3 = string.Join(", ", list.ToArray());
			text2 += text3;
		}
		else
		{
			text2 += " -";
		}
		text2 = text2 + Environment.NewLine + Environment.NewLine;
		text2 += "<h2>Listens to tells</h2>";
		if (list2.Count >= 1)
		{
			list2.Sort();
			string text4 = string.Join(", ", list2.ToArray());
			text2 += text4;
		}
		else
		{
			text2 += " -";
		}
		GameObject gameObject = base.SwitchTo(DialogType.PageableText, string.Empty);
		PageableTextDialog component = gameObject.GetComponent<PageableTextDialog>();
		component.headline = "Hears & Tell listeners";
		component.text = text2;
	}

	// Token: 0x06000C26 RID: 3110 RVA: 0x0006A240 File Offset: 0x00068640
	private new void Update()
	{
		if (this.thing != null)
		{
			Vector3? vector = this.targetScale;
			if (vector != null)
			{
				Transform transform = this.thing.transform;
				Vector3 localScale = this.thing.transform.localScale;
				Vector3? vector2 = this.targetScale;
				transform.localScale = Vector3.Lerp(localScale, vector2.Value, 0.5f);
			}
		}
		base.ReactToOnClick();
		base.ReactToOnClickInWrapper(this.backsideWrapper);
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x0006A2BC File Offset: 0x000686BC
	public void ResolveToRootThing()
	{
		Thing myRootThing = this.thing.GetMyRootThing();
		if (myRootThing != this.thing)
		{
			this.thing = myRootThing;
			this.thingId = this.thing.thingId;
			this.thingObject = myRootThing.gameObject;
		}
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0006A30C File Offset: 0x0006870C
	private void OnDestroy()
	{
		if (this.thing != null && Our.contextHighlightPlacements)
		{
			Managers.thingManager.RemoveOutlineHighlightMaterial(this.thing, true);
		}
		if (this.thing != null)
		{
			float? num = this.scaleAtStart;
			if (num != null && !this.thing.isLocked && !string.IsNullOrEmpty(this.thing.placementId) && Managers.areaManager.weAreEditorOfCurrentArea && Universe.features.changeThings)
			{
				Vector3? vector = this.targetScale;
				if (vector != null)
				{
					Transform transform = this.thing.transform;
					Vector3? vector2 = this.targetScale;
					transform.localScale = vector2.Value;
				}
				float? num2 = this.scaleAtStart;
				if (num2.Value != this.thing.transform.localScale.x)
				{
					Managers.areaManager.UpdateThingPlacement(this.thing.gameObject);
				}
				this.SyncPlacementScaleToOthers();
			}
		}
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x0006A424 File Offset: 0x00068824
	private void SwitchToBrowserDialog(Browser browser)
	{
		GameObject gameObject = base.SwitchTo(DialogType.Browser, string.Empty);
		BrowserDialog component = gameObject.GetComponent<BrowserDialog>();
		component.browser = browser;
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x0006A44D File Offset: 0x0006884D
	private void AddCreateDuplicatesInterface()
	{
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x0006A450 File Offset: 0x00068850
	private void HandleEditClone()
	{
		if (this.thing.isLocked)
		{
			base.CloseDialog();
		}
		else
		{
			CreationHelper.thingThatWasClonedFromIdIfRelevant = this.thingThatWasClonedFromIdIfRelevant;
			Managers.achievementManager.RegisterAchievement(Achievement.ClonedSomething);
			bool flag = !string.IsNullOrEmpty(this.thingThatWasClonedFromIdIfRelevant) && !this.weAreCreator;
			base.StartCoroutine(this.DoClone(flag));
		}
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x0006A4BC File Offset: 0x000688BC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		Our.SetPreferentialHandSide(this.hand);
		switch (contextName)
		{
		case "flagCreation":
			if (!this.flagIsBeingToggled)
			{
				this.flagIsBeingToggled = true;
				Managers.thingManager.ToggleThingFlag(this.thingId, delegate(bool isFlagged)
				{
					this.UpdateFlag(isFlagged, false, contextName);
					if (isFlagged)
					{
						Misc.DestroyMultiple(new GameObject[] { this.exportButton, this.hearsAndTellsButton });
					}
					this.flagIsBeingToggled = false;
				});
			}
			break;
		case "userInfo":
		{
			this.hand.lastContextInfoHit = null;
			bool flag = contextId == Managers.personManager.ourPerson.userId;
			if (flag && Our.mode == EditModes.Thing)
			{
				base.SwitchTo(DialogType.Create, string.Empty);
			}
			else
			{
				Our.personIdOfInterest = contextId;
				base.SwitchTo(DialogType.Profile, string.Empty);
			}
			break;
		}
		case "videoControl":
			base.SwitchTo(DialogType.VideoControl, string.Empty);
			break;
		case "web":
		{
			Browser browser = this.thing.GetComponentInChildren<Browser>();
			if (browser == null)
			{
				Component[] componentsInChildren = this.thing.gameObject.GetComponentsInChildren<ThingPart>();
				foreach (ThingPart thingPart in componentsInChildren)
				{
					if (thingPart.offersScreen)
					{
						browser = Managers.browserManager.TryAttachBrowser(thingPart, null, false);
					}
				}
			}
			if (browser != null)
			{
				this.SwitchToBrowserDialog(browser);
			}
			break;
		}
		case "slideshowControl":
		{
			GameObject gameObject = base.SwitchTo(DialogType.SlideshowControl, string.Empty);
			SlideshowControlDialog component = gameObject.GetComponent<SlideshowControlDialog>();
			component.thingPart = this.specialFeatureThingPart;
			break;
		}
		case "cameraControl":
			base.SwitchTo(DialogType.CameraControl, string.Empty);
			break;
		case "clonedFrom":
			this.thingObject = null;
			this.thingId = null;
			this.hand.lastContextInfoHit = null;
			Our.thingIdOfInterest = contextId;
			base.SwitchTo(DialogType.Thing, string.Empty);
			break;
		case "includedSubThingOf":
			this.thingObject = null;
			this.thingId = null;
			this.hand.lastContextInfoHit = null;
			Our.thingIdOfInterest = null;
			if (this.thing != null)
			{
				Thing myRootThing = this.thing.GetMyRootThing();
				GameObject gameObject2 = base.SwitchTo(DialogType.Thing, string.Empty);
				ThingDialog component2 = gameObject2.GetComponent<ThingDialog>();
				component2.thing = myRootThing;
				component2.thingObject = myRootThing.gameObject;
			}
			else
			{
				base.CloseDialog();
			}
			break;
		case "editClone":
			this.HandleEditClone();
			break;
		case "addToCreation":
		{
			this.hand.lastContextInfoHit = null;
			GameObject gameObject3 = base.SwitchTo(DialogType.IncludeThing, string.Empty);
			IncludeThingDialog component3 = gameObject3.GetComponent<IncludeThingDialog>();
			component3.thing = this.thing;
			break;
		}
		case "undo":
			this.thing.UndoPositionAndRotationAsAuthority();
			break;
		case "createDuplicates":
			this.CreateThingDuplicates(int.Parse(contextId), false);
			break;
		case "createDuplicatesNearby":
			this.CreateThingDuplicates(int.Parse(contextId), true);
			break;
		case "clearDuplicates":
			this.ClearThingDuplicates();
			break;
		case "toggleLock":
			if (!this.waitingForCallback && (!Managers.areaManager.onlyOwnerSetsLocks || Managers.areaManager.weAreOwnerOfCurrentArea))
			{
				global::UnityEngine.Object.Destroy(this.placementAttributeInfoLabel);
				global::UnityEngine.Object.Destroy(this.undoButton);
				this.waitingForCallback = true;
				this.thing.isLocked = !this.thing.isLocked;
				if (this.thing.isLocked)
				{
					Managers.areaManager.SetPlacementAttribute(this.thing.placementId, PlacementAttribute.Locked, delegate(bool ok)
					{
						this.waitingForCallback = false;
						if (ok)
						{
							this.HandlePlacementAttributeSuccess("Protected against moving, deleting & editing");
						}
						else
						{
							this.thing.isLocked = !this.thing.isLocked;
						}
						this.UpdateLockButton();
						this.UpdateInvisibleToEditorsButton();
						this.UpdateCloneOrAddButton();
						this.UpdateScaleSlider();
						this.ReopenIfWeAreResized();
					});
				}
				else
				{
					Managers.areaManager.ClearPlacementAttribute(this.thing.placementId, PlacementAttribute.Locked, delegate(bool ok)
					{
						this.waitingForCallback = false;
						if (ok)
						{
							Managers.soundManager.Play("pickUp", this.transform, 0.2f, false, false);
						}
						else
						{
							this.thing.isLocked = !this.thing.isLocked;
						}
						this.UpdateLockButton();
						this.UpdateInvisibleToEditorsButton();
						this.UpdateCloneOrAddButton();
						this.UpdateScaleSlider();
						this.ReopenIfWeAreResized();
					});
				}
			}
			break;
		case "toggleInvisibleToEditors":
			if (!this.waitingForCallback && !this.thing.isLocked)
			{
				global::UnityEngine.Object.Destroy(this.placementAttributeInfoLabel);
				global::UnityEngine.Object.Destroy(this.undoButton);
				this.waitingForCallback = true;
				this.thing.isInvisibleToEditors = !this.thing.isInvisibleToEditors;
				if (this.thing.isInvisibleToEditors)
				{
					Managers.areaManager.SetPlacementAttribute(this.thing.placementId, PlacementAttribute.InvisibleToEditors, delegate(bool ok)
					{
						this.waitingForCallback = false;
						if (ok)
						{
							if (!Our.seeInvisibleAsEditor)
							{
								Misc.SetAllObjectLayers(this.thing.gameObject, "InvisibleToOurPerson");
							}
							this.HandlePlacementAttributeSuccess("Invisible to editors (unless \"See Invisible\" is on)");
						}
						else
						{
							this.thing.isInvisibleToEditors = !this.thing.isInvisibleToEditors;
						}
						this.UpdateInvisibleToEditorsButton();
						this.ReopenIfWeAreResized();
					});
				}
				else
				{
					Managers.areaManager.ClearPlacementAttribute(this.thing.placementId, PlacementAttribute.InvisibleToEditors, delegate(bool ok)
					{
						this.waitingForCallback = false;
						if (ok)
						{
							Misc.SetAllObjectLayers(this.thing.gameObject, "Default");
							Managers.soundManager.Play("pickUp", this.transform, 0.2f, false, false);
						}
						else
						{
							this.thing.isInvisibleToEditors = !this.thing.isInvisibleToEditors;
						}
						this.UpdateInvisibleToEditorsButton();
						this.ReopenIfWeAreResized();
					});
				}
			}
			break;
		case "showPlacedSubThingMaster":
			this.thingObject = null;
			this.thingId = null;
			this.hand.lastContextInfoHit = null;
			Our.thingIdOfInterest = contextId;
			base.SwitchTo(DialogType.Thing, string.Empty);
			break;
		case "export":
		{
			Misc.DestroyMultiple(new GameObject[] { this.exportButton, this.flagButton, this.hearsAndTellsButton, this.slowBuildCreationButton });
			string text = Managers.thingManager.ExportThing(this.thing);
			string text2 = "✔  Exported to\n" + text + "\nAlso see: anyland.com/info/thing-format.html";
			base.AddLabel(text2, -445, 370, 0.55f, false, TextColor.Green, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
			if (!ThingDialog.didExportThisSession)
			{
				ThingDialog.didExportThisSession = true;
				Misc.OpenWindowsExplorerAtPath(text);
			}
			break;
		}
		case "hearsAndTells":
			this.ShowHearsAndTells();
			break;
		case "slowBuildCreation":
		{
			Managers.desktopManager.ShowDialogFront();
			GameObject gameObject4 = base.SwitchTo(DialogType.SlowBuildCreation, string.Empty);
			SlowBuildCreationDialog component4 = gameObject4.GetComponent<SlowBuildCreationDialog>();
			component4.thing = this.thing;
			break;
		}
		case "delete":
			if (this.thing != null)
			{
				Thing myRootThing2 = this.thing.GetMyRootThing();
				if (!myRootThing2.isLocked && (!myRootThing2.isInvisibleToEditors || Our.seeInvisibleAsEditor))
				{
					Managers.personManager.DoDeletePlacement(myRootThing2.gameObject, false);
					this.thingObject = null;
					this.thing = null;
					base.CloseDialog();
				}
				else
				{
					Managers.soundManager.Play("no", this.transform, 1f, false, false);
				}
			}
			break;
		case "areaCopyCreditsDialog":
			base.SwitchTo(DialogType.AreaCopyCredits, string.Empty);
			break;
		case "controllableControl":
		{
			Managers.personManager.ourPerson.StartControlControllable(this.thing);
			GameObject gameObject5 = base.SwitchTo(DialogType.Joystick, string.Empty);
			JoystickDialog component5 = gameObject5.GetComponent<JoystickDialog>();
			component5.controllable = Managers.personManager.ourPerson.controlledControllable.GetComponent<Thing>();
			break;
		}
		case "tags":
		{
			GameObject gameObject6 = base.SwitchTo(DialogType.ThingTags, string.Empty);
			ThingTagsDialog component6 = gameObject6.GetComponent<ThingTagsDialog>();
			component6.thing = this.thing;
			component6.weAreCreator = this.weAreCreator;
			component6.isUnlisted = this.isUnlisted;
			break;
		}
		case "copyPaste":
		{
			GameObject gameObject7 = base.SwitchTo(DialogType.ThingCopyPaste, string.Empty);
			ThingCopyPasteDialog component7 = gameObject7.GetComponent<ThingCopyPasteDialog>();
			component7.thing = this.thing;
			break;
		}
		case "suppress":
		{
			GameObject gameObject8 = base.SwitchTo(DialogType.PlacementSuppress, string.Empty);
			PlacementSuppressDialog component8 = gameObject8.GetComponent<PlacementSuppressDialog>();
			component8.thing = this.thing;
			break;
		}
		case "distanceToShow":
		{
			float? distanceToShow2 = this.thing.distanceToShow;
			string text3 = ((distanceToShow2 == null) ? string.Empty : this.thing.distanceToShow.ToString());
			string text4 = "distance in meters at which active (empty: default)";
			Managers.dialogManager.GetFloatInput(text3, text4, delegate(float? distanceToShow)
			{
				this.thing.distanceToShow = distanceToShow;
				float? distanceToShow3 = this.thing.distanceToShow;
				if (distanceToShow3 != null)
				{
					float? distanceToShow4 = this.thing.distanceToShow;
					if (distanceToShow4.Value <= 0f)
					{
						this.thing.distanceToShow = null;
					}
					else
					{
						Thing thing = this.thing;
						float? distanceToShow5 = this.thing.distanceToShow;
						thing.distanceToShow = new float?(Mathf.Clamp(distanceToShow5.Value, 1f, 100000f));
					}
				}
				Managers.areaManager.UpdateThingPlacement(this.thing.gameObject);
				Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
				this.CloseDialog();
				Managers.personManager.DoUpdatePlacementShowAt(this.thing);
			}, false, true);
			break;
		}
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x04000911 RID: 2321
	public Thing thing;

	// Token: 0x04000912 RID: 2322
	public GameObject thingObject;

	// Token: 0x04000913 RID: 2323
	private string thingId;

	// Token: 0x04000914 RID: 2324
	private GameObject flagButton;

	// Token: 0x04000915 RID: 2325
	private GameObject exportButton;

	// Token: 0x04000916 RID: 2326
	private GameObject cloneOrAddButton;

	// Token: 0x04000917 RID: 2327
	private GameObject lockButton;

	// Token: 0x04000918 RID: 2328
	private GameObject invisibleToEditorsButton;

	// Token: 0x04000919 RID: 2329
	private GameObject clonedFromButton;

	// Token: 0x0400091A RID: 2330
	private GameObject deleteButton;

	// Token: 0x0400091B RID: 2331
	private GameObject specialFeatureButton;

	// Token: 0x0400091C RID: 2332
	private GameObject hearsAndTellsButton;

	// Token: 0x0400091D RID: 2333
	private GameObject slowBuildCreationButton;

	// Token: 0x0400091E RID: 2334
	private string thingThatWasClonedFromIdIfRelevant = string.Empty;

	// Token: 0x0400091F RID: 2335
	private static bool didExportThisSession;

	// Token: 0x04000920 RID: 2336
	private bool weAreCreator;

	// Token: 0x04000921 RID: 2337
	private bool isUnlisted;

	// Token: 0x04000922 RID: 2338
	private TextMesh placementAttributeInfoLabel;

	// Token: 0x04000923 RID: 2339
	private ThingPart specialFeatureThingPart;

	// Token: 0x04000924 RID: 2340
	private GameObject debugPlacements;

	// Token: 0x04000925 RID: 2341
	private TextMesh debugClonesLabel;

	// Token: 0x04000926 RID: 2342
	private bool waitingForCallback;

	// Token: 0x04000927 RID: 2343
	private GameObject undoButton;

	// Token: 0x04000928 RID: 2344
	private TextMesh collectedLabel;

	// Token: 0x04000929 RID: 2345
	private TextMesh placedLabel;

	// Token: 0x0400092A RID: 2346
	private bool weAreResized;

	// Token: 0x0400092B RID: 2347
	private bool offerCloning;

	// Token: 0x0400092C RID: 2348
	private bool addedWebButton;

	// Token: 0x0400092D RID: 2349
	private DialogSlider scaleSlider;

	// Token: 0x0400092E RID: 2350
	private float? scaleAtStart;

	// Token: 0x0400092F RID: 2351
	private static Dictionary<float, float> sliderValueToScale;

	// Token: 0x04000930 RID: 2352
	private const float scaleSliderValueMax = 2f;

	// Token: 0x04000931 RID: 2353
	private Vector3? lastScaleSyncedToOthers;

	// Token: 0x04000932 RID: 2354
	private Vector3? targetScale;

	// Token: 0x04000933 RID: 2355
	public bool autoForwardIfContainsBrowser = true;

	// Token: 0x04000934 RID: 2356
	private bool allowBrowserOnThisThingType;

	// Token: 0x04000935 RID: 2357
	private float openingTime = -1f;

	// Token: 0x04000936 RID: 2358
	private bool cloneWhenWeKnowWeMay;
}
