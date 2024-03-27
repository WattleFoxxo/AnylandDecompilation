using System;
using UnityEngine;

// Token: 0x020001EB RID: 491
public class ErrorManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001DF RID: 479
	// (get) Token: 0x060010C8 RID: 4296 RVA: 0x00091A0C File Offset: 0x0008FE0C
	// (set) Token: 0x060010C9 RID: 4297 RVA: 0x00091A14 File Offset: 0x0008FE14
	public ManagerStatus status { get; private set; }

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x060010CA RID: 4298 RVA: 0x00091A1D File Offset: 0x0008FE1D
	// (set) Token: 0x060010CB RID: 4299 RVA: 0x00091A25 File Offset: 0x0008FE25
	public string failMessage { get; private set; }

	// Token: 0x060010CC RID: 4300 RVA: 0x00091A2E File Offset: 0x0008FE2E
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.inWorldErrorLogAlwaysOn = false;
		if (this.inWorldErrorLogAlwaysOn)
		{
			this.ShowInWorldErrorLog(true);
		}
		this.status = ManagerStatus.Started;
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x00091A57 File Offset: 0x0008FE57
	public void Update()
	{
		if (this.status != ManagerStatus.Started)
		{
			return;
		}
		if (this.goToErrorSceneNextFrame)
		{
			this.goToErrorSceneNextFrame = false;
			this.GoToErrorScene();
		}
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x00091A80 File Offset: 0x0008FE80
	private bool checkIfWorldIsDisplayed()
	{
		return Managers.serverManager.status == ManagerStatus.Started && Managers.steamManager.status == ManagerStatus.Started && Managers.areaManager.status == ManagerStatus.Started;
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x00091ABF File Offset: 0x0008FEBF
	public void BeepError()
	{
		if (Managers.soundManager != null)
		{
			Managers.soundManager.Play("no", null, 1f, false, false);
		}
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x00091AE8 File Offset: 0x0008FEE8
	public void ShowCriticalHaltError(string message, bool showBoilerplate = true, bool goStraightToErrorScene = false, bool haveErrorSceneReturnToUniverse = true, bool speedUpReturnToUniverse = false, bool allowClosingInsteadOfRestarting = false)
	{
		SteamVR_Fade.Start(Color.clear, 0f, false);
		Managers.pollManager.StopPolling();
		Log.Error(message);
		bool flag = !goStraightToErrorScene && this.checkIfWorldIsDisplayed();
		bool flag2 = !goStraightToErrorScene && !flag;
		if (flag)
		{
			Managers.dialogManager.ShowError(message, showBoilerplate, allowClosingInsteadOfRestarting, -1);
		}
		else
		{
			ErrorSceneData.Message = message;
			ErrorSceneData.ShowBoilerplate = showBoilerplate || flag2;
			ErrorSceneData.AutoReturnToUniverse = haveErrorSceneReturnToUniverse;
			ErrorSceneData.SpeedUpReturnToUniverse = speedUpReturnToUniverse;
			this.goToErrorSceneNextFrame = true;
		}
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x00091B77 File Offset: 0x0008FF77
	public void GoToErrorScene()
	{
		Log.Info("Loading Error Scene", false);
		Misc.CleanUpBeforeSceneChange();
		Application.LoadLevel("ErrorScene");
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x00091B94 File Offset: 0x0008FF94
	public void ShowMaintenanceMessage()
	{
		string text = "Sorry, Anyland is currently down for maintenance and will be back soon!";
		this.ShowCriticalHaltError(text, true, true, false, false, false);
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x00091BB4 File Offset: 0x0008FFB4
	public void ShowInWorldErrorLog(bool doShow)
	{
		if (doShow)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/LogDisplay", typeof(GameObject))) as GameObject;
			gameObject.transform.parent = this.ourHeadCore.transform;
		}
		else
		{
			Transform transform = this.ourHeadCore.transform.Find("LogDisplay");
			if (transform != null)
			{
				global::UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x040010C8 RID: 4296
	public bool inWorldErrorLogAlwaysOn;

	// Token: 0x040010C9 RID: 4297
	public GameObject ourHeadCore;

	// Token: 0x040010CA RID: 4298
	private bool goToErrorSceneNextFrame;
}
