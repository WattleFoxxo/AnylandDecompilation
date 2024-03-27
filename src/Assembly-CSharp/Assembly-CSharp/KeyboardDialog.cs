using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200011C RID: 284
public class KeyboardDialog : Dialog
{
	// Token: 0x06000A58 RID: 2648 RVA: 0x0004C62C File Offset: 0x0004AA2C
	public void Start()
	{
		Managers.forumManager.commentThingIdBeingCreated = null;
		string text = this.textContextName;
		if (text != null)
		{
			if (!(text == "createForumThreadComment") && !(text == "createForumThreadInitialComment"))
			{
				if (text == "editBehaviorLine")
				{
					this.isBehaviorScript = true;
				}
			}
			else
			{
				this.isWaitingForThingInclusion = true;
			}
		}
		if (this.isBehaviorScript || this.textContextName == "editThingTags" || this.textContextName == "findAreas" || this.textContextName == "editCreationText" || this.textContextName == "editBrowserUrl" || this.textContextName == "editThingPartName")
		{
			this.autoCompletion = new AutoCompletion();
		}
		base.Init(base.gameObject, false, false, false);
		base.AddButton("close", null, null, "ButtonClosePositioned", 440, 0, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		this.AddKeyboardButtons();
		this.AddKeyboardBacksideButtons();
		Side side = ((!(this.transform.parent.name == "HandCoreLeft")) ? Side.Right : Side.Left);
		if (this.transformToUseAtStart != null)
		{
			this.transform.parent = null;
			this.transformToUseAtStart.ApplyToTransform(this.transform);
		}
		else
		{
			float num = 5f;
			if (side == Side.Right)
			{
				num *= -1f;
			}
			if (!CrossDevice.desktopMode)
			{
				this.transform.parent = null;
				this.transform.Rotate(Vector3.forward * num);
				this.transform.parent = Managers.personManager.ourPerson.Rig.transform;
			}
			if (this.wasOpenedViaBackside)
			{
				this.transform.parent = null;
				this.transform.Rotate(Vector3.forward * 180f);
				this.transform.parent = Managers.personManager.ourPerson.Rig.transform;
			}
		}
		this.SetAppropriateTextMesh();
		if (this.placeholderHint != string.Empty)
		{
			this.placeholderHintTextMesh = base.AddLabel(string.Empty, -930, -465, 1.4f * this.placeholderFontSizeAdjust, false, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		this.caretPosition = this.textEntered.Length;
		this.UpdateTextMesh();
		base.Invoke("FlipCaretShows", this.caretFlipSeconds);
		if (this.autoCompletion != null)
		{
			if (this.isBehaviorScript)
			{
				this.CreateCompletionsBackIfNotExisting();
			}
			this.helpText = base.AddLabel(string.Empty, -950, -1230, 1.1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			this.helpText.transform.localPosition -= this.helpText.transform.up * 0.01f;
			this.UpdateAutoCompletion();
			if (this.isBehaviorScript)
			{
				this.helpText.text = "e.g. when touched then play doorbell, state 2 in 1s".ToUpper();
			}
		}
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x0004C990 File Offset: 0x0004AD90
	private void SetAppropriateTextMesh()
	{
		if (this.textContextName == "editCreationText")
		{
			this.caret = "_";
			global::UnityEngine.Object.Destroy(this.transform.Find("TextBackQuad").gameObject);
			this.textMesh = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<TextMesh>();
			this.textMesh.text = this.textEntered;
			this.isFontWithInternationalCharacters = this.textMesh.GetComponent<ThingPart>().baseType == ThingPartBase.TextArialBold;
		}
		else
		{
			Transform transform = base.AddHeadline(this.textEntered, -930, -465, TextColor.Default, TextAlignment.Left, false);
			transform.localScale = new Vector3(0.002025f, 0.002025f, 0.002025f);
			this.textMesh = transform.GetComponent<TextMesh>();
		}
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0004CA58 File Offset: 0x0004AE58
	public bool HandleAddedThingFromInventory(GameObject thingToBeAdded)
	{
		bool flag = false;
		if (this.isWaitingForThingInclusion)
		{
			Thing component = thingToBeAdded.GetComponent<Thing>();
			if (this.textContextName == "createForumThreadComment" || this.textContextName == "createForumThreadInitialComment")
			{
				flag = true;
				Managers.forumManager.commentThingIdBeingCreated = component.thingId;
				Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
			}
			else if (this.isBehaviorScript && CreationHelper.thingBeingEdited != null)
			{
				flag = true;
				Thing component2 = CreationHelper.thingBeingEdited.GetComponent<Thing>();
				string text = null;
				foreach (KeyValuePair<string, string> keyValuePair in component2.includedNameIds)
				{
					if (keyValuePair.Value == component.thingId)
					{
						text = keyValuePair.Key;
						break;
					}
				}
				if (text == null)
				{
					for (int i = 1; i <= 1000; i++)
					{
						string text2 = component.givenName;
						if (i > 1)
						{
							text2 = text2 + " " + i;
						}
						if (!component2.includedNameIds.ContainsKey(text2))
						{
							text = text2;
							component2.includedNameIds.Add(text, component.thingId);
							break;
						}
					}
				}
				this.isWaitingForThingInclusion = false;
				this.textEntered = this.textEntered.Replace("[drop from inventory]", text);
				this.caretPosition = this.textEntered.Length;
				this.UpdateTextMesh();
				Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
			}
		}
		return flag;
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x0004CC34 File Offset: 0x0004B034
	private void ResetCaret()
	{
		base.CancelInvoke("FlipCaretShows");
		this.caretShows = true;
		base.Invoke("FlipCaretShows", this.caretFlipSeconds);
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x0004CC5C File Offset: 0x0004B05C
	private void FlipCaretShows()
	{
		this.caretShows = !this.caretShows;
		if (this.KeepCaretSteadilyVisible())
		{
			this.caretShows = true;
		}
		this.UpdateTextMesh();
		base.Invoke("FlipCaretShows", this.caretFlipSeconds * (float)((!this.caretShows) ? 1 : 2));
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x0004CCB8 File Offset: 0x0004B0B8
	private bool KeepCaretSteadilyVisible()
	{
		bool flag = false;
		ThingPart component = this.textMesh.GetComponent<ThingPart>();
		if (component != null && (component.textAlignCenter || component.textAlignRight))
		{
			flag = true;
		}
		return flag;
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x0004CCF8 File Offset: 0x0004B0F8
	private void AddKeyboardButtons()
	{
		if (this.keysWrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.keysWrapper);
		}
		this.keysWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.keysWrapper);
		bool flag = this.textContextName == "createArea" || this.textContextName == "editAreaName" || this.textContextName == "createForum";
		string[] array = new string[]
		{
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"0",
			"-",
			"backspace",
			"newline",
			"q",
			"w",
			"e",
			"r",
			"t",
			"y",
			"u",
			"i",
			"o",
			"p",
			(!flag) ? null : "&",
			(!flag) ? null : "'",
			"newline",
			"spacing",
			"a",
			"s",
			"d",
			"f",
			"g",
			"h",
			"j",
			"k",
			"l",
			(!flag) ? null : ",",
			"newline",
			"spacing",
			"spacing",
			"z",
			"x",
			"c",
			"v",
			"b",
			"n",
			"m",
			"newline",
			"space",
			"done"
		};
		string[] array2 = new string[]
		{
			"1",
			"2",
			"3",
			"4",
			"5",
			"6",
			"7",
			"8",
			"9",
			"0",
			"-",
			"backspace",
			"newline",
			"q",
			"w",
			"e",
			"r",
			"t",
			"y",
			"u",
			"i",
			"o",
			"p",
			"!",
			"deleteWord",
			"newline",
			"spacing",
			"a",
			"s",
			"d",
			"f",
			"g",
			"h",
			"j",
			"k",
			"l",
			"'",
			"\"",
			"newline",
			"spacing",
			"spacing",
			"z",
			"x",
			"c",
			"v",
			"b",
			"n",
			"m",
			",",
			".",
			"?",
			"newline",
			(!this.allowSpecialKeysToggling) ? null : "nextKeysPage",
			"space",
			"done"
		};
		string[] array3 = new string[]
		{
			"~", "@", "#", "$", "%", "^", "&", "*", "(", ")",
			"_", "backspace", "newline", "[", "]", "{", "}", ";", ":", "<",
			">", "/", "=", "é", "newline", "spacing", "§", "+", "★", "☆",
			"☑", "☛", "✓", "→", "←", "|", "newline", "newline", "nextKeysPage", "space",
			"done"
		};
		string[] array4 = new string[]
		{
			"[", "]", "{", "}", ";", ":", "/", "<", ">", "=",
			"(", ")", "newline", "%", "$", "~", "@", "&", "*", "_",
			"+", "^", "#", "|", "!=", "newline", "newline", "newline", "nextKeysPage", "space",
			"done"
		};
		if (this.restrictToNumbers)
		{
			string[] array5 = new string[]
			{
				"1",
				"2",
				"3",
				"4",
				"5",
				"6",
				"7",
				"8",
				"9",
				"0",
				".",
				"backspace",
				"newline",
				(!this.allowNegativeNumbers) ? null : "-",
				"done"
			};
			array = array5;
		}
		if (this.allowSpecialKeysForPersonName)
		{
			string[] array6 = new string[]
			{
				"1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
				"-", "backspace", "newline", "q", "w", "e", "r", "t", "y", "u",
				"i", "o", "p", "[", "]", "newline", "spacing", "a", "s", "d",
				"f", "g", "h", "j", "k", "l", ",", ".", "newline", "@",
				"z", "x", "c", "v", "b", "n", "m", "&", "'", "\"",
				"_", "newline", "space", "done"
			};
			array = array6;
		}
		string[] array7 = ((!this.useExtendedKeys) ? array : array2);
		string[] array8 = null;
		if (Array.IndexOf<string>(array7, "nextKeysPage") >= 0)
		{
			array8 = ((!(this.textContextName == "editCreationText") && !(this.textContextName == "editBehaviorLine")) ? array3 : array4);
		}
		this.currentlyAllowedKeys = string.Empty;
		this.currentlyAllowedKeys += string.Join(string.Empty, array7);
		if (this.allowMixedCase)
		{
			this.currentlyAllowedKeys += this.currentlyAllowedKeys.ToUpper();
		}
		if (array8 != null)
		{
			this.currentlyAllowedKeys += string.Join(string.Empty, array8);
		}
		this.currentlyAllowedKeys += "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ -_'";
		string[] array9 = ((this.keysPage != 0) ? array8 : array7);
		int num = -1050;
		int num2 = -270;
		int i = 0;
		while (i < array9.Length)
		{
			string text = array9[i];
			switch (text)
			{

				goto IL_B4E;
			case "newline":
				num = -1050;
				num2 += 160;
				goto IL_D7E;
			case "spacing":
				num += 80;
				goto IL_D7E;
			case "space":
				num = 0;
				base.AddButton("key", " ", " ", "SpaceKey", num, num2, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				goto IL_D7E;
			case "done":
				base.AddButton(text, null, "✔", "ButtonSmall", 870, num2, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				goto IL_D7E;
			case "backspace":
				num += 160;
				base.AddButton(text, null, "←", "ButtonSmall", num, num2, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				goto IL_D7E;
			case "deleteWord":
				if (!this.allowNewlines)
				{
					num += 160;
					base.AddButton(text, null, "⇤", "ButtonSmall", num, num2, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
				goto IL_D7E;
			case "nextKeysPage":
			{
				num = -890;
				string text2 = ((this.keysPage != 0) ? "abc" : ((!(this.textContextName == "editCreationText")) ? "!#%" : "[ ]  /"));
				base.AddButton("nextKeysPage", null, text2, "ButtonSmall", num, num2, null, false, 0.39f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				goto IL_D7E;
			}
			case null:
				break;
			default:
			{
				num += 160;
				string text3 = text;
				if (this.upperCaseCurrentlyActive)
				{
					text3 = text3.ToUpper();
				}
				base.AddButton("key", text3, text3, "ButtonSmall", num, num2, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, true, false, TextAlignment.Center, false, false);
				break;
			}
			}
			IL_D7E:
			i++;
			continue;
			IL_B4E:
			goto IL_D7E;
		}
		if (this.allowNewlines && this.keysPage == 0)
		{
			base.AddButton("lineBreak", null, "↵", "ButtonSmall", 870, -110, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		if (this.keysPage == 0)
		{
			if (this.allowMixedCase)
			{
				base.AddButton("toggleUpperCase", null, null, "ButtonSmall", -890, 210, "upperCase", this.upperCaseCurrentlyActive, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
		}
		else if (this.keysPage == 1)
		{
			if (this.textContextName == "editCreationText" || (this.isBehaviorScript && this.textEntered.Contains(" then say")))
			{
				base.AddLabel("Placeholders: Type \"[\"", -940, 20, 1f, false, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
				if (this.isFontWithInternationalCharacters)
				{
					base.AddLabel("This font allows Ctrl+V pasting international characters", -940, -440, 0.9f, false, TextColor.Green, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
				}
			}
			if (this.textContextName == "createForumThreadComment" || this.textContextName == "createForumThreadInitialComment")
			{
				base.AddLabel("To add things:  Drag from your inventory into world\nTo add links:  Write [area name] or [board: board name]\nTo add video:  Copy a YouTube URL and hit Ctrl+V", -940, 140, 0.9f, false, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			}
		}
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x0004DC40 File Offset: 0x0004C040
	private void AddKeyboardBacksideButtons()
	{
		this.backsideWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.backsideWrapper);
		base.AddButton("copy", null, "Copy", "Button", -300, -75, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.AddButton("paste", null, "Paste", "Button", 300, -75, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.RotateBacksideWrapper();
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x0004DCF5 File Offset: 0x0004C0F5
	private void GoToNextKeysPage()
	{
		global::UnityEngine.Object.Destroy(this.keysWrapper);
		this.keysPage++;
		if (this.keysPage > 1)
		{
			this.keysPage = 0;
		}
		this.doUpdateKeysNextFrame = true;
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x0004DD2C File Offset: 0x0004C12C
	private new void Update()
	{
		if (this.doUpdateKeysNextFrame)
		{
			this.doUpdateKeysNextFrame = false;
			this.AddKeyboardButtons();
		}
		base.ReactToOnClick();
		this.HandleClipboardCopyPaste();
		base.ReactToOnClickInWrapper(this.completionsWrapper);
		base.ReactToOnClickInWrapper(this.keysWrapper);
		base.ReactToOnClickInWrapper(this.backsideWrapper);
		this.HandleHandPressDelete();
		this.UpdateAutoCompletion();
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0004DD8D File Offset: 0x0004C18D
	private void HandleClipboardCopyPaste()
	{
		if (Misc.CtrlIsPressed())
		{
			if (Input.GetKeyDown(KeyCode.C))
			{
				this.ClipboardCopy();
			}
			else if (Input.GetKeyDown(KeyCode.V))
			{
				this.ClipboardPaste();
			}
		}
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x0004DDC4 File Offset: 0x0004C1C4
	private void HandleHandPressDelete()
	{
		if (this.hand.GetPressDown(CrossDevice.button_delete) || this.hand.otherHandScript.GetPressDown(CrossDevice.button_delete))
		{
			if (this.helpText != null)
			{
				this.helpText.text = string.Empty;
			}
			this.ClearText();
			Managers.achievementManager.RegisterAchievement(Achievement.DeletedAllTextViaDeviceAction);
		}
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x0004DE34 File Offset: 0x0004C234
	public void ExecuteDoTypeTextCommand(string text)
	{
		if (Validator.ContainsOnly(text, "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ -_'!?\".,@#$%^&*()~[]{};:<>/=§+★☆☑☛✓→←|é"))
		{
			bool flag = text.IndexOf("when ") == 0;
			if (flag)
			{
				this.textEntered = text;
			}
			else
			{
				this.textEntered += text;
			}
			if (this.maxLength != -1)
			{
				this.textEntered = Misc.Truncate(this.textEntered, this.maxLength, false);
			}
			Managers.soundManager.Play("putDown", this.transform, 0.5f, false, false);
			this.caretPosition = this.textEntered.Length;
			this.UpdateTextMesh();
		}
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x0004DEDB File Offset: 0x0004C2DB
	private void ClipboardCopy()
	{
		GUIUtility.systemCopyBuffer = this.textEntered;
		Managers.soundManager.Play("pickUp", this.transform, 0.5f, false, false);
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x0004DF04 File Offset: 0x0004C304
	private void ClipboardPaste()
	{
		string systemCopyBuffer = GUIUtility.systemCopyBuffer;
		TextLink textLink = new TextLink();
		if (this.allowSpecialKeysToggling && this.textEntered.Contains("show web"))
		{
			this.textEntered += systemCopyBuffer;
		}
		else if (this.textContextName != "editBrowserUrl" && this.allowSpecialKeysToggling && textLink.TryParseURL(systemCopyBuffer))
		{
			this.textEntered += textLink.GetFullLink();
		}
		else if (this.allowSpecialKeysToggling && this.isFontWithInternationalCharacters)
		{
			this.textEntered += systemCopyBuffer;
		}
		else if (this.textContextName != "editCreationText" && this.textContextName != "editBehaviorLine" && this.allowSpecialKeysToggling && this.useExtendedKeys)
		{
			this.textEntered += systemCopyBuffer;
		}
		else if (Validator.ContainsOnly(systemCopyBuffer, this.currentlyAllowedKeys))
		{
			if (this.textContextName == "editCreationText" || systemCopyBuffer.Contains("["))
			{
				this.textEntered += systemCopyBuffer;
			}
			else
			{
				this.textEntered += systemCopyBuffer.ToLower();
			}
		}
		else
		{
			Managers.soundManager.Play("no", this.transform, 0.5f, false, false);
		}
		if (this.maxLength != -1)
		{
			this.textEntered = Misc.Truncate(this.textEntered, this.maxLength, false);
		}
		Managers.soundManager.Play("putDown", this.transform, 0.5f, false, false);
		this.caretPosition = this.textEntered.Length;
		this.UpdateTextMesh();
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x0004E0FC File Offset: 0x0004C4FC
	private void ClearText()
	{
		this.textEntered = string.Empty;
		this.caretPosition = 0;
		this.ResetCaret();
		this.UpdateTextMesh();
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0004E11C File Offset: 0x0004C51C
	private void UpdateAutoCompletion()
	{
		if (this.autoCompletion != null && (this.textEntered != this.lastTextSuggestedFor || this.autoCompletion.pageNumber != this.autoCompletion.lastUsedPageNumber))
		{
			if (this.lastTextSuggestedFor != null && this.textEntered.Trim() == this.lastTextSuggestedFor.Trim() && this.autoCompletion.pageNumber == this.autoCompletion.lastUsedPageNumber)
			{
				this.autoCompletion.pageNumber = 1;
				this.lastTextSuggestedFor = this.textEntered;
				return;
			}
			bool flag = this.completionsWrapper != null;
			if (flag)
			{
				global::UnityEngine.Object.Destroy(this.completionsWrapper);
				this.completionsWrapper = null;
			}
			else
			{
				List<AutoCompletionData> list = new List<AutoCompletionData>();
				if (this.textContextName == "editThingTags")
				{
					if (this.textEntered == string.Empty)
					{
						list = this.autoCompletion.GetThingTagStringsCompletions(this.textEntered);
					}
				}
				else if (this.textContextName == "findAreas")
				{
					if (this.textEntered == string.Empty)
					{
						list = this.autoCompletion.GetFindAreasStringsCompletions(this.textEntered);
					}
				}
				else if (this.textContextName == "editCreationText")
				{
					list = this.autoCompletion.GetCreationTextStringsCompletions(this.textEntered);
					if (list.Count == 0 && this.completionsBack != null)
					{
						global::UnityEngine.Object.Destroy(this.completionsBack);
					}
				}
				else if (this.textContextName == "editBrowserUrl")
				{
					list = this.autoCompletion.GetBrowserFavoritesCompletions(this.textEntered);
					if (list.Count == 0 && this.completionsBack != null)
					{
						global::UnityEngine.Object.Destroy(this.completionsBack);
					}
				}
				else if (this.textContextName == "editThingPartName")
				{
					list = this.autoCompletion.GetEditThingPartNameStringsCompletions(this.textEntered);
					if (list.Count == 0 && this.completionsBack != null)
					{
						global::UnityEngine.Object.Destroy(this.completionsBack);
					}
				}
				else
				{
					list = this.autoCompletion.GetBehaviorScriptCompletions(this.textEntered);
				}
				if (list.Count > 0)
				{
					this.CreateCompletionsBackIfNotExisting();
				}
				this.completionsWrapper = base.GetUiWrapper();
				base.SetUiWrapper(this.completionsWrapper);
				int num = -500 - 100 * list.Count;
				if (this.autoCompletion.pageCount > 1)
				{
					num -= 100;
					base.AddModelButton("ButtonBack", "previousCompletionsPage", null, -100, -590, false);
					base.AddModelButton("ButtonForward", "nextCompletionsPage", null, 60, -590, false);
				}
				int num2 = num;
				foreach (AutoCompletionData autoCompletionData in list)
				{
					string completion = autoCompletionData.completion;
					string text = string.Empty;
					if (this.autoCompletion.isFullLastWordReplacement)
					{
						char[] array = new char[] { ' ' };
						string[] array2 = this.textEntered.Split(array, StringSplitOptions.RemoveEmptyEntries);
						array2[array2.Length - 1] = completion;
						text = string.Join(" ", array2);
					}
					else if (this.autoCompletion.isSoundSearch)
					{
						string[] array3 = Misc.Split(this.textEntered, " play ", StringSplitOptions.None);
						string text2 = string.Empty;
						for (int i = 0; i < array3.Length - 1; i++)
						{
							text2 = text2 + array3[i] + " play ";
						}
						text = text2 + completion;
					}
					else
					{
						text = this.textEntered + completion;
					}
					text = Misc.ReplaceAll(text, "  ", " ");
					text = Misc.ReplaceAll(text, "when when ", "when ");
					string text3 = text;
					text3 = text3.Replace("[drop from inventory]", "[thing]");
					if (this.textContextName != "editCreationText")
					{
						text3 = Misc.TruncateRightAligned(text3.Trim(), 32, "..");
					}
					text = text.Replace("[area]", "...");
					text = text.Replace("[thing]", "...");
					text = text.Replace("[url]", string.Empty);
					text = text.Replace("[name]", string.Empty);
					text = text.Replace("[optional name]", string.Empty);
					text = text.Replace("[quest name]", string.Empty);
					text = text.Replace("[link]", string.Empty);
					text = text.Replace("[state]", string.Empty);
					text = text.Replace("[number]", string.Empty);
					text = text.Replace("[speech]", string.Empty);
					text = text.Replace("[text]", "\"");
					text = text.Replace("[search]", string.Empty);
					text = text.Replace("[strength]", string.Empty);
					text = text.Replace("[area name]", string.Empty);
					text = text.Replace("[thing name]", string.Empty);
					text = text.Replace("[person name]", string.Empty);
					text = text.Replace("[sound name]", string.Empty);
					text = text.Replace("[enter an area]", string.Empty);
					text = text.Replace("[play to record]", string.Empty);
					text3 += autoCompletionData.addToCompletionDisplay;
					base.AddButton("autoCompletion", text, text3, "AutoCompletionButton", -95, num2, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
					int num3 = 830;
					if (this.autoCompletion.isSoundRelated && Managers.soundLibraryManager.SoundExists(completion.Trim()))
					{
						if (completion.IndexOf("[") == -1)
						{
							base.AddModelButton("ContextHelpButton", "playSound", completion.Trim(), num3, num2, false);
						}
					}
					else if (this.autoCompletion.isLoopSoundRelated)
					{
						if (completion.IndexOf("[") == -1)
						{
							base.AddModelButton("ContextHelpButton", "playLoopSound", completion.Trim(), num3, num2, false);
						}
					}
					else if (autoCompletionData.help != string.Empty)
					{
						base.AddModelButton("ContextHelpButton", "completionHelp", autoCompletionData.help, num3, num2, false);
					}
					num2 += 100;
				}
				base.SetUiWrapper(base.gameObject);
				if (!this.textEntered.Contains("then play track") || this.textEntered.Contains("with"))
				{
					string text4 = this.textEntered.Trim();
					if (!(text4 == string.Empty) && !(text4 == "when"))
					{
						this.helpText.text = string.Empty;
					}
				}
				this.lastTextSuggestedFor = this.textEntered;
			}
		}
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0004E8B8 File Offset: 0x0004CCB8
	private void StartSoundTrackRecordingIfNeeded()
	{
		if (this.textContextName == "editBehaviorLine" && this.textEntered.Contains(" then play track ") && this.timeLastSoundRecorded == -1f)
		{
			this.timeLastSoundRecorded = Time.time;
			Managers.soundLibraryManager.recordingKeyboard = this;
			ThingPart component = CreationHelper.thingPartWhoseStatesAreEdited.GetComponent<ThingPart>();
			if (component != null)
			{
				component.ResetSoundTracks();
			}
			string text = "Recording started. Play on any Anyland instrument now." + Environment.NewLine + "Optionally add \"with\" afterwards to change settings.";
			this.helpText.text = text.ToUpper();
		}
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x0004E958 File Offset: 0x0004CD58
	private void CreateCompletionsBackIfNotExisting()
	{
		if (this.completionsBack == null)
		{
			this.completionsBack = GameObject.CreatePrimitive(PrimitiveType.Cube);
			Renderer component = this.completionsBack.GetComponent<Renderer>();
			component.material.color = new Color(0.6f, 0.7f, 0.7f);
			this.completionsBack.transform.parent = this.transform;
			this.completionsBack.transform.localRotation = Quaternion.identity;
			this.completionsBack.transform.localPosition = new Vector3(0f, 0f, 0.2f);
			this.completionsBack.transform.localScale = new Vector3(0.48f, 0.01f, 0.225f);
		}
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x0004EA20 File Offset: 0x0004CE20
	public void MoveCaretIfPossible(TextCaretDirection direction, Transform handDotTransform = null, Hand hand = null)
	{
		int num = this.caretPosition;
		switch (direction)
		{
		case TextCaretDirection.WordBack:
			if (this.caretPosition > 1)
			{
				int num2 = this.textEntered.LastIndexOf(" ", this.caretPosition - 2);
				this.caretPosition = ((num2 < 0) ? 0 : (num2 + 1));
			}
			else
			{
				this.caretPosition = 0;
			}
			break;
		case TextCaretDirection.WordForward:
			if (this.caretPosition < this.textEntered.Length)
			{
				int num3 = this.textEntered.IndexOf(" ", this.caretPosition + 1);
				this.caretPosition = ((num3 < 0) ? this.textEntered.Length : (num3 + 1));
				if (this.caretPosition > this.textEntered.Length)
				{
					this.caretPosition = this.textEntered.Length;
				}
			}
			break;
		case TextCaretDirection.LetterBack:
			if (this.caretPosition > 0)
			{
				this.caretPosition--;
			}
			break;
		case TextCaretDirection.LetterForward:
			if (this.caretPosition < this.textEntered.Length)
			{
				this.caretPosition++;
			}
			break;
		}
		this.ResetCaret();
		string text = "textCaretMoveNone";
		if (num != this.caretPosition)
		{
			this.UpdateTextMesh();
			if (direction == TextCaretDirection.WordBack || direction == TextCaretDirection.WordForward)
			{
				text = "textCaretMoveWord";
			}
			else
			{
				text = "textCaretMoveLetter";
			}
		}
		if (handDotTransform != null)
		{
			Managers.soundManager.Play(text, handDotTransform, 0.1f, false, false);
		}
		if (hand != null)
		{
			hand.TriggerHapticPulse(600);
		}
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0004EBD3 File Offset: 0x0004CFD3
	public void MoveCaretToStart()
	{
		this.caretPosition = 0;
		this.ResetCaret();
		this.UpdateTextMesh();
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x0004EBE8 File Offset: 0x0004CFE8
	public void MoveCaretToEnd()
	{
		this.caretPosition = this.textEntered.Length;
		this.ResetCaret();
		this.UpdateTextMesh();
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x0004EC08 File Offset: 0x0004D008
	private void UpdateTextMesh()
	{
		string text = this.textEntered;
		int length = this.textEntered.Length;
		if (this.caretPosition < 0)
		{
			this.caretPosition = 0;
		}
		else if (this.caretPosition > length)
		{
			this.caretPosition = length;
		}
		if (!this.allowMixedCase)
		{
			text = text.ToUpper();
		}
		int num = this.caretPosition;
		if (this.textContextName != "editCreationText" && length > 30)
		{
			int num2 = this.caretPosition - 15;
			if (num2 < 0)
			{
				num2 = 0;
			}
			int num3 = num2 + 30;
			if (num3 > length)
			{
				num3 = length;
			}
			if (length > 30 && this.caretPosition > length - 15)
			{
				num3 = length;
				num2 = num3 - 30;
			}
			text = text.Substring(num2, num3 - num2);
			num -= num2;
			if (num2 > 0)
			{
				text = ".." + text.Substring("..".Length);
			}
			if (num3 < length)
			{
				text = text.Substring(0, text.Length - "..".Length) + "..";
			}
		}
		string text2 = ((!this.caretShows) ? " " : this.caret);
		text = text.Insert(num, text2);
		this.textMesh.text = text;
		if (this.placeholderHintTextMesh != null && this.placeholderHint != null)
		{
			if (this.textEntered == string.Empty)
			{
				this.placeholderHintTextMesh.text = this.placeholderHint.ToUpper();
			}
			else
			{
				this.placeholderHintTextMesh.text = string.Empty;
			}
		}
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0004EDBC File Offset: 0x0004D1BC
	public void HandleDone()
	{
		if (this.textEntered.Trim() == string.Empty)
		{
			this.textEntered = string.Empty;
		}
		if (this.textContextName == "editCreationText")
		{
			if (this.textEntered == string.Empty)
			{
				this.textEntered = "abc";
			}
			this.textMesh.text = this.textEntered;
		}
		if (this.textEntered != string.Empty)
		{
			Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
		}
		if (this.floatCallback != null)
		{
			float num;
			if (float.TryParse(this.textEntered, out num))
			{
				this.floatCallback(new float?(num));
			}
			else
			{
				this.floatCallback(null);
			}
		}
		else if (this.stringCallback != null)
		{
			this.stringCallback(this.textEntered);
		}
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x0004EECD File Offset: 0x0004D2CD
	private void ToggleUpperCase()
	{
		this.upperCaseCurrentlyActive = !this.upperCaseCurrentlyActive;
		global::UnityEngine.Object.Destroy(this.keysWrapper);
		this.doUpdateKeysNextFrame = true;
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0004EEF0 File Offset: 0x0004D2F0
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "close":
			this.HandleAndCloseDialog();
			return;
		case "done":
			this.HandleDone();
			return;
		case "backspace":
			this.AddBackspace();
			return;
		case "deleteWord":
		{
			string text = this.textEntered.Substring(0, this.caretPosition);
			string text2 = this.textEntered.Substring(this.caretPosition);
			text = text.TrimEnd(new char[0]);
			int num2 = text.LastIndexOf(" ");
			if (num2 >= 0)
			{
				text = text.Substring(0, num2 + 1);
				this.caretPosition = num2 + 1;
			}
			else
			{
				text = string.Empty;
				this.caretPosition = 0;
			}
			this.textEntered = text + text2;
			this.textEntered = this.textEntered.Replace("  ", " ");
			if (!Managers.achievementManager.DidAchieve(Achievement.DeletedAllTextViaDeviceAction) && this.helpText != null)
			{
				this.helpText.text = string.Empty;
				string text3 = this.textEntered.Trim();
				if (text3 == "when" || text3 == string.Empty)
				{
					string text4 = "Tip: Full lines can also be deleted by pressing the" + Environment.NewLine;
					if (CrossDevice.desktopMode)
					{
						text4 += "middle mouse button";
					}
					else if (CrossDevice.type == global::DeviceType.Vive)
					{
						text4 += "controller grip";
					}
					else if (CrossDevice.type == global::DeviceType.Index)
					{
						text4 += "a button";
					}
					else if (CrossDevice.type == global::DeviceType.OculusTouch)
					{
						if (CrossDevice.oculusTouchLegacyMode)
						{
							text4 += "thumb-stick";
						}
						else
						{
							text4 += "a or x button";
						}
					}
					else
					{
						text4 += "controller's delete button";
					}
					this.helpText.text = text4.ToUpper();
				}
			}
			this.UpdateTextMesh();
			return;
		}
		case "autoCompletion":
			this.isWaitingForThingInclusion = false;
			if (contextId.IndexOf("[your search text]") >= 0)
			{
				global::UnityEngine.Object.Destroy(this.completionsWrapper);
				this.completionsWrapper = null;
			}
			else if (contextId.IndexOf("[drop from inventory]") >= 0)
			{
				this.isWaitingForThingInclusion = true;
				this.textEntered = contextId;
			}
			else if (contextId.IndexOf("[closest held]") >= 0)
			{
				this.textEntered = contextId;
			}
			else
			{
				string text5 = contextId.Replace("[e.g. ", string.Empty);
				text5 = text5.Replace("/ day/ hour etc.", string.Empty);
				if (this.textContextName != "editCreationText")
				{
					text5 = text5.Replace("]", string.Empty);
				}
				this.textEntered = text5;
			}
			this.caretPosition = this.textEntered.Length;
			this.autoCompletion.pageNumber = 1;
			if (this.textEntered == CreationHelper.lastScriptLineEntered || this.textContextName == "editThingTags" || this.textContextName == "findAreas" || this.textContextName == "editBrowserUrl" || this.textContextName == "editThingPartName")
			{
				this.HandleDone();
			}
			else
			{
				this.ResetCaret();
				this.UpdateTextMesh();
			}
			this.StartSoundTrackRecordingIfNeeded();
			return;
		case "previousCompletionsPage":
		{
			int num = --this.autoCompletion.pageNumber;
			if (num < 1)
			{
				this.autoCompletion.pageNumber = this.autoCompletion.pageCount;
			}
			return;
		}
		case "nextCompletionsPage":
		{
			int num = ++this.autoCompletion.pageNumber;
			if (num > this.autoCompletion.pageCount)
			{
				this.autoCompletion.pageNumber = 1;
			}
			return;
		}
		case "toggleUpperCase":
			this.ToggleUpperCase();
			this.autoToggleToLowerCase = true;
			if (this.upperCaseCurrentlyActive && this.textEntered.Length >= 2)
			{
				char c = this.textEntered[this.textEntered.Length - 1];
				if (c >= 'A' && c <= 'Z')
				{
					this.autoToggleToLowerCase = false;
				}
			}
			return;
		case "playSound":
		case "playLoopSound":
		{
			bool flag = contextName == "playLoopSound";
			string filePath = Managers.soundLibraryManager.GetFilePath(contextId, flag, false);
			if (filePath != null)
			{
				if (this.previewSound == null)
				{
					this.previewSound = base.gameObject.AddComponent<AudioSource>();
					this.previewSound.spatialBlend = 1f;
					this.previewSound.volume = 0.25f;
				}
				else
				{
					this.previewSound.Stop();
				}
				this.previewSound.clip = Resources.Load(filePath) as AudioClip;
				this.previewSound.Play();
			}
			return;
		}
		case "completionHelp":
			this.helpText.text = this.ToUpperExceptCertainKeywords(contextId);
			return;
		case "nextKeysPage":
			this.GoToNextKeysPage();
			return;
		case "lineBreak":
			this.AddNewLine();
			return;
		case "copy":
			this.ClipboardCopy();
			return;
		case "paste":
			this.ClipboardPaste();
			return;
		}
		this.AddCharacter(contextId, false);
		if (this.autoToggleToLowerCase && this.upperCaseCurrentlyActive)
		{
			this.ToggleUpperCase();
		}
		this.StartSoundTrackRecordingIfNeeded();
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x0004F56C File Offset: 0x0004D96C
	private string ToUpperExceptCertainKeywords(string s)
	{
		string[] array = new string[] { "AnylandTell(text)", "AnylandTellAny(text)", "AnylandClosePage()", "AnylandOpenBoards()", "AnylandTold(s, isAuthority)", "AnylandToldByAny(s, isAuthority)" };
		s = s.ToUpper();
		foreach (string text in array)
		{
			s = s.Replace(text.ToUpper(), text);
		}
		return s;
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0004F5E4 File Offset: 0x0004D9E4
	public void AddCharacter(string character, bool validate = true)
	{
		bool flag = true;
		if (validate)
		{
			flag = this.isFontWithInternationalCharacters || Validator.ContainsOnly(character, this.currentlyAllowedKeys);
		}
		if (flag)
		{
			if (this.maxLength == -1 || this.textEntered.Length < this.maxLength)
			{
				if (this.allowMixedCase)
				{
					this.textEntered = this.textEntered.Insert(this.caretPosition, character);
				}
				else
				{
					this.textEntered = this.textEntered.Insert(this.caretPosition, character.ToLower());
				}
				this.caretPosition += character.Length;
			}
			this.ResetCaret();
			this.UpdateTextMesh();
		}
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0004F6A0 File Offset: 0x0004DAA0
	public void DeleteNextCharacter()
	{
		if (this.textEntered != string.Empty && this.caretPosition < this.textEntered.Length)
		{
			this.textEntered = this.textEntered.Remove(this.caretPosition, 1);
			this.ResetCaret();
			this.UpdateTextMesh();
		}
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0004F6FC File Offset: 0x0004DAFC
	public void AddBackspace()
	{
		if (this.textEntered != string.Empty && this.caretPosition > 0)
		{
			this.textEntered = this.textEntered.Remove(--this.caretPosition, 1);
			if (this.caretPosition > 0 && this.textEntered[this.caretPosition - 1] == '\r')
			{
				this.textEntered = this.textEntered.Remove(--this.caretPosition, 1);
			}
		}
		this.ResetCaret();
		this.UpdateTextMesh();
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0004F7A4 File Offset: 0x0004DBA4
	public void AddNewLine()
	{
		if (this.maxLength == -1 || this.textEntered.Length < this.maxLength)
		{
			this.textEntered = this.textEntered.Insert(this.caretPosition, Environment.NewLine);
			this.caretPosition += Environment.NewLine.Length;
		}
		this.ResetCaret();
		this.UpdateTextMesh();
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0004F814 File Offset: 0x0004DC14
	public void HandleAndCloseDialog()
	{
		Managers.forumManager.commentThingIdBeingCreated = null;
		if (this.textContextName == "editCreationText")
		{
			this.HandleDone();
		}
		else if (this.floatCallback != null)
		{
			this.floatCallback(null);
		}
		else if (this.stringCallback != null)
		{
			this.stringCallback(null);
		}
		else
		{
			this.hand.SwitchToNewDialog(DialogType.Start, string.Empty);
		}
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x0004F8A0 File Offset: 0x0004DCA0
	public void RecordSound(Sound sound)
	{
		if (this.isBehaviorScript && this.textEntered.Contains("then play track"))
		{
			string text = string.Empty;
			if (this.timeLastSoundRecorded != -1f)
			{
				float num = Time.time - this.timeLastSoundRecorded;
				if (num >= 0.01f)
				{
					string compressedStringFromFloat = Misc.GetCompressedStringFromFloat(num, 2, true);
					text = text + " " + compressedStringFromFloat;
				}
			}
			text += " ";
			if (sound.HasModulators())
			{
				string stringData = sound.GetStringData();
				int num2 = this.customSounds.IndexOf(stringData);
				if (num2 == -1)
				{
					this.customSounds.Add(stringData);
					num2 = this.customSounds.Count - 1;
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"c",
						num2 + 1,
						"=",
						stringData
					});
				}
				else
				{
					text = text + "c" + (num2 + 1);
				}
				text = text.Replace("1.9999", "2");
			}
			else
			{
				text += Managers.soundLibraryManager.GetIdByName(sound.name);
			}
			if (this.textEntered.Length + text.Length <= 10000)
			{
				this.textEntered += text;
				this.MoveCaretToEnd();
				this.timeLastSoundRecorded = Time.time;
			}
		}
	}

	// Token: 0x040007BF RID: 1983
	public string textContextName = string.Empty;

	// Token: 0x040007C0 RID: 1984
	public string textEntered = string.Empty;

	// Token: 0x040007C1 RID: 1985
	public int maxLength = -1;

	// Token: 0x040007C2 RID: 1986
	public const string thingInclusionPlaceholder = "[drop from inventory]";

	// Token: 0x040007C3 RID: 1987
	public string placeholderHint = string.Empty;

	// Token: 0x040007C4 RID: 1988
	public bool useExtendedKeys;

	// Token: 0x040007C5 RID: 1989
	public bool allowSpecialKeysToggling;

	// Token: 0x040007C6 RID: 1990
	public bool allowNewlines;

	// Token: 0x040007C7 RID: 1991
	public bool allowMixedCase;

	// Token: 0x040007C8 RID: 1992
	public float placeholderFontSizeAdjust = 1f;

	// Token: 0x040007C9 RID: 1993
	public bool restrictToNumbers;

	// Token: 0x040007CA RID: 1994
	public bool allowSpecialKeysForPersonName;

	// Token: 0x040007CB RID: 1995
	public bool allowNegativeNumbers;

	// Token: 0x040007CC RID: 1996
	public bool wasOpenedViaBackside;

	// Token: 0x040007CD RID: 1997
	private string currentlyAllowedKeys = string.Empty;

	// Token: 0x040007CE RID: 1998
	private TextMesh textMesh;

	// Token: 0x040007CF RID: 1999
	private TextMesh placeholderHintTextMesh;

	// Token: 0x040007D0 RID: 2000
	private bool caretShows = true;

	// Token: 0x040007D1 RID: 2001
	private float caretFlipSeconds = 0.5f;

	// Token: 0x040007D2 RID: 2002
	private int caretPosition;

	// Token: 0x040007D3 RID: 2003
	private bool isBehaviorScript;

	// Token: 0x040007D4 RID: 2004
	private AutoCompletion autoCompletion;

	// Token: 0x040007D5 RID: 2005
	private string lastTextSuggestedFor;

	// Token: 0x040007D6 RID: 2006
	private GameObject completionsWrapper;

	// Token: 0x040007D7 RID: 2007
	private TextMesh helpText;

	// Token: 0x040007D8 RID: 2008
	private bool isWaitingForThingInclusion;

	// Token: 0x040007D9 RID: 2009
	private string caret = " \u0332";

	// Token: 0x040007DA RID: 2010
	private bool isFontWithInternationalCharacters;

	// Token: 0x040007DB RID: 2011
	private GameObject completionsBack;

	// Token: 0x040007DC RID: 2012
	private const int maxKeysPage = 1;

	// Token: 0x040007DD RID: 2013
	private int keysPage;

	// Token: 0x040007DE RID: 2014
	private GameObject keysWrapper;

	// Token: 0x040007DF RID: 2015
	private bool doUpdateKeysNextFrame;

	// Token: 0x040007E0 RID: 2016
	private bool upperCaseCurrentlyActive = true;

	// Token: 0x040007E1 RID: 2017
	private bool autoToggleToLowerCase;

	// Token: 0x040007E2 RID: 2018
	public Action<float?> floatCallback;

	// Token: 0x040007E3 RID: 2019
	public Action<string> stringCallback;

	// Token: 0x040007E4 RID: 2020
	private float timeLastSoundRecorded = -1f;

	// Token: 0x040007E5 RID: 2021
	private List<string> customSounds = new List<string>();

	// Token: 0x040007E6 RID: 2022
	public TransformClipboard transformToUseAtStart;

	// Token: 0x040007E7 RID: 2023
	private AudioSource previewSound;
}
