using System;
using System.Collections;
using System.Diagnostics;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000087 RID: 135
internal class PhotonHandler : MonoBehaviour
{
	// Token: 0x06000463 RID: 1123 RVA: 0x000149C8 File Offset: 0x00012DC8
	protected void Awake()
	{
		if (PhotonHandler.SP != null && PhotonHandler.SP != this && PhotonHandler.SP.gameObject != null)
		{
			global::UnityEngine.Object.DestroyImmediate(PhotonHandler.SP.gameObject);
		}
		PhotonHandler.SP = this;
		global::UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.updateInterval = 1000 / PhotonNetwork.sendRate;
		this.updateIntervalOnSerialize = 1000 / PhotonNetwork.sendRateOnSerialize;
		PhotonHandler.StartFallbackSendAckThread();
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x00014A51 File Offset: 0x00012E51
	protected void Start()
	{
		SceneManager.sceneLoaded += delegate(Scene scene, LoadSceneMode loadingMode)
		{
			PhotonNetwork.networkingPeer.NewSceneLoaded();
			PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(SceneManagerHelper.ActiveSceneName);
		};
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x00014A75 File Offset: 0x00012E75
	protected void OnApplicationQuit()
	{
		PhotonHandler.AppQuits = true;
		PhotonHandler.StopFallbackSendAckThread();
		PhotonNetwork.Disconnect();
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x00014A88 File Offset: 0x00012E88
	protected void OnApplicationPause(bool pause)
	{
		if (PhotonNetwork.BackgroundTimeout > 0.1f)
		{
			if (PhotonHandler.timerToStopConnectionInBackground == null)
			{
				PhotonHandler.timerToStopConnectionInBackground = new Stopwatch();
			}
			PhotonHandler.timerToStopConnectionInBackground.Reset();
			if (pause)
			{
				PhotonHandler.timerToStopConnectionInBackground.Start();
			}
			else
			{
				PhotonHandler.timerToStopConnectionInBackground.Stop();
			}
		}
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x00014AE1 File Offset: 0x00012EE1
	protected void OnDestroy()
	{
		PhotonHandler.StopFallbackSendAckThread();
	}

	// Token: 0x06000468 RID: 1128 RVA: 0x00014AE8 File Offset: 0x00012EE8
	protected void Update()
	{
		if (PhotonNetwork.networkingPeer == null)
		{
			global::UnityEngine.Debug.LogError("NetworkPeer broke!");
			return;
		}
		if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated || PhotonNetwork.connectionStateDetailed == ClientState.Disconnected || PhotonNetwork.offlineMode)
		{
			return;
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			return;
		}
		bool flag = true;
		while (PhotonNetwork.isMessageQueueRunning && flag)
		{
			flag = PhotonNetwork.networkingPeer.DispatchIncomingCommands();
		}
		int num = (int)(Time.realtimeSinceStartup * 1000f);
		if (PhotonNetwork.isMessageQueueRunning && num > this.nextSendTickCountOnSerialize)
		{
			PhotonNetwork.networkingPeer.RunViewUpdate();
			this.nextSendTickCountOnSerialize = num + this.updateIntervalOnSerialize;
			this.nextSendTickCount = 0;
		}
		num = (int)(Time.realtimeSinceStartup * 1000f);
		if (num > this.nextSendTickCount)
		{
			bool flag2 = true;
			while (PhotonNetwork.isMessageQueueRunning && flag2)
			{
				flag2 = PhotonNetwork.networkingPeer.SendOutgoingCommands();
			}
			this.nextSendTickCount = num + this.updateInterval;
		}
	}

	// Token: 0x06000469 RID: 1129 RVA: 0x00014BE4 File Offset: 0x00012FE4
	protected void OnJoinedRoom()
	{
		PhotonNetwork.networkingPeer.LoadLevelIfSynced();
	}

	// Token: 0x0600046A RID: 1130 RVA: 0x00014BF0 File Offset: 0x00012FF0
	protected void OnCreatedRoom()
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(SceneManagerHelper.ActiveSceneName);
	}

	// Token: 0x0600046B RID: 1131 RVA: 0x00014C01 File Offset: 0x00013001
	public static void StartFallbackSendAckThread()
	{
		if (PhotonHandler.sendThreadShouldRun)
		{
			return;
		}
		PhotonHandler.sendThreadShouldRun = true;
		SupportClass.StartBackgroundCalls(new Func<bool>(PhotonHandler.FallbackSendAckThread), 100, string.Empty);
	}

	// Token: 0x0600046C RID: 1132 RVA: 0x00014C3E File Offset: 0x0001303E
	public static void StopFallbackSendAckThread()
	{
		PhotonHandler.sendThreadShouldRun = false;
	}

	// Token: 0x0600046D RID: 1133 RVA: 0x00014C48 File Offset: 0x00013048
	public static bool FallbackSendAckThread()
	{
		if (PhotonHandler.sendThreadShouldRun && !PhotonNetwork.offlineMode && PhotonNetwork.networkingPeer != null)
		{
			if (PhotonHandler.timerToStopConnectionInBackground != null && PhotonNetwork.BackgroundTimeout > 0.1f && (float)PhotonHandler.timerToStopConnectionInBackground.ElapsedMilliseconds > PhotonNetwork.BackgroundTimeout * 1000f)
			{
				if (PhotonNetwork.connected)
				{
					PhotonNetwork.Disconnect();
				}
				PhotonHandler.timerToStopConnectionInBackground.Stop();
				PhotonHandler.timerToStopConnectionInBackground.Reset();
				return PhotonHandler.sendThreadShouldRun;
			}
			if (!PhotonNetwork.isMessageQueueRunning || PhotonNetwork.networkingPeer.ConnectionTime - PhotonNetwork.networkingPeer.LastSendOutgoingTime > 200)
			{
				PhotonNetwork.networkingPeer.SendAcksOnly();
			}
		}
		return PhotonHandler.sendThreadShouldRun;
	}

	// Token: 0x170000B3 RID: 179
	// (get) Token: 0x0600046E RID: 1134 RVA: 0x00014D0C File Offset: 0x0001310C
	// (set) Token: 0x0600046F RID: 1135 RVA: 0x00014D3E File Offset: 0x0001313E
	internal static CloudRegionCode BestRegionCodeInPreferences
	{
		get
		{
			string @string = PlayerPrefs.GetString("PUNCloudBestRegion", string.Empty);
			if (!string.IsNullOrEmpty(@string))
			{
				return Region.Parse(@string);
			}
			return CloudRegionCode.none;
		}
		set
		{
			if (value == CloudRegionCode.none)
			{
				PlayerPrefs.DeleteKey("PUNCloudBestRegion");
			}
			else
			{
				PlayerPrefs.SetString("PUNCloudBestRegion", value.ToString());
			}
		}
	}

	// Token: 0x06000470 RID: 1136 RVA: 0x00014D6D File Offset: 0x0001316D
	protected internal static void PingAvailableRegionsAndConnectToBest()
	{
		PhotonHandler.SP.StartCoroutine(PhotonHandler.SP.PingAvailableRegionsCoroutine(true));
	}

	// Token: 0x06000471 RID: 1137 RVA: 0x00014D88 File Offset: 0x00013188
	internal IEnumerator PingAvailableRegionsCoroutine(bool connectToBest)
	{
		while (PhotonNetwork.networkingPeer.AvailableRegions == null)
		{
			if (PhotonNetwork.connectionStateDetailed != ClientState.ConnectingToNameServer && PhotonNetwork.connectionStateDetailed != ClientState.ConnectedToNameServer)
			{
				global::UnityEngine.Debug.LogError("Call ConnectToNameServer to ping available regions.");
				yield break;
			}
			global::UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Waiting for AvailableRegions. State: ",
				PhotonNetwork.connectionStateDetailed,
				" Server: ",
				PhotonNetwork.Server,
				" PhotonNetwork.networkingPeer.AvailableRegions ",
				PhotonNetwork.networkingPeer.AvailableRegions != null
			}));
			yield return new WaitForSeconds(0.25f);
		}
		if (PhotonNetwork.networkingPeer.AvailableRegions == null || PhotonNetwork.networkingPeer.AvailableRegions.Count == 0)
		{
			global::UnityEngine.Debug.LogError("No regions available. Are you sure your appid is valid and setup?");
			yield break;
		}
		PhotonPingManager pingManager = new PhotonPingManager();
		foreach (Region region in PhotonNetwork.networkingPeer.AvailableRegions)
		{
			PhotonHandler.SP.StartCoroutine(pingManager.PingSocket(region));
		}
		while (!pingManager.Done)
		{
			yield return new WaitForSeconds(0.1f);
		}
		Region best = pingManager.BestRegion;
		PhotonHandler.BestRegionCodeInPreferences = best.Code;
		global::UnityEngine.Debug.Log(string.Concat(new object[] { "Found best region: '", best.Code, "' ping: ", best.Ping, ". Calling ConnectToRegionMaster() is: ", connectToBest }));
		if (connectToBest)
		{
			PhotonNetwork.networkingPeer.ConnectToRegionMaster(best.Code);
		}
		yield break;
	}

	// Token: 0x0400036F RID: 879
	public static PhotonHandler SP;

	// Token: 0x04000370 RID: 880
	public int updateInterval;

	// Token: 0x04000371 RID: 881
	public int updateIntervalOnSerialize;

	// Token: 0x04000372 RID: 882
	private int nextSendTickCount;

	// Token: 0x04000373 RID: 883
	private int nextSendTickCountOnSerialize;

	// Token: 0x04000374 RID: 884
	private static bool sendThreadShouldRun;

	// Token: 0x04000375 RID: 885
	private static Stopwatch timerToStopConnectionInBackground;

	// Token: 0x04000376 RID: 886
	protected internal static bool AppQuits;

	// Token: 0x04000377 RID: 887
	protected internal static Type PingImplementation;

	// Token: 0x04000378 RID: 888
	private const string PlayerPrefsKey = "PUNCloudBestRegion";
}
