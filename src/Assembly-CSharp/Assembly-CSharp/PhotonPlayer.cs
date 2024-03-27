using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x0200008B RID: 139
public class PhotonPlayer : IComparable<PhotonPlayer>, IComparable<int>, IEquatable<PhotonPlayer>, IEquatable<int>
{
	// Token: 0x0600050C RID: 1292 RVA: 0x000174B7 File Offset: 0x000158B7
	public PhotonPlayer(bool isLocal, int actorID, string name)
	{
		this.CustomProperties = new Hashtable();
		this.IsLocal = isLocal;
		this.actorID = actorID;
		this.nameField = name;
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x000174F1 File Offset: 0x000158F1
	protected internal PhotonPlayer(bool isLocal, int actorID, Hashtable properties)
	{
		this.CustomProperties = new Hashtable();
		this.IsLocal = isLocal;
		this.actorID = actorID;
		this.InternalCacheProperties(properties);
	}

	// Token: 0x170000E3 RID: 227
	// (get) Token: 0x0600050E RID: 1294 RVA: 0x0001752B File Offset: 0x0001592B
	public int ID
	{
		get
		{
			return this.actorID;
		}
	}

	// Token: 0x170000E4 RID: 228
	// (get) Token: 0x0600050F RID: 1295 RVA: 0x00017533 File Offset: 0x00015933
	// (set) Token: 0x06000510 RID: 1296 RVA: 0x0001753C File Offset: 0x0001593C
	public string NickName
	{
		get
		{
			return this.nameField;
		}
		set
		{
			if (!this.IsLocal)
			{
				Debug.LogError("Error: Cannot change the name of a remote player!");
				return;
			}
			if (string.IsNullOrEmpty(value) || value.Equals(this.nameField))
			{
				return;
			}
			this.nameField = value;
			PhotonNetwork.playerName = value;
		}
	}

	// Token: 0x170000E5 RID: 229
	// (get) Token: 0x06000511 RID: 1297 RVA: 0x00017589 File Offset: 0x00015989
	// (set) Token: 0x06000512 RID: 1298 RVA: 0x00017591 File Offset: 0x00015991
	public string UserId { get; internal set; }

	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000513 RID: 1299 RVA: 0x0001759A File Offset: 0x0001599A
	public bool IsMasterClient
	{
		get
		{
			return PhotonNetwork.networkingPeer.mMasterClientId == this.ID;
		}
	}

	// Token: 0x170000E7 RID: 231
	// (get) Token: 0x06000514 RID: 1300 RVA: 0x000175AE File Offset: 0x000159AE
	// (set) Token: 0x06000515 RID: 1301 RVA: 0x000175B6 File Offset: 0x000159B6
	public bool IsInactive { get; set; }

	// Token: 0x170000E8 RID: 232
	// (get) Token: 0x06000516 RID: 1302 RVA: 0x000175BF File Offset: 0x000159BF
	// (set) Token: 0x06000517 RID: 1303 RVA: 0x000175C7 File Offset: 0x000159C7
	public Hashtable CustomProperties { get; internal set; }

	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x06000518 RID: 1304 RVA: 0x000175D0 File Offset: 0x000159D0
	public Hashtable AllProperties
	{
		get
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Merge(this.CustomProperties);
			hashtable[byte.MaxValue] = this.NickName;
			return hashtable;
		}
	}

	// Token: 0x06000519 RID: 1305 RVA: 0x00017608 File Offset: 0x00015A08
	public override bool Equals(object p)
	{
		PhotonPlayer photonPlayer = p as PhotonPlayer;
		return photonPlayer != null && this.GetHashCode() == photonPlayer.GetHashCode();
	}

	// Token: 0x0600051A RID: 1306 RVA: 0x00017633 File Offset: 0x00015A33
	public override int GetHashCode()
	{
		return this.ID;
	}

	// Token: 0x0600051B RID: 1307 RVA: 0x0001763B File Offset: 0x00015A3B
	internal void InternalChangeLocalID(int newID)
	{
		if (!this.IsLocal)
		{
			Debug.LogError("ERROR You should never change PhotonPlayer IDs!");
			return;
		}
		this.actorID = newID;
	}

