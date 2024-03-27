using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001E9 RID: 489
public class DialogManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001DD RID: 477
	// (get) Token: 0x060010A9 RID: 4265 RVA: 0x000905D2 File Offset: 0x0008E9D2
	// (set) Token: 0x060010AA RID: 4266 RVA: 0x000905DA File Offset: 0x0008E9DA
	public ManagerStatus status { get; private set; }

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x060010AB RID: 4267 RVA: 0x000905E3 File Offset: 0x0008E9E3
	// (set) Token: 0x060010AC RID: 4268 RVA: 0x000905EB File Offset: 0x0008E9EB
	public string failMessage { get; private set; }

	// Token: 0x060010AD RID: 4269 RVA: 0x000905F4 File Offset: 0x0008E9F4
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x060010AE RID: 4270 RVA: 0x00090604 File Offset: 0x0008EA04
	public GameObject SwitchToNewDialog(DialogType dialogType, Hand hand = null, string tabName = "")
	{
		if (hand == null)
		{
			hand = this.GetDialogHand();
		}
		string text = dialogType.ToString();
		hand.dialogBacksideOpenFactor = 0f;
		DialogType[] array = new DialogType[]
		{
			DialogType.MySize,
			DialogType.Start,
			DialogType.Thing,
			DialogType.Profile,
			DialogType.Alert
		};
		if (Managers.personManager.WeAreResized() && Array.IndexOf<DialogType>(array, dialogType) == -1)
		{
			Managers.personManager.ResetAndCachePhotonRigScale();
		}
		this.TriggerTellBodyEvents(hand, dialogType);
		if (hand.currentDialog != null)
		{
			hand.dialogTypeToBeOpened = dialogType;
			global::UnityEngine.Object.Destroy(hand.currentDialog);
		}
		if (Managers.personManager.ourPerson.isSoftBanned)
		{
			if (dialogType == DialogType.Profile || dialogType == DialogType.Areas || dialogType == DialogType.FindAreas)
			{
				Managers.soundManager.Play("no", base.transform, 1f, false, false);
				dialogType = DialogType.Softban;
			}
		}
		hand.currentDialog = this.GetDialogObject(dialogType);
		hand.currentDialogType = dialogType;
		hand.currentDialog.gameObject.name = text;
		hand.currentDialog.transform.parent = ((dialogType != DialogType.Start) ? hand.transform : hand.skeleton.GetRingBone());
		hand.currentDialog.SetActive(true);
		Dialog component = hand.currentDialog.GetComponent<Dialog>();
		component.dialogType = dialogType;
		component.tabName = tabName;
		hand.TriggerHapticPulse(Universe.miniBurstPulse);
		return hand.currentDialog;
	}

	// Token: 0x060010AF RID: 4271 RVA: 0x00090784 File Offset: 0x0008EB84
	private void TriggerTellBodyEvents(Hand hand, DialogType dialogType)
	{
		if (dialogType != hand.currentDialogType && hand.currentDialog != null)
		{
			string text = hand.side.ToString().ToLower();
			if (hand.currentDialogType != DialogType.Start)
			{
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "dialog " + hand.currentDialogType.ToString().ToLower() + " closed " + text, true);
			}
			if (dialogType != DialogType.Start)
			{
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "dialog " + dialogType.ToString().ToLower() + " opened " + text, true);
			}
			if (hand.currentDialogType == DialogType.Start && dialogType != DialogType.Start)
			{
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "dialog opened " + text, true);
			}
			else if (dialogType == DialogType.Start)
			{
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "dialog closed " + text, true);
			}
		}
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x000908B4 File Offset: 0x0008ECB4
	public void GetInput(Action<string> callback, string contextName = "", string existingText = "", int maxLength = 60, string placeholderHint = "", bool useExtendedKeys = false, bool allowSpecialKeysToggling = false, bool allowNewlines = false, bool allowMixedCase = false, float placeholderFontSizeAdjust = 1f, bool allowSpecialKeysForPersonName = false, bool restrictToNumbers = false, Hand hand = null, bool wasOpenedViaBackside = false)
	{
		if (hand == null)
		{
			hand = this.GetDialogHand();
		}
		TransformClipboard transformClipboard = null;
		if (hand.currentDialog != null && hand.currentDialogType == DialogType.Keyboard)
		{
			transformClipboard = new TransformClipboard();
			transformClipboard.SetFromTransform(hand.currentDialog.transform);
		}
		hand.SwitchToNewDialog(DialogType.Keyboard, string.Empty);
		KeyboardDialog component = hand.currentDialog.GetComponent<KeyboardDialog>();
		component.textContextName = contextName;
		component.stringCallback = callback;
		component.textEntered = ((existingText != null) ? existingText : string.Empty);
		component.maxLength = maxLength;
		component.placeholderHint = ((placeholderHint != null) ? placeholderHint : string.Empty);
		component.useExtendedKeys = useExtendedKeys;
		component.allowSpecialKeysToggling = allowSpecialKeysToggling;
		component.allowNewlines = allowNewlines;
		component.allowMixedCase = allowMixedCase;
		component.placeholderFontSizeAdjust = placeholderFontSizeAdjust;
		component.allowSpecialKeysForPersonName = allowSpecialKeysForPersonName;
		component.restrictToNumbers = restrictToNumbers;
		component.wasOpenedViaBackside = wasOpenedViaBackside;
		component.transformToUseAtStart = transformClipboard;
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x000909BC File Offset: 0x0008EDBC
	public void GetFloatInput(string existingText, string placeholderHint, Action<float?> callback, bool allowNegative = false, bool wasOpenedViaBackside = false)
	{
		Hand dialogHand = this.GetDialogHand();
		dialogHand.SwitchToNewDialog(DialogType.Keyboard, string.Empty);
		KeyboardDialog component = dialogHand.currentDialog.GetComponent<KeyboardDialog>();
		component.textEntered = existingText;
		component.placeholderHint = placeholderHint;
		component.floatCallback = callback;
		component.allowNegativeNumbers = allowNegative;
		component.restrictToNumbers = true;
		component.wasOpenedViaBackside = wasOpenedViaBackside;
	}

	// Token: 0x060010B2 RID: 4274 RVA: 0x00090A18 File Offset: 0x0008EE18
	public AlertDialog ShowInfo(string text, bool showOkButton = false, bool autoAddNewlines = true, int maxBuzzes = -1, DialogType dialogToReturnTo = DialogType.Start, float textSizeFactor = 1f, bool channelToTwitchChatOnlyIfPossible = false, TextColor textColor = TextColor.Default, TextAlignment align = TextAlignment.Left)
	{
		AlertDialog alertDialog = null;
		if (!channelToTwitchChatOnlyIfPossible || !this.TryChannelInfoToTwitchChat(text))
		{
			Hand dialogHand = this.GetDialogHand();
			Our.dialogToGoBackTo = DialogType.None;
			dialogHand.SwitchToNewDialog(DialogType.Alert, string.Empty);
			alertDialog = dialogHand.currentDialog.GetComponent<AlertDialog>();
			alertDialog.SetInfo(text);
			alertDialog.showOkButton = showOkButton;
			alertDialog.autoAddNewlines = autoAddNewlines;
			alertDialog.maxBuzzes = maxBuzzes;
			alertDialog.dialogToReturnTo = dialogToReturnTo;
			alertDialog.textSizeFactor = textSizeFactor;
			alertDialog.textColor = textColor;
			alertDialog.align = align;
		}
		return alertDialog;
	}

	// Token: 0x060010B3 RID: 4275 RVA: 0x00090AA0 File Offset: 0x0008EEA0
	public void ShowError(string message, bool showDefaultIntroText = true, bool allowClosingInsteadOfRestarting = false, int maxBuzzes = -1)
	{
		GameObject gameObject = this.SwitchToNewDialog(DialogType.Alert, null, string.Empty);
		AlertDialog component = gameObject.GetComponent<AlertDialog>();
		component.SetError(message, showDefaultIntroText, allowClosingInsteadOfRestarting);
		component.maxBuzzes = maxBuzzes;
	}

	// Token: 0x060010B4 RID: 4276 RVA: 0x00090AD4 File Offset: 0x0008EED4
	public bool CurrentNonStartDialogTypeIsOneOf(DialogType[] dialogTypes)
	{
		DialogType currentNonStartDialogType = this.GetCurrentNonStartDialogType();
		return Array.IndexOf<DialogType>(dialogTypes, currentNonStartDialogType) >= 0;
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x00090AF8 File Offset: 0x0008EEF8
	public DialogType GetCurrentNonStartDialogType()
	{
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		return (!(currentNonStartDialog != null)) ? DialogType.None : currentNonStartDialog.GetComponent<Dialog>().dialogType;
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x00090B28 File Offset: 0x0008EF28
	public void ShowPing(global::Ping ping)
	{
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		DialogType dialogType = ((!(currentNonStartDialog != null)) ? DialogType.None : currentNonStartDialog.GetComponent<Dialog>().dialogType);
		bool flag = dialogType != DialogType.Keyboard && dialogType != DialogType.ApproveBody && dialogType != DialogType.Joystick;
		if (flag)
		{
			Hand dialogHand = this.GetDialogHand();
			Our.dialogToGoBackTo = DialogType.None;
			Universe.hearEchoOfMyVoice = false;
			dialogHand.SwitchToNewDialog(DialogType.Alert, string.Empty);
			AlertDialog component = dialogHand.currentDialog.GetComponent<AlertDialog>();
			component.SetPing(ping);
		}
		else
		{
			Managers.soundManager.Play("ping", currentNonStartDialog.transform, 1f, false, false);
		}
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x00090BD0 File Offset: 0x0008EFD0
	public ThingDialog GetThingDialogIfAvailable()
	{
		ThingDialog thingDialog = null;
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		if (currentNonStartDialog != null)
		{
			thingDialog = currentNonStartDialog.GetComponent<ThingDialog>();
		}
		return thingDialog;
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x00090BFC File Offset: 0x0008EFFC
	private bool TryChannelInfoToTwitchChat(string text)
	{
		bool flag = false;
		IEnumerator enumerator = Enum.GetValues(typeof(Side)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				string text2 = "/OurPersonRig/HandCore" + ((Side)obj).ToString() + "/" + DialogType.TwitchChat.ToString();
				GameObject @object = Managers.treeManager.GetObject(text2);
				if (@object != null && @object.activeSelf)
				{
					TwitchChatDialog component = @object.GetComponent<TwitchChatDialog>();
					if (component != null)
					{
						component.OutputInfo(text);
						flag = true;
						break;
					}
				}
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
		return flag;
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x00090CE4 File Offset: 0x0008F0E4
	private Hand GetDialogHand()
	{
		Hand hand = null;
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		if (currentNonStartDialog != null && currentNonStartDialog.transform.parent != null)
		{
			hand = currentNonStartDialog.transform.parent.GetComponent<Hand>();
			if (hand == null)
			{
				try
				{
					hand = currentNonStartDialog.transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<Hand>();
				}
				catch
				{
				}
			}
		}
		if (hand == null)
		{
			hand = Managers.personManager.ourPerson.GetAHand();
		}
		return hand;
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x00090DA0 File Offset: 0x0008F1A0
	public Dialog GetCurrentNonStartDialogAsDialog()
	{
		Dialog dialog = null;
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		if (currentNonStartDialog != null)
		{
			dialog = currentNonStartDialog.GetComponent<Dialog>();
		}
		return dialog;
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x00090DC9 File Offset: 0x0008F1C9
	public void ApplyEmissionColor(Transform thisTransform, bool isActive = true)
	{
		if (thisTransform != null)
		{
			this.ApplyEmissionColor(thisTransform.gameObject, isActive);
		}
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x00090DE4 File Offset: 0x0008F1E4
	public void ApplyEmissionColor(GameObject thisObject, bool isActive = true)
	{
		Renderer component = thisObject.GetComponent<Renderer>();
		component.material.SetColor("_EmissionColor", (!isActive) ? Color.black : new Color(0.75f, 0.75f, 0.75f));
	}

	// Token: 0x060010BD RID: 4285 RVA: 0x00090E2C File Offset: 0x0008F22C
	public bool KeyboardIsOpen()
	{
		Hand dialogHand = this.GetDialogHand();
		return dialogHand != null && dialogHand.currentDialogType == DialogType.Keyboard;
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x00090E5C File Offset: 0x0008F25C
	public void CloseDialog()
	{
		Hand dialogHand = this.GetDialogHand();
		dialogHand.SwitchToNewDialog(DialogType.Start, string.Empty);
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x00090E80 File Offset: 0x0008F280
	public GameObject GetDialogObject(DialogType dialogType)
	{
		bool flag = dialogType == DialogType.Create || dialogType == DialogType.Keyboard || dialogType == DialogType.Material;
		GameObject gameObject;
		if (flag)
		{
			string text = "Dialogs/" + dialogType.ToString();
			gameObject = global::UnityEngine.Object.Instantiate<GameObject>((GameObject)Resources.Load(text, typeof(GameObject)));
		}
		else
		{
			gameObject = new GameObject(dialogType.ToString());
			switch (dialogType)
			{
			case DialogType.Alert:
				gameObject.AddComponent<AlertDialog>();
				break;
			case DialogType.PageableText:
				gameObject.AddComponent<PageableTextDialog>();
				break;
			case DialogType.ApproveBody:
				gameObject.AddComponent<ApproveBodyDialog>();
				break;
			case DialogType.AreaAttributes:
				gameObject.AddComponent<AreaAttributesDialog>();
				break;
			case DialogType.AreaCopyCredits:
				gameObject.AddComponent<AreaCopyCreditsDialog>();
				break;
			case DialogType.Area:
				gameObject.AddComponent<AreaDialog>();
				break;
			case DialogType.AreaFilters:
				gameObject.AddComponent<AreaFiltersDialog>();
				break;
			case DialogType.AreaPlacements:
				gameObject.AddComponent<AreaPlacementsDialog>();
				break;
			case DialogType.AreaRights:
				gameObject.AddComponent<AreaRightsDialog>();
				break;
			case DialogType.Areas:
				gameObject.AddComponent<AreasDialog>();
				break;
			case DialogType.BodyMotions:
				gameObject.AddComponent<BodyMotionsDialog>();
				break;
			case DialogType.CameraControl:
				gameObject.AddComponent<CameraControlDialog>();
				break;
			case DialogType.Confirm:
				gameObject.AddComponent<ConfirmDialog>();
				break;
			case DialogType.CurrentAndRecentPeople:
				gameObject.AddComponent<CurrentAndRecentPeopleDialog>();
				break;
			case DialogType.DesktopCamera:
				gameObject.AddComponent<DesktopCameraDialog>();
				break;
			case DialogType.Environment:
				gameObject.AddComponent<EnvironmentDialog>();
				break;
			case DialogType.FindAreas:
				gameObject.AddComponent<FindAreasDialog>();
				break;
			case DialogType.Friends:
				gameObject.AddComponent<FriendsDialog>();
				break;
			case DialogType.GetEditTools:
				gameObject.AddComponent<GetEditToolsDialog>();
				break;
			case DialogType.Gifts:
				gameObject.AddComponent<GiftsDialog>();
				break;
			case DialogType.Inventory:
				gameObject.AddComponent<InventoryDialog>();
				break;
			case DialogType.Joystick:
				gameObject.AddComponent<JoystickDialog>();
				break;
			case DialogType.Main:
				gameObject.AddComponent<MainDialog>();
				break;
			case DialogType.Controllable:
				gameObject.AddComponent<ControllableDialog>();
				break;
			case DialogType.MySize:
				gameObject.AddComponent<MySizeDialog>();
				break;
			case DialogType.Microphone:
				gameObject.AddComponent<MicrophoneDialog>();
				break;
			case DialogType.OwnProfile:
				gameObject.AddComponent<OwnProfileDialog>();
				break;
			case DialogType.Profile:
				gameObject.AddComponent<ProfileDialog>();
				break;
			case DialogType.Question:
				gameObject.AddComponent<QuestionDialog>();
				break;
			case DialogType.Settings:
				gameObject.AddComponent<SettingsDialog>();
				break;
			case DialogType.SettingsMore:
				gameObject.AddComponent<SettingsMoreDialog>();
				break;
			case DialogType.SlideshowControl:
				gameObject.AddComponent<SlideshowControlDialog>();
				break;
			case DialogType.SlowBuildCreation:
				gameObject.AddComponent<SlowBuildCreationDialog>();
				break;
			case DialogType.Softban:
				gameObject.AddComponent<SoftbanDialog>();
				break;
			case DialogType.Start:
				gameObject.AddComponent<StartDialog>();
				break;
			case DialogType.SubAreas:
				gameObject.AddComponent<SubAreasDialog>();
				break;
			case DialogType.PlacedSubThings:
				gameObject.AddComponent<PlacedSubThingsDialog>();
				break;
			case DialogType.IncludedSubThings:
				gameObject.AddComponent<IncludedSubThingsDialog>();
				break;
			case DialogType.IncludedSubThing:
				gameObject.AddComponent<IncludedSubThingDialog>();
				break;
			case DialogType.ThingAttributes:
				gameObject.AddComponent<ThingAttributesDialog>();
				break;
			case DialogType.ThingAttributesMore:
				gameObject.AddComponent<ThingAttributesMoreDialog>();
				break;
			case DialogType.Thing:
				gameObject.AddComponent<ThingDialog>();
				break;
			case DialogType.ThingCopyPaste:
				gameObject.AddComponent<ThingCopyPasteDialog>();
				break;
			case DialogType.IncludeThing:
				gameObject.AddComponent<IncludeThingDialog>();
				break;
			case DialogType.ThingPart:
				gameObject.AddComponent<ThingPartDialog>();
				break;
			case DialogType.ThingPartAttributes:
				gameObject.AddComponent<ThingPartAttributesDialog>();
				break;
			case DialogType.ThingPartAttributesMore:
				gameObject.AddComponent<ThingPartAttributesMoreDialog>();
				break;
			case DialogType.ThingPartAutoContinuation:
				gameObject.AddComponent<ThingPartAutoContinuationDialog>();
				break;
			case DialogType.ThingPartScreenSettings:
				gameObject.AddComponent<ThingPartScreenSettingsDialog>();
				break;
			case DialogType.ThingPartCopyPaste:
				gameObject.AddComponent<ThingPartCopyPasteDialog>();
				break;
			case DialogType.ThingPhysics:
				gameObject.AddComponent<ThingPhysicsDialog>();
				break;
			case DialogType.ThingTags:
				gameObject.AddComponent<ThingTagsDialog>();
				break;
			case DialogType.PlacementSuppress:
				gameObject.AddComponent<PlacementSuppressDialog>();
				break;
			case DialogType.TwitchChat:
				gameObject.AddComponent<TwitchChatDialog>();
				break;
			case DialogType.TwitchGuiding:
				gameObject.AddComponent<TwitchGuidingDialog>();
				break;
			case DialogType.TwitchSettings:
				gameObject.AddComponent<TwitchSettingsDialog>();
				break;
			case DialogType.VideoControl:
				gameObject.AddComponent<VideoControlDialog>();
				break;
			case DialogType.Video:
				gameObject.AddComponent<VideoDialog>();
				break;
			case DialogType.Browser:
				gameObject.AddComponent<BrowserDialog>();
				break;
			case DialogType.Equipment:
				gameObject.AddComponent<EquipmentDialog>();
				break;
			case DialogType.HighlightThings:
				gameObject.AddComponent<HighlightThingsDialog>();
				break;
			case DialogType.InventorySearchWords:
				gameObject.AddComponent<InventorySearchWordsDialog>();
				break;
			case DialogType.VertexMover:
				gameObject.AddComponent<VertexMoverDialog>();
				break;
			case DialogType.Subdivide:
				gameObject.AddComponent<SubdivideDialog>();
				break;
			case DialogType.Forum:
				gameObject.AddComponent<ForumDialog>();
				break;
			case DialogType.Forums:
				gameObject.AddComponent<ForumsDialog>();
				break;
			case DialogType.ForumSettings:
				gameObject.AddComponent<ForumSettingsDialog>();
				break;
			case DialogType.ForumsSearchResult:
				gameObject.AddComponent<ForumsSearchResultDialog>();
				break;
			case DialogType.ForumThreadSearchResult:
				gameObject.AddComponent<ForumThreadSearchResultDialog>();
				break;
			case DialogType.ForumThread:
				gameObject.AddComponent<ForumThreadDialog>();
				break;
			case DialogType.ForumThreadSettings:
				gameObject.AddComponent<ForumThreadSettingsDialog>();
				break;
			}
		}
		return gameObject;
	}
}
