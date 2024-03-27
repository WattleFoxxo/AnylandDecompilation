using System;
using ExitGames.Client.Photon;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class InRoomRoundTimer : MonoBehaviour
{
	// Token: 0x06000631 RID: 1585 RVA: 0x0001CE8C File Offset: 0x0001B28C
	private void StartRoundNow()
	{
		if (PhotonNetwork.time < 9.999999747378752E-05)
		{
			this.startRoundWhenTimeIsSynced = true;
			return;
		}
		this.startRoundWhenTimeIsSynced = false;
		Hashtable hashtable = new Hashtable();
		hashtable["st"] = PhotonNetwork.time;
		PhotonNetwork.room.SetCustomProperties(hashtable, null, false);
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x0001CEE3 File Offset: 0x0001B2E3
	public void OnJoinedRoom()
	{
		if (PhotonNetwork.isMasterClient)
		{
			this.StartRoundNow();
		}
		else
		{
			Debug.Log("StartTime already set: " + PhotonNetwork.room.CustomProperties.ContainsKey("st"));
		}
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x0001CF22 File Offset: 0x0001B322
	public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		if (propertiesThatChanged.ContainsKey("st"))
		{
			this.StartTime = (double)propertiesThatChanged["st"];
		}
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x0001CF4A File Offset: 0x0001B34A
	public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (!PhotonNetwork.room.CustomProperties.ContainsKey("st"))
		{
			Debug.Log("The new master starts a new round, cause we didn't start yet.");
			this.StartRoundNow();
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0001CF75 File Offset: 0x0001B375
	private void Update()
	{
		if (this.startRoundWhenTimeIsSynced)
		{
			this.StartRoundNow();
		}
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0001CF88 File Offset: 0x0001B388
	public void OnGUI()
	{
		double num = PhotonNetwork.time - this.StartTime;
		double num2 = (double)this.SecondsPerTurn - num % (double)this.SecondsPerTurn;
		int num3 = (int)(num / (double)this.SecondsPerTurn);
		GUILayout.BeginArea(this.TextPos);
		GUILayout.Label(string.Format("elapsed: {0:0.000}", num), new GUILayoutOption[0]);
		GUILayout.Label(string.Format("remaining: {0:0.000}", num2), new GUILayoutOption[0]);
		GUILayout.Label(string.Format("turn: {0:0}", num3), new GUILayoutOption[0]);
		if (GUILayout.Button("new round", new GUILayoutOption[0]))
		{
			this.StartRoundNow();
		}
		GUILayout.EndArea();
	}

	// Token: 0x040004A2 RID: 1186
	public int SecondsPerTurn = 5;

	// Token: 0x040004A3 RID: 1187
	public double StartTime;

	// Token: 0x040004A4 RID: 1188
	public Rect TextPos = new Rect(0f, 80f, 150f, 300f);

	// Token: 0x040004A5 RID: 1189
	private bool startRoundWhenTimeIsSynced;

	// Token: 0x040004A6 RID: 1190
	private const string StartTimeKey = "st";
}
