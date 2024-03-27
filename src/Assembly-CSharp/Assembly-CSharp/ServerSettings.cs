using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000099 RID: 153
[Serializable]
public class ServerSettings : ScriptableObject
{
	// Token: 0x060005B5 RID: 1461 RVA: 0x00019EB5 File Offset: 0x000182B5
	public void UseCloudBestRegion(string cloudAppid)
	{
		this.HostType = ServerSettings.HostingOption.BestRegion;
		this.AppID = cloudAppid;
	}

	// Token: 0x060005B6 RID: 1462 RVA: 0x00019EC5 File Offset: 0x000182C5
	public void UseCloud(string cloudAppid)
	{
		this.HostType = ServerSettings.HostingOption.PhotonCloud;
		this.AppID = cloudAppid;
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x00019ED5 File Offset: 0x000182D5
	public void UseCloud(string cloudAppid, CloudRegionCode code)
	{
		this.HostType = ServerSettings.HostingOption.PhotonCloud;
		this.AppID = cloudAppid;
		this.PreferredRegion = code;
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x00019EEC File Offset: 0x000182EC
	public void UseMyServer(string serverAddress, int serverPort, string application)
	{
		this.HostType = ServerSettings.HostingOption.SelfHosted;
		this.AppID = ((application == null) ? "master" : application);
		this.ServerAddress = serverAddress;
		this.ServerPort = serverPort;
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x00019F1C File Offset: 0x0001831C
	public static bool IsAppId(string val)
	{
		try
		{
			new Guid(val);
		}
		catch
		{
			return false;
		}
		return true;
	}

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x060005BA RID: 1466 RVA: 0x00019F50 File Offset: 0x00018350
	public static CloudRegionCode BestRegionCodeInPreferences
	{
		get
		{
			return PhotonHandler.BestRegionCodeInPreferences;
		}
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x00019F57 File Offset: 0x00018357
	public static void ResetBestRegionCodeInPreferences()
	{
		PhotonHandler.BestRegionCodeInPreferences = CloudRegionCode.none;
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x00019F5F File Offset: 0x0001835F
	public override string ToString()
	{
		return string.Concat(new object[] { "ServerSettings: ", this.HostType, " ", this.ServerAddress });
	}

	// Token: 0x040003F9 RID: 1017
	public string AppID = string.Empty;

	// Token: 0x040003FA RID: 1018
	public string VoiceAppID = string.Empty;

	// Token: 0x040003FB RID: 1019
	public string ChatAppID = string.Empty;

	// Token: 0x040003FC RID: 1020
	public ServerSettings.HostingOption HostType;

	// Token: 0x040003FD RID: 1021
	public CloudRegionCode PreferredRegion;

	// Token: 0x040003FE RID: 1022
	public CloudRegionFlag EnabledRegions = (CloudRegionFlag)(-1);

	// Token: 0x040003FF RID: 1023
	public ConnectionProtocol Protocol;

	// Token: 0x04000400 RID: 1024
	public string ServerAddress = string.Empty;

	// Token: 0x04000401 RID: 1025
	public int ServerPort = 5055;

	// Token: 0x04000402 RID: 1026
	public int VoiceServerPort = 5055;

	// Token: 0x04000403 RID: 1027
	public bool JoinLobby;

	// Token: 0x04000404 RID: 1028
	public bool EnableLobbyStatistics;

	// Token: 0x04000405 RID: 1029
	public PhotonLogLevel PunLogging;

	// Token: 0x04000406 RID: 1030
	public DebugLevel NetworkLogging = DebugLevel.ERROR;

	// Token: 0x04000407 RID: 1031
	public bool RunInBackground = true;

	// Token: 0x04000408 RID: 1032
	public List<string> RpcList = new List<string>();

	// Token: 0x04000409 RID: 1033
	[HideInInspector]
	public bool DisableAutoOpenWizard;

	// Token: 0x0200009A RID: 154
	public enum HostingOption
	{
		// Token: 0x0400040B RID: 1035
		NotSet,
		// Token: 0x0400040C RID: 1036
		PhotonCloud,
		// Token: 0x0400040D RID: 1037
		SelfHosted,
		// Token: 0x0400040E RID: 1038
		OfflineMode,
		// Token: 0x0400040F RID: 1039
		BestRegion
	}
}
