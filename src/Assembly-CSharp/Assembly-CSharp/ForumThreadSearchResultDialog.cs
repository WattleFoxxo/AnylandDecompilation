using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class ForumThreadSearchResultDialog : Dialog
{
	// Token: 0x060009D7 RID: 2519 RVA: 0x00042780 File Offset: 0x00040B80
	public void Start()
	{
		if (ForumThreadSearchResultDialog.threads == null || ForumThreadSearchResultDialog.searchQuery == null)
		{
			this.GetSearchInput();
			return;
		}
		base.Init(base.gameObject, false, true, false);
		base.AddDialogThingFundamentIfNeeded();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddGenericFindButton("search");
		this.maxPages = Mathf.CeilToInt((float)ForumThreadSearchResultDialog.threads.Count / 15f);
		string text = "Finding " + ForumThreadSearchResultDialog.searchQuery + " in " + Managers.forumManager.currentForumData.name;
		text = Misc.Truncate(text, 40, true);
		base.AddLabel(text, -800, -950, 1.5f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		if (this.maxPages > 1)
		{
			base.AddModelButton("ButtonForward", "NextPage", null, 80, 880, false);
		}
		if (ForumThreadSearchResultDialog.threads != null && ForumThreadSearchResultDialog.threads.Count >= 1)
		{
			base.StartCoroutine(this.AddResultList());
		}
		else
		{
			base.AddLabel("No threads found", 0, -90, 1.3f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x060009D8 RID: 2520 RVA: 0x000428B4 File Offset: 0x00040CB4
	public IEnumerator AddResultList()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int minI = this.page * 15;
		int maxI = Mathf.Min(minI + 15 - 1, ForumThreadSearchResultDialog.threads.Count - 1);
		int row = 0;
		for (int i = minI; i <= maxI; i++)
		{
			ForumThreadSummary forumThreadSummary = ForumThreadSearchResultDialog.threads[i];
			string text = Misc.Truncate(forumThreadSummary.title, 40, true);
			int num = -700 + row * 100;
			string text2 = "thread";
			string id = forumThreadSummary.id;
			string text3 = text;
			string text4 = "ButtonNoIconLong";
			int num2 = -400;
			int num3 = num;
			TextColor customDefaultTextColor = this.customDefaultTextColor;
			string dialogColor = Managers.forumManager.currentForumData.dialogColor;
			base.AddButton(text2, id, text3, text4, num2, num3, null, false, 1f, customDefaultTextColor, 1f, 0f, 0f, dialogColor, false, false, TextAlignment.Left, false, false);
			row++;
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x060009D9 RID: 2521 RVA: 0x000428D0 File Offset: 0x00040CD0
	private void GetSearchInput()
	{
		Managers.dialogManager.GetInput(delegate(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				this.ClearData();
				base.SwitchTo(DialogType.Forum, string.Empty);
			}
			else
			{
				Managers.forumManager.SearchForumThreads(Managers.forumManager.currentForumId, text, delegate(string error, List<ForumThreadSummary> theseThreads)
				{
					ForumThreadSearchResultDialog.threads = theseThreads;
					ForumThreadSearchResultDialog.searchQuery = text;
					this.SwitchTo(DialogType.ForumThreadSearchResult, string.Empty);
				});
			}
		}, string.Empty, string.Empty, 100, string.Empty, false, false, false, false, 1f, false, false, null, false);
	}

	// Token: 0x060009DA RID: 2522 RVA: 0x00042911 File Offset: 0x00040D11
	private void ClearData()
	{
		ForumThreadSearchResultDialog.threads = null;
		ForumThreadSearchResultDialog.searchQuery = null;
	}

	// Token: 0x060009DB RID: 2523 RVA: 0x00042920 File Offset: 0x00040D20
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "thread"))
			{
				if (!(contextName == "NextPage"))
				{
					if (!(contextName == "search"))
					{
						if (!(contextName == "back"))
						{
							if (contextName == "close")
							{
								this.ClearData();
								base.CloseDialog();
							}
						}
						else
						{
							this.ClearData();
							base.SwitchTo(DialogType.Forum, string.Empty);
						}
					}
					else
					{
						this.GetSearchInput();
					}
				}
				else
				{
					if (++this.page > this.maxPages - 1)
					{
						this.page = 0;
					}
					base.StartCoroutine(this.AddResultList());
				}
			}
			else
			{
				Managers.forumManager.currentForumThreadId = contextId;
				base.SwitchTo(DialogType.ForumThread, string.Empty);
			}
		}
	}

	// Token: 0x0400075E RID: 1886
	public static List<ForumThreadSummary> threads;

	// Token: 0x0400075F RID: 1887
	public static string searchQuery;

	// Token: 0x04000760 RID: 1888
	private int page;

	// Token: 0x04000761 RID: 1889
	private const int resultsPerPage = 15;

	// Token: 0x04000762 RID: 1890
	private int maxPages = -1;
}
