using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000107 RID: 263
public class SubAreasDialog : Dialog
{
	// Token: 0x06000965 RID: 2405 RVA: 0x0003BED0 File Offset: 0x0003A2D0
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		if (!Managers.areaManager.weAreEditorOfCurrentArea)
		{
			return;
		}
		Managers.areaManager.GetInfo(delegate(AreaInfo areaInfo)
		{
			if (this == null)
			{
				return;
			}
			bool flag = areaInfo.parentAreaId == Managers.areaManager.currentAreaId;
			if (flag)
			{
				Managers.areaManager.SetParentArea(Managers.areaManager.currentAreaId, null, delegate(bool ok)
				{
					if (ok)
					{
						Managers.soundManager.Play("pickUp", this.transform, 1f, false, false);
						base.SwitchTo(DialogType.SubAreas, string.Empty);
					}
				});
			}
			else if (string.IsNullOrEmpty(areaInfo.parentAreaId))
			{
				base.AddLabel("sub-areas are hidden in the default area list & searches", -440, 20, 0.65f, false, TextColor.Gray, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
				base.AddSeparator(0, 100, false);
				this.LoadData();
			}
			else
			{
				base.AddLabel("this area is a sub-area of another", -300, -100, 0.75f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
				base.AddButton("areaById", areaInfo.parentAreaId, "to main area", "Button", 0, 220, "teleportTo", false, 0.8f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			}
		});
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x0003BF25 File Offset: 0x0003A325
	private void LoadData()
	{
		Managers.areaManager.GetSubAreasOfArea(Managers.areaManager.currentAreaId, delegate(List<AreaOverview> subAreasSet)
		{
			if (this == null)
			{
				return;
			}
			this.subAreas = subAreasSet;
			this.SortAreaOverviewListAlphabetically(this.subAreas);
			Managers.areaManager.GetAreaLists(delegate(AreaListSet areaListSet)
			{
				if (this == null)
				{
					return;
				}
				this.createdAreas = areaListSet.created;
				this.ClearCurrentSubAreasAndCurrentAreaFromCreatedAreas();
				this.SortAreaOverviewListAlphabetically(this.createdAreas);
				Managers.soundManager.Play("whoosh", this.transform, 1f, false, false);
				base.StartCoroutine(this.UpdateAreaDisplay());
			});
		});
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x0003BF47 File Offset: 0x0003A347
	private void SortAreaOverviewListAlphabetically(List<AreaOverview> areas)
	{
		areas.Sort((AreaOverview i1, AreaOverview i2) => i1.name.CompareTo(i2.name));
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x0003BF6C File Offset: 0x0003A36C
	private void ClearCurrentSubAreasAndCurrentAreaFromCreatedAreas()
	{
		foreach (AreaOverview areaOverview in this.createdAreas)
		{
			if (areaOverview.id == Managers.areaManager.currentAreaId)
			{
				areaOverview.id = string.Empty;
			}
			else
			{
				foreach (AreaOverview areaOverview2 in this.subAreas)
				{
					if (areaOverview.id == areaOverview2.id)
					{
						areaOverview.id = string.Empty;
						break;
					}
				}
			}
		}
		for (int i = this.createdAreas.Count - 1; i >= 0; i--)
		{
			if (this.createdAreas[i].id == string.Empty)
			{
				this.createdAreas.RemoveAt(i);
			}
		}
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x0003C0A8 File Offset: 0x0003A4A8
	private IEnumerator UpdateAreaDisplay()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		this.UpdateAreaDisplayPage("subAreas", "sub-areas of this area", this.subAreas, ref this.subAreasPage, -300, true);
		this.UpdateAreaDisplayPage("created", "add from created areas", this.createdAreas, ref this.createdAreasPage, 150, false);
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x0003C0C4 File Offset: 0x0003A4C4
	private void UpdateAreaDisplayPage(string name, string label, List<AreaOverview> areas, ref int page, int y, bool isMainHeading = false)
	{
		if (isMainHeading)
		{
			base.AddLabel(label, -215, y, 0.85f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		else
		{
			base.AddLabel(label, -205, y, 0.7f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		if (areas != null && areas.Count >= 1)
		{
			int num = 0;
			int num2 = Mathf.CeilToInt((float)areas.Count / 3f);
			if (page > num2 - 1)
			{
				page = 0;
			}
			else if (page < 0)
			{
				page = num2 - 1;
			}
			int num3 = page * 3;
			int num4 = Mathf.Min(num3 + 3 - 1, areas.Count - 1);
			if (num2 >= 2)
			{
				int num5 = y + 20;
				Vector3 vector = new Vector3(0.5f, 1f, 0.5f);
				GameObject gameObject = base.AddModelButton("ButtonForward", name + "NextPage", null, 420, num5, false);
				gameObject.transform.localScale = vector;
				GameObject gameObject2 = base.AddModelButton("ButtonBack", name + "PreviousPage", null, -420, num5, false);
				gameObject2.transform.localScale = vector;
			}
			for (int i = num3; i <= num4; i++)
			{
				AreaOverview areaOverview = areas[i];
				int num6 = y + 105 + num * 75;
				float num7 = 1f;
				float num8 = 0f;
				if (areaOverview.name.Length >= 30)
				{
					num7 -= 0.2f;
					num8 -= 0.0006f;
				}
				string text = "area";
				string name2 = areaOverview.name;
				string text2 = Misc.Truncate(areaOverview.name, 60, true);
				string text3 = "ButtonCompactSmallIconLong";
				int num9 = -130;
				int num10 = num6;
				string text4 = "teleportTo";
				float num11 = num7;
				float num12 = num8;
				base.AddButton(text, name2, text2, text3, num9, num10, text4, false, num11, TextColor.Default, 0.6f, num12, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				if (name == "subAreas")
				{
					base.AddButton("removeArea", areaOverview.id, "- remove", "ButtonCompactNoIconShort", 305, num6, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
				else
				{
					base.AddButton("addArea", areaOverview.id, "+ add", "ButtonCompactNoIconShort", 305, num6, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				}
				num++;
			}
		}
		else
		{
			base.AddLabel("none yet", -60, y + 130, 0.6f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x0003C3A4 File Offset: 0x0003A7A4
	private void MoveAreaFromOneToOtherList(List<AreaOverview> sourceList, List<AreaOverview> targetList, string idToMove)
	{
		for (int i = 0; i < sourceList.Count; i++)
		{
			if (sourceList[i].id == idToMove)
			{
				targetList.Add(sourceList[i]);
				sourceList.RemoveAt(i);
				this.SortAreaOverviewListAlphabetically(targetList);
				break;
			}
		}
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0003C400 File Offset: 0x0003A800
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "subAreasPreviousPage":
			this.subAreasPage--;
			base.StartCoroutine(this.UpdateAreaDisplay());
			break;
		case "subAreasNextPage":
			this.subAreasPage++;
			base.StartCoroutine(this.UpdateAreaDisplay());
			break;
		case "createdPreviousPage":
			this.createdAreasPage--;
			base.StartCoroutine(this.UpdateAreaDisplay());
			break;
		case "createdNextPage":
			this.createdAreasPage++;
			base.StartCoroutine(this.UpdateAreaDisplay());
			break;
		case "area":
			base.CloseDialog();
			base.ClickedAreaTeleportButton(contextId);
			break;
		case "areaById":
			base.CloseDialog();
			Managers.areaManager.TryTransportToAreaById(contextId);
			break;
		case "addArea":
		case "removeArea":
		{
			if (!Managers.areaManager.weAreEditorOfCurrentArea)
			{
				return;
			}
			if (contextName == "addArea")
			{
				this.MoveAreaFromOneToOtherList(this.createdAreas, this.subAreas, contextId);
				Managers.soundManager.Play("putDown", this.transform, 1f, false, false);
			}
			else
			{
				this.MoveAreaFromOneToOtherList(this.subAreas, this.createdAreas, contextId);
				Managers.soundManager.Play("pickUp", this.transform, 1f, false, false);
			}
			base.StartCoroutine(this.UpdateAreaDisplay());
			string text = ((!(contextName == "addArea")) ? null : Managers.areaManager.currentAreaId);
			Managers.areaManager.SetParentArea(contextId, text, delegate(bool ok)
			{
				if (ok)
				{
					if (contextName == "addArea")
					{
						Managers.achievementManager.RegisterAchievement(Achievement.DidSetSubArea);
					}
				}
				else
				{
					Managers.soundManager.Play("no", this.transform, 1f, false, false);
					this.LoadData();
				}
			});
			break;
		}
		case "back":
			base.SwitchTo(DialogType.Area, string.Empty);
			break;
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x0400071E RID: 1822
	private List<AreaOverview> subAreas;

	// Token: 0x0400071F RID: 1823
	private List<AreaOverview> createdAreas;

	// Token: 0x04000720 RID: 1824
	private int subAreasPage;

	// Token: 0x04000721 RID: 1825
	private int createdAreasPage;

	// Token: 0x04000722 RID: 1826
	private const int maxPerPage = 3;
}
