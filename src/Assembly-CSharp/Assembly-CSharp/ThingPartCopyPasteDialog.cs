using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class ThingPartCopyPasteDialog : Dialog
{
	// Token: 0x06000C56 RID: 3158 RVA: 0x0006ECB0 File Offset: 0x0006D0B0
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddHeadline("Copy & Paste", -370, -460, TextColor.Default, TextAlignment.Left, false);
		if (this.hand.lastContextInfoHit != null)
		{
			this.thingPart = this.hand.lastContextInfoHit.GetComponent<ThingPart>();
		}
		this.AddButtons();
		if (this.thingPart.isLocked)
		{
			this.AddLockButton();
		}
	}

	// Token: 0x06000C57 RID: 3159 RVA: 0x0006ED45 File Offset: 0x0006D145
	private void AddButtons()
	{
		base.StartCoroutine(this.DoAddButtons());
		base.StartCoroutine(this.DoAddBacksideButtons());
	}

	// Token: 0x06000C58 RID: 3160 RVA: 0x0006ED64 File Offset: 0x0006D164
	private IEnumerator DoAddButtons()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int y = -290;
		TextColor defaultColor = TextColor.Default;
		TextColor ghostedColor = TextColor.Gray;
		TransformClipboard clipboard = CreationHelper.transformClipboard;
		base.AddLabel("Position", -435, y + -21, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton("clipboard_copyPosition", null, "Copy ➜", "ButtonCompactNoIconShortCentered", -80, y, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		string text = "clipboard_pastePosition";
		string text2 = null;
		string text3 = "➜ Paste";
		string text4 = "ButtonCompactNoIconShortCentered";
		int num = 220;
		int num2 = y;
		Vector3? position = clipboard.position;
		TextColor textColor = ((position == null || this.thingPart.isLocked) ? ghostedColor : defaultColor);
		base.AddButton(text, text2, text3, text4, num, num2, null, false, 1f, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		y += 100;
		base.AddLabel("Rotation", -435, y + -21, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton("clipboard_copyRotation", null, "Copy ➜", "ButtonCompactNoIconShortCentered", -80, y, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		text4 = "clipboard_pasteRotation";
		text3 = null;
		text2 = "➜ Paste";
		text = "ButtonCompactNoIconShortCentered";
		num2 = 220;
		num = y;
		Vector3? rotation = clipboard.rotation;
		textColor = ((rotation == null || this.thingPart.isLocked) ? ghostedColor : defaultColor);
		base.AddButton(text4, text3, text2, text, num2, num, null, false, 1f, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		y += 100;
		base.AddLabel("Size", -435, y + -21, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton("clipboard_copyScale", null, "Copy ➜", "ButtonCompactNoIconShortCentered", -80, y, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		text = "clipboard_pasteScale";
		text2 = null;
		text3 = "➜ Paste";
		text4 = "ButtonCompactNoIconShortCentered";
		num = 220;
		num2 = y;
		Vector3? scale = clipboard.scale;
		textColor = ((scale == null || this.thingPart.isLocked) ? ghostedColor : defaultColor);
		base.AddButton(text, text2, text3, text4, num, num2, null, false, 1f, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		y += 85;
		base.AddSeparator(0, y, false);
		y += 85;
		y -= 40;
		base.AddLabel("Scripts", -435, y, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		y += 95;
		base.AddLabel("This State", -435, y + -21, 0.7f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton("copyState", null, "Copy ➜", "ButtonCompactNoIconShortCentered", -80, y, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		text4 = "pasteState";
		text3 = null;
		text2 = "➜ Paste";
		text = "ButtonCompactNoIconShortCentered";
		num2 = 220;
		num = y;
		textColor = ((CreationHelper.statesCopy == null || this.thingPart.isLocked) ? ghostedColor : defaultColor);
		base.AddButton(text4, text3, text2, text, num2, num, null, false, 1f, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		GameObject searchAndReplaceState = base.AddModelButton("Find", "findAndReplaceInState", null, 405, y + 12, false);
		base.ScaleModelButtonWidthHeight(searchAndReplaceState, 0.6f);
		y += 100;
		base.AddLabel("All States", -435, y + -21, 0.7f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddButton("copyStates", null, "Copy ➜", "ButtonCompactNoIconShortCentered", -80, y, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		text = "pasteStates";
		text2 = null;
		text3 = "➜ Paste";
		text4 = "ButtonCompactNoIconShortCentered";
		num = 220;
		num2 = y;
		textColor = ((CreationHelper.statesCopy == null || this.thingPart.isLocked) ? ghostedColor : defaultColor);
		base.AddButton(text, text2, text3, text4, num, num2, null, false, 1f, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		GameObject searchAndReplaceStates = base.AddModelButton("Find", "findAndReplaceInStates", null, 405, y + 12, false);
		base.ScaleModelButtonWidthHeight(searchAndReplaceStates, 0.6f);
		if (this.PartSupportsImagePasting())
		{
			y += 85;
			base.AddSeparator(0, y, false);
			y += 85;
			base.AddLabel("Image", -435, y + -21, 0.7f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			bool flag = !string.IsNullOrEmpty(this.thingPart.imageUrl);
			if (flag)
			{
				GameObject gameObject = base.AddModelButton("Trash", "removeImageUrl", null, -270, y + 10, false);
				base.ScaleModelButtonWidthHeight(gameObject, 0.6f);
			}
			text4 = "copyImageUrl";
			text3 = null;
			text2 = "Copy ➜";
			text = "ButtonCompactNoIconShortCentered";
			num2 = -80;
			num = y;
			textColor = ((!flag) ? ghostedColor : defaultColor);
			base.AddButton(text4, text3, text2, text, num2, num, null, false, 1f, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			text = "pasteImageUrl";
			text2 = null;
			text3 = "➜ Paste";
			text4 = "ButtonCompactNoIconShortCentered";
			num = 220;
			num2 = y;
			textColor = ((string.IsNullOrEmpty(GUIUtility.systemCopyBuffer) || this.thingPart.isLocked) ? ghostedColor : defaultColor);
			base.AddButton(text, text2, text3, text4, num, num2, null, false, 1f, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			GameObject gameObject2 = base.AddModelButton("ContextHelpButton", "imageUrl_help", null, 420, y, false);
			base.ScaleModelButtonWidthHeight(gameObject2, 0.65f);
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x0006ED80 File Offset: 0x0006D180
	private IEnumerator DoAddBacksideButtons()
	{
		if (this.backsideWrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.backsideWrapper);
			yield return false;
		}
		this.backsideWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.backsideWrapper);
		base.AddModelButton("Find", "findAndReplaceInAllPartStates", null, -280, 0, true);
		base.AddLabel("Replace in all parts...", -180, -52, 0.85f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, true, TextAnchor.MiddleLeft);
		base.RotateBacksideWrapper();
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x0006ED9C File Offset: 0x0006D19C
	private void AddLockButton()
	{
		GameObject gameObject = base.AddModelButton("Lock", "toggleLock", null, 225, -433, false);
		DialogPart component = gameObject.GetComponent<DialogPart>();
		component.autoStopHighlight = false;
		component.state = this.thingPart.isLocked;
		base.ApplyEmissionColorToShape(gameObject, this.thingPart.isLocked);
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x0006EDF7 File Offset: 0x0006D1F7
	private bool PartSupportsImagePasting()
	{
		return !this.thingPart.isText;
	}

	// Token: 0x06000C5C RID: 3164 RVA: 0x0006EE08 File Offset: 0x0006D208
	private bool PasteImageIfValidates()
	{
		bool flag = false;
		string text = GUIUtility.systemCopyBuffer;
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Replace("http://steamuserimages", "https://steamuserimages");
			if (text.IndexOf("https://steamuserimages-a.akamaihd.net/ugc/") == 0)
			{
				string text2 = text.Replace("https://steamuserimages-a.akamaihd.net/ugc/", string.Empty);
				string[] array = Misc.Split(text2, "?", StringSplitOptions.RemoveEmptyEntries);
				if (array.Length >= 1)
				{
					text2 = array[0];
				}
				if (Misc.GetThisCharInStringCount(text2, '/') == 2 && Validator.ContainsOnly(text2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789/"))
				{
					this.thingPart.imageUrl = text2;
					this.thingPart.imageType = ImageType.Jpeg;
					flag = true;
				}
			}
			else if (text.IndexOf("http://www.coverbrowser.com/image/") == 0)
			{
				string text3 = text.Replace("http://www.coverbrowser.com/image/", string.Empty);
				if (Validator.ContainsOnly(text3, "abcdefghijklmnopqrstuvwxyz0123456789-_/."))
				{
					this.thingPart.imageUrl = text.Replace("http://www.coverbrowser.com/image/", "http://cache.coverbrowser.com/image/");
					this.thingPart.imageType = ImageType.Jpeg;
					flag = true;
				}
			}
		}
		if (flag)
		{
			this.thingPart.startedAutoAddingImage = false;
			this.thingPart.ResetStates();
		}
		return flag;
	}

	// Token: 0x06000C5D RID: 3165 RVA: 0x0006EF2C File Offset: 0x0006D32C
	private void HandleFindAndReplace(bool forSingleState = false, bool forAllParts = false, bool wasOpenedViaBackside = false)
	{
		ThingPartCopyPasteDialog.<HandleFindAndReplace>c__AnonStorey3 <HandleFindAndReplace>c__AnonStorey = new ThingPartCopyPasteDialog.<HandleFindAndReplace>c__AnonStorey3();
		<HandleFindAndReplace>c__AnonStorey.forAllParts = forAllParts;
		<HandleFindAndReplace>c__AnonStorey.forSingleState = forSingleState;
		<HandleFindAndReplace>c__AnonStorey.$this = this;
		DialogManager dialogManager = Managers.dialogManager;
		Action<string> action = delegate(string textToFind)
		{
			if (!string.IsNullOrEmpty(textToFind) && <HandleFindAndReplace>c__AnonStorey.$this.thingPart != null)
			{
				Managers.dialogManager.GetInput(delegate(string textToReplace)
				{
					if (!string.IsNullOrEmpty(textToReplace) && <HandleFindAndReplace>c__AnonStorey.thingPart != null)
					{
						string text = string.Empty;
						TextColor textColor = TextColor.Default;
						if (textToFind == textToReplace)
						{
							text = "Oops, text to find cannot be same as text to replace...";
							textColor = TextColor.Red;
						}
						else
						{
							int num = 0;
							if (<HandleFindAndReplace>c__AnonStorey.forAllParts)
							{
								IEnumerator enumerator = <HandleFindAndReplace>c__AnonStorey.thingPart.transform.parent.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										object obj = enumerator.Current;
										Transform transform = (Transform)obj;
										ThingPart component = transform.GetComponent<ThingPart>();
										if (component != null)
										{
											num += Managers.thingManager.FindAndReplaceInScripts(component, textToFind, textToReplace, <HandleFindAndReplace>c__AnonStorey.forSingleState);
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
							}
							else
							{
								num += Managers.thingManager.FindAndReplaceInScripts(<HandleFindAndReplace>c__AnonStorey.thingPart, textToFind, textToReplace, <HandleFindAndReplace>c__AnonStorey.forSingleState);
							}
							if (num > 0)
							{
								text = "✔ Replaced " + num + " occurrences";
								textColor = TextColor.Green;
							}
							else
							{
								text = "This wasn't found...";
							}
						}
						Managers.dialogManager.ShowInfo(text, true, true, 0, DialogType.ThingPart, 1f, false, textColor, TextAlignment.Left);
					}
					else
					{
						<HandleFindAndReplace>c__AnonStorey.SwitchTo(DialogType.ThingPartCopyPaste, string.Empty);
					}
				}, string.Empty, string.Empty, 60, "... text to replace with", true, true, false, false, 1f, false, false, null, false);
			}
			else
			{
				<HandleFindAndReplace>c__AnonStorey.$this.SwitchTo(DialogType.ThingPartCopyPaste, string.Empty);
			}
		};
		dialogManager.GetInput(action, string.Empty, string.Empty, -1, "Find & Replace: Text to find...", true, true, false, false, 1f, false, false, null, wasOpenedViaBackside);
	}

	// Token: 0x06000C5E RID: 3166 RVA: 0x0006EF8C File Offset: 0x0006D38C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (this.thingPart == null)
		{
			base.CloseDialog();
		}
		bool flag = contextName == "copyState" || contextName == "pasteState" || contextName == "findAndReplaceInState";
		switch (contextName)
		{
		case "copyState":
		case "copyStates":
			CreationHelper.statesCopy = new ThingPartStatesCopy();
			if (flag)
			{
				CreationHelper.statesCopy.CopySingleFromThingPart(this.thingPart);
			}
			else
			{
				CreationHelper.statesCopy.CopyAllFromThingPart(this.thingPart);
			}
			Managers.soundManager.Play("pickUp", this.transform, 0.35f, false, false);
			this.AddButtons();
			break;
		case "pasteState":
		case "pasteStates":
			if (!this.thingPart.isLocked && CreationHelper.statesCopy != null)
			{
				if (flag)
				{
					CreationHelper.statesCopy.PasteSingleToThingPart(this.thingPart);
				}
				else
				{
					CreationHelper.statesCopy.PasteAllToThingPart(this.thingPart);
				}
				Managers.soundManager.Play("putDown", this.transform, 0.35f, false, false);
				base.SwitchTo(DialogType.ThingPart, string.Empty);
			}
			else
			{
				Managers.soundManager.Play("no", this.transform, 0.35f, false, false);
			}
			break;
		case "findAndReplaceInState":
		case "findAndReplaceInStates":
			this.HandleFindAndReplace(flag, false, false);
			break;
		case "findAndReplaceInAllPartStates":
			if (!CrossDevice.desktopMode || Managers.desktopManager.showDialogBackside)
			{
				this.HandleFindAndReplace(false, true, true);
			}
			break;
		case "clipboard_copyPosition":
			CreationHelper.transformClipboard.position = new Vector3?(this.thingPart.transform.position);
			Managers.soundManager.Play("pickUp", this.transform, 0.35f, false, false);
			this.AddButtons();
			break;
		case "clipboard_copyRotation":
			CreationHelper.transformClipboard.rotation = new Vector3?(this.thingPart.transform.eulerAngles);
			Managers.soundManager.Play("pickUp", this.transform, 0.35f, false, false);
			this.AddButtons();
			break;
		case "clipboard_copyScale":
			CreationHelper.transformClipboard.scale = new Vector3?(this.thingPart.transform.localScale);
			Managers.soundManager.Play("pickUp", this.transform, 0.35f, false, false);
			this.AddButtons();
			break;
		case "clipboard_pastePosition":
			if (!this.thingPart.isLocked)
			{
				Vector3? position = CreationHelper.transformClipboard.position;
				if (position != null)
				{
					Transform transform = this.thingPart.transform;
					Vector3? position2 = CreationHelper.transformClipboard.position;
					transform.position = position2.Value;
					this.thingPart.SetStatePropertiesByTransform(true);
					Managers.soundManager.Play("putDown", this.transform, 0.35f, false, false);
					break;
				}
			}
			Managers.soundManager.Play("no", this.transform, 0.35f, false, false);
			break;
		case "clipboard_pasteRotation":
			if (!this.thingPart.isLocked)
			{
				Vector3? rotation = CreationHelper.transformClipboard.rotation;
				if (rotation != null)
				{
					Transform transform2 = this.thingPart.transform;
					Vector3? rotation2 = CreationHelper.transformClipboard.rotation;
					transform2.eulerAngles = rotation2.Value;
					this.thingPart.SetStatePropertiesByTransform(true);
					Managers.soundManager.Play("putDown", this.transform, 0.35f, false, false);
					break;
				}
			}
			Managers.soundManager.Play("no", this.transform, 0.35f, false, false);
			break;
		case "clipboard_pasteScale":
			if (!this.thingPart.isLocked)
			{
				Vector3? scale = CreationHelper.transformClipboard.scale;
				if (scale != null)
				{
					Transform transform3 = this.thingPart.transform;
					Vector3? scale2 = CreationHelper.transformClipboard.scale;
					transform3.localScale = scale2.Value;
					this.thingPart.SetStatePropertiesByTransform(true);
					Managers.soundManager.Play("putDown", this.transform, 0.35f, false, false);
					break;
				}
			}
			Managers.soundManager.Play("no", this.transform, 0.35f, false, false);
			break;
		case "toggleLock":
			this.thingPart.isLocked = state;
			if (state)
			{
				Managers.soundManager.Play("success", this.transform, 0.1f, false, false);
			}
			this.AddButtons();
			break;
		case "copyImageUrl":
			if (!string.IsNullOrEmpty(this.thingPart.imageUrl))
			{
				GUIUtility.systemCopyBuffer = this.thingPart.GetFullImageUrl(true, 75);
				Managers.soundManager.Play("pickUp", this.transform, 0.35f, false, false);
				this.AddButtons();
			}
			else
			{
				Managers.soundManager.Play("no", this.transform, 0.35f, false, false);
			}
			break;
		case "pasteImageUrl":
			if (this.PasteImageIfValidates())
			{
				Managers.soundManager.Play("putDown", this.transform, 0.35f, false, false);
				this.AddButtons();
			}
			else
			{
				Managers.soundManager.Play("no", this.transform, 0.35f, false, false);
			}
			break;
		case "removeImageUrl":
			if (!string.IsNullOrEmpty(this.thingPart.imageUrl))
			{
				this.thingPart.RemoveImageTexture();
				Managers.soundManager.Play("delete", this.transform, 0.35f, false, false);
				this.AddButtons();
			}
			break;
		case "imageUrl_help":
			if (!this.didOpenImageUploadUrl)
			{
				this.didOpenImageUploadUrl = true;
				Application.OpenURL("http://steamcommunity.com/sharedfiles/edititem/505700/3/");
			}
			base.ToggleHelpLabel("Upload your art to the Steam Anyland section that just opened on desktop, then copy the image URL to your clipboard to paste as texture here. (CoverBrowser.com works too.) Toggle Me-setting \"Disable all snapping\" to stretch cubes.", -700, 1f, 50, 0.7f);
			break;
		case "back":
			base.SwitchTo(DialogType.ThingPart, string.Empty);
			break;
		case "close":
			CreationHelper.thingPartWhoseStatesAreEdited = null;
			base.SwitchTo(DialogType.Create, string.Empty);
			break;
		}
	}

	// Token: 0x04000951 RID: 2385
	private ThingPart thingPart;

	// Token: 0x04000952 RID: 2386
	private bool didOpenImageUploadUrl;

	// Token: 0x04000953 RID: 2387
	private const float volume = 0.35f;
}
