using System;
using System.Collections;
using Photon;
using UnityEngine;

// Token: 0x020000BC RID: 188
[RequireComponent(typeof(PhotonView))]
public class OnClickDestroy : global::Photon.MonoBehaviour
{
	// Token: 0x06000653 RID: 1619 RVA: 0x0001DB13 File Offset: 0x0001BF13
	public void OnClick()
	{
		if (!this.DestroyByRpc)
		{
			PhotonNetwork.Destroy(base.gameObject);
		}
		else
		{
			base.photonView.RPC("DestroyRpc", PhotonTargets.AllBuffered, new object[0]);
		}
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x0001DB48 File Offset: 0x0001BF48
	[PunRPC]
	public IEnumerator DestroyRpc()
	{
		global::UnityEngine.Object.Destroy(base.gameObject);
		yield return 0;
		PhotonNetwork.UnAllocateViewID(base.photonView.viewID);
		yield break;
	}

	// Token: 0x040004B8 RID: 1208
	public bool DestroyByRpc;
}
