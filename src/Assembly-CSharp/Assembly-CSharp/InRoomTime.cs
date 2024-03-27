using System;
using System.Collections;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x020000B7 RID: 183
public class InRoomTime : MonoBehaviour
{
	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000638 RID: 1592 RVA: 0x0001D044 File Offset: 0x0001B444
	public double RoomTime
	{
		get
		{
			uint roomTimestamp = (uint)this.RoomTimestamp;
			double num = roomTimestamp;
			return num / 1000.0;
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001D067 File Offset: 0x0001B467
	public int RoomTimestamp
	{
		get
		{
			return (!PhotonNetwork.inRoom) ? 0 : (PhotonNetwork.ServerTimestamp - this.roomStartTimestamp);
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x0600063A RID: 1594 RVA: 0x0001D085 File Offset: 0x0001B485
	public bool IsRoomTimeSet
	{
		get
		{
			return PhotonNetwork.inRoom && PhotonNetwork.room.CustomProperties.ContainsKey("#rt");
		}
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0001D0A8 File Offset: 0x0001B4A8
	internal IEnumerator SetRoomStartTimestamp()
	{
		if (this.IsRoomTimeSet || !PhotonNetwork.isMasterClient)
		{
			yield break;
		}
		if (PhotonNetwork.ServerTimestamp == 0)
		{
			yield return 0;
		}
		ExitGames.Client.Photon.Hashtable startTimeProp = new ExitGames.Client.Photon.Hashtable();
		startTimeProp["#rt"] = PhotonNetwork.ServerTimestamp;
		PhotonNetwork.room.SetCustomProperties(startTimeProp, null, false);
		yield break;
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0001D0C3 File Offset: 0x0001B4C3
	public void OnJoinedRoom()
	{
		base.StartCoroutine("SetRoomStartTimestamp");
	}

	// Token: 0x0600063D RID: 1597 RVA: 0x0001D0D1 File Offset: 0x0001B4D1
	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		base.StartCoroutine("SetRoomStartTimestamp");
	}

	// Token: 0x0600063E RID: 1598 RVA: 0x0001D0DF File Offset: 0x0001B4DF
	public void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("#rt"))
		{
			this.roomStartTimestamp = (int)propertiesThatChanged["#rt"];
		}
	}

	// Token: 0x040004A7 RID: 1191
	private int roomStartTimestamp;

	// Token: 0x040004A8 RID: 1192
	private const string StartTimeKey = "#rt";
}
