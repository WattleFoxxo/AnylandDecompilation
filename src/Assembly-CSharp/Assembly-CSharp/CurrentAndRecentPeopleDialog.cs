using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010B RID: 267
public class CurrentAndRecentPeopleDialog : Dialog
{
	// Token: 0x0600099B RID: 2459 RVA: 0x0003E578 File Offset: 0x0003C978
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		this.AddPeopleHereNowButtons();
		this.AddPeopleRecentlySeenButtons();
	}

	// Token: 0x0600099C RID: 2460 RVA: 0x0003E5A8 File Offset: 0x0003C9A8
	private void AddPeopleHereNowButtons()
	{
		List<PersonInfo> currentAreaPersonInfos = Managers.personManager.GetCurrentAreaPersonInfos(0);
		base.AddLabel("People here now (" + currentAreaPersonInfos.Count + ")", 0, -450, 1f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		this.AddPeople(currentAreaPersonInfos, -320);
	}

	// Token: 0x0600099D RID: 2461 RVA: 0x0003E608 File Offset: 0x0003CA08
	private void AddPeopleRecentlySeenButtons()
	{
		this.AddRecentPersonsButExcludePeopleHereNow();
		base.AddLabel("Other people you recently saw", 0, 200, 0.8f, false, TextColor.Default, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		if (this.recentPersons.Count > 2)
		{
			this.maxPages = Mathf.CeilToInt((float)this.recentPersons.Count / 2f);
			base.AddDefaultPagingButtons(80, 420, "Page", false, 0, 0.85f, false);
		}
		base.StartCoroutine(this.UpdateRecentPeopleButtons());
	}

	// Token: 0x0600099E RID: 2462 RVA: 0x0003E694 File Offset: 0x0003CA94
	private void AddRecentPersonsButExcludePeopleHereNow()
	{
		List<PersonInfo> currentAreaPersonInfos = Managers.personManager.GetCurrentAreaPersonInfos(0);
		foreach (PersonInfo personInfo in Managers.personManager.recentPersons)
		{
			bool flag = false;
			foreach (PersonInfo personInfo2 in currentAreaPersonInfos)
			{
				if (personInfo.id == personInfo2.id)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.recentPersons.Add(personInfo);
			}
		}
	}

	// Token: 0x0600099F RID: 2463 RVA: 0x0003E76C File Offset: 0x0003CB6C
	private IEnumerator UpdateRecentPeopleButtons()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int minI = this.page * 2;
		int maxI = Mathf.Min(minI + 2 - 1, this.recentPersons.Count - 1);
		List<PersonInfo> currentPagePersons = new List<PersonInfo>();
		for (int i = minI; i <= maxI; i++)
		{
			currentPagePersons.Add(this.recentPersons[i]);
		}
		this.AddPeople(currentPagePersons, 310);
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x060009A0 RID: 2464 RVA: 0x0003E788 File Offset: 0x0003CB88
	private void AddPeople(List<PersonInfo> persons, int offsetY)
	{
		int num = 0;
		int num2 = 0;
		if (persons.Count >= 1)
		{
			foreach (PersonInfo personInfo in persons)
			{
				string text = Managers.personManager.GetScreenNameWithDiscloser(personInfo.id, personInfo.screenName);
				text = Misc.Truncate(text, 25, true);
				base.AddButton("userInfo", personInfo.id, text, "ButtonCompactNoIcon", -225 + num * 450, offsetY + num2 * 80, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
				if (++num == 2)
				{
					num = 0;
					num2++;
				}
			}
		}
		else
		{
			base.AddLabel("...", 0, offsetY - 20, 1f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x060009A1 RID: 2465 RVA: 0x0003E894 File Offset: 0x0003CC94
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "userInfo"))
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
							base.SwitchTo(DialogType.Area, string.Empty);
						}
					}
					else
					{
						if (--this.page < 0)
						{
							this.page = this.maxPages - 1;
						}
						base.StartCoroutine(this.UpdateRecentPeopleButtons());
					}
				}
				else
				{
					if (++this.page >= this.maxPages)
					{
						this.page = 0;
					}
					base.StartCoroutine(this.UpdateRecentPeopleButtons());
				}
			}
			else
			{
				Our.personIdOfInterest = contextId;
				this.hand.lastContextInfoHit = null;
				base.SwitchTo(DialogType.Profile, string.Empty);
			}
		}
	}

	// Token: 0x04000734 RID: 1844
	private List<PersonInfo> recentPersons = new List<PersonInfo>();

	// Token: 0x04000735 RID: 1845
	private const int recentPersonsPerPage = 2;

	// Token: 0x04000736 RID: 1846
	private int page;

	// Token: 0x04000737 RID: 1847
	private int maxPages;
}
