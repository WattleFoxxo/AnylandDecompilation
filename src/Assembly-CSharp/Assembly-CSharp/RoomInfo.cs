using System;
using ExitGames.Client.Photon;

// Token: 0x02000096 RID: 150
public class RoomInfo
{
	// Token: 0x06000591 RID: 1425 RVA: 0x00019357 File Offset: 0x00017757
	protected internal RoomInfo(string roomName, Hashtable properties)
	{
		this.InternalCacheProperties(properties);
		this.nameField = roomName;
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000592 RID: 1426 RVA: 0x00019391 File Offset: 0x00017791
	// (set) Token: 0x06000593 RID: 1427 RVA: 0x00019399 File Offset: 0x00017799
	public bool removedFromList { get; internal set; }

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000594 RID: 1428 RVA: 0x000193A2 File Offset: 0x000177A2
	// (set) Token: 0x06000595 RID: 1429 RVA: 0x000193AA File Offset: 0x000177AA
	protected internal bool serverSideMasterClient { get; private set; }

	// Token: 0x17000110 RID: 272
	// (get) Token: 0x06000596 RID: 1430 RVA: 0x000193B3 File Offset: 0x000177B3
	public Hashtable CustomProperties
	{
		get
		{
			return this.customPropertiesField;
		}
	}

	// Token: 0x17000111 RID: 273
	// (get) Token: 0x06000597 RID: 1431 RVA: 0x000193BB File Offset: 0x000177BB
	public string Name
	{
		get
		{
			return this.nameField;
		}
	}

	// Token: 0x17000112 RID: 274
	// (get) Token: 0x06000598 RID: 1432 RVA: 0x000193C3 File Offset: 0x000177C3
	// (set) Token: 0x06000599 RID: 1433 RVA: 0x000193CB File Offset: 0x000177CB
	public int PlayerCount { get; private set; }

	// Token: 0x17000113 RID: 275
	// (get) Token: 0x0600059A RID: 1434 RVA: 0x000193D4 File Offset: 0x000177D4
	// (set) Token: 0x0600059B RID: 1435 RVA: 0x000193DC File Offset: 0x000177DC
	public bool IsLocalClientInside { get; set; }

	// Token: 0x17000114 RID: 276
	// (get) Token: 0x0600059C RID: 1436 RVA: 0x000193E5 File Offset: 0x000177E5
	public byte MaxPlayers
	{
		get
		{
			return this.maxPlayersField;
		}
	}

	// Token: 0x17000115 RID: 277
	// (get) Token: 0x0600059D RID: 1437 RVA: 0x000193ED File Offset: 0x000177ED
	public bool IsOpen
	{
		get
		{
			return this.openField;
		}
	}

	// Token: 0x17000116 RID: 278
	// (get) Token: 0x0600059E RID: 1438 RVA: 0x000193F5 File Offset: 0x000177F5
	public bool IsVisible
	{
		get
		{
			return this.visibleField;
		}
	}

	// Token: 0x0600059F RID: 1439 RVA: 0x00019400 File Offset: 0x00017800
	public override bool Equals(object other)
	{
		RoomInfo roomInfo = other as RoomInfo;
		return roomInfo != null && this.Name.Equals(roomInfo.nameField);
	}

	// Token: 0x060005A0 RID: 1440 RVA: 0x0001942E File Offset: 0x0001782E
	public override int GetHashCode()
	{
		return this.nameField.GetHashCode();
	}

	// Token: 0x060005A1 RID: 1441 RVA: 0x0001943C File Offset: 0x0001783C
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

	// Token: 0x060005A2 RID: 1442 RVA: 0x000194B8 File Offset: 0x000178B8
	public string ToStringFull()
	{
		return string.Format("Room: '{0}' {1},{2} {4}/{3} players.\ncustomProps: {5}", new object[]
		{
			this.nameField,
			(!this.visibleField) ? "hidden" : "visible",
			(!this.openField) ? "closed" : "open",
			this.maxPlayersField,
			this.PlayerCount,
			this.customPropertiesField.ToStringFull()
		});
	}

	// Token: 0x060005A3 RID: 1443 RVA: 0x00019544 File Offset: 0x00017944
	protected internal void InternalCacheProperties(Hashtable propertiesToCache)
	{
		if (propertiesToCache == null || propertiesToCache.Count == 0 || this.customPropertiesField.Equals(propertiesToCache))
		{
			return;
		}
		if (propertiesToCache.ContainsKey(251))
		{
			this.removedFromList = (bool)propertiesToCache[251];
			if (this.removedFromList)
			{
				return;
			}
		}
		if (propertiesToCache.ContainsKey(255))
		{
			this.maxPlayersField = (byte)propertiesToCache[byte.MaxValue];
		}
		if (propertiesToCache.ContainsKey(253))
		{
			this.openField = (bool)propertiesToCache[253];
		}
		if (propertiesToCache.ContainsKey(254))
		{
			this.visibleField = (bool)propertiesToCache[254];
		}
		if (propertiesToCache.ContainsKey(252))
		{
			this.PlayerCount = (int)((byte)propertiesToCache[252]);
		}
		if (propertiesToCache.ContainsKey(249))
		{
			this.autoCleanUpField = (bool)propertiesToCache[249];
		}
		if (propertiesToCache.ContainsKey(248))
		{
			this.serverSideMasterClient = true;
			bool flag = this.masterClientIdField != 0;
			this.masterClientIdField = (int)propertiesToCache[248];
			if (flag)
			{
				PhotonNetwork.networkingPeer.UpdateMasterClient();
			}
		}
		if (propertiesToCache.ContainsKey(247))
		{
			this.expectedUsersField = (string[])propertiesToCache[247];
		}
		this.customPropertiesField.MergeStringKeys(propertiesToCache);
		this.customPropertiesField.StripKeysWithNullValues();
	}

	// Token: 0x17000117 RID: 279
	// (get) Token: 0x060005A4 RID: 1444 RVA: 0x0001973B File Offset: 0x00017B3B
	[Obsolete("Please use CustomProperties (updated case for naming).")]
	public Hashtable customProperties
	{
		get
		{
			return this.CustomProperties;
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00019743 File Offset: 0x00017B43
	[Obsolete("Please use Name (updated case for naming).")]
	public string name
	{
		get
		{
			return this.Name;
		}
	}

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x060005A6 RID: 1446 RVA: 0x0001974B File Offset: 0x00017B4B
	// (set) Token: 0x060005A7 RID: 1447 RVA: 0x00019753 File Offset: 0x00017B53
	[Obsolete("Please use PlayerCount (updated case for naming).")]
	public int playerCount
	{
		get
		{
			return this.PlayerCount;
		}
		set
		{
			this.PlayerCount = value;
		}
	}

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0001975C File Offset: 0x00017B5C
	// (set) Token: 0x060005A9 RID: 1449 RVA: 0x00019764 File Offset: 0x00017B64
	[Obsolete("Please use IsLocalClientInside (updated case for naming).")]
	public bool isLocalClientInside
	{
		get
		{
			return this.IsLocalClientInside;
		}
		set
		{
			this.IsLocalClientInside = value;
		}
	}

	// Token: 0x1700011B RID: 283
	// (get) Token: 0x060005AA RID: 1450 RVA: 0x0001976D File Offset: 0x00017B6D
	[Obsolete("Please use MaxPlayers (updated case for naming).")]
	public byte maxPlayers
	{
		get
		{
			return this.MaxPlayers;
		}
	}

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x060005AB RID: 1451 RVA: 0x00019775 File Offset: 0x00017B75
	[Obsolete("Please use IsOpen (updated case for naming).")]
	public bool open
	{
		get
		{
			return this.IsOpen;
		}
	}

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x060005AC RID: 1452 RVA: 0x0001977D File Offset: 0x00017B7D
	[Obsolete("Please use IsVisible (updated case for naming).")]
	public bool visible
	{
		get
		{
			return this.IsVisible;
		}
	}

	// Token: 0x040003EA RID: 1002
	private Hashtable customPropertiesField = new Hashtable();

	// Token: 0x040003EB RID: 1003
	protected byte maxPlayersField;

	// Token: 0x040003EC RID: 1004
	protected string[] expectedUsersField;

	// Token: 0x040003ED RID: 1005
	protected bool openField = true;

	// Token: 0x040003EE RID: 1006
	protected bool visibleField = true;

	// Token: 0x040003EF RID: 1007
	protected bool autoCleanUpField = PhotonNetwork.autoCleanUpPlayerObjects;

	// Token: 0x040003F0 RID: 1008
	protected string nameField;

	// Token: 0x040003F1 RID: 1009
	protected internal int masterClientIdField;
}
