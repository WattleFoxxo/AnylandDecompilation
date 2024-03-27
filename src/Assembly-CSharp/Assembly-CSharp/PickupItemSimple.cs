using System;
using Photon;
using UnityEngine;

// Token: 0x020000C4 RID: 196
[RequireComponent(typeof(PhotonView))]
public class PickupItemSimple : global::Photon.MonoBehaviour
{
	// Token: 0x0600067C RID: 1660 RVA: 0x0001E5A4 File Offset: 0x0001C9A4
	public void OnTriggerEnter(Collider other)
	{
		PhotonView component = other.GetComponent<PhotonView>();
		if (this.PickupOnCollide && component != null && component.isMine)
		{
			this.Pickup();
		}
	}

	// Token: 0x0600067D RID: 1661 RVA: 0x0001E5E0 File Offset: 0x0001C9E0
	public void Pickup()
	{
		if (this.SentPickup)
		{
			return;
		}
		this.SentPickup = true;
		base.photonView.RPC("PunPickupSimple", PhotonTargets.AllViaServer, new object[0]);
	}

	// Token: 0x0600067E RID: 1662 RVA: 0x0001E60C File Offset: 0x0001CA0C
	[PunRPC]
	public void PunPickupSimple(PhotonMessageInfo msgInfo)
	{
		if (!this.SentPickup || !msgInfo.sender.IsLocal || base.gameObject.GetActive())
		{
		}
		this.SentPickup = false;
		if (!base.gameObject.GetActive())
		{
			Debug.Log("Ignored PU RPC, cause item is inactive. " + base.gameObject);
			return;
		}
		double num = PhotonNetwork.time - msgInfo.timestamp;
		float num2 = this.SecondsBeforeRespawn - (float)num;
		if (num2 > 0f)
		{
			base.gameObject.SetActive(false);
			base.Invoke("RespawnAfter", num2);
		}
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x0001E6B2 File Offset: 0x0001CAB2
	public void RespawnAfter()
	{
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(true);
		}
	}

	// Token: 0x040004CF RID: 1231
	public float SecondsBeforeRespawn = 2f;

	// Token: 0x040004D0 RID: 1232
	public bool PickupOnCollide;

	// Token: 0x040004D1 RID: 1233
	public bool SentPickup;
}
