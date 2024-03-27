using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000103 RID: 259
public class AreasDialog : Dialog
{
	// Token: 0x06000928 RID: 2344 RVA: 0x00037CC8 File Offset: 0x000360C8
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		this.showSuggestedAreasDebugInfo = false;
		if (Our.lastSuggestedAreasPage != -1)
		{
			this.suggestedAreasPage = Our.lastSuggestedAreasPage;
			Our.lastSuggestedAreasPage = -1;
		}
		base.AddGenericFindButton("findAreas");
		if (this.areaListSetCache != null)
		{
			this.SetDataByAreaListSet(this.areaListSetCache);
			this.areaListSetCache = null;
		}
		else
		{
			Managers.areaManager.GetAreaLists(delegate(AreaListSet areaListSet)
			{
				if (this == null)
				{
					return;
				}
				this.SetDataByAreaListSet(areaListSet);
			});
		}
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x00037D60 File Offset: 0x00036160
	private void SetDataByAreaListSet(AreaListSet areaListSet)
	{
		this.receivedAreaListSet = areaListSet;
		int num = areaListSet.totalOnline;
		if (num <= 0)
		{
			num = 1;
		}
		base.AddLabel("people in universe now", 400, -400, 1f, true, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddLabel(num.ToString(), 400, -340, 2f, true, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		int num2 = 42;
		if (num % num2 == 0)
		{
			base.AddLabel("DON'T PANIC", 400, -80, 2.2f, true, TextColor.Gold, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		this.totalAreas = areaListSet.totalAreas;
		this.totalPublicAreas = areaListSet.totalPublicAreas;
		this.totalSearchablePublicAreas = areaListSet.totalSearchablePublicAreas;
		base.AddLabel("areas in universe", 400, 270, 1f, true, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddLabel(this.totalAreas.ToString(), 400, 330, 2f, true, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		base.AddLabel(this.totalPublicAreas + " public", 100, 367, 1f, true, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		this.SetSuggestedAreasMixFromDataSets(areaListSet);
		this.visitedAreas = areaListSet.visited;
		this.createdAreas = areaListSet.created;
		this.favorites = areaListSet.favorite;
		this.MoveAreaWithMostPeopleOnlineToOrNearStart(this.createdAreas, 1, false);
		this.RemoveCurrentAreaFromListIfEnoughOthers(this.createdAreas);
		this.UpdateAreaDisplay();
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x00037F08 File Offset: 0x00036308
	private void SetSuggestedAreasMixFromDataSets(AreaListSet areaListSet)
	{
		this.suggestedAreas = new List<AreaOverview>();
		AreaResultType[] array = new AreaResultType[]
		{
			AreaResultType.popular,
			AreaResultType.popularRandom,
			AreaResultType.lively,
			AreaResultType.lively,
			AreaResultType.lively,
			AreaResultType.lively,
			AreaResultType.mostFavorited,
			AreaResultType.mostFavorited,
			AreaResultType.popularNewRandom,
			AreaResultType.popularRandom,
			AreaResultType.popular,
			AreaResultType.popularRandom,
			AreaResultType.mostFavorited,
			AreaResultType.popularRandom,
			AreaResultType.popularNew,
			AreaResultType.random,
			AreaResultType.quests,
			AreaResultType.popular,
			AreaResultType.newest,
			AreaResultType.lively,
			AreaResultType.lively,
			AreaResultType.mostFavorited,
			AreaResultType.mostFavorited,
			AreaResultType.mostFavorited,
			AreaResultType.mostFavorited,
			AreaResultType.mostFavorited,
			AreaResultType.popular,
			AreaResultType.popular,
			AreaResultType.popularNew,
			AreaResultType.popularNew,
			AreaResultType.mostFavorited,
			AreaResultType.mostFavorited,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.lively,
			AreaResultType.lively,
			AreaResultType.newest,
			AreaResultType.popular,
			AreaResultType.popularNew,
			AreaResultType.popularNew,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.lively,
			AreaResultType.lively,
			AreaResultType.newest,
			AreaResultType.popular,
			AreaResultType.popularNew,
			AreaResultType.popularNew,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.popular,
			AreaResultType.popularNew,
			AreaResultType.popularNew,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.popularRandom,
			AreaResultType.searches,
			AreaResultType.searchAll
		};
		foreach (AreaResultType areaResultType in array)
		{
			AreaOverview areaOverview = null;
			switch (areaResultType)
			{
			case AreaResultType.popular:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.popular, this.suggestedAreas);
				break;
			case AreaResultType.popularRandom:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.popular_rnd, this.suggestedAreas);
				break;
			case AreaResultType.newest:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.newest, this.suggestedAreas);
				break;
			case AreaResultType.popularNew:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.popularNew, this.suggestedAreas);
				break;
			case AreaResultType.popularNewRandom:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.popularNew_rnd, this.suggestedAreas);
				break;
			case AreaResultType.revisited:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.revisited, this.suggestedAreas);
				break;
			case AreaResultType.revisitedRandom:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.revisited_rnd, this.suggestedAreas);
				break;
			case AreaResultType.lively:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.lively, this.suggestedAreas);
				break;
			case AreaResultType.random:
				areaOverview = new AreaOverview();
				areaOverview.id = "[random]";
				areaOverview.name = "random";
				areaOverview.playerCount = 0;
				break;
			case AreaResultType.searches:
				this.AddSuggestedSearches(this.suggestedAreas);
				break;
			case AreaResultType.searchAll:
				areaOverview = new AreaOverview();
				areaOverview.id = "[searchAll]";
				areaOverview.name = string.Empty;
				areaOverview.playerCount = 0;
				break;
			case AreaResultType.mostFavorited:
				areaOverview = Managers.areaManager.GetAndRemoveFirstDistinctAreaOverview(areaListSet.mostFavorited, this.suggestedAreas);
				break;
			case AreaResultType.quests:
				areaOverview = new AreaOverview();
				areaOverview.id = "[quests]";
				areaOverview.name = "quests";
				areaOverview.playerCount = 0;
				break;
			}
			if (areaOverview != null)
			{
				if (this.showSuggestedAreasDebugInfo)
				{
					string text = areaResultType.ToString();
					if (text != "popular")
					{
						text = text.Replace("popular", "pop-");
					}
					text = Misc.Truncate(text, 7, false);
					areaOverview.name = "[" + text + "] " + areaOverview.name;
				}
				this.suggestedAreas.Add(areaOverview);
			}
		}
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x000381A8 File Offset: 0x000365A8
	private void AddSubAreasDialogLink(List<AreaOverview> areas)
	{
		if (areas.Count >= 1 && Managers.achievementManager.DidAchieve(Achievement.DidSetSubArea) && Managers.areaManager.weAreEditorOfCurrentArea)
		{
			AreaOverview areaOverview = new AreaOverview();
			areaOverview.id = "[sub-areas]";
			areaOverview.name = "sub-areas";
			areaOverview.playerCount = 0;
			areas.Insert(areas.Count, areaOverview);
		}
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00038214 File Offset: 0x00036614
	private void MoveAreaWithMostPeopleOnlineToOrNearStart(List<AreaOverview> list, int minPeople = 1, bool insertAtSecondPlace = false)
	{
		int indexOfAreaWithMostPeopleOnline = this.GetIndexOfAreaWithMostPeopleOnline(list);
		if (indexOfAreaWithMostPeopleOnline >= 1 && list[indexOfAreaWithMostPeopleOnline].playerCount >= minPeople)
		{
			this.MoveItemToStartOfList(list, indexOfAreaWithMostPeopleOnline);
			if (insertAtSecondPlace)
			{
				this.MoveItemToStartOfList(list, 1);
			}
		}
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00038258 File Offset: 0x00036658
	private void SortAreasAlphabetically(List<AreaOverview> list)
	{
		list.Sort((AreaOverview overview1, AreaOverview overview2) => overview1.name.CompareTo(overview2.name));
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00038280 File Offset: 0x00036680
	private int GetIndexOfAreaWithMostPeopleOnline(List<AreaOverview> list)
	{
		int num = -1;
		for (int i = 0; i < list.Count; i++)
		{
			if (num == -1 || list[i].playerCount > list[num].playerCount)
			{
				num = i;
			}
		}
		return num;
	}

	// Token: 0x0600092F RID: 2351 RVA: 0x000382D0 File Offset: 0x000366D0
	private void MoveItemToStartOfList(List<AreaOverview> list, int indexOfItemToMove)
	{
		AreaOverview areaOverview = list[indexOfItemToMove];
		for (int i = indexOfItemToMove; i > 0; i--)
		{
			list[i] = list[i - 1];
		}
		list[0] = areaOverview;
	}

	// Token: 0x06000930 RID: 2352 RVA: 0x00038310 File Offset: 0x00036710
	private void AddSuggestedSearches(List<AreaOverview> areas)
	{
		string[] array = new string[]
		{
			"hub", "chat", "help", "art", "friends", "game", "adventure", "heads", "items", "party",
			"science", "fun", "welcome", "philosophy", "sports", "arena", "movies", "code", "horror", "quest",
			"meet", "language", "fashion", "space", "medieval", "music", "stage", "home", "relax", "vacation",
			"nature", "food", "copyable"
		};
		Array.Sort<string>(array);
		foreach (string text in array)
		{
			areas.Add(new AreaOverview
			{
				name = text
			});
		}
	}

	// Token: 0x06000931 RID: 2353 RVA: 0x0003847C File Offset: 0x0003687C
	private void RemoveCurrentAreaFromListIfEnoughOthers(List<AreaOverview> areas)
	{
		if (areas.Count >= 2)
		{
			for (int i = areas.Count - 1; i >= 0; i--)
			{
				if (areas[i].name == Managers.areaManager.currentAreaName)
				{
					areas.RemoveAt(i);
				}
			}
		}
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x000384D8 File Offset: 0x000368D8
	private void UpdateAreaDisplay()
	{
		if (this.areasWrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.areasWrapper);
		}
		this.areasWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.areasWrapper);
		string text = this.fullViewSectionName;
		if (text != null)
		{
			if (text == "visited")
			{
				this.UpdateAreaDisplayPage("visited", this.visitedAreas, ref this.visitedAreasPage, -420, 20, false);
				goto IL_24B;
			}
			if (text == "created")
			{
				if (!this.sortedCreatedAreas)
				{
					this.sortedCreatedAreas = true;
					this.SortAreasAlphabetically(this.createdAreas);
					this.AddSubAreasDialogLink(this.createdAreas);
				}
				this.UpdateAreaDisplayPage("created", this.createdAreas, ref this.createdAreasPage, -420, 16, false);
				base.AddSeparator(0, 340, false);
				if (Universe.features.createArea)
				{
					base.AddButton("createArea", null, "create area...", "ButtonCompact", 0, 410, "createArea", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
				goto IL_24B;
			}
			if (text == "favorites")
			{
				this.UpdateAreaDisplayPage("favorites", this.favorites, ref this.favoritesPage, -420, 20, false);
				goto IL_24B;
			}
		}
		this.UpdateAreaDisplayPage("suggested", this.suggestedAreas, ref this.suggestedAreasPage, -420, 10, true);
		if (Universe.features.visitedAreasList)
		{
			this.UpdateAreaDisplayPage("visited", this.visitedAreas, ref this.visitedAreasPage, 95, 2, false);
		}
		if (Universe.features.createdAreasList)
		{
			this.UpdateAreaDisplayPage("created", this.createdAreas, ref this.createdAreasPage, 305, 2, false);
		}
		if (this.createdAreas.Count <= 1 && Universe.features.createArea)
		{
			base.AddButton("createArea", null, "create area...", "ButtonCompact", 225, 410, "createArea", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		}
		IL_24B:
		if (this.fullViewSectionName != "favorites" && this.favorites != null && this.favorites.Count >= 1 && Universe.features.favoritedAreasList)
		{
			base.AddModelButton("Favorite", "showSectionInFullView", "favorites", -320, -422, false);
		}
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x000387A0 File Offset: 0x00036BA0
	private void UpdateAreaDisplayPage(string name, List<AreaOverview> areas, ref int page, int y, int maxPerPage, bool isMainHeading = false)
	{
		if (isMainHeading)
		{
			base.AddLabel("areas", -150, y - 10, 1.2f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		else
		{
			base.AddLabel(name, -145, y, 0.85f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		bool flag = areas != null && areas.Count >= 1;
		bool flag2 = this.fullViewSectionName == string.Empty && (name == "created" || name == "visited");
		int num = y + 20;
		Vector3 vector = new Vector3(0.5f, 1f, 0.5f);
		if (flag2)
		{
			GameObject gameObject = base.AddModelButton("ButtonForward", "showSectionInFullView", name, 80, num, false);
			gameObject.transform.localScale = vector;
		}
		if (flag)
		{
			int num2 = 0;
			int num3 = 0;
			int num4 = 75;
			if (name == "favorites" && areas.Count <= 16)
			{
				num4 += 25;
			}
			int num5 = Mathf.CeilToInt((float)areas.Count / (float)maxPerPage);
			if (page > num5 - 1)
			{
				page = 0;
			}
			else if (page < 0)
			{
				page = num5 - 1;
			}
			int num6 = page * maxPerPage;
			int num7 = Mathf.Min(num6 + maxPerPage - 1, areas.Count - 1);
			if (num5 >= 2)
			{
				if (!flag2)
				{
					GameObject gameObject2 = base.AddModelButton("ButtonForward", name + "NextPage", null, 80, num, false);
					gameObject2.transform.localScale = vector;
				}
				bool flag3 = this.didShowBackButtonForThisName.ContainsKey(name);
				if (page >= 1 || flag3)
				{
					GameObject gameObject3 = base.AddModelButton("ButtonBack", name + "PreviousPage", null, -220, num, false);
					gameObject3.transform.localScale = vector;
					if (!flag3)
					{
						this.didShowBackButtonForThisName.Add(name, true);
					}
				}
			}
			if (page >= 1)
			{
				Managers.achievementManager.RegisterAchievement(Achievement.PagedAreasDialog);
			}
			for (int i = num6; i <= num7; i++)
			{
				AreaOverview areaOverview = areas[i];
				bool flag4 = areaOverview.id == string.Empty || areaOverview.id == null;
				string text = areaOverview.name;
				int num8 = -225 + num2 * 450;
				int num9 = y + 105 + num3 * num4;
				if (flag4)
				{
					base.AddButton("areaSearchKeyword", areaOverview.name, text, "ButtonCompactSmallIcon", num8, num9, "search", false, 1f, TextColor.Default, 0.75f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
				else if (areaOverview.id == "[searchAll]")
				{
					text = "search " + this.totalSearchablePublicAreas + " areas...";
					base.AddButton("findAreas", null, text, "ButtonCompactSmallIcon", num8, num9, "search", false, 0.875f, TextColor.Default, 0.75f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
				else if (areaOverview.name == "sub-areas")
				{
					base.AddButton("subAreas", null, "sub-areas", "ButtonCompactSmallIcon", num8, num9, "subConnections-wide", false, 1f, TextColor.Default, 0.4f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
				else
				{
					text = base.GetTeleportToAreaText(areaOverview.name, areaOverview.playerCount);
					GameObject gameObject4 = base.AddButton("area", areaOverview.name, text, "AreaWithPeopleButton", num8, num9, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
					base.AddPeopleCountToButton(gameObject4, areaOverview.playerCount);
				}
				if (++num2 >= 2)
				{
					num2 = 0;
					num3++;
				}
			}
		}
		else if (name == "created" && this.fullViewSectionName == string.Empty)
		{
			if (Universe.features.createArea)
			{
				base.AddButton("createArea", null, "create area...", "ButtonCompact", 0, 410, "createArea", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
		}
		else
		{
			base.AddLabel("none yet", -115, y + 70, 0.6f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00038C6B File Offset: 0x0003706B
	private new void Update()
	{
		if (this.doUpdateAreasNextFrame)
		{
			this.doUpdateAreasNextFrame = false;
			this.UpdateAreaDisplay();
		}
		base.ReactToOnClick();
		base.ReactToOnClickInWrapper(this.areasWrapper);
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x00038C97 File Offset: 0x00037097
	private void GoToNextPage(List<AreaOverview> areas, ref int page)
	{
		global::UnityEngine.Object.Destroy(this.areasWrapper);
		page++;
		this.doUpdateAreasNextFrame = true;
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x00038CB1 File Offset: 0x000370B1
	private void GoToPreviousPage(List<AreaOverview> areas, ref int page)
	{
		global::UnityEngine.Object.Destroy(this.areasWrapper);
		page--;
		this.doUpdateAreasNextFrame = true;
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x00038CCC File Offset: 0x000370CC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "createArea":
			if (CrossDevice.desktopMode && !Our.createThingsInDesktopMode && !Managers.achievementManager.DidAchieve(Achievement.CreatedNewArea))
			{
				GameObject gameObject = base.SwitchTo(DialogType.Alert, string.Empty);
				AlertDialog component = gameObject.GetComponent<AlertDialog>();
				string text4 = "Creating areas & building things is only available in VR. Anyland supports Vive, Oculus Touch, and Windows Mixed Reality. Hope to see you in VR!";
				component.SetInfo(text4);
			}
			else if (!Managers.personManager.ourPerson.hasEditTools)
			{
				GameObject gameObject2 = base.SwitchTo(DialogType.GetEditTools, string.Empty);
				GetEditToolsDialog component2 = gameObject2.GetComponent<GetEditToolsDialog>();
				if (component2 != null)
				{
					component2.dialogToForwardToAfterHasEditTools = DialogType.Areas;
				}
			}
			else
			{
				int areaCount = Managers.personManager.ourPerson.areaCount;
				bool isInEditToolsTrial = Managers.personManager.ourPerson.isInEditToolsTrial;
				int num2 = ((!isInEditToolsTrial) ? 250 : 25);
				if (areaCount >= num2)
				{
					if (isInEditToolsTrial)
					{
						GameObject gameObject3 = base.SwitchTo(DialogType.GetEditTools, string.Empty);
						GetEditToolsDialog component3 = gameObject3.GetComponent<GetEditToolsDialog>();
						component3.introText = "Oops, you've reached the maximum number of areas one can create during the trial.";
						component3.dialogToForwardToAfterHasEditTools = DialogType.Areas;
					}
					else
					{
						string text2 = "Oops, you've reached the maximum number of areas one can create at the moment. Please email us at we@anyland.com so we can look into this. Thanks!";
						Managers.dialogManager.ShowInfo(text2, false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
					}
				}
				else
				{
					Managers.dialogManager.GetInput(delegate(string text)
					{
						if (!string.IsNullOrEmpty(text))
						{
							string text5;
							if (!Validator.AreaNameIsValid(text, out text5))
							{
								Managers.dialogManager.ShowInfo(text5, false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
							}
							else
							{
								base.CloseDialog();
								Managers.areaManager.CreateArea(text);
							}
						}
						else
						{
							base.CloseDialog();
						}
					}, contextName, string.Empty, 40, "area name", false, false, false, false, 1f, false, false, null, false);
				}
			}
			break;
		case "findAreas":
			Managers.dialogManager.GetInput(delegate(string text)
			{
				Our.lastAreasSearchText = text;
				if (string.IsNullOrEmpty(Our.lastAreasSearchText))
				{
					base.SwitchTo(DialogType.Areas, string.Empty);
					Our.lastAreasSearchText = null;
				}
				else
				{
					base.SwitchTo(DialogType.FindAreas, string.Empty);
				}
			}, contextName, string.Empty, 40, "find areas (optionally, use \"by somename\" and \"copyable\")", true, true, false, false, 0.875f, false, false, null, false);
			break;
		case "areaSearchKeyword":
			Our.lastSuggestedAreasPage = this.suggestedAreasPage;
			Our.lastAreasSearchText = contextId;
			this.areaListSetCache = this.receivedAreaListSet;
			base.SwitchTo(DialogType.FindAreas, string.Empty);
			break;
		case "showSectionInFullView":
			global::UnityEngine.Object.Destroy(this.areasWrapper);
			this.fullViewSectionName = contextId;
			this.doUpdateAreasNextFrame = true;
			break;
		case "suggestedNextPage":
			this.GoToNextPage(this.suggestedAreas, ref this.suggestedAreasPage);
			break;
		case "visitedNextPage":
			this.GoToNextPage(this.visitedAreas, ref this.visitedAreasPage);
			break;
		case "createdNextPage":
			this.GoToNextPage(this.createdAreas, ref this.createdAreasPage);
			break;
		case "favoritesNextPage":
			this.GoToNextPage(this.favorites, ref this.favoritesPage);
			break;
		case "suggestedPreviousPage":
			this.GoToPreviousPage(this.suggestedAreas, ref this.suggestedAreasPage);
			break;
		case "visitedPreviousPage":
			this.GoToPreviousPage(this.visitedAreas, ref this.visitedAreasPage);
			break;
		case "createdPreviousPage":
			this.GoToPreviousPage(this.createdAreas, ref this.createdAreasPage);
			break;
		case "favoritesPreviousPage":
			this.GoToPreviousPage(this.favorites, ref this.favoritesPage);
			break;
		case "subAreas":
			base.SwitchTo(DialogType.SubAreas, string.Empty);
			break;
		case "area":
			if (contextId == "random")
			{
				Managers.areaManager.TransportToRandomArea();
			}
			else if (contextId == "quests")
			{
				Managers.forumManager.OpenForumByName(this.hand, "quests", false);
			}
			else
			{
				string text3 = contextId;
				if (this.showSuggestedAreasDebugInfo)
				{
					int num3 = text3.IndexOf("]");
					if (num3 >= 0)
					{
						text3 = text3.Substring(num3);
					}
				}
				base.ClickedAreaTeleportButton(text3);
			}
			break;
		case "back":
			if (this.fullViewSectionName == string.Empty)
			{
				base.SwitchTo(DialogType.Main, string.Empty);
			}
			else
			{
				global::UnityEngine.Object.Destroy(this.areasWrapper);
				this.fullViewSectionName = string.Empty;
				this.doUpdateAreasNextFrame = true;
			}
			break;
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x040006E4 RID: 1764
	private bool showSuggestedAreasDebugInfo;

	// Token: 0x040006E5 RID: 1765
	private List<AreaOverview> suggestedAreas;

	// Token: 0x040006E6 RID: 1766
	private List<AreaOverview> visitedAreas;

	// Token: 0x040006E7 RID: 1767
	private List<AreaOverview> createdAreas;

	// Token: 0x040006E8 RID: 1768
	private List<AreaOverview> favorites;

	// Token: 0x040006E9 RID: 1769
	private string fullViewSectionName = string.Empty;

	// Token: 0x040006EA RID: 1770
	private int suggestedAreasPage;

	// Token: 0x040006EB RID: 1771
	private int visitedAreasPage;

	// Token: 0x040006EC RID: 1772
	private int createdAreasPage;

	// Token: 0x040006ED RID: 1773
	private int favoritesPage;

	// Token: 0x040006EE RID: 1774
	private const string nameForRandomArea = "random";

	// Token: 0x040006EF RID: 1775
	private const string nameForSubAreasDialog = "sub-areas";

	// Token: 0x040006F0 RID: 1776
	private const string nameForQuests = "quests";

	// Token: 0x040006F1 RID: 1777
	private int totalAreas;

	// Token: 0x040006F2 RID: 1778
	private int totalPublicAreas;

	// Token: 0x040006F3 RID: 1779
	private int totalSearchablePublicAreas;

	// Token: 0x040006F4 RID: 1780
	private const int suggestedAreasPerPage = 10;

	// Token: 0x040006F5 RID: 1781
	private AreaListSet receivedAreaListSet;

	// Token: 0x040006F6 RID: 1782
	private GameObject areasWrapper;

	// Token: 0x040006F7 RID: 1783
	private bool doUpdateAreasNextFrame;

	// Token: 0x040006F8 RID: 1784
	private Dictionary<string, bool> didShowBackButtonForThisName = new Dictionary<string, bool>();

	// Token: 0x040006F9 RID: 1785
	private bool sortedCreatedAreas;
}
