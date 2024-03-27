using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x020000C5 RID: 197
[RequireComponent(typeof(PhotonView))]
public class PickupItemSyncer : global::Photon.MonoBehaviour
{
	// Token: 0x06000681 RID: 1665 RVA: 0x0001E6D9 File Offset: 0x0001CAD9
	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.SendPickedUpItems(newPlayer);
		}
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x0001E6EC File Offset: 0x0001CAEC
	public void OnJoinedRoom()
	{
		Debug.Log(string.Concat(new object[]
		{
			"Joined Room. isMasterClient: ",
			PhotonNetwork.isMasterClient,
			" id: ",
			PhotonNetwork.player.ID
		}));
		this.IsWaitingForPickupInit = !PhotonNetwork.isMasterClient;
		if (PhotonNetwork.playerList.Length >= 2)
		{
			base.Invoke("AskForPickupItemSpawnTimes", 2f);
		}
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x0001E764 File Offset: 0x0001CB64
	public void AskForPickupItemSpawnTimes()
	{
		if (this.IsWaitingForPickupInit)
		{
			if (PhotonNetwork.playerList.Length < 2)
			{
				Debug.Log("Cant ask anyone else for PickupItem spawn times.");
				this.IsWaitingForPickupInit = false;
				return;
			}
			PhotonPlayer photonPlayer = PhotonNetwork.masterClient.GetNext();
			if (photonPlayer == null || photonPlayer.Equals(PhotonNetwork.player))
			{
				photonPlayer = PhotonNetwork.player.GetNext();
			}
			if (photonPlayer != null && !photonPlayer.Equals(PhotonNetwork.player))
			{
				base.photonView.RPC("RequestForPickupItems", photonPlayer, new object[0]);
			}
			else
			{
				Debug.Log("No player left to ask");
				this.IsWaitingForPickupInit = false;
			}
		}
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x0001E80A File Offset: 0x0001CC0A
	[PunRPC]
	[Obsolete("Use RequestForPickupItems(PhotonMessageInfo msgInfo) with corrected typing instead.")]
	public void RequestForPickupTimes(PhotonMessageInfo msgInfo)
	{
		this.RequestForPickupItems(msgInfo);
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x0001E813 File Offset: 0x0001CC13
	[PunRPC]
	public void RequestForPickupItems(PhotonMessageInfo msgInfo)
	{
		if (msgInfo.sender == null)
		{
			Debug.LogError("Unknown player asked for PickupItems");
			return;
		}
		this.SendPickedUpItems(msgInfo.sender);
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0001E83C File Offset: 0x0001CC3C
	private void SendPickedUpItems(PhotonPlayer targetPlayer)
	{
		if (targetPlayer == null)
		{
			Debug.LogWarning("Cant send PickupItem spawn times to unknown targetPlayer.");
			return;
		}
		double time = PhotonNetwork.time;
		double num = time + 0.20000000298023224;
		PickupItem[] array = new PickupItem[PickupItem.DisabledPickupItems.Count];
		PickupItem.DisabledPickupItems.CopyTo(array);
		List<float> list = new List<float>(array.Length * 2);
		foreach (PickupItem pickupItem in array)
		{
			if (pickupItem.SecondsBeforeRespawn <= 0f)
			{
				list.Add((float)pickupItem.ViewID);
				list.Add(0f);
			}
			else
			{
				double num2 = pickupItem.TimeOfRespawn - PhotonNetwork.time;
				if (pickupItem.TimeOfRespawn > num)
				{
					Debug.Log(string.Concat(new object[]
					{
						pickupItem.ViewID,
						" respawn: ",
						pickupItem.TimeOfRespawn,
						" timeUntilRespawn: ",
						num2,
						" (now: ",
						PhotonNetwork.time,
						")"
					}));
					list.Add((float)pickupItem.ViewID);
					list.Add((float)num2);
				}
			}
		}
		Debug.Log(string.Concat(new object[] { "Sent count: ", list.Count, " now: ", time }));
		base.photonView.RPC("PickupItemInit", targetPlayer, new object[]
		{
			PhotonNetwork.time,
			list.ToArray()
		});
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x0001E9DC File Offset: 0x0001CDDC
	[PunRPC]
	public void PickupItemInit(double timeBase, float[] inactivePickupsAndTimes)
	{
		this.IsWaitingForPickupInit = false;
		for (int i = 0; i < inactivePickupsAndTimes.Length / 2; i++)
		{
			int num = i * 2;
			int num2 = (int)inactivePickupsAndTimes[num];
			float num3 = inactivePickupsAndTimes[num + 1];
			PhotonView photonView = PhotonView.Find(num2);
			PickupItem component = photonView.GetComponent<PickupItem>();
			if (num3 <= 0f)
			{
				component.PickedUp(0f);
			}
			else
			{
				double num4 = (double)num3 + timeBase;
				Debug.Log(string.Concat(new object[] { photonView.viewID, " respawn: ", num4, " timeUntilRespawnBasedOnTimeBase:", num3, " SecondsBeforeRespawn: ", component.SecondsBeforeRespawn }));
				double num5 = num4 - PhotonNetwork.time;
				if (num3 <= 0f)
				{
					num5 = 0.0;
				}
				component.PickedUp((float)num5);
			}
		}
	}

	// Token: 0x040004D2 RID: 1234
	public bool IsWaitingForPickupInit;

	// Token: 0x040004D3 RID: 1235
	private const float TimeDeltaToIgnore = 0.2f;
}
