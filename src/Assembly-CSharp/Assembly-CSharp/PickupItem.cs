using System;
using System.Collections.Generic;
using Photon;
using UnityEngine;

// Token: 0x020000C3 RID: 195
[RequireComponent(typeof(PhotonView))]
public class PickupItem : global::Photon.MonoBehaviour, IPunObservable
{
	// Token: 0x17000129 RID: 297
	// (get) Token: 0x06000670 RID: 1648 RVA: 0x0001E26A File Offset: 0x0001C66A
	public int ViewID
	{
		get
		{
			return base.photonView.viewID;
		}
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x0001E278 File Offset: 0x0001C678
	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnTrigger && component != null && component.isMine)
		{
			this.Pickup();
		}
	}

	// Token: 0x06000672 RID: 1650 RVA: 0x0001E2B4 File Offset: 0x0001C6B4
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting && this.SecondsBeforeRespawn <= 0f)
		{
			stream.SendNext(base.gameObject.transform.position);
		}
		else
		{
			Vector3 vector = (Vector3)stream.ReceiveNext();
			base.gameObject.transform.position = vector;
		}
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0001E319 File Offset: 0x0001C719
	public void Pickup()
	{
		if (this.SentPickup)
		{
			return;
		}
		this.SentPickup = true;
		base.photonView.RPC("PunPickup", PhotonTargets.AllViaServer, new object[0]);
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0001E345 File Offset: 0x0001C745
	public void Drop()
	{
		if (this.PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, new object[0]);
		}
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0001E369 File Offset: 0x0001C769
	public void Drop(Vector3 newPosition)
	{
		if (this.PickupIsMine)
		{
			base.photonView.RPC("PunRespawn", PhotonTargets.AllViaServer, new object[] { newPosition });
		}
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0001E398 File Offset: 0x0001C798
	[PunRPC]
	public void PunPickup(PhotonMessageInfo msgInfo)
	{
		if (msgInfo.sender.IsLocal)
		{
			this.SentPickup = false;
		}
		if (!base.gameObject.GetActive())
		{
			Debug.Log(string.Concat(new object[]
			{
				"Ignored PU RPC, cause item is inactive. ",
				base.gameObject,
				" SecondsBeforeRespawn: ",
				this.SecondsBeforeRespawn,
				" TimeOfRespawn: ",
				this.TimeOfRespawn,
				" respawn in future: ",
				this.TimeOfRespawn > PhotonNetwork.time
			}));
			return;
		}
		this.PickupIsMine = msgInfo.sender.IsLocal;
		if (this.OnPickedUpCall != null)
		{
			this.OnPickedUpCall.SendMessage("OnPickedUp", this);
		}
		if (this.SecondsBeforeRespawn <= 0f)
		{
			this.PickedUp(0f);
		}
		else
		{
			double num = PhotonNetwork.time - msgInfo.timestamp;
			double num2 = (double)this.SecondsBeforeRespawn - num;
			if (num2 > 0.0)
			{
				this.PickedUp((float)num2);
			}
		}
	}

	// Token: 0x06000677 RID: 1655 RVA: 0x0001E4BC File Offset: 0x0001C8BC
	internal void PickedUp(float timeUntilRespawn)
	{
		base.gameObject.SetActive(false);
		PickupItem.DisabledPickupItems.Add(this);
		this.TimeOfRespawn = 0.0;
		if (timeUntilRespawn > 0f)
		{
			this.TimeOfRespawn = PhotonNetwork.time + (double)timeUntilRespawn;
			base.Invoke("PunRespawn", timeUntilRespawn);
		}
	}

	// Token: 0x06000678 RID: 1656 RVA: 0x0001E515 File Offset: 0x0001C915
	[PunRPC]
	internal void PunRespawn(Vector3 pos)
	{
		Debug.Log("PunRespawn with Position.");
		this.PunRespawn();
		base.gameObject.transform.position = pos;
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0001E538 File Offset: 0x0001C938
	[PunRPC]
	internal void PunRespawn()
	{
		PickupItem.DisabledPickupItems.Remove(this);
		this.TimeOfRespawn = 0.0;
		this.PickupIsMine = false;
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x040004C8 RID: 1224
	public float SecondsBeforeRespawn = 2f;

	// Token: 0x040004C9 RID: 1225
	public bool PickupOnTrigger;

	// Token: 0x040004CA RID: 1226
	public bool PickupIsMine;

	// Token: 0x040004CB RID: 1227
	public global::UnityEngine.MonoBehaviour OnPickedUpCall;

	// Token: 0x040004CC RID: 1228
	public bool SentPickup;

	// Token: 0x040004CD RID: 1229
	public double TimeOfRespawn;

	// Token: 0x040004CE RID: 1230
	public static HashSet<PickupItem> DisabledPickupItems = new HashSet<PickupItem>();
}
