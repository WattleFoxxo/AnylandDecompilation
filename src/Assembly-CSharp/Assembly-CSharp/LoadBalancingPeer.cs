using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000061 RID: 97
internal class LoadBalancingPeer : PhotonPeer
{
	// Token: 0x06000323 RID: 803 RVA: 0x0000CA66 File Offset: 0x0000AE66
	public LoadBalancingPeer(ConnectionProtocol protocolType)
		: base(protocolType)
	{
	}

	// Token: 0x06000324 RID: 804 RVA: 0x0000CA7A File Offset: 0x0000AE7A
	public LoadBalancingPeer(IPhotonPeerListener listener, ConnectionProtocol protocolType)
		: this(protocolType)
	{
		base.Listener = listener;
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x06000325 RID: 805 RVA: 0x0000CA8A File Offset: 0x0000AE8A
	internal bool IsProtocolSecure
	{
		get
		{
			return base.UsedProtocol == ConnectionProtocol.WebSocketSecure;
		}
	}

	// Token: 0x06000326 RID: 806 RVA: 0x0000CA98 File Offset: 0x0000AE98
	public virtual bool OpGetRegions(string appId)
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[224] = appId;
		return this.OpCustom(220, dictionary, true, 0, true);
	}

	// Token: 0x06000327 RID: 807 RVA: 0x0000CAC8 File Offset: 0x0000AEC8
	public virtual bool OpJoinLobby(TypedLobby lobby = null)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinLobby()");
		}
		Dictionary<byte, object> dictionary = null;
		if (lobby != null && !lobby.IsDefault)
		{
			dictionary = new Dictionary<byte, object>();
			dictionary[213] = lobby.Name;
			dictionary[212] = (byte)lobby.Type;
		}
		return this.OpCustom(229, dictionary, true);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x0000CB3F File Offset: 0x0000AF3F
	public virtual bool OpLeaveLobby()
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpLeaveLobby()");
		}
		return this.OpCustom(228, null, true);
	}

	// Token: 0x06000329 RID: 809 RVA: 0x0000CB6C File Offset: 0x0000AF6C
	private void RoomOptionsToOpParameters(Dictionary<byte, object> op, RoomOptions roomOptions)
	{
		if (roomOptions == null)
		{
			roomOptions = new RoomOptions();
		}
		Hashtable hashtable = new Hashtable();
		hashtable[253] = roomOptions.IsOpen;
		hashtable[254] = roomOptions.IsVisible;
		hashtable[250] = ((roomOptions.CustomRoomPropertiesForLobby != null) ? roomOptions.CustomRoomPropertiesForLobby : new string[0]);
		hashtable.MergeStringKeys(roomOptions.CustomRoomProperties);
		if (roomOptions.MaxPlayers > 0)
		{
			hashtable[byte.MaxValue] = roomOptions.MaxPlayers;
		}
		op[248] = hashtable;
		int num = 0;
		op[241] = roomOptions.CleanupCacheOnLeave;
		if (roomOptions.CleanupCacheOnLeave)
		{
			num |= 2;
			hashtable[249] = true;
		}
		if (roomOptions.PlayerTtl > 0 || roomOptions.PlayerTtl == -1)
		{
			num |= 1;
			op[232] = true;
			op[235] = roomOptions.PlayerTtl;
		}
		if (roomOptions.EmptyRoomTtl > 0)
		{
			op[236] = roomOptions.EmptyRoomTtl;
		}
		if (roomOptions.SuppressRoomEvents)
		{
			num |= 4;
			op[237] = true;
		}
		if (roomOptions.Plugins != null)
		{
			op[204] = roomOptions.Plugins;
		}
		if (roomOptions.PublishUserId)
		{
			num |= 8;
			op[239] = true;
		}
		if (roomOptions.DeleteNullProperties)
		{
			num |= 16;
		}
		op[191] = num;
	}

	// Token: 0x0600032A RID: 810 RVA: 0x0000CD4C File Offset: 0x0000B14C
	public virtual bool OpCreateRoom(EnterRoomParams opParams)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpCreateRoom()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (!string.IsNullOrEmpty(opParams.RoomName))
		{
			dictionary[byte.MaxValue] = opParams.RoomName;
		}
		if (opParams.Lobby != null && !string.IsNullOrEmpty(opParams.Lobby.Name))
		{
			dictionary[213] = opParams.Lobby.Name;
			dictionary[212] = (byte)opParams.Lobby.Type;
		}
		if (opParams.ExpectedUsers != null && opParams.ExpectedUsers.Length > 0)
		{
			dictionary[238] = opParams.ExpectedUsers;
		}
		if (opParams.OnGameServer)
		{
			if (opParams.PlayerProperties != null && opParams.PlayerProperties.Count > 0)
			{
				dictionary[249] = opParams.PlayerProperties;
				dictionary[250] = true;
			}
			this.RoomOptionsToOpParameters(dictionary, opParams.RoomOptions);
		}
		return this.OpCustom(227, dictionary, true);
	}

	// Token: 0x0600032B RID: 811 RVA: 0x0000CE7C File Offset: 0x0000B27C
	public virtual bool OpJoinRoom(EnterRoomParams opParams)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinRoom()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (!string.IsNullOrEmpty(opParams.RoomName))
		{
			dictionary[byte.MaxValue] = opParams.RoomName;
		}
		if (opParams.CreateIfNotExists)
		{
			dictionary[215] = 1;
			if (opParams.Lobby != null)
			{
				dictionary[213] = opParams.Lobby.Name;
				dictionary[212] = (byte)opParams.Lobby.Type;
			}
		}
		if (opParams.RejoinOnly)
		{
			dictionary[215] = 3;
		}
		if (opParams.ExpectedUsers != null && opParams.ExpectedUsers.Length > 0)
		{
			dictionary[238] = opParams.ExpectedUsers;
		}
		if (opParams.OnGameServer)
		{
			if (opParams.PlayerProperties != null && opParams.PlayerProperties.Count > 0)
			{
				dictionary[249] = opParams.PlayerProperties;
				dictionary[250] = true;
			}
			if (opParams.CreateIfNotExists)
			{
				this.RoomOptionsToOpParameters(dictionary, opParams.RoomOptions);
			}
		}
		return this.OpCustom(226, dictionary, true);
	}

	// Token: 0x0600032C RID: 812 RVA: 0x0000CFDC File Offset: 0x0000B3DC
	public virtual bool OpJoinRandomRoom(OpJoinRandomRoomParams opJoinRandomRoomParams)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpJoinRandomRoom()");
		}
		Hashtable hashtable = new Hashtable();
		hashtable.MergeStringKeys(opJoinRandomRoomParams.ExpectedCustomRoomProperties);
		if (opJoinRandomRoomParams.ExpectedMaxPlayers > 0)
		{
			hashtable[byte.MaxValue] = opJoinRandomRoomParams.ExpectedMaxPlayers;
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (hashtable.Count > 0)
		{
			dictionary[248] = hashtable;
		}
		if (opJoinRandomRoomParams.MatchingType != MatchmakingMode.FillRoom)
		{
			dictionary[223] = (byte)opJoinRandomRoomParams.MatchingType;
		}
		if (opJoinRandomRoomParams.TypedLobby != null && !string.IsNullOrEmpty(opJoinRandomRoomParams.TypedLobby.Name))
		{
			dictionary[213] = opJoinRandomRoomParams.TypedLobby.Name;
			dictionary[212] = (byte)opJoinRandomRoomParams.TypedLobby.Type;
		}
		if (!string.IsNullOrEmpty(opJoinRandomRoomParams.SqlLobbyFilter))
		{
			dictionary[245] = opJoinRandomRoomParams.SqlLobbyFilter;
		}
		if (opJoinRandomRoomParams.ExpectedUsers != null && opJoinRandomRoomParams.ExpectedUsers.Length > 0)
		{
			dictionary[238] = opJoinRandomRoomParams.ExpectedUsers;
		}
		return this.OpCustom(225, dictionary, true);
	}

	// Token: 0x0600032D RID: 813 RVA: 0x0000D128 File Offset: 0x0000B528
	public virtual bool OpLeaveRoom(bool becomeInactive)
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (becomeInactive)
		{
			dictionary[233] = becomeInactive;
		}
		return this.OpCustom(254, dictionary, true);
	}

	// Token: 0x0600032E RID: 814 RVA: 0x0000D160 File Offset: 0x0000B560
	public virtual bool OpGetGameList(TypedLobby lobby, string queryData)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpGetGameList()");
		}
		if (lobby == null)
		{
			if (this.DebugOut >= DebugLevel.INFO)
			{
				base.Listener.DebugReturn(DebugLevel.INFO, "OpGetGameList not sent. Lobby cannot be null.");
			}
			return false;
		}
		if (lobby.Type != LobbyType.SqlLobby)
		{
			if (this.DebugOut >= DebugLevel.INFO)
			{
				base.Listener.DebugReturn(DebugLevel.INFO, "OpGetGameList not sent. LobbyType must be SqlLobby.");
			}
			return false;
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary[213] = lobby.Name;
		dictionary[212] = (byte)lobby.Type;
		dictionary[245] = queryData;
		return this.OpCustom(217, dictionary, true);
	}

	// Token: 0x0600032F RID: 815 RVA: 0x0000D220 File Offset: 0x0000B620
	public virtual bool OpFindFriends(string[] friendsToFind)
	{
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (friendsToFind != null && friendsToFind.Length > 0)
		{
			dictionary[1] = friendsToFind;
		}
		return this.OpCustom(222, dictionary, true);
	}

	// Token: 0x06000330 RID: 816 RVA: 0x0000D257 File Offset: 0x0000B657
	public bool OpSetCustomPropertiesOfActor(int actorNr, Hashtable actorProperties)
	{
		return this.OpSetPropertiesOfActor(actorNr, actorProperties.StripToStringKeys(), null, false);
	}

	// Token: 0x06000331 RID: 817 RVA: 0x0000D268 File Offset: 0x0000B668
	protected internal bool OpSetPropertiesOfActor(int actorNr, Hashtable actorProperties, Hashtable expectedProperties = null, bool webForward = false)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfActor()");
		}
		if (actorNr <= 0 || actorProperties == null)
		{
			if (this.DebugOut >= DebugLevel.INFO)
			{
				base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfActor not sent. ActorNr must be > 0 and actorProperties != null.");
			}
			return false;
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary.Add(251, actorProperties);
		dictionary.Add(254, actorNr);
		dictionary.Add(250, true);
		if (expectedProperties != null && expectedProperties.Count != 0)
		{
			dictionary.Add(231, expectedProperties);
		}
		if (webForward)
		{
			dictionary[234] = true;
		}
		return this.OpCustom(252, dictionary, true, 0, false);
	}

	// Token: 0x06000332 RID: 818 RVA: 0x0000D338 File Offset: 0x0000B738
	protected void OpSetPropertyOfRoom(byte propCode, object value)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[propCode] = value;
		this.OpSetPropertiesOfRoom(hashtable, null, false);
	}

	// Token: 0x06000333 RID: 819 RVA: 0x0000D362 File Offset: 0x0000B762
	public bool OpSetCustomPropertiesOfRoom(Hashtable gameProperties, bool broadcast, byte channelId)
	{
		return this.OpSetPropertiesOfRoom(gameProperties.StripToStringKeys(), null, false);
	}

	// Token: 0x06000334 RID: 820 RVA: 0x0000D374 File Offset: 0x0000B774
	protected internal bool OpSetPropertiesOfRoom(Hashtable gameProperties, Hashtable expectedProperties = null, bool webForward = false)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpSetPropertiesOfRoom()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		dictionary.Add(251, gameProperties);
		dictionary.Add(250, true);
		if (expectedProperties != null && expectedProperties.Count != 0)
		{
			dictionary.Add(231, expectedProperties);
		}
		if (webForward)
		{
			dictionary[234] = true;
		}
		return this.OpCustom(252, dictionary, true, 0, false);
	}

	// Token: 0x06000335 RID: 821 RVA: 0x0000D404 File Offset: 0x0000B804
	public virtual bool OpAuthenticate(string appId, string appVersion, AuthenticationValues authValues, string regionCode, bool getLobbyStatistics)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpAuthenticate()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (getLobbyStatistics)
		{
			dictionary[211] = true;
		}
		if (authValues != null && authValues.Token != null)
		{
			dictionary[221] = authValues.Token;
			return this.OpCustom(230, dictionary, true, 0, false);
		}
		dictionary[220] = appVersion;
		dictionary[224] = appId;
		if (!string.IsNullOrEmpty(regionCode))
		{
			dictionary[210] = regionCode;
		}
		if (authValues != null)
		{
			if (!string.IsNullOrEmpty(authValues.UserId))
			{
				dictionary[225] = authValues.UserId;
			}
			if (authValues.AuthType != CustomAuthenticationType.None)
			{
				if (!this.IsProtocolSecure && !base.IsEncryptionAvailable)
				{
					base.Listener.DebugReturn(DebugLevel.ERROR, "OpAuthenticate() failed. When you want Custom Authentication encryption is mandatory.");
					return false;
				}
				dictionary[217] = (byte)authValues.AuthType;
				if (!string.IsNullOrEmpty(authValues.Token))
				{
					dictionary[221] = authValues.Token;
				}
				else
				{
					if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
					{
						dictionary[216] = authValues.AuthGetParameters;
					}
					if (authValues.AuthPostData != null)
					{
						dictionary[214] = authValues.AuthPostData;
					}
				}
			}
		}
		bool flag = this.OpCustom(230, dictionary, true, 0, base.IsEncryptionAvailable);
		if (!flag)
		{
			base.Listener.DebugReturn(DebugLevel.ERROR, "Error calling OpAuthenticate! Did not work. Check log output, AuthValues and if you're connected.");
		}
		return flag;
	}

	// Token: 0x06000336 RID: 822 RVA: 0x0000D5B8 File Offset: 0x0000B9B8
	public virtual bool OpAuthenticateOnce(string appId, string appVersion, AuthenticationValues authValues, string regionCode, EncryptionMode encryptionMode, ConnectionProtocol expectedProtocol)
	{
		if (this.DebugOut >= DebugLevel.INFO)
		{
			base.Listener.DebugReturn(DebugLevel.INFO, "OpAuthenticate()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (authValues != null && authValues.Token != null)
		{
			dictionary[221] = authValues.Token;
			return this.OpCustom(231, dictionary, true, 0, false);
		}
		if (encryptionMode == EncryptionMode.DatagramEncryption && expectedProtocol != ConnectionProtocol.Udp)
		{
			Debug.LogWarning("Expected protocol set to UDP, due to encryption mode DatagramEncryption. Changing protocol in PhotonServerSettings from: " + PhotonNetwork.PhotonServerSettings.Protocol);
			PhotonNetwork.PhotonServerSettings.Protocol = ConnectionProtocol.Udp;
			expectedProtocol = ConnectionProtocol.Udp;
		}
		dictionary[195] = (byte)expectedProtocol;
		dictionary[193] = (byte)encryptionMode;
		dictionary[220] = appVersion;
		dictionary[224] = appId;
		if (!string.IsNullOrEmpty(regionCode))
		{
			dictionary[210] = regionCode;
		}
		if (authValues != null)
		{
			if (!string.IsNullOrEmpty(authValues.UserId))
			{
				dictionary[225] = authValues.UserId;
			}
			if (authValues.AuthType != CustomAuthenticationType.None)
			{
				dictionary[217] = (byte)authValues.AuthType;
				if (!string.IsNullOrEmpty(authValues.Token))
				{
					dictionary[221] = authValues.Token;
				}
				else
				{
					if (!string.IsNullOrEmpty(authValues.AuthGetParameters))
					{
						dictionary[216] = authValues.AuthGetParameters;
					}
					if (authValues.AuthPostData != null)
					{
						dictionary[214] = authValues.AuthPostData;
					}
				}
			}
		}
		return this.OpCustom(231, dictionary, true, 0, base.IsEncryptionAvailable);
	}

	// Token: 0x06000337 RID: 823 RVA: 0x0000D770 File Offset: 0x0000BB70
	public virtual bool OpChangeGroups(byte[] groupsToRemove, byte[] groupsToAdd)
	{
		if (this.DebugOut >= DebugLevel.ALL)
		{
			base.Listener.DebugReturn(DebugLevel.ALL, "OpChangeGroups()");
		}
		Dictionary<byte, object> dictionary = new Dictionary<byte, object>();
		if (groupsToRemove != null)
		{
			dictionary[239] = groupsToRemove;
		}
		if (groupsToAdd != null)
		{
			dictionary[238] = groupsToAdd;
		}
		return this.OpCustom(248, dictionary, true, 0);
	}

	// Token: 0x06000338 RID: 824 RVA: 0x0000D7D4 File Offset: 0x0000BBD4
	public virtual bool OpRaiseEvent(byte eventCode, object customEventContent, bool sendReliable, RaiseEventOptions raiseEventOptions)
	{
		this.opParameters.Clear();
		this.opParameters[244] = eventCode;
		if (customEventContent != null)
		{
			this.opParameters[245] = customEventContent;
		}
		if (raiseEventOptions == null)
		{
			raiseEventOptions = RaiseEventOptions.Default;
		}
		else
		{
			if (raiseEventOptions.CachingOption != EventCaching.DoNotCache)
			{
				this.opParameters[247] = (byte)raiseEventOptions.CachingOption;
			}
			if (raiseEventOptions.Receivers != ReceiverGroup.Others)
			{
				this.opParameters[246] = (byte)raiseEventOptions.Receivers;
			}
			if (raiseEventOptions.InterestGroup != 0)
			{
				this.opParameters[240] = raiseEventOptions.InterestGroup;
			}
			if (raiseEventOptions.TargetActors != null)
			{
				this.opParameters[252] = raiseEventOptions.TargetActors;
			}
			if (raiseEventOptions.ForwardToWebhook)
			{
				this.opParameters[234] = true;
			}
		}
		return this.OpCustom(253, this.opParameters, sendReliable, raiseEventOptions.SequenceChannel, raiseEventOptions.Encrypt);
	}

	// Token: 0x06000339 RID: 825 RVA: 0x0000D90C File Offset: 0x0000BD0C
	public virtual bool OpSettings(bool receiveLobbyStats)
	{
		if (this.DebugOut >= DebugLevel.ALL)
		{
			base.Listener.DebugReturn(DebugLevel.ALL, "OpSettings()");
		}
		this.opParameters.Clear();
		if (receiveLobbyStats)
		{
			this.opParameters[0] = receiveLobbyStats;
		}
		return this.opParameters.Count == 0 || this.OpCustom(218, this.opParameters, true);
	}

	// Token: 0x040001F9 RID: 505
	private readonly Dictionary<byte, object> opParameters = new Dictionary<byte, object>();

	// Token: 0x02000062 RID: 98
	private enum RoomOptionBit
	{
		// Token: 0x040001FB RID: 507
		CheckUserOnJoin = 1,
		// Token: 0x040001FC RID: 508
		DeleteCacheOnLeave,
		// Token: 0x040001FD RID: 509
		SuppressRoomEvents = 4,
		// Token: 0x040001FE RID: 510
		PublishUserId = 8,
		// Token: 0x040001FF RID: 511
		DeleteNullProps = 16,
		// Token: 0x04000200 RID: 512
		BroadcastPropsChangeToAll = 32
	}
}
