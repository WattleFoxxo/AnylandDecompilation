using System;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x020001FE RID: 510
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000237 RID: 567
	// (get) Token: 0x0600134D RID: 4941 RVA: 0x000AD638 File Offset: 0x000ABA38
	// (set) Token: 0x0600134E RID: 4942 RVA: 0x000AD640 File Offset: 0x000ABA40
	public ManagerStatus status { get; private set; }

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x0600134F RID: 4943 RVA: 0x000AD649 File Offset: 0x000ABA49
	// (set) Token: 0x06001350 RID: 4944 RVA: 0x000AD651 File Offset: 0x000ABA51
	public string failMessage { get; private set; }

	// Token: 0x06001351 RID: 4945 RVA: 0x000AD65A File Offset: 0x000ABA5A
	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Log.Warning(pchDebugText.ToString());
	}

	// Token: 0x06001352 RID: 4946 RVA: 0x000AD668 File Offset: 0x000ABA68
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		if (Misc.ShouldDisableVR())
		{
			SteamVR.enabled = false;
			Log.Info("SteamManager Dissabling VR", false);
		}
		if (Misc.ShouldBypassAuth())
		{
			this.status = ManagerStatus.Started;
			return;
		}
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		if (!Packsize.Test())
		{
			Log.Error("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Log.Error("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
			{
				this.status = ManagerStatus.Failed;
				Log.Info("SteamManager restarting app", false);
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Log.Error("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
			this.failMessage = "Could not load steam library.";
			this.status = ManagerStatus.Failed;
			return;
		}
		Log.Info("Steam manager about to call steamAPI.Init", false);
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Log.Error("[Steamworks.NET] SteamAPI_Init() failed. IS STEAM STARTED? ARE YOU LOGGED INTO STEAM?.", this);
			this.failMessage = "Could not connect to steam - Is steam client running? Are you logged in?";
			this.status = ManagerStatus.Failed;
			return;
		}
		SteamManager.s_EverInitialized = true;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x000AD798 File Offset: 0x000ABB98
	private void OnEnable()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06001354 RID: 4948 RVA: 0x000AD7CE File Offset: 0x000ABBCE
	private void OnDestroy()
	{
		Log.Info(">> SteamManager OnDestroy", false);
		if (!this.m_bInitialized)
		{
			return;
		}
		this.CancelCurrentAuthSessionTicket();
		Log.Info(">> Shutting down steam (ondestroy)", false);
		SteamAPI.Shutdown();
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x000AD7FD File Offset: 0x000ABBFD
	private void Update()
	{
		if (this.status != ManagerStatus.Started)
		{
			return;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x000AD820 File Offset: 0x000ABC20
	public string GetAuthSessionTicket()
	{
		byte[] array = new byte[1024];
		this.CancelCurrentAuthSessionTicket();
		uint num;
		this.lastSteamAuthenticationTicketHandle = SteamUser.GetAuthSessionTicket(array, 1024, out num);
		Array.Resize<byte>(ref array, (int)num);
		string text = SteamManager.ByteArrayToString(array);
		if (text == null)
		{
			Log.Error("Failed to get auth session ticket from Steam");
		}
		else
		{
			Log.Info("GetAuthSessionTicket - " + text, false);
		}
		return text;
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x000AD889 File Offset: 0x000ABC89
	public void CancelCurrentAuthSessionTicket()
	{
		Log.Info("Cancelling authenticationSessionTicket", false);
		SteamUser.CancelAuthTicket(this.lastSteamAuthenticationTicketHandle);
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x000AD8A4 File Offset: 0x000ABCA4
	private static string ByteArrayToString(byte[] ba)
	{
		string text = BitConverter.ToString(ba);
		return text.Replace("-", string.Empty);
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x000AD8C8 File Offset: 0x000ABCC8
	public void ShutdownSteam()
	{
		Log.Info("ShutdownSteam", false);
		try
		{
			this.CancelCurrentAuthSessionTicket();
			SteamAPI.Shutdown();
		}
		catch (Exception ex)
		{
			Debug.Log(ex);
			Log.Info("Error during steam shutdown, probably not started", false);
		}
		SteamManager.s_EverInitialized = false;
	}

	// Token: 0x040011B4 RID: 4532
	public HAuthTicket lastSteamAuthenticationTicketHandle;

	// Token: 0x040011B5 RID: 4533
	private static bool s_EverInitialized;

	// Token: 0x040011B6 RID: 4534
	private bool m_bInitialized;

	// Token: 0x040011B7 RID: 4535
	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
