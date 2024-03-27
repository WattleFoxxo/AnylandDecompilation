using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200007C RID: 124
internal class NetworkingPeer : LoadBalancingPeer, IPhotonPeerListener
{
	// Token: 0x06000379 RID: 889 RVA: 0x0000DD74 File Offset: 0x0000C174
	public NetworkingPeer(string playername, ConnectionProtocol connectionProtocol)
		: base(connectionProtocol)
	{
		base.Listener = this;
		base.LimitOfUnreliableCommands = 40;
		this.lobby = TypedLobby.Default;
		this.PlayerName = playername;
		this.LocalPlayer = new PhotonPlayer(true, -1, this.playername);
		this.AddNewPlayer(this.LocalPlayer.ID, this.LocalPlayer);
		this.rpcShortcuts = new Dictionary<string, int>(PhotonNetwork.PhotonServerSettings.RpcList.Count);
		for (int i = 0; i < PhotonNetwork.PhotonServerSettings.RpcList.Count; i++)
		{
			string text = PhotonNetwork.PhotonServerSettings.RpcList[i];
			this.rpcShortcuts[text] = i;
		}
		this.State = ClientState.PeerCreated;
	}

	// Token: 0x17000092 RID: 146
	// (get) Token: 0x0600037A RID: 890 RVA: 0x0000DEF4 File Offset: 0x0000C2F4
	protected internal string AppVersion
	{
		get
		{
			return string.Format("{0}_{1}", PhotonNetwork.gameVersion, "1.87");
		}
	}

	// Token: 0x17000093 RID: 147
	// (get) Token: 0x0600037B RID: 891 RVA: 0x0000DF0A File Offset: 0x0000C30A
	// (set) Token: 0x0600037C RID: 892 RVA: 0x0000DF12 File Offset: 0x0000C312
	public AuthenticationValues AuthValues { get; set; }

	// Token: 0x17000094 RID: 148
	// (get) Token: 0x0600037D RID: 893 RVA: 0x0000DF1B File Offset: 0x0000C31B
	private string TokenForInit
	{
		get
		{
			if (this.AuthMode == AuthModeOption.Auth)
			{
				return null;
			}
			return (this.AuthValues == null) ? null : this.AuthValues.Token;
		}
	}

	// Token: 0x17000095 RID: 149
	// (get) Token: 0x0600037E RID: 894 RVA: 0x0000DF46 File Offset: 0x0000C346
	// (set) Token: 0x0600037F RID: 895 RVA: 0x0000DF4E File Offset: 0x0000C34E
	public bool IsUsingNameServer { get; protected internal set; }

	// Token: 0x17000096 RID: 150
	// (get) Token: 0x06000380 RID: 896 RVA: 0x0000DF57 File Offset: 0x0000C357
	public string NameServerAddress
	{
		get
		{
			return this.GetNameServerAddress();
		}
	}

	// Token: 0x17000097 RID: 151
	// (get) Token: 0x06000381 RID: 897 RVA: 0x0000DF5F File Offset: 0x0000C35F
	// (set) Token: 0x06000382 RID: 898 RVA: 0x0000DF67 File Offset: 0x0000C367
	public string MasterServerAddress { get; protected internal set; }

	// Token: 0x17000098 RID: 152
	// (get) Token: 0x06000383 RID: 899 RVA: 0x0000DF70 File Offset: 0x0000C370
	// (set) Token: 0x06000384 RID: 900 RVA: 0x0000DF78 File Offset: 0x0000C378
	public string GameServerAddress { get; protected internal set; }

	// Token: 0x17000099 RID: 153
	// (get) Token: 0x06000385 RID: 901 RVA: 0x0000DF81 File Offset: 0x0000C381
	// (set) Token: 0x06000386 RID: 902 RVA: 0x0000DF89 File Offset: 0x0000C389
	protected internal ServerConnection Server { get; private set; }

	// Token: 0x1700009A RID: 154
	// (get) Token: 0x06000387 RID: 903 RVA: 0x0000DF92 File Offset: 0x0000C392
	// (set) Token: 0x06000388 RID: 904 RVA: 0x0000DF9A File Offset: 0x0000C39A
	public ClientState State { get; internal set; }

	// Token: 0x1700009B RID: 155
	// (get) Token: 0x06000389 RID: 905 RVA: 0x0000DFA3 File Offset: 0x0000C3A3
	// (set) Token: 0x0600038A RID: 906 RVA: 0x0000DFAB File Offset: 0x0000C3AB
	public TypedLobby lobby { get; set; }

	// Token: 0x1700009C RID: 156
	// (get) Token: 0x0600038B RID: 907 RVA: 0x0000DFB4 File Offset: 0x0000C3B4
	private bool requestLobbyStatistics
	{
		get
		{
			return PhotonNetwork.EnableLobbyStatistics && this.Server == ServerConnection.MasterServer;
		}
	}

	// Token: 0x1700009D RID: 157
	// (get) Token: 0x0600038C RID: 908 RVA: 0x0000DFCC File Offset: 0x0000C3CC
	// (set) Token: 0x0600038D RID: 909 RVA: 0x0000DFD4 File Offset: 0x0000C3D4
	public string PlayerName
	{
		get
		{
			return this.playername;
		}
		set
		{
			if (string.IsNullOrEmpty(value) || value.Equals(this.playername))
			{
				return;
			}
			if (this.LocalPlayer != null)
			{
				this.LocalPlayer.NickName = value;
			}
			this.playername = value;
			if (this.CurrentRoom != null)
			{
				this.SendPlayerName();
			}
		}
	}

	// Token: 0x1700009E RID: 158
	// (get) Token: 0x0600038E RID: 910 RVA: 0x0000E02D File Offset: 0x0000C42D
	// (set) Token: 0x0600038F RID: 911 RVA: 0x0000E052 File Offset: 0x0000C452
	public Room CurrentRoom
	{
		get
		{
			if (this.currentRoom != null && this.currentRoom.IsLocalClientInside)
			{
				return this.currentRoom;
			}
			return null;
		}
		private set
		{
			this.currentRoom = value;
		}
	}

	// Token: 0x1700009F RID: 159
	// (get) Token: 0x06000390 RID: 912 RVA: 0x0000E05B File Offset: 0x0000C45B
	// (set) Token: 0x06000391 RID: 913 RVA: 0x0000E063 File Offset: 0x0000C463
	public PhotonPlayer LocalPlayer { get; internal set; }

	// Token: 0x170000A0 RID: 160
	// (get) Token: 0x06000392 RID: 914 RVA: 0x0000E06C File Offset: 0x0000C46C
	// (set) Token: 0x06000393 RID: 915 RVA: 0x0000E074 File Offset: 0x0000C474
	public int PlayersOnMasterCount { get; internal set; }

	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000394 RID: 916 RVA: 0x0000E07D File Offset: 0x0000C47D
	// (set) Token: 0x06000395 RID: 917 RVA: 0x0000E085 File Offset: 0x0000C485
	public int PlayersInRoomsCount { get; internal set; }

	// Token: 0x170000A2 RID: 162
	// (get) Token: 0x06000396 RID: 918 RVA: 0x0000E08E File Offset: 0x0000C48E
	// (set) Token: 0x06000397 RID: 919 RVA: 0x0000E096 File Offset: 0x0000C496
	public int RoomsCount { get; internal set; }

