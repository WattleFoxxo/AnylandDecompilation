using System;
using Photon;
using UnityEngine;

// Token: 0x020000D3 RID: 211
[RequireComponent(typeof(PhotonView))]
public class SmoothSyncMovement : global::Photon.MonoBehaviour, IPunObservable
{
	// Token: 0x060006BE RID: 1726 RVA: 0x0001F6B8 File Offset: 0x0001DAB8
	public void Awake()
	{
		bool flag = false;
		foreach (Component component in base.photonView.ObservedComponents)
		{
			if (component == this)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Debug.LogWarning(this + " is not observed by this object's photonView! OnPhotonSerializeView() in this class won't be used.");
		}
	}

	// Token: 0x060006BF RID: 1727 RVA: 0x0001F740 File Offset: 0x0001DB40
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(base.transform.position);
			stream.SendNext(base.transform.rotation);
		}
		else
		{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
	}

	// Token: 0x060006C0 RID: 1728 RVA: 0x0001F7AC File Offset: 0x0001DBAC
	public void Update()
	{
		if (!base.photonView.isMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, this.correctPlayerPos, Time.deltaTime * this.SmoothingDelay);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.correctPlayerRot, Time.deltaTime * this.SmoothingDelay);
		}
	}

	// Token: 0x040004EB RID: 1259
	public float SmoothingDelay = 5f;

	// Token: 0x040004EC RID: 1260
	private Vector3 correctPlayerPos = Vector3.zero;

	// Token: 0x040004ED RID: 1261
	private Quaternion correctPlayerRot = Quaternion.identity;
}
