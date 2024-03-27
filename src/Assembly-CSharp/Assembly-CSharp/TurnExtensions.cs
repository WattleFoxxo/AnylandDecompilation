using System;
using ExitGames.Client.Photon;

// Token: 0x020000CE RID: 206
public static class TurnExtensions
{
	// Token: 0x060006AD RID: 1709 RVA: 0x0001F13C File Offset: 0x0001D53C
	public static void SetTurn(this Room room, int turn, bool setStartTime = false)
	{
		if (room == null || room.CustomProperties == null)
		{
			return;
		}
		Hashtable hashtable = new Hashtable();
		hashtable[TurnExtensions.TurnPropKey] = turn;
		if (setStartTime)
		{
			hashtable[TurnExtensions.TurnStartPropKey] = PhotonNetwork.ServerTimestamp;
		}
		room.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x0001F196 File Offset: 0x0001D596
	public static int GetTurn(this RoomInfo room)
	{
		if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnExtensions.TurnPropKey))
		{
			return 0;
		}
		return (int)room.CustomProperties[TurnExtensions.TurnPropKey];
	}

	// Token: 0x060006AF RID: 1711 RVA: 0x0001F1D5 File Offset: 0x0001D5D5
	public static int GetTurnStart(this RoomInfo room)
	{
		if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnExtensions.TurnStartPropKey))
		{
			return 0;
		}
		return (int)room.CustomProperties[TurnExtensions.TurnStartPropKey];
	}

	// Token: 0x060006B0 RID: 1712 RVA: 0x0001F214 File Offset: 0x0001D614
	public static int GetFinishedTurn(this PhotonPlayer player)
	{
		Room room = PhotonNetwork.room;
		if (room == null || room.CustomProperties == null || !room.CustomProperties.ContainsKey(TurnExtensions.TurnPropKey))
		{
			return 0;
		}
		string text = TurnExtensions.FinishedTurnPropKey + player.ID;
		return (int)room.CustomProperties[text];
	}

	// Token: 0x060006B1 RID: 1713 RVA: 0x0001F278 File Offset: 0x0001D678
	public static void SetFinishedTurn(this PhotonPlayer player, int turn)
	{
		Room room = PhotonNetwork.room;
		if (room == null || room.CustomProperties == null)
		{
			return;
		}
		string text = TurnExtensions.FinishedTurnPropKey + player.ID;
		Hashtable hashtable = new Hashtable();
		hashtable[text] = turn;
		room.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x040004E2 RID: 1250
	public static readonly string TurnPropKey = "Turn";

	// Token: 0x040004E3 RID: 1251
	public static readonly string TurnStartPropKey = "TStart";

	// Token: 0x040004E4 RID: 1252
	public static readonly string FinishedTurnPropKey = "FToA";
}
