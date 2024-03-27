using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x02000095 RID: 149
public class Room : RoomInfo
{
	// Token: 0x0600056C RID: 1388 RVA: 0x00019788 File Offset: 0x00017B88
	internal Room(string roomName, RoomOptions options)
		: base(roomName, null)
	{
		if (options == null)
		{
			options = new RoomOptions();
		}
		this.visibleField = options.IsVisible;
		this.openField = options.IsOpen;
		this.maxPlayersField = options.MaxPlayers;
		this.autoCleanUpField = false;
		base.InternalCacheProperties(options.CustomRoomProperties);
		this.PropertiesListedInLobby = options.CustomRoomPropertiesForLobby;
	}

	// Token: 0x170000FC RID: 252
	// (get) Token: 0x0600056D RID: 1389 RVA: 0x000197ED File Offset: 0x00017BED
	// (set) Token: 0x0600056E RID: 1390 RVA: 0x000197F5 File Offset: 0x00017BF5
	public new string Name
	{
		get
		{
			return this.nameField;
		}
		internal set
		{
			this.nameField = value;
		}
	}

	// Token: 0x170000FD RID: 253
	// (get) Token: 0x0600056F RID: 1391 RVA: 0x000197FE File Offset: 0x00017BFE
	// (set) Token: 0x06000570 RID: 1392 RVA: 0x00019808 File Offset: 0x00017C08
	public new bool IsOpen
	{
		get
		{
			return this.openField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set open when not in that room.");
			}
			if (value != this.openField && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable { { 253, value } }, null, false);
			}
			this.openField = value;
		}
	}

	// Token: 0x170000FE RID: 254
	// (get) Token: 0x06000571 RID: 1393 RVA: 0x00019876 File Offset: 0x00017C76
	// (set) Token: 0x06000572 RID: 1394 RVA: 0x00019880 File Offset: 0x00017C80
	public new bool IsVisible
	{
		get
		{
			return this.visibleField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set visible when not in that room.");
			}
			if (value != this.visibleField && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable { { 254, value } }, null, false);
			}
			this.visibleField = value;
		}
	}

	// Token: 0x170000FF RID: 255
	// (get) Token: 0x06000573 RID: 1395 RVA: 0x000198EE File Offset: 0x00017CEE
	// (set) Token: 0x06000574 RID: 1396 RVA: 0x000198F6 File Offset: 0x00017CF6
	public string[] PropertiesListedInLobby { get; private set; }

	// Token: 0x17000100 RID: 256
	// (get) Token: 0x06000575 RID: 1397 RVA: 0x000198FF File Offset: 0x00017CFF
	public bool AutoCleanUp
	{
		get
		{
			return this.autoCleanUpField;
		}
	}

	// Token: 0x17000101 RID: 257
	// (get) Token: 0x06000576 RID: 1398 RVA: 0x00019907 File Offset: 0x00017D07
	// (set) Token: 0x06000577 RID: 1399 RVA: 0x00019910 File Offset: 0x00017D10
	public new int MaxPlayers
	{
		get
		{
			return (int)this.maxPlayersField;
		}
		set
		{
			if (!this.Equals(PhotonNetwork.room))
			{
				Debug.LogWarning("Can't set MaxPlayers when not in that room.");
			}
			if (value > 255)
			{
				Debug.LogWarning("Can't set Room.MaxPlayers to: " + value + ". Using max value: 255.");
				value = 255;
			}
			if (value != (int)this.maxPlayersField && !PhotonNetwork.offlineMode)
			{
				PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(new Hashtable { 
				{
					byte.MaxValue,
					(byte)value
				} }, null, false);
			}
			this.maxPlayersField = (byte)value;
		}
	}

	// Token: 0x17000102 RID: 258
	// (get) Token: 0x06000578 RID: 1400 RVA: 0x000199AC File Offset: 0x00017DAC
	public new int PlayerCount
	{
		get
		{
			if (PhotonNetwork.playerList != null)
			{
				return PhotonNetwork.playerList.Length;
			}
			return 0;
		}
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000579 RID: 1401 RVA: 0x000199C1 File Offset: 0x00017DC1
	public string[] ExpectedUsers
	{
		get
		{
			return this.expectedUsersField;
		}
	}

	// Token: 0x17000104 RID: 260
	// (get) Token: 0x0600057A RID: 1402 RVA: 0x000199C9 File Offset: 0x00017DC9
	// (set) Token: 0x0600057B RID: 1403 RVA: 0x000199D1 File Offset: 0x00017DD1
	protected internal int MasterClientId
	{
		get
		{
			return this.masterClientIdField;
		}
		set
		{
			this.masterClientIdField = value;
		}
	}

	// Token: 0x0600057C RID: 1404 RVA: 0x000199DC File Offset: 0x00017DDC
	public void SetCustomProperties(Hashtable propertiesToSet, Hashtable expectedValues = null, bool webForward = false)
	{
		if (propertiesToSet == null)
		{
			return;
		}
		Hashtable hashtable = propertiesToSet.StripToStringKeys();
		Hashtable hashtable2 = expectedValues.StripToStringKeys();
		bool flag = hashtable2 == null || hashtable2.Count == 0;
		if (PhotonNetwork.offlineMode || flag)
		{
			base.CustomProperties.Merge(hashtable);
			base.CustomProperties.StripKeysWithNullValues();
		}
		if (!PhotonNetwork.offlineMode)
		{
			PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, hashtable2, webForward);
		}
		if (PhotonNetwork.offlineMode || flag)
		{
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonCustomRoomPropertiesChanged, new object[] { hashtable });
		}
	}

	// Token: 0x0600057D RID: 1405 RVA: 0x00019A74 File Offset: 0x00017E74
	public void SetPropertiesListedInLobby(string[] propsListedInLobby)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[250] = propsListedInLobby;
		PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, null, false);
		this.PropertiesListedInLobby = propsListedInLobby;
	}

	// Token: 0x0600057E RID: 1406 RVA: 0x00019AB0 File Offset: 0x00017EB0
	public void ClearExpectedUsers()
	{
		Hashtable hashtable = new Hashtable();
		hashtable[247] = new string[0];
		Hashtable hashtable2 = new Hashtable();
		hashtable2[247] = this.ExpectedUsers;
		PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, hashtable2, false);
	}

	// Token: 0x0600057F RID: 1407 RVA: 0x00019B04 File Offset: 0x00017F04
	public void SetExpectedUsers(string[] expectedUsers)
	{
		Hashtable hashtable = new Hashtable();
		hashtable[247] = expectedUsers;
		Hashtable hashtable2 = new Hashtable();
		hashtable2[247] = this.ExpectedUsers;
		PhotonNetwork.networkingPeer.OpSetPropertiesOfRoom(hashtable, hashtable2, false);
	}

	// Token: 0x06000580 RID: 1408 RVA: 0x00019B54 File Offset: 0x00017F54
	public override string ToString()
	{
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.", new object[]
		{
			this.nameField,
			(!this.visibleField) ? "hidden" : "visible",
			(!this.openField) ? "closed" : "open",
			this.maxPlayersField,
			this.PlayerCount
		});
	}

	// Token: 0x06000581 RID: 1409 RVA: 0x00019BD0 File Offset: 0x00017FD0
	public new string ToStringFull()
	{
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", new object[]
		{
			this.nameField,
			(!this.visibleField) ? "hidden" : "visible",
			(!this.openField) ? "closed" : "open",
			this.maxPlayersField,
			this.PlayerCount,
			base.CustomProperties.ToStringFull()
		});
	}

	// Token: 0x17000105 RID: 261
	// (get) Token: 0x06000582 RID: 1410 RVA: 0x00019C5A File Offset: 0x0001805A
	// (set) Token: 0x06000583 RID: 1411 RVA: 0x00019C62 File Offset: 0x00018062
	[Obsolete("Please use Name (updated case for naming).")]
	public new string name
	{
		get
		{
			return this.Name;
		}
		internal set
		{
			this.Name = value;
		}
	}

	// Token: 0x17000106 RID: 262
	// (get) Token: 0x06000584 RID: 1412 RVA: 0x00019C6B File Offset: 0x0001806B
	// (set) Token: 0x06000585 RID: 1413 RVA: 0x00019C73 File Offset: 0x00018073
	[Obsolete("Please use IsOpen (updated case for naming).")]
	public new bool open
	{
		get
		{
			return this.IsOpen;
		}
		set
		{
			this.IsOpen = value;
		}
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x06000586 RID: 1414 RVA: 0x00019C7C File Offset: 0x0001807C
	// (set) Token: 0x06000587 RID: 1415 RVA: 0x00019C84 File Offset: 0x00018084
	[Obsolete("Please use IsVisible (updated case for naming).")]
	public new bool visible
	{
		get
		{
			return this.IsVisible;
		}
		set
		{
			this.IsVisible = value;
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x06000588 RID: 1416 RVA: 0x00019C8D File Offset: 0x0001808D
	// (set) Token: 0x06000589 RID: 1417 RVA: 0x00019C95 File Offset: 0x00018095
	[Obsolete("Please use PropertiesListedInLobby (updated case for naming).")]
	public string[] propertiesListedInLobby
	{
		get
		{
			return this.PropertiesListedInLobby;
		}
		private set
		{
			this.PropertiesListedInLobby = value;
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x0600058A RID: 1418 RVA: 0x00019C9E File Offset: 0x0001809E
	[Obsolete("Please use AutoCleanUp (updated case for naming).")]
	public bool autoCleanUp
	{
		get
		{
			return this.AutoCleanUp;
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x0600058B RID: 1419 RVA: 0x00019CA6 File Offset: 0x000180A6
	// (set) Token: 0x0600058C RID: 1420 RVA: 0x00019CAE File Offset: 0x000180AE
	[Obsolete("Please use MaxPlayers (updated case for naming).")]
	public new int maxPlayers
	{
		get
		{
			return this.MaxPlayers;
		}
		set
		{
			this.MaxPlayers = value;
		}
	}

	// Token: 0x1700010B RID: 267
	// (get) Token: 0x0600058D RID: 1421 RVA: 0x00019CB7 File Offset: 0x000180B7
	[Obsolete("Please use PlayerCount (updated case for naming).")]
	public new int playerCount
	{
		get
		{
			return this.PlayerCount;
		}
	}

	// Token: 0x1700010C RID: 268
	// (get) Token: 0x0600058E RID: 1422 RVA: 0x00019CBF File Offset: 0x000180BF
	[Obsolete("Please use ExpectedUsers (updated case for naming).")]
	public string[] expectedUsers
	{
		get
		{
			return this.ExpectedUsers;
		}
	}

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x0600058F RID: 1423 RVA: 0x00019CC7 File Offset: 0x000180C7
	// (set) Token: 0x06000590 RID: 1424 RVA: 0x00019CCF File Offset: 0x000180CF
	[Obsolete("Please use MasterClientId (updated case for naming).")]
	protected internal int masterClientId
	{
		get
		{
			return this.MasterClientId;
		}
		set
		{
			this.MasterClientId = value;
		}
	}
}
