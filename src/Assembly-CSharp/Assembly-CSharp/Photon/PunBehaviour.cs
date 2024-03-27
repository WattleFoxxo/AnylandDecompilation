using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;

namespace Photon
{
	// Token: 0x02000081 RID: 129
	public class PunBehaviour : MonoBehaviour, IPunCallbacks
	{
		// Token: 0x06000420 RID: 1056 RVA: 0x00014262 File Offset: 0x00012662
		public virtual void OnConnectedToPhoton()
		{
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00014264 File Offset: 0x00012664
		public virtual void OnLeftRoom()
		{
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00014266 File Offset: 0x00012666
		public virtual void OnMasterClientSwitched(PhotonPlayer newMasterClient)
		{
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00014268 File Offset: 0x00012668
		public virtual void OnPhotonCreateRoomFailed(object[] codeAndMsg)
		{
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0001426A File Offset: 0x0001266A
		public virtual void OnPhotonJoinRoomFailed(object[] codeAndMsg)
		{
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001426C File Offset: 0x0001266C
		public virtual void OnCreatedRoom()
		{
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0001426E File Offset: 0x0001266E
		public virtual void OnJoinedLobby()
		{
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00014270 File Offset: 0x00012670
		public virtual void OnLeftLobby()
		{
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00014272 File Offset: 0x00012672
		public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
		{
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00014274 File Offset: 0x00012674
		public virtual void OnDisconnectedFromPhoton()
		{
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00014276 File Offset: 0x00012676
		public virtual void OnConnectionFail(DisconnectCause cause)
		{
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00014278 File Offset: 0x00012678
		public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
		{
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001427A File Offset: 0x0001267A
		public virtual void OnReceivedRoomListUpdate()
		{
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0001427C File Offset: 0x0001267C
		public virtual void OnJoinedRoom()
		{
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0001427E File Offset: 0x0001267E
		public virtual void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00014280 File Offset: 0x00012680
		public virtual void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00014282 File Offset: 0x00012682
		public virtual void OnPhotonRandomJoinFailed(object[] codeAndMsg)
		{
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00014284 File Offset: 0x00012684
		public virtual void OnConnectedToMaster()
		{
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00014286 File Offset: 0x00012686
		public virtual void OnPhotonMaxCccuReached()
		{
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00014288 File Offset: 0x00012688
		public virtual void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
		{
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001428A File Offset: 0x0001268A
		public virtual void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
		{
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001428C File Offset: 0x0001268C
		public virtual void OnUpdatedFriendList()
		{
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x0001428E File Offset: 0x0001268E
		public virtual void OnCustomAuthenticationFailed(string debugMessage)
		{
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00014290 File Offset: 0x00012690
		public virtual void OnCustomAuthenticationResponse(Dictionary<string, object> data)
		{
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00014292 File Offset: 0x00012692
		public virtual void OnWebRpcResponse(OperationResponse response)
		{
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00014294 File Offset: 0x00012694
		public virtual void OnOwnershipRequest(object[] viewAndPlayer)
		{
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00014296 File Offset: 0x00012696
		public virtual void OnLobbyStatisticsUpdate()
		{
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00014298 File Offset: 0x00012698
		public virtual void OnPhotonPlayerActivityChanged(PhotonPlayer otherPlayer)
		{
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001429A File Offset: 0x0001269A
		public virtual void OnOwnershipTransfered(object[] viewAndPlayers)
		{
		}
	}
}
