using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x020000CB RID: 203
public static class TeamExtensions
{
	// Token: 0x06000697 RID: 1687 RVA: 0x0001EDBC File Offset: 0x0001D1BC
	public static PunTeams.Team GetTeam(this PhotonPlayer player)
	{
		object obj;
		if (player.CustomProperties.TryGetValue("team", out obj))
		{
			return (PunTeams.Team)obj;
		}
		return PunTeams.Team.none;
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x0001EDE8 File Offset: 0x0001D1E8
	public static void SetTeam(this PhotonPlayer player, PunTeams.Team team)
	{
		if (!PhotonNetwork.connectedAndReady)
		{
			Debug.LogWarning("JoinTeam was called in state: " + PhotonNetwork.connectionStateDetailed + ". Not connectedAndReady.");
			return;
		}
		PunTeams.Team team2 = player.GetTeam();
		if (team2 != team)
		{
			player.SetCustomProperties(new Hashtable { 
			{
				"team",
				(byte)team
			} }, null, false);
		}
	}
}