	// Token: 0x0600051C RID: 1308 RVA: 0x0001765C File Offset: 0x00015A5C
	internal void InternalCacheProperties(Hashtable properties)
	{
		if (properties == null || properties.Count == 0 || this.CustomProperties.Equals(properties))
		{
			return;
		}
		if (properties.ContainsKey(255))
		{
			this.nameField = (string)properties[byte.MaxValue];
		}
		if (properties.ContainsKey(253))
		{
			this.UserId = (string)properties[253];
		}
		if (properties.ContainsKey(254))
		{
			this.IsInactive = (bool)properties[254];
		}
		this.CustomProperties.MergeStringKeys(properties);
		this.CustomProperties.StripKeysWithNullValues();
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x00017734 File Offset: 0x00015B34
	public void SetCustomProperties(Hashtable propertiesToSet, Hashtable expectedValues = null, bool webForward = false)
	{
		if (propertiesToSet == null)
		{
			return;
		}
		Hashtable hashtable = propertiesToSet.StripToStringKeys();
		Hashtable hashtable2 = expectedValues.StripToStringKeys();
		bool flag = hashtable2 == null || hashtable2.Count == 0;
		bool flag2 = this.actorID > 0 && !PhotonNetwork.offlineMode;
		if (flag)
		{
			this.CustomProperties.Merge(hashtable);
			this.CustomProperties.StripKeysWithNullValues();
		}
		if (flag2)
		{
			PhotonNetwork.networkingPeer.OpSetPropertiesOfActor(this.actorID, hashtable, hashtable2, webForward);
		}
		if (!flag2 || flag)
		{
			this.InternalCacheProperties(hashtable);
			NetworkingPeer.SendMonoMessage(PhotonNetworkingMessage.OnPhotonPlayerPropertiesChanged, new object[] { this, hashtable });
		}
	}

	// Token: 0x0600051E RID: 1310 RVA: 0x000177E0 File Offset: 0x00015BE0
	public static PhotonPlayer Find(int ID)
	{
		if (PhotonNetwork.networkingPeer != null)
		{
			return PhotonNetwork.networkingPeer.GetPlayerWithId(ID);
		}
		return null;
	}

	// Token: 0x0600051F RID: 1311 RVA: 0x000177F9 File Offset: 0x00015BF9
	public PhotonPlayer Get(int id)
	{
		return PhotonPlayer.Find(id);
	}

	// Token: 0x06000520 RID: 1312 RVA: 0x00017801 File Offset: 0x00015C01
	public PhotonPlayer GetNext()
	{
		return this.GetNextFor(this.ID);
	}

	// Token: 0x06000521 RID: 1313 RVA: 0x0001780F File Offset: 0x00015C0F
	public PhotonPlayer GetNextFor(PhotonPlayer currentPlayer)
	{
		if (currentPlayer == null)
		{
			return null;
		}
		return this.GetNextFor(currentPlayer.ID);
	}

	// Token: 0x06000522 RID: 1314 RVA: 0x00017828 File Offset: 0x00015C28
	public PhotonPlayer GetNextFor(int currentPlayerId)
	{
		if (PhotonNetwork.networkingPeer == null || PhotonNetwork.networkingPeer.mActors == null || PhotonNetwork.networkingPeer.mActors.Count < 2)
		{
			return null;
		}
		Dictionary<int, PhotonPlayer> mActors = PhotonNetwork.networkingPeer.mActors;
		int num = int.MaxValue;
		int num2 = currentPlayerId;
		foreach (int num3 in mActors.Keys)
		{
			if (num3 < num2)
			{
				num2 = num3;
			}
			else if (num3 > currentPlayerId && num3 < num)
			{
				num = num3;
			}
		}
		return (num == int.MaxValue) ? mActors[num2] : mActors[num];
	}

	// Token: 0x06000523 RID: 1315 RVA: 0x00017900 File Offset: 0x00015D00
	public int CompareTo(PhotonPlayer other)
	{
		if (other == null)
		{
			return 0;
		}
		return this.GetHashCode().CompareTo(other.GetHashCode());
	}

	// Token: 0x06000524 RID: 1316 RVA: 0x0001792C File Offset: 0x00015D2C
	public int CompareTo(int other)
	{
		return this.GetHashCode().CompareTo(other);
	}

	// Token: 0x06000525 RID: 1317 RVA: 0x00017948 File Offset: 0x00015D48
	public bool Equals(PhotonPlayer other)
	{
		return other != null && this.GetHashCode().Equals(other.GetHashCode());
	}

	// Token: 0x06000526 RID: 1318 RVA: 0x00017974 File Offset: 0x00015D74
	public bool Equals(int other)
	{
		return this.GetHashCode().Equals(other);
	}

	// Token: 0x06000527 RID: 1319 RVA: 0x00017990 File Offset: 0x00015D90
	public override string ToString()
	{
		if (string.IsNullOrEmpty(this.NickName))
		{
			return string.Format("#{0:00}{1}{2}", this.ID, (!this.IsInactive) ? " " : " (inactive)", (!this.IsMasterClient) ? string.Empty : "(master)");
		}
		return string.Format("'{0}'{1}{2}", this.NickName, (!this.IsInactive) ? " " : " (inactive)", (!this.IsMasterClient) ? string.Empty : "(master)");
	}

	// Token: 0x06000528 RID: 1320 RVA: 0x00017A3C File Offset: 0x00015E3C
	public string ToStringFull()
	{
		return string.Format("#{0:00} '{1}'{2} {3}", new object[]
		{
			this.ID,
			this.NickName,
			(!this.IsInactive) ? string.Empty : " (inactive)",
			this.CustomProperties.ToStringFull()
		});
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06000529 RID: 1321 RVA: 0x00017A9B File Offset: 0x00015E9B
	// (set) Token: 0x0600052A RID: 1322 RVA: 0x00017AA3 File Offset: 0x00015EA3
	[Obsolete("Please use NickName (updated case for naming).")]
	public string name
	{
		get
		{
			return this.NickName;
		}
		set
		{
			this.NickName = value;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x0600052B RID: 1323 RVA: 0x00017AAC File Offset: 0x00015EAC
	// (set) Token: 0x0600052C RID: 1324 RVA: 0x00017AB4 File Offset: 0x00015EB4
	[Obsolete("Please use UserId (updated case for naming).")]
	public string userId
	{
		get
		{
			return this.UserId;
		}
		internal set
		{
			this.UserId = value;
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x0600052D RID: 1325 RVA: 0x00017ABD File Offset: 0x00015EBD
	[Obsolete("Please use IsLocal (updated case for naming).")]
	public bool isLocal
	{
		get
		{
			return this.IsLocal;
		}
	}

	// Token: 0x170000ED RID: 237
	// (get) Token: 0x0600052E RID: 1326 RVA: 0x00017AC5 File Offset: 0x00015EC5
	[Obsolete("Please use IsMasterClient (updated case for naming).")]
	public bool isMasterClient
	{
		get
		{
			return this.IsMasterClient;
		}
	}

	// Token: 0x170000EE RID: 238
	// (get) Token: 0x0600052F RID: 1327 RVA: 0x00017ACD File Offset: 0x00015ECD
	// (set) Token: 0x06000530 RID: 1328 RVA: 0x00017AD5 File Offset: 0x00015ED5
	[Obsolete("Please use IsInactive (updated case for naming).")]
	public bool isInactive
	{
		get
		{
			return this.IsInactive;
		}
		set
		{
			this.IsInactive = value;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06000531 RID: 1329 RVA: 0x00017ADE File Offset: 0x00015EDE
	// (set) Token: 0x06000532 RID: 1330 RVA: 0x00017AE6 File Offset: 0x00015EE6
	[Obsolete("Please use CustomProperties (updated case for naming).")]
	public Hashtable customProperties
	{
		get
		{
			return this.CustomProperties;
		}
		internal set
		{
			this.CustomProperties = value;
		}
	}

	// Token: 0x170000F0 RID: 240
	// (get) Token: 0x06000533 RID: 1331 RVA: 0x00017AEF File Offset: 0x00015EEF
	[Obsolete("Please use AllProperties (updated case for naming).")]
	public Hashtable allProperties
	{
		get
		{
			return this.AllProperties;
		}
	}

	// Token: 0x040003A1 RID: 929
	private int actorID = -1;

	// Token: 0x040003A2 RID: 930
	private string nameField = string.Empty;

	// Token: 0x040003A4 RID: 932
	public readonly bool IsLocal;

	// Token: 0x040003A7 RID: 935
	public object TagObject;
}
