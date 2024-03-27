using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010E RID: 270
public class ForumsDialog : Dialog
{
	// Token: 0x060009AF RID: 2479 RVA: 0x0003F924 File Offset: 0x0003DD24
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddLabel("boards", -140, -450, 1.2f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
		Managers.forumManager.currentForumData = null;
		Managers.forumManager.GetFavoriteForums(delegate(string reasonFailed, List<ForumData> theseForums)
		{
			if (this != null && !Managers.forumManager.HandleFailIfNeeded(reasonFailed) && theseForums != null && theseForums.Count >= 1)
			{
				this.forums = theseForums;
				this.maxPages = Mathf.CeilToInt((float)this.forums.Count / 8f);
				Managers.forumManager.CacheThingIdsAndColors(this.forums);
				base.StartCoroutine(this.AddForumsList());
				if (this.maxPages > 1)
				{
					base.AddModelButton("ButtonForward", "NextPage", null, 80, 410, false);
				}
			}
		});
		base.AddSeparator(0, 340, false);
		base.AddButton("createForum", null, "Create board...", "ButtonCompact", -235, 410, "createForum", false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.AddModelButton("Find", "findForums", null, 380, 430, false);
		base.AddTimeDisplay();
	}

	// Token: 0x060009B0 RID: 2480 RVA: 0x0003FA14 File Offset: 0x0003DE14
	private IEnumerator AddForumsList()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		DateTime latestCommentDateSeen = Managers.forumManager.GetLatestCommentDateSeen();
		int minI = this.page * 8;
		int maxI = Mathf.Min(minI + 8 - 1, this.forums.Count - 1);
		int row = 0;
		for (int i = minI; i <= maxI; i++)
		{
			ForumData forumData = this.forums[i];
			string text = "★  " + Misc.Truncate(forumData.name, 25, true);
			int num = -300 + row * 80;
			string text2 = "forum";
			string id = forumData.id;
			string text3 = text;
			string text4 = "ButtonCompactNoIcon";
			int num2 = -235;
			int num3 = num;
			TextColor textColorForBackground = base.GetTextColorForBackground(forumData.dialogColor);
			GameObject gameObject = base.AddButton(text2, id, text3, text4, num2, num3, null, false, 1f, textColorForBackground, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
			if (!string.IsNullOrEmpty(forumData.dialogColor))
			{
				base.SetButtonColor(gameObject, Misc.ColorStringToColor(forumData.dialogColor));
			}
			if (forumData.latestCommentDate != null)
			{
				string text5 = Misc.GetHowLongAgoText(forumData.latestCommentDate);
				if (!string.IsNullOrEmpty(forumData.latestCommentUserName))
				{
					text5 = text5 + "\nby " + Misc.Truncate(forumData.latestCommentUserName, 25, true);
				}
				bool flag = DateTime.Parse(forumData.latestCommentDate) > latestCommentDateSeen;
				TextColor textColor = ((!flag) ? TextColor.Default : TextColor.White);
				base.AddLabel(text5, 40, num - 30, 0.6f, false, textColor, false, TextAlignment.Left, -1, 0.85f, false, TextAnchor.MiddleLeft);
			}
			row++;
		}
		base.SetUiWrapper(base.gameObject);
		Managers.forumManager.SetLatestCommentDateSeenToNow();
		yield break;
	}

	// Token: 0x060009B1 RID: 2481 RVA: 0x0003FA30 File Offset: 0x0003DE30
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "forum"))
			{
				if (!(contextName == "createForum"))
				{
					if (!(contextName == "NextPage"))
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
							}, contextName, string.Empty, 40, "find board", false, false, false, false, 1f, false, false, null, false);
						}
					}
					else
					{
						if (++this.page > this.maxPages - 1)
						{
							this.page = 0;
						}
						base.StartCoroutine(this.AddForumsList());
					}
				}
				else if (Managers.personManager.ourPerson.hasEditTools)
				{
					Managers.dialogManager.GetInput(delegate(string text)
					{
						if (string.IsNullOrEmpty(text))
						{
							base.SwitchTo(DialogType.Forums, string.Empty);
						}
						else
						{
							Managers.forumManager.CreateForum(text, string.Empty, delegate(string reasonFailed, string forumId)
							{
								if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
								{
									Managers.forumManager.ToggleFavoriteForum(forumId, delegate(string reasonFailed2, bool isFavorite)
									{
										if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed2))
										{
											Managers.forumManager.currentForumId = forumId;
											this.SwitchTo(DialogType.Forum, string.Empty);
										}
									});
								}
							});
						}
					}, contextName, string.Empty, 40, "board name", false, false, false, false, 1f, false, false, null, false);
				}
				else
				{
					base.SwitchTo(DialogType.GetEditTools, string.Empty);
				}
			}
			else
			{
				Managers.forumManager.currentForumId = contextId;
				base.SwitchTo(DialogType.Forum, string.Empty);
			}
		}
	}

	// Token: 0x04000746 RID: 1862
	private int page;

	// Token: 0x04000747 RID: 1863
	private const int forumsPerPage = 8;

	// Token: 0x04000748 RID: 1864
	private int maxPages = -1;

	// Token: 0x04000749 RID: 1865
	private List<ForumData> forums;
}
