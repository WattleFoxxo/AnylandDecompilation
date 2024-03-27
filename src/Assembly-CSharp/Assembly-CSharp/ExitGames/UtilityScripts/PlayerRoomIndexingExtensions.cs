using System;
using UnityEngine;

namespace ExitGames.UtilityScripts
{
	// Token: 0x020000C2 RID: 194
	public static class PlayerRoomIndexingExtensions
	{
		// Token: 0x0600066E RID: 1646 RVA: 0x0001E22E File Offset: 0x0001C62E
		public static int GetRoomIndex(this PhotonPlayer player)
		{
			if (PlayerRoomIndexing.instance == null)
			{
				Debug.LogError("Missing PlayerRoomIndexing Component in Scene");
				return -1;
			}
			return PlayerRoomIndexing.instance.GetRoomIndex(player);
		}
	}
}
