using System;
using ExitGames.Client.Photon;
using Photon;

// Token: 0x020001E3 RID: 483
public class BroadcastNetworkManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x00089863 File Offset: 0x00087C63
	// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x0008986B File Offset: 0x00087C6B
	public ManagerStatus status { get; private set; }

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x00089874 File Offset: 0x00087C74
	// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x0008987C File Offset: 0x00087C7C
	public string failMessage { get; private set; }

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06000FEA RID: 4074 RVA: 0x00089885 File Offset: 0x00087C85
	public bool inRoom
	{
		get
		{
			return this.inPhotonDownMode || PhotonNetwork.inRoom;
		}
	}

	// Token: 0x06000FEB RID: 4075 RVA: 0x0008989C File Offset: 0x00087C9C
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		Log.Info("Connecting to Photon", false);
		PhotonNetwork.OnEventCall = (PhotonNetwork.EventCallback)Delegate.Combine(PhotonNetwork.OnEventCall, new PhotonNetwork.EventCallback(this.OnEvent));
		this.OverridePhotonAppId();
		this.ConfigureAuthentication();
		PhotonNetwork.sendRate = 20;
		PhotonNetwork.sendRateOnSerialize = 10;
		this.TryConnect();
		Log.Info("Waiting for Photon Connection", false);
	}

	// Token: 0x06000FEC RID: 4076 RVA: 0x00089906 File Offset: 0x00087D06
	public void TryConnect()
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x00089913 File Offset: 0x00087D13
	public void Disconnect()
	{
		PhotonNetwork.Disconnect();
	}

	// Token: 0x06000FEE RID: 4078 RVA: 0x0008991A File Offset: 0x00087D1A
	public void OverridePhotonAppId()
	{
		if (Managers.serverManager.UseLocalServer)
		{
			PhotonNetwork.PhotonServerSettings.AppID = "90472ac8-2315-4cd0-8df7-6530f25ac196";
		}
		else
		{
			PhotonNetwork.PhotonServerSettings.AppID = "cf74a93d-0768-4459-815c-adfeab8eeed2";
		}
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x00089950 File Offset: 0x00087D50
	public void ConfigureAuthentication()
	{
		if (!Managers.serverManager.UseLocalServer)
		{
			PhotonNetwork.AuthValues = new AuthenticationValues(Managers.personManager.ourPerson.userId);
			PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
			PhotonNetwork.AuthValues.AddAuthParameter("uid", Managers.personManager.ourPerson.userId);
			PhotonNetwork.AuthValues.AddAuthParameter("ast", Managers.serverManager.SteamAuthSessionTicket ?? string.Empty);
		}
	}

	// Token: 0x06000FF0 RID: 4080 RVA: 0x000899D8 File Offset: 0x00087DD8
	public void OnApplicationQuit()
	{
		PhotonNetwork.Disconnect();
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x000899E0 File Offset: 0x00087DE0
	public void Update()
	{
		if (this.status != ManagerStatus.Started)
		{
			return;
		}
		if (!string.IsNullOrEmpty(this.JoiningArea) && PhotonNetwork.connectionStateDetailed == ClientState.ConnectedToMaster)
		{
			Log.Info("Left room - now trying to join room : " + this.JoiningArea, false);
			this.JoinOrCreateAreaRoom(this.JoiningArea);
			this.JoiningArea = null;
		}
	}

	// Token: 0x06000FF2 RID: 4082 RVA: 0x00089A40 File Offset: 0x00087E40
	public void JoinArea(string urlName)
	{
		Log.Info("Trying to join area: " + urlName, false);
		Log.Info("Leaving current room first...", false);
		if (PhotonNetwork.room != null)
		{
			PhotonNetwork.LeaveRoom();
		}
		Managers.personManager.ourPerson.ClearComponentNetworkViewIds();
		this.JoiningArea = urlName;
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x00089A90 File Offset: 0x00087E90
	private void JoinOrCreateAreaRoom(string name)
	{
		Log.Info("Attempting to Join/Create photon room: " + name, false);
		PhotonNetwork.JoinOrCreateRoom(name, new RoomOptions
		{
			maxPlayers = 12,
			isVisible = false,
			publishUserId = true
		}, TypedLobby.Default);
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x00089AD7 File Offset: 0x00087ED7
	public RoomInfo[] GetRoomsList()
	{
		return PhotonNetwork.GetRoomList();
	}

	// Token: 0x06000FF5 RID: 4085 RVA: 0x00089AE0 File Offset: 0x00087EE0
	public int GetNumberOfPeopleInRoom(string roomName)
	{
		int num = 0;
		foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
		{
			if (roomInfo.name == roomName)
			{
				num = roomInfo.playerCount;
				break;
			}
		}
		return num;
	}

	// Token: 0x06000FF6 RID: 4086 RVA: 0x00089B2C File Offset: 0x00087F2C
	public bool RoomHasSpace(string roomName)
	{
		return this.GetNumberOfPeopleInRoom(roomName) < 12;
	}

	// Token: 0x06000FF7 RID: 4087 RVA: 0x00089B46 File Offset: 0x00087F46
	public void PauseMessageQueue()
	{
		PhotonNetwork.isMessageQueueRunning = false;
	}

	// Token: 0x06000FF8 RID: 4088 RVA: 0x00089B4E File Offset: 0x00087F4E
	public void UnpauseMessageQueue()
	{
		PhotonNetwork.isMessageQueueRunning = true;
	}

	// Token: 0x06000FF9 RID: 4089 RVA: 0x00089B56 File Offset: 0x00087F56
	private PhotonPlayer[] GetOtherPlayersInRoomFromPhoton()
	{
		return PhotonNetwork.otherPlayers;
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x00089B5D File Offset: 0x00087F5D
	private void OnConnectedToPhoton()
	{
		Log.Info("PHOTON: Connected.  Waiting for OnConnectedToMaster.", false);
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x00089B6C File Offset: 0x00087F6C
	private void OnConnectedToMaster()
	{
		Log.Info("PHOTON: Connected to master. AppId Is : " + PhotonNetwork.PhotonServerSettings.AppID + " . UserId is : " + PhotonNetwork.AuthValues.UserId, false);
		this.INFO_Current_AppId = PhotonNetwork.PhotonServerSettings.AppID;
		this.INFO_Photon_Auth_UserId = PhotonNetwork.AuthValues.UserId;
		if (!Managers.serverManager.UseLocalServer && PhotonNetwork.AuthValues.UserId != Managers.personManager.ourPerson.userId)
		{
			Managers.errorManager.ShowCriticalHaltError("BUG!! PhotonNetwork.AuthValues.UserId does not match our userId!!!", true, false, true, false, false);
		}
		this.inPhotonDownMode = false;
		if (this.status == ManagerStatus.Initializing)
		{
			this.status = ManagerStatus.Started;
		}
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x00089C22 File Offset: 0x00088022
	public void OnDisconnectedFromPhoton()
	{
		Log.Info("PHOTON: OnDisconnectedFromPhoton...", false);
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x00089C2F File Offset: 0x0008802F
	public void OnCreatedRoom()
	{
		Log.Info("PHOTON: Created Room", false);
	}

	// Token: 0x06000FFE RID: 4094 RVA: 0x00089C3C File Offset: 0x0008803C
	public void OnJoinedLobby()
	{
		Log.Info("PHOTON: Joined Lobby", false);
	}

	// Token: 0x06000FFF RID: 4095 RVA: 0x00089C4C File Offset: 0x0008804C
	public void OnJoinedRoom()
	{
		Log.Info("PHOTON: Joined Room: " + PhotonNetwork.room.name, false);
		this.INFO_Current_Room = PhotonNetwork.room.name;
		this.INFO_Current_PlayerCount = PhotonNetwork.room.playerCount;
		this.INFO_ActorId = PhotonNetwork.player.ID;
		if (Managers.areaManager != null && Managers.personManager != null && Managers.areaManager.didFinishLoadingPlacements)
		{
			Managers.personManager.DoFinalizeLoadedAllPlacements();
		}
	}

	// Token: 0x06001000 RID: 4096 RVA: 0x00089CDC File Offset: 0x000880DC
	public void OnLeftRoom()
	{
		Log.Info("PHOTON: Left Room", false);
		this.INFO_Current_Room = null;
		this.INFO_Current_PlayerCount = 0;
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x00089CF7 File Offset: 0x000880F7
	public void OnPhotonPlayerConnected(PhotonPlayer p)
	{
		Log.Info("PHOTON: A REMOTE PLAYER CONNECTED: " + p.name, false);
		this.INFO_Current_PlayerCount = PhotonNetwork.room.playerCount;
	}

	// Token: 0x06001002 RID: 4098 RVA: 0x00089D1F File Offset: 0x0008811F
	public void OnPhotonPlayerDisconnected(PhotonPlayer p)
	{
		Log.Info("PHOTON: A remote player disconnected: " + p.name, false);
		this.INFO_Current_PlayerCount = PhotonNetwork.room.playerCount;
	}

	// Token: 0x06001003 RID: 4099 RVA: 0x00089D47 File Offset: 0x00088147
	public void OnJoinedInstantiate()
	{
		Log.Info("PHOTON: OnJoinedInstantiate", false);
	}

	// Token: 0x06001004 RID: 4100 RVA: 0x00089D54 File Offset: 0x00088154
	public void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Log.Error("PHOTON: OnFailedToConnectToPhoton : " + cause.ToString());
		this.StartInPhotonDownMode();
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x00089D78 File Offset: 0x00088178
	public void OnConnectionFail(DisconnectCause cause)
	{
		Log.Error("PHOTON: OnConnectionFail : " + cause.ToString());
		Managers.areaManager.HandlePhotonConnectionFailure();
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x00089DA0 File Offset: 0x000881A0
	public void OnCustomAuthenticationFailed(string debugMessage)
	{
		Log.Error("PHOTON: Custom authentication failed");
		this.StartInPhotonDownMode();
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x00089DB4 File Offset: 0x000881B4
	public void OnPhotonCreateRoomFailed(object[] codeAndMsg)
	{
		short num = (short)codeAndMsg[0];
		string text = (string)codeAndMsg[1];
		Log.Error(string.Concat(new object[]
		{
			"PHOTON: OnPhotonCreateRoomFailed : ",
			num.ToString(),
			' ',
			text
		}));
		Managers.areaManager.HandlePhotonJoinRoomFailure();
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x00089E14 File Offset: 0x00088214
	public void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		short num = (short)codeAndMsg[0];
		string text = (string)codeAndMsg[1];
		Log.Error(string.Concat(new object[]
		{
			"PHOTON: OnPhotonJoinRoomFailed : ",
			num.ToString(),
			' ',
			text
		}));
		Managers.areaManager.HandlePhotonJoinRoomFailure();
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x00089E73 File Offset: 0x00088273
	public void StartInPhotonDownMode()
	{
		Log.Info("Starting in photon-down mode (solo mode)", false);
		this.inPhotonDownMode = true;
		if (this.status == ManagerStatus.Initializing)
		{
			this.status = ManagerStatus.Started;
		}
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x00089E9C File Offset: 0x0008829C
	public void UpdatePhotonCustomRoomProperty(string name, string value)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add(name, value);
		if (PhotonNetwork.room != null)
		{
			PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
		}
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x00089ED0 File Offset: 0x000882D0
	public string GetPhotonCustomRoomProperty(string name)
	{
		string text = null;
		if (PhotonNetwork.room != null)
		{
			object obj;
			PhotonNetwork.room.customProperties.TryGetValue(name, out obj);
			if (obj != null)
			{
				text = (string)obj;
			}
		}
		return text;
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x00089F0A File Offset: 0x0008830A
	public void RaiseEvent_ReloadArea()
	{
		this.RaiseEvent_ReloadArea(string.Empty);
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x00089F18 File Offset: 0x00088318
	public void RaiseEvent_ReloadArea(string targetUserId)
	{
		Log.Info("RaiseEvent_ReloadArea", false);
		bool flag = true;
		PhotonNetwork.RaiseEvent(0, targetUserId, flag, new RaiseEventOptions
		{
			Receivers = ReceiverGroup.All
		});
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x00089F4C File Offset: 0x0008834C
	private void OnEvent(byte eventcode, object content, int senderid)
	{
		if (eventcode == 0)
		{
			Log.Info("Got room event: ReloadArea", false);
			string text = (string)content;
			if (string.IsNullOrEmpty(text))
			{
				Log.Info("ReloadArea not targeted - reloading", false);
				Managers.areaManager.ReloadCurrentArea();
			}
			else if (text == Managers.personManager.ourPerson.userId)
			{
				Log.Info("ReloadArea was targeted at us", false);
				Managers.areaManager.ReloadCurrentArea();
			}
			else
			{
				Log.Info("ReloadArea was NOT targeted at us", false);
			}
		}
		else
		{
			Log.Warning("Got UNKNOWN room event: " + eventcode.ToString());
		}
	}

	// Token: 0x04001029 RID: 4137
	private string JoiningArea;

	// Token: 0x0400102A RID: 4138
	public const int MAX_NORMAL_PLAYERS_PER_ROOM = 10;

	// Token: 0x0400102B RID: 4139
	public const int MAX_PLAYERS_PER_ROOM = 12;

	// Token: 0x0400102C RID: 4140
	public string INFO_Current_Room;

	// Token: 0x0400102D RID: 4141
	public string INFO_Current_AppId;

	// Token: 0x0400102E RID: 4142
	public string INFO_Photon_Auth_UserId;

	// Token: 0x0400102F RID: 4143
	public int INFO_Current_PlayerCount;

	// Token: 0x04001030 RID: 4144
	public int INFO_ActorId;

	// Token: 0x04001031 RID: 4145
	public bool inPhotonDownMode;

	// Token: 0x020001E4 RID: 484
	private enum RoomEvents : byte
	{
		// Token: 0x04001033 RID: 4147
		ReloadArea
	}
}
