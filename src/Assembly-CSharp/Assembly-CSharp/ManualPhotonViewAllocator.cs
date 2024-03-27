using System;
using UnityEngine;

// Token: 0x020000B8 RID: 184
[RequireComponent(typeof(PhotonView))]
public class ManualPhotonViewAllocator : MonoBehaviour
{
	// Token: 0x06000640 RID: 1600 RVA: 0x0001D1F8 File Offset: 0x0001B5F8
	public void AllocateManualPhotonView()
	{
		PhotonView photonView = base.gameObject.GetPhotonView();
		if (photonView == null)
		{
			Debug.LogError("Can't do manual instantiation without PhotonView component.");
			return;
		}
		int num = PhotonNetwork.AllocateViewID();
		photonView.RPC("InstantiateRpc", PhotonTargets.AllBuffered, new object[] { num });
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0001D24C File Offset: 0x0001B64C
	[PunRPC]
	public void InstantiateRpc(int viewID)
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.Prefab, InputToEvent.inputHitPos + new Vector3(0f, 5f, 0f), Quaternion.identity);
		gameObject.GetPhotonView().viewID = viewID;
		OnClickDestroy component = gameObject.GetComponent<OnClickDestroy>();
		component.DestroyByRpc = true;
	}

	// Token: 0x040004A9 RID: 1193
	public GameObject Prefab;
}
