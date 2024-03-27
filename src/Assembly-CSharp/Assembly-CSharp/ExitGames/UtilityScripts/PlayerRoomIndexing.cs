using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

namespace ExitGames.UtilityScripts
{
	// Token: 0x020000C0 RID: 192
	public class PlayerRoomIndexing : PunBehaviour
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x0001DE21 File Offset: 0x0001C221
		public int[] PlayerIds
		{
			get
			{
				return this._playerIds;
			}
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x0001DE29 File Offset: 0x0001C229
		public void Awake()
		{
			if (PlayerRoomIndexing.instance != null)
			{
				Debug.LogError("Existing instance of PlayerRoomIndexing found. Only One instance is required at the most. Please correct and have only one at any time.");
			}
			PlayerRoomIndexing.instance = this;
			if (PhotonNetwork.room != null)
			{
				this.SanitizeIndexing(true);
			}
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x0001DE5C File Offset: 0x0001C25C
		public override void OnJoinedRoom()
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.AssignIndex(PhotonNetwork.player);
			}
			else
			{
				this.RefreshData();
			}
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x0001DE7E File Offset: 0x0001C27E
		public override void OnLeftRoom()
		{
			this.RefreshData();
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x0001DE86 File Offset: 0x0001C286
		public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.AssignIndex(newPlayer);
			}
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x0001DE99 File Offset: 0x0001C299
		public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.UnAssignIndex(otherPlayer);
			}
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0001DEAC File Offset: 0x0001C2AC
		public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
		{
			if (propertiesThatChanged.ContainsKey("PlayerIndexes"))
			{
				this.RefreshData();
			}
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001DEC4 File Offset: 0x0001C2C4
		public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
		{
			if (PhotonNetwork.isMasterClient)
			{
				this.SanitizeIndexing(false);
			}
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x0001DED7 File Offset: 0x0001C2D7
		public int GetRoomIndex(PhotonPlayer player)
		{
			if (this._indexesLUT != null && this._indexesLUT.ContainsKey(player.ID))
			{
				return this._indexesLUT[player.ID];
			}
			return -1;
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x0001DF10 File Offset: 0x0001C310
		private void SanitizeIndexing(bool forceIndexing = false)
		{
			if (!forceIndexing && !PhotonNetwork.isMasterClient)
			{
				return;
			}
			if (PhotonNetwork.room == null)
			{
				return;
			}
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			if (PhotonNetwork.room.CustomProperties.TryGetValue("PlayerIndexes", out this._indexes))
			{
				dictionary = this._indexes as Dictionary<int, int>;
			}
			if (dictionary.Count != PhotonNetwork.room.PlayerCount)
			{
				foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
				{
					if (!dictionary.ContainsKey(photonPlayer.ID))
					{
						this.AssignIndex(photonPlayer);
					}
				}
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x0001DFB8 File Offset: 0x0001C3B8
		private void RefreshData()
		{
			if (PhotonNetwork.room != null)
			{
				this._playerIds = new int[PhotonNetwork.room.MaxPlayers];
				if (PhotonNetwork.room.CustomProperties.TryGetValue("PlayerIndexes", out this._indexes))
				{
					this._indexesLUT = this._indexes as Dictionary<int, int>;
					foreach (KeyValuePair<int, int> keyValuePair in this._indexesLUT)
					{
						this._p = PhotonPlayer.Find(keyValuePair.Key);
						this._playerIds[keyValuePair.Value] = this._p.ID;
					}
				}
			}
			else
			{
				this._playerIds = new int[0];
			}
			if (this.OnRoomIndexingChanged != null)
			{
				this.OnRoomIndexingChanged();
			}
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x0001E0B0 File Offset: 0x0001C4B0
		private void AssignIndex(PhotonPlayer player)
		{
			if (PhotonNetwork.room.CustomProperties.TryGetValue("PlayerIndexes", out this._indexes))
			{
				this._indexesLUT = this._indexes as Dictionary<int, int>;
			}
			else
			{
				this._indexesLUT = new Dictionary<int, int>();
			}
			List<bool> list = new List<bool>(new bool[PhotonNetwork.room.MaxPlayers]);
			foreach (KeyValuePair<int, int> keyValuePair in this._indexesLUT)
			{
				list[keyValuePair.Value] = true;
			}
			this._indexesLUT[player.ID] = Mathf.Max(0, list.IndexOf(false));
			PhotonNetwork.room.SetCustomProperties(new Hashtable { { "PlayerIndexes", this._indexesLUT } }, null, false);
			this.RefreshData();
		}

		// Token: 0x06000669 RID: 1641 RVA: 0x0001E1B0 File Offset: 0x0001C5B0
		private void UnAssignIndex(PhotonPlayer player)
		{
			if (PhotonNetwork.room.CustomProperties.TryGetValue("PlayerIndexes", out this._indexes))
			{
				this._indexesLUT = this._indexes as Dictionary<int, int>;
				this._indexesLUT.Remove(player.ID);
				PhotonNetwork.room.SetCustomProperties(new Hashtable { { "PlayerIndexes", this._indexesLUT } }, null, false);
			}
			this.RefreshData();
		}

		// Token: 0x040004C0 RID: 1216
		public static PlayerRoomIndexing instance;

		// Token: 0x040004C1 RID: 1217
		public PlayerRoomIndexing.RoomIndexingChanged OnRoomIndexingChanged;

		// Token: 0x040004C2 RID: 1218
		public const string RoomPlayerIndexedProp = "PlayerIndexes";

		// Token: 0x040004C3 RID: 1219
		private int[] _playerIds;

		// Token: 0x040004C4 RID: 1220
		private object _indexes;

		// Token: 0x040004C5 RID: 1221
		private Dictionary<int, int> _indexesLUT;

		// Token: 0x040004C6 RID: 1222
		private List<bool> _indexesPool;

		// Token: 0x040004C7 RID: 1223
		private PhotonPlayer _p;

		// Token: 0x020000C1 RID: 193
		// (Invoke) Token: 0x0600066B RID: 1643
		public delegate void RoomIndexingChanged();
	}
}
