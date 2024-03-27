using System;
using System.Collections.Generic;
using System.Diagnostics;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000089 RID: 137
public static class PhotonNetwork
{
	// Token: 0x0600047B RID: 1147 RVA: 0x0001532C File Offset: 0x0001372C
	static PhotonNetwork()
	{
		if (PhotonNetwork.PhotonServerSettings != null)
		{
			Application.runInBackground = PhotonNetwork.PhotonServerSettings.RunInBackground;
		}
		GameObject gameObject = new GameObject();
		PhotonNetwork.photonMono = gameObject.AddComponent<PhotonHandler>();
		gameObject.name = "PhotonMono";
		gameObject.hideFlags = HideFlags.HideInHierarchy;
		ConnectionProtocol protocol = PhotonNetwork.PhotonServerSettings.Protocol;
		PhotonNetwork.networkingPeer = new NetworkingPeer(string.Empty, protocol);
		PhotonNetwork.networkingPeer.QuickResendAttempts = 2;
		PhotonNetwork.networkingPeer.SentCountAllowance = 7;
		if (PhotonNetwork.UsePreciseTimer)
		{
			global::UnityEngine.Debug.Log("Using Stopwatch as precision timer for PUN.");
			PhotonNetwork.startupStopwatch = new Stopwatch();
			PhotonNetwork.startupStopwatch.Start();
			PhotonNetwork.networkingPeer.LocalMsTimestampDelegate = () => (int)PhotonNetwork.startupStopwatch.ElapsedMilliseconds;
		}
		CustomTypes.Register();
	}

	// Token: 0x170000B5 RID: 181
	// (get) Token: 0x0600047C RID: 1148 RVA: 0x000154B8 File Offset: 0x000138B8
	// (set) Token: 0x0600047D RID: 1149 RVA: 0x000154BF File Offset: 0x000138BF
	public static string gameVersion { get; set; }

