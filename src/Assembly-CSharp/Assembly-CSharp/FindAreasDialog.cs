using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000105 RID: 261
public class FindAreasDialog : Dialog
{
	// Token: 0x06000941 RID: 2369 RVA: 0x00039740 File Offset: 0x00037B40
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddGenericFindButton("findAreas");
		string searchText = string.Empty;
		string byCreatorName = string.Empty;
		string empty = string.Empty;
		if (FindAreasDialog.findByCreatorId != string.Empty)
		{
			empty = FindAreasDialog.findByCreatorId;
			FindAreasDialog.findByCreatorId = string.Empty;
		}
		else
		{
			searchText = Our.lastAreasSearchText;
			byCreatorName = this.ExtractCreatorNameFromSearchIfNeeded(ref searchText);
		}
		Managers.areaManager.SearchAreas(searchText, byCreatorName, empty, delegate(AreaList areaList)
		{
			if (this == null)
			{
				return;
			}
			this.areas = areaList.areas;
			if (areaList.ownPrivateAreas != null)
			{
				foreach (AreaOverview areaOverview in areaList.ownPrivateAreas)
				{
					areaOverview.isPrivate = true;
					areaOverview.description = "Private & shown here for editors only";
					this.areas.Add(areaOverview);
				}
			}
			this.maxPages = Mathf.CeilToInt((float)this.areas.Count / 8f);
			if (this.areas.Count >= 1)
			{
				this.MemorizeInput(searchText, byCreatorName);
			}
			if (this.maxPages >= 2)
			{
				this.AddPagingButtons();
			}
			this.UpdateResultDisplay();
		});
	}

	// Token: 0x06000942 RID: 2370 RVA: 0x00039810 File Offset: 0x00037C10
	private void MemorizeInput(string searchText, string byCreatorName)
	{
		string text = searchText;
		if (byCreatorName != string.Empty)
		{
			text = text + " by " + byCreatorName;
		}
		text = text.Trim().ToLower();
		if (text != string.Empty)
		{
			FindAreasDialog.recentlyEnteredInput = base.AddToRecentlyEnteredInput(FindAreasDialog.recentlyEnteredInput, text);
		}
	}

	// Token: 0x06000943 RID: 2371 RVA: 0x0003986C File Offset: 0x00037C6C
	private string ExtractCreatorNameFromSearchIfNeeded(ref string searchText)
	{
		string text = string.Empty;
		int num = searchText.IndexOf("by ");
		if (num == 0)
		{
			text = searchText.Substring("by ".Length);
			searchText = string.Empty;
		}
		else if (num > 0)
		{
			string[] array = Misc.Split(searchText, "by ", StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 2)
			{
				searchText = array[0];
				text = array[1];
			}
		}
		return text;
	}

	// Token: 0x06000944 RID: 2372 RVA: 0x000398D8 File Offset: 0x00037CD8
	private void AddPagingButtons()
	{
		GameObject gameObject = base.AddModelButton("ButtonBack", "previousPage", null, -100, 410, false);
		gameObject.transform.localScale = new Vector3(0.8f, 1f, 0.8f);
		GameObject gameObject2 = base.AddModelButton("ButtonForward", "nextPage", null, 100, 410, false);
		gameObject2.transform.localScale = new Vector3(0.8f, 1f, 0.8f);
		this.pageLabel = base.AddLabel(string.Empty, -12, 385, 1f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
	}

	// Token: 0x06000945 RID: 2373 RVA: 0x00039980 File Offset: 0x00037D80
	private void UpdatePageLabel()
	{
		if (this.pageLabel != null)
		{
			this.pageLabel.text = ((this.page != 0) ? (this.page + 1).ToString() : string.Empty);
		}
	}

	// Token: 0x06000946 RID: 2374 RVA: 0x000399D4 File Offset: 0x00037DD4
	private void UpdateResultDisplay()
	{
		global::UnityEngine.Object.Destroy(this.wrapper);
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		string text = Misc.Truncate(Our.lastAreasSearchText, 40, true);
		if (FindAreasDialog.findByCreatorId_nameForReference != string.Empty)
		{
			text = "by " + FindAreasDialog.findByCreatorId_nameForReference;
			FindAreasDialog.findByCreatorId_nameForReference = string.Empty;
		}
		base.AddLabel(text, -250, -420, 0.9f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		if (this.areas.Count == 0)
		{
			string text2 = "nothing found yet";
			if (text.IndexOf("by ") != 0)
			{
				text2 += "\nin names and descriptions";
			}
			text2 += "...";
			base.AddLabel(text2, -250, -270, 0.6f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		else
		{
			int num = this.page * 8;
			int num2 = num + 8 - 1;
			if (num2 > this.areas.Count - 1)
			{
				num2 = this.areas.Count - 1;
			}
			int num3 = 0;
			for (int i = num; i <= num2; i++)
			{
				AreaOverview areaOverview = this.areas[i];
				string teleportToAreaText = base.GetTeleportToAreaText(areaOverview.name, areaOverview.playerCount);
				int num4 = -225;
				int num5 = -315 + num3++ * 85;
				string text3 = ((!areaOverview.isPrivate) ? string.Empty : "180,200,210");
				string text4 = "area";
				string name = areaOverview.name;
				string text5 = teleportToAreaText;
				string text6 = "AreaWithPeopleButton";
				int num6 = num4;
				int num7 = num5;
				string text7 = text3;
				GameObject gameObject = base.AddButton(text4, name, text5, text6, num6, num7, null, false, 1f, TextColor.Default, 1f, 0f, 0f, text7, false, false, TextAlignment.Left, false, false);
				base.AddPeopleCountToButton(gameObject, areaOverview.playerCount);
				string text8 = areaOverview.description;
				if (text8 != null)
				{
					text8 = text8.Trim();
					text8 = Misc.Truncate(text8, 60, true);
					text8 = Misc.WrapWithNewlines(text8, 30, 2);
					TextColor textColor = ((!areaOverview.isPrivate) ? TextColor.Default : TextColor.Gray);
					text7 = text8;
					num7 = 20;
					num6 = num5 - 30;
					float num8 = 0.6f;
					TextColor textColor2 = textColor;
					base.AddLabel(text7, num7, num6, num8, false, textColor2, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
				}
			}
		}
		base.SetUiWrapper(base.gameObject);
		this.UpdatePageLabel();
	}

	// Token: 0x06000947 RID: 2375 RVA: 0x00039C64 File Offset: 0x00038064
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "findAreas"))
			{
				if (!(contextName == "area"))
				{
					if (!(contextName == "previousPage"))
					{
						if (!(contextName == "nextPage"))
						{
							if (!(contextName == "back"))
							{
								if (contextName == "close")
								{
									base.CloseDialog();
								}
							}
							else
							{
								base.SwitchTo(DialogType.Areas, string.Empty);
							}
						}
						else
						{
							if (++this.page >= this.maxPages)
							{
								this.page = 0;
							}
							this.UpdateResultDisplay();
						}
					}
					else
					{
						if (--this.page < 0)
						{
							this.page = this.maxPages - 1;
						}
						this.UpdateResultDisplay();
					}
				}
				else
				{
					base.ClickedAreaTeleportButton(contextId);
				}
			}
			else
			{
				string text2 = string.Empty;
				if (Our.lastAreasSearchText != null && Our.lastAreasSearchText.IndexOf("copyable") == 0)
				{
					text2 = "copyable ";
				}
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
				}, contextName, text2, 60, "find areas (optionally, use \"by somename\" and \"copyable\")", true, true, false, false, 0.875f, false, false, null, false);
			}
		}
	}

	// Token: 0x04000702 RID: 1794
	private const int resultsPerPage = 8;

	// Token: 0x04000703 RID: 1795
	private int page;

	// Token: 0x04000704 RID: 1796
	private int maxPages = -1;

	// Token: 0x04000705 RID: 1797
	private List<AreaOverview> areas;

	// Token: 0x04000706 RID: 1798
	private TextMesh pageLabel;

	// Token: 0x04000707 RID: 1799
	public const string defaultSearchPlaceholder = "find areas (optionally, use \"by somename\" and \"copyable\")";

	// Token: 0x04000708 RID: 1800
	public const float defaultSearchPlaceholderSize = 0.875f;

	// Token: 0x04000709 RID: 1801
	public static string findByCreatorId = string.Empty;

	// Token: 0x0400070A RID: 1802
	public static string findByCreatorId_nameForReference = string.Empty;

	// Token: 0x0400070B RID: 1803
	public static List<string> recentlyEnteredInput = new List<string>();
}
