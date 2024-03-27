using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000FF RID: 255
public class AreaDialog : Dialog
{
	// Token: 0x060008EE RID: 2286 RVA: 0x00034E7C File Offset: 0x0003327C
	public void Start()
	{
		this.environmentChangers = Managers.treeManager.GetObject("/Universe/EnvironmentChangers");
		this.areaId = ((Our.areaIdOfInterest == null) ? Managers.areaManager.currentAreaId : Our.areaIdOfInterest);
		GameObject @object = Managers.treeManager.GetObject("/Universe/EnvironmentManager");
		this.environmentManager = @object.GetComponent<EnvironmentManager>();
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddCloseButton();
		base.AddBackButton();
		Managers.areaManager.GetInfo(delegate(AreaInfo areaInfo)
		{
			if (this == null)
			{
				return;
			}
			this.editors = areaInfo.editors;
			this.copiedFromAreas = areaInfo.copiedFromAreas;
			this.description = areaInfo.description;
			this.AddDescription(this.description);
			this.UpdateEditorsDisplay();
			base.AddLabel("all-time visitors: " + areaInfo.totalVisitors, -180, 90, 0.85f, false, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			base.AddLabel("Days: " + Misc.GetHowManyDaysAgo(areaInfo.creationDate), 430, -430, 1f, true, TextColor.Default, false, TextAlignment.Left, -1, 1f, true, TextAnchor.MiddleLeft);
			this.renamesLeft = ((!Managers.areaManager.weAreOwnerOfCurrentArea) ? 0 : (10 - areaInfo.renameCount));
			if (this.renamesLeft >= 1)
			{
				base.AddModelButton("EditTextButton", "editAreaName", null, -320, -445, false);
				base.AddFlexibleSizeHeadline(Managers.areaManager.currentAreaName, -270, -465, TextColor.Default, 16);
			}
			else
			{
				base.AddFlexibleSizeHeadline(Managers.areaManager.currentAreaName, -350, -465, TextColor.Default, 16);
			}
			if (Managers.areaManager.weAreEditorOfCurrentArea)
			{
				GameObject gameObject3 = base.AddButton("subAreas", null, null, "ButtonSmall", 387, 230, "subConnections", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				base.MinifyButton(gameObject3, 1f, 1f, 0.55f, false);
			}
			this.isFavorite = areaInfo.isFavorited;
			this.UpdateFavoriteButton();
			if (this.copiedFromAreas.Count > 0 && !this.AllPastesWereFromOwnedToOwned(this.copiedFromAreas, this.editors[0].id))
			{
				base.AddButton("areaCopyCreditsDialog", null, "with pastes from...", "ButtonCompactNoIcon", 0, 0, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
		});
		if (Managers.areaManager.weAreEditorOfCurrentArea)
		{
			this.AddEnvironmentButtons();
			Our.SetMode(EditModes.Environment, false);
			base.AddBrush();
			bool flag = this.SetEnvironmentChangersEnabled(true);
			if (flag)
			{
				this.UpdateEnvironmentChangersPosition();
			}
			GameObject gameObject = base.AddButton("attributes", null, null, "ButtonSmall", -387, 230, "attributes", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			base.MinifyButton(gameObject, 1f, 1f, 0.55f, false);
		}
		this.AddBacksideButtons();
		if (Managers.areaManager.weAreOwnerOfCurrentArea || Managers.areaManager.isCopyable)
		{
			GameObject gameObject2 = base.AddButton("areaPlacementsDialog", null, null, "ButtonSmall", 0, 230, "copyPasteArea", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			base.MinifyButton(gameObject2, 1f, 1f, 0.55f, false);
		}
		this.UpdateAreaThingStatsOnBackside();
		Managers.achievementManager.RegisterAchievement(Achievement.OpenedAreaDialog);
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00035030 File Offset: 0x00033430
	private void AddBacksideButtons()
	{
		if (Managers.areaManager.weAreOwnerOfCurrentArea)
		{
			this.DoAddBacksideButtons();
		}
		else
		{
			Managers.areaManager.GetAreaFlagStatus(this.areaId, delegate(bool _isFlagged)
			{
				if (this == null)
				{
					return;
				}
				this.isFlagged = _isFlagged;
				this.DoAddBacksideButtons();
			});
		}
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x00035068 File Offset: 0x00033468
	private void DoAddBacksideButtons()
	{
		this.backsideWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.backsideWrapper);
		if (Managers.areaManager.weAreOwnerOfCurrentArea || Managers.areaManager.isCopyable)
		{
			this.exportButton = base.AddButton("export", null, string.Empty, "ButtonSmall", 0, 390, "export", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		if (!Managers.areaManager.weAreOwnerOfCurrentArea)
		{
			this.flagButton = base.AddFlag("flagArea", this.isFlagged, "flagArea", false, -410, 420);
		}
		base.AddButton("currentAndRecentPeople", null, string.Empty, "ButtonSmall", 390, 390, "people", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		if (Managers.areaManager.weAreEditorOfCurrentArea || Managers.areaManager.rights.highlighting == true)
		{
			base.AddButton("highlightThings", null, "Highlight", "ButtonCompactNoIconShortCentered", -310, 0, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, true, TextAlignment.Left, false, false);
		}
		base.RotateBacksideWrapper();
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x000351F8 File Offset: 0x000335F8
	private bool AllPastesWereFromOwnedToOwned(List<AreaIdNameAndCreatorId> copiedFromAreas, string currentAreaOwnerId)
	{
		bool flag = true;
		foreach (AreaIdNameAndCreatorId areaIdNameAndCreatorId in copiedFromAreas)
		{
			if (areaIdNameAndCreatorId.creatorId != currentAreaOwnerId)
			{
				flag = false;
				break;
			}
		}
		return flag;
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x00035264 File Offset: 0x00033664
	private void UpdateFavoriteButton()
	{
		global::UnityEngine.Object.Destroy(this.favoriteButton);
		this.favoriteButton = base.AddModelButton("Favorite", "toggleFavorite", null, 310, -425, false);
		this.favoriteButton.GetComponent<DialogPart>().autoStopHighlight = false;
		base.ApplyEmissionColorToShape(this.favoriteButton, this.isFavorite);
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x000352C4 File Offset: 0x000336C4
	private void UpdateEditorsDisplay()
	{
		if (this.editors != null && this.editors.Count >= 1)
		{
			this.editorsWrapper = base.GetUiWrapper();
			base.SetUiWrapper(this.editorsWrapper);
			int num = 0;
			int num2 = 0;
			int num3 = Mathf.CeilToInt((float)this.editors.Count / 2f);
			if (this.editorsPage > num3 - 1)
			{
				this.editorsPage = 0;
			}
			int num4 = this.editorsPage * 2;
			int num5 = Mathf.Min(num4 + 2 - 1, this.editors.Count - 1);
			if (this.editors.Count == 1 || num3 <= 1)
			{
				base.AddLabel("editors", 0, -210, 0.85f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
			}
			else
			{
				base.AddLabel("editors", -145, -210, 0.85f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			}
			if (num3 >= 2)
			{
				GameObject gameObject = base.AddModelButton("ButtonForward", "nextPage", null, 80, -190, false);
				gameObject.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
			}
			for (int i = num4; i <= num5; i++)
			{
				EditorInfo editorInfo = this.editors[i];
				string text = ((!(Managers.personManager.ourPerson.userId == editorInfo.id)) ? editorInfo.name : "me");
				text = Misc.Truncate(text, 22, true);
				int num6 = -225 + num * 450;
				int num7 = -105 + num2 * 75;
				if (this.editors.Count == 1)
				{
					num6 = 0;
				}
				base.AddButton("userInfo", editorInfo.id, text, "ButtonCompactNoIcon", num6, num7, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				if (++num >= 2)
				{
					num = 0;
					num2++;
				}
			}
			base.SetUiWrapper(base.gameObject);
		}
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x000354F0 File Offset: 0x000338F0
	public void UpdateAreaThingStatsOnBackside()
	{
		Managers.thingManager.UpdateStats();
		int percentInt = Misc.GetPercentInt(10000, Managers.thingManager.statsThingsInArea);
		string text = Managers.thingManager.statsThingsInArea.ToString();
		if (percentInt >= 75)
		{
			string text2 = text;
			text = string.Concat(new object[] { text2, " (", percentInt, "%)" });
		}
		string text3 = string.Concat(new object[]
		{
			text,
			" Things in area",
			Environment.NewLine,
			Environment.NewLine,
			Managers.thingManager.statsThingsAroundPosition,
			" Things around here",
			Environment.NewLine,
			Managers.thingManager.statsThingPartsAroundPosition,
			" Thing Parts around here"
		});
		if (this.statsLabel == null)
		{
			this.statsLabel = base.AddLabel(string.Empty, 430, -300, 0.9f, true, TextColor.Default, false, TextAlignment.Left, -1, 1f, true, TextAnchor.MiddleLeft);
		}
		this.statsLabel.text = text3.ToUpper();
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x00035618 File Offset: 0x00033A18
	private void AddDescription(string description)
	{
		float num = 0.75f;
		if (Managers.areaManager.weAreEditorOfCurrentArea)
		{
			if (string.IsNullOrEmpty(description))
			{
				description = "description";
				num = 0.85f;
			}
			base.AddModelButton("EditTextButton", "editAreaDescription", null, -400, -340, false);
		}
		int num2 = ((!Managers.areaManager.weAreEditorOfCurrentArea) ? (-450) : (-350));
		if (string.IsNullOrEmpty(description))
		{
			description = string.Empty;
		}
		string text = Misc.WrapWithNewlines(description, 45, 2);
		base.AddLabel(text, num2, -350, num, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x000356C8 File Offset: 0x00033AC8
	private void AddEnvironmentButtons()
	{
		int num = -385;
		int num2 = 380;
		int num3 = 0;
		IEnumerator enumerator = Enum.GetValues(typeof(EnvironmentType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				EnvironmentType environmentType = (EnvironmentType)obj;
				string text = environmentType.ToString();
				bool flag = environmentType == this.environmentManager.type;
				this.environmentCheckboxes[num3] = base.AddButton("environment", text, null, "ButtonSmall", num, num2, Misc.GetNumberlessString(text), flag, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				num += 130;
				num3++;
				if (num3 >= this.environmentCheckboxes.Length)
				{
					break;
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
		base.AddModelButton("ButtonForward", "toEnvironmentDialog", null, 410, 385, false);
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x000357E4 File Offset: 0x00033BE4
	private bool SetEnvironmentChangersEnabled(bool enabledState)
	{
		bool flag = false;
		if (base.gameObject != null && this.environmentChangers != null)
		{
			IEnumerator enumerator = this.environmentChangers.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.gameObject.CompareTag("EnvironmentChanger"))
					{
						Renderer component = transform.gameObject.GetComponent<Renderer>();
						if (!component.enabled && enabledState)
						{
							flag = true;
						}
						component.enabled = enabledState;
						global::UnityEngine.Object.Destroy(transform.gameObject.GetComponent<MeshCollider>());
						MeshCollider meshCollider = transform.gameObject.AddComponent<MeshCollider>();
						meshCollider.convex = true;
						meshCollider.enabled = enabledState;
						if (enabledState && flag)
						{
							transform.localPosition = Vector3.zero;
							string name = transform.gameObject.name;
							if (name != null)
							{
								if (!(name == "sun"))
								{
									if (!(name == "night"))
									{
										if (!(name == "ambientLight"))
										{
											if (!(name == "fog"))
											{
												if (name == "clouds")
												{
													transform.position += Vector3.up * -0.25f;
												}
											}
											else
											{
												transform.position += Vector3.up * -0.175f;
											}
										}
										else
										{
											transform.position += Vector3.up * -0.075f;
										}
									}
									else
									{
										transform.position += Vector3.up * -0.025f;
									}
								}
								else
								{
									transform.position += Vector3.up * 0.095f;
								}
							}
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
		}
		return flag;
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x00035A20 File Offset: 0x00033E20
	public void UpdateEnvironmentChangersPosition()
	{
		GameObject @object = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
		Side side = ((!(this.transform.parent.name == "HandCoreLeft")) ? Side.Right : Side.Left);
		float num = 0.25f;
		if (side == Side.Right)
		{
			num *= -1f;
		}
		this.environmentChangers.transform.position = this.transform.position + @object.transform.right * num;
	}

	// Token: 0x060008F9 RID: 2297 RVA: 0x00035AAA File Offset: 0x00033EAA
	private new void Update()
	{
		if (this.doUpdateEditorsNextFrame)
		{
			this.doUpdateEditorsNextFrame = false;
			this.UpdateEditorsDisplay();
		}
		this.HandleAreaNameCopying();
		base.ReactToOnClick();
		base.ReactToOnClickInWrapper(this.backsideWrapper);
		base.ReactToOnClickInWrapper(this.editorsWrapper);
	}

	// Token: 0x060008FA RID: 2298 RVA: 0x00035AE8 File Offset: 0x00033EE8
	private void HandleAreaNameCopying()
	{
		if (Misc.CtrlIsPressed() && Input.GetKeyDown(KeyCode.C))
		{
			GUIUtility.systemCopyBuffer = "[" + Managers.areaManager.currentAreaName.ToLower() + "]";
			Managers.soundManager.Play("pickUp", this.transform, 0.5f, false, false);
		}
	}

	// Token: 0x060008FB RID: 2299 RVA: 0x00035B4A File Offset: 0x00033F4A
	private void GoToNextEditorsPage()
	{
		global::UnityEngine.Object.Destroy(this.editorsWrapper);
		this.editorsPage++;
		this.doUpdateEditorsNextFrame = true;
	}

	// Token: 0x060008FC RID: 2300 RVA: 0x00035B6C File Offset: 0x00033F6C
	private void OnDestroy()
	{
		if (this.hand.dialogTypeToBeOpened != DialogType.Material)
		{
			if (Managers.areaManager.weAreEditorOfCurrentArea)
			{
				this.SetEnvironmentChangersEnabled(false);
			}
			if (Our.mode == EditModes.Environment)
			{
				Our.SetPreviousMode();
			}
		}
	}

	// Token: 0x060008FD RID: 2301 RVA: 0x00035BA8 File Offset: 0x00033FA8
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "flagArea":
			if (!this.flagIsBeingToggled)
			{
				this.flagIsBeingToggled = true;
				if (this.isFlagged)
				{
					Managers.areaManager.ToggleAreaFlag(this.areaId, null, delegate(bool _isFlagged)
					{
						this.isFlagged = _isFlagged;
						this.UpdateFlag(this.isFlagged, false, string.Empty);
						this.flagIsBeingToggled = false;
					});
				}
				else
				{
					Managers.dialogManager.GetInput(delegate(string text)
					{
						if (text == string.Empty)
						{
							Managers.soundManager.Play("no", this.transform, 1f, false, false);
						}
						if (!string.IsNullOrEmpty(text))
						{
							string currentAreaId = Managers.areaManager.currentAreaId;
							Managers.areaManager.ToggleAreaFlag(currentAreaId, text, delegate(bool isFlagged)
							{
								this.SwitchTo(DialogType.Area, string.Empty);
							});
						}
						else
						{
							this.SwitchTo(DialogType.Area, string.Empty);
						}
					}, contextName, string.Empty, 200, "reason for reporting", true, false, false, false, 1f, false, false, null, true);
				}
			}
			break;
		case "environment":
		{
			int num2 = 0;
			IEnumerator enumerator = Enum.GetValues(typeof(EnvironmentType)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					EnvironmentType environmentType = (EnvironmentType)obj;
					if (num2 < this.environmentCheckboxes.Length)
					{
						bool flag = contextId == environmentType.ToString();
						base.SetCheckboxState(this.environmentCheckboxes[num2], flag, true);
						if (flag)
						{
							this.environmentManager.SetToType(environmentType);
							Managers.personManager.DoSetEnvironmentType(environmentType);
						}
					}
					num2++;
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
			break;
		}
		case "editAreaDescription":
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (text != null)
				{
					Managers.areaManager.SetDescription(text, new Action<bool>(this.hand.SwitchToNewDialog_AreaDialog));
				}
				else
				{
					this.SwitchTo(DialogType.Area, string.Empty);
				}
			}, contextName, this.description, 120, string.Empty, true, true, false, false, 1f, false, false, null, false);
			break;
		case "editAreaName":
			Managers.dialogManager.GetInput(delegate(string text)
			{
				if (!string.IsNullOrEmpty(text))
				{
					string text2;
					if (!Validator.AreaNameIsValid(text, out text2))
					{
						Managers.dialogManager.ShowInfo(text2, false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
					}
					else
					{
						Managers.areaManager.RenameArea(Managers.areaManager.currentAreaId, text, delegate(bool renamedOk, string reasonFailed)
						{
							if (renamedOk)
							{
								Managers.broadcastNetworkManager.RaiseEvent_ReloadArea();
							}
							else if (!string.IsNullOrEmpty(reasonFailed))
							{
								Managers.dialogManager.ShowInfo("Oops, " + reasonFailed, false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
							}
						});
					}
				}
				else
				{
					this.SwitchTo(DialogType.Area, string.Empty);
				}
			}, contextName, string.Empty, 60, string.Concat(new object[]
			{
				"you can rename this area ",
				this.renamesLeft,
				" more ",
				Misc.GetPluralOrSingular("time", (float)this.renamesLeft)
			}), false, false, false, false, 1f, false, false, null, false);
			break;
		case "nextPage":
			this.GoToNextEditorsPage();
			break;
		case "userInfo":
			Our.personIdOfInterest = contextId;
			this.hand.lastContextInfoHit = null;
			base.SwitchTo(DialogType.Profile, string.Empty);
			break;
		case "toggleFavorite":
			if (!this.waitingForCallback)
			{
				this.waitingForCallback = true;
				this.isFavorite = !this.isFavorite;
				Managers.areaManager.SetFavoriteArea(Managers.areaManager.currentAreaId, this.isFavorite, delegate(bool isOk)
				{
					this.waitingForCallback = false;
					if (isOk)
					{
						this.UpdateFavoriteButton();
						string text3 = ((!this.isFavorite) ? "pickUp" : "success");
						Managers.soundManager.Play(text3, this.transform, 0.2f, false, false);
					}
					else
					{
						this.isFavorite = !this.isFavorite;
						Managers.soundManager.Play("no", this.transform, 0.4f, false, false);
					}
				});
			}
			break;
		case "toEnvironmentDialog":
			base.SwitchTo(DialogType.Environment, string.Empty);
			break;
		case "attributes":
			base.SwitchTo(DialogType.AreaAttributes, string.Empty);
			break;
		case "subAreas":
			base.SwitchTo(DialogType.SubAreas, string.Empty);
			break;
		case "areaPlacementsDialog":
			base.SwitchTo(DialogType.AreaPlacements, string.Empty);
			break;
		case "areaCopyCreditsDialog":
			base.SwitchTo(DialogType.AreaCopyCredits, string.Empty);
			break;
		case "currentAndRecentPeople":
			base.SwitchTo(DialogType.CurrentAndRecentPeople, string.Empty);
			break;
		case "highlightThings":
			base.SwitchTo(DialogType.HighlightThings, string.Empty);
			break;
		case "export":
		{
			global::UnityEngine.Object.Destroy(this.exportButton);
			global::UnityEngine.Object.Destroy(this.flagButton);
			base.HideHelpLabel();
			TextMesh label = base.AddLabel("Exporting...", -445, 370, 0.55f, false, TextColor.Green, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			Managers.areaManager.GetThingIdsInAreaByOtherCreators(delegate(bool isOk, List<string> thingIds)
			{
				if (isOk)
				{
					string text4 = Managers.thingManager.ExportAllThings(thingIds);
					label.text = string.Concat(new string[]
					{
						"✔  Exported own or clonable creations to",
						Environment.NewLine,
						text4,
						Environment.NewLine,
						"Also see: anyland.com/info/area-format.html"
					});
					Managers.soundManager.Play("success", this.transform, 0.2f, false, false);
					if (!AreaDialog.didExportThisSession)
					{
						AreaDialog.didExportThisSession = true;
						Misc.OpenWindowsExplorerAtPath(text4);
					}
				}
				else
				{
					label.text = "Oops, something went wrong.";
				}
			});
			break;
		}
		case "back":
			Our.areaIdOfInterest = null;
			base.SwitchTo(DialogType.Main, string.Empty);
			break;
		case "close":
			Our.areaIdOfInterest = null;
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x040006AB RID: 1707
	private GameObject environmentChangers;

	// Token: 0x040006AC RID: 1708
	private EnvironmentManager environmentManager;

	// Token: 0x040006AD RID: 1709
	private GameObject[] environmentCheckboxes = new GameObject[6];

	// Token: 0x040006AE RID: 1710
	private TextMesh statsLabel;

	// Token: 0x040006AF RID: 1711
	private bool isFlagged;

	// Token: 0x040006B0 RID: 1712
	private string areaId;

	// Token: 0x040006B1 RID: 1713
	private string description;

	// Token: 0x040006B2 RID: 1714
	private static bool didExportThisSession;

	// Token: 0x040006B3 RID: 1715
	private GameObject favoriteButton;

	// Token: 0x040006B4 RID: 1716
	private GameObject exportButton;

	// Token: 0x040006B5 RID: 1717
	private GameObject flagButton;

	// Token: 0x040006B6 RID: 1718
	private bool isFavorite;

	// Token: 0x040006B7 RID: 1719
	private bool waitingForCallback;

	// Token: 0x040006B8 RID: 1720
	private List<EditorInfo> editors;

	// Token: 0x040006B9 RID: 1721
	private int editorsPage;

	// Token: 0x040006BA RID: 1722
	private GameObject editorsWrapper;

	// Token: 0x040006BB RID: 1723
	private bool doUpdateEditorsNextFrame;

	// Token: 0x040006BC RID: 1724
	private List<AreaIdNameAndCreatorId> copiedFromAreas;

	// Token: 0x040006BD RID: 1725
	private const int miniButtonsY = 230;

	// Token: 0x040006BE RID: 1726
	private int renamesLeft;
}