	// Token: 0x170000B6 RID: 182
	// (get) Token: 0x0600047E RID: 1150 RVA: 0x000154C7 File Offset: 0x000138C7
	public static string ServerAddress
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null) ? "<not connected>" : PhotonNetwork.networkingPeer.ServerAddress;
		}
	}

	// Token: 0x170000B7 RID: 183
	// (get) Token: 0x0600047F RID: 1151 RVA: 0x000154E7 File Offset: 0x000138E7
	public static CloudRegionCode CloudRegion
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null || !PhotonNetwork.connected || PhotonNetwork.Server == ServerConnection.NameServer) ? CloudRegionCode.none : PhotonNetwork.networkingPeer.CloudRegion;
		}
	}

	// Token: 0x170000B8 RID: 184
	// (get) Token: 0x06000480 RID: 1152 RVA: 0x00015518 File Offset: 0x00013918
	public static bool connected
	{
		get
		{
			return PhotonNetwork.offlineMode || (PhotonNetwork.networkingPeer != null && (!PhotonNetwork.networkingPeer.IsInitialConnect && PhotonNetwork.networkingPeer.State != ClientState.PeerCreated && PhotonNetwork.networkingPeer.State != ClientState.Disconnected && PhotonNetwork.networkingPeer.State != ClientState.Disconnecting) && PhotonNetwork.networkingPeer.State != ClientState.ConnectingToNameServer);
		}
	}

	// Token: 0x170000B9 RID: 185
	// (get) Token: 0x06000481 RID: 1153 RVA: 0x00015592 File Offset: 0x00013992
	public static bool connecting
	{
		get
		{
			return PhotonNetwork.networkingPeer.IsInitialConnect && !PhotonNetwork.offlineMode;
		}
	}

	// Token: 0x170000BA RID: 186
	// (get) Token: 0x06000482 RID: 1154 RVA: 0x000155B0 File Offset: 0x000139B0
	public static bool connectedAndReady
	{
		get
		{
			if (!PhotonNetwork.connected)
			{
				return false;
			}
			if (PhotonNetwork.offlineMode)
			{
				return true;
			}
			ClientState connectionStateDetailed = PhotonNetwork.connectionStateDetailed;
			switch (connectionStateDetailed)
			{
			case ClientState.ConnectingToMasterserver:
			case ClientState.Disconnecting:
			case ClientState.Disconnected:
			case ClientState.ConnectingToNameServer:
			case ClientState.Authenticating:
				break;
			default:
				switch (connectionStateDetailed)
				{
				case ClientState.ConnectingToGameserver:
				case ClientState.Joining:
					break;
				default:
					if (connectionStateDetailed != ClientState.PeerCreated)
					{
						return true;
					}
					break;
				}
				break;
			}
			return false;
		}
	}

	// Token: 0x170000BB RID: 187
	// (get) Token: 0x06000483 RID: 1155 RVA: 0x0001562C File Offset: 0x00013A2C
	public static ConnectionState connectionState
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return ConnectionState.Connected;
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return ConnectionState.Disconnected;
			}
			PeerStateValue peerState = PhotonNetwork.networkingPeer.PeerState;
			switch (peerState)
			{
			case PeerStateValue.Disconnected:
				return ConnectionState.Disconnected;
			case PeerStateValue.Connecting:
				return ConnectionState.Connecting;
			default:
				if (peerState != PeerStateValue.InitializingApplication)
				{
					return ConnectionState.Disconnected;
				}
				return ConnectionState.InitializingApplication;
			case PeerStateValue.Connected:
				return ConnectionState.Connected;
			case PeerStateValue.Disconnecting:
				return ConnectionState.Disconnecting;
			}
		}
	}

	// Token: 0x170000BC RID: 188
	// (get) Token: 0x06000484 RID: 1156 RVA: 0x0001568E File Offset: 0x00013A8E
	public static ClientState connectionStateDetailed
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return (PhotonNetwork.offlineModeRoom == null) ? ClientState.ConnectedToMaster : ClientState.Joined;
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return ClientState.Disconnected;
			}
			return PhotonNetwork.networkingPeer.State;
		}
	}

	// Token: 0x170000BD RID: 189
	// (get) Token: 0x06000485 RID: 1157 RVA: 0x000156C5 File Offset: 0x00013AC5
	public static ServerConnection Server
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null) ? ServerConnection.NameServer : PhotonNetwork.networkingPeer.Server;
		}
	}

	// Token: 0x170000BE RID: 190
	// (get) Token: 0x06000486 RID: 1158 RVA: 0x000156E1 File Offset: 0x00013AE1
	// (set) Token: 0x06000487 RID: 1159 RVA: 0x000156FD File Offset: 0x00013AFD
	public static AuthenticationValues AuthValues
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null) ? null : PhotonNetwork.networkingPeer.AuthValues;
		}
		set
		{
			if (PhotonNetwork.networkingPeer != null)
			{
				PhotonNetwork.networkingPeer.AuthValues = value;
			}
		}
	}

	// Token: 0x170000BF RID: 191
	// (get) Token: 0x06000488 RID: 1160 RVA: 0x00015714 File Offset: 0x00013B14
	public static Room room
	{
		get
		{
			if (PhotonNetwork.isOfflineMode)
			{
				return PhotonNetwork.offlineModeRoom;
			}
			return PhotonNetwork.networkingPeer.CurrentRoom;
		}
	}

	// Token: 0x170000C0 RID: 192
	// (get) Token: 0x06000489 RID: 1161 RVA: 0x00015730 File Offset: 0x00013B30
	public static PhotonPlayer player
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return null;
			}
			return PhotonNetwork.networkingPeer.LocalPlayer;
		}
	}

	// Token: 0x170000C1 RID: 193
	// (get) Token: 0x0600048A RID: 1162 RVA: 0x00015748 File Offset: 0x00013B48
	public static PhotonPlayer masterClient
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return PhotonNetwork.player;
			}
			if (PhotonNetwork.networkingPeer == null)
			{
				return null;
			}
			return PhotonNetwork.networkingPeer.GetPlayerWithId(PhotonNetwork.networkingPeer.mMasterClientId);
		}
	}

	// Token: 0x170000C2 RID: 194
	// (get) Token: 0x0600048B RID: 1163 RVA: 0x0001577A File Offset: 0x00013B7A
	// (set) Token: 0x0600048C RID: 1164 RVA: 0x00015786 File Offset: 0x00013B86
	public static string playerName
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayerName;
		}
		set
		{
			PhotonNetwork.networkingPeer.PlayerName = value;
		}
	}

	// Token: 0x170000C3 RID: 195
	// (get) Token: 0x0600048D RID: 1165 RVA: 0x00015793 File Offset: 0x00013B93
	public static PhotonPlayer[] playerList
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return PhotonNetwork.networkingPeer.mPlayerListCopy;
		}
	}

	// Token: 0x170000C4 RID: 196
	// (get) Token: 0x0600048E RID: 1166 RVA: 0x000157B0 File Offset: 0x00013BB0
	public static PhotonPlayer[] otherPlayers
	{
		get
		{
			if (PhotonNetwork.networkingPeer == null)
			{
				return new PhotonPlayer[0];
			}
			return PhotonNetwork.networkingPeer.mOtherPlayerListCopy;
		}
	}

	// Token: 0x170000C5 RID: 197
	// (get) Token: 0x0600048F RID: 1167 RVA: 0x000157CD File Offset: 0x00013BCD
	// (set) Token: 0x06000490 RID: 1168 RVA: 0x000157D4 File Offset: 0x00013BD4
	public static List<FriendInfo> Friends { get; internal set; }

	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x06000491 RID: 1169 RVA: 0x000157DC File Offset: 0x00013BDC
	public static int FriendsListAge
	{
		get
		{
			return (PhotonNetwork.networkingPeer == null) ? 0 : PhotonNetwork.networkingPeer.FriendListAge;
		}
	}

	// Token: 0x170000C7 RID: 199
	// (get) Token: 0x06000492 RID: 1170 RVA: 0x000157F8 File Offset: 0x00013BF8
	// (set) Token: 0x06000493 RID: 1171 RVA: 0x00015804 File Offset: 0x00013C04
	public static IPunPrefabPool PrefabPool
	{
		get
		{
			return PhotonNetwork.networkingPeer.ObjectPool;
		}
		set
		{
			PhotonNetwork.networkingPeer.ObjectPool = value;
		}
	}

	// Token: 0x170000C8 RID: 200
	// (get) Token: 0x06000494 RID: 1172 RVA: 0x00015811 File Offset: 0x00013C11
	// (set) Token: 0x06000495 RID: 1173 RVA: 0x00015818 File Offset: 0x00013C18
	public static bool offlineMode
	{
		get
		{
			return PhotonNetwork.isOfflineMode;
		}
		set
		{
			if (value == PhotonNetwork.isOfflineMode)
			{
				return;
			}
			if (value && PhotonNetwork.connected)
			{
				global::UnityEngine.Debug.LogError("Can't start OFFLINE mode while connected!");
				return;
			}
			if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
			{
				PhotonNetwork.networkingPeer.Disconnect();
			}
			PhotonNetwork.isOfflineMode = value;
			if (PhotonNetwork.isOfflineMode)
			{
				PhotonNetwork.networkingPeer.ChangeLocalID(-1);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster, new object[0]);
			}
			else
			{
				PhotonNetwork.offlineModeRoom = null;
				PhotonNetwork.networkingPeer.ChangeLocalID(-1);
			}
		}
	}

	// Token: 0x170000C9 RID: 201
	// (get) Token: 0x06000496 RID: 1174 RVA: 0x000158A3 File Offset: 0x00013CA3
	// (set) Token: 0x06000497 RID: 1175 RVA: 0x000158AA File Offset: 0x00013CAA
	public static bool automaticallySyncScene
	{
		get
		{
			return PhotonNetwork._mAutomaticallySyncScene;
		}
		set
		{
			PhotonNetwork._mAutomaticallySyncScene = value;
			if (PhotonNetwork._mAutomaticallySyncScene && PhotonNetwork.room != null)
			{
				PhotonNetwork.networkingPeer.LoadLevelIfSynced();
			}
		}
	}

	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000498 RID: 1176 RVA: 0x000158D0 File Offset: 0x00013CD0
	// (set) Token: 0x06000499 RID: 1177 RVA: 0x000158D7 File Offset: 0x00013CD7
	public static bool autoCleanUpPlayerObjects
	{
		get
		{
			return PhotonNetwork.m_autoCleanUpPlayerObjects;
		}
		set
		{
			if (PhotonNetwork.room != null)
			{
				global::UnityEngine.Debug.LogError("Setting autoCleanUpPlayerObjects while in a room is not supported.");
			}
			else
			{
				PhotonNetwork.m_autoCleanUpPlayerObjects = value;
			}
		}
	}

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x0600049A RID: 1178 RVA: 0x000158F8 File Offset: 0x00013CF8
	// (set) Token: 0x0600049B RID: 1179 RVA: 0x00015904 File Offset: 0x00013D04
	public static bool autoJoinLobby
	{
		get
		{
			return PhotonNetwork.PhotonServerSettings.JoinLobby;
		}
		set
		{
			PhotonNetwork.PhotonServerSettings.JoinLobby = value;
		}
	}

	// Token: 0x170000CC RID: 204
	// (get) Token: 0x0600049C RID: 1180 RVA: 0x00015911 File Offset: 0x00013D11
	// (set) Token: 0x0600049D RID: 1181 RVA: 0x0001591D File Offset: 0x00013D1D
	public static bool EnableLobbyStatistics
	{
		get
		{
			return PhotonNetwork.PhotonServerSettings.EnableLobbyStatistics;
		}
		set
		{
			PhotonNetwork.PhotonServerSettings.EnableLobbyStatistics = value;
		}
	}

	// Token: 0x170000CD RID: 205
	// (get) Token: 0x0600049E RID: 1182 RVA: 0x0001592A File Offset: 0x00013D2A
	// (set) Token: 0x0600049F RID: 1183 RVA: 0x00015936 File Offset: 0x00013D36
	public static List<TypedLobbyInfo> LobbyStatistics
	{
		get
		{
			return PhotonNetwork.networkingPeer.LobbyStatistics;
		}
		private set
		{
			PhotonNetwork.networkingPeer.LobbyStatistics = value;
		}
	}

	// Token: 0x170000CE RID: 206
	// (get) Token: 0x060004A0 RID: 1184 RVA: 0x00015943 File Offset: 0x00013D43
	public static bool insideLobby
	{
		get
		{
			return PhotonNetwork.networkingPeer.insideLobby;
		}
	}

	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0001594F File Offset: 0x00013D4F
	// (set) Token: 0x060004A2 RID: 1186 RVA: 0x0001595B File Offset: 0x00013D5B
	public static TypedLobby lobby
	{
		get
		{
			return PhotonNetwork.networkingPeer.lobby;
		}
		set
		{
			PhotonNetwork.networkingPeer.lobby = value;
		}
	}

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00015968 File Offset: 0x00013D68
	// (set) Token: 0x060004A4 RID: 1188 RVA: 0x00015975 File Offset: 0x00013D75
	public static int sendRate
	{
		get
		{
			return 1000 / PhotonNetwork.sendInterval;
		}
		set
		{
			PhotonNetwork.sendInterval = 1000 / value;
			if (PhotonNetwork.photonMono != null)
			{
				PhotonNetwork.photonMono.updateInterval = PhotonNetwork.sendInterval;
			}
			if (value < PhotonNetwork.sendRateOnSerialize)
			{
				PhotonNetwork.sendRateOnSerialize = value;
			}
		}
	}

	// Token: 0x170000D1 RID: 209
	// (get) Token: 0x060004A5 RID: 1189 RVA: 0x000159B3 File Offset: 0x00013DB3
	// (set) Token: 0x060004A6 RID: 1190 RVA: 0x000159C0 File Offset: 0x00013DC0
	public static int sendRateOnSerialize
	{
		get
		{
			return 1000 / PhotonNetwork.sendIntervalOnSerialize;
		}
		set
		{
			if (value > PhotonNetwork.sendRate)
			{
				global::UnityEngine.Debug.LogError("Error: Can not set the OnSerialize rate higher than the overall SendRate.");
				value = PhotonNetwork.sendRate;
			}
			PhotonNetwork.sendIntervalOnSerialize = 1000 / value;
			if (PhotonNetwork.photonMono != null)
			{
				PhotonNetwork.photonMono.updateIntervalOnSerialize = PhotonNetwork.sendIntervalOnSerialize;
			}
		}
	}

	// Token: 0x170000D2 RID: 210
	// (get) Token: 0x060004A7 RID: 1191 RVA: 0x00015A14 File Offset: 0x00013E14
	// (set) Token: 0x060004A8 RID: 1192 RVA: 0x00015A1B File Offset: 0x00013E1B
	public static bool isMessageQueueRunning
	{
		get
		{
			return PhotonNetwork.m_isMessageQueueRunning;
		}
		set
		{
			if (value)
			{
				PhotonHandler.StartFallbackSendAckThread();
			}
			PhotonNetwork.networkingPeer.IsSendingOnlyAcks = !value;
			PhotonNetwork.m_isMessageQueueRunning = value;
		}
	}

	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x060004A9 RID: 1193 RVA: 0x00015A3C File Offset: 0x00013E3C
	// (set) Token: 0x060004AA RID: 1194 RVA: 0x00015A48 File Offset: 0x00013E48
	public static int unreliableCommandsLimit
	{
		get
		{
			return PhotonNetwork.networkingPeer.LimitOfUnreliableCommands;
		}
		set
		{
			PhotonNetwork.networkingPeer.LimitOfUnreliableCommands = value;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x060004AB RID: 1195 RVA: 0x00015A58 File Offset: 0x00013E58
	public static double time
	{
		get
		{
			uint serverTimestamp = (uint)PhotonNetwork.ServerTimestamp;
			double num = serverTimestamp;
			return num / 1000.0;
		}
	}

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x060004AC RID: 1196 RVA: 0x00015A7C File Offset: 0x00013E7C
	public static int ServerTimestamp
	{
		get
		{
			if (!PhotonNetwork.offlineMode)
			{
				return PhotonNetwork.networkingPeer.ServerTimeInMilliSeconds;
			}
			if (PhotonNetwork.UsePreciseTimer && PhotonNetwork.startupStopwatch != null && PhotonNetwork.startupStopwatch.IsRunning)
			{
				return (int)PhotonNetwork.startupStopwatch.ElapsedMilliseconds;
			}
			return Environment.TickCount;
		}
	}

	// Token: 0x170000D6 RID: 214
	// (get) Token: 0x060004AD RID: 1197 RVA: 0x00015AD2 File Offset: 0x00013ED2
	public static bool isMasterClient
	{
		get
		{
			return PhotonNetwork.offlineMode || PhotonNetwork.networkingPeer.mMasterClientId == PhotonNetwork.player.ID;
		}
	}

	// Token: 0x170000D7 RID: 215
	// (get) Token: 0x060004AE RID: 1198 RVA: 0x00015AF6 File Offset: 0x00013EF6
	public static bool inRoom
	{
		get
		{
			return PhotonNetwork.connectionStateDetailed == ClientState.Joined;
		}
	}

	// Token: 0x170000D8 RID: 216
	// (get) Token: 0x060004AF RID: 1199 RVA: 0x00015B01 File Offset: 0x00013F01
	public static bool isNonMasterClientInRoom
	{
		get
		{
			return !PhotonNetwork.isMasterClient && PhotonNetwork.room != null;
		}
	}

	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00015B1B File Offset: 0x00013F1B
	public static int countOfPlayersOnMaster
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayersOnMasterCount;
		}
	}

	// Token: 0x170000DA RID: 218
	// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00015B27 File Offset: 0x00013F27
	public static int countOfPlayersInRooms
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayersInRoomsCount;
		}
	}

	// Token: 0x170000DB RID: 219
	// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00015B33 File Offset: 0x00013F33
	public static int countOfPlayers
	{
		get
		{
			return PhotonNetwork.networkingPeer.PlayersInRoomsCount + PhotonNetwork.networkingPeer.PlayersOnMasterCount;
		}
	}

	// Token: 0x170000DC RID: 220
	// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00015B4A File Offset: 0x00013F4A
	public static int countOfRooms
	{
		get
		{
			return PhotonNetwork.networkingPeer.RoomsCount;
		}
	}

	// Token: 0x170000DD RID: 221
	// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00015B56 File Offset: 0x00013F56
	// (set) Token: 0x060004B5 RID: 1205 RVA: 0x00015B62 File Offset: 0x00013F62
	public static bool NetworkStatisticsEnabled
	{
		get
		{
			return PhotonNetwork.networkingPeer.TrafficStatsEnabled;
		}
		set
		{
			PhotonNetwork.networkingPeer.TrafficStatsEnabled = value;
		}
	}

	// Token: 0x170000DE RID: 222
	// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00015B6F File Offset: 0x00013F6F
	public static int ResentReliableCommands
	{
		get
		{
			return PhotonNetwork.networkingPeer.ResentReliableCommands;
		}
	}

	// Token: 0x170000DF RID: 223
	// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00015B7B File Offset: 0x00013F7B
	// (set) Token: 0x060004B8 RID: 1208 RVA: 0x00015B88 File Offset: 0x00013F88
	public static bool CrcCheckEnabled
	{
		get
		{
			return PhotonNetwork.networkingPeer.CrcEnabled;
		}
		set
		{
			if (!PhotonNetwork.connected && !PhotonNetwork.connecting)
			{
				PhotonNetwork.networkingPeer.CrcEnabled = value;
			}
			else
			{
				global::UnityEngine.Debug.Log("Can't change CrcCheckEnabled while being connected. CrcCheckEnabled stays " + PhotonNetwork.networkingPeer.CrcEnabled);
			}
		}
	}

	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x060004B9 RID: 1209 RVA: 0x00015BD7 File Offset: 0x00013FD7
	public static int PacketLossByCrcCheck
	{
		get
		{
			return PhotonNetwork.networkingPeer.PacketLossByCrc;
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x060004BA RID: 1210 RVA: 0x00015BE3 File Offset: 0x00013FE3
	// (set) Token: 0x060004BB RID: 1211 RVA: 0x00015BEF File Offset: 0x00013FEF
	public static int MaxResendsBeforeDisconnect
	{
		get
		{
			return PhotonNetwork.networkingPeer.SentCountAllowance;
		}
		set
		{
			if (value < 3)
			{
				value = 3;
			}
			if (value > 10)
			{
				value = 10;
			}
			PhotonNetwork.networkingPeer.SentCountAllowance = value;
		}
	}

	// Token: 0x170000E2 RID: 226
	// (get) Token: 0x060004BC RID: 1212 RVA: 0x00015C12 File Offset: 0x00014012
	// (set) Token: 0x060004BD RID: 1213 RVA: 0x00015C1E File Offset: 0x0001401E
	public static int QuickResends
	{
		get
		{
			return (int)PhotonNetwork.networkingPeer.QuickResendAttempts;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			if (value > 3)
			{
				value = 3;
			}
			PhotonNetwork.networkingPeer.QuickResendAttempts = (byte)value;
		}
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x00015C40 File Offset: 0x00014040
	public static void SwitchToProtocol(ConnectionProtocol cp)
	{
		PhotonNetwork.networkingPeer.TransportProtocol = cp;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x00015C50 File Offset: 0x00014050
	public static bool ConnectUsingSettings(string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			global::UnityEngine.Debug.LogWarning("ConnectUsingSettings() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			global::UnityEngine.Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.NotSet)
		{
			global::UnityEngine.Debug.LogError("You did not select a Hosting Type in your PhotonServerSettings. Please set it up or don't use ConnectUsingSettings().");
			return false;
		}
		if (PhotonNetwork.logLevel == PhotonLogLevel.ErrorsOnly)
		{
			PhotonNetwork.logLevel = PhotonNetwork.PhotonServerSettings.PunLogging;
		}
		if (PhotonNetwork.networkingPeer.DebugOut == DebugLevel.ERROR)
		{
			PhotonNetwork.networkingPeer.DebugOut = PhotonNetwork.PhotonServerSettings.NetworkLogging;
		}
		PhotonNetwork.SwitchToProtocol(PhotonNetwork.PhotonServerSettings.Protocol);
		PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			PhotonNetwork.offlineMode = true;
			return true;
		}
		if (PhotonNetwork.offlineMode)
		{
			global::UnityEngine.Debug.LogWarning("ConnectUsingSettings() disabled the offline mode. No longer offline.");
		}
		PhotonNetwork.offlineMode = false;
		PhotonNetwork.isMessageQueueRunning = true;
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.SelfHosted)
		{
			PhotonNetwork.networkingPeer.IsUsingNameServer = false;
			PhotonNetwork.networkingPeer.MasterServerAddress = ((PhotonNetwork.PhotonServerSettings.ServerPort != 0) ? (PhotonNetwork.PhotonServerSettings.ServerAddress + ":" + PhotonNetwork.PhotonServerSettings.ServerPort) : PhotonNetwork.PhotonServerSettings.ServerAddress);
			return PhotonNetwork.networkingPeer.Connect(PhotonNetwork.networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion)
		{
			return PhotonNetwork.ConnectToBestCloudServer(gameVersion);
		}
		return PhotonNetwork.networkingPeer.ConnectToRegionMaster(PhotonNetwork.PhotonServerSettings.PreferredRegion);
	}

	// Token: 0x060004C0 RID: 1216 RVA: 0x00015E10 File Offset: 0x00014210
	public static bool ConnectToMaster(string masterServerAddress, int port, string appID, string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			global::UnityEngine.Debug.LogWarning("ConnectToMaster() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			global::UnityEngine.Debug.LogWarning("ConnectToMaster() disabled the offline mode. No longer offline.");
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			global::UnityEngine.Debug.LogWarning("ConnectToMaster() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
		}
		PhotonNetwork.networkingPeer.SetApp(appID, gameVersion);
		PhotonNetwork.networkingPeer.IsUsingNameServer = false;
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		PhotonNetwork.networkingPeer.MasterServerAddress = ((port != 0) ? (masterServerAddress + ":" + port) : masterServerAddress);
		return PhotonNetwork.networkingPeer.Connect(PhotonNetwork.networkingPeer.MasterServerAddress, ServerConnection.MasterServer);
	}

	// Token: 0x060004C1 RID: 1217 RVA: 0x00015EE0 File Offset: 0x000142E0
	public static bool Reconnect()
	{
		if (string.IsNullOrEmpty(PhotonNetwork.networkingPeer.MasterServerAddress))
		{
			global::UnityEngine.Debug.LogWarning("Reconnect() failed. It seems the client wasn't connected before?! Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			global::UnityEngine.Debug.LogWarning("Reconnect() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			global::UnityEngine.Debug.LogWarning("Reconnect() disabled the offline mode. No longer offline.");
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			global::UnityEngine.Debug.LogWarning("Reconnect() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
		}
		PhotonNetwork.networkingPeer.IsUsingNameServer = false;
		PhotonNetwork.networkingPeer.IsInitialConnect = false;
		return PhotonNetwork.networkingPeer.ReconnectToMaster();
	}

	// Token: 0x060004C2 RID: 1218 RVA: 0x00015FA4 File Offset: 0x000143A4
	public static bool ReconnectAndRejoin()
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			global::UnityEngine.Debug.LogWarning("ReconnectAndRejoin() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			global::UnityEngine.Debug.LogWarning("ReconnectAndRejoin() disabled the offline mode. No longer offline.");
		}
		if (string.IsNullOrEmpty(PhotonNetwork.networkingPeer.GameServerAddress))
		{
			global::UnityEngine.Debug.LogWarning("ReconnectAndRejoin() failed. It seems the client wasn't connected to a game server before (no address).");
			return false;
		}
		if (PhotonNetwork.networkingPeer.enterRoomParamsCache == null)
		{
			global::UnityEngine.Debug.LogWarning("ReconnectAndRejoin() failed. It seems the client doesn't have any previous room to re-join.");
			return false;
		}
		if (!PhotonNetwork.isMessageQueueRunning)
		{
			PhotonNetwork.isMessageQueueRunning = true;
			global::UnityEngine.Debug.LogWarning("ReconnectAndRejoin() enabled isMessageQueueRunning. Needs to be able to dispatch incoming messages.");
		}
		PhotonNetwork.networkingPeer.IsUsingNameServer = false;
		PhotonNetwork.networkingPeer.IsInitialConnect = false;
		return PhotonNetwork.networkingPeer.ReconnectAndRejoin();
	}

	// Token: 0x060004C3 RID: 1219 RVA: 0x00016070 File Offset: 0x00014470
	public static bool ConnectToBestCloudServer(string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			global::UnityEngine.Debug.LogWarning("ConnectToBestCloudServer() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			global::UnityEngine.Debug.LogError("Can't connect: Loading settings failed. ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			return PhotonNetwork.ConnectUsingSettings(gameVersion);
		}
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
		CloudRegionCode bestRegionCodeInPreferences = PhotonHandler.BestRegionCodeInPreferences;
		if (bestRegionCodeInPreferences != CloudRegionCode.none)
		{
			global::UnityEngine.Debug.Log("Best region found in PlayerPrefs. Connecting to: " + bestRegionCodeInPreferences);
			return PhotonNetwork.networkingPeer.ConnectToRegionMaster(bestRegionCodeInPreferences);
		}
		return PhotonNetwork.networkingPeer.ConnectToNameServer();
	}

	// Token: 0x060004C4 RID: 1220 RVA: 0x0001613C File Offset: 0x0001453C
	public static bool ConnectToRegion(CloudRegionCode region, string gameVersion)
	{
		if (PhotonNetwork.networkingPeer.PeerState != PeerStateValue.Disconnected)
		{
			global::UnityEngine.Debug.LogWarning("ConnectToRegion() failed. Can only connect while in state 'Disconnected'. Current state: " + PhotonNetwork.networkingPeer.PeerState);
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings == null)
		{
			global::UnityEngine.Debug.LogError("Can't connect: ServerSettings asset must be in any 'Resources' folder as: PhotonServerSettings");
			return false;
		}
		if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.OfflineMode)
		{
			return PhotonNetwork.ConnectUsingSettings(gameVersion);
		}
		PhotonNetwork.networkingPeer.IsInitialConnect = true;
		PhotonNetwork.networkingPeer.SetApp(PhotonNetwork.PhotonServerSettings.AppID, gameVersion);
		if (region != CloudRegionCode.none)
		{
			global::UnityEngine.Debug.Log("ConnectToRegion: " + region);
			return PhotonNetwork.networkingPeer.ConnectToRegionMaster(region);
		}
		return false;
	}

	// Token: 0x060004C5 RID: 1221 RVA: 0x000161F4 File Offset: 0x000145F4
	public static void OverrideBestCloudServer(CloudRegionCode region)
	{
		PhotonHandler.BestRegionCodeInPreferences = region;
	}

	// Token: 0x060004C6 RID: 1222 RVA: 0x000161FC File Offset: 0x000145FC
	public static void RefreshCloudServerRating()
	{
		throw new NotImplementedException("not available at the moment");
	}

	// Token: 0x060004C7 RID: 1223 RVA: 0x00016208 File Offset: 0x00014608
	public static void NetworkStatisticsReset()
	{
		PhotonNetwork.networkingPeer.TrafficStatsReset();
	}

	// Token: 0x060004C8 RID: 1224 RVA: 0x00016214 File Offset: 0x00014614
	public static string NetworkStatisticsToString()
	{
		if (PhotonNetwork.networkingPeer == null || PhotonNetwork.offlineMode)
		{
			return "Offline or in OfflineMode. No VitalStats available.";
		}
		return PhotonNetwork.networkingPeer.VitalStatsToString(false);
	}

	// Token: 0x060004C9 RID: 1225 RVA: 0x0001623B File Offset: 0x0001463B
	[Obsolete("Used for compatibility with Unity networking only. Encryption is automatically initialized while connecting.")]
	public static void InitializeSecurity()
	{
	}

	// Token: 0x060004CA RID: 1226 RVA: 0x0001623D File Offset: 0x0001463D
	private static bool VerifyCanUseNetwork()
	{
		if (PhotonNetwork.connected)
		{
			return true;
		}
		global::UnityEngine.Debug.LogError("Cannot send messages when not connected. Either connect to Photon OR use offline mode!");
		return false;
	}

	// Token: 0x060004CB RID: 1227 RVA: 0x00016258 File Offset: 0x00014658
	public static void Disconnect()
	{
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineMode = false;
			PhotonNetwork.offlineModeRoom = null;
			PhotonNetwork.networkingPeer.State = ClientState.Disconnecting;
			PhotonNetwork.networkingPeer.OnStatusChanged(StatusCode.Disconnect);
			return;
		}
		if (PhotonNetwork.networkingPeer == null)
		{
			return;
		}
		PhotonNetwork.networkingPeer.Disconnect();
	}

	// Token: 0x060004CC RID: 1228 RVA: 0x000162AC File Offset: 0x000146AC
	public static bool FindFriends(string[] friendsToFind)
	{
		return PhotonNetwork.networkingPeer != null && !PhotonNetwork.isOfflineMode && PhotonNetwork.networkingPeer.OpFindFriends(friendsToFind);
	}

	// Token: 0x060004CD RID: 1229 RVA: 0x000162CF File Offset: 0x000146CF
	public static bool CreateRoom(string roomName)
	{
		return PhotonNetwork.CreateRoom(roomName, null, null, null);
	}

	// Token: 0x060004CE RID: 1230 RVA: 0x000162DA File Offset: 0x000146DA
	public static bool CreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		return PhotonNetwork.CreateRoom(roomName, roomOptions, typedLobby, null);
	}

	// Token: 0x060004CF RID: 1231 RVA: 0x000162E8 File Offset: 0x000146E8
	public static bool CreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, string[] expectedUsers)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				global::UnityEngine.Debug.LogError("CreateRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom(roomName, roomOptions, true);
			return true;
		}
		else
		{
			if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
			{
				global::UnityEngine.Debug.LogError("CreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			typedLobby = typedLobby ?? ((!PhotonNetwork.networkingPeer.insideLobby) ? null : PhotonNetwork.networkingPeer.lobby);
			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName = roomName;
			enterRoomParams.RoomOptions = roomOptions;
			enterRoomParams.Lobby = typedLobby;
			enterRoomParams.ExpectedUsers = expectedUsers;
			return PhotonNetwork.networkingPeer.OpCreateGame(enterRoomParams);
		}
	}

	// Token: 0x060004D0 RID: 1232 RVA: 0x0001639A File Offset: 0x0001479A
	public static bool JoinRoom(string roomName)
	{
		return PhotonNetwork.JoinRoom(roomName, null);
	}

	// Token: 0x060004D1 RID: 1233 RVA: 0x000163A4 File Offset: 0x000147A4
	public static bool JoinRoom(string roomName, string[] expectedUsers)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				global::UnityEngine.Debug.LogError("JoinRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom(roomName, null, true);
			return true;
		}
		else
		{
			if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
			{
				global::UnityEngine.Debug.LogError("JoinRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			if (string.IsNullOrEmpty(roomName))
			{
				global::UnityEngine.Debug.LogError("JoinRoom failed. A roomname is required. If you don't know one, how will you join?");
				return false;
			}
			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName = roomName;
			enterRoomParams.ExpectedUsers = expectedUsers;
			return PhotonNetwork.networkingPeer.OpJoinRoom(enterRoomParams);
		}
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x00016436 File Offset: 0x00014836
	public static bool JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby)
	{
		return PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby, null);
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x00016444 File Offset: 0x00014844
	public static bool JoinOrCreateRoom(string roomName, RoomOptions roomOptions, TypedLobby typedLobby, string[] expectedUsers)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				global::UnityEngine.Debug.LogError("JoinOrCreateRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom(roomName, roomOptions, true);
			return true;
		}
		else
		{
			if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
			{
				global::UnityEngine.Debug.LogError("JoinOrCreateRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			if (string.IsNullOrEmpty(roomName))
			{
				global::UnityEngine.Debug.LogError("JoinOrCreateRoom failed. A roomname is required. If you don't know one, how will you join?");
				return false;
			}
			typedLobby = typedLobby ?? ((!PhotonNetwork.networkingPeer.insideLobby) ? null : PhotonNetwork.networkingPeer.lobby);
			EnterRoomParams enterRoomParams = new EnterRoomParams();
			enterRoomParams.RoomName = roomName;
			enterRoomParams.RoomOptions = roomOptions;
			enterRoomParams.Lobby = typedLobby;
			enterRoomParams.CreateIfNotExists = true;
			enterRoomParams.PlayerProperties = PhotonNetwork.player.CustomProperties;
			enterRoomParams.ExpectedUsers = expectedUsers;
			return PhotonNetwork.networkingPeer.OpJoinRoom(enterRoomParams);
		}
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00016524 File Offset: 0x00014924
	public static bool JoinRandomRoom()
	{
		return PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, null, null, null);
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x00016531 File Offset: 0x00014931
	public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers)
	{
		return PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, expectedMaxPlayers, MatchmakingMode.FillRoom, null, null, null);
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x00016540 File Offset: 0x00014940
	public static bool JoinRandomRoom(Hashtable expectedCustomRoomProperties, byte expectedMaxPlayers, MatchmakingMode matchingType, TypedLobby typedLobby, string sqlLobbyFilter, string[] expectedUsers = null)
	{
		if (PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.offlineModeRoom != null)
			{
				global::UnityEngine.Debug.LogError("JoinRandomRoom failed. In offline mode you still have to leave a room to enter another.");
				return false;
			}
			PhotonNetwork.EnterOfflineRoom("offline room", null, true);
			return true;
		}
		else
		{
			if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
			{
				global::UnityEngine.Debug.LogError("JoinRandomRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
				return false;
			}
			typedLobby = typedLobby ?? ((!PhotonNetwork.networkingPeer.insideLobby) ? null : PhotonNetwork.networkingPeer.lobby);
			OpJoinRandomRoomParams opJoinRandomRoomParams = new OpJoinRandomRoomParams();
			opJoinRandomRoomParams.ExpectedCustomRoomProperties = expectedCustomRoomProperties;
			opJoinRandomRoomParams.ExpectedMaxPlayers = expectedMaxPlayers;
			opJoinRandomRoomParams.MatchingType = matchingType;
			opJoinRandomRoomParams.TypedLobby = typedLobby;
			opJoinRandomRoomParams.SqlLobbyFilter = sqlLobbyFilter;
			opJoinRandomRoomParams.ExpectedUsers = expectedUsers;
			return PhotonNetwork.networkingPeer.OpJoinRandomRoom(opJoinRandomRoomParams);
		}
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00016608 File Offset: 0x00014A08
	public static bool ReJoinRoom(string roomName)
	{
		if (PhotonNetwork.offlineMode)
		{
			global::UnityEngine.Debug.LogError("ReJoinRoom failed due to offline mode.");
			return false;
		}
		if (PhotonNetwork.networkingPeer.Server != ServerConnection.MasterServer || !PhotonNetwork.connectedAndReady)
		{
			global::UnityEngine.Debug.LogError("ReJoinRoom failed. Client is not on Master Server or not yet ready to call operations. Wait for callback: OnJoinedLobby or OnConnectedToMaster.");
			return false;
		}
		if (string.IsNullOrEmpty(roomName))
		{
			global::UnityEngine.Debug.LogError("ReJoinRoom failed. A roomname is required. If you don't know one, how will you join?");
			return false;
		}
		EnterRoomParams enterRoomParams = new EnterRoomParams();
		enterRoomParams.RoomName = roomName;
		enterRoomParams.RejoinOnly = true;
		enterRoomParams.PlayerProperties = PhotonNetwork.player.CustomProperties;
		return PhotonNetwork.networkingPeer.OpJoinRoom(enterRoomParams);
	}

	// Token: 0x060004D8 RID: 1240 RVA: 0x00016698 File Offset: 0x00014A98
	private static void EnterOfflineRoom(string roomName, RoomOptions roomOptions, bool createdRoom)
	{
		PhotonNetwork.offlineModeRoom = new Room(roomName, roomOptions);
		PhotonNetwork.networkingPeer.ChangeLocalID(1);
		PhotonNetwork.offlineModeRoom.MasterClientId = 1;
		if (createdRoom)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom, new object[0]);
		}
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom, new object[0]);
	}

	// Token: 0x060004D9 RID: 1241 RVA: 0x000166E6 File Offset: 0x00014AE6
	public static bool JoinLobby()
	{
		return PhotonNetwork.JoinLobby(null);
	}

	// Token: 0x060004DA RID: 1242 RVA: 0x000166F0 File Offset: 0x00014AF0
	public static bool JoinLobby(TypedLobby typedLobby)
	{
		if (PhotonNetwork.connected && PhotonNetwork.Server == ServerConnection.MasterServer)
		{
			if (typedLobby == null)
			{
				typedLobby = TypedLobby.Default;
			}
			bool flag = PhotonNetwork.networkingPeer.OpJoinLobby(typedLobby);
			if (flag)
			{
				PhotonNetwork.networkingPeer.lobby = typedLobby;
			}
			return flag;
		}
		return false;
	}

	// Token: 0x060004DB RID: 1243 RVA: 0x0001673E File Offset: 0x00014B3E
	public static bool LeaveLobby()
	{
		return PhotonNetwork.connected && PhotonNetwork.Server == ServerConnection.MasterServer && PhotonNetwork.networkingPeer.OpLeaveLobby();
	}

	// Token: 0x060004DC RID: 1244 RVA: 0x00016760 File Offset: 0x00014B60
	public static bool LeaveRoom()
	{
		if (PhotonNetwork.offlineMode)
		{
			PhotonNetwork.offlineModeRoom = null;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom, new object[0]);
			return true;
		}
		if (PhotonNetwork.room == null)
		{
			global::UnityEngine.Debug.LogWarning("PhotonNetwork.room is null. You don't have to call LeaveRoom() when you're not in one. State: " + PhotonNetwork.connectionStateDetailed);
		}
		return PhotonNetwork.networkingPeer.OpLeave();
	}

	// Token: 0x060004DD RID: 1245 RVA: 0x000167BD File Offset: 0x00014BBD
	public static bool GetCustomRoomList(TypedLobby typedLobby, string sqlLobbyFilter)
	{
		return PhotonNetwork.networkingPeer.OpGetGameList(typedLobby, sqlLobbyFilter);
	}

	// Token: 0x060004DE RID: 1246 RVA: 0x000167CB File Offset: 0x00014BCB
	public static RoomInfo[] GetRoomList()
	{
		if (PhotonNetwork.offlineMode || PhotonNetwork.networkingPeer == null)
		{
			return new RoomInfo[0];
		}
		return PhotonNetwork.networkingPeer.mGameListCopy;
	}

	// Token: 0x060004DF RID: 1247 RVA: 0x000167F4 File Offset: 0x00014BF4
	public static void SetPlayerCustomProperties(Hashtable customProperties)
	{
		if (customProperties == null)
		{
			customProperties = new Hashtable();
			foreach (object obj in PhotonNetwork.player.CustomProperties.Keys)
			{
				customProperties[(string)obj] = null;
			}
		}
		if (PhotonNetwork.room != null && PhotonNetwork.room.IsLocalClientInside)
		{
			PhotonNetwork.player.SetCustomProperties(customProperties, null, false);
		}
		else
		{
			PhotonNetwork.player.InternalCacheProperties(customProperties);
		}
	}

	// Token: 0x060004E0 RID: 1248 RVA: 0x000168A4 File Offset: 0x00014CA4
	public static void RemovePlayerCustomProperties(string[] customPropertiesToDelete)
	{
		if (customPropertiesToDelete == null || customPropertiesToDelete.Length == 0 || PhotonNetwork.player.CustomProperties == null)
		{
			PhotonNetwork.player.CustomProperties = new Hashtable();
			return;
		}
		foreach (string text in customPropertiesToDelete)
		{
			if (PhotonNetwork.player.CustomProperties.ContainsKey(text))
			{
				PhotonNetwork.player.CustomProperties.Remove(text);
			}
		}
	}

	// Token: 0x060004E1 RID: 1249 RVA: 0x0001691C File Offset: 0x00014D1C
	public static bool RaiseEvent(byte eventCode, object eventContent, bool sendReliable, RaiseEventOptions options)
	{
		if (!PhotonNetwork.inRoom || eventCode >= 200)
		{
			global::UnityEngine.Debug.LogWarning("RaiseEvent() failed. Your event is not being sent! Check if your are in a Room and the eventCode must be less than 200 (0..199).");
			return false;
		}
		return PhotonNetwork.networkingPeer.OpRaiseEvent(eventCode, eventContent, sendReliable, options);
	}

	// Token: 0x060004E2 RID: 1250 RVA: 0x00016950 File Offset: 0x00014D50
	public static int AllocateViewID()
	{
		int num = PhotonNetwork.AllocateViewID(PhotonNetwork.player.ID);
		PhotonNetwork.manuallyAllocatedViewIds.Add(num);
		return num;
	}

	// Token: 0x060004E3 RID: 1251 RVA: 0x0001697C File Offset: 0x00014D7C
	public static int AllocateSceneViewID()
	{
		if (!PhotonNetwork.isMasterClient)
		{
			global::UnityEngine.Debug.LogError("Only the Master Client can AllocateSceneViewID(). Check PhotonNetwork.isMasterClient!");
			return -1;
		}
		int num = PhotonNetwork.AllocateViewID(0);
		PhotonNetwork.manuallyAllocatedViewIds.Add(num);
		return num;
	}

	// Token: 0x060004E4 RID: 1252 RVA: 0x000169B4 File Offset: 0x00014DB4
	private static int AllocateViewID(int ownerId)
	{
		if (ownerId == 0)
		{
			int num = PhotonNetwork.lastUsedViewSubIdStatic;
			int num2 = ownerId * PhotonNetwork.MAX_VIEW_IDS;
			for (int i = 1; i < PhotonNetwork.MAX_VIEW_IDS; i++)
			{
				num = (num + 1) % PhotonNetwork.MAX_VIEW_IDS;
				if (num != 0)
				{
					int num3 = num + num2;
					if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(num3))
					{
						PhotonNetwork.lastUsedViewSubIdStatic = num;
						return num3;
					}
				}
			}
			throw new Exception(string.Format("AllocateViewID() failed. Room (user {0}) is out of 'scene' viewIDs. It seems all available are in use.", ownerId));
		}
		int num4 = PhotonNetwork.lastUsedViewSubId;
		int num5 = ownerId * PhotonNetwork.MAX_VIEW_IDS;
		for (int j = 1; j < PhotonNetwork.MAX_VIEW_IDS; j++)
		{
			num4 = (num4 + 1) % PhotonNetwork.MAX_VIEW_IDS;
			if (num4 != 0)
			{
				int num6 = num4 + num5;
				if (!PhotonNetwork.networkingPeer.photonViewList.ContainsKey(num6) && !PhotonNetwork.manuallyAllocatedViewIds.Contains(num6))
				{
					PhotonNetwork.lastUsedViewSubId = num4;
					return num6;
				}
			}
		}
		throw new Exception(string.Format("AllocateViewID() failed. User {0} is out of subIds, as all viewIDs are used.", ownerId));
	}

	// Token: 0x060004E5 RID: 1253 RVA: 0x00016AC8 File Offset: 0x00014EC8
	private static int[] AllocateSceneViewIDs(int countOfNewViews)
	{
		int[] array = new int[countOfNewViews];
		for (int i = 0; i < countOfNewViews; i++)
		{
			array[i] = PhotonNetwork.AllocateViewID(0);
		}
		return array;
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00016AF8 File Offset: 0x00014EF8
	public static void UnAllocateViewID(int viewID)
	{
		PhotonNetwork.manuallyAllocatedViewIds.Remove(viewID);
		if (PhotonNetwork.networkingPeer.photonViewList.ContainsKey(viewID))
		{
			global::UnityEngine.Debug.LogWarning(string.Format("UnAllocateViewID() should be called after the PhotonView was destroyed (GameObject.Destroy()). ViewID: {0} still found in: {1}", viewID, PhotonNetwork.networkingPeer.photonViewList[viewID]));
		}
	}

	// Token: 0x060004E7 RID: 1255 RVA: 0x00016B4B File Offset: 0x00014F4B
	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, byte group)
	{
		return PhotonNetwork.Instantiate(prefabName, position, rotation, group, null);
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00016B58 File Offset: 0x00014F58
	public static GameObject Instantiate(string prefabName, Vector3 position, Quaternion rotation, byte group, object[] data)
	{
		if (!PhotonNetwork.connected || (PhotonNetwork.InstantiateInRoomOnly && !PhotonNetwork.inRoom))
		{
			global::UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Failed to Instantiate prefab: ",
				prefabName,
				". Client should be in a room. Current connectionStateDetailed: ",
				PhotonNetwork.connectionStateDetailed
			}));
			return null;
		}
		GameObject gameObject;
		if (!PhotonNetwork.UsePrefabCache || !PhotonNetwork.PrefabCache.TryGetValue(prefabName, out gameObject))
		{
			gameObject = (GameObject)Resources.Load(prefabName, typeof(GameObject));
			if (PhotonNetwork.UsePrefabCache)
			{
				PhotonNetwork.PrefabCache.Add(prefabName, gameObject);
			}
		}
		if (gameObject == null)
		{
			global::UnityEngine.Debug.LogError("Failed to Instantiate prefab: " + prefabName + ". Verify the Prefab is in a Resources folder (and not in a subfolder)");
			return null;
		}
		if (gameObject.GetComponent<PhotonView>() == null)
		{
			global::UnityEngine.Debug.LogError("Failed to Instantiate prefab:" + prefabName + ". Prefab must have a PhotonView component.");
			return null;
		}
		Component[] photonViewsInChildren = gameObject.GetPhotonViewsInChildren();
		int[] array = new int[photonViewsInChildren.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = PhotonNetwork.AllocateViewID(PhotonNetwork.player.ID);
		}
		Hashtable hashtable = PhotonNetwork.networkingPeer.SendInstantiate(prefabName, position, rotation, group, array, data, false);
		return PhotonNetwork.networkingPeer.DoInstantiate(hashtable, PhotonNetwork.networkingPeer.LocalPlayer, gameObject);
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x00016CAC File Offset: 0x000150AC
	public static GameObject InstantiateSceneObject(string prefabName, Vector3 position, Quaternion rotation, byte group, object[] data)
	{
		if (!PhotonNetwork.connected || (PhotonNetwork.InstantiateInRoomOnly && !PhotonNetwork.inRoom))
		{
			global::UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Failed to InstantiateSceneObject prefab: ",
				prefabName,
				". Client should be in a room. Current connectionStateDetailed: ",
				PhotonNetwork.connectionStateDetailed
			}));
			return null;
		}
		if (!PhotonNetwork.isMasterClient)
		{
			global::UnityEngine.Debug.LogError("Failed to InstantiateSceneObject prefab: " + prefabName + ". Client is not the MasterClient in this room.");
			return null;
		}
		GameObject gameObject;
		if (!PhotonNetwork.UsePrefabCache || !PhotonNetwork.PrefabCache.TryGetValue(prefabName, out gameObject))
		{
			gameObject = (GameObject)Resources.Load(prefabName, typeof(GameObject));
			if (PhotonNetwork.UsePrefabCache)
			{
				PhotonNetwork.PrefabCache.Add(prefabName, gameObject);
			}
		}
		if (gameObject == null)
		{
			global::UnityEngine.Debug.LogError("Failed to InstantiateSceneObject prefab: " + prefabName + ". Verify the Prefab is in a Resources folder (and not in a subfolder)");
			return null;
		}
		if (gameObject.GetComponent<PhotonView>() == null)
		{
			global::UnityEngine.Debug.LogError("Failed to InstantiateSceneObject prefab:" + prefabName + ". Prefab must have a PhotonView component.");
			return null;
		}
		Component[] photonViewsInChildren = gameObject.GetPhotonViewsInChildren();
		int[] array = PhotonNetwork.AllocateSceneViewIDs(photonViewsInChildren.Length);
		if (array == null)
		{
			global::UnityEngine.Debug.LogError(string.Concat(new object[]
			{
				"Failed to InstantiateSceneObject prefab: ",
				prefabName,
				". No ViewIDs are free to use. Max is: ",
				PhotonNetwork.MAX_VIEW_IDS
			}));
			return null;
		}
		Hashtable hashtable = PhotonNetwork.networkingPeer.SendInstantiate(prefabName, position, rotation, group, array, data, true);
		return PhotonNetwork.networkingPeer.DoInstantiate(hashtable, PhotonNetwork.networkingPeer.LocalPlayer, gameObject);
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00016E30 File Offset: 0x00015230
	public static int GetPing()
	{
		return PhotonNetwork.networkingPeer.RoundTripTime;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00016E3C File Offset: 0x0001523C
	public static void FetchServerTimestamp()
	{
		if (PhotonNetwork.networkingPeer != null)
		{
			PhotonNetwork.networkingPeer.FetchServerTimestamp();
		}
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00016E52 File Offset: 0x00015252
	public static void SendOutgoingCommands()
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		while (PhotonNetwork.networkingPeer.SendOutgoingCommands())
		{
		}
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00016E74 File Offset: 0x00015274
	public static bool CloseConnection(PhotonPlayer kickPlayer)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return false;
		}
		if (!PhotonNetwork.player.IsMasterClient)
		{
			global::UnityEngine.Debug.LogError("CloseConnection: Only the masterclient can kick another player.");
			return false;
		}
		if (kickPlayer == null)
		{
			global::UnityEngine.Debug.LogError("CloseConnection: No such player connected!");
			return false;
		}
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			TargetActors = new int[] { kickPlayer.ID }
		};
		return PhotonNetwork.networkingPeer.OpRaiseEvent(203, null, true, raiseEventOptions);
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00016EEC File Offset: 0x000152EC
	public static bool SetMasterClient(PhotonPlayer masterClientPlayer)
	{
		if (!PhotonNetwork.inRoom || !PhotonNetwork.VerifyCanUseNetwork() || PhotonNetwork.offlineMode)
		{
			if (PhotonNetwork.logLevel == PhotonLogLevel.Informational)
			{
				global::UnityEngine.Debug.Log("Can not SetMasterClient(). Not in room or in offlineMode.");
			}
			return false;
		}
		if (PhotonNetwork.room.serverSideMasterClient)
		{
			Hashtable hashtable = new Hashtable { { 248, masterClientPlayer.ID } };
			Hashtable hashtable2 = new Hashtable { 
			{
				248,
				PhotonNetwork.networkingPeer.mMasterClientId
			} };
			return PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, hashtable2, false);
		}
		return PhotonNetwork.isMasterClient && PhotonNetwork.networkingPeer.SetMasterClient(masterClientPlayer.ID, true);
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00016FB2 File Offset: 0x000153B2
	public static void Destroy(PhotonView targetView)
	{
		if (targetView != null)
		{
			PhotonNetwork.networkingPeer.RemoveInstantiatedGO(targetView.gameObject, !PhotonNetwork.inRoom);
		}
		else
		{
			global::UnityEngine.Debug.LogError("Destroy(targetPhotonView) failed, cause targetPhotonView is null.");
		}
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00016FE7 File Offset: 0x000153E7
	public static void Destroy(GameObject targetGo)
	{
		PhotonNetwork.networkingPeer.RemoveInstantiatedGO(targetGo, !PhotonNetwork.inRoom);
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x00016FFC File Offset: 0x000153FC
	public static void DestroyPlayerObjects(PhotonPlayer targetPlayer)
	{
		if (PhotonNetwork.player == null)
		{
			global::UnityEngine.Debug.LogError("DestroyPlayerObjects() failed, cause parameter 'targetPlayer' was null.");
		}
		PhotonNetwork.DestroyPlayerObjects(targetPlayer.ID);
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00017020 File Offset: 0x00015420
	public static void DestroyPlayerObjects(int targetPlayerId)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (PhotonNetwork.player.IsMasterClient || targetPlayerId == PhotonNetwork.player.ID)
		{
			PhotonNetwork.networkingPeer.DestroyPlayerObjects(targetPlayerId, false);
		}
		else
		{
			global::UnityEngine.Debug.LogError("DestroyPlayerObjects() failed, cause players can only destroy their own GameObjects. A Master Client can destroy anyone's. This is master: " + PhotonNetwork.isMasterClient);
		}
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00017081 File Offset: 0x00015481
	public static void DestroyAll()
	{
		if (PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.networkingPeer.DestroyAll(false);
		}
		else
		{
			global::UnityEngine.Debug.LogError("Couldn't call DestroyAll() as only the master client is allowed to call this.");
		}
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x000170A7 File Offset: 0x000154A7
	public static void RemoveRPCs(PhotonPlayer targetPlayer)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (!targetPlayer.IsLocal && !PhotonNetwork.isMasterClient)
		{
			global::UnityEngine.Debug.LogError("Error; Only the MasterClient can call RemoveRPCs for other players.");
			return;
		}
		PhotonNetwork.networkingPeer.OpCleanRpcBuffer(targetPlayer.ID);
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x000170E4 File Offset: 0x000154E4
	public static void RemoveRPCs(PhotonView targetPhotonView)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.CleanRpcBufferIfMine(targetPhotonView);
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x000170FC File Offset: 0x000154FC
	public static void RemoveRPCsInGroup(int targetGroup)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.RemoveRPCsInGroup(targetGroup);
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x00017114 File Offset: 0x00015514
	internal static void RPC(PhotonView view, string methodName, PhotonTargets target, bool encrypt, params object[] parameters)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (PhotonNetwork.room == null)
		{
			global::UnityEngine.Debug.LogWarning("RPCs can only be sent in rooms. Call of \"" + methodName + "\" gets executed locally only, if at all.");
			return;
		}
		if (PhotonNetwork.networkingPeer != null)
		{
			if (PhotonNetwork.room.serverSideMasterClient)
			{
				PhotonNetwork.networkingPeer.RPC(view, methodName, target, null, encrypt, parameters);
			}
			else if (PhotonNetwork.networkingPeer.hasSwitchedMC && target == PhotonTargets.MasterClient)
			{
				PhotonNetwork.networkingPeer.RPC(view, methodName, PhotonTargets.Others, PhotonNetwork.masterClient, encrypt, parameters);
			}
			else
			{
				PhotonNetwork.networkingPeer.RPC(view, methodName, target, null, encrypt, parameters);
			}
		}
		else
		{
			global::UnityEngine.Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
		}
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x000171D8 File Offset: 0x000155D8
	internal static void RPC(PhotonView view, string methodName, PhotonPlayer targetPlayer, bool encrpyt, params object[] parameters)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (PhotonNetwork.room == null)
		{
			global::UnityEngine.Debug.LogWarning("RPCs can only be sent in rooms. Call of \"" + methodName + "\" gets executed locally only, if at all.");
			return;
		}
		if (PhotonNetwork.player == null)
		{
			global::UnityEngine.Debug.LogError("RPC can't be sent to target PhotonPlayer being null! Did not send \"" + methodName + "\" call.");
		}
		if (PhotonNetwork.networkingPeer != null)
		{
			PhotonNetwork.networkingPeer.RPC(view, methodName, PhotonTargets.Others, targetPlayer, encrpyt, parameters);
		}
		else
		{
			global::UnityEngine.Debug.LogWarning("Could not execute RPC " + methodName + ". Possible scene loading in progress?");
		}
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x00017264 File Offset: 0x00015664
	public static void CacheSendMonoMessageTargets(Type type)
	{
		if (type == null)
		{
			type = PhotonNetwork.SendMonoMessageTargetType;
		}
		PhotonNetwork.SendMonoMessageTargets = PhotonNetwork.FindGameObjectsWithComponent(type);
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00017280 File Offset: 0x00015680
	public static HashSet<GameObject> FindGameObjectsWithComponent(Type type)
	{
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		Component[] array = (Component[])global::UnityEngine.Object.FindObjectsOfType(type);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				hashSet.Add(array[i].gameObject);
			}
		}
		return hashSet;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x000172D1 File Offset: 0x000156D1
	[Obsolete("Use SetInterestGroups(byte group, bool enabled) instead.")]
	public static void SetReceivingEnabled(int group, bool enabled)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.SetInterestGroups((byte)group, enabled);
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x000172E8 File Offset: 0x000156E8
	public static void SetInterestGroups(byte group, bool enabled)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		if (enabled)
		{
			byte[] array = new byte[] { group };
			PhotonNetwork.networkingPeer.SetInterestGroups(null, array);
		}
		else
		{
			byte[] array2 = new byte[] { group };
			PhotonNetwork.networkingPeer.SetInterestGroups(array2, null);
		}
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x0001733C File Offset: 0x0001573C
	[Obsolete("Use SetInterestGroups(byte[] disableGroups, byte[] enableGroups) instead. Mind the parameter order!")]
	public static void SetReceivingEnabled(int[] enableGroups, int[] disableGroups)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		byte[] array = null;
		byte[] array2 = null;
		if (enableGroups != null)
		{
			array2 = new byte[enableGroups.Length];
			Array.Copy(enableGroups, array2, enableGroups.Length);
		}
		if (disableGroups != null)
		{
			array = new byte[disableGroups.Length];
			Array.Copy(disableGroups, array, disableGroups.Length);
		}
		PhotonNetwork.networkingPeer.SetInterestGroups(array, array2);
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x00017396 File Offset: 0x00015796
	public static void SetInterestGroups(byte[] disableGroups, byte[] enableGroups)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetInterestGroups(disableGroups, enableGroups);
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x000173AF File Offset: 0x000157AF
	[Obsolete("Use SetSendingEnabled(byte group, bool enabled). Interest Groups have a byte-typed ID. Mind the parameter order.")]
	public static void SetSendingEnabled(int group, bool enabled)
	{
		PhotonNetwork.SetSendingEnabled((byte)group, enabled);
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x000173B9 File Offset: 0x000157B9
	public static void SetSendingEnabled(byte group, bool enabled)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetSendingEnabled(group, enabled);
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x000173D4 File Offset: 0x000157D4
	[Obsolete("Use SetSendingEnabled(byte group, bool enabled). Interest Groups have a byte-typed ID. Mind the parameter order.")]
	public static void SetSendingEnabled(int[] enableGroups, int[] disableGroups)
	{
		byte[] array = null;
		byte[] array2 = null;
		if (enableGroups != null)
		{
			array2 = new byte[enableGroups.Length];
			Array.Copy(enableGroups, array2, enableGroups.Length);
		}
		if (disableGroups != null)
		{
			array = new byte[disableGroups.Length];
			Array.Copy(disableGroups, array, disableGroups.Length);
		}
		PhotonNetwork.SetSendingEnabled(array, array2);
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x0001741E File Offset: 0x0001581E
	public static void SetSendingEnabled(byte[] disableGroups, byte[] enableGroups)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetSendingEnabled(disableGroups, enableGroups);
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x00017437 File Offset: 0x00015837
	public static void SetLevelPrefix(short prefix)
	{
		if (!PhotonNetwork.VerifyCanUseNetwork())
		{
			return;
		}
		PhotonNetwork.networkingPeer.SetLevelPrefix(prefix);
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x0001744F File Offset: 0x0001584F
	public static void LoadLevel(int levelNumber)
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(levelNumber);
		PhotonNetwork.isMessageQueueRunning = false;
		PhotonNetwork.networkingPeer.loadingLevelAndPausedNetwork = true;
		SceneManager.LoadScene(levelNumber);
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00017478 File Offset: 0x00015878
	public static void LoadLevel(string levelName)
	{
		PhotonNetwork.networkingPeer.SetLevelInPropsIfSynced(levelName);
		PhotonNetwork.isMessageQueueRunning = false;
		PhotonNetwork.networkingPeer.loadingLevelAndPausedNetwork = true;
		SceneManager.LoadScene(levelName);
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x0001749C File Offset: 0x0001589C
	public static bool WebRpc(string name, object parameters)
	{
		return PhotonNetwork.networkingPeer.WebRpc(name, parameters);
	}

	// Token: 0x0400037F RID: 895
	public const string versionPUN = "1.87";

	// Token: 0x04000381 RID: 897
	internal static readonly PhotonHandler photonMono;

	// Token: 0x04000382 RID: 898
	internal static NetworkingPeer networkingPeer;

	// Token: 0x04000383 RID: 899
	public static readonly int MAX_VIEW_IDS = 1000;

	// Token: 0x04000384 RID: 900
	internal const string serverSettingsAssetFile = "PhotonServerSettings";

	// Token: 0x04000385 RID: 901
	public static ServerSettings PhotonServerSettings = (ServerSettings)Resources.Load("PhotonServerSettings", typeof(ServerSettings));

	// Token: 0x04000386 RID: 902
	public static bool InstantiateInRoomOnly = true;

	// Token: 0x04000387 RID: 903
	public static PhotonLogLevel logLevel = PhotonLogLevel.ErrorsOnly;

	// Token: 0x04000389 RID: 905
	public static float precisionForVectorSynchronization = 9.9E-05f;

	// Token: 0x0400038A RID: 906
	public static float precisionForQuaternionSynchronization = 1f;

	// Token: 0x0400038B RID: 907
	public static float precisionForFloatSynchronization = 0.01f;

	// Token: 0x0400038C RID: 908
	public static bool UseRpcMonoBehaviourCache;

	// Token: 0x0400038D RID: 909
	public static bool UsePrefabCache = true;

	// Token: 0x0400038E RID: 910
	public static Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();

	// Token: 0x0400038F RID: 911
	public static HashSet<GameObject> SendMonoMessageTargets;

	// Token: 0x04000390 RID: 912
	public static Type SendMonoMessageTargetType = typeof(MonoBehaviour);

	// Token: 0x04000391 RID: 913
	public static bool StartRpcsAsCoroutine = true;

	// Token: 0x04000392 RID: 914
	private static bool isOfflineMode = false;

	// Token: 0x04000393 RID: 915
	private static Room offlineModeRoom = null;

	// Token: 0x04000394 RID: 916
	[Obsolete("Used for compatibility with Unity networking only.")]
	public static int maxConnections;

	// Token: 0x04000395 RID: 917
	private static bool _mAutomaticallySyncScene = false;

	// Token: 0x04000396 RID: 918
	private static bool m_autoCleanUpPlayerObjects = true;

	// Token: 0x04000397 RID: 919
	private static int sendInterval = 50;

	// Token: 0x04000398 RID: 920
	private static int sendIntervalOnSerialize = 100;

	// Token: 0x04000399 RID: 921
	private static bool m_isMessageQueueRunning = true;

	// Token: 0x0400039A RID: 922
	private static bool UsePreciseTimer = false;

	// Token: 0x0400039B RID: 923
	private static Stopwatch startupStopwatch;

	// Token: 0x0400039C RID: 924
	public static float BackgroundTimeout = 60f;

	// Token: 0x0400039D RID: 925
	public static PhotonNetwork.EventCallback OnEventCall;

	// Token: 0x0400039E RID: 926
	internal static int lastUsedViewSubId = 0;

	// Token: 0x0400039F RID: 927
	internal static int lastUsedViewSubIdStatic = 0;

	// Token: 0x040003A0 RID: 928
	internal static List<int> manuallyAllocatedViewIds = new List<int>();

	// Token: 0x0200008A RID: 138
	// (Invoke) Token: 0x06000509 RID: 1289
	public delegate void EventCallback(byte eventCode, object content, int senderId);
}
