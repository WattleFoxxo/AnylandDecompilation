using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x02000114 RID: 276
public class FriendsDialog : Dialog
{
	// Token: 0x060009E9 RID: 2537 RVA: 0x000430EC File Offset: 0x000414EC
	public void Start()
	{
		this.showDebugInfo = false;
		this.isSearchResult = !string.IsNullOrEmpty(this.nameSearch);
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddLabel("Friends", 0, -450, 1.2f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		if (this.isSearchResult)
		{
			this.ShowFriendsSearchResult();
		}
		else
		{
			try
			{
				Managers.personManager.GetFriendsByStrength(delegate(FriendListInfoCollection online, FriendListInfoCollection offline)
				{
					if (this == null)
					{
						return;
					}
					if (online != null)
					{
						this.persons.AddRange(online.friends);
					}
					if (offline != null)
					{
						this.persons.AddRange(offline.friends);
					}
					FriendsDialog.personsCache = this.persons;
					this.persons.Insert(0, this.GetOurPersonAsFriendListInfo());
					base.StartCoroutine(this.UpdatePersonsDisplay());
				});
			}
			catch (Exception ex)
			{
				Log.Warning("There was an issue loading the friends list");
			}
		}
		this.AddPatronsThanks();
		Managers.achievementManager.RegisterAchievement(Achievement.OpenedFriendsDialog);
	}

	// Token: 0x060009EA RID: 2538 RVA: 0x000431BC File Offset: 0x000415BC
	private void AddPatronsThanks()
	{
		string[] array = new string[]
		{
			"Aaron", "Fazzi", "Arch", "Koolala", "Shevtsov", "Jinx", "Delco", "Ferny", "Frash", "Fwiller",
			"Gabgab", "Garry", "Giodude", "Jacob", "Jennproc", "Laughing Flower", "Nebula", "Lady Eight", "Nicolas Cage", "Pen",
			"Thifa Fordring", "Shannon", "Silverfish", "Silver", "Tombot", "Zid", "Hungry Bunny", "Xau", "SirDesh", "Saf",
			"Yoofaloof", "Zetaphor", "DAgility", "Dr. Tangerine", "Brian P", "Simon L", "Reddof"
		};
		Array.Sort<string>(array);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Replace(" ", "~");
		}
		string text = "Thanks to all patrons at patreon.com/anyland!" + Environment.NewLine + Environment.NewLine + string.Join("    ", array);
		text = Misc.WrapWithNewlines(text, 40, -1);
		text = text.Replace("~", " ");
		base.AddLabel(text, 0, -310, 0.9f, true, TextColor.Gold, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x000433A0 File Offset: 0x000417A0
	private string GetPatronsHTML(string[] patrons)
	{
		string text = string.Empty;
		text = text + "<!-- Start of auto-generated credits -->" + Environment.NewLine;
		text = text + "<div id=\"patrons\">" + Environment.NewLine;
		text = text + "Thanks to all <a href=\"https://www.patreon.com/anyland\">Patreon</a> patrons!<br/><br/>" + Environment.NewLine;
		text = text + string.Join(" &nbsp; &nbsp; ", patrons) + Environment.NewLine;
		text = text + "</div>" + Environment.NewLine;
		text += "<!-- End of of auto-generated credits -->";
		return text.Replace("~", "&nbsp;");
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x0004342C File Offset: 0x0004182C
	private void ShowFriendsSearchResult()
	{
		if (this.persons.Count >= 1)
		{
			base.StartCoroutine(this.UpdatePersonsDisplay());
		}
		else
		{
			string text = string.Concat(new string[]
			{
				"No friend names containing",
				Environment.NewLine,
				"\"",
				Misc.Truncate(this.nameSearch, 30, true),
				"\" found"
			});
			base.AddLabel(text, 0, -50, 0.9f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
			this.findButton = base.AddGenericFindButton("find");
		}
	}

	// Token: 0x060009ED RID: 2541 RVA: 0x000434C8 File Offset: 0x000418C8
	private void UpdatePageLabel()
	{
		if (this.maxPages >= 2)
		{
			if (this.pageLabel == null)
			{
				this.pageLabel = base.AddLabel(string.Empty, 0, 396, 0.8f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
			}
			this.pageLabel.text = ((!this.currentlyShowingLinkedAreas && this.page != 0) ? (this.page + 1).ToString() : string.Empty);
		}
	}

	// Token: 0x060009EE RID: 2542 RVA: 0x0004355C File Offset: 0x0004195C
	private FriendListInfo GetOurPersonAsFriendListInfo()
	{
		FriendListInfo friendListInfo = new FriendListInfo();
		friendListInfo.id = Managers.personManager.ourPerson.userId;
		friendListInfo.screenName = "me";
		friendListInfo.statusText = Managers.personManager.ourPerson.statusText;
		if (Managers.personManager.ourPerson.isFindable)
		{
			friendListInfo.currentAreaId = Managers.areaManager.currentAreaId;
			friendListInfo.currentAreaName = Managers.areaManager.currentAreaName;
			friendListInfo.currentAreaTotal = Managers.personManager.GetCurrentAreaPersonCount();
		}
		friendListInfo.isOnline = true;
		return friendListInfo;
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x000435F0 File Offset: 0x000419F0
	private void RemoveAllFriendsToTest()
	{
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x000435F4 File Offset: 0x000419F4
	private IEnumerator UpdatePersonsDisplay()
	{
		if (this.maxPages == -1)
		{
			this.maxPages = Mathf.CeilToInt((float)this.persons.Count / 4f);
			if (this.maxPages >= 2)
			{
				base.AddDefaultPagingButtons(80, 415, "Page", true, 0, 0.85f, false);
			}
		}
		if (this.findButton == null && (this.maxPages >= 2 || this.isSearchResult))
		{
			this.findButton = base.AddGenericFindButton("find");
		}
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.UpdatePageLabel();
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int minI = this.page * 4;
		int maxI = Mathf.Min(minI + 4 - 1, this.persons.Count - 1);
		int row = 0;
		for (int i = minI; i <= maxI; i++)
		{
			FriendListInfo friendListInfo = this.persons[i];
			int num = 225;
			int num2 = -295 + row * 175;
			friendListInfo.screenName = Managers.personManager.GetScreenNameWithDiscloser(friendListInfo.id, friendListInfo.screenName);
			string text = Misc.Truncate(friendListInfo.screenName.ToUpper(), 25, true);
			TextColor textColor = ((!friendListInfo.isOnline) ? TextColor.Default : TextColor.Green);
			base.AddButton("userInfo", friendListInfo.id, text, "ButtonCompactNoIcon", -num, num2, null, false, 1f, textColor, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			if (friendListInfo.currentAreaName != null)
			{
				bool flag = friendListInfo.currentAreaName == Managers.areaManager.currentAreaName;
				string text2 = friendListInfo.currentAreaName;
				bool flag2 = friendListInfo.currentAreaTotal > 0;
				if (flag)
				{
					base.AddAreaWithPeopleCountLabel(text2, friendListInfo.currentAreaTotal, num + 12, num2);
				}
				else
				{
					text2 = Misc.Truncate(text2, (!flag2) ? 17 : 14, true);
					GameObject gameObject = base.AddButton("toAreaByPerson", friendListInfo.id, text2, "AreaWithPeopleButton", num, num2, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
					base.AddPeopleCountToButton(gameObject, friendListInfo.currentAreaTotal);
				}
			}
			else if (friendListInfo.isOnline)
			{
				string areaFromReceivedPings = this.GetAreaFromReceivedPings(friendListInfo.id, 10);
				if (areaFromReceivedPings != null)
				{
					string text3 = "Pinged you from\n" + Misc.Truncate(areaFromReceivedPings, 22, true);
					GameObject gameObject2 = base.AddButton("toAreaByPersonPing", friendListInfo.id, text3, "ButtonCompactSmallIcon", num, num2, "teleportTo", false, 0.8f, TextColor.Green, 1f, 0.00225f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
			}
			bool flag3 = friendListInfo.id == Managers.personManager.ourPerson.userId;
			if (this.showDebugInfo && !flag3)
			{
				base.AddLabel(this.GetDebugInfo(friendListInfo), -445, num2 + 50, 0.6f, false, TextColor.Blue, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			}
			else if (friendListInfo.statusText != null)
			{
				int num3 = this.AddLinkButtonsIfNeeded(friendListInfo, num2);
				int num4 = ((friendListInfo.statusText.IndexOf(" ") != -1) ? 60 : 40);
				num4 -= num3 * 8;
				string text4 = Misc.Truncate(friendListInfo.statusText.ToUpper(), num4, true);
				base.AddLabel(text4, -445, num2 + 50, 0.6f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			}
			int num5 = num2 + 110;
			base.AddSeparator(0, num5, true);
			row++;
		}
		if (this.persons.Count <= 1 && !this.isSearchResult)
		{
			this.AddHelpImageOnHowToAddFriends();
			this.AddTeleportToLivelyAreaIfAvailable();
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x060009F1 RID: 2545 RVA: 0x00043610 File Offset: 0x00041A10
	private string GetDebugInfo(FriendListInfo person)
	{
		string text = string.Empty;
		text = text + "Friendship strength: " + person.strength;
		text += "   |  ";
		return text + "Last activity: " + Misc.GetHowLongAgoText(person.lastActivityOn);
	}

	// Token: 0x060009F2 RID: 2546 RVA: 0x00043660 File Offset: 0x00041A60
	private string GetAreaFromReceivedPings(string personId, int maxMinutesAge = -1)
	{
		string text = null;
		foreach (global::Ping ping in Managers.pollManager.receivedPings)
		{
			if (ping.originPersonId == personId)
			{
				if (ping.originAreaName != Managers.areaManager.currentAreaName)
				{
					TimeSpan timeSpan = DateTime.Now - ping.receivedOn;
					if (maxMinutesAge == -1 || timeSpan.Minutes <= maxMinutesAge)
					{
						text = ping.originAreaName;
					}
				}
				break;
			}
		}
		return text;
	}

	// Token: 0x060009F3 RID: 2547 RVA: 0x00043718 File Offset: 0x00041B18
	private int AddLinkButtonsIfNeeded(FriendListInfo person, int displayY)
	{
		int num = 0;
		List<string> links = this.GetLinks(person.statusText, "area");
		List<string> links2 = this.GetLinks(person.statusText, "board");
		int num2 = 402;
		int num3 = displayY + 67;
		if (links.Count >= 1)
		{
			GameObject gameObject = base.AddModelButton("AreaButtonNonPositioned", "toLinkedAreas", person.id, num2 - 5 - num * 104, num3 - 7, false);
			gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(0.35f, 1f, 0.35f));
			num++;
		}
		if (links2.Count >= 1)
		{
			GameObject gameObject2 = base.AddModelButton("ForumsButtonNonPositioned", "toForum", links2[0], num2 + 14 - num * 104, num3, false);
			gameObject2.transform.localScale = Vector3.Scale(gameObject2.transform.localScale, new Vector3(0.45f, 1.2f, 0.45f));
			num++;
		}
		return num;
	}

	// Token: 0x060009F4 RID: 2548 RVA: 0x00043824 File Offset: 0x00041C24
	private void AddHelpImageOnHowToAddFriends()
	{
		string text = "HelpAddFriends";
		if (CrossDevice.desktopMode)
		{
			text += "_KeyboardMouse";
		}
		else if (CrossDevice.type == global::DeviceType.OculusTouch)
		{
			text += "_OculusTouch";
		}
		else if (CrossDevice.type == global::DeviceType.Index)
		{
			text += "_Index";
		}
		base.Add(text, -210, 320, false);
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x00043898 File Offset: 0x00041C98
	private void AddTeleportToLivelyAreaIfAvailable()
	{
		Managers.areaManager.GetAreaLists(delegate(AreaListSet areaListSet)
		{
			if (this == null)
			{
				return;
			}
			foreach (AreaOverview areaOverview in areaListSet.lively)
			{
				if (areaOverview.id != Managers.areaManager.currentAreaId)
				{
					base.AddButton("toArea", areaOverview.name, "there's people here", "ButtonCompactSmallIcon", 235, 400, "teleportTo", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
					break;
				}
			}
		});
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x000438B0 File Offset: 0x00041CB0
	private List<string> GetLinks(string text, string type)
	{
		List<string> list = new List<string>();
		if (!string.IsNullOrEmpty(text))
		{
			MatchCollection matchCollection = Regex.Matches(text, "\\[(.*?)\\]");
			for (int i = 0; i < matchCollection.Count; i++)
			{
				string text2 = matchCollection[i].ToString().Substring(1, matchCollection[i].Length - 2);
				if (text2.IndexOf(":") == -1)
				{
					text2 = "area:" + text2;
				}
				string text3 = type + ":";
				if (text2.IndexOf(text3) == 0)
				{
					list.Add(text2.Substring(text3.Length).Trim());
				}
			}
		}
		return list;
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x00043964 File Offset: 0x00041D64
	private IEnumerator ShowLinkedAreas(string personId)
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.currentlyShowingLinkedAreas = true;
		this.UpdatePageLabel();
		FriendListInfo person = this.GetPersonById(personId);
		List<string> areas = this.GetLinks(person.statusText, "area");
		base.SetActiveDefaultPagingButtons(false);
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int buttonX = 0;
		int buttonY = 0;
		int num = 0;
		while (num < areas.Count && num < 10)
		{
			string text = areas[num];
			string text2 = Misc.Truncate(text, 21, true);
			int num2 = -225 + buttonX * 450;
			int num3 = -300 + buttonY * 75;
			base.AddButton("toArea", text, text2, "ButtonCompactSmallIcon", num2, num3, "teleportTo", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			if (++buttonX >= 2)
			{
				buttonX = 0;
				buttonY++;
			}
			num++;
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x00043988 File Offset: 0x00041D88
	private FriendListInfo GetPersonById(string id)
	{
		FriendListInfo friendListInfo = null;
		foreach (FriendListInfo friendListInfo2 in this.persons)
		{
			if (friendListInfo2.id == id)
			{
				friendListInfo = friendListInfo2;
				break;
			}
		}
		return friendListInfo;
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x000439F8 File Offset: 0x00041DF8
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "userInfo":
		{
			Our.personIdOfInterest = contextId;
			this.hand.lastContextInfoHit = null;
			Our.dialogToGoBackTo = DialogType.Friends;
			if (this.isSearchResult)
			{
				Managers.personManager.IncreaseFriendshipStrength(contextId, delegate
				{
				});
			}
			GameObject gameObject = base.SwitchTo(DialogType.Profile, string.Empty);
			ProfileDialog component = gameObject.GetComponent<ProfileDialog>();
			component.friendsDialogPageToGoBackTo = this.page;
			break;
		}
		case "toAreaByPerson":
		{
			FriendListInfo personById = this.GetPersonById(contextId);
			Managers.personManager.IncreaseFriendshipStrength(contextId, delegate
			{
			});
			base.ClickedAreaTeleportButton(personById.currentAreaName);
			break;
		}
		case "toAreaByPersonPing":
		{
			string areaFromReceivedPings = this.GetAreaFromReceivedPings(contextId, -1);
			if (areaFromReceivedPings != null)
			{
				FriendListInfo personById2 = this.GetPersonById(contextId);
				Managers.personManager.IncreaseFriendshipStrength(contextId, delegate
				{
				});
				base.ClickedAreaTeleportButton(areaFromReceivedPings);
			}
			break;
		}
		case "toLinkedAreas":
			base.StartCoroutine(this.ShowLinkedAreas(contextId));
			break;
		case "toForum":
			Managers.forumManager.OpenForumByName(this.hand, contextId, false);
			break;
		case "toArea":
			base.ClickedAreaTeleportButton(contextId);
			break;
		case "previousPage":
		{
			int num = --this.page;
			if (num < 0)
			{
				this.page = this.maxPages - 1;
			}
			base.StartCoroutine(this.UpdatePersonsDisplay());
			break;
		}
		case "nextPage":
		{
			int num = ++this.page;
			if (num > this.maxPages - 1)
			{
				this.page = 0;
			}
			base.StartCoroutine(this.UpdatePersonsDisplay());
			break;
		}
		case "find":
			Managers.dialogManager.GetInput(delegate(string text)
			{
				GameObject gameObject2 = base.SwitchTo(DialogType.Friends, string.Empty);
				if (!string.IsNullOrEmpty(text))
				{
					FriendsDialog component2 = gameObject2.GetComponent<FriendsDialog>();
					List<FriendListInfo> list = new List<FriendListInfo>();
					foreach (FriendListInfo friendListInfo in FriendsDialog.personsCache)
					{
						if (friendListInfo.screenName.ToLower().Contains(text))
						{
							list.Add(friendListInfo);
						}
					}
					component2.persons = list;
					component2.nameSearch = text;
				}
			}, string.Empty, string.Empty, 60, "friend's name", false, false, false, false, 1f, false, false, null, false);
			break;
		case "back":
			if (this.currentlyShowingLinkedAreas)
			{
				this.currentlyShowingLinkedAreas = false;
				base.SetActiveDefaultPagingButtons(true);
				base.StartCoroutine(this.UpdatePersonsDisplay());
			}
			else
			{
				base.SwitchTo(DialogType.Main, string.Empty);
			}
			break;
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x04000769 RID: 1897
	private const bool generatePatronHtml = false;

	// Token: 0x0400076A RID: 1898
	private bool showDebugInfo;

	// Token: 0x0400076B RID: 1899
	public int page;

	// Token: 0x0400076C RID: 1900
	public List<FriendListInfo> persons = new List<FriendListInfo>();

	// Token: 0x0400076D RID: 1901
	public static List<FriendListInfo> personsCache;

	// Token: 0x0400076E RID: 1902
	private const string contextIdSeparator = "|";

	// Token: 0x0400076F RID: 1903
	private GameObject backButton;

	// Token: 0x04000770 RID: 1904
	private GameObject forwardButton;

	// Token: 0x04000771 RID: 1905
	private int maxPages = -1;

	// Token: 0x04000772 RID: 1906
	private const int maxPerPage = 4;

	// Token: 0x04000773 RID: 1907
	private bool currentlyShowingLinkedAreas;

	// Token: 0x04000774 RID: 1908
	private TextMesh pageLabel;

	// Token: 0x04000775 RID: 1909
	private GameObject findButton;

	// Token: 0x04000776 RID: 1910
	public string nameSearch;

	// Token: 0x04000777 RID: 1911
	private bool isSearchResult;

	// Token: 0x04000778 RID: 1912
	private const string spacePlaceholder = "~";
}
