using System;
using Photon;
using UnityEngine;

// Token: 0x020000BB RID: 187
[RequireComponent(typeof(PhotonView))]
public class OnAwakeUsePhotonView : global::Photon.MonoBehaviour
{
	// Token: 0x0600064E RID: 1614 RVA: 0x0001DA63 File Offset: 0x0001BE63
	private void Awake()
	{
		if (!base.photonView.isMine)
		{
			return;
		}
		base.photonView.RPC("OnAwakeRPC", PhotonTargets.All, new object[0]);
	}

	// Token: 0x0600064F RID: 1615 RVA: 0x0001DA8D File Offset: 0x0001BE8D
	private void Start()
	{
		if (!base.photonView.isMine)
		{
			return;
		}
		base.photonView.RPC("OnAwakeRPC", PhotonTargets.All, new object[] { 1 });
	}

	// Token: 0x06000650 RID: 1616 RVA: 0x0001DAC0 File Offset: 0x0001BEC0
	[PunRPC]
	public void OnAwakeRPC()
	{
		Debug.Log("RPC: 'OnAwakeRPC' PhotonView: " + base.photonView);
	}

	// Token: 0x06000651 RID: 1617 RVA: 0x0001DAD7 File Offset: 0x0001BED7
	[PunRPC]
	public void OnAwakeRPC(byte myParameter)
	{
		Debug.Log(string.Concat(new object[] { "RPC: 'OnAwakeRPC' Parameter: ", myParameter, " PhotonView: ", base.photonView }));
	}
}
