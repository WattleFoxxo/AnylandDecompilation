using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000110 RID: 272
public class ForumsSearchResultDialog : Dialog
{
	// Token: 0x060009BF RID: 2495 RVA: 0x00040618 File Offset: 0x0003EA18
	public void SetResults(List<ForumData> theseForums, string searchText)
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		Managers.forumManager.currentForumData = null;
		base.AddModelButton("Find", "findForums", null, 380, 430, false);
		base.AddSeparator(0, 340, false);
		this.lastSearchText = searchText;
		this.forums = theseForums;
		this.maxPages = Mathf.CeilToInt((float)this.forums.Count / 8f);
		base.AddLabel(searchText, -250, -420, 0.9f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		if (this.maxPages > 1)
		{
			base.AddModelButton("ButtonForward", "NextPage", null, 80, 410, false);
		}
		if (this.forums != null && this.forums.Count >= 1)
		{
			base.StartCoroutine(this.AddResultList());
		}
		else
		{
			base.AddLabel("No boards by this\nname were found", 0, -70, 0.825f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x00040740 File Offset: 0x0003EB40
	public IEnumerator AddResultList()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int minI = this.page * 8;
		int maxI = Mathf.Min(minI + 8 - 1, this.forums.Count - 1);
		int row = 0;
		for (int i = minI; i <= maxI; i++)
		{
			ForumData forumData = this.forums[i];
			string text = Misc.Truncate(forumData.name, 28, true);
			int num = -300 + row * 80;
			base.AddButton("forum", forumData.id, text, "ButtonCompactNoIcon", -235, num, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			if (!string.IsNullOrEmpty(forumData.description))
			{
				string text2 = Misc.WrapWithNewlines(forumData.description, 31, 2);
				base.AddLabel(text2, 30, num - 28, 0.55f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			}
			row++;
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x060009C1 RID: 2497 RVA: 0x0004075C File Offset: 0x0003EB5C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "NextPage"))
			{
				if (!(contextName == "forum"))
				{
					if (!(contextName == "findForums"))
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
							base.SwitchTo(DialogType.Main, string.Empty);
						}
					}
					else
					{
						Managers.dialogManager.GetInput(delegate(string text)
						{
							if (string.IsNullOrEmpty(text))
							{
								base.SwitchTo(DialogType.Forums, string.Empty);
							}
							else
							{
								Managers.forumManager.SearchForums(text, delegate(string reasonFailed, List<ForumData> forums)
								{
									if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
									{
										GameObject gameObject = this.SwitchTo(DialogType.ForumsSearchResult, string.Empty);
										ForumsSearchResultDialog component = gameObject.GetComponent<ForumsSearchResultDialog>();
										component.SetResults(forums, text);
									}
								});
							}
						}, contextName, this.lastSearchText, 40, "find board", false, false, false, false, 1f, false, false, null, false);
					}
				}
				else
				{
					Managers.forumManager.currentForumId = contextId;
					base.SwitchTo(DialogType.Forum, string.Empty);
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
	}

	// Token: 0x0400074B RID: 1867
	private int page;

	// Token: 0x0400074C RID: 1868
	private const int forumsPerPage = 8;

	// Token: 0x0400074D RID: 1869
	private int maxPages = -1;

	// Token: 0x0400074E RID: 1870
	private List<ForumData> forums;

	// Token: 0x0400074F RID: 1871
	private string lastSearchText = string.Empty;
}
