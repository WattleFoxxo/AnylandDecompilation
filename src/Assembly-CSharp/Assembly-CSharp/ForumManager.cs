using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001EF RID: 495
public class ForumManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x060010F6 RID: 4342 RVA: 0x00092E5C File Offset: 0x0009125C
	// (set) Token: 0x060010F7 RID: 4343 RVA: 0x00092E64 File Offset: 0x00091264
	public ManagerStatus status { get; private set; }

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x060010F8 RID: 4344 RVA: 0x00092E6D File Offset: 0x0009126D
	// (set) Token: 0x060010F9 RID: 4345 RVA: 0x00092E75 File Offset: 0x00091275
	public string failMessage { get; private set; }

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x060010FA RID: 4346 RVA: 0x00092E7E File Offset: 0x0009127E
	// (set) Token: 0x060010FB RID: 4347 RVA: 0x00092E86 File Offset: 0x00091286
	public string currentForumId { get; set; }

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x060010FC RID: 4348 RVA: 0x00092E8F File Offset: 0x0009128F
	// (set) Token: 0x060010FD RID: 4349 RVA: 0x00092E97 File Offset: 0x00091297
	public string currentForumThreadId { get; set; }

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x060010FE RID: 4350 RVA: 0x00092EA0 File Offset: 0x000912A0
	// (set) Token: 0x060010FF RID: 4351 RVA: 0x00092EA8 File Offset: 0x000912A8
	public ForumData currentForumData { get; set; }

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x06001100 RID: 4352 RVA: 0x00092EB1 File Offset: 0x000912B1
	// (set) Token: 0x06001101 RID: 4353 RVA: 0x00092EB9 File Offset: 0x000912B9
	public Dictionary<string, string> defaultForumIds { get; private set; }

	// Token: 0x06001102 RID: 4354 RVA: 0x00092EC2 File Offset: 0x000912C2
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.InitDefaultForumIds();
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001103 RID: 4355 RVA: 0x00092ED8 File Offset: 0x000912D8
	private void InitDefaultForumIds()
	{
		this.defaultForumIds = new Dictionary<string, string>
		{
			{ "Help", "5846f540e8593a971395c0aa" },
			{ "Events", "5846f54d5a84a62410ce2e66" },
			{ "Quests", "5c43465c9e61d1567d9c69bd" },
			{ "Updates", "5846f556b09fa5d709e5f6fe" },
			{ "Showcase", "5846f567b09fa5d709e5f6ff" },
			{ "Suggestions", "5846f571c966811d10993e1e" },
			{ "Hangout", "5846f5785a84a62410ce2e67" }
		};
	}

	// Token: 0x06001104 RID: 4356 RVA: 0x00092F64 File Offset: 0x00091364
	public void CreateForum(string forumName, string forumDescription, Action<string, string> callback)
	{
		base.StartCoroutine(Managers.serverManager.CreateForum(forumName, forumDescription, delegate(CreateForum_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed, response.forumId);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001105 RID: 4357 RVA: 0x00092FA0 File Offset: 0x000913A0
	public void GetForum(string forumId, Action<string, ForumData, List<ForumThreadData>, List<ForumThreadData>> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetForum(forumId, delegate(GetForum_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed, response.forum, response.stickies, response.threads);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001106 RID: 4358 RVA: 0x00092FD8 File Offset: 0x000913D8
	public void GetForumInfo(string forumId, Action<string, ForumData> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetForum(forumId, delegate(GetForum_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed, response.forum);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001107 RID: 4359 RVA: 0x00093010 File Offset: 0x00091410
	public void EditForumInfo(string forumId, string forumDescription, Action<string> callback)
	{
		base.StartCoroutine(Managers.serverManager.EditForumInfo(forumId, forumDescription, delegate(EditForumInfo_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001108 RID: 4360 RVA: 0x0009304C File Offset: 0x0009144C
	public void GetForumIdFromName(string forumName, Action<string, string> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetForumIdFromName(forumName, delegate(GetForumIdFromName_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed, response.forumId);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001109 RID: 4361 RVA: 0x00093084 File Offset: 0x00091484
	public void GetForumThread(string threadId, Action<string, ForumData, ForumThreadData> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetForumThread(threadId, delegate(GetForumThread_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed, response.forum, response.thread);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x0600110A RID: 4362 RVA: 0x000930BC File Offset: 0x000914BC
	public void GetForumThreadInfo(string threadId, Action<string, ForumData, ForumThreadData> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetForumThread(threadId, delegate(GetForumThread_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed, response.forum, response.thread);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x0600110B RID: 4363 RVA: 0x000930F4 File Offset: 0x000914F4
	public void AddForumThread(string forumId, string threadTitle, string commentText, string optionalThingId, Action<string, string> callback)
	{
		base.StartCoroutine(Managers.serverManager.AddForumThread(forumId, threadTitle, commentText, optionalThingId, delegate(AddForumThread_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed, response.threadId);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x0600110C RID: 4364 RVA: 0x00093134 File Offset: 0x00091534
	public void RemoveForumThread(string threadId, Action<string> callback)
	{
		base.StartCoroutine(Managers.serverManager.RemoveForumThread(threadId, delegate(RemoveForumThread_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x0600110D RID: 4365 RVA: 0x0009316C File Offset: 0x0009156C
	public void AddForumComment(string threadId, string commentText, string optionalThingId, Action<string> callback)
	{
		base.StartCoroutine(Managers.serverManager.AddForumComment(threadId, commentText, optionalThingId, delegate(AddForumComment_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x000931A8 File Offset: 0x000915A8
	public void EditForumComment(string threadId, string commentDate, string newCommentText, Action<string> callback)
	{
		base.StartCoroutine(Managers.serverManager.EditForumComment(threadId, commentDate, newCommentText, delegate(EditForumComment_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x000931E4 File Offset: 0x000915E4
	public void RemoveForumComment(string threadId, string commentUserId, string commentDate, Action<string> callback)
	{
		base.StartCoroutine(Managers.serverManager.RemoveForumComment(threadId, commentUserId, commentDate, delegate(RemoveForumComment_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001110 RID: 4368 RVA: 0x00093220 File Offset: 0x00091620
	public void LikeForumComment(string threadId, string commentUserId, string commentDate, Action callback)
	{
		base.StartCoroutine(Managers.serverManager.LikeForumComment(threadId, commentUserId, commentDate, delegate(AcknowledgeOperation_Response response)
		{
			if (response.error == null)
			{
				callback();
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001111 RID: 4369 RVA: 0x0009325C File Offset: 0x0009165C
	public void ToggleForumThreadSticky(string threadId, Action<string, bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.ToggleForumThreadSticky(threadId, delegate(ToggleForumThreadSticky_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed, response.isSticky);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001112 RID: 4370 RVA: 0x00093294 File Offset: 0x00091694
	public void ToggleForumThreadLocked(string threadId, Action<string, bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.ToggleForumThreadLocked(threadId, delegate(ToggleForumThreadLocked_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed, response.isLocked);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001113 RID: 4371 RVA: 0x000932CC File Offset: 0x000916CC
	public void ClarifyForumThreadTitle(string threadId, string titleClarification, Action<string> callback)
	{
		base.StartCoroutine(Managers.serverManager.ClarifyForumThreadTitle(threadId, titleClarification, delegate(ClarifyForumThreadTitle_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001114 RID: 4372 RVA: 0x00093308 File Offset: 0x00091708
	public void GetFavoriteForums(Action<string, List<ForumData>> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetFavoriteForums(delegate(GetFavoriteForums_Response response)
		{
			if (response.error == null)
			{
				response.forums = this.GetFixedSortOrderForDefaultForums(response.forums);
				callback(null, response.forums);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001115 RID: 4373 RVA: 0x00093348 File Offset: 0x00091748
	private List<ForumData> GetFixedSortOrderForDefaultForums(List<ForumData> forums)
	{
		List<ForumData> list = new List<ForumData>();
		foreach (KeyValuePair<string, string> keyValuePair in this.defaultForumIds)
		{
			foreach (ForumData forumData in forums)
			{
				if (forumData.id == keyValuePair.Value)
				{
					list.Add(forumData);
					break;
				}
			}
		}
		foreach (ForumData forumData2 in forums)
		{
			if (!this.defaultForumIds.ContainsValue(forumData2.id))
			{
				list.Add(forumData2);
			}
		}
		return list;
	}

	// Token: 0x06001116 RID: 4374 RVA: 0x00093468 File Offset: 0x00091868
	public void ToggleFavoriteForum(string forumId, Action<string, bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.ToggleFavoriteForum(forumId, delegate(ToggleFavoriteForum_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed, response.isFavorited);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001117 RID: 4375 RVA: 0x000934A0 File Offset: 0x000918A0
	public void SearchForums(string searchTerm, Action<string, List<ForumData>> callback)
	{
		base.StartCoroutine(Managers.serverManager.SearchForums(searchTerm, delegate(SearchForums_Response response)
		{
			if (response.error == null)
			{
				this.AddDefaultForumMatches(response.forums, searchTerm);
				callback(null, response.forums);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001118 RID: 4376 RVA: 0x000934EC File Offset: 0x000918EC
	private void AddDefaultForumMatches(List<ForumData> forums, string searchTerm)
	{
		searchTerm = searchTerm.ToLower();
		foreach (KeyValuePair<string, string> keyValuePair in this.defaultForumIds)
		{
			string text = keyValuePair.Key.ToLower();
			string value = keyValuePair.Value;
			if (text.IndexOf(searchTerm) >= 0 && !this.IdExistsInForumDatas(forums, value))
			{
				forums.Insert(0, new ForumData
				{
					name = text,
					id = value,
					description = "A default forum"
				});
			}
		}
	}

	// Token: 0x06001119 RID: 4377 RVA: 0x000935A4 File Offset: 0x000919A4
	public bool NameExistsInForumDatas(List<ForumData> forums, string forumName)
	{
		bool flag = false;
		forumName = forumName.ToLower();
		foreach (ForumData forumData in forums)
		{
			if (forumData.name.ToLower() == forumName)
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x0600111A RID: 4378 RVA: 0x0009361C File Offset: 0x00091A1C
	private bool IdExistsInForumDatas(List<ForumData> forums, string forumId)
	{
		bool flag = false;
		foreach (ForumData forumData in forums)
		{
			if (forumData.id == forumId)
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x0600111B RID: 4379 RVA: 0x00093688 File Offset: 0x00091A88
	public void SearchForumThreads(string forumId, string searchTerm, Action<string, List<ForumThreadSummary>> callback)
	{
		base.StartCoroutine(Managers.serverManager.SearchForumThreads(forumId, searchTerm, delegate(SearchForumThreads_Response response)
		{
			if (response.error == null)
			{
				callback(null, response.threads);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x0600111C RID: 4380 RVA: 0x000936C4 File Offset: 0x00091AC4
	public void SetForumProtectionLevel(string forumId, int protectionLevel, Action<string> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetForumProtectionLevel(forumId, protectionLevel, delegate(SetForumProtectionLevel_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x0600111D RID: 4381 RVA: 0x00093700 File Offset: 0x00091B00
	public void SetForumDialog(string forumId, string thingId, string color, Action<string> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetForumDialog(forumId, thingId, color, delegate(SetForumDialog_Response response)
		{
			if (response.error == null)
			{
				callback(response.reasonFailed);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x0600111E RID: 4382 RVA: 0x0009373C File Offset: 0x00091B3C
	public bool HandleFailIfNeeded(string reasonFailed)
	{
		bool flag = !string.IsNullOrEmpty(reasonFailed);
		if (flag)
		{
			string text = string.Empty;
			if (reasonFailed != null)
			{
				if (reasonFailed == "NAME_TAKEN")
				{
					text = "This name is taken already";
					goto IL_98;
				}
				if (reasonFailed == "NOT_PERMITTED")
				{
					text = "This can't be done";
					goto IL_98;
				}
				if (reasonFailed == "BAD_THREAD")
				{
					text = "Something went wrong thread-wise";
					goto IL_98;
				}
				if (reasonFailed == "MODS_ONLY_FORUM")
				{
					text = "This board is for editors only";
					goto IL_98;
				}
			}
			text = "Something went wrong";
			IL_98:
			Managers.dialogManager.ShowInfo("Oops! " + text, false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
		}
		return flag;
	}

	// Token: 0x0600111F RID: 4383 RVA: 0x00093808 File Offset: 0x00091C08
	public void CacheThingIdsAndColors(List<ForumData> forums)
	{
		foreach (ForumData forumData in forums)
		{
			string id = forumData.id;
			if (!this.dialogThingIdsByForumId.ContainsKey(id))
			{
				this.dialogThingIdsByForumId.Add(id, forumData.dialogThingId);
			}
			if (!this.dialogColorsByForumId.ContainsKey(id))
			{
				this.dialogColorsByForumId.Add(id, forumData.dialogColor);
			}
		}
	}

	// Token: 0x06001120 RID: 4384 RVA: 0x000938A8 File Offset: 0x00091CA8
	public void UpdateCurrentThingIdAndColorCache()
	{
		string currentForumId = this.currentForumId;
		if (!string.IsNullOrEmpty(currentForumId))
		{
			if (this.dialogThingIdsByForumId.ContainsKey(currentForumId))
			{
				this.dialogThingIdsByForumId[currentForumId] = this.currentForumData.dialogThingId;
			}
			if (this.dialogColorsByForumId.ContainsKey(currentForumId))
			{
				this.dialogColorsByForumId[currentForumId] = this.currentForumData.dialogColor;
			}
		}
	}

	// Token: 0x06001121 RID: 4385 RVA: 0x00093918 File Offset: 0x00091D18
	public void OpenForumByName(Hand hand, string forumName, bool doHapticPulseWhenDone = false)
	{
		if (!this.isCurrentlyTryingToOpenForum)
		{
			this.isCurrentlyTryingToOpenForum = true;
			Managers.forumManager.GetForumIdFromName(forumName, delegate(string reasonFailed, string forumId)
			{
				if (string.IsNullOrEmpty(reasonFailed))
				{
					this.currentForumData = null;
					this.currentForumId = forumId;
					hand.SwitchToNewDialog(DialogType.Forum, string.Empty);
					if (doHapticPulseWhenDone)
					{
						hand.StartShrinkingHapticPulseOverTime();
					}
					this.isCurrentlyTryingToOpenForum = false;
				}
			});
		}
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x0009396C File Offset: 0x00091D6C
	public void OpenThreadById(Hand hand, string threadId, bool doHapticPulseWhenDone = false)
	{
		if (!this.isCurrentlyTryingToOpenForum)
		{
			this.isCurrentlyTryingToOpenForum = true;
			Managers.forumManager.GetForumThreadInfo(threadId, delegate(string reasonFailed, ForumData forum, ForumThreadData thread)
			{
				if (!this.HandleFailIfNeeded(reasonFailed))
				{
					this.currentForumData = forum;
					this.currentForumId = forum.id;
					this.currentForumThreadId = threadId;
					hand.SwitchToNewDialog(DialogType.ForumThread, string.Empty);
					if (doHapticPulseWhenDone)
					{
						hand.StartShrinkingHapticPulseOverTime();
					}
					this.isCurrentlyTryingToOpenForum = false;
				}
			});
		}
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x000939CC File Offset: 0x00091DCC
	public DateTime GetLatestCommentDateOfTheseForums(List<ForumData> forumDatas)
	{
		DateTime dateTime = DateTime.MinValue;
		foreach (ForumData forumData in forumDatas)
		{
			if (!string.IsNullOrEmpty(forumData.latestCommentDate))
			{
				DateTime dateTime2 = DateTime.Parse(forumData.latestCommentDate);
				if (dateTime2 > dateTime)
				{
					dateTime = dateTime2;
				}
			}
		}
		return dateTime;
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x00093A50 File Offset: 0x00091E50
	public DateTime GetLatestCommentDateSeen()
	{
		DateTime dateTime = DateTime.MinValue;
		string @string = PlayerPrefs.GetString("latestForumCommentDateSeen", null);
		if (!string.IsNullOrEmpty(@string))
		{
			dateTime = DateTime.Parse(@string);
		}
		return dateTime;
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x00093A84 File Offset: 0x00091E84
	public void SetLatestCommentDateSeenToNow()
	{
		this.didLastFindNewForumComments = false;
		PlayerPrefs.SetString("latestForumCommentDateSeen", DateTime.Now.ToString("o"));
	}

	// Token: 0x06001126 RID: 4390 RVA: 0x00093AB4 File Offset: 0x00091EB4
	public float MinutesSinceLastNewCommentsCheck()
	{
		float num = -1f;
		if (this.lastNewForumCommentsCheck != -1f)
		{
			float num2 = Time.time - this.lastNewForumCommentsCheck;
			num = ((num2 <= 0f) ? 0f : (num2 / 60f));
		}
		return num;
	}

	// Token: 0x040010FB RID: 4347
	public string commentThingIdBeingCreated;

	// Token: 0x040010FC RID: 4348
	public Dictionary<string, string> dialogThingIdsByForumId = new Dictionary<string, string>();

	// Token: 0x040010FD RID: 4349
	public Dictionary<string, string> dialogColorsByForumId = new Dictionary<string, string>();

	// Token: 0x040010FE RID: 4350
	public const int commentMaxLength = 180;

	// Token: 0x040010FF RID: 4351
	public const int maxCommentsPerThread = 250;

	// Token: 0x04001100 RID: 4352
	private bool isCurrentlyTryingToOpenForum;

	// Token: 0x04001101 RID: 4353
	public float lastNewForumCommentsCheck = -1f;

	// Token: 0x04001102 RID: 4354
	public bool didLastFindNewForumComments;

	// Token: 0x04001103 RID: 4355
	private const string latestCommentDateSeenKey = "latestForumCommentDateSeen";
}
