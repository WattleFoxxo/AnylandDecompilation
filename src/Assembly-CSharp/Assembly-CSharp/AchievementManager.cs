using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E0 RID: 480
public class AchievementManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001AF RID: 431
	// (get) Token: 0x06000F04 RID: 3844 RVA: 0x0008380B File Offset: 0x00081C0B
	// (set) Token: 0x06000F05 RID: 3845 RVA: 0x00083813 File Offset: 0x00081C13
	public ManagerStatus status { get; private set; }

	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x06000F06 RID: 3846 RVA: 0x0008381C File Offset: 0x00081C1C
	// (set) Token: 0x06000F07 RID: 3847 RVA: 0x00083824 File Offset: 0x00081C24
	public string failMessage { get; private set; }

	// Token: 0x06000F08 RID: 3848 RVA: 0x0008382D File Offset: 0x00081C2D
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06000F09 RID: 3849 RVA: 0x00083840 File Offset: 0x00081C40
	public void InitializeAchievements(List<Achievement> initialAchievements)
	{
		foreach (Achievement achievement in initialAchievements)
		{
			if (!this.achievements.ContainsKey(achievement))
			{
				this.achievements.Add(achievement, true);
			}
		}
		this.achievementsInitialized = true;
	}

	// Token: 0x06000F0A RID: 3850 RVA: 0x000838B8 File Offset: 0x00081CB8
	public bool DidAchieve(Achievement achievement)
	{
		return this.achievements.ContainsKey(achievement);
	}

	// Token: 0x06000F0B RID: 3851 RVA: 0x000838C8 File Offset: 0x00081CC8
	public void RegisterAchievement(Achievement achievement)
	{
		if (!this.achievements.ContainsKey(achievement))
		{
			this.achievements.Add(achievement, true);
			if (!this.achievementsWereClearedForTesting)
			{
				base.StartCoroutine(Managers.serverManager.RegisterAchievement(achievement, delegate(ExtendedServerResponse response)
				{
					if (!response.ok)
					{
						Log.Warning("Problem registering achievement");
					}
				}));
			}
		}
	}

	// Token: 0x06000F0C RID: 3852 RVA: 0x0008392D File Offset: 0x00081D2D
	public void ClearAchievementsLocallyAndTemporarilyForTesting()
	{
		this.achievementsWereClearedForTesting = true;
		this.achievements = new Dictionary<Achievement, bool>();
	}

	// Token: 0x04000FC9 RID: 4041
	private bool achievementsInitialized;

	// Token: 0x04000FCA RID: 4042
	private Dictionary<Achievement, bool> achievements = new Dictionary<Achievement, bool>();

	// Token: 0x04000FCB RID: 4043
	private bool achievementsWereClearedForTesting;
}
