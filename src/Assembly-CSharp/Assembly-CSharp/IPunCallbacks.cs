using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

// Token: 0x0200007E RID: 126
public interface IPunCallbacks
{
	// Token: 0x060003FE RID: 1022
	void OnConnectedToPhoton();

	// Token: 0x060003FF RID: 1023
	void OnLeftRoom();

	// Token: 0x06000400 RID: 1024
	void OnMasterClientSwitched(PhotonPlayer newMasterClient);

	// Token: 0x06000401 RID: 1025
	void OnPhotonCreateRoomFailed(object[] codeAndMsg);

	// Token: 0x06000402 RID: 1026
	void OnPhotonJoinRoomFailed(object[] codeAndMsg);

	// Token: 0x06000403 RID: 1027
	void OnCreatedRoom();

	// Token: 0x06000404 RID: 1028
	void OnJoinedLobby();

	// Token: 0x06000405 RID: 1029
	void OnLeftLobby();

	// Token: 0x06000406 RID: 1030
	void OnFailedToConnectToPhoton(DisconnectCause cause);

	// Token: 0x06000407 RID: 1031
	void OnConnectionFail(DisconnectCause cause);

	// Token: 0x06000408 RID: 1032
	void OnDisconnectedFromPhoton();

	// Token: 0x06000409 RID: 1033
	void OnPhotonInstantiate(PhotonMessageInfo info);

	// Token: 0x0600040A RID: 1034
	void OnReceivedRoomListUpdate();

	// Token: 0x0600040B RID: 1035
	void OnJoinedRoom();

	// Token: 0x0600040C RID: 1036
	void OnPhotonPlayerConnected(PhotonPlayer newPlayer);

	// Token: 0x0600040D RID: 1037
	void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer);

	// Token: 0x0600040E RID: 1038
	void OnPhotonRandomJoinFailed(object[] codeAndMsg);

	// Token: 0x0600040F RID: 1039
	void OnConnectedToMaster();

	// Token: 0x06000410 RID: 1040
	void OnPhotonMaxCccuReached();

	// Token: 0x06000411 RID: 1041
	void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged);

	// Token: 0x06000412 RID: 1042
	void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps);

	// Token: 0x06000413 RID: 1043
	void OnUpdatedFriendList();

	// Token: 0x06000414 RID: 1044
	void OnCustomAuthenticationFailed(string debugMessage);

	// Token: 0x06000415 RID: 1045
	void OnCustomAuthenticationResponse(Dictionary<string, object> data);

	// Token: 0x06000416 RID: 1046
	void OnWebRpcResponse(OperationResponse response);

	// Token: 0x06000417 RID: 1047
	void OnOwnershipRequest(object[] viewAndPlayer);

	// Token: 0x06000418 RID: 1048
	void OnLobbyStatisticsUpdate();

	// Token: 0x06000419 RID: 1049
	void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer);

	// Token: 0x0600041A RID: 1050
	void OnOwnershipTransfered(object[] viewAndPlayers);
}
