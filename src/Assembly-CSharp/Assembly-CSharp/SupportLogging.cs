using System;
using System.Text;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class SupportLogging : MonoBehaviour
{
	// Token: 0x060006C4 RID: 1732 RVA: 0x0001F884 File Offset: 0x0001DC84
	public void Start()
	{
		if (this.LogTrafficStats)
		{
			base.InvokeRepeating("LogStats", 10f, 10f);
		}
	}

	// Token: 0x060006C5 RID: 1733 RVA: 0x0001F8A6 File Offset: 0x0001DCA6
	protected void OnApplicationPause(bool pause)
	{
		Debug.Log(string.Concat(new object[]
		{
			"SupportLogger OnApplicationPause: ",
			pause,
			" connected: ",
			PhotonNetwork.connected
		}));
	}

	// Token: 0x060006C6 RID: 1734 RVA: 0x0001F8DE File Offset: 0x0001DCDE
	public void OnApplicationQuit()
	{
		base.CancelInvoke();
	}

	// Token: 0x060006C7 RID: 1735 RVA: 0x0001F8E6 File Offset: 0x0001DCE6
	public void LogStats()
	{
		if (this.LogTrafficStats)
		{
			Debug.Log("SupportLogger " + PhotonNetwork.NetworkStatisticsToString());
		}
	}

	// Token: 0x060006C8 RID: 1736 RVA: 0x0001F908 File Offset: 0x0001DD08
	private void LogBasics()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat("SupportLogger Info: PUN {0}: ", "1.87");
		stringBuilder.AppendFormat("AppID: {0}*** GameVersion: {1} PeerId: {2} ", PhotonNetwork.networkingPeer.AppId.Substring(0, 8), PhotonNetwork.networkingPeer.AppVersion, PhotonNetwork.networkingPeer.PeerID);
		stringBuilder.AppendFormat("Server: {0}. Region: {1} ", PhotonNetwork.ServerAddress, PhotonNetwork.networkingPeer.CloudRegion);
		stringBuilder.AppendFormat("HostType: {0} ", PhotonNetwork.PhotonServerSettings.HostType);
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x060006C9 RID: 1737 RVA: 0x0001F9A3 File Offset: 0x0001DDA3
	public void OnConnectedToPhoton()
	{
		Debug.Log("SupportLogger OnConnectedToPhoton().");
		this.LogBasics();
		if (this.LogTrafficStats)
		{
			PhotonNetwork.NetworkStatisticsEnabled = true;
		}
	}

	// Token: 0x060006CA RID: 1738 RVA: 0x0001F9C6 File Offset: 0x0001DDC6
	public void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log("SupportLogger OnFailedToConnectToPhoton(" + cause + ").");
		this.LogBasics();
	}

	// Token: 0x060006CB RID: 1739 RVA: 0x0001F9E8 File Offset: 0x0001DDE8
	public void OnJoinedLobby()
	{
		Debug.Log("SupportLogger OnJoinedLobby(" + PhotonNetwork.lobby + ").");
	}

	// Token: 0x060006CC RID: 1740 RVA: 0x0001FA04 File Offset: 0x0001DE04
	public void OnJoinedRoom()
	{
		Debug.Log(string.Concat(new object[]
		{
			"SupportLogger OnJoinedRoom(",
			PhotonNetwork.room,
			"). ",
			PhotonNetwork.lobby,
			" GameServer:",
			PhotonNetwork.ServerAddress
		}));
	}

	// Token: 0x060006CD RID: 1741 RVA: 0x0001FA54 File Offset: 0x0001DE54
	public void OnCreatedRoom()
	{
		Debug.Log(string.Concat(new object[]
		{
			"SupportLogger OnCreatedRoom(",
			PhotonNetwork.room,
			"). ",
			PhotonNetwork.lobby,
			" GameServer:",
			PhotonNetwork.ServerAddress
		}));
	}

	// Token: 0x060006CE RID: 1742 RVA: 0x0001FAA1 File Offset: 0x0001DEA1
	public void OnLeftRoom()
	{
		Debug.Log("SupportLogger OnLeftRoom().");
	}

	// Token: 0x060006CF RID: 1743 RVA: 0x0001FAAD File Offset: 0x0001DEAD
	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("SupportLogger OnDisconnectedFromPhoton().");
	}

	// Token: 0x040004EF RID: 1263
	public bool LogTrafficStats;
}
