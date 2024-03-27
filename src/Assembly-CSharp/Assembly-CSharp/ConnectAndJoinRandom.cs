using System;
using Photon;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class ConnectAndJoinRandom : global::Photon.MonoBehaviour
{
	// Token: 0x060005FF RID: 1535 RVA: 0x0001BD6E File Offset: 0x0001A16E
	public virtual void Start()
	{
		PhotonNetwork.autoJoinLobby = false;
	}

	// Token: 0x06000600 RID: 1536 RVA: 0x0001BD78 File Offset: 0x0001A178
	public virtual void Update()
	{
		if (this.ConnectInUpdate && this.AutoConnect && !PhotonNetwork.connected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			this.ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings(this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
		}
	}

	// Token: 0x06000601 RID: 1537 RVA: 0x0001BDDB File Offset: 0x0001A1DB
	public virtual void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}

	// Token: 0x06000602 RID: 1538 RVA: 0x0001BDED File Offset: 0x0001A1ED
	public virtual void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x0001BE00 File Offset: 0x0001A200
	public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		PhotonNetwork.CreateRoom(null, new RoomOptions
		{
			MaxPlayers = 4
		}, null);
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x0001BE2D File Offset: 0x0001A22D
	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x0001BE44 File Offset: 0x0001A244
	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
	}

	// Token: 0x0400046F RID: 1135
	public bool AutoConnect = true;

	// Token: 0x04000470 RID: 1136
	public byte Version = 1;

	// Token: 0x04000471 RID: 1137
	private bool ConnectInUpdate = true;
}
