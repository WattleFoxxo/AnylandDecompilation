using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C9 RID: 201
public class PunTeams : MonoBehaviour
{
	// Token: 0x0600068F RID: 1679 RVA: 0x0001EC48 File Offset: 0x0001D048
	public void Start()
	{
		PunTeams.PlayersPerTeam = new Dictionary<PunTeams.Team, List<PhotonPlayer>>();
		Array values = Enum.GetValues(typeof(PunTeams.Team));
		IEnumerator enumerator = values.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PunTeams.PlayersPerTeam[(PunTeams.Team)obj] = new List<PhotonPlayer>();
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x0001ECCC File Offset: 0x0001D0CC
	public void OnDisable()
	{
		PunTeams.PlayersPerTeam = new Dictionary<PunTeams.Team, List<PhotonPlayer>>();
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x0001ECD8 File Offset: 0x0001D0D8
	public void OnJoinedRoom()
	{
		this.UpdateTeams();
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x0001ECE0 File Offset: 0x0001D0E0
	public void OnLeftRoom()
	{
		this.Start();
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x0001ECE8 File Offset: 0x0001D0E8
	public void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
	{
		this.UpdateTeams();
	}

	// Token: 0x06000694 RID: 1684 RVA: 0x0001ECF0 File Offset: 0x0001D0F0
	public void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		this.UpdateTeams();
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0001ECF8 File Offset: 0x0001D0F8
	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		this.UpdateTeams();
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x0001ED00 File Offset: 0x0001D100
	public void UpdateTeams()
	{
		Array values = Enum.GetValues(typeof(PunTeams.Team));
		IEnumerator enumerator = values.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				PunTeams.PlayersPerTeam[(PunTeams.Team)obj].Clear();
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
		{
			PhotonPlayer photonPlayer = PhotonNetwork.playerList[i];
			PunTeams.Team team = photonPlayer.GetTeam();
			PunTeams.PlayersPerTeam[team].Add(photonPlayer);
		}
	}

	// Token: 0x040004D5 RID: 1237
	public static Dictionary<PunTeams.Team, List<PhotonPlayer>> PlayersPerTeam;

	// Token: 0x040004D6 RID: 1238
	public const string TeamPlayerProp = "team";

	// Token: 0x020000CA RID: 202
	public enum Team : byte
	{
		// Token: 0x040004D8 RID: 1240
		none,
		// Token: 0x040004D9 RID: 1241
		red,
		// Token: 0x040004DA RID: 1242
		blue
	}
}
