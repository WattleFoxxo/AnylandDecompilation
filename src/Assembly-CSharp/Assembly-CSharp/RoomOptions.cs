using System;
using ExitGames.Client.Photon;

// Token: 0x02000070 RID: 112
public class RoomOptions
{
	// Token: 0x1700007D RID: 125
	// (get) Token: 0x06000343 RID: 835 RVA: 0x0000D9F1 File Offset: 0x0000BDF1
	// (set) Token: 0x06000344 RID: 836 RVA: 0x0000D9F9 File Offset: 0x0000BDF9
	public bool IsVisible
	{
		get
		{
			return this.isVisibleField;
		}
		set
		{
			this.isVisibleField = value;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000345 RID: 837 RVA: 0x0000DA02 File Offset: 0x0000BE02
	// (set) Token: 0x06000346 RID: 838 RVA: 0x0000DA0A File Offset: 0x0000BE0A
	public bool IsOpen
	{
		get
		{
			return this.isOpenField;
		}
		set
		{
			this.isOpenField = value;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x06000347 RID: 839 RVA: 0x0000DA13 File Offset: 0x0000BE13
	// (set) Token: 0x06000348 RID: 840 RVA: 0x0000DA1B File Offset: 0x0000BE1B
	public bool CleanupCacheOnLeave
	{
		get
		{
			return this.cleanupCacheOnLeaveField;
		}
		set
		{
			this.cleanupCacheOnLeaveField = value;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000349 RID: 841 RVA: 0x0000DA24 File Offset: 0x0000BE24
	public bool SuppressRoomEvents
	{
		get
		{
			return this.suppressRoomEventsField;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600034A RID: 842 RVA: 0x0000DA2C File Offset: 0x0000BE2C
	// (set) Token: 0x0600034B RID: 843 RVA: 0x0000DA34 File Offset: 0x0000BE34
	public bool PublishUserId
	{
		get
		{
			return this.publishUserIdField;
		}
		set
		{
			this.publishUserIdField = value;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x0600034C RID: 844 RVA: 0x0000DA3D File Offset: 0x0000BE3D
	// (set) Token: 0x0600034D RID: 845 RVA: 0x0000DA45 File Offset: 0x0000BE45
	public bool DeleteNullProperties
	{
		get
		{
			return this.deleteNullPropertiesField;
		}
		set
		{
			this.deleteNullPropertiesField = value;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x0600034E RID: 846 RVA: 0x0000DA4E File Offset: 0x0000BE4E
	// (set) Token: 0x0600034F RID: 847 RVA: 0x0000DA56 File Offset: 0x0000BE56
	[Obsolete("Use property with uppercase naming instead.")]
	public bool isVisible
	{
		get
		{
			return this.isVisibleField;
		}
		set
		{
			this.isVisibleField = value;
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000350 RID: 848 RVA: 0x0000DA5F File Offset: 0x0000BE5F
	// (set) Token: 0x06000351 RID: 849 RVA: 0x0000DA67 File Offset: 0x0000BE67
	[Obsolete("Use property with uppercase naming instead.")]
	public bool isOpen
	{
		get
		{
			return this.isOpenField;
		}
		set
		{
			this.isOpenField = value;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000352 RID: 850 RVA: 0x0000DA70 File Offset: 0x0000BE70
	// (set) Token: 0x06000353 RID: 851 RVA: 0x0000DA78 File Offset: 0x0000BE78
	[Obsolete("Use property with uppercase naming instead.")]
	public byte maxPlayers
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

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000354 RID: 852 RVA: 0x0000DA81 File Offset: 0x0000BE81
	// (set) Token: 0x06000355 RID: 853 RVA: 0x0000DA89 File Offset: 0x0000BE89
	[Obsolete("Use property with uppercase naming instead.")]
	public bool cleanupCacheOnLeave
	{
		get
		{
			return this.cleanupCacheOnLeaveField;
		}
		set
		{
			this.cleanupCacheOnLeaveField = value;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000356 RID: 854 RVA: 0x0000DA92 File Offset: 0x0000BE92
	// (set) Token: 0x06000357 RID: 855 RVA: 0x0000DA9A File Offset: 0x0000BE9A
	[Obsolete("Use property with uppercase naming instead.")]
	public Hashtable customRoomProperties
	{
		get
		{
			return this.CustomRoomProperties;
		}
		set
		{
			this.CustomRoomProperties = value;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000358 RID: 856 RVA: 0x0000DAA3 File Offset: 0x0000BEA3
	// (set) Token: 0x06000359 RID: 857 RVA: 0x0000DAAB File Offset: 0x0000BEAB
	[Obsolete("Use property with uppercase naming instead.")]
	public string[] customRoomPropertiesForLobby
	{
		get
		{
			return this.CustomRoomPropertiesForLobby;
		}
		set
		{
			this.CustomRoomPropertiesForLobby = value;
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x0600035A RID: 858 RVA: 0x0000DAB4 File Offset: 0x0000BEB4
	// (set) Token: 0x0600035B RID: 859 RVA: 0x0000DABC File Offset: 0x0000BEBC
	[Obsolete("Use property with uppercase naming instead.")]
	public string[] plugins
	{
		get
		{
			return this.Plugins;
		}
		set
		{
			this.Plugins = value;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x0600035C RID: 860 RVA: 0x0000DAC5 File Offset: 0x0000BEC5
	[Obsolete("Use property with uppercase naming instead.")]
	public bool suppressRoomEvents
	{
		get
		{
			return this.suppressRoomEventsField;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600035D RID: 861 RVA: 0x0000DACD File Offset: 0x0000BECD
	// (set) Token: 0x0600035E RID: 862 RVA: 0x0000DAD5 File Offset: 0x0000BED5
	[Obsolete("Use property with uppercase naming instead.")]
	public bool publishUserId
	{
		get
		{
			return this.publishUserIdField;
		}
		set
		{
			this.publishUserIdField = value;
		}
	}

	// Token: 0x040002BC RID: 700
	private bool isVisibleField = true;

	// Token: 0x040002BD RID: 701
	private bool isOpenField = true;

	// Token: 0x040002BE RID: 702
	public byte MaxPlayers;

	// Token: 0x040002BF RID: 703
	public int PlayerTtl;

	// Token: 0x040002C0 RID: 704
	public int EmptyRoomTtl;

	// Token: 0x040002C1 RID: 705
	private bool cleanupCacheOnLeaveField = PhotonNetwork.autoCleanUpPlayerObjects;

	// Token: 0x040002C2 RID: 706
	public Hashtable CustomRoomProperties;

	// Token: 0x040002C3 RID: 707
	public string[] CustomRoomPropertiesForLobby = new string[0];

	// Token: 0x040002C4 RID: 708
	public string[] Plugins;

	// Token: 0x040002C5 RID: 709
	private bool suppressRoomEventsField;

	// Token: 0x040002C6 RID: 710
	private bool publishUserIdField;

	// Token: 0x040002C7 RID: 711
	private bool deleteNullPropertiesField;
}
