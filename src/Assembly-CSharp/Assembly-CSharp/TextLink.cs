using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x020001D3 RID: 467
public class TextLink
{
	// Token: 0x170001A9 RID: 425
	// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x000812BB File Offset: 0x0007F6BB
	// (set) Token: 0x06000EA4 RID: 3748 RVA: 0x000812C3 File Offset: 0x0007F6C3
	public TextLink.Type type { get; private set; }

	// Token: 0x170001AA RID: 426
	// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x000812CC File Offset: 0x0007F6CC
	// (set) Token: 0x06000EA6 RID: 3750 RVA: 0x000812D4 File Offset: 0x0007F6D4
	public string content { get; private set; }

	// Token: 0x170001AB RID: 427
	// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x000812DD File Offset: 0x0007F6DD
	// (set) Token: 0x06000EA8 RID: 3752 RVA: 0x000812E5 File Offset: 0x0007F6E5
	public string linkInner { get; private set; }

	// Token: 0x06000EA9 RID: 3753 RVA: 0x000812EE File Offset: 0x0007F6EE
	public bool TryParseText(string text)
	{
		return this.TryParse(text);
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x000812F7 File Offset: 0x0007F6F7
	public bool TryParseLinkInner(string linkInner)
	{
		return this.TryParse("[" + linkInner + "]");
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x00081310 File Offset: 0x0007F710
	public bool TryParseURL(string s)
	{
		if (s.IndexOf("youtube.com/") >= 0)
		{
			string text = "?v=";
			int num = s.IndexOf(text);
			if (num >= 0)
			{
				string text2 = s.Substring(num + text.Length);
				if (text2.Length >= 1 && Validator.ContainsOnly(text2, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -_'"))
				{
					this.type = TextLink.Type.youtube;
					this.content = text2;
					this.linkInner = this.type.ToString() + ": " + this.content;
				}
			}
		}
		else if (s.IndexOf("steamuserimages-a.akamaihd.net/") >= 0)
		{
			string text = "/ugc/";
			int num = s.IndexOf(text);
			if (num >= 0)
			{
				string text3 = s.Substring(num + text.Length);
				if (text3.Length >= 1 && Validator.ContainsOnly(text3, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -_'/"))
				{
					this.type = TextLink.Type.steamimage;
					this.content = text3;
					this.linkInner = this.type.ToString() + ": " + this.content;
				}
			}
		}
		else if (s.IndexOf("i.imgur.com/") >= 0)
		{
			string text = ".com/";
			int num = s.IndexOf(text);
			if (num >= 0)
			{
				string text4 = s.Substring(num + text.Length);
				if (text4.Length >= 1 && Validator.ContainsOnly(text4, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -_'."))
				{
					this.type = TextLink.Type.imgur;
					this.content = text4;
					this.linkInner = this.type.ToString() + ": " + this.content;
				}
			}
		}
		return this.type != TextLink.Type.none;
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x000814DA File Offset: 0x0007F8DA
	public bool IsImageType()
	{
		return this.type == TextLink.Type.steamimage || this.type == TextLink.Type.imgur;
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x000814F4 File Offset: 0x0007F8F4
	public string RemoveLink(string s)
	{
		return s.Replace(this.GetFullLink(), string.Empty);
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00081508 File Offset: 0x0007F908
	public string GetFullLink()
	{
		string text = string.Empty;
		if (this.type != TextLink.Type.none)
		{
			text = "[" + this.linkInner + "]";
		}
		return text;
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x00081540 File Offset: 0x0007F940
	public string GetButtonText()
	{
		string text = string.Empty;
		switch (this.type)
		{
		case TextLink.Type.thread:
			text = "Thread";
			goto IL_5F;
		case TextLink.Type.youtube:
			text = "Video";
			goto IL_5F;
		case TextLink.Type.quest:
			text = "Start quest";
			goto IL_5F;
		}
		text = this.content;
		IL_5F:
		return Misc.Truncate(text, 14, true);
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x000815B8 File Offset: 0x0007F9B8
	public TextColor GetButtonTextColor()
	{
		TextColor textColor = TextColor.Default;
		TextLink.Type type = this.type;
		if (type == TextLink.Type.quest)
		{
			Quest questFromContent = this.GetQuestFromContent();
			if (Managers.questManager.QuestExists(questFromContent))
			{
				textColor = TextColor.Green;
			}
		}
		return textColor;
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x000815FC File Offset: 0x0007F9FC
	public string GetFullUrl()
	{
		string text = string.Empty;
		if (!string.IsNullOrEmpty(this.content))
		{
			TextLink.Type type = this.type;
			if (type != TextLink.Type.steamimage)
			{
				if (type == TextLink.Type.imgur)
				{
					text = "https://i.imgur.com/" + this.content;
				}
			}
			else
			{
				text = "https://steamuserimages-a.akamaihd.net/ugc/" + this.content;
			}
		}
		return text;
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x00081668 File Offset: 0x0007FA68
	private bool TryParse(string text)
	{
		text = this.ReplaceEscapeSquareBrackets(text, false);
		text = text.Replace("[-> ", "[inForumSearch:");
		MatchCollection matchCollection = Regex.Matches(text, "\\[(.*?)\\]");
		if (matchCollection.Count >= 1)
		{
			string text2 = matchCollection[0].ToString().Substring(1, matchCollection[0].Length - 2);
			this.content = text2;
			this.linkInner = text2;
			if (BehaviorScriptParser.IsPlaceholderContent(text2))
			{
				this.type = TextLink.Type.none;
			}
			else if (text2.IndexOf(":") >= 1)
			{
				string[] array = text2.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length == 2)
				{
					try
					{
						this.type = (TextLink.Type)Enum.Parse(typeof(TextLink.Type), array[0]);
					}
					catch (Exception ex)
					{
						Debug.Log(ex);
						this.type = TextLink.Type.none;
					}
					this.content = array[1].Trim();
				}
			}
			else
			{
				this.type = TextLink.Type.area;
			}
		}
		bool flag = this.type != TextLink.Type.none && !string.IsNullOrEmpty(this.content) && !string.IsNullOrEmpty(this.linkInner);
		text = this.ReplaceEscapeSquareBrackets(text, true);
		return flag;
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x000817B8 File Offset: 0x0007FBB8
	private string ReplaceEscapeSquareBrackets(string text, bool reverse = false)
	{
		if (reverse)
		{
			text = text.Replace("ESCAPED_START", "[[");
			text = text.Replace("ESCAPED_END", "]]");
		}
		else
		{
			text = text.Replace("[[", "ESCAPED_START");
			text = text.Replace("]]", "ESCAPED_END");
		}
		return text;
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x0008181C File Offset: 0x0007FC1C
	public string GetIconName()
	{
		string text = null;
		switch (this.type)
		{
		case TextLink.Type.area:
		case TextLink.Type.thread:
		case TextLink.Type.quest:
			text = "teleportTo";
			break;
		case TextLink.Type.youtube:
			text = "play";
			break;
		}
		return text;
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x00081878 File Offset: 0x0007FC78
	public void Execute(Hand hand)
	{
		switch (this.type)
		{
		case TextLink.Type.area:
			Managers.areaManager.TryTransportToAreaByNameOrUrlName(this.content, string.Empty, false);
			break;
		case TextLink.Type.board:
			Managers.forumManager.OpenForumByName(hand, this.content, false);
			break;
		case TextLink.Type.thread:
			Managers.forumManager.OpenThreadById(hand, this.content, false);
			break;
		case TextLink.Type.youtube:
		{
			GameObject gameObject = hand.SwitchToNewDialog(DialogType.Video, string.Empty);
			VideoDialog component = gameObject.GetComponent<VideoDialog>();
			component.youTubeVideoId = this.content;
			break;
		}
		case TextLink.Type.inForumSearch:
			Managers.forumManager.SearchForumThreads(Managers.forumManager.currentForumId, this.content, delegate(string error, List<ForumThreadSummary> theseThreads)
			{
				ForumThreadSearchResultDialog.searchQuery = this.content;
				ForumThreadSearchResultDialog.threads = theseThreads;
				Managers.dialogManager.SwitchToNewDialog(DialogType.ForumThreadSearchResult, hand, string.Empty);
			});
			break;
		case TextLink.Type.quest:
		{
			Quest questFromContent = this.GetQuestFromContent();
			if (questFromContent != null)
			{
				Managers.questManager.AddQuest(questFromContent);
				Managers.soundManager.Play("success", hand.transform, 0.2f, false, false);
				Managers.dialogManager.CloseDialog();
				if (questFromContent.areaName != Managers.areaManager.currentAreaName)
				{
					Managers.areaManager.TryTransportToAreaByNameOrUrlName(questFromContent.areaName, string.Empty, false);
				}
			}
			else
			{
				Managers.soundManager.Play("no", hand.transform, 0.2f, false, false);
			}
			break;
		}
		}
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x00081A1C File Offset: 0x0007FE1C
	private Quest GetQuestFromContent()
	{
		Quest quest = null;
		string[] array = Misc.Split(this.content, "-", StringSplitOptions.RemoveEmptyEntries);
		if (array.Length == 2)
		{
			quest = new Quest();
			quest.areaName = array[0].Trim().ToLower();
			quest.name = array[1].Trim().ToLower();
			quest.forumId = Managers.forumManager.currentForumId;
			quest.forumThreadId = Managers.forumManager.currentForumThreadId;
		}
		return quest;
	}

	// Token: 0x04000F81 RID: 3969
	private const TextLink.Type defaultType = TextLink.Type.area;

	// Token: 0x04000F82 RID: 3970
	private const string charSetBasic = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -_'";

	// Token: 0x020001D4 RID: 468
	public enum Type
	{
		// Token: 0x04000F84 RID: 3972
		none,
		// Token: 0x04000F85 RID: 3973
		area,
		// Token: 0x04000F86 RID: 3974
		board,
		// Token: 0x04000F87 RID: 3975
		thread,
		// Token: 0x04000F88 RID: 3976
		youtube,
		// Token: 0x04000F89 RID: 3977
		steamimage,
		// Token: 0x04000F8A RID: 3978
		imgur,
		// Token: 0x04000F8B RID: 3979
		inForumSearch,
		// Token: 0x04000F8C RID: 3980
		quest
	}
}
