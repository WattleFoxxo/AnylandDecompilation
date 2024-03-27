using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000135 RID: 309
public class CreateDialog : Dialog
{
	// Token: 0x06000B9D RID: 2973 RVA: 0x00061594 File Offset: 0x0005F994
	public void Start()
	{
		if (CrossDevice.desktopMode && (!Universe.features.createThings || !Our.createThingsInDesktopMode))
		{
			this.DoDiscard();
			base.ShowDesktopHelp();
			return;
		}
		if (!Managers.personManager.ourPerson.hasEditTools)
		{
			base.SwitchTo(DialogType.GetEditTools, string.Empty);
			return;
		}
		if (!Universe.features.createThings)
		{
			base.SwitchTo(DialogType.Start, string.Empty);
			return;
		}
		if (CreationHelper.thingBeingEdited == null)
		{
			CreationHelper.thingBeingEdited = global::UnityEngine.Object.Instantiate<GameObject>(Managers.thingManager.thingGameObject);
			CreationHelper.thingBeingEdited.transform.parent = Managers.thingManager.placements.transform;
			CreationHelper.thingBeingEdited.name = CreationHelper.thingDefaultName;
		}
		CreationHelper.thingBeingEdited.transform.localScale = Vector3.one;
		this.doHandleGuidance = !Managers.achievementManager.DidAchieve(Achievement.DraggedBaseShape) || !Managers.achievementManager.DidAchieve(Achievement.ResizedThingPart) || !Managers.achievementManager.DidAchieve(Achievement.DuplicatedThingPart) || !Managers.achievementManager.DidAchieve(Achievement.DeletedSomething) || !Managers.achievementManager.DidAchieve(Achievement.ClickedCreateDialogBacksideButton);
		this.thingScript = CreationHelper.thingBeingEdited.GetComponent<Thing>();
		if (this.thingScript.givenName == string.Empty)
		{
			this.thingScript.givenName = CreationHelper.thingDefaultName;
		}
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddMinimizeButton();
		base.AddModelButton("EditTextButton", "editCreationName", null, -400, -440, false);
		string text = Misc.Truncate(this.thingScript.givenName, 18, true);
		base.AddHeadline(text, -350, -450, TextColor.Default, TextAlignment.Left, false);
		base.AddButton("attributes", null, null, "ButtonSmall", -400, 120, "attributes", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		Vector3 localPosition = this.thingPartBaseWrapper.transform.localPosition;
		localPosition.x -= 25f * this.scaleFactor;
		localPosition.z += 270f * this.scaleFactor;
		this.thingPartBaseWrapper.transform.localPosition = localPosition;
		this.UpdateBaseShapes();
		base.AddButton("resetAllStates", null, null, "ButtonSmall", 400, 120, "resetAllStates", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.AddModelButton("ButtonBack", "previousCreationMode", null, -130, 120, false);
		base.AddModelButton("ButtonForward", "nextCreationMode", null, 130, 120, false);
		base.AddSeparator(0, 240, false);
		this.discardButton = base.AddButton("askIfReallyDiscard", null, string.Empty, "DeemphasizedButton", -235, 380, "cross", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		this.saveButton = base.AddButton("save", null, "done", "Button", 235, 380, "checkmark", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.AddBrush();
		base.AddGenericHelpButton();
		base.Invoke("AutoSetLastTransformHandledIfOnlySingleThingPart", 0.1f);
		base.AddBacksideEditButtons();
		Managers.settingManager.TriggerAllSettingChangesForAllAttachments();
		if (this.thingScript != null)
		{
			Managers.thingManager.UpdateShowThingPartDirectionArrows(this.thingScript, true);
		}
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x00061970 File Offset: 0x0005FD70
	private void UpdateBaseShapes()
	{
		if (CreationHelper.shapesTab == 10)
		{
			this.bigDialogLabel = base.AddLabel("board dialog", -120, -310, 0.75f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		else
		{
			global::UnityEngine.Object.Destroy(this.bigDialogLabel);
		}
		foreach (GameObject gameObject in this.baseParts)
		{
			global::UnityEngine.Object.Destroy(gameObject);
		}
		this.baseParts = new List<GameObject>();
		IEnumerator enumerator2 = Enum.GetValues(typeof(ThingPartBase)).GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj = enumerator2.Current;
				ThingPartBase thingPartBase = (ThingPartBase)obj;
				bool flag = false;
				int num = (int)thingPartBase;
				switch (CreationHelper.shapesTab)
				{
				case 0:
					flag = num >= 1 && num <= 17;
					break;
				case 1:
					flag = (num >= 26 && num <= 63) || num == 205;
					break;
				case 2:
					flag = (num >= 64 && num <= 89) || (num >= 144 && num <= 149);
					break;
				case 3:
					flag = num >= 150 && num <= 180;
					break;
				case 4:
					flag = num >= 181 && num <= 204;
					break;
				case 5:
					flag = num >= 247 && num <= 251;
					break;
				case 6:
					flag = (num >= 18 && num <= 23) || (num >= 90 && num <= 98);
					break;
				case 7:
					flag = num >= 99 && num <= 113;
					break;
				case 8:
					flag = num >= 114 && num <= 128;
					break;
				case 9:
					flag = num >= 129 && num <= 143;
					break;
				case 10:
					flag = thingPartBase == ThingPartBase.BigDialog;
					break;
				}
				if (flag)
				{
					GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(Managers.thingManager.thingPartBases[num], this.thingPartBaseWrapper.transform);
					this.baseParts.Add(gameObject2);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator2 as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x00061C7C File Offset: 0x0006007C
	private void AutoSetLastTransformHandledIfOnlySingleThingPart()
	{
		Our.AutoSetLastTransformHandledIfOnlySingleThingPart();
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x00061C84 File Offset: 0x00060084
	private void AskIfReallyDiscard()
	{
		global::UnityEngine.Object.Destroy(this.discardButton);
		base.AddButton("doDiscard", null, "discard?", "DeemphasizedButton", -235, 380, "cross", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x00061CE2 File Offset: 0x000600E2
	private new void Update()
	{
		this.HandleShowGuidance();
		base.ReactToOnClick();
		base.HandleBacksideEditButtons();
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x00061CF8 File Offset: 0x000600F8
	private void HandleShowGuidance()
	{
		if (!this.doHandleGuidance)
		{
			return;
		}
		if (!Managers.achievementManager.DidAchieve(Achievement.DraggedBaseShape))
		{
			if (this.guidanceLabelDrag == null)
			{
				this.guidanceLabelDrag = base.AddLabel("Click shapes with finger tip & drag into world", 0, -24, 0.8f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
			}
		}
		else if (!Managers.achievementManager.DidAchieve(Achievement.ResizedThingPart))
		{
			Misc.DestroyMultiple(new TextMesh[] { this.guidanceLabelDrag });
			if (this.guidanceLabelResize == null)
			{
				this.guidanceLabelResize = base.AddLabel("Resize by pressing both triggers in mid-air", 0, -24, 0.8f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
			}
		}
		else if (!Managers.achievementManager.DidAchieve(Achievement.DuplicatedThingPart))
		{
			Misc.DestroyMultiple(new TextMesh[] { this.guidanceLabelDrag, this.guidanceLabelResize });
			if (this.guidanceLabelDuplicated == null)
			{
				this.guidanceLabelDuplicated = base.AddLabel("Hold & drag out with other hand to duplicate", 0, -24, 0.8f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
			}
		}
		else if (!Managers.achievementManager.DidAchieve(Achievement.DeletedSomething))
		{
			Misc.DestroyMultiple(new TextMesh[] { this.guidanceLabelDrag, this.guidanceLabelResize, this.guidanceLabelDuplicated });
			if (this.guidanceLabelDelete == null)
			{
				string text = string.Empty;
				global::DeviceType type = CrossDevice.type;
				if (type != global::DeviceType.OculusTouch)
				{
					if (type != global::DeviceType.Index)
					{
						text = "grip";
					}
					else
					{
						text = "a";
					}
				}
				else
				{
					text = "x or a";
				}
				this.guidanceLabelDelete = base.AddLabel("Press " + text + " to delete something", 0, -24, 0.8f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
			}
		}
		else if (!Managers.achievementManager.DidAchieve(Achievement.ClickedCreateDialogBacksideButton))
		{
			Misc.DestroyMultiple(new TextMesh[] { this.guidanceLabelDrag, this.guidanceLabelResize, this.guidanceLabelDuplicated, this.guidanceLabelDelete });
			if (this.guidanceLabelClickedBacksideButton == null)
			{
				this.guidanceLabelClickedBacksideButton = base.AddLabel("Turn dialog around for more options", 0, -24, 0.8f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
			}
		}
		else
		{
			Misc.DestroyMultiple(new TextMesh[] { this.guidanceLabelDrag, this.guidanceLabelResize, this.guidanceLabelDuplicated, this.guidanceLabelDelete, this.guidanceLabelClickedBacksideButton });
			this.doHandleGuidance = false;
		}
	}

	// Token: 0x06000BA3 RID: 2979 RVA: 0x00061F9C File Offset: 0x0006039C
	private void RestoreThingThatWasClonedFromIfNeeded()
	{
		if (CreationHelper.thingThatWasClonedFrom != null)
		{
			CreationHelper.thingThatWasClonedFrom.SetActive(true);
			Thing component = CreationHelper.thingThatWasClonedFrom.GetComponent<Thing>();
			if (component != null)
			{
				component.HandleAssignMyPlacedSubThingsAndMeAsPlacedSubThing();
			}
			CreationHelper.thingThatWasClonedFrom = null;
			CreationHelper.thingThatWasClonedFromIdIfRelevant = null;
		}
	}

	// Token: 0x06000BA4 RID: 2980 RVA: 0x00061FF0 File Offset: 0x000603F0
	private void DoDiscard()
	{
		CreateDialog.wasLastMinimized = false;
		global::UnityEngine.Object.Destroy(this.discardButton);
		global::UnityEngine.Object.Destroy(this.saveButton);
		global::UnityEngine.Object.Destroy(CreationHelper.referenceObject);
		global::UnityEngine.Object.Destroy(CreationHelper.thingBeingEdited);
		CreationHelper.thingPartWhoseStatesAreEdited = null;
		CreationHelper.thingBeingEdited = null;
		base.CloseDialog();
		Our.SetPreviousMode();
		this.RestoreThingThatWasClonedFromIfNeeded();
		CreationHelper.referenceImage = null;
		CreationHelper.materialType = MaterialTypes.None;
		CreationHelper.particleSystemType = ParticleSystemType.None;
		CreationHelper.shapesTab = 0;
		CreationHelper.thingThatWasClonedFrom = null;
		CreationHelper.thingThatWasClonedFromIdIfRelevant = null;
		CreationHelper.replaceInstancesInAreaOneTime = false;
		Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
		Managers.skyManager.AutoFindThingPartMatchIfNeeded();
		Managers.settingManager.TriggerAllSettingChangesForAllAttachments();
	}

	// Token: 0x06000BA5 RID: 2981 RVA: 0x00062098 File Offset: 0x00060498
	private void DoSave(string json, int vertexCount)
	{
		CreateDialog.wasLastMinimized = false;
		global::UnityEngine.Object.Destroy(this.discardButton);
		global::UnityEngine.Object.Destroy(this.saveButton);
		global::UnityEngine.Object.Destroy(CreationHelper.referenceObject);
		base.StartCoroutine(Managers.serverManager.SaveThing(json, vertexCount, CreationHelper.thingThatWasClonedFromIdIfRelevant, delegate(SaveThing_Response response)
		{
			if (response.error == null)
			{
				Managers.thingManager.StoreThingJsonInAllCaches(response.thingId, json);
				Managers.achievementManager.RegisterAchievement(Achievement.SavedThing);
				string text = string.Empty;
				string text2 = string.Empty;
				if (CreationHelper.thingThatWasClonedFrom != null)
				{
					Thing component = CreationHelper.thingThatWasClonedFrom.GetComponent<Thing>();
					text = component.placementId;
					text2 = component.thingId;
					Misc.Destroy(CreationHelper.thingThatWasClonedFrom);
				}
				this.thingScript.thingId = response.thingId;
				this.thingScript.UpdateContainsBehaviorScriptValue();
				string text3 = null;
				if (Managers.areaManager.weAreEditorOfCurrentArea)
				{
					text3 = Managers.personManager.DoPlaceJustCreatedThing(CreationHelper.thingBeingEdited, text, text2);
				}
				else
				{
					Managers.personManager.DoAddJustCreatedTemporaryThing(CreationHelper.thingBeingEdited);
				}
				CreationHelper.referenceImage = null;
				CreationHelper.thingThatWasClonedFrom = null;
				CreationHelper.thingBeingEdited = null;
				CreationHelper.thingThatWasClonedFromIdIfRelevant = null;
				if (CrossDevice.desktopMode)
				{
					Our.SetMode(EditModes.None, false);
				}
				else if (Managers.areaManager.weAreEditorOfCurrentArea && !this.thingScript.isHoldable && !this.thingScript.movableByEveryone && !this.GetIsGrabbable(this.thingScript))
				{
					Our.SetMode(EditModes.Area, false);
				}
				else
				{
					Our.SetPreviousMode();
				}
				Managers.settingManager.TriggerAllSettingChangesForAllAttachments();
				this.CloseDialog();
				Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(text3);
				Managers.skyManager.AutoFindThingPartMatchIfNeeded();
				Managers.thingManager.UpdateAllVisibilityAndCollision(false);
			}
			else
			{
				this.HandleDoSaveError(response.error);
			}
		}));
	}

	// Token: 0x06000BA6 RID: 2982 RVA: 0x00062108 File Offset: 0x00060508
	private void HandleDoSaveError(string error)
	{
		if (error == null)
		{
			error = string.Empty;
		}
		Log.Error(error);
		CreationHelper.thingBeingEdited = null;
		this.RestoreThingThatWasClonedFromIfNeeded();
		CreationHelper.thingThatWasClonedFrom = null;
		CreationHelper.thingThatWasClonedFromIdIfRelevant = null;
		string text = string.Empty;
		string text2 = error.ToLower();
		if (text2 != null)
		{
			if (text2 == "cannot resolve destination host")
			{
				text = "Oops! Something went wrong with your local connection while trying to save this item.";
				goto IL_B2;
			}
			if (text2 == "cannot connect to destination host")
			{
				text = "Oops! Something went wrong with the server connection while trying to save this item. Please let us know if you're seeing this.";
				goto IL_B2;
			}
			if (text2 == "400 bad request")
			{
				text = "Oops! Something went wrong saving this specific item. Please let us know if you're seeing this.";
				goto IL_B2;
			}
		}
		text = "Oops! Something went wrong during saving (\"" + Misc.Truncate(error, 100, true) + "\"). Please let us know if you're seeing this.";
		IL_B2:
		Managers.dialogManager.ShowError(text, false, true, -1);
	}

	// Token: 0x06000BA7 RID: 2983 RVA: 0x000621D8 File Offset: 0x000605D8
	private bool GetIsGrabbable(Thing thingScript)
	{
		bool flag = false;
		ThingPart[] componentsInChildren = thingScript.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.isGrabbable)
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06000BA8 RID: 2984 RVA: 0x00062220 File Offset: 0x00060620
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "help":
			Managers.browserManager.OpenGuideBrowser(null, null);
			break;
		case "editCreationName":
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (!string.IsNullOrEmpty(text))
				{
					Thing component = CreationHelper.thingBeingEdited.GetComponent<Thing>();
					component.givenName = text;
					CreationHelper.thingBeingEdited.name = component.givenName;
				}
				base.SwitchTo(DialogType.Create, string.Empty);
			}, contextName, (!(this.thingScript.givenName == CreationHelper.thingDefaultName)) ? this.thingScript.givenName : string.Empty, 37, string.Empty, false, false, false, false, 1f, false, false, null, false);
			break;
		case "askIfReallyDiscard":
			this.AskIfReallyDiscard();
			break;
		case "doDiscard":
			this.DoDiscard();
			break;
		case "save":
		{
			int num2 = 0;
			int num3 = 0;
			CreationHelper.thingPartWhoseStatesAreEdited = null;
			CreationHelper.materialType = MaterialTypes.None;
			CreationHelper.particleSystemType = ParticleSystemType.None;
			CreationHelper.shapesTab = 0;
			Managers.thingManager.UpdatePlacedSubThingsInfo(this.thingScript);
			string json = ThingToJsonConverter.GetJson(CreationHelper.thingBeingEdited, ref num2, ref num3);
			int num4 = json.Length / 1024;
			if (num4 > 1000)
			{
				Managers.dialogManager.ShowInfo(string.Concat(new object[] { "Sorry, this creation is too complex (", num4, " kb of available ", 1000, " kb). An emergency backup was saved in the backup folder that opened on desktop. Please contact us so we can help investigate." }), false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
				this.CreateJsonBackup(json);
			}
			else if (num3 > 2500)
			{
				Managers.dialogManager.ShowInfo(string.Concat(new object[] { "Sorry, there are too many changed surface points (", num3, " of available ", 2500, "). An emergency backup was saved in the backup folder that opened on desktop. Please contact us so we can help investigate." }), false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
				this.CreateJsonBackup(json);
			}
			else if (num2 == 0)
			{
				this.DoDiscard();
			}
			else
			{
				int thingPartCount = Managers.thingManager.GetThingPartCount(CreationHelper.thingBeingEdited);
				if (thingPartCount <= 1000 || CreationHelper.alreadyExceededMaxThingPartCountOnCloning)
				{
					if (json.Length <= 300000)
					{
						int otherInstancesThisWillReplaceCount = this.GetOtherInstancesThisWillReplaceCount();
						if (otherInstancesThisWillReplaceCount > 0 && !this.didAskIfWantsToReplaceInstances)
						{
							base.SetText(this.saveButton, "Change all " + (otherInstancesThisWillReplaceCount + 1) + "?", 0.7f, null, false);
							this.didAskIfWantsToReplaceInstances = true;
						}
						else if (this.EverythingIsSameAsCloneSource(json))
						{
							this.DoDiscard();
						}
						else
						{
							this.DoSave(json, num2);
						}
					}
					else
					{
						Managers.dialogManager.ShowInfo("Sorry, this creation exceeds the size limit for saving!", false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
					}
				}
				else
				{
					Managers.dialogManager.ShowInfo(string.Concat(new object[] { "Sorry, this creation has too many parts to save! There are ", thingPartCount, " parts, and the limit is ", 1000, "." }), false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
				}
			}
			break;
		}
		case "resetAllStates":
			CreationHelper.thingPartWhoseStatesAreEdited = null;
			this.thingScript.ResetStates();
			if (this.resetAllStatesTextMesh != null)
			{
				global::UnityEngine.Object.Destroy(this.resetAllStatesTextMesh.gameObject);
			}
			else if (!this.MultipleStatesFoundInAThingPart())
			{
				float num5 = 0.6f;
				this.resetAllStatesTextMesh = base.AddLabel("This sets all states to 1 to check context laser scripts", -405, -3, num5, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			}
			break;
		case "nextCreationMode":
			CreationHelper.shapesTab++;
			if (CreationHelper.shapesTab == 10 && !CreationHelper.showDialogShapesTab)
			{
				CreationHelper.shapesTab++;
			}
			if (CreationHelper.shapesTab > 10)
			{
				CreationHelper.shapesTab = 0;
			}
			if (this.resetAllStatesTextMesh != null)
			{
				global::UnityEngine.Object.Destroy(this.resetAllStatesTextMesh.gameObject);
			}
			this.UpdateBaseShapes();
			Managers.achievementManager.RegisterAchievement(Achievement.PagedBaseShapes);
			break;
		case "previousCreationMode":
			CreationHelper.shapesTab--;
			if (CreationHelper.shapesTab < 0)
			{
				CreationHelper.shapesTab = ((!CreationHelper.showDialogShapesTab) ? 9 : 10);
			}
			if (this.resetAllStatesTextMesh != null)
			{
				global::UnityEngine.Object.Destroy(this.resetAllStatesTextMesh.gameObject);
			}
			this.UpdateBaseShapes();
			Managers.achievementManager.RegisterAchievement(Achievement.PagedBaseShapes);
			break;
		case "attributes":
			base.SwitchTo(DialogType.ThingAttributes, string.Empty);
			break;
		case "minimize":
			this.Minimize();
			break;
		}
	}

	// Token: 0x06000BA9 RID: 2985 RVA: 0x00062764 File Offset: 0x00060B64
	private void CreateJsonBackup(string json)
	{
		string text = Application.persistentDataPath + "/backup";
		Directory.CreateDirectory(text);
		string text2 = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss");
		string text3 = text + "/" + text2 + ".json";
		File.WriteAllText(text3, json);
		Misc.OpenWindowsExplorerAtPath(text);
	}

	// Token: 0x06000BAA RID: 2986 RVA: 0x000627BC File Offset: 0x00060BBC
	private bool EverythingIsSameAsCloneSource(string newJson)
	{
		bool flag = false;
		if (CreationHelper.thingThatWasClonedFrom != null)
		{
			string thingJsonFromCache = Managers.thingManager.GetThingJsonFromCache(CreationHelper.thingThatWasClonedFrom.GetComponent<Thing>());
			flag = thingJsonFromCache != null && newJson == thingJsonFromCache;
		}
		return flag;
	}

	// Token: 0x06000BAB RID: 2987 RVA: 0x00062802 File Offset: 0x00060C02
	public void Minimize()
	{
		CreateDialog.wasLastMinimized = true;
		base.CloseDialog();
	}

	// Token: 0x06000BAC RID: 2988 RVA: 0x00062810 File Offset: 0x00060C10
	private bool MultipleStatesFoundInAThingPart()
	{
		bool flag = false;
		Component[] componentsInChildren = CreationHelper.thingBeingEdited.GetComponentsInChildren(typeof(ThingPart));
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.states.Count >= 2)
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06000BAD RID: 2989 RVA: 0x00062874 File Offset: 0x00060C74
	private int GetOtherInstancesThisWillReplaceCount()
	{
		int num = 0;
		if ((this.thingScript.replaceInstancesInArea || CreationHelper.replaceInstancesInAreaOneTime) && CreationHelper.thingThatWasClonedFrom != null)
		{
			Thing component = CreationHelper.thingThatWasClonedFrom.GetComponent<Thing>();
			Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Thing), true);
			foreach (Thing thing in componentsInChildren)
			{
				if (thing.thingId == component.thingId && thing != component && thing != this.thingScript && thing.gameObject.name != Universe.objectNameIfAlreadyDestroyed)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x06000BAE RID: 2990 RVA: 0x0006294C File Offset: 0x00060D4C
	public override void RecreateInterfaceAfterSettingsChangeIfNeeded()
	{
		global::UnityEngine.Object.Destroy(this.backsideWrapper);
		base.AddBacksideEditButtons();
	}

	// Token: 0x040008CB RID: 2251
	public GameObject thingPartBaseWrapper;

	// Token: 0x040008CC RID: 2252
	private Thing thingScript;

	// Token: 0x040008CD RID: 2253
	private GameObject discardButton;

	// Token: 0x040008CE RID: 2254
	private GameObject saveButton;

	// Token: 0x040008CF RID: 2255
	private GameObject toggleShapesButton;

	// Token: 0x040008D0 RID: 2256
	private Transform baseShapes;

	// Token: 0x040008D1 RID: 2257
	private TextMesh resetAllStatesTextMesh;

	// Token: 0x040008D2 RID: 2258
	public static bool wasLastMinimized;

	// Token: 0x040008D3 RID: 2259
	public const int tabIndex_shapes = 0;

	// Token: 0x040008D4 RID: 2260
	public const int tabIndex_shapes2 = 1;

	// Token: 0x040008D5 RID: 2261
	public const int tabIndex_shapes3 = 2;

	// Token: 0x040008D6 RID: 2262
	public const int tabIndex_shapes4 = 3;

	// Token: 0x040008D7 RID: 2263
	public const int tabIndex_shapes5 = 4;

	// Token: 0x040008D8 RID: 2264
	public const int tabIndex_shapes6 = 5;

	// Token: 0x040008D9 RID: 2265
	public const int tabIndex_text = 6;

	// Token: 0x040008DA RID: 2266
	public const int tabIndex_text2 = 7;

	// Token: 0x040008DB RID: 2267
	public const int tabIndex_text3 = 8;

	// Token: 0x040008DC RID: 2268
	public const int tabIndex_text4 = 9;

	// Token: 0x040008DD RID: 2269
	public const int tabIndex_dialogShapes = 10;

	// Token: 0x040008DE RID: 2270
	private List<GameObject> baseParts = new List<GameObject>();

	// Token: 0x040008DF RID: 2271
	private TextMesh bigDialogLabel;

	// Token: 0x040008E0 RID: 2272
	private TextMesh guidanceLabelDrag;

	// Token: 0x040008E1 RID: 2273
	private TextMesh guidanceLabelResize;

	// Token: 0x040008E2 RID: 2274
	private TextMesh guidanceLabelDuplicated;

	// Token: 0x040008E3 RID: 2275
	private TextMesh guidanceLabelDelete;

	// Token: 0x040008E4 RID: 2276
	private TextMesh guidanceLabelClickedBacksideButton;

	// Token: 0x040008E5 RID: 2277
	private bool doHandleGuidance;

	// Token: 0x040008E6 RID: 2278
	private bool didAskIfWantsToReplaceInstances;

	// Token: 0x040008E7 RID: 2279
	private const int jsonSizeAllowedKb = 1000;

	// Token: 0x040008E8 RID: 2280
	private const int changedVerticesIndicesCountMax = 2500;
}