	// Token: 0x170000A3 RID: 163
	// (get) Token: 0x06000398 RID: 920 RVA: 0x0000E09F File Offset: 0x0000C49F
	protected internal int FriendListAge
	{
		get
		{
			return (!this.isFetchingFriendList && this.friendListTimestamp != 0) ? (Environment.TickCount - this.friendListTimestamp) : 0;
		}
	}

	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06000399 RID: 921 RVA: 0x0000E0C9 File Offset: 0x0000C4C9
	public bool IsAuthorizeSecretAvailable
	{
		get
		{
			return this.AuthValues != null && !string.IsNullOrEmpty(this.AuthValues.Token);
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x0600039A RID: 922 RVA: 0x0000E0EC File Offset: 0x0000C4EC
	// (set) Token: 0x0600039B RID: 923 RVA: 0x0000E0F4 File Offset: 0x0000C4F4
	public List<Region> AvailableRegions { get; protected internal set; }

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x0600039C RID: 924 RVA: 0x0000E0FD File Offset: 0x0000C4FD
	// (set) Token: 0x0600039D RID: 925 RVA: 0x0000E105 File Offset: 0x0000C505
	public CloudRegionCode CloudRegion { get; protected internal set; }

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x0600039E RID: 926 RVA: 0x0000E10E File Offset: 0x0000C50E
	// (set) Token: 0x0600039F RID: 927 RVA: 0x0000E142 File Offset: 0x0000C542
	public int mMasterClientId
	{
		get
		{
			if (PhotonNetwork.offlineMode)
			{
				return this.LocalPlayer.ID;
			}
			return (this.CurrentRoom != null) ? this.CurrentRoom.MasterClientId : 0;
		}
		private set
		{
			if (this.CurrentRoom != null)
			{
				this.CurrentRoom.MasterClientId = value;
			}
		}
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0000E15C File Offset: 0x0000C55C
	private string GetNameServerAddress()
	{
		ConnectionProtocol transportProtocol = base.TransportProtocol;
		int num = 0;
		NetworkingPeer.ProtocolToNameServerPort.TryGetValue(transportProtocol, out num);
		string text = string.Empty;
		if (transportProtocol == ConnectionProtocol.WebSocket)
		{
			text = "ws://";
		}
		else if (transportProtocol == ConnectionProtocol.WebSocketSecure)
		{
			text = "wss://";
		}
		return string.Format("{0}{1}:{2}", text, "ns.exitgames.com", num);
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0000E1BD File Offset: 0x0000C5BD
	public override bool Connect(string serverAddress, string applicationName)
	{
		Debug.LogError("Avoid using this directly. Thanks.");
		return false;
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0000E1CA File Offset: 0x0000C5CA
	public bool ReconnectToMaster()
	{
		if (this.AuthValues == null)
		{
			Debug.LogWarning("ReconnectToMaster() with AuthValues == null is not correct!");
			this.AuthValues = new AuthenticationValues();
		}
		this.AuthValues.Token = this.tokenCache;
		return this.Connect(this.MasterServerAddress, ServerConnection.MasterServer);
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0000E20C File Offset: 0x0000C60C
	public bool ReconnectAndRejoin()
	{
		if (this.AuthValues == null)
		{
			Debug.LogWarning("ReconnectAndRejoin() with AuthValues == null is not correct!");
			this.AuthValues = new AuthenticationValues();
		}
		this.AuthValues.Token = this.tokenCache;
		if (!string.IsNullOrEmpty(this.GameServerAddress) && this.enterRoomParamsCache != null)
		{
			this.lastJoinType = JoinType.JoinRoom;
			this.enterRoomParamsCache.RejoinOnly = true;
			return this.Connect(this.GameServerAddress, ServerConnection.GameServer);
		}
		return false;
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0000E288 File Offset: 0x0000C688
	public bool Connect(string serverAddress, ServerConnection type)
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		if (this.State == ClientState.Disconnecting)
		{
			Debug.LogError("Connect() failed. Can't connect while disconnecting (still). Current state: " + PhotonNetwork.connectionStateDetailed);
			return false;
		}
		this.cachedProtocolType = type;
		this.cachedServerAddress = serverAddress;
		this.cachedApplicationName = string.Empty;
		this.SetupProtocol(type);
		bool flag = base.Connect(serverAddress, string.Empty, this.TokenForInit);
		if (flag)
		{
			if (type != ServerConnection.NameServer)
			{
				if (type != ServerConnection.MasterServer)
				{
					if (type == ServerConnection.GameServer)
					{
						this.State = ClientState.ConnectingToGameserver;
					}
				}
				else
				{
					this.State = ClientState.ConnectingToMasterserver;
				}
			}
			else
			{
				this.State = ClientState.ConnectingToNameServer;
			}
		}
		return flag;
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0000E34C File Offset: 0x0000C74C
	private bool Reconnect()
	{
		this._isReconnecting = true;
		PhotonNetwork.SwitchToProtocol(PhotonNetwork.PhotonServerSettings.Protocol);
		this.SetupProtocol(this.cachedProtocolType);
		bool flag = base.Connect(this.cachedServerAddress, this.cachedApplicationName, this.TokenForInit);
		if (flag)
		{
			ServerConnection serverConnection = this.cachedProtocolType;
			if (serverConnection != ServerConnection.NameServer)
			{
				if (serverConnection != ServerConnection.MasterServer)
				{
					if (serverConnection == ServerConnection.GameServer)
					{
						this.State = ClientState.ConnectingToGameserver;
					}
				}
				else
				{
					this.State = ClientState.ConnectingToMasterserver;
				}
			}
			else
			{
				this.State = ClientState.ConnectingToNameServer;
			}
		}
		return flag;
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0000E3E4 File Offset: 0x0000C7E4
	public bool ConnectToNameServer()
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		this.IsUsingNameServer = true;
		this.CloudRegion = CloudRegionCode.none;
		if (this.State == ClientState.ConnectedToNameServer)
		{
			return true;
		}
		this.SetupProtocol(ServerConnection.NameServer);
		this.cachedProtocolType = ServerConnection.NameServer;
		this.cachedServerAddress = this.NameServerAddress;
		this.cachedApplicationName = "ns";
		if (!base.Connect(this.NameServerAddress, "ns", this.TokenForInit))
		{
			return false;
		}
		this.State = ClientState.ConnectingToNameServer;
		return true;
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0000E470 File Offset: 0x0000C870
	public bool ConnectToRegionMaster(CloudRegionCode region)
	{
		if (PhotonHandler.AppQuits)
		{
			Debug.LogWarning("Ignoring Connect() because app gets closed. If this is an error, check PhotonHandler.AppQuits.");
			return false;
		}
		this.IsUsingNameServer = true;
		this.CloudRegion = region;
		if (this.State == ClientState.ConnectedToNameServer)
		{
			return this.CallAuthenticate();
		}
		this.cachedProtocolType = ServerConnection.NameServer;
		this.cachedServerAddress = this.NameServerAddress;
		this.cachedApplicationName = "ns";
		this.SetupProtocol(ServerConnection.NameServer);
		if (!base.Connect(this.NameServerAddress, "ns", this.TokenForInit))
		{
			return false;
		}
		this.State = ClientState.ConnectingToNameServer;
		return true;
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0000E504 File Offset: 0x0000C904
	protected internal void SetupProtocol(ServerConnection serverType)
	{
		ConnectionProtocol connectionProtocol = base.TransportProtocol;
		if (this.AuthMode == AuthModeOption.AuthOnceWss)
		{
			if (serverType != ServerConnection.NameServer)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.ErrorsOnly)
				{
					Debug.LogWarning("Using PhotonServerSettings.Protocol when leaving the NameServer (AuthMode is AuthOnceWss): " + PhotonNetwork.PhotonServerSettings.Protocol);
				}
				connectionProtocol = PhotonNetwork.PhotonServerSettings.Protocol;
			}
			else
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.ErrorsOnly)
				{
					Debug.LogWarning("Using WebSocket to connect NameServer (AuthMode is AuthOnceWss).");
				}
				connectionProtocol = ConnectionProtocol.WebSocketSecure;
			}
		}
		Type type = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, Assembly-CSharp", false);
		if (type == null)
		{
			type = Type.GetType("ExitGames.Client.Photon.SocketWebTcp, Assembly-CSharp-firstpass", false);
		}
		if (type != null)
		{
			this.SocketImplementationConfig[ConnectionProtocol.WebSocket] = type;
			this.SocketImplementationConfig[ConnectionProtocol.WebSocketSecure] = type;
		}
		if (PhotonHandler.PingImplementation == null)
		{
			PhotonHandler.PingImplementation = typeof(PingMono);
		}
		if (base.TransportProtocol == connectionProtocol)
		{
			return;
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.ErrorsOnly)
		{
			Debug.LogWarning(string.Concat(new object[] { "Protocol switch from: ", base.TransportProtocol, " to: ", connectionProtocol, "." }));
		}
		base.TransportProtocol = connectionProtocol;
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0000E632 File Offset: 0x0000CA32
	public override void Disconnect()
	{
		if (base.PeerState == PeerStateValue.Disconnected)
		{
			if (!PhotonHandler.AppQuits)
			{
				Debug.LogWarning(string.Format("Can't execute Disconnect() while not connected. Nothing changed. State: {0}", this.State));
			}
			return;
		}
		this.State = ClientState.Disconnecting;
		base.Disconnect();
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0000E674 File Offset: 0x0000CA74
	private bool CallAuthenticate()
	{
		AuthenticationValues authenticationValues;
		if ((authenticationValues = this.AuthValues) == null)
		{
			authenticationValues = new AuthenticationValues
			{
				UserId = this.PlayerName
			};
		}
		AuthenticationValues authenticationValues2 = authenticationValues;
		if (this.AuthMode == AuthModeOption.Auth)
		{
			return this.OpAuthenticate(this.AppId, this.AppVersion, authenticationValues2, this.CloudRegion.ToString(), this.requestLobbyStatistics);
		}
		return this.OpAuthenticateOnce(this.AppId, this.AppVersion, authenticationValues2, this.CloudRegion.ToString(), this.EncryptionMode, PhotonNetwork.PhotonServerSettings.Protocol);
	}

	// Token: 0x060003AB RID: 939 RVA: 0x0000E714 File Offset: 0x0000CB14
	private void DisconnectToReconnect()
	{
		ServerConnection server = this.Server;
		if (server != ServerConnection.NameServer)
		{
			if (server != ServerConnection.MasterServer)
			{
				if (server == ServerConnection.GameServer)
				{
					this.State = ClientState.DisconnectingFromGameserver;
					base.Disconnect();
				}
			}
			else
			{
				this.State = ClientState.DisconnectingFromMasterserver;
				base.Disconnect();
			}
		}
		else
		{
			this.State = ClientState.DisconnectingFromNameServer;
			base.Disconnect();
		}
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0000E77C File Offset: 0x0000CB7C
	public bool GetRegions()
	{
		if (this.Server != ServerConnection.NameServer)
		{
			return false;
		}
		bool flag = this.OpGetRegions(this.AppId);
		if (flag)
		{
			this.AvailableRegions = null;
		}
		return flag;
	}

	// Token: 0x060003AD RID: 941 RVA: 0x0000E7B2 File Offset: 0x0000CBB2
	public override bool OpFindFriends(string[] friendsToFind)
	{
		if (this.isFetchingFriendList)
		{
			return false;
		}
		this.friendListRequested = friendsToFind;
		this.isFetchingFriendList = true;
		return base.OpFindFriends(friendsToFind);
	}

	// Token: 0x060003AE RID: 942 RVA: 0x0000E7D8 File Offset: 0x0000CBD8
	public bool OpCreateGame(EnterRoomParams enterRoomParams)
	{
		bool flag = this.Server == ServerConnection.GameServer;
		enterRoomParams.OnGameServer = flag;
		enterRoomParams.PlayerProperties = this.GetLocalActorProperties();
		if (!flag)
		{
			this.enterRoomParamsCache = enterRoomParams;
		}
		this.lastJoinType = JoinType.CreateRoom;
		return base.OpCreateRoom(enterRoomParams);
	}

	// Token: 0x060003AF RID: 943 RVA: 0x0000E820 File Offset: 0x0000CC20
	public override bool OpJoinRoom(EnterRoomParams opParams)
	{
		bool flag = this.Server == ServerConnection.GameServer;
		opParams.OnGameServer = flag;
		if (!flag)
		{
			this.enterRoomParamsCache = opParams;
		}
		this.lastJoinType = ((!opParams.CreateIfNotExists) ? JoinType.JoinRoom : JoinType.JoinOrCreateRoom);
		return base.OpJoinRoom(opParams);
	}

	// Token: 0x060003B0 RID: 944 RVA: 0x0000E86A File Offset: 0x0000CC6A
	public override bool OpJoinRandomRoom(OpJoinRandomRoomParams opJoinRandomRoomParams)
	{
		this.enterRoomParamsCache = new EnterRoomParams();
		this.enterRoomParamsCache.Lobby = opJoinRandomRoomParams.TypedLobby;
		this.enterRoomParamsCache.ExpectedUsers = opJoinRandomRoomParams.ExpectedUsers;
		this.lastJoinType = JoinType.JoinRandomRoom;
		return base.OpJoinRandomRoom(opJoinRandomRoomParams);
	}

	// Token: 0x060003B1 RID: 945 RVA: 0x0000E8A7 File Offset: 0x0000CCA7
	public virtual bool OpLeave()
	{
		if (this.State != ClientState.Joined)
		{
			Debug.LogWarning("Not sending leave operation. State is not 'Joined': " + this.State);
			return false;
		}
		return this.OpCustom(254, null, true, 0);
	}

	// Token: 0x060003B2 RID: 946 RVA: 0x0000E8E0 File Offset: 0x0000CCE0
	public override bool OpRaiseEvent(byte eventCode, object customEventContent, bool sendReliable, RaiseEventOptions raiseEventOptions)
	{
		return !PhotonNetwork.offlineMode && base.OpRaiseEvent(eventCode, customEventContent, sendReliable, raiseEventOptions);
	}

	// Token: 0x060003B3 RID: 947 RVA: 0x0000E8FC File Offset: 0x0000CCFC
	private void ReadoutProperties(ExitGames.Client.Photon.Hashtable gameProperties, ExitGames.Client.Photon.Hashtable pActorProperties, int targetActorNr)
	{
		if (pActorProperties != null && pActorProperties.Count > 0)
		{
			if (targetActorNr > 0)
			{
				PhotonPlayer playerWithId = this.GetPlayerWithId(targetActorNr);
				if (playerWithId != null)
				{
					ExitGames.Client.Photon.Hashtable hashtable = this.ReadoutPropertiesForActorNr(pActorProperties, targetActorNr);
					playerWithId.InternalCacheProperties(hashtable);
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, new object[] { playerWithId, hashtable });
				}
			}
			else
			{
				foreach (object obj in pActorProperties.Keys)
				{
					int num = (int)obj;
					ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)pActorProperties[obj];
					string text = (string)hashtable2[byte.MaxValue];
					PhotonPlayer photonPlayer = this.GetPlayerWithId(num);
					if (photonPlayer == null)
					{
						photonPlayer = new PhotonPlayer(false, num, text);
						this.AddNewPlayer(num, photonPlayer);
					}
					photonPlayer.InternalCacheProperties(hashtable2);
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, new object[] { photonPlayer, hashtable2 });
				}
			}
		}
		if (this.CurrentRoom != null && gameProperties != null)
		{
			this.CurrentRoom.InternalCacheProperties(gameProperties);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, new object[] { gameProperties });
			if (PhotonNetwork.automaticallySyncScene)
			{
				this.LoadLevelIfSynced();
			}
		}
	}

	// Token: 0x060003B4 RID: 948 RVA: 0x0000EA54 File Offset: 0x0000CE54
	private ExitGames.Client.Photon.Hashtable ReadoutPropertiesForActorNr(ExitGames.Client.Photon.Hashtable actorProperties, int actorNr)
	{
		if (actorProperties.ContainsKey(actorNr))
		{
			return (ExitGames.Client.Photon.Hashtable)actorProperties[actorNr];
		}
		return actorProperties;
	}

	// Token: 0x060003B5 RID: 949 RVA: 0x0000EA7C File Offset: 0x0000CE7C
	public void ChangeLocalID(int newID)
	{
		if (this.LocalPlayer == null)
		{
			Debug.LogWarning(string.Format("LocalPlayer is null or not in mActors! LocalPlayer: {0} mActors==null: {1} newID: {2}", this.LocalPlayer, this.mActors == null, newID));
		}
		if (this.mActors.ContainsKey(this.LocalPlayer.ID))
		{
			this.mActors.Remove(this.LocalPlayer.ID);
		}
		this.LocalPlayer.InternalChangeLocalID(newID);
		this.mActors[this.LocalPlayer.ID] = this.LocalPlayer;
		this.RebuildPlayerListCopies();
	}

	// Token: 0x060003B6 RID: 950 RVA: 0x0000EB1D File Offset: 0x0000CF1D
	private void LeftLobbyCleanup()
	{
		this.mGameList = new Dictionary<string, RoomInfo>();
		this.mGameListCopy = new RoomInfo[0];
		if (this.insideLobby)
		{
			this.insideLobby = false;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftLobby, new object[0]);
		}
	}

	// Token: 0x060003B7 RID: 951 RVA: 0x0000EB54 File Offset: 0x0000CF54
	private void LeftRoomCleanup()
	{
		bool flag = this.CurrentRoom != null;
		bool flag2 = ((this.CurrentRoom == null) ? PhotonNetwork.autoCleanUpPlayerObjects : this.CurrentRoom.AutoCleanUp);
		this.hasSwitchedMC = false;
		this.CurrentRoom = null;
		this.mActors = new Dictionary<int, PhotonPlayer>();
		this.mPlayerListCopy = new PhotonPlayer[0];
		this.mOtherPlayerListCopy = new PhotonPlayer[0];
		this.allowedReceivingGroups = new HashSet<byte>();
		this.blockSendingGroups = new HashSet<byte>();
		this.mGameList = new Dictionary<string, RoomInfo>();
		this.mGameListCopy = new RoomInfo[0];
		this.isFetchingFriendList = false;
		this.ChangeLocalID(-1);
		if (flag2)
		{
			this.LocalCleanupAnythingInstantiated(true);
			PhotonNetwork.manuallyAllocatedViewIds = new List<int>();
		}
		if (flag)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLeftRoom, new object[0]);
		}
	}

	// Token: 0x060003B8 RID: 952 RVA: 0x0000EC24 File Offset: 0x0000D024
	protected internal void LocalCleanupAnythingInstantiated(bool destroyInstantiatedGameObjects)
	{
		if (this.tempInstantiationData.Count > 0)
		{
			Debug.LogWarning("It seems some instantiation is not completed, as instantiation data is used. You should make sure instantiations are paused when calling this method. Cleaning now, despite this.");
		}
		if (destroyInstantiatedGameObjects)
		{
			HashSet<GameObject> hashSet = new HashSet<GameObject>();
			foreach (PhotonView photonView in this.photonViewList.Values)
			{
				if (photonView.isRuntimeInstantiated)
				{
					hashSet.Add(photonView.gameObject);
				}
			}
			foreach (GameObject gameObject in hashSet)
			{
				this.RemoveInstantiatedGO(gameObject, true);
			}
		}
		this.tempInstantiationData.Clear();
		PhotonNetwork.lastUsedViewSubId = 0;
		PhotonNetwork.lastUsedViewSubIdStatic = 0;
	}

	// Token: 0x060003B9 RID: 953 RVA: 0x0000ED1C File Offset: 0x0000D11C
	private void GameEnteredOnGameServer(OperationResponse operationResponse)
	{
		if (operationResponse.ReturnCode != 0)
		{
			byte operationCode = operationResponse.OperationCode;
			if (operationCode != 227)
			{
				if (operationCode != 226)
				{
					if (operationCode == 225)
					{
						if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
						{
							Debug.Log("Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
							if (operationResponse.ReturnCode == 32758)
							{
								Debug.Log("Most likely the game became empty during the switch to GameServer.");
							}
						}
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
					}
				}
				else
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.Log("Join failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
						if (operationResponse.ReturnCode == 32758)
						{
							Debug.Log("Most likely the game became empty during the switch to GameServer.");
						}
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
				}
			}
			else
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.Log("Create failed on GameServer. Changing back to MasterServer. Msg: " + operationResponse.DebugMessage);
				}
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
			}
			this.DisconnectToReconnect();
			return;
		}
		this.CurrentRoom = new Room(this.enterRoomParamsCache.RoomName, null)
		{
			IsLocalClientInside = true
		};
		this.State = ClientState.Joined;
		if (operationResponse.Parameters.ContainsKey(252))
		{
			int[] array = (int[])operationResponse.Parameters[252];
			this.UpdatedActorList(array);
		}
		int num = (int)operationResponse[254];
		this.ChangeLocalID(num);
		ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
		ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
		this.ReadoutProperties(hashtable2, hashtable, 0);
		if (!this.CurrentRoom.serverSideMasterClient)
		{
			this.CheckMasterClient(-1);
		}
		if (this.mPlayernameHasToBeUpdated)
		{
			this.SendPlayerName();
		}
		byte operationCode2 = operationResponse.OperationCode;
		if (operationCode2 != 227)
		{
			if (operationCode2 != 226 && operationCode2 != 225)
			{
			}
		}
		else
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom, new object[0]);
		}
	}

	// Token: 0x060003BA RID: 954 RVA: 0x0000EF85 File Offset: 0x0000D385
	private void AddNewPlayer(int ID, PhotonPlayer player)
	{
		if (!this.mActors.ContainsKey(ID))
		{
			this.mActors[ID] = player;
			this.RebuildPlayerListCopies();
		}
		else
		{
			Debug.LogError("Adding player twice: " + ID);
		}
	}

	// Token: 0x060003BB RID: 955 RVA: 0x0000EFC5 File Offset: 0x0000D3C5
	private void RemovePlayer(int ID, PhotonPlayer player)
	{
		this.mActors.Remove(ID);
		if (!player.IsLocal)
		{
			this.RebuildPlayerListCopies();
		}
	}

	// Token: 0x060003BC RID: 956 RVA: 0x0000EFE8 File Offset: 0x0000D3E8
	private void RebuildPlayerListCopies()
	{
		this.mPlayerListCopy = new PhotonPlayer[this.mActors.Count];
		this.mActors.Values.CopyTo(this.mPlayerListCopy, 0);
		List<PhotonPlayer> list = new List<PhotonPlayer>();
		for (int i = 0; i < this.mPlayerListCopy.Length; i++)
		{
			PhotonPlayer photonPlayer = this.mPlayerListCopy[i];
			if (!photonPlayer.IsLocal)
			{
				list.Add(photonPlayer);
			}
		}
		this.mOtherPlayerListCopy = list.ToArray();
	}

	// Token: 0x060003BD RID: 957 RVA: 0x0000F068 File Offset: 0x0000D468
	private void ResetPhotonViewsOnSerialize()
	{
		foreach (PhotonView photonView in this.photonViewList.Values)
		{
			photonView.lastOnSerializeDataSent = null;
		}
	}

	// Token: 0x060003BE RID: 958 RVA: 0x0000F0CC File Offset: 0x0000D4CC
	private void HandleEventLeave(int actorID, EventData evLeave)
	{
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(string.Concat(new object[]
			{
				"HandleEventLeave for player ID: ",
				actorID,
				" evLeave: ",
				evLeave.ToStringFull()
			}));
		}
		PhotonPlayer playerWithId = this.GetPlayerWithId(actorID);
		if (playerWithId == null)
		{
			Debug.LogError(string.Format("Received event Leave for unknown player ID: {0}", actorID));
			return;
		}
		bool isInactive = playerWithId.IsInactive;
		if (evLeave.Parameters.ContainsKey(233))
		{
			playerWithId.IsInactive = (bool)evLeave.Parameters[233];
			if (playerWithId.IsInactive != isInactive)
			{
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerActivityChanged, new object[] { playerWithId });
			}
			if (playerWithId.IsInactive && isInactive)
			{
				Debug.LogWarning(string.Concat(new object[] { "HandleEventLeave for player ID: ", actorID, " isInactive: ", playerWithId.IsInactive, ". Stopping handling if inactive." }));
				return;
			}
		}
		if (evLeave.Parameters.ContainsKey(203))
		{
			int num = (int)evLeave[203];
			if (num != 0)
			{
				this.mMasterClientId = (int)evLeave[203];
				this.UpdateMasterClient();
			}
		}
		else if (!this.CurrentRoom.serverSideMasterClient)
		{
			this.CheckMasterClient(actorID);
		}
		if (playerWithId.IsInactive && !isInactive)
		{
			return;
		}
		if (this.CurrentRoom != null && this.CurrentRoom.AutoCleanUp)
		{
			this.DestroyPlayerObjects(actorID, true);
		}
		this.RemovePlayer(actorID, playerWithId);
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerDisconnected, new object[] { playerWithId });
	}

	// Token: 0x060003BF RID: 959 RVA: 0x0000F294 File Offset: 0x0000D694
	private void CheckMasterClient(int leavingPlayerId)
	{
		bool flag = this.mMasterClientId == leavingPlayerId;
		bool flag2 = leavingPlayerId > 0;
		if (flag2 && !flag)
		{
			return;
		}
		int num;
		if (this.mActors.Count <= 1)
		{
			num = this.LocalPlayer.ID;
		}
		else
		{
			num = int.MaxValue;
			foreach (int num2 in this.mActors.Keys)
			{
				if (num2 < num && num2 != leavingPlayerId)
				{
					num = num2;
				}
			}
		}
		this.mMasterClientId = num;
		if (flag2)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, new object[] { this.GetPlayerWithId(num) });
		}
	}

	// Token: 0x060003C0 RID: 960 RVA: 0x0000F368 File Offset: 0x0000D768
	protected internal void UpdateMasterClient()
	{
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, new object[] { PhotonNetwork.masterClient });
	}

	// Token: 0x060003C1 RID: 961 RVA: 0x0000F380 File Offset: 0x0000D780
	private static int ReturnLowestPlayerId(PhotonPlayer[] players, int playerIdToIgnore)
	{
		if (players == null || players.Length == 0)
		{
			return -1;
		}
		int num = int.MaxValue;
		foreach (PhotonPlayer photonPlayer in players)
		{
			if (photonPlayer.ID != playerIdToIgnore)
			{
				if (photonPlayer.ID < num)
				{
					num = photonPlayer.ID;
				}
			}
		}
		return num;
	}

	// Token: 0x060003C2 RID: 962 RVA: 0x0000F3E0 File Offset: 0x0000D7E0
	protected internal bool SetMasterClient(int playerId, bool sync)
	{
		bool flag = this.mMasterClientId != playerId;
		if (!flag || !this.mActors.ContainsKey(playerId))
		{
			return false;
		}
		if (sync && !this.OpRaiseEvent(208, new ExitGames.Client.Photon.Hashtable { { 1, playerId } }, true, null))
		{
			return false;
		}
		this.hasSwitchedMC = true;
		this.CurrentRoom.MasterClientId = playerId;
		NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnMasterClientSwitched, new object[] { this.GetPlayerWithId(playerId) });
		return true;
	}

	// Token: 0x060003C3 RID: 963 RVA: 0x0000F474 File Offset: 0x0000D874
	public bool SetMasterClient(int nextMasterId)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable { { 248, nextMasterId } };
		ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable { { 248, this.mMasterClientId } };
		return base.OpSetPropertiesOfRoom(hashtable, hashtable2, false);
	}

	// Token: 0x060003C4 RID: 964 RVA: 0x0000F4CC File Offset: 0x0000D8CC
	protected internal PhotonPlayer GetPlayerWithId(int number)
	{
		if (this.mActors == null)
		{
			return null;
		}
		PhotonPlayer photonPlayer = null;
		this.mActors.TryGetValue(number, out photonPlayer);
		return photonPlayer;
	}

	// Token: 0x060003C5 RID: 965 RVA: 0x0000F4F8 File Offset: 0x0000D8F8
	private void SendPlayerName()
	{
		if (this.State == ClientState.Joining)
		{
			this.mPlayernameHasToBeUpdated = true;
			return;
		}
		if (this.LocalPlayer != null)
		{
			this.LocalPlayer.NickName = this.PlayerName;
			ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
			hashtable[byte.MaxValue] = this.PlayerName;
			if (this.LocalPlayer.ID > 0)
			{
				base.OpSetPropertiesOfActor(this.LocalPlayer.ID, hashtable, null, false);
				this.mPlayernameHasToBeUpdated = false;
			}
		}
	}

	// Token: 0x060003C6 RID: 966 RVA: 0x0000F580 File Offset: 0x0000D980
	private ExitGames.Client.Photon.Hashtable GetLocalActorProperties()
	{
		if (PhotonNetwork.player != null)
		{
			return PhotonNetwork.player.AllProperties;
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[byte.MaxValue] = this.PlayerName;
		return hashtable;
	}

	// Token: 0x060003C7 RID: 967 RVA: 0x0000F5C0 File Offset: 0x0000D9C0
	public void DebugReturn(DebugLevel level, string message)
	{
		if (level == DebugLevel.ERROR)
		{
			Debug.LogError(message);
		}
		else if (level == DebugLevel.WARNING)
		{
			Debug.LogWarning(message);
		}
		else if (level == DebugLevel.INFO && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(message);
		}
		else if (level == DebugLevel.ALL && PhotonNetwork.logLevel == PhotonLogLevel.Full)
		{
			Debug.Log(message);
		}
	}

	// Token: 0x060003C8 RID: 968 RVA: 0x0000F628 File Offset: 0x0000DA28
	public void OnOperationResponse(OperationResponse operationResponse)
	{
		if (PhotonNetwork.networkingPeer.State == ClientState.Disconnecting)
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log("OperationResponse ignored while disconnecting. Code: " + operationResponse.OperationCode);
			}
			return;
		}
		if (operationResponse.ReturnCode == 0)
		{
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log(operationResponse.ToString());
			}
		}
		else if (operationResponse.ReturnCode == -3)
		{
			Debug.LogError("Operation " + operationResponse.OperationCode + " could not be executed (yet). Wait for state JoinedLobby or ConnectedToMaster and their callbacks before calling operations. WebRPCs need a server-side configuration. Enum OperationCode helps identify the operation.");
		}
		else if (operationResponse.ReturnCode == 32752)
		{
			Debug.LogError(string.Concat(new object[] { "Operation ", operationResponse.OperationCode, " failed in a server-side plugin. Check the configuration in the Dashboard. Message from server-plugin: ", operationResponse.DebugMessage }));
		}
		else if (operationResponse.ReturnCode == 32760)
		{
			Debug.LogWarning("Operation failed: " + operationResponse.ToStringFull());
		}
		else
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Operation failed: ",
				operationResponse.ToStringFull(),
				" Server: ",
				this.Server
			}));
		}
		if (operationResponse.Parameters.ContainsKey(221))
		{
			if (this.AuthValues == null)
			{
				this.AuthValues = new AuthenticationValues();
			}
			this.AuthValues.Token = operationResponse[221] as string;
			this.tokenCache = this.AuthValues.Token;
		}
		byte operationCode = operationResponse.OperationCode;
		switch (operationCode)
		{
		case 217:
			if (operationResponse.ReturnCode != 0)
			{
				this.DebugReturn(DebugLevel.ERROR, "GetGameList failed: " + operationResponse.ToStringFull());
			}
			else
			{
				this.mGameList = new Dictionary<string, RoomInfo>();
				ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)operationResponse[222];
				foreach (object obj in hashtable.Keys)
				{
					string text = (string)obj;
					this.mGameList[text] = new RoomInfo(text, (ExitGames.Client.Photon.Hashtable)hashtable[obj]);
				}
				this.mGameListCopy = new RoomInfo[this.mGameList.Count];
				this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate, new object[0]);
			}
			break;
		default:
			switch (operationCode)
			{
			case 251:
			{
				ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)operationResponse[249];
				ExitGames.Client.Photon.Hashtable hashtable3 = (ExitGames.Client.Photon.Hashtable)operationResponse[248];
				this.ReadoutProperties(hashtable3, hashtable2, 0);
				break;
			}
			case 252:
				break;
			case 253:
				break;
			case 254:
				this.DisconnectToReconnect();
				break;
			default:
				Debug.LogWarning(string.Format("OperationResponse unhandled: {0}", operationResponse.ToString()));
				break;
			}
			break;
		case 219:
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnWebRpcResponse, new object[] { operationResponse });
			break;
		case 220:
			if (operationResponse.ReturnCode == 32767)
			{
				Debug.LogError(string.Format("The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account.", new object[0]));
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { DisconnectCause.InvalidAuthentication });
				this.State = ClientState.Disconnecting;
				this.Disconnect();
			}
			else if (operationResponse.ReturnCode != 0)
			{
				Debug.LogError(string.Concat(new object[] { "GetRegions failed. Can't provide regions list. Error: ", operationResponse.ReturnCode, ": ", operationResponse.DebugMessage }));
			}
			else
			{
				string[] array = operationResponse[210] as string[];
				string[] array2 = operationResponse[230] as string[];
				if (array == null || array2 == null || array.Length != array2.Length)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"The region arrays from Name Server are not ok. Must be non-null and same length. ",
						array == null,
						" ",
						array2 == null,
						"\n",
						operationResponse.ToStringFull()
					}));
				}
				else
				{
					this.AvailableRegions = new List<Region>(array.Length);
					for (int i = 0; i < array.Length; i++)
					{
						string text2 = array[i];
						if (!string.IsNullOrEmpty(text2))
						{
							text2 = text2.ToLower();
							CloudRegionCode cloudRegionCode = Region.Parse(text2);
							bool flag = true;
							if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion && PhotonNetwork.PhotonServerSettings.EnabledRegions != (CloudRegionFlag)0)
							{
								CloudRegionFlag cloudRegionFlag = Region.ParseFlag(cloudRegionCode);
								flag = (PhotonNetwork.PhotonServerSettings.EnabledRegions & cloudRegionFlag) != (CloudRegionFlag)0;
								if (!flag && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
								{
									Debug.Log("Skipping region because it's not in PhotonServerSettings.EnabledRegions: " + cloudRegionCode);
								}
							}
							if (flag)
							{
								this.AvailableRegions.Add(new Region(cloudRegionCode, text2, array2[i]));
							}
						}
					}
					if (PhotonNetwork.PhotonServerSettings.HostType == ServerSettings.HostingOption.BestRegion)
					{
						PhotonHandler.PingAvailableRegionsAndConnectToBest();
					}
				}
			}
			break;
		case 222:
		{
			bool[] array3 = operationResponse[1] as bool[];
			string[] array4 = operationResponse[2] as string[];
			if (array3 != null && array4 != null && this.friendListRequested != null && array3.Length == this.friendListRequested.Length)
			{
				List<FriendInfo> list = new List<FriendInfo>(this.friendListRequested.Length);
				for (int j = 0; j < this.friendListRequested.Length; j++)
				{
					list.Insert(j, new FriendInfo
					{
						Name = this.friendListRequested[j],
						Room = array4[j],
						IsOnline = array3[j]
					});
				}
				PhotonNetwork.Friends = list;
			}
			else
			{
				Debug.LogError("FindFriends failed to apply the result, as a required value wasn't provided or the friend list length differed from result.");
			}
			this.friendListRequested = null;
			this.isFetchingFriendList = false;
			this.friendListTimestamp = Environment.TickCount;
			if (this.friendListTimestamp == 0)
			{
				this.friendListTimestamp = 1;
			}
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnUpdatedFriendList, new object[0]);
			break;
		}
		case 225:
			if (operationResponse.ReturnCode != 0)
			{
				if (operationResponse.ReturnCode == 32760)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
					{
						Debug.Log("JoinRandom failed: No open game. Calling: OnPhotonRandomJoinFailed() and staying on master server.");
					}
				}
				else if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.LogWarning(string.Format("JoinRandom failed: {0}.", operationResponse.ToStringFull()));
				}
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonRandomJoinFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
			}
			else
			{
				string text3 = (string)operationResponse[byte.MaxValue];
				this.enterRoomParamsCache.RoomName = text3;
				this.GameServerAddress = (string)operationResponse[230];
				this.DisconnectToReconnect();
			}
			break;
		case 226:
			if (this.Server != ServerConnection.GameServer)
			{
				if (operationResponse.ReturnCode != 0)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.Log(string.Format("JoinRoom failed (room maybe closed by now). Client stays on masterserver: {0}. State: {1}", operationResponse.ToStringFull(), this.State));
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonJoinRoomFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
				}
				else
				{
					this.GameServerAddress = (string)operationResponse[230];
					this.DisconnectToReconnect();
				}
			}
			else
			{
				this.GameEnteredOnGameServer(operationResponse);
			}
			break;
		case 227:
			if (this.Server == ServerConnection.GameServer)
			{
				this.GameEnteredOnGameServer(operationResponse);
			}
			else if (operationResponse.ReturnCode != 0)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.LogWarning(string.Format("CreateRoom failed, client stays on masterserver: {0}.", operationResponse.ToStringFull()));
				}
				this.State = ((!this.insideLobby) ? ClientState.ConnectedToMaster : ClientState.JoinedLobby);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCreateRoomFailed, new object[] { operationResponse.ReturnCode, operationResponse.DebugMessage });
			}
			else
			{
				string text4 = (string)operationResponse[byte.MaxValue];
				if (!string.IsNullOrEmpty(text4))
				{
					this.enterRoomParamsCache.RoomName = text4;
				}
				this.GameServerAddress = (string)operationResponse[230];
				this.DisconnectToReconnect();
			}
			break;
		case 228:
			this.State = ClientState.Authenticated;
			this.LeftLobbyCleanup();
			break;
		case 229:
			this.State = ClientState.JoinedLobby;
			this.insideLobby = true;
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedLobby, new object[0]);
			break;
		case 230:
		case 231:
			if (operationResponse.ReturnCode != 0)
			{
				if (operationResponse.ReturnCode == -2)
				{
					Debug.LogError(string.Format("If you host Photon yourself, make sure to start the 'Instance LoadBalancing' " + base.ServerAddress, new object[0]));
				}
				else if (operationResponse.ReturnCode == 32767)
				{
					Debug.LogError(string.Format("The appId this client sent is unknown on the server (Cloud). Check settings. If using the Cloud, check account.", new object[0]));
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { DisconnectCause.InvalidAuthentication });
				}
				else if (operationResponse.ReturnCode == 32755)
				{
					Debug.LogError(string.Format("Custom Authentication failed (either due to user-input or configuration or AuthParameter string format). Calling: OnCustomAuthenticationFailed()", new object[0]));
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCustomAuthenticationFailed, new object[] { operationResponse.DebugMessage });
				}
				else
				{
					Debug.LogError(string.Format("Authentication failed: '{0}' Code: {1}", operationResponse.DebugMessage, operationResponse.ReturnCode));
				}
				this.State = ClientState.Disconnecting;
				this.Disconnect();
				if (operationResponse.ReturnCode == 32757)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.LogWarning(string.Format("Currently, the limit of users is reached for this title. Try again later. Disconnecting", new object[0]));
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonMaxCccuReached, new object[0]);
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { DisconnectCause.MaxCcuReached });
				}
				else if (operationResponse.ReturnCode == 32756)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.LogError(string.Format("The used master server address is not available with the subscription currently used. Got to Photon Cloud Dashboard or change URL. Disconnecting.", new object[0]));
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { DisconnectCause.InvalidRegion });
				}
				else if (operationResponse.ReturnCode == 32753)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
					{
						Debug.LogError(string.Format("The authentication ticket expired. You need to connect (and authenticate) again. Disconnecting.", new object[0]));
					}
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { DisconnectCause.AuthenticationTicketExpired });
				}
			}
			else
			{
				if (this.Server == ServerConnection.NameServer || this.Server == ServerConnection.MasterServer)
				{
					if (operationResponse.Parameters.ContainsKey(225))
					{
						string text5 = (string)operationResponse.Parameters[225];
						if (!string.IsNullOrEmpty(text5))
						{
							if (this.AuthValues == null)
							{
								this.AuthValues = new AuthenticationValues();
							}
							this.AuthValues.UserId = text5;
							PhotonNetwork.player.UserId = text5;
							if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
							{
								this.DebugReturn(DebugLevel.INFO, string.Format("Received your UserID from server. Updating local value to: {0}", text5));
							}
						}
					}
					if (operationResponse.Parameters.ContainsKey(202))
					{
						this.PlayerName = (string)operationResponse.Parameters[202];
						if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
						{
							this.DebugReturn(DebugLevel.INFO, string.Format("Received your NickName from server. Updating local value to: {0}", this.playername));
						}
					}
					if (operationResponse.Parameters.ContainsKey(192))
					{
						this.SetupEncryption((Dictionary<byte, object>)operationResponse.Parameters[192]);
					}
				}
				if (this.Server == ServerConnection.NameServer)
				{
					this.MasterServerAddress = operationResponse[230] as string;
					this.DisconnectToReconnect();
				}
				else if (this.Server == ServerConnection.MasterServer)
				{
					if (this.AuthMode != AuthModeOption.Auth)
					{
						this.OpSettings(this.requestLobbyStatistics);
					}
					if (PhotonNetwork.autoJoinLobby)
					{
						this.State = ClientState.Authenticated;
						this.OpJoinLobby(this.lobby);
					}
					else
					{
						this.State = ClientState.ConnectedToMaster;
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToMaster, new object[0]);
					}
				}
				else if (this.Server == ServerConnection.GameServer)
				{
					this.State = ClientState.Joining;
					this.enterRoomParamsCache.PlayerProperties = this.GetLocalActorProperties();
					this.enterRoomParamsCache.OnGameServer = true;
					if (this.lastJoinType == JoinType.JoinRoom || this.lastJoinType == JoinType.JoinRandomRoom || this.lastJoinType == JoinType.JoinOrCreateRoom)
					{
						this.OpJoinRoom(this.enterRoomParamsCache);
					}
					else if (this.lastJoinType == JoinType.CreateRoom)
					{
						this.OpCreateGame(this.enterRoomParamsCache);
					}
				}
				if (operationResponse.Parameters.ContainsKey(245))
				{
					Dictionary<string, object> dictionary = (Dictionary<string, object>)operationResponse.Parameters[245];
					if (dictionary != null)
					{
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCustomAuthenticationResponse, new object[] { dictionary });
					}
				}
			}
			break;
		}
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x00010368 File Offset: 0x0000E768
	public void OnStatusChanged(StatusCode statusCode)
	{
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(string.Format("OnStatusChanged: {0} current State: {1}", statusCode.ToString(), this.State));
		}
		switch (statusCode)
		{
		case StatusCode.ExceptionOnReceive:
		case StatusCode.DisconnectByServer:
		case StatusCode.DisconnectByServerUserLimit:
		case StatusCode.DisconnectByServerLogic:
			if (this.IsInitialConnect)
			{
				Debug.LogWarning(string.Concat(new object[] { statusCode, " while connecting to: ", base.ServerAddress, ". Check if the server is available." }));
				this.IsInitialConnect = false;
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { disconnectCause });
			}
			else
			{
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { disconnectCause });
			}
			if (this.AuthValues != null)
			{
				this.AuthValues.Token = null;
			}
			this.Disconnect();
			return;
		case StatusCode.TimeoutDisconnect:
			if (this.IsInitialConnect)
			{
				if (!this._isReconnecting)
				{
					Debug.LogWarning(string.Concat(new object[] { statusCode, " while connecting to: ", base.ServerAddress, ". Check if the server is available." }));
					this.IsInitialConnect = false;
					DisconnectCause disconnectCause = (DisconnectCause)statusCode;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { disconnectCause });
				}
			}
			else
			{
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { disconnectCause });
			}
			if (this.AuthValues != null)
			{
				this.AuthValues.Token = null;
			}
			this.Disconnect();
			return;
		default:
			switch (statusCode)
			{
			case StatusCode.SecurityExceptionOnConnect:
			case StatusCode.ExceptionOnConnect:
			{
				this.IsInitialConnect = false;
				this.State = ClientState.PeerCreated;
				if (this.AuthValues != null)
				{
					this.AuthValues.Token = null;
				}
				DisconnectCause disconnectCause = (DisconnectCause)statusCode;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { disconnectCause });
				return;
			}
			case StatusCode.Connect:
				if (this.State == ClientState.ConnectingToNameServer)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
					{
						Debug.Log("Connected to NameServer.");
					}
					this.Server = ServerConnection.NameServer;
					if (this.AuthValues != null)
					{
						this.AuthValues.Token = null;
					}
				}
				if (this.State == ClientState.ConnectingToGameserver)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
					{
						Debug.Log("Connected to gameserver.");
					}
					this.Server = ServerConnection.GameServer;
					this.State = ClientState.ConnectedToGameserver;
				}
				if (this.State == ClientState.ConnectingToMasterserver)
				{
					if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
					{
						Debug.Log("Connected to masterserver.");
					}
					this.Server = ServerConnection.MasterServer;
					this.State = ClientState.Authenticating;
					if (this.IsInitialConnect)
					{
						this.IsInitialConnect = false;
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectedToPhoton, new object[0]);
					}
				}
				if (base.TransportProtocol != ConnectionProtocol.WebSocketSecure)
				{
					if (this.Server == ServerConnection.NameServer || this.AuthMode == AuthModeOption.Auth)
					{
						base.EstablishEncryption();
					}
					return;
				}
				if (this.DebugOut == DebugLevel.INFO)
				{
					Debug.Log("Skipping EstablishEncryption. Protocol is secure.");
				}
				goto IL_1AC;
			case StatusCode.Disconnect:
				this.didAuthenticate = false;
				this.isFetchingFriendList = false;
				if (this.Server == ServerConnection.GameServer)
				{
					this.LeftRoomCleanup();
				}
				if (this.Server == ServerConnection.MasterServer)
				{
					this.LeftLobbyCleanup();
				}
				if (this.State == ClientState.DisconnectingFromMasterserver)
				{
					if (this.Connect(this.GameServerAddress, ServerConnection.GameServer))
					{
						this.State = ClientState.ConnectingToGameserver;
					}
				}
				else if (this.State == ClientState.DisconnectingFromGameserver || this.State == ClientState.DisconnectingFromNameServer)
				{
					this.SetupProtocol(ServerConnection.MasterServer);
					if (this.Connect(this.MasterServerAddress, ServerConnection.MasterServer))
					{
						this.State = ClientState.ConnectingToMasterserver;
					}
				}
				else
				{
					if (this._isReconnecting)
					{
						return;
					}
					if (this.AuthValues != null)
					{
						this.AuthValues.Token = null;
					}
					this.IsInitialConnect = false;
					this.State = ClientState.PeerCreated;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnDisconnectedFromPhoton, new object[0]);
				}
				return;
			case StatusCode.Exception:
				if (this.IsInitialConnect)
				{
					Debug.LogError("Exception while connecting to: " + base.ServerAddress + ". Check if the server is available.");
					if (base.ServerAddress == null || base.ServerAddress.StartsWith("127.0.0.1"))
					{
						Debug.LogWarning("The server address is 127.0.0.1 (localhost): Make sure the server is running on this machine. Android and iOS emulators have their own localhost.");
						if (base.ServerAddress == this.GameServerAddress)
						{
							Debug.LogWarning("This might be a misconfiguration in the game server config. You need to edit it to a (public) address.");
						}
					}
					this.State = ClientState.PeerCreated;
					DisconnectCause disconnectCause = (DisconnectCause)statusCode;
					this.IsInitialConnect = false;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnFailedToConnectToPhoton, new object[] { disconnectCause });
				}
				else
				{
					this.State = ClientState.PeerCreated;
					DisconnectCause disconnectCause = (DisconnectCause)statusCode;
					NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnConnectionFail, new object[] { disconnectCause });
				}
				this.Disconnect();
				return;
			case StatusCode.SendError:
				return;
			}
			Debug.LogError("Received unknown status code: " + statusCode);
			return;
		case StatusCode.EncryptionEstablished:
			break;
		case StatusCode.EncryptionFailedToEstablish:
		{
			Debug.LogError("Encryption wasn't established: " + statusCode + ". Going to authenticate anyways.");
			AuthenticationValues authenticationValues;
			if ((authenticationValues = this.AuthValues) == null)
			{
				authenticationValues = new AuthenticationValues
				{
					UserId = this.PlayerName
				};
			}
			AuthenticationValues authenticationValues2 = authenticationValues;
			this.OpAuthenticate(this.AppId, this.AppVersion, authenticationValues2, this.CloudRegion.ToString(), this.requestLobbyStatistics);
			return;
		}
		}
		IL_1AC:
		this._isReconnecting = false;
		if (this.Server == ServerConnection.NameServer)
		{
			this.State = ClientState.ConnectedToNameServer;
			if (!this.didAuthenticate && this.CloudRegion == CloudRegionCode.none)
			{
				this.OpGetRegions(this.AppId);
			}
		}
		if (this.Server != ServerConnection.NameServer && (this.AuthMode == AuthModeOption.AuthOnce || this.AuthMode == AuthModeOption.AuthOnceWss))
		{
			Debug.Log(string.Concat(new object[] { "didAuthenticate ", this.didAuthenticate, " AuthMode ", this.AuthMode }));
		}
		else if (!this.didAuthenticate && (!this.IsUsingNameServer || this.CloudRegion != CloudRegionCode.none))
		{
			this.didAuthenticate = this.CallAuthenticate();
			if (this.didAuthenticate)
			{
				this.State = ClientState.Authenticating;
			}
		}
	}

	// Token: 0x060003CA RID: 970 RVA: 0x000109BC File Offset: 0x0000EDBC
	public void OnEvent(EventData photonEvent)
	{
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log(string.Format("OnEvent: {0}", photonEvent.ToString()));
		}
		int num = -1;
		PhotonPlayer photonPlayer = null;
		if (photonEvent.Parameters.ContainsKey(254))
		{
			num = (int)photonEvent[254];
			photonPlayer = this.GetPlayerWithId(num);
		}
		byte code = photonEvent.Code;
		switch (code)
		{
		case 200:
			this.ExecuteRpc(photonEvent[245] as ExitGames.Client.Photon.Hashtable, photonPlayer.ID);
			break;
		case 201:
		case 206:
		{
			ExitGames.Client.Photon.Hashtable hashtable = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int num2 = (int)hashtable[0];
			short num3 = -1;
			byte b = 10;
			int num4 = 1;
			if (hashtable.ContainsKey(1))
			{
				num3 = (short)hashtable[1];
				num4 = 2;
			}
			byte b2 = b;
			while ((int)(b2 - b) < hashtable.Count - num4)
			{
				this.OnSerializeRead(hashtable[b2] as object[], photonPlayer, num2, num3);
				b2 += 1;
			}
			break;
		}
		case 202:
			this.DoInstantiate((ExitGames.Client.Photon.Hashtable)photonEvent[245], photonPlayer, null);
			break;
		case 203:
			if (photonPlayer == null || !photonPlayer.IsMasterClient)
			{
				Debug.LogError("Error: Someone else(" + photonPlayer + ") then the masterserver requests a disconnect!");
			}
			else
			{
				PhotonNetwork.LeaveRoom();
			}
			break;
		case 204:
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int num5 = (int)hashtable2[0];
			PhotonView photonView = null;
			if (this.photonViewList.TryGetValue(num5, out photonView))
			{
				this.RemoveInstantiatedGO(photonView.gameObject, true);
			}
			else if (this.DebugOut >= DebugLevel.ERROR)
			{
				Debug.LogError(string.Concat(new object[] { "Ev Destroy Failed. Could not find PhotonView with instantiationId ", num5, ". Sent by actorNr: ", num }));
			}
			break;
		}
		default:
			switch (code)
			{
			case 223:
				if (this.AuthValues == null)
				{
					this.AuthValues = new AuthenticationValues();
				}
				this.AuthValues.Token = photonEvent[221] as string;
				this.tokenCache = this.AuthValues.Token;
				break;
			case 224:
			{
				string[] array = photonEvent[213] as string[];
				byte[] array2 = photonEvent[212] as byte[];
				int[] array3 = photonEvent[229] as int[];
				int[] array4 = photonEvent[228] as int[];
				this.LobbyStatistics.Clear();
				for (int i = 0; i < array.Length; i++)
				{
					TypedLobbyInfo typedLobbyInfo = new TypedLobbyInfo();
					typedLobbyInfo.Name = array[i];
					typedLobbyInfo.Type = (LobbyType)array2[i];
					typedLobbyInfo.PlayerCount = array3[i];
					typedLobbyInfo.RoomCount = array4[i];
					this.LobbyStatistics.Add(typedLobbyInfo);
				}
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnLobbyStatisticsUpdate, new object[0]);
				break;
			}
			default:
				switch (code)
				{
				case 251:
					if (PhotonNetwork.OnEventCall != null)
					{
						object obj = photonEvent[218];
						PhotonNetwork.OnEventCall(photonEvent.Code, obj, num);
					}
					else
					{
						Debug.LogWarning("Warning: Unhandled Event ErrorInfo (251). Set PhotonNetwork.OnEventCall to the method PUN should call for this event.");
					}
					return;
				case 253:
				{
					int num6 = (int)photonEvent[253];
					ExitGames.Client.Photon.Hashtable hashtable3 = null;
					ExitGames.Client.Photon.Hashtable hashtable4 = null;
					if (num6 == 0)
					{
						hashtable3 = (ExitGames.Client.Photon.Hashtable)photonEvent[251];
					}
					else
					{
						hashtable4 = (ExitGames.Client.Photon.Hashtable)photonEvent[251];
					}
					this.ReadoutProperties(hashtable3, hashtable4, num6);
					return;
				}
				case 254:
					this.HandleEventLeave(num, photonEvent);
					return;
				case 255:
				{
					bool flag = false;
					ExitGames.Client.Photon.Hashtable hashtable5 = (ExitGames.Client.Photon.Hashtable)photonEvent[249];
					if (photonPlayer == null)
					{
						bool flag2 = this.LocalPlayer.ID == num;
						this.AddNewPlayer(num, new PhotonPlayer(flag2, num, hashtable5));
						this.ResetPhotonViewsOnSerialize();
					}
					else
					{
						flag = photonPlayer.IsInactive;
						photonPlayer.InternalCacheProperties(hashtable5);
						photonPlayer.IsInactive = false;
					}
					if (num == this.LocalPlayer.ID)
					{
						int[] array5 = (int[])photonEvent[252];
						this.UpdatedActorList(array5);
						if (this.lastJoinType == JoinType.JoinOrCreateRoom && this.LocalPlayer.ID == 1)
						{
							NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnCreatedRoom, new object[0]);
						}
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnJoinedRoom, new object[0]);
					}
					else
					{
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerConnected, new object[] { this.mActors[num] });
						if (flag)
						{
							NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerActivityChanged, new object[] { this.mActors[num] });
						}
					}
					return;
				}
				}
				if (photonEvent.Code < 200)
				{
					if (PhotonNetwork.OnEventCall != null)
					{
						object obj2 = photonEvent[245];
						PhotonNetwork.OnEventCall(photonEvent.Code, obj2, num);
					}
					else
					{
						Debug.LogWarning("Warning: Unhandled event " + photonEvent + ". Set PhotonNetwork.OnEventCall.");
					}
				}
				break;
			case 226:
				this.PlayersInRoomsCount = (int)photonEvent[229];
				this.PlayersOnMasterCount = (int)photonEvent[227];
				this.RoomsCount = (int)photonEvent[228];
				break;
			case 229:
			{
				ExitGames.Client.Photon.Hashtable hashtable6 = (ExitGames.Client.Photon.Hashtable)photonEvent[222];
				foreach (object obj3 in hashtable6.Keys)
				{
					string text = (string)obj3;
					RoomInfo roomInfo = new RoomInfo(text, (ExitGames.Client.Photon.Hashtable)hashtable6[obj3]);
					if (roomInfo.removedFromList)
					{
						this.mGameList.Remove(text);
					}
					else
					{
						this.mGameList[text] = roomInfo;
					}
				}
				this.mGameListCopy = new RoomInfo[this.mGameList.Count];
				this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate, new object[0]);
				break;
			}
			case 230:
			{
				this.mGameList = new Dictionary<string, RoomInfo>();
				ExitGames.Client.Photon.Hashtable hashtable7 = (ExitGames.Client.Photon.Hashtable)photonEvent[222];
				foreach (object obj4 in hashtable7.Keys)
				{
					string text2 = (string)obj4;
					this.mGameList[text2] = new RoomInfo(text2, (ExitGames.Client.Photon.Hashtable)hashtable7[obj4]);
				}
				this.mGameListCopy = new RoomInfo[this.mGameList.Count];
				this.mGameList.Values.CopyTo(this.mGameListCopy, 0);
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnReceivedRoomListUpdate, new object[0]);
				break;
			}
			}
			break;
		case 207:
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int num7 = (int)hashtable2[0];
			if (num7 >= 0)
			{
				this.DestroyPlayerObjects(num7, true);
			}
			else
			{
				if (this.DebugOut >= DebugLevel.INFO)
				{
					Debug.Log("Ev DestroyAll! By PlayerId: " + num);
				}
				this.DestroyAll(true);
			}
			break;
		}
		case 208:
		{
			ExitGames.Client.Photon.Hashtable hashtable2 = (ExitGames.Client.Photon.Hashtable)photonEvent[245];
			int num8 = (int)hashtable2[1];
			this.SetMasterClient(num8, false);
			break;
		}
		case 209:
		{
			int[] array6 = (int[])photonEvent.Parameters[245];
			int num9 = array6[0];
			int num10 = array6[1];
			PhotonView photonView2 = PhotonView.Find(num9);
			if (photonView2 == null)
			{
				Debug.LogWarning("Can't find PhotonView of incoming OwnershipRequest. ViewId not found: " + num9);
			}
			else
			{
				if (PhotonNetwork.logLevel == PhotonLogLevel.Informational)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Ev OwnershipRequest ",
						photonView2.ownershipTransfer,
						". ActorNr: ",
						num,
						" takes from: ",
						num10,
						". local RequestedView.ownerId: ",
						photonView2.ownerId,
						" isOwnerActive: ",
						photonView2.isOwnerActive,
						". MasterClient: ",
						this.mMasterClientId,
						". This client's player: ",
						PhotonNetwork.player.ToStringFull()
					}));
				}
				switch (photonView2.ownershipTransfer)
				{
				case OwnershipOption.Fixed:
					Debug.LogWarning("Ownership mode == fixed. Ignoring request.");
					break;
				case OwnershipOption.Takeover:
					if (num10 == photonView2.ownerId || (num10 == 0 && photonView2.ownerId == this.mMasterClientId) || photonView2.ownerId == 0)
					{
						photonView2.OwnerShipWasTransfered = true;
						int ownerId = photonView2.ownerId;
						PhotonPlayer playerWithId = this.GetPlayerWithId(ownerId);
						photonView2.ownerId = num;
						if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
						{
							Debug.LogWarning(photonView2 + " ownership transfered to: " + num);
						}
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnOwnershipTransfered, new object[] { photonView2, photonPlayer, playerWithId });
					}
					break;
				case OwnershipOption.Request:
					if ((num10 == PhotonNetwork.player.ID || PhotonNetwork.player.IsMasterClient) && (photonView2.ownerId == PhotonNetwork.player.ID || (PhotonNetwork.player.IsMasterClient && !photonView2.isOwnerActive)))
					{
						NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnOwnershipRequest, new object[] { photonView2, photonPlayer });
					}
					break;
				}
			}
			break;
		}
		case 210:
		{
			int[] array7 = (int[])photonEvent.Parameters[245];
			if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
			{
				Debug.Log(string.Concat(new object[]
				{
					"Ev OwnershipTransfer. ViewID ",
					array7[0],
					" to: ",
					array7[1],
					" Time: ",
					Environment.TickCount % 1000
				}));
			}
			int num11 = array7[0];
			int num12 = array7[1];
			PhotonView photonView3 = PhotonView.Find(num11);
			if (photonView3 != null)
			{
				int ownerId2 = photonView3.ownerId;
				photonView3.OwnerShipWasTransfered = true;
				photonView3.ownerId = num12;
				NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnOwnershipTransfered, new object[]
				{
					photonView3,
					PhotonPlayer.Find(num12),
					PhotonPlayer.Find(ownerId2)
				});
			}
			break;
		}
		}
	}

	// Token: 0x060003CB RID: 971 RVA: 0x00011520 File Offset: 0x0000F920
	public void OnMessage(object messages)
	{
	}

	// Token: 0x060003CC RID: 972 RVA: 0x00011524 File Offset: 0x0000F924
	private void SetupEncryption(Dictionary<byte, object> encryptionData)
	{
		if (this.AuthMode == AuthModeOption.Auth && this.DebugOut == DebugLevel.ERROR)
		{
			Debug.LogWarning("SetupEncryption() called but ignored. Not XB1 compiled. EncryptionData: " + encryptionData.ToStringFull());
			return;
		}
		if (this.DebugOut == DebugLevel.INFO)
		{
			Debug.Log("SetupEncryption() got called. " + encryptionData.ToStringFull());
		}
		EncryptionMode encryptionMode = (EncryptionMode)((byte)encryptionData[0]);
		if (encryptionMode != EncryptionMode.PayloadEncryption)
		{
			if (encryptionMode != EncryptionMode.DatagramEncryption)
			{
				throw new ArgumentOutOfRangeException();
			}
			byte[] array = (byte[])encryptionData[1];
			byte[] array2 = (byte[])encryptionData[2];
			base.InitDatagramEncryption(array, array2);
		}
		else
		{
			byte[] array3 = (byte[])encryptionData[1];
			base.InitPayloadEncryption(array3);
		}
	}

	// Token: 0x060003CD RID: 973 RVA: 0x000115E8 File Offset: 0x0000F9E8
	protected internal void UpdatedActorList(int[] actorsInRoom)
	{
		foreach (int num in actorsInRoom)
		{
			if (this.LocalPlayer.ID != num && !this.mActors.ContainsKey(num))
			{
				this.AddNewPlayer(num, new PhotonPlayer(false, num, string.Empty));
			}
		}
	}

	// Token: 0x060003CE RID: 974 RVA: 0x00011644 File Offset: 0x0000FA44
	private void SendVacantViewIds()
	{
		Debug.Log("SendVacantViewIds()");
		List<int> list = new List<int>();
		foreach (PhotonView photonView in this.photonViewList.Values)
		{
			if (!photonView.isOwnerActive)
			{
				list.Add(photonView.viewID);
			}
		}
		Debug.Log("Sending vacant view IDs. Length: " + list.Count);
		this.OpRaiseEvent(211, list.ToArray(), true, null);
	}

	// Token: 0x060003CF RID: 975 RVA: 0x000116F4 File Offset: 0x0000FAF4
	public static void SendMonoMessage(PhotonNetworkingMessage methodString, params object[] parameters)
	{
		HashSet<GameObject> hashSet;
		if (PhotonNetwork.SendMonoMessageTargets != null)
		{
			hashSet = PhotonNetwork.SendMonoMessageTargets;
		}
		else
		{
			hashSet = PhotonNetwork.FindGameObjectsWithComponent(PhotonNetwork.SendMonoMessageTargetType);
		}
		string text = methodString.ToString();
		object obj = ((parameters == null || parameters.Length != 1) ? parameters : parameters[0]);
		foreach (GameObject gameObject in hashSet)
		{
			if (gameObject != null)
			{
				gameObject.SendMessage(text, obj, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// Token: 0x060003D0 RID: 976 RVA: 0x000117A4 File Offset: 0x0000FBA4
	protected internal void ExecuteRpc(ExitGames.Client.Photon.Hashtable rpcData, int senderID = 0)
	{
		if (rpcData == null || !rpcData.ContainsKey(0))
		{
			Debug.LogError("Malformed RPC; this should never occur. Content: " + SupportClass.DictionaryToString(rpcData));
			return;
		}
		int num = (int)rpcData[0];
		int num2 = 0;
		if (rpcData.ContainsKey(1))
		{
			num2 = (int)((short)rpcData[1]);
		}
		string text;
		if (rpcData.ContainsKey(5))
		{
			int num3 = (int)((byte)rpcData[5]);
			if (num3 > PhotonNetwork.PhotonServerSettings.RpcList.Count - 1)
			{
				Debug.LogError("Could not find RPC with index: " + num3 + ". Going to ignore! Check PhotonServerSettings.RpcList");
				return;
			}
			text = PhotonNetwork.PhotonServerSettings.RpcList[num3];
		}
		else
		{
			text = (string)rpcData[3];
		}
		object[] array = null;
		if (rpcData.ContainsKey(4))
		{
			array = (object[])rpcData[4];
		}
		if (array == null)
		{
			array = new object[0];
		}
		PhotonView photonView = this.GetPhotonView(num);
		if (photonView == null)
		{
			int num4 = num / PhotonNetwork.MAX_VIEW_IDS;
			bool flag = num4 == this.LocalPlayer.ID;
			bool flag2 = num4 == senderID;
			if (flag)
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"Received RPC \"",
					text,
					"\" for viewID ",
					num,
					" but this PhotonView does not exist! View was/is ours.",
					(!flag2) ? " Remote called." : " Owner called.",
					" By: ",
					senderID
				}));
			}
			else
			{
				Debug.LogWarning(string.Concat(new object[]
				{
					"Received RPC \"",
					text,
					"\" for viewID ",
					num,
					" but this PhotonView does not exist! Was remote PV.",
					(!flag2) ? " Remote called." : " Owner called.",
					" By: ",
					senderID,
					" Maybe GO was destroyed but RPC not cleaned up."
				}));
			}
			return;
		}
		if (photonView.prefix != num2)
		{
			Debug.LogError(string.Concat(new object[] { "Received RPC \"", text, "\" on viewID ", num, " with a prefix of ", num2, ", our prefix is ", photonView.prefix, ". The RPC has been ignored." }));
			return;
		}
		if (string.IsNullOrEmpty(text))
		{
			Debug.LogError("Malformed RPC; this should never occur. Content: " + SupportClass.DictionaryToString(rpcData));
			return;
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log("Received RPC: " + text);
		}
		if (photonView.group != 0 && !this.allowedReceivingGroups.Contains(photonView.group))
		{
			return;
		}
		Type[] array2 = new Type[0];
		if (array.Length > 0)
		{
			array2 = new Type[array.Length];
			int num5 = 0;
			foreach (object obj in array)
			{
				if (obj == null)
				{
					array2[num5] = null;
				}
				else
				{
					array2[num5] = obj.GetType();
				}
				num5++;
			}
		}
		int num6 = 0;
		int num7 = 0;
		if (!PhotonNetwork.UseRpcMonoBehaviourCache || photonView.RpcMonoBehaviours == null || photonView.RpcMonoBehaviours.Length == 0)
		{
			photonView.RefreshRpcMonoBehaviourCache();
		}
		for (int j = 0; j < photonView.RpcMonoBehaviours.Length; j++)
		{
			MonoBehaviour monoBehaviour = photonView.RpcMonoBehaviours[j];
			if (monoBehaviour == null)
			{
				Debug.LogError("ERROR You have missing MonoBehaviours on your gameobjects!");
			}
			else
			{
				Type type = monoBehaviour.GetType();
				List<MethodInfo> list = null;
				if (!this.monoRPCMethodsCache.TryGetValue(type, out list))
				{
					List<MethodInfo> methods = SupportClass.GetMethods(type, typeof(PunRPC));
					this.monoRPCMethodsCache[type] = methods;
					list = methods;
				}
				if (list != null)
				{
					for (int k = 0; k < list.Count; k++)
					{
						MethodInfo methodInfo = list[k];
						if (methodInfo.Name.Equals(text))
						{
							num7++;
							ParameterInfo[] cachedParemeters = methodInfo.GetCachedParemeters();
							if (cachedParemeters.Length == array2.Length)
							{
								if (this.CheckTypeMatch(cachedParemeters, array2))
								{
									num6++;
									object obj2 = methodInfo.Invoke(monoBehaviour, array);
									if (PhotonNetwork.StartRpcsAsCoroutine && methodInfo.ReturnType == typeof(IEnumerator))
									{
										monoBehaviour.StartCoroutine((IEnumerator)obj2);
									}
								}
							}
							else if (cachedParemeters.Length - 1 == array2.Length)
							{
								if (this.CheckTypeMatch(cachedParemeters, array2) && cachedParemeters[cachedParemeters.Length - 1].ParameterType == typeof(PhotonMessageInfo))
								{
									num6++;
									int num8 = (int)rpcData[2];
									object[] array3 = new object[array.Length + 1];
									array.CopyTo(array3, 0);
									array3[array3.Length - 1] = new PhotonMessageInfo(this.GetPlayerWithId(senderID), num8, photonView);
									object obj3 = methodInfo.Invoke(monoBehaviour, array3);
									if (PhotonNetwork.StartRpcsAsCoroutine && methodInfo.ReturnType == typeof(IEnumerator))
									{
										monoBehaviour.StartCoroutine((IEnumerator)obj3);
									}
								}
							}
							else if (cachedParemeters.Length == 1 && cachedParemeters[0].ParameterType.IsArray)
							{
								num6++;
								object obj4 = methodInfo.Invoke(monoBehaviour, new object[] { array });
								if (PhotonNetwork.StartRpcsAsCoroutine && methodInfo.ReturnType == typeof(IEnumerator))
								{
									monoBehaviour.StartCoroutine((IEnumerator)obj4);
								}
							}
						}
					}
				}
			}
		}
		if (num6 != 1)
		{
			string text2 = string.Empty;
			foreach (Type type2 in array2)
			{
				if (text2 != string.Empty)
				{
					text2 += ", ";
				}
				if (type2 == null)
				{
					text2 += "null";
				}
				else
				{
					text2 += type2.Name;
				}
			}
			if (num6 == 0)
			{
				if (num7 == 0)
				{
					Debug.LogError(string.Concat(new object[] { "PhotonView with ID ", num, " has no method \"", text, "\" marked with the [PunRPC](C#) or @PunRPC(JS) property! Args: ", text2 }));
				}
				else
				{
					Debug.LogError(string.Concat(new object[] { "PhotonView with ID ", num, " has no method \"", text, "\" that takes ", array2.Length, " argument(s): ", text2 }));
				}
			}
			else
			{
				Debug.LogError(string.Concat(new object[]
				{
					"PhotonView with ID ", num, " has ", num6, " methods \"", text, "\" that takes ", array2.Length, " argument(s): ", text2,
					". Should be just one?"
				}));
			}
		}
	}

	// Token: 0x060003D1 RID: 977 RVA: 0x00011F30 File Offset: 0x00010330
	private bool CheckTypeMatch(ParameterInfo[] methodParameters, Type[] callParameterTypes)
	{
		if (methodParameters.Length < callParameterTypes.Length)
		{
			return false;
		}
		for (int i = 0; i < callParameterTypes.Length; i++)
		{
			Type parameterType = methodParameters[i].ParameterType;
			if (callParameterTypes[i] != null && !parameterType.IsAssignableFrom(callParameterTypes[i]) && (!parameterType.IsEnum || !Enum.GetUnderlyingType(parameterType).IsAssignableFrom(callParameterTypes[i])))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060003D2 RID: 978 RVA: 0x00011FA0 File Offset: 0x000103A0
	internal ExitGames.Client.Photon.Hashtable SendInstantiate(string prefabName, Vector3 position, Quaternion rotation, byte group, int[] viewIDs, object[] data, bool isGlobalObject)
	{
		int num = viewIDs[0];
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[0] = prefabName;
		if (position != Vector3.zero)
		{
			hashtable[1] = position;
		}
		if (rotation != Quaternion.identity)
		{
			hashtable[2] = rotation;
		}
		if (group != 0)
		{
			hashtable[3] = group;
		}
		if (viewIDs.Length > 1)
		{
			hashtable[4] = viewIDs;
		}
		if (data != null)
		{
			hashtable[5] = data;
		}
		if (this.currentLevelPrefix > 0)
		{
			hashtable[8] = this.currentLevelPrefix;
		}
		hashtable[6] = PhotonNetwork.ServerTimestamp;
		hashtable[7] = num;
		this.OpRaiseEvent(202, hashtable, true, new RaiseEventOptions
		{
			CachingOption = ((!isGlobalObject) ? EventCaching.AddToRoomCache : EventCaching.AddToRoomCacheGlobal)
		});
		return hashtable;
	}

	// Token: 0x060003D3 RID: 979 RVA: 0x000120C8 File Offset: 0x000104C8
	internal GameObject DoInstantiate(ExitGames.Client.Photon.Hashtable evData, PhotonPlayer photonPlayer, GameObject resourceGameObject)
	{
		string text = (string)evData[0];
		int num = (int)evData[6];
		int num2 = (int)evData[7];
		Vector3 vector;
		if (evData.ContainsKey(1))
		{
			vector = (Vector3)evData[1];
		}
		else
		{
			vector = Vector3.zero;
		}
		Quaternion quaternion = Quaternion.identity;
		if (evData.ContainsKey(2))
		{
			quaternion = (Quaternion)evData[2];
		}
		byte b = 0;
		if (evData.ContainsKey(3))
		{
			b = (byte)evData[3];
		}
		short num3 = 0;
		if (evData.ContainsKey(8))
		{
			num3 = (short)evData[8];
		}
		int[] array;
		if (evData.ContainsKey(4))
		{
			array = (int[])evData[4];
		}
		else
		{
			array = new int[] { num2 };
		}
		object[] array2;
		if (evData.ContainsKey(5))
		{
			array2 = (object[])evData[5];
		}
		else
		{
			array2 = null;
		}
		if (b != 0 && !this.allowedReceivingGroups.Contains(b))
		{
			return null;
		}
		if (this.ObjectPool != null)
		{
			GameObject gameObject = this.ObjectPool.Instantiate(text, vector, quaternion);
			PhotonView[] photonViewsInChildren = gameObject.GetPhotonViewsInChildren();
			if (photonViewsInChildren.Length != array.Length)
			{
				throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
			}
			for (int i = 0; i < photonViewsInChildren.Length; i++)
			{
				photonViewsInChildren[i].didAwake = false;
				photonViewsInChildren[i].viewID = 0;
				photonViewsInChildren[i].prefix = (int)num3;
				photonViewsInChildren[i].instantiationId = num2;
				photonViewsInChildren[i].isRuntimeInstantiated = true;
				photonViewsInChildren[i].instantiationDataField = array2;
				photonViewsInChildren[i].didAwake = true;
				photonViewsInChildren[i].viewID = array[i];
			}
			gameObject.SendMessage(NetworkingPeer.OnPhotonInstantiateString, new PhotonMessageInfo(photonPlayer, num, null), SendMessageOptions.DontRequireReceiver);
			return gameObject;
		}
		else
		{
			if (resourceGameObject == null)
			{
				if (!NetworkingPeer.UsePrefabCache || !NetworkingPeer.PrefabCache.TryGetValue(text, out resourceGameObject))
				{
					resourceGameObject = (GameObject)Resources.Load(text, typeof(GameObject));
					if (NetworkingPeer.UsePrefabCache)
					{
						NetworkingPeer.PrefabCache.Add(text, resourceGameObject);
					}
				}
				if (resourceGameObject == null)
				{
					Debug.LogError("PhotonNetwork error: Could not Instantiate the prefab [" + text + "]. Please verify you have this gameobject in a Resources folder.");
					return null;
				}
			}
			PhotonView[] photonViewsInChildren2 = resourceGameObject.GetPhotonViewsInChildren();
			if (photonViewsInChildren2.Length != array.Length)
			{
				throw new Exception("Error in Instantiation! The resource's PhotonView count is not the same as in incoming data.");
			}
			for (int j = 0; j < array.Length; j++)
			{
				photonViewsInChildren2[j].viewID = array[j];
				photonViewsInChildren2[j].prefix = (int)num3;
				photonViewsInChildren2[j].instantiationId = num2;
				photonViewsInChildren2[j].isRuntimeInstantiated = true;
			}
			this.StoreInstantiationData(num2, array2);
			GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(resourceGameObject, vector, quaternion);
			for (int k = 0; k < array.Length; k++)
			{
				photonViewsInChildren2[k].viewID = 0;
				photonViewsInChildren2[k].prefix = -1;
				photonViewsInChildren2[k].prefixBackup = -1;
				photonViewsInChildren2[k].instantiationId = -1;
				photonViewsInChildren2[k].isRuntimeInstantiated = false;
			}
			this.RemoveInstantiationData(num2);
			gameObject2.SendMessage(NetworkingPeer.OnPhotonInstantiateString, new PhotonMessageInfo(photonPlayer, num, null), SendMessageOptions.DontRequireReceiver);
			return gameObject2;
		}
	}

	// Token: 0x060003D4 RID: 980 RVA: 0x00012468 File Offset: 0x00010868
	private void StoreInstantiationData(int instantiationId, object[] instantiationData)
	{
		this.tempInstantiationData[instantiationId] = instantiationData;
	}

	// Token: 0x060003D5 RID: 981 RVA: 0x00012478 File Offset: 0x00010878
	public object[] FetchInstantiationData(int instantiationId)
	{
		object[] array = null;
		if (instantiationId == 0)
		{
			return null;
		}
		this.tempInstantiationData.TryGetValue(instantiationId, out array);
		return array;
	}

	// Token: 0x060003D6 RID: 982 RVA: 0x0001249F File Offset: 0x0001089F
	private void RemoveInstantiationData(int instantiationId)
	{
		this.tempInstantiationData.Remove(instantiationId);
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x000124B0 File Offset: 0x000108B0
	public void DestroyPlayerObjects(int playerId, bool localOnly)
	{
		if (playerId <= 0)
		{
			Debug.LogError("Failed to Destroy objects of playerId: " + playerId);
			return;
		}
		if (!localOnly)
		{
			this.OpRemoveFromServerInstantiationsOfPlayer(playerId);
			this.OpCleanRpcBuffer(playerId);
			this.SendDestroyOfPlayer(playerId);
		}
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		foreach (PhotonView photonView in this.photonViewList.Values)
		{
			if (photonView != null && photonView.CreatorActorNr == playerId)
			{
				hashSet.Add(photonView.gameObject);
			}
		}
		foreach (GameObject gameObject in hashSet)
		{
			this.RemoveInstantiatedGO(gameObject, true);
		}
		foreach (PhotonView photonView2 in this.photonViewList.Values)
		{
			if (photonView2.ownerId == playerId)
			{
				photonView2.ownerId = photonView2.CreatorActorNr;
			}
		}
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x0001261C File Offset: 0x00010A1C
	public void DestroyAll(bool localOnly)
	{
		if (!localOnly)
		{
			this.OpRemoveCompleteCache();
			this.SendDestroyOfAll();
		}
		this.LocalCleanupAnythingInstantiated(true);
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x00012638 File Offset: 0x00010A38
	protected internal void RemoveInstantiatedGO(GameObject go, bool localOnly)
	{
		if (go == null)
		{
			Debug.LogError("Failed to 'network-remove' GameObject because it's null.");
			return;
		}
		PhotonView[] componentsInChildren = go.GetComponentsInChildren<PhotonView>(true);
		if (componentsInChildren == null || componentsInChildren.Length <= 0)
		{
			Debug.LogError("Failed to 'network-remove' GameObject because has no PhotonView components: " + go);
			return;
		}
		PhotonView photonView = componentsInChildren[0];
		int creatorActorNr = photonView.CreatorActorNr;
		int instantiationId = photonView.instantiationId;
		if (!localOnly)
		{
			if (!photonView.isMine)
			{
				Debug.LogError("Failed to 'network-remove' GameObject. Client is neither owner nor masterClient taking over for owner who left: " + photonView);
				return;
			}
			if (instantiationId < 1)
			{
				Debug.LogError("Failed to 'network-remove' GameObject because it is missing a valid InstantiationId on view: " + photonView + ". Not Destroying GameObject or PhotonViews!");
				return;
			}
		}
		if (!localOnly)
		{
			this.ServerCleanInstantiateAndDestroy(instantiationId, creatorActorNr, photonView.isRuntimeInstantiated);
		}
		for (int i = componentsInChildren.Length - 1; i >= 0; i--)
		{
			PhotonView photonView2 = componentsInChildren[i];
			if (!(photonView2 == null))
			{
				if (photonView2.instantiationId >= 1)
				{
					this.LocalCleanPhotonView(photonView2);
				}
				if (!localOnly)
				{
					this.OpCleanRpcBuffer(photonView2);
				}
			}
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log("Network destroy Instantiated GO: " + go.name);
		}
		if (this.ObjectPool != null)
		{
			PhotonView[] photonViewsInChildren = go.GetPhotonViewsInChildren();
			for (int j = 0; j < photonViewsInChildren.Length; j++)
			{
				photonViewsInChildren[j].viewID = 0;
			}
			this.ObjectPool.Destroy(go);
		}
		else
		{
			global::UnityEngine.Object.Destroy(go);
		}
	}

	// Token: 0x060003DA RID: 986 RVA: 0x000127B0 File Offset: 0x00010BB0
	private void ServerCleanInstantiateAndDestroy(int instantiateId, int creatorId, bool isRuntimeInstantiated)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[7] = instantiateId;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[] { creatorId }
		};
		this.OpRaiseEvent(202, hashtable, true, raiseEventOptions);
		ExitGames.Client.Photon.Hashtable hashtable2 = new ExitGames.Client.Photon.Hashtable();
		hashtable2[0] = instantiateId;
		raiseEventOptions = null;
		if (!isRuntimeInstantiated)
		{
			raiseEventOptions = new RaiseEventOptions();
			raiseEventOptions.CachingOption = EventCaching.AddToRoomCacheGlobal;
			Debug.Log("Destroying GO as global. ID: " + instantiateId);
		}
		this.OpRaiseEvent(204, hashtable2, true, raiseEventOptions);
	}

	// Token: 0x060003DB RID: 987 RVA: 0x00012854 File Offset: 0x00010C54
	private void SendDestroyOfPlayer(int actorNr)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[0] = actorNr;
		this.OpRaiseEvent(207, hashtable, true, null);
	}

	// Token: 0x060003DC RID: 988 RVA: 0x00012888 File Offset: 0x00010C88
	private void SendDestroyOfAll()
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[0] = -1;
		this.OpRaiseEvent(207, hashtable, true, null);
	}

	// Token: 0x060003DD RID: 989 RVA: 0x000128BC File Offset: 0x00010CBC
	private void OpRemoveFromServerInstantiationsOfPlayer(int actorNr)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[] { actorNr }
		};
		this.OpRaiseEvent(202, null, true, raiseEventOptions);
	}

	// Token: 0x060003DE RID: 990 RVA: 0x000128F8 File Offset: 0x00010CF8
	protected internal void RequestOwnership(int viewID, int fromOwner)
	{
		Debug.Log(string.Concat(new object[]
		{
			"RequestOwnership(): ",
			viewID,
			" from: ",
			fromOwner,
			" Time: ",
			Environment.TickCount % 1000
		}));
		this.OpRaiseEvent(209, new int[] { viewID, fromOwner }, true, new RaiseEventOptions
		{
			Receivers = ReceiverGroup.All
		});
	}

	// Token: 0x060003DF RID: 991 RVA: 0x0001297C File Offset: 0x00010D7C
	protected internal void TransferOwnership(int viewID, int playerID)
	{
		Debug.Log(string.Concat(new object[]
		{
			"TransferOwnership() view ",
			viewID,
			" to: ",
			playerID,
			" Time: ",
			Environment.TickCount % 1000
		}));
		this.OpRaiseEvent(210, new int[] { viewID, playerID }, true, new RaiseEventOptions
		{
			Receivers = ReceiverGroup.All
		});
	}

	// Token: 0x060003E0 RID: 992 RVA: 0x000129FF File Offset: 0x00010DFF
	public bool LocalCleanPhotonView(PhotonView view)
	{
		view.removedFromLocalViewList = true;
		return this.photonViewList.Remove(view.viewID);
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x00012A1C File Offset: 0x00010E1C
	public PhotonView GetPhotonView(int viewID)
	{
		PhotonView photonView = null;
		this.photonViewList.TryGetValue(viewID, out photonView);
		if (photonView == null)
		{
			foreach (PhotonView photonView2 in global::UnityEngine.Object.FindObjectsOfType(typeof(PhotonView)) as PhotonView[])
			{
				if (photonView2.viewID == viewID)
				{
					if (photonView2.didAwake)
					{
						Debug.LogWarning("Had to lookup view that wasn't in photonViewList: " + photonView2);
					}
					return photonView2;
				}
			}
		}
		return photonView;
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x00012AA0 File Offset: 0x00010EA0
	public void RegisterPhotonView(PhotonView netView)
	{
		if (!Application.isPlaying)
		{
			this.photonViewList = new Dictionary<int, PhotonView>();
			return;
		}
		if (netView.viewID == 0)
		{
			Debug.Log("PhotonView register is ignored, because viewID is 0. No id assigned yet to: " + netView);
			return;
		}
		PhotonView photonView = null;
		bool flag = this.photonViewList.TryGetValue(netView.viewID, out photonView);
		if (flag)
		{
			if (!(netView != photonView))
			{
				return;
			}
			if (!Universe.controllerNotOnDuringStartIssueOccurred)
			{
				Universe.controllerNotOnDuringStartIssueOccurred = true;
				Managers.errorManager.ShowCriticalHaltError("Setting up controllers...", false, true, true, true, false);
			}
			else
			{
				Managers.errorManager.ShowCriticalHaltError(string.Concat(new string[]
				{
					"A controller may not have been on during start. Sorry, please ensure both are on and restart. We hope to fix this in the future! \n(View id duplicate: ",
					netView.viewID.ToString(),
					"/",
					netView.ToString(),
					")"
				}), true, true, true, false, false);
			}
			this.RemoveInstantiatedGO(photonView.gameObject, true);
		}
		this.photonViewList.Add(netView.viewID, netView);
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log("Registered PhotonView: " + netView.viewID);
		}
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x00012BCC File Offset: 0x00010FCC
	public void OpCleanRpcBuffer(int actorNumber)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[] { actorNumber }
		};
		this.OpRaiseEvent(200, null, true, raiseEventOptions);
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x00012C08 File Offset: 0x00011008
	public void OpRemoveCompleteCacheOfPlayer(int actorNumber)
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			TargetActors = new int[] { actorNumber }
		};
		this.OpRaiseEvent(0, null, true, raiseEventOptions);
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x00012C40 File Offset: 0x00011040
	public void OpRemoveCompleteCache()
	{
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache,
			Receivers = ReceiverGroup.MasterClient
		};
		this.OpRaiseEvent(0, null, true, raiseEventOptions);
	}

	// Token: 0x060003E6 RID: 998 RVA: 0x00012C70 File Offset: 0x00011070
	private void RemoveCacheOfLeftPlayers()
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[244] = 0;
		dictionary[247] = 7;
		this.OpCustom(253, dictionary, true, 0);
	}

	// Token: 0x060003E7 RID: 999 RVA: 0x00012CB4 File Offset: 0x000110B4
	public void CleanRpcBufferIfMine(PhotonView view)
	{
		if (view.ownerId != this.LocalPlayer.ID && !this.LocalPlayer.IsMasterClient)
		{
			Debug.LogError(string.Concat(new object[] { "Cannot remove cached RPCs on a PhotonView thats not ours! ", view.owner, " scene: ", view.isSceneView }));
			return;
		}
		this.OpCleanRpcBuffer(view);
	}

	// Token: 0x060003E8 RID: 1000 RVA: 0x00012D28 File Offset: 0x00011128
	public void OpCleanRpcBuffer(PhotonView view)
	{
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[0] = view.viewID;
		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			CachingOption = EventCaching.RemoveFromRoomCache
		};
		this.OpRaiseEvent(200, hashtable, true, raiseEventOptions);
	}

	// Token: 0x060003E9 RID: 1001 RVA: 0x00012D70 File Offset: 0x00011170
	public void RemoveRPCsInGroup(int group)
	{
		foreach (PhotonView photonView in this.photonViewList.Values)
		{
			if ((int)photonView.group == group)
			{
				this.CleanRpcBufferIfMine(photonView);
			}
		}
	}

	// Token: 0x060003EA RID: 1002 RVA: 0x00012DE0 File Offset: 0x000111E0
	public void SetLevelPrefix(short prefix)
	{
		this.currentLevelPrefix = prefix;
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x00012DEC File Offset: 0x000111EC
	internal void RPC(PhotonView view, string methodName, PhotonTargets target, PhotonPlayer player, bool encrypt, params object[] parameters)
	{
		if (this.blockSendingGroups.Contains(view.group))
		{
			return;
		}
		if (view.viewID < 1)
		{
			Debug.LogError(string.Concat(new object[]
			{
				"Illegal view ID:",
				view.viewID,
				" method: ",
				methodName,
				" GO:",
				view.gameObject.name
			}));
		}
		if (PhotonNetwork.logLevel >= PhotonLogLevel.Full)
		{
			Debug.Log(string.Concat(new object[] { "Sending RPC \"", methodName, "\" to target: ", target, " or player:", player, "." }));
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		hashtable[0] = view.viewID;
		if (view.prefix > 0)
		{
			hashtable[1] = (short)view.prefix;
		}
		hashtable[2] = PhotonNetwork.ServerTimestamp;
		int num = 0;
		if (this.rpcShortcuts.TryGetValue(methodName, out num))
		{
			hashtable[5] = (byte)num;
		}
		else
		{
			hashtable[3] = methodName;
		}
		if (parameters != null && parameters.Length > 0)
		{
			hashtable[4] = parameters;
		}
		if (player != null)
		{
			if (this.LocalPlayer.ID == player.ID)
			{
				this.ExecuteRpc(hashtable, player.ID);
			}
			else
			{
				RaiseEventOptions raiseEventOptions = new RaiseEventOptions
				{
					TargetActors = new int[] { player.ID },
					Encrypt = encrypt
				};
				this.OpRaiseEvent(200, hashtable, true, raiseEventOptions);
			}
			return;
		}
		if (target == PhotonTargets.All)
		{
			RaiseEventOptions raiseEventOptions2 = new RaiseEventOptions
			{
				InterestGroup = view.group,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions2);
			this.ExecuteRpc(hashtable, this.LocalPlayer.ID);
		}
		else if (target == PhotonTargets.Others)
		{
			RaiseEventOptions raiseEventOptions3 = new RaiseEventOptions
			{
				InterestGroup = view.group,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions3);
		}
		else if (target == PhotonTargets.AllBuffered)
		{
			RaiseEventOptions raiseEventOptions4 = new RaiseEventOptions
			{
				CachingOption = EventCaching.AddToRoomCache,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions4);
			this.ExecuteRpc(hashtable, this.LocalPlayer.ID);
		}
		else if (target == PhotonTargets.OthersBuffered)
		{
			RaiseEventOptions raiseEventOptions5 = new RaiseEventOptions
			{
				CachingOption = EventCaching.AddToRoomCache,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions5);
		}
		else if (target == PhotonTargets.MasterClient)
		{
			if (this.mMasterClientId == this.LocalPlayer.ID)
			{
				this.ExecuteRpc(hashtable, this.LocalPlayer.ID);
			}
			else
			{
				RaiseEventOptions raiseEventOptions6 = new RaiseEventOptions
				{
					Receivers = ReceiverGroup.MasterClient,
					Encrypt = encrypt
				};
				this.OpRaiseEvent(200, hashtable, true, raiseEventOptions6);
			}
		}
		else if (target == PhotonTargets.AllViaServer)
		{
			RaiseEventOptions raiseEventOptions7 = new RaiseEventOptions
			{
				InterestGroup = view.group,
				Receivers = ReceiverGroup.All,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions7);
			if (PhotonNetwork.offlineMode)
			{
				this.ExecuteRpc(hashtable, this.LocalPlayer.ID);
			}
		}
		else if (target == PhotonTargets.AllBufferedViaServer)
		{
			RaiseEventOptions raiseEventOptions8 = new RaiseEventOptions
			{
				InterestGroup = view.group,
				Receivers = ReceiverGroup.All,
				CachingOption = EventCaching.AddToRoomCache,
				Encrypt = encrypt
			};
			this.OpRaiseEvent(200, hashtable, true, raiseEventOptions8);
			if (PhotonNetwork.offlineMode)
			{
				this.ExecuteRpc(hashtable, this.LocalPlayer.ID);
			}
		}
		else
		{
			Debug.LogError("Unsupported target enum: " + target);
		}
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x000131FC File Offset: 0x000115FC
	public void SetInterestGroups(byte[] disableGroups, byte[] enableGroups)
	{
		if (disableGroups != null)
		{
			if (disableGroups.Length == 0)
			{
				this.allowedReceivingGroups.Clear();
			}
			else
			{
				foreach (byte b in disableGroups)
				{
					if (b <= 0)
					{
						Debug.LogError("Error: PhotonNetwork.SetInterestGroups was called with an illegal group number: " + b + ". The group number should be at least 1.");
					}
					else if (this.allowedReceivingGroups.Contains(b))
					{
						this.allowedReceivingGroups.Remove(b);
					}
				}
			}
		}
		if (enableGroups != null)
		{
			if (enableGroups.Length == 0)
			{
				for (byte b2 = 0; b2 < 255; b2 += 1)
				{
					this.allowedReceivingGroups.Add(b2);
				}
				this.allowedReceivingGroups.Add(byte.MaxValue);
			}
			else
			{
				foreach (byte b3 in enableGroups)
				{
					if (b3 <= 0)
					{
						Debug.LogError("Error: PhotonNetwork.SetInterestGroups was called with an illegal group number: " + b3 + ". The group number should be at least 1.");
					}
					else
					{
						this.allowedReceivingGroups.Add(b3);
					}
				}
			}
		}
		this.OpChangeGroups(disableGroups, enableGroups);
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x00013323 File Offset: 0x00011723
	public void SetSendingEnabled(byte group, bool enabled)
	{
		if (!enabled)
		{
			this.blockSendingGroups.Add(group);
		}
		else
		{
			this.blockSendingGroups.Remove(group);
		}
	}

	// Token: 0x060003EE RID: 1006 RVA: 0x0001334C File Offset: 0x0001174C
	public void SetSendingEnabled(byte[] disableGroups, byte[] enableGroups)
	{
		if (disableGroups != null)
		{
			foreach (byte b in disableGroups)
			{
				this.blockSendingGroups.Add(b);
			}
		}
		if (enableGroups != null)
		{
			foreach (byte b2 in enableGroups)
			{
				this.blockSendingGroups.Remove(b2);
			}
		}
	}

	// Token: 0x060003EF RID: 1007 RVA: 0x000133B0 File Offset: 0x000117B0
	public void NewSceneLoaded()
	{
		if (this.loadingLevelAndPausedNetwork)
		{
			this.loadingLevelAndPausedNetwork = false;
			PhotonNetwork.isMessageQueueRunning = true;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, PhotonView> keyValuePair in this.photonViewList)
		{
			PhotonView value = keyValuePair.Value;
			if (value == null)
			{
				list.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			int num = list[i];
			this.photonViewList.Remove(num);
		}
		if (list.Count > 0 && PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
		{
			Debug.Log("New level loaded. Removed " + list.Count + " scene view IDs from last level.");
		}
	}

	// Token: 0x060003F0 RID: 1008 RVA: 0x000134B0 File Offset: 0x000118B0
	public void RunViewUpdate()
	{
		if (!PhotonNetwork.connected || PhotonNetwork.offlineMode || this.mActors == null)
		{
			return;
		}
		if (this.mActors.Count <= 1)
		{
			return;
		}
		int num = 0;
		this.options.Reset();
		List<int> list = null;
		foreach (KeyValuePair<int, PhotonView> keyValuePair in this.photonViewList)
		{
			PhotonView value = keyValuePair.Value;
			if (value == null)
			{
				string text = "PhotonView with ID {0} wasn't properly unregistered! Please report this case to developer@photonengine.com";
				Dictionary<int, PhotonView>.Enumerator enumerator;
				KeyValuePair<int, PhotonView> keyValuePair2 = enumerator.Current;
				Debug.LogError(string.Format(text, keyValuePair2.Key));
				if (list == null)
				{
					list = new List<int>(4);
				}
				List<int> list2 = list;
				KeyValuePair<int, PhotonView> keyValuePair3 = enumerator.Current;
				list2.Add(keyValuePair3.Key);
			}
			else if (value.synchronization != ViewSynchronization.Off && value.isMine && value.gameObject.activeInHierarchy)
			{
				if (!this.blockSendingGroups.Contains(value.group))
				{
					object[] array = this.OnSerializeWrite(value);
					if (array != null)
					{
						if (value.synchronization == ViewSynchronization.ReliableDeltaCompressed || value.mixedModeIsReliable)
						{
							ExitGames.Client.Photon.Hashtable hashtable = null;
							if (!this.dataPerGroupReliable.TryGetValue((int)value.group, out hashtable))
							{
								hashtable = new ExitGames.Client.Photon.Hashtable(NetworkingPeer.ObjectsInOneUpdate);
								this.dataPerGroupReliable[(int)value.group] = hashtable;
							}
							hashtable.Add((byte)(hashtable.Count + 10), array);
							num++;
							if (hashtable.Count >= NetworkingPeer.ObjectsInOneUpdate)
							{
								num -= hashtable.Count;
								this.options.InterestGroup = value.group;
								hashtable[0] = PhotonNetwork.ServerTimestamp;
								if (this.currentLevelPrefix >= 0)
								{
									hashtable[1] = this.currentLevelPrefix;
								}
								this.OpRaiseEvent(206, hashtable, true, this.options);
								hashtable.Clear();
							}
						}
						else
						{
							ExitGames.Client.Photon.Hashtable hashtable2 = null;
							if (!this.dataPerGroupUnreliable.TryGetValue((int)value.group, out hashtable2))
							{
								hashtable2 = new ExitGames.Client.Photon.Hashtable(NetworkingPeer.ObjectsInOneUpdate);
								this.dataPerGroupUnreliable[(int)value.group] = hashtable2;
							}
							hashtable2.Add((byte)(hashtable2.Count + 10), array);
							num++;
							if (hashtable2.Count >= NetworkingPeer.ObjectsInOneUpdate)
							{
								num -= hashtable2.Count;
								this.options.InterestGroup = value.group;
								hashtable2[0] = PhotonNetwork.ServerTimestamp;
								if (this.currentLevelPrefix >= 0)
								{
									hashtable2[1] = this.currentLevelPrefix;
								}
								this.OpRaiseEvent(201, hashtable2, false, this.options);
								hashtable2.Clear();
							}
						}
					}
				}
			}
		}
		if (list != null)
		{
			int i = 0;
			int count = list.Count;
			while (i < count)
			{
				this.photonViewList.Remove(list[i]);
				i++;
			}
		}
		if (num == 0)
		{
			return;
		}
		foreach (int num2 in this.dataPerGroupReliable.Keys)
		{
			this.options.InterestGroup = (byte)num2;
			ExitGames.Client.Photon.Hashtable hashtable3 = this.dataPerGroupReliable[num2];
			if (hashtable3.Count != 0)
			{
				hashtable3[0] = PhotonNetwork.ServerTimestamp;
				if (this.currentLevelPrefix >= 0)
				{
					hashtable3[1] = this.currentLevelPrefix;
				}
				this.OpRaiseEvent(206, hashtable3, true, this.options);
				hashtable3.Clear();
			}
		}
		foreach (int num3 in this.dataPerGroupUnreliable.Keys)
		{
			this.options.InterestGroup = (byte)num3;
			ExitGames.Client.Photon.Hashtable hashtable4 = this.dataPerGroupUnreliable[num3];
			if (hashtable4.Count != 0)
			{
				hashtable4[0] = PhotonNetwork.ServerTimestamp;
				if (this.currentLevelPrefix >= 0)
				{
					hashtable4[1] = this.currentLevelPrefix;
				}
				this.OpRaiseEvent(201, hashtable4, false, this.options);
				hashtable4.Clear();
			}
		}
	}

	// Token: 0x060003F1 RID: 1009 RVA: 0x0001399C File Offset: 0x00011D9C
	private object[] OnSerializeWrite(PhotonView view)
	{
		if (view.synchronization == ViewSynchronization.Off)
		{
			return null;
		}
		PhotonMessageInfo photonMessageInfo = new PhotonMessageInfo(this.LocalPlayer, PhotonNetwork.ServerTimestamp, view);
		this.pStream.ResetWriteStream();
		this.pStream.SendNext(null);
		this.pStream.SendNext(null);
		this.pStream.SendNext(null);
		view.SerializeView(this.pStream, photonMessageInfo);
		if (this.pStream.Count <= 3)
		{
			return null;
		}
		object[] array = this.pStream.ToArray();
		array[0] = view.viewID;
		array[1] = false;
		array[2] = null;
		if (view.synchronization == ViewSynchronization.Unreliable)
		{
			return array;
		}
		if (view.synchronization == ViewSynchronization.UnreliableOnChange)
		{
			if (this.AlmostEquals(array, view.lastOnSerializeDataSent))
			{
				if (view.mixedModeIsReliable)
				{
					return null;
				}
				view.mixedModeIsReliable = true;
				view.lastOnSerializeDataSent = array;
			}
			else
			{
				view.mixedModeIsReliable = false;
				view.lastOnSerializeDataSent = array;
			}
			return array;
		}
		if (view.synchronization == ViewSynchronization.ReliableDeltaCompressed)
		{
			object[] array2 = this.DeltaCompressionWrite(view.lastOnSerializeDataSent, array);
			view.lastOnSerializeDataSent = array;
			return array2;
		}
		return null;
	}

	// Token: 0x060003F2 RID: 1010 RVA: 0x00013AC0 File Offset: 0x00011EC0
	private void OnSerializeRead(object[] data, PhotonPlayer sender, int networkTime, short correctPrefix)
	{
		int num = (int)data[0];
		PhotonView photonView = this.GetPhotonView(num);
		if (photonView == null)
		{
			Debug.LogWarning(string.Concat(new object[] { "Received OnSerialization for view ID ", num, ". We have no such PhotonView! Ignored this if you're leaving a room. State: ", this.State }));
			return;
		}
		if (photonView.prefix > 0 && (int)correctPrefix != photonView.prefix)
		{
			Debug.LogError(string.Concat(new object[] { "Received OnSerialization for view ID ", num, " with prefix ", correctPrefix, ". Our prefix is ", photonView.prefix }));
			return;
		}
		if (photonView.group != 0 && !this.allowedReceivingGroups.Contains(photonView.group))
		{
			return;
		}
		if (photonView.synchronization == ViewSynchronization.ReliableDeltaCompressed)
		{
			object[] array = this.DeltaCompressionRead(photonView.lastOnSerializeDataReceived, data);
			if (array == null)
			{
				if (PhotonNetwork.logLevel >= PhotonLogLevel.Informational)
				{
					Debug.Log(string.Concat(new object[] { "Skipping packet for ", photonView.name, " [", photonView.viewID, "] as we haven't received a full packet for delta compression yet. This is OK if it happens for the first few frames after joining a game." }));
				}
				return;
			}
			photonView.lastOnSerializeDataReceived = array;
			data = array;
		}
		if (sender.ID != photonView.ownerId && (!photonView.OwnerShipWasTransfered || photonView.ownerId == 0) && photonView.currentMasterID == -1)
		{
			photonView.ownerId = sender.ID;
		}
		this.readStream.SetReadStream(data, 3);
		PhotonMessageInfo photonMessageInfo = new PhotonMessageInfo(sender, networkTime, photonView);
		photonView.DeserializeView(this.readStream, photonMessageInfo);
	}

	// Token: 0x060003F3 RID: 1011 RVA: 0x00013C80 File Offset: 0x00012080
	private object[] DeltaCompressionWrite(object[] previousContent, object[] currentContent)
	{
		if (currentContent == null || previousContent == null || previousContent.Length != currentContent.Length)
		{
			return currentContent;
		}
		if (currentContent.Length <= 3)
		{
			return null;
		}
		previousContent[1] = false;
		int num = 0;
		Queue<int> queue = null;
		for (int i = 3; i < currentContent.Length; i++)
		{
			object obj = currentContent[i];
			object obj2 = previousContent[i];
			if (this.AlmostEquals(obj, obj2))
			{
				num++;
				previousContent[i] = null;
			}
			else
			{
				previousContent[i] = obj;
				if (obj == null)
				{
					if (queue == null)
					{
						queue = new Queue<int>(currentContent.Length);
					}
					queue.Enqueue(i);
				}
			}
		}
		if (num > 0)
		{
			if (num == currentContent.Length - 3)
			{
				return null;
			}
			previousContent[1] = true;
			if (queue != null)
			{
				previousContent[2] = queue.ToArray();
			}
		}
		previousContent[0] = currentContent[0];
		return previousContent;
	}

	// Token: 0x060003F4 RID: 1012 RVA: 0x00013D50 File Offset: 0x00012150
	private object[] DeltaCompressionRead(object[] lastOnSerializeDataReceived, object[] incomingData)
	{
		if (!(bool)incomingData[1])
		{
			return incomingData;
		}
		if (lastOnSerializeDataReceived == null)
		{
			return null;
		}
		int[] array = incomingData[2] as int[];
		for (int i = 3; i < incomingData.Length; i++)
		{
			if (array == null || !array.Contains(i))
			{
				if (incomingData[i] == null)
				{
					object obj = lastOnSerializeDataReceived[i];
					incomingData[i] = obj;
				}
			}
		}
		return incomingData;
	}

	// Token: 0x060003F5 RID: 1013 RVA: 0x00013DBC File Offset: 0x000121BC
	private bool AlmostEquals(object[] lastData, object[] currentContent)
	{
		if (lastData == null && currentContent == null)
		{
			return true;
		}
		if (lastData == null || currentContent == null || lastData.Length != currentContent.Length)
		{
			return false;
		}
		for (int i = 0; i < currentContent.Length; i++)
		{
			object obj = currentContent[i];
			object obj2 = lastData[i];
			if (!this.AlmostEquals(obj, obj2))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060003F6 RID: 1014 RVA: 0x00013E1C File Offset: 0x0001221C
	private bool AlmostEquals(object one, object two)
	{
		if (one == null || two == null)
		{
			return one == null && two == null;
		}
		if (!one.Equals(two))
		{
			if (one is Vector3)
			{
				Vector3 vector = (Vector3)one;
				Vector3 vector2 = (Vector3)two;
				if (vector.AlmostEquals(vector2, PhotonNetwork.precisionForVectorSynchronization))
				{
					return true;
				}
			}
			else if (one is Vector2)
			{
				Vector2 vector3 = (Vector2)one;
				Vector2 vector4 = (Vector2)two;
				if (vector3.AlmostEquals(vector4, PhotonNetwork.precisionForVectorSynchronization))
				{
					return true;
				}
			}
			else if (one is Quaternion)
			{
				Quaternion quaternion = (Quaternion)one;
				Quaternion quaternion2 = (Quaternion)two;
				if (quaternion.AlmostEquals(quaternion2, PhotonNetwork.precisionForQuaternionSynchronization))
				{
					return true;
				}
			}
			else if (one is float)
			{
				float num = (float)one;
				float num2 = (float)two;
				if (num.AlmostEquals(num2, PhotonNetwork.precisionForFloatSynchronization))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x00013F1C File Offset: 0x0001231C
	protected internal static bool GetMethod(MonoBehaviour monob, string methodType, out MethodInfo mi)
	{
		mi = null;
		if (monob == null || string.IsNullOrEmpty(methodType))
		{
			return false;
		}
		List<MethodInfo> methods = SupportClass.GetMethods(monob.GetType(), null);
		for (int i = 0; i < methods.Count; i++)
		{
			MethodInfo methodInfo = methods[i];
			if (methodInfo.Name.Equals(methodType))
			{
				mi = methodInfo;
				return true;
			}
		}
		return false;
	}

	// Token: 0x060003F8 RID: 1016 RVA: 0x00013F88 File Offset: 0x00012388
	protected internal void LoadLevelIfSynced()
	{
		if (!PhotonNetwork.automaticallySyncScene || PhotonNetwork.isMasterClient || PhotonNetwork.room == null)
		{
			return;
		}
		if (!PhotonNetwork.room.CustomProperties.ContainsKey("curScn"))
		{
			return;
		}
		object obj = PhotonNetwork.room.CustomProperties["curScn"];
		if (obj is int)
		{
			if (SceneManagerHelper.ActiveSceneBuildIndex != (int)obj)
			{
				PhotonNetwork.LoadLevel((int)obj);
			}
		}
		else if (obj is string && SceneManagerHelper.ActiveSceneName != (string)obj)
		{
			PhotonNetwork.LoadLevel((string)obj);
		}
	}

	// Token: 0x060003F9 RID: 1017 RVA: 0x0001403C File Offset: 0x0001243C
	protected internal void SetLevelInPropsIfSynced(object levelId)
	{
		if (!PhotonNetwork.automaticallySyncScene || !PhotonNetwork.isMasterClient || PhotonNetwork.room == null)
		{
			return;
		}
		if (levelId == null)
		{
			Debug.LogError("Parameter levelId can't be null!");
			return;
		}
		if (PhotonNetwork.room.CustomProperties.ContainsKey("curScn"))
		{
			object obj = PhotonNetwork.room.CustomProperties["curScn"];
			if (obj is int && SceneManagerHelper.ActiveSceneBuildIndex == (int)obj)
			{
				return;
			}
			if (obj is string && SceneManagerHelper.ActiveSceneName != null && SceneManagerHelper.ActiveSceneName.Equals((string)obj))
			{
				return;
			}
		}
		ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
		if (levelId is int)
		{
			hashtable["curScn"] = (int)levelId;
		}
		else if (levelId is string)
		{
			hashtable["curScn"] = (string)levelId;
		}
		else
		{
			Debug.LogError("Parameter levelId must be int or string!");
		}
		PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
		this.SendOutgoingCommands();
	}

	// Token: 0x060003FA RID: 1018 RVA: 0x00014159 File Offset: 0x00012559
	public void SetApp(string appId, string gameVersion)
	{
		this.AppId = appId.Trim();
		if (!string.IsNullOrEmpty(gameVersion))
		{
			PhotonNetwork.gameVersion = gameVersion.Trim();
		}
	}

	// Token: 0x060003FB RID: 1019 RVA: 0x00014180 File Offset: 0x00012580
	public bool WebRpc(string uriPath, object parameters)
	{
		return this.OpCustom(219, new Dictionary<byte, object>
		{
			{ 209, uriPath },
			{ 208, parameters }
		}, true);
	}

	// Token: 0x04000316 RID: 790
	protected internal string AppId;

	// Token: 0x04000318 RID: 792
	private string tokenCache;

	// Token: 0x04000319 RID: 793
	public AuthModeOption AuthMode;

	// Token: 0x0400031A RID: 794
	public EncryptionMode EncryptionMode;

	// Token: 0x0400031C RID: 796
	public const string NameServerHost = "ns.exitgames.com";

	// Token: 0x0400031D RID: 797
	public const string NameServerHttp = "http://ns.exitgamescloud.com:80/photon/n";

	// Token: 0x0400031E RID: 798
	private static readonly Dictionary<ConnectionProtocol, int> ProtocolToNameServerPort = new Dictionary<ConnectionProtocol, int>
	{
		{
			ConnectionProtocol.Udp,
			5058
		},
		{
			ConnectionProtocol.Tcp,
			4533
		},
		{
			ConnectionProtocol.WebSocket,
			9093
		},
		{
			ConnectionProtocol.WebSocketSecure,
			19093
		}
	};

	// Token: 0x04000323 RID: 803
	public bool IsInitialConnect;

	// Token: 0x04000324 RID: 804
	public bool insideLobby;

	// Token: 0x04000326 RID: 806
	protected internal List<TypedLobbyInfo> LobbyStatistics = new List<TypedLobbyInfo>();

	// Token: 0x04000327 RID: 807
	public Dictionary<string, RoomInfo> mGameList = new Dictionary<string, RoomInfo>();

	// Token: 0x04000328 RID: 808
	public RoomInfo[] mGameListCopy = new RoomInfo[0];

	// Token: 0x04000329 RID: 809
	private string playername = string.Empty;

	// Token: 0x0400032A RID: 810
	private bool mPlayernameHasToBeUpdated;

	// Token: 0x0400032B RID: 811
	private Room currentRoom;

	// Token: 0x04000330 RID: 816
	private JoinType lastJoinType;

	// Token: 0x04000331 RID: 817
	protected internal EnterRoomParams enterRoomParamsCache;

	// Token: 0x04000332 RID: 818
	private bool didAuthenticate;

	// Token: 0x04000333 RID: 819
	private string[] friendListRequested;

	// Token: 0x04000334 RID: 820
	private int friendListTimestamp;

	// Token: 0x04000335 RID: 821
	private bool isFetchingFriendList;

	// Token: 0x04000338 RID: 824
	public Dictionary<int, PhotonPlayer> mActors = new Dictionary<int, PhotonPlayer>();

	// Token: 0x04000339 RID: 825
	public PhotonPlayer[] mOtherPlayerListCopy = new PhotonPlayer[0];

	// Token: 0x0400033A RID: 826
	public PhotonPlayer[] mPlayerListCopy = new PhotonPlayer[0];

	// Token: 0x0400033B RID: 827
	public bool hasSwitchedMC;

	// Token: 0x0400033C RID: 828
	private HashSet<byte> allowedReceivingGroups = new HashSet<byte>();

	// Token: 0x0400033D RID: 829
	private HashSet<byte> blockSendingGroups = new HashSet<byte>();

	// Token: 0x0400033E RID: 830
	protected internal Dictionary<int, PhotonView> photonViewList = new Dictionary<int, PhotonView>();

	// Token: 0x0400033F RID: 831
	private readonly PhotonStream readStream = new PhotonStream(false, null);

	// Token: 0x04000340 RID: 832
	private readonly PhotonStream pStream = new PhotonStream(true, null);

	// Token: 0x04000341 RID: 833
	private readonly Dictionary<int, ExitGames.Client.Photon.Hashtable> dataPerGroupReliable = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();

	// Token: 0x04000342 RID: 834
	private readonly Dictionary<int, ExitGames.Client.Photon.Hashtable> dataPerGroupUnreliable = new Dictionary<int, ExitGames.Client.Photon.Hashtable>();

	// Token: 0x04000343 RID: 835
	protected internal short currentLevelPrefix;

	// Token: 0x04000344 RID: 836
	protected internal bool loadingLevelAndPausedNetwork;

	// Token: 0x04000345 RID: 837
	protected internal const string CurrentSceneProperty = "curScn";

	// Token: 0x04000346 RID: 838
	public static bool UsePrefabCache = true;

	// Token: 0x04000347 RID: 839
	internal IPunPrefabPool ObjectPool;

	// Token: 0x04000348 RID: 840
	public static Dictionary<string, GameObject> PrefabCache = new Dictionary<string, GameObject>();

	// Token: 0x04000349 RID: 841
	private Dictionary<Type, List<MethodInfo>> monoRPCMethodsCache = new Dictionary<Type, List<MethodInfo>>();

	// Token: 0x0400034A RID: 842
	private readonly Dictionary<string, int> rpcShortcuts;

	// Token: 0x0400034B RID: 843
	private static readonly string OnPhotonInstantiateString = PhotonNetworkingMessage.OnPhotonInstantiate.ToString();

	// Token: 0x0400034C RID: 844
	private string cachedServerAddress;

	// Token: 0x0400034D RID: 845
	private string cachedApplicationName;

	// Token: 0x0400034E RID: 846
	private ServerConnection cachedProtocolType;

	// Token: 0x0400034F RID: 847
	private bool _isReconnecting;

	// Token: 0x04000350 RID: 848
	private Dictionary<int, object[]> tempInstantiationData = new Dictionary<int, object[]>();

	// Token: 0x04000351 RID: 849
	public static int ObjectsInOneUpdate = 10;

	// Token: 0x04000352 RID: 850
	private RaiseEventOptions options = new RaiseEventOptions();

	// Token: 0x04000353 RID: 851
	public const int SyncViewId = 0;

	// Token: 0x04000354 RID: 852
	public const int SyncCompressed = 1;

	// Token: 0x04000355 RID: 853
	public const int SyncNullValues = 2;

	// Token: 0x04000356 RID: 854
	public const int SyncFirstValue = 3;
}
