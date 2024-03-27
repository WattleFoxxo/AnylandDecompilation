using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;

// Token: 0x020001F8 RID: 504
public class QuestManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000229 RID: 553
	// (get) Token: 0x0600127F RID: 4735 RVA: 0x0009C41E File Offset: 0x0009A81E
	// (set) Token: 0x06001280 RID: 4736 RVA: 0x0009C426 File Offset: 0x0009A826
	public ManagerStatus status { get; private set; }

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06001281 RID: 4737 RVA: 0x0009C42F File Offset: 0x0009A82F
	// (set) Token: 0x06001282 RID: 4738 RVA: 0x0009C437 File Offset: 0x0009A837
	public string failMessage { get; private set; }

	// Token: 0x06001283 RID: 4739 RVA: 0x0009C440 File Offset: 0x0009A840
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.jsonFilePath = Application.persistentDataPath + "/quests.json";
		this.LoadQuests();
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001284 RID: 4740 RVA: 0x0009C46C File Offset: 0x0009A86C
	private void LoadQuests()
	{
		if (File.Exists(this.jsonFilePath))
		{
			string text = File.ReadAllText(this.jsonFilePath);
			JSONNode jsonnode = JSON.Parse(text);
			if (jsonnode != null)
			{
				int count = jsonnode["quests"].Count;
				for (int i = 0; i < count; i++)
				{
					JSONNode jsonnode2 = jsonnode["quests"][i];
					Quest quest = new Quest();
					quest.SetFromJson(jsonnode2);
					this.quests.Add(quest);
				}
			}
		}
	}

	// Token: 0x06001285 RID: 4741 RVA: 0x0009C4FC File Offset: 0x0009A8FC
	private void SaveQuests()
	{
		if (this.quests.Count >= 1)
		{
			string text = string.Empty;
			foreach (Quest quest in this.quests)
			{
				if (text != string.Empty)
				{
					text += ",";
				}
				text += quest.GetJson();
			}
			text = "{\"quests\":[" + text + "]}";
			File.WriteAllText(this.jsonFilePath, text);
		}
		else if (File.Exists(this.jsonFilePath))
		{
			File.Delete(this.jsonFilePath);
		}
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x0009C5D0 File Offset: 0x0009A9D0
	public void AddQuest(Quest quest)
	{
		if (!this.QuestExists(quest))
		{
			this.quests.Add(quest);
			if (this.quests.Count > 1000)
			{
				this.quests.RemoveAt(0);
			}
			this.SaveQuests();
		}
		else
		{
			Debug.Log("Ignored adding quest " + quest.name + " as it exists already.");
		}
	}

	// Token: 0x06001287 RID: 4743 RVA: 0x0009C63C File Offset: 0x0009AA3C
	public bool QuestExists(Quest quest)
	{
		bool flag = false;
		if (quest != null)
		{
			foreach (Quest quest2 in this.quests)
			{
				if (quest2.areaName == quest.areaName && quest2.name == quest.name)
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x06001288 RID: 4744 RVA: 0x0009C6D0 File Offset: 0x0009AAD0
	public void HandleQuestAction(QuestAction questAction)
	{
		int questIndex = this.GetQuestIndex(Managers.areaManager.currentAreaName, questAction.questName);
		if (questIndex >= 0)
		{
			Quest quest = this.quests[questIndex];
			QuestActionType actionType = questAction.actionType;
			if (actionType != QuestActionType.Achieve)
			{
				if (actionType != QuestActionType.Unachieve)
				{
					if (actionType == QuestActionType.Remove)
					{
						this.quests.RemoveAt(questIndex);
						this.SaveQuests();
					}
				}
				else
				{
					quest.achieved = false;
					this.SaveQuests();
				}
			}
			else if (!quest.achieved)
			{
				Managers.forumManager.AddForumComment(quest.forumThreadId, "✓ achieved", null, delegate(string reasonFailed)
				{
					if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
					{
						quest.achieved = true;
						this.SaveQuests();
						this.StartCoroutine(this.OpenForumThreadForQuestIfNeeded(quest));
					}
				});
			}
			else
			{
				base.StartCoroutine(this.OpenForumThreadForQuestIfNeeded(quest));
			}
		}
	}

	// Token: 0x06001289 RID: 4745 RVA: 0x0009C7BC File Offset: 0x0009ABBC
	private IEnumerator OpenForumThreadForQuestIfNeeded(Quest quest)
	{
		if (!this.currentlyOpeningForumThread)
		{
			this.currentlyOpeningForumThread = true;
			yield return new WaitForSeconds(1f);
			if (Managers.dialogManager.GetCurrentNonStartDialogType() != DialogType.ForumThread || !(Managers.forumManager.currentForumId == quest.forumId) || !(Managers.forumManager.currentForumThreadId == quest.forumThreadId))
			{
				Managers.forumManager.currentForumId = quest.forumId;
				Managers.forumManager.currentForumThreadId = quest.forumThreadId;
				Managers.forumManager.GetForum(quest.forumId, delegate(string reasonFailed, ForumData forum, List<ForumThreadData> stickyThreads, List<ForumThreadData> theseThreads)
				{
					this.currentlyOpeningForumThread = false;
					if (!Managers.forumManager.HandleFailIfNeeded(reasonFailed))
					{
						Managers.forumManager.currentForumData = forum;
						Managers.dialogManager.SwitchToNewDialog(DialogType.ForumThread, null, string.Empty);
						Managers.soundManager.Play("success", null, 0.2f, false, false);
					}
				});
			}
			else
			{
				this.currentlyOpeningForumThread = false;
			}
		}
		yield break;
	}

	// Token: 0x0600128A RID: 4746 RVA: 0x0009C7E0 File Offset: 0x0009ABE0
	private int GetQuestIndex(string areaName, string name)
	{
		int num = -1;
		for (int i = 0; i < this.quests.Count; i++)
		{
			Quest quest = this.quests[i];
			if (quest.areaName == areaName && quest.name == name)
			{
				num = i;
				break;
			}
		}
		return num;
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x0009C844 File Offset: 0x0009AC44
	private void DebugShowData()
	{
		foreach (Quest quest in this.quests)
		{
			Debug.Log(quest.GetJson());
		}
	}

	// Token: 0x04001175 RID: 4469
	private List<Quest> quests = new List<Quest>();

	// Token: 0x04001176 RID: 4470
	private string jsonFilePath = string.Empty;

	// Token: 0x04001177 RID: 4471
	private bool currentlyOpeningForumThread;

	// Token: 0x04001178 RID: 4472
	private const int maxQuests = 1000;
}
