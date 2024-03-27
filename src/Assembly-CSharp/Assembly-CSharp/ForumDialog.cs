using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200010D RID: 269
public class ForumDialog : Dialog
{
	// Token: 0x060009A8 RID: 2472 RVA: 0x0003EF94 File Offset: 0x0003D394
	public void Start()
	{
		base.Init(base.gameObject, false, true, false);
		base.AddBackButton();
		base.AddCloseButton();
		base.AddGenericFindButton("search");
		base.AddDialogThingFundamentIfNeeded();
		Managers.forumManager.GetForum(Managers.forumManager.currentForumId, delegate(string reasonFailed, ForumData forum, List<ForumThreadData> stickyThreads, List<ForumThreadData> theseThreads)
		{
			if (this != null && !Managers.forumManager.HandleFailIfNeeded(reasonFailed))
			{
				Managers.forumManager.currentForumData = forum;
				this.rights = new ForumRights(forum);
				base.AddDialogThingFundamentIfNeeded();
				base.AddHeadline(forum.name, -370, -460, TextColor.Default, TextAlignment.Left, false);
				if (!this.rights.onlyModsCanRead || this.rights.isModerator)
				{
					theseThreads.InsertRange(0, stickyThreads);
					this.threads = theseThreads;
					this.maxPages = Mathf.CeilToInt((float)this.threads.Count / 14f);
					base.StartCoroutine(this.AddForumThreadsList());
					if (this.maxPages > 1)
					{
						this.backButton = base.AddModelButton("ButtonBack", "PreviousPage", null, -100, 900, false);
						this.backButton.SetActive(false);
						this.forwardButton = base.AddModelButton("ButtonForward", "NextPage", null, 100, 900, false);
					}
				}
				int num = 890;
				base.AddSeparator(0, num - 60, false);
				string text = "forumSettings";
				string text2 = null;
				string text3 = null;
				string text4 = "ButtonSmall";
				int num2 = -840;
				int num3 = num + 20;
				string text5 = "attributes";
				TextColor customDefaultTextColor = this.customDefaultTextColor;
				string dialogColor = Managers.forumManager.currentForumData.dialogColor;
				GameObject gameObject = base.AddButton(text, text2, text3, text4, num2, num3, text5, false, 1f, customDefaultTextColor, 1f, 0f, 0f, dialogColor, false, false, TextAlignment.Left, false, false);
				Transform transform = gameObject.transform.Find("Cube");
				transform.localScale = new Vector3(1f, 1f, 0.55f);
				if (!this.rights.onlyModsCanAddThreads || this.rights.isModerator)
				{
					base.AddModelButton("EditTextButton", "createForumThreadTitle", null, 675, num, false);
					base.AddLabel("new topic", 725, num - 5, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
				}
				Managers.achievementManager.RegisterAchievement(Achievement.VisitedAForum);
			}
		});
		base.AddTimeDisplay();
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x0003EFF4 File Offset: 0x0003D3F4
	private IEnumerator AddForumThreadsList()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int minI = this.page * 14;
		int maxI = Mathf.Min(minI + 14 - 1, this.threads.Count - 1);
		if (this.forwardButton != null)
		{
			this.forwardButton.SetActive(this.page < this.maxPages - 1);
		}
		if (this.backButton != null)
		{
			this.backButton.SetActive(this.page >= 1);
		}
		int row = 0;
		for (int i = minI; i <= maxI; i++)
		{
			ForumThreadData forumThreadData = this.threads[i];
			string text = Misc.Truncate(forumThreadData.title, 40, true);
			if (!string.IsNullOrEmpty(forumThreadData.titleClarification))
			{
				text = Misc.Truncate(forumThreadData.title, 14, true) + " [" + Misc.Truncate(forumThreadData.titleClarification, 19, true) + "]";
			}
			int num = -760 + row * 112;
			string dialogColor = Managers.forumManager.currentForumData.dialogColor;
			string text2 = "thread";
			string id = forumThreadData.id;
			string text3 = text;
			string text4 = "ButtonNoIconLong";
			int num2 = -400;
			int num3 = num;
			TextColor customDefaultTextColor = this.customDefaultTextColor;
			GameObject gameObject = base.AddButton(text2, id, text3, text4, num2, num3, null, false, 1f, customDefaultTextColor, 1f, 0f, 0f, dialogColor, false, false, TextAlignment.Left, false, false);
			if (forumThreadData.isSticky)
			{
				if (!string.IsNullOrEmpty(dialogColor))
				{
					Color color = Misc.ColorStringToColor(dialogColor);
					float num4;
					float num5;
					float num6;
					Color.RGBToHSV(color, out num4, out num5, out num6);
					num6 -= 0.2f;
					color = Color.HSVToRGB(Mathf.Clamp(num4, 0f, 1f), Mathf.Clamp(num5, 0f, 1f), Mathf.Clamp(num6, 0f, 1f));
					base.SetButtonColor(gameObject, color);
				}
				else
				{
					base.SetButtonColor(gameObject, new Color32(164, 194, 215, byte.MaxValue));
				}
			}
			if (forumThreadData.latestCommentDate != null)
			{
				string text5 = Misc.GetHowLongAgoText(forumThreadData.latestCommentDate) + " by " + Misc.Truncate(forumThreadData.latestCommentUserName, 20, true);
				if (forumThreadData.isSticky)
				{
					text5 = "[sticky] " + text5;
				}
				base.AddLabel(text5, 175, num - 20, 0.8f, false, TextColor.Default, false, TextAlignment.Left, -1, 1f, false, TextAnchor.MiddleLeft);
			}
			row++;
		}
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x060009AA RID: 2474 RVA: 0x0003F00F File Offset: 0x0003D40F
	private new void Update()
	{
		base.Update();
		if (Misc.CtrlIsPressed() && Input.GetKeyDown(KeyCode.M))
		{
			base.SwitchTo(DialogType.ForumSettings, string.Empty);
		}
	}

	// Token: 0x060009AB RID: 2475 RVA: 0x0003F03C File Offset: 0x0003D43C
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		switch (contextName)
		{
		case "thread":
			Managers.forumManager.currentForumThreadId = contextId;
			base.SwitchTo(DialogType.ForumThread, string.Empty);
			break;
		case "createForumThreadTitle":
			Managers.dialogManager.GetInput(delegate(string threadTitle)
			{
				if (string.IsNullOrEmpty(threadTitle))
				{
					base.SwitchTo(DialogType.Forum, string.Empty);
				}
				else
				{
					Managers.dialogManager.GetInput(delegate(string initialComment)
					{
						if (string.IsNullOrEmpty(initialComment))
						{
							this.SwitchTo(DialogType.Forum, string.Empty);
						}
						else
						{
							string commentThingIdBeingCreated = Managers.forumManager.commentThingIdBeingCreated;
							Managers.forumManager.commentThingIdBeingCreated = null;
							Managers.forumManager.AddForumThread(Managers.forumManager.currentForumId, threadTitle, initialComment, commentThingIdBeingCreated, delegate(string reasonFailed, string threadId)
							{
								if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
								{
									Managers.forumManager.currentForumThreadId = threadId;
									Managers.forumManager.SetLatestCommentDateSeenToNow();
									this.SwitchTo(DialogType.ForumThread, string.Empty);
								}
							});
						}
					}, "createForumThreadInitialComment", string.Empty, 180, "comment", true, true, false, false, 1f, false, false, null, false);
				}
			}, contextName, string.Empty, 80, "topic", true, true, false, false, 1f, false, false, null, false);
			break;
		case "forumSettings":
			base.SwitchTo(DialogType.ForumSettings, string.Empty);
			break;
		case "PreviousPage":
			this.page--;
			base.StartCoroutine(this.AddForumThreadsList());
			break;
		case "NextPage":
			this.page++;
			base.StartCoroutine(this.AddForumThreadsList());
			break;
		case "search":
			base.SwitchTo(DialogType.ForumThreadSearchResult, string.Empty);
			break;
		case "back":
			base.SwitchTo(DialogType.Forums, string.Empty);
			break;
		case "close":
			base.CloseDialog();
			break;
		}
	}

	// Token: 0x0400073E RID: 1854
	private int page;

	// Token: 0x0400073F RID: 1855
	private const int threadsPerPage = 14;

	// Token: 0x04000740 RID: 1856
	private int maxPages = -1;

	// Token: 0x04000741 RID: 1857
	private List<ForumThreadData> threads;

	// Token: 0x04000742 RID: 1858
	private ForumRights rights;

	// Token: 0x04000743 RID: 1859
	private GameObject backButton;

	// Token: 0x04000744 RID: 1860
	private GameObject forwardButton;
}
