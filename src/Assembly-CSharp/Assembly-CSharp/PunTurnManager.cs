using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class PunTurnManager : PunBehaviour
{
	// Token: 0x1700012A RID: 298
	// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001EE6A File Offset: 0x0001D26A
	// (set) Token: 0x0600069B RID: 1691 RVA: 0x0001EE76 File Offset: 0x0001D276
	public int Turn
	{
		get
		{
			return PhotonNetwork.room.GetTurn();
		}
		private set
		{
			this._isOverCallProcessed = false;
			PhotonNetwork.room.SetTurn(value, true);
		}
	}

	// Token: 0x1700012B RID: 299
	// (get) Token: 0x0600069C RID: 1692 RVA: 0x0001EE8B File Offset: 0x0001D28B
	public float ElapsedTimeInTurn
	{
		get
		{
			return (float)(PhotonNetwork.ServerTimestamp - PhotonNetwork.room.GetTurnStart()) / 1000f;
		}
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x0600069D RID: 1693 RVA: 0x0001EEA4 File Offset: 0x0001D2A4
	public float RemainingSecondsInTurn
	{
		get
		{
			return Mathf.Max(0f, this.TurnDuration - this.ElapsedTimeInTurn);
		}
	}

	// Token: 0x1700012D RID: 301
	// (get) Token: 0x0600069E RID: 1694 RVA: 0x0001EEBD File Offset: 0x0001D2BD
	public bool IsCompletedByAll
	{
		get
		{
			return PhotonNetwork.room != null && this.Turn > 0 && this.finishedPlayers.Count == PhotonNetwork.room.PlayerCount;
		}
	}

	// Token: 0x1700012E RID: 302
	// (get) Token: 0x0600069F RID: 1695 RVA: 0x0001EEEF File Offset: 0x0001D2EF
	public bool IsFinishedByMe
	{
		get
		{
			return this.finishedPlayers.Contains(PhotonNetwork.player);
		}
	}

	// Token: 0x1700012F RID: 303
	// (get) Token: 0x060006A0 RID: 1696 RVA: 0x0001EF01 File Offset: 0x0001D301
	public bool IsOver
	{
		get
		{
			return this.RemainingSecondsInTurn <= 0f;
		}
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x0001EF13 File Offset: 0x0001D313
	private void Start()
	{
		PhotonNetwork.OnEventCall = new PhotonNetwork.EventCallback(this.OnEvent);
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x0001EF26 File Offset: 0x0001D326
	private void Update()
	{
		if (this.Turn > 0 && this.IsOver && !this._isOverCallProcessed)
		{
			this._isOverCallProcessed = true;
			this.TurnManagerListener.OnTurnTimeEnds(this.Turn);
		}
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0001EF62 File Offset: 0x0001D362
	public void BeginTurn()
	{
		this.Turn++;
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x0001EF74 File Offset: 0x0001D374
	public void SendMove(object move, bool finished)
	{
		if (this.IsFinishedByMe)
		{
			Debug.LogWarning("Can't SendMove. Turn is finished by this player.");
			return;
		}
		Hashtable hashtable = new Hashtable();
		hashtable.Add("turn", this.Turn);
		hashtable.Add("move", move);
		byte b = ((!finished) ? 1 : 2);
		PhotonNetwork.RaiseEvent(b, hashtable, true, new RaiseEventOptions
		{
			CachingOption = EventCaching.AddToRoomCache
		});
		if (finished)
		{
			PhotonNetwork.player.SetFinishedTurn(this.Turn);
		}
		this.OnEvent(b, hashtable, PhotonNetwork.player.ID);
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x0001F00C File Offset: 0x0001D40C
	public bool GetPlayerFinishedTurn(PhotonPlayer player)
	{
		return player != null && this.finishedPlayers != null && this.finishedPlayers.Contains(player);
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x0001F034 File Offset: 0x0001D434
	public void OnEvent(byte eventCode, object content, int senderId)
	{
		PhotonPlayer photonPlayer = PhotonPlayer.Find(senderId);
		if (eventCode != 1)
		{
			if (eventCode == 2)
			{
				Hashtable hashtable = content as Hashtable;
				int num = (int)hashtable["turn"];
				object obj = hashtable["move"];
				if (num == this.Turn)
				{
					this.finishedPlayers.Add(photonPlayer);
					this.TurnManagerListener.OnPlayerFinished(photonPlayer, num, obj);
				}
				if (this.IsCompletedByAll)
				{
					this.TurnManagerListener.OnTurnCompleted(this.Turn);
				}
			}
		}
		else
		{
			Hashtable hashtable2 = content as Hashtable;
			int num2 = (int)hashtable2["turn"];
			object obj2 = hashtable2["move"];
			this.TurnManagerListener.OnPlayerMove(photonPlayer, num2, obj2);
		}
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x0001F106 File Offset: 0x0001D506
	public override void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("Turn"))
		{
			this._isOverCallProcessed = false;
			this.finishedPlayers.Clear();
			this.TurnManagerListener.OnTurnBegins(this.Turn);
		}
	}

	// Token: 0x040004DB RID: 1243
	public float TurnDuration = 20f;

	// Token: 0x040004DC RID: 1244
	public IPunTurnManagerCallbacks TurnManagerListener;

	// Token: 0x040004DD RID: 1245
	private readonly HashSet<PhotonPlayer> finishedPlayers = new HashSet<PhotonPlayer>();

	// Token: 0x040004DE RID: 1246
	public const byte TurnManagerEventOffset = 0;

	// Token: 0x040004DF RID: 1247
	public const byte EvMove = 1;

	// Token: 0x040004E0 RID: 1248
	public const byte EvFinalMove = 2;

	// Token: 0x040004E1 RID: 1249
	private bool _isOverCallProcessed;
}
