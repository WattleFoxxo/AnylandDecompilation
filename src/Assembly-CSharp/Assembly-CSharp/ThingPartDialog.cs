using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000145 RID: 325
public class ThingPartDialog : Dialog
{
	// Token: 0x06000C60 RID: 3168 RVA: 0x000703F0 File Offset: 0x0006E7F0
	public void Start()
	{
		this.currentLinesPage = CreationHelper.currentThingPartStateLinesPage;
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		this.thingPart = this.hand.lastContextInfoHit.GetComponent<ThingPart>();
		if (this.hand != null)
		{
			CreationHelper.thingPartWhoseStatesAreEdited = this.hand.lastContextInfoHit;
		}
		this.AddStateLabels();
		this.stateButtonsOffset = this.thingPart.currentState - 4 + 1;
		if (this.stateButtonsOffset < 0)
		{
			this.stateButtonsOffset = 0;
		}
		this.UpdateSubThingsButton();
		this.UpdateStateButtons();
		this.AddBehaviorScriptLines();
		base.AddBrush();
		GameObject gameObject = base.AddButton("attributes", null, null, "ButtonSmall", -400, 420, "attributes", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.MinifyButton(gameObject, 1f, 1f, 0.55f, false);
		if (this.thingPart.isText)
		{
			base.AddModelButton("EditTextButton", "editCreationText", null, 340, 410, false);
			base.AddLabel("abc", 383, 402, 1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		else
		{
			this.AddVertexMover();
		}
		this.UpdateUndoButton();
		GameObject gameObject2 = base.AddModelButton("Lock", "toggleLock", null, -240, 410, false);
		DialogPart component = gameObject2.GetComponent<DialogPart>();
		component.autoStopHighlight = false;
		component.state = this.thingPart.isLocked;
		base.ApplyEmissionColorToShape(gameObject2, this.thingPart.isLocked);
		base.AddBacksideEditButtons();
		this.copyPasteButton = base.AddButton("copyPaste", null, "Copy & Paste", "ButtonCompactNoIconShortCentered", 0, 420, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		this.thingPart.ResetAndStopStateAsCurrentlyEditing();
		this.thingPart.GetMyRootThing().SetLightShadows(true, null);
		Managers.achievementManager.RegisterAchievement(Achievement.ContextLaseredThingPartDuringCreation);
	}

	// Token: 0x06000C61 RID: 3169 RVA: 0x00070628 File Offset: 0x0006EA28
	private void AddVertexMover()
	{
		GameObject andAttachPrefabInstance = base.GetAndAttachPrefabInstance("VertexMover");
		if (this.handSide == Side.Right)
		{
			Vector3 localPosition = andAttachPrefabInstance.transform.localPosition;
			localPosition.x *= -1f;
			andAttachPrefabInstance.transform.localPosition = localPosition;
			Vector3 localEulerAngles = andAttachPrefabInstance.transform.localEulerAngles;
			localEulerAngles.z *= -1f;
			andAttachPrefabInstance.transform.localEulerAngles = localEulerAngles;
		}
	}

	// Token: 0x06000C62 RID: 3170 RVA: 0x000706A4 File Offset: 0x0006EAA4
	private void UpdateSubThingsButton()
	{
		this.showSubThings = !this.thingPart.isText && !this.thingPart.isLocked && Managers.areaManager.weAreEditorOfCurrentArea;
		if (this.showSubThings)
		{
			this.subThingsButton = base.AddButton("subThings", null, null, "ButtonSmall", 400, 420, "subConnections", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			base.MinifyButton(this.subThingsButton, 1f, 1f, 0.55f, false);
			if (this.thingPart.HasIncludedSubThings() || this.HasPlacedSubThingsEvenIfNotCurrentlyAttached())
			{
				base.SetButtonHighlight(this.subThingsButton, true);
			}
		}
		else
		{
			global::UnityEngine.Object.Destroy(this.subThingsButton);
		}
	}

	// Token: 0x06000C63 RID: 3171 RVA: 0x0007078C File Offset: 0x0006EB8C
	public void UpdateUndoButton()
	{
		if (this.undoButton != null)
		{
			global::UnityEngine.Object.Destroy(this.undoButton);
		}
		if (this.thingPart.HasUndoForThisState())
		{
			int num = ((!this.thingPart.isText && !this.showSubThings) ? 430 : 220);
			this.undoButton = base.AddButton("undo", null, null, "ButtonVerySmall", num, 420, "undo", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
	}

	// Token: 0x06000C64 RID: 3172 RVA: 0x00070834 File Offset: 0x0006EC34
	private void AddStateLabels()
	{
		float num = 0.75f;
		if (this.thingPart.states.Count == 1)
		{
			int num2 = -180;
			this.stateLabels[0] = base.AddLabel("you can move & color this", num2, -440, num, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			this.stateLabels[1] = base.AddLabel("shape for each state", num2, -390, num, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x000708B0 File Offset: 0x0006ECB0
	private void HideStateLabels()
	{
		for (int i = 0; i < this.stateLabels.Length; i++)
		{
			if (this.stateLabels[i] != null)
			{
				this.stateLabels[i].text = string.Empty;
			}
		}
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x000708FC File Offset: 0x0006ECFC
	public void ExecuteDoTypeTextCommand(string text)
	{
		if (Validator.ContainsOnly(text, "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ -_'!?\".,@#$%^&*()~[]{};:<>/=§+★☆☑☛✓→←|é") && this.thingPart != null)
		{
			ThingPartState thingPartState = this.thingPart.states[this.thingPart.currentState];
			bool flag = text.IndexOf("when ") == 0 || thingPartState.scriptLines.Count == 0;
			int num = thingPartState.scriptLines.Count - 1;
			if (flag)
			{
				num++;
			}
			if (num < 100)
			{
				if (flag)
				{
					thingPartState.scriptLines.Add(text);
				}
				else
				{
					List<string> scriptLines;
					int num2;
					(scriptLines = thingPartState.scriptLines)[num2 = num] = scriptLines[num2] + text;
				}
				thingPartState.scriptLines[num] = Misc.Truncate(thingPartState.scriptLines[num], 10000, false);
				Thing component = CreationHelper.thingBeingEdited.GetComponent<Thing>();
				thingPartState.ParseScriptLinesIntoListeners(component, this.thingPart, false);
				if (!string.IsNullOrEmpty(thingPartState.scriptLines[num]) && thingPartState.scriptLines[num].Trim() != "when")
				{
					CreationHelper.lastScriptLineEntered = thingPartState.scriptLines[num];
				}
				Managers.soundManager.Play("putDown", this.transform, 0.5f, false, false);
				global::UnityEngine.Object.Destroy(this.behaviorWrapper);
				this.AddBehaviorScriptLines();
			}
			else
			{
				Managers.soundManager.Play("no", this.transform, 0.5f, false, false);
			}
		}
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x00070A94 File Offset: 0x0006EE94
	private void UpdateStateButtons()
	{
		int num = this.stateButtonsOffset;
		int num2 = this.stateButtonsOffset + 4 - 1;
		for (int i = 0; i < this.stateButtons.Length; i++)
		{
			if (this.stateButtons[i] != null)
			{
				global::UnityEngine.Object.Destroy(this.stateButtons[i]);
			}
		}
		int num3 = 0;
		int num4 = 0;
		while (num4 < this.thingPart.states.Count + 1 && num4 < 50)
		{
			if (num4 >= num && num4 <= num2)
			{
				int num5 = -400 + num3 * 140;
				if (this.thingPart.states.Count > 3)
				{
					num5 += 121;
				}
				bool flag = num4 == this.thingPart.currentState;
				string text = (num4 + 1).ToString();
				float num6 = ((text.Length < 2) ? 1f : 0.8f);
				TextColor textColor = ((num4 != this.thingPart.states.Count) ? TextColor.Default : TextColor.LightGray);
				this.stateButtons[num3] = base.AddButton("state", num4.ToString(), text, "ButtonSmall", num5, -400, null, flag, num6, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				this.stateButtons[num3].SetActive(num4 < this.thingPart.states.Count + 1);
				Transform transform = this.stateButtons[num3].transform.Find("Text");
				TextMesh component = transform.GetComponent<TextMesh>();
				if (flag)
				{
					this.currentStateButton = this.stateButtons[num3];
				}
				component.alignment = TextAlignment.Center;
				component.anchor = TextAnchor.MiddleCenter;
				Vector3 localPosition = component.transform.localPosition;
				localPosition.x = 0f;
				localPosition.z = 0.00115f;
				component.transform.localPosition = localPosition;
				num3++;
			}
			num4++;
		}
		this.AddStatePageButtonsIfNeeded();
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x00070CB4 File Offset: 0x0006F0B4
	private void AddStatePageButtonsIfNeeded()
	{
		if (this.thingPart.states.Count > 3 && this.stateButtonsForward == null)
		{
			this.stateButtonsBack = base.AddModelButton("ButtonBack", "PreviousStateButtonsPage", null, -420, -400, false);
			this.stateButtonsBack.transform.localScale = new Vector3(0.85f, 1f, 0.85f);
			this.stateButtonsForward = base.AddModelButton("ButtonForward", "NextStateButtonsPage", null, 290, -400, false);
			this.stateButtonsForward.transform.localScale = new Vector3(0.85f, 1f, 0.85f);
		}
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x00070D74 File Offset: 0x0006F174
	private void GoToPreviousStateButtonsPage()
	{
		this.currentLinesPage = 0;
		this.stateButtonsOffset--;
		if (this.stateButtonsOffset < 0)
		{
			this.stateButtonsOffset = this.thingPart.states.Count + 2 - 4;
		}
		this.UpdateStateButtons();
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x00070DC4 File Offset: 0x0006F1C4
	private void GoToNextStateButtonsPage()
	{
		this.currentLinesPage = 0;
		this.stateButtonsOffset++;
		if (this.stateButtonsOffset + 4 > this.thingPart.states.Count + 2)
		{
			this.stateButtonsOffset = 0;
		}
		this.UpdateStateButtons();
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x00070E14 File Offset: 0x0006F214
	private IEnumerator UpdateBehaviorScriptLines()
	{
		global::UnityEngine.Object.Destroy(this.behaviorWrapper);
		yield return false;
		this.AddBehaviorScriptLines();
		yield break;
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x00070E30 File Offset: 0x0006F230
	private void AddBehaviorScriptLines()
	{
		int num = -350;
		int num2 = -220;
		bool flag = false;
		int num3 = -300;
		this.behaviorWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.behaviorWrapper);
		ThingPartState thingPartState = this.thingPart.states[this.thingPart.currentState];
		int currentStateLinesToDisplayCount = this.GetCurrentStateLinesToDisplayCount();
		int num4 = this.currentLinesPage * 3;
		int num5 = Mathf.Min(num4 + 3 - 1, currentStateLinesToDisplayCount - 1);
		int currentStateLinesPageCount = this.GetCurrentStateLinesPageCount();
		bool flag2 = false;
		if (num5 > 99)
		{
			num5 = 99;
		}
		for (int i = num4; i <= num5; i++)
		{
			base.AddModelButton("EditTextButton", "editBehaviorLine", i.ToString(), -400, num2 - 20, false);
			string text = ((i >= thingPartState.scriptLines.Count) ? "when .. then .." : thingPartState.scriptLines[i]);
			if (text != string.Empty)
			{
				string[] array = new string[2];
				char[] array2 = new char[] { ' ' };
				string[] array3 = text.Split(array2, StringSplitOptions.RemoveEmptyEntries);
				int num6 = 0;
				foreach (string text2 in array3)
				{
					if (array[num6] != null && array[num6].Length + text2.Length > 32 && num6 < array.Length - 1)
					{
						num6++;
					}
					string[] array5;
					int num7;
					(array5 = array)[num7 = num6] = array5[num7] + text2 + " ";
				}
				TextColor textColor = TextColor.Default;
				bool flag3 = this.MissingIsWord(text);
				if (flag3)
				{
					flag2 = true;
					textColor = TextColor.Red;
				}
				int num8 = num2 - 50;
				for (int k = 0; k < array.Length; k++)
				{
					if (array[k] != null)
					{
						string text3 = array[k].Trim();
						if (k == array.Length - 1)
						{
							text3 = Misc.TruncateRightAligned(text3, 32, "..");
						}
						int num9 = num8 + k * 50;
						string text4 = text3;
						int num10 = num;
						int num11 = num9;
						TextColor textColor2 = textColor;
						base.AddLabel(text4, num10, num11, 1f, false, textColor2, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
						if (text3.IndexOf(" told by any ") >= 0)
						{
							base.AddModelButton("ContextHelpButton", "showTellSources", array[k], 420, num9 + 50, false);
						}
					}
				}
			}
			if (num5 > i || i < currentStateLinesToDisplayCount - 1)
			{
				int num12 = num2 + 80;
				base.AddSeparator(0, num12, false);
				if (num12 == num3)
				{
					flag = true;
				}
			}
			num2 += 150;
		}
		if (!flag)
		{
			base.AddSeparator(0, num3, false);
		}
		if (currentStateLinesToDisplayCount > 3)
		{
			base.AddDefaultPagingButtons(80, 275, "LinesPage", false, 0, 0.85f, true);
		}
		base.SetUiWrapper(base.gameObject);
		if (flag2)
		{
			base.ShowHelpLabel("Add \"is\" when using variables, like \"When is gold >= 10 then...\" or \"...then is silver = 5\".", 50, 0.7f, TextAlignment.Left, -700, false, false, 1f, TextColor.Default);
		}
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x0007114C File Offset: 0x0006F54C
	private bool MissingIsWord(string s)
	{
		bool flag = false;
		s = s.ToLower();
		string[] array = new string[] { " write ", " type ", " say ", " show " };
		if (!Misc.ContainsAny(s, array))
		{
			string[] array2 = new string[] { ">", "<", "=", "++" };
			string[] array3 = Misc.Split(s, " then ", StringSplitOptions.RemoveEmptyEntries);
			if (array3.Length == 2)
			{
				foreach (string text in array3)
				{
					if (Misc.ContainsAny(text, array2))
					{
						string text2 = " " + text + " ";
						flag = !text2.Contains(" is ");
						if (flag)
						{
							break;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x00071232 File Offset: 0x0006F632
	private new void Update()
	{
		if (this.thingPart == null)
		{
			this.Close();
		}
		base.ReactToOnClick();
		base.ReactToOnClickInWrapper(this.behaviorWrapper);
		base.HandleBacksideEditButtons();
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x00071264 File Offset: 0x0006F664
	public void SwitchToState(int currentState)
	{
		if (this.thingPart.isLocked && currentState > this.thingPart.states.Count - 1)
		{
			Managers.soundManager.Play("no", this.transform, 1f, false, false);
			return;
		}
		if (this.thingPart.currentState != currentState)
		{
			this.currentLinesPage = 0;
		}
		this.HideStateLabels();
		if (currentState > this.thingPart.states.Count - 1)
		{
			int currentState2 = this.thingPart.currentState;
			this.thingPart.states.Add(new ThingPartState());
			this.thingPart.currentState = currentState;
			ThingPartState thingPartState = this.thingPart.states[currentState];
			thingPartState.particleSystemProperty = Managers.thingManager.CloneParticleSystemProperty(this.thingPart.states[currentState2].particleSystemProperty);
			thingPartState.particleSystemColor = this.thingPart.states[currentState2].particleSystemColor;
			thingPartState.textureProperties = Managers.thingManager.CloneTextureProperties(this.thingPart.states[currentState2].textureProperties);
			thingPartState.textureColors[0] = this.thingPart.states[currentState2].textureColors[0];
			thingPartState.textureColors[1] = this.thingPart.states[currentState2].textureColors[1];
			this.thingPart.SetStatePropertiesByTransform(false);
		}
		this.thingPart.currentState = currentState;
		this.thingPart.SetTransformPropertiesByState(false, true);
		this.UpdateStateButtons();
		global::UnityEngine.Object.Destroy(this.behaviorWrapper);
		this.AddBehaviorScriptLines();
		this.thingPart.ResetAndStopStateAsCurrentlyEditing();
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00071440 File Offset: 0x0006F840
	private int GetCurrentStateLinesToDisplayCount()
	{
		ThingPartState thingPartState = this.thingPart.states[this.thingPart.currentState];
		return thingPartState.scriptLines.Count + 1;
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x00071476 File Offset: 0x0006F876
	private int GetCurrentStateLinesPageCount()
	{
		return (int)Mathf.Ceil((float)this.GetCurrentStateLinesToDisplayCount() / 3f);
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x0007148C File Offset: 0x0006F88C
	private void ShowTellSources(string line)
	{
		if (!this.isShowingTellAnyIndicators)
		{
			int num = line.IndexOf(" told by any ");
			string text = line.Substring(num + " told by any ".Length);
			if (text.IndexOf(" then ") >= 0)
			{
				text = Misc.Split(text, " then ", StringSplitOptions.RemoveEmptyEntries)[0];
			}
			text = text.Trim().ToLower();
			this.SetTellAnyIndicatorLine(false, string.Empty);
			List<string> list = this.SetTellAnyIndicatorLine(true, text);
			string text2 = "All outside \"tell any " + text + "\" placements are now pointed to with a long upwards line in this area. ";
			if (list.Count >= 1)
			{
				string text3 = string.Join("\", \"", list.ToArray());
				text2 = text2 + "Found e.g. \"" + text3 + "\"...";
			}
			else
			{
				text2 += "None were found.";
			}
			base.ShowHelpLabel(text2, 50, 0.7f, TextAlignment.Left, -700, false, false, 1f, TextColor.Default);
		}
		else
		{
			this.SetTellAnyIndicatorLine(false, string.Empty);
			base.HideHelpLabel();
		}
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x00071590 File Offset: 0x0006F990
	private List<string> SetTellAnyIndicatorLine(bool doShow, string tellString = "")
	{
		List<string> list = new List<string>();
		this.isShowingTellAnyIndicators = doShow;
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Thing), true);
		foreach (Thing thing in componentsInChildren)
		{
			thing.temporarilyBenefitsFromShowingAtDistance = false;
		}
		Component[] componentsInChildren2 = Managers.thingManager.placements.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (ThingPart thingPart in componentsInChildren2)
		{
			if (thingPart.transform.parent.gameObject != CreationHelper.thingBeingEdited)
			{
				global::UnityEngine.Object.Destroy(thingPart.gameObject.GetComponent<LineRenderer>());
			}
		}
		if (doShow)
		{
			Component[] componentsInChildren3 = Managers.thingManager.placements.GetComponentsInChildren(typeof(ThingPart), true);
			foreach (ThingPart thingPart2 in componentsInChildren3)
			{
				if (thingPart2.transform.parent.gameObject != CreationHelper.thingBeingEdited)
				{
					foreach (ThingPartState thingPartState in thingPart2.states)
					{
						foreach (StateListener stateListener in thingPartState.listeners)
						{
							if (stateListener.tells != null)
							{
								foreach (KeyValuePair<TellType, string> keyValuePair in stateListener.tells)
								{
									if (keyValuePair.Key == TellType.Any && keyValuePair.Value == tellString && thingPart2.gameObject.GetComponent<LineRenderer>() == null)
									{
										LineRenderer lineRenderer = thingPart2.gameObject.AddComponent<LineRenderer>();
										lineRenderer.SetWidth(0.1f, 0.1f);
										lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
										lineRenderer.useWorldSpace = true;
										lineRenderer.SetPosition(0, thingPart2.transform.position);
										lineRenderer.SetPosition(1, thingPart2.transform.position + Vector3.up * 100000f);
										Thing component = thingPart2.transform.parent.GetComponent<Thing>();
										component.temporarilyBenefitsFromShowingAtDistance = true;
										component.gameObject.SetActive(true);
										if (list.Count < 5 && list.IndexOf(component.gameObject.name) == -1)
										{
											list.Add(component.gameObject.name);
										}
									}
								}
							}
						}
					}
				}
			}
		}
		if (list.Count >= 0)
		{
			Managers.soundManager.Play("whoosh", this.transform, 0.35f, false, false);
		}
		return list;
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x00071910 File Offset: 0x0006FD10
	private bool HasPlacedSubThingsEvenIfNotCurrentlyAttached()
	{
		return this.thingPart.placedSubThingIdsWithOriginalInfo != null && this.thingPart.placedSubThingIdsWithOriginalInfo.Count >= 1;
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x0007193C File Offset: 0x0006FD3C
	public void ShowStateDeleteButtonIfNeeded()
	{
		if (this.currentStateButton != null)
		{
			base.SetText(this.currentStateButton, "Delete?", 0.225f, new TextColor?(TextColor.Red), false);
			this.currentStateButton.GetComponent<DialogPart>().contextName = "deleteState";
			this.currentStateButton = null;
		}
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x00071994 File Offset: 0x0006FD94
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		Our.SetPreferentialHandSide(this.hand);
		switch (contextName)
		{
		case "editBehaviorLine":
		{
			CreationHelper.currentThingPartStateLinesPage = this.currentLinesPage;
			int lineNumber = int.Parse(contextId);
			string text3 = "when ";
			if (lineNumber < this.thingPart.states[this.thingPart.currentState].scriptLines.Count)
			{
				text3 = this.thingPart.states[this.thingPart.currentState].scriptLines[lineNumber];
			}
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (this.thingPart != null)
				{
					if (text != null)
					{
						if (lineNumber > this.thingPart.states[this.thingPart.currentState].scriptLines.Count - 1)
						{
							this.thingPart.states[this.thingPart.currentState].scriptLines.Add(string.Empty);
						}
						this.thingPart.states[this.thingPart.currentState].scriptLines[lineNumber] = text;
						if (text == string.Empty)
						{
							this.thingPart.states[this.thingPart.currentState].scriptLines.RemoveAt(lineNumber);
						}
						Thing component = CreationHelper.thingBeingEdited.GetComponent<Thing>();
						this.thingPart.states[this.thingPart.currentState].ParseScriptLinesIntoListeners(component, this.thingPart, false);
						Managers.achievementManager.RegisterAchievement(Achievement.AddedScriptLine);
						string text4 = text.Trim();
						if (text4 != string.Empty && text4 != "when")
						{
							CreationHelper.lastScriptLineEntered = text;
						}
					}
					this.SwitchTo(DialogType.ThingPart, string.Empty);
				}
			}, contextName, text3, 10000, string.Empty, true, true, false, false, 1f, false, false, null, false);
			break;
		}
		case "editCreationText":
		{
			CreationHelper.currentThingPartStateLinesPage = this.currentLinesPage;
			string text2 = this.thingPart.GetComponent<TextMesh>().text;
			if (text2 == "ABC")
			{
				text2 = string.Empty;
			}
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (this.thingPart == null)
				{
					this.SwitchTo(DialogType.Create, string.Empty);
				}
				else
				{
					if (!string.IsNullOrEmpty(text))
					{
						this.thingPart.SetOriginalText(text);
					}
					this.SwitchTo(DialogType.ThingPart, string.Empty);
				}
			}, contextName, text2, 10000, string.Empty, true, true, true, true, 1f, false, false, null, false);
			break;
		}
		case "PreviousStateButtonsPage":
			this.GoToPreviousStateButtonsPage();
			break;
		case "NextStateButtonsPage":
			this.GoToNextStateButtonsPage();
			break;
		case "state":
			this.SwitchToState(int.Parse(contextId));
			this.UpdateUndoButton();
			break;
		case "previousLinesPage":
		{
			int num = --this.currentLinesPage;
			if (num < 0)
			{
				this.currentLinesPage = this.GetCurrentStateLinesPageCount() - 1;
				if (this.currentLinesPage < 0)
				{
					this.currentLinesPage = 0;
				}
			}
			base.StartCoroutine(this.UpdateBehaviorScriptLines());
			break;
		}
		case "nextLinesPage":
		{
			int num = ++this.currentLinesPage;
			if (num >= this.GetCurrentStateLinesPageCount())
			{
				this.currentLinesPage = 0;
			}
			base.StartCoroutine(this.UpdateBehaviorScriptLines());
			break;
		}
		case "attributes":
			base.SwitchTo(DialogType.ThingPartAttributes, string.Empty);
			break;
		case "subThings":
			if (this.HasPlacedSubThingsEvenIfNotCurrentlyAttached() && !this.thingPart.HasIncludedSubThings())
			{
				base.SwitchTo(DialogType.PlacedSubThings, string.Empty);
			}
			else
			{
				base.SwitchTo(DialogType.IncludedSubThings, string.Empty);
			}
			break;
		case "toggleLock":
			if (this.thingPart != null)
			{
				this.thingPart.isLocked = state;
				if (state)
				{
					Managers.soundManager.Play("success", this.transform, 0.1f, false, false);
				}
				this.UpdateSubThingsButton();
			}
			break;
		case "undo":
			this.thingPart.Undo();
			this.UpdateUndoButton();
			break;
		case "showTellSources":
			this.ShowTellSources(contextId);
			break;
		case "copyPaste":
			base.SwitchTo(DialogType.ThingPartCopyPaste, string.Empty);
			break;
		case "deleteState":
			if (this.thingPart != null)
			{
				this.thingPart.DeleteCurrentStateDuringEditing();
				Managers.soundManager.Play("delete", this.transform, 0.3f, false, false);
				base.SwitchTo(DialogType.ThingPart, string.Empty);
			}
			break;
		case "close":
			this.Close();
			break;
		}
	}

	// Token: 0x06000C77 RID: 3191 RVA: 0x00071DD8 File Offset: 0x000701D8
	private void Close()
	{
		if (this.isShowingTellAnyIndicators)
		{
			this.SetTellAnyIndicatorLine(false, string.Empty);
		}
		CreationHelper.currentThingPartStateLinesPage = 0;
		CreationHelper.thingPartWhoseStatesAreEdited = null;
		base.SwitchTo((!CreateDialog.wasLastMinimized) ? DialogType.Create : DialogType.Start, string.Empty);
	}

	// Token: 0x06000C78 RID: 3192 RVA: 0x00071E28 File Offset: 0x00070228
	public override void RecreateInterfaceAfterSettingsChangeIfNeeded()
	{
		global::UnityEngine.Object.Destroy(this.backsideWrapper);
		base.AddBacksideEditButtons();
	}

	// Token: 0x04000955 RID: 2389
	public Shader dynamicTexture;

	// Token: 0x04000956 RID: 2390
	private ThingPart thingPart;

	// Token: 0x04000957 RID: 2391
	private GameObject[] stateButtons = new GameObject[50];

	// Token: 0x04000958 RID: 2392
	private TextMesh[] stateLabels = new TextMesh[2];

	// Token: 0x04000959 RID: 2393
	private GameObject behaviorWrapper;

	// Token: 0x0400095A RID: 2394
	private int stateButtonsOffset;

	// Token: 0x0400095B RID: 2395
	private GameObject stateButtonsBack;

	// Token: 0x0400095C RID: 2396
	private GameObject stateButtonsForward;

	// Token: 0x0400095D RID: 2397
	private GameObject undoButton;

	// Token: 0x0400095E RID: 2398
	private const int maxStateButtonsToDisplayAtOnce = 4;

	// Token: 0x0400095F RID: 2399
	private GameObject textureButton;

	// Token: 0x04000960 RID: 2400
	private bool showSubThings;

	// Token: 0x04000961 RID: 2401
	private GameObject subThingsButton;

	// Token: 0x04000962 RID: 2402
	private int currentLinesPage;

	// Token: 0x04000963 RID: 2403
	private const int linesPerPage = 3;

	// Token: 0x04000964 RID: 2404
	private bool isShowingTellAnyIndicators;

	// Token: 0x04000965 RID: 2405
	private GameObject copyPasteButton;

	// Token: 0x04000966 RID: 2406
	private const int miniButtonsY = 420;

	// Token: 0x04000967 RID: 2407
	private GameObject currentStateButton;
}
