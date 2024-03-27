using System;
using Photon;
using UnityEngine;

// Token: 0x020000B3 RID: 179
[RequireComponent(typeof(PhotonView))]
public class HighlightOwnedGameObj : global::Photon.MonoBehaviour
{
	// Token: 0x06000620 RID: 1568 RVA: 0x0001C850 File Offset: 0x0001AC50
	private void Update()
	{
		if (base.photonView.isMine)
		{
			if (this.markerTransform == null)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.PointerPrefab);
				gameObject.transform.parent = base.gameObject.transform;
				this.markerTransform = gameObject.transform;
			}
			Vector3 position = base.gameObject.transform.position;
			this.markerTransform.position = new Vector3(position.x, position.y + this.Offset, position.z);
			this.markerTransform.rotation = Quaternion.identity;
		}
		else if (this.markerTransform != null)
		{
			global::UnityEngine.Object.Destroy(this.markerTransform.gameObject);
			this.markerTransform = null;
		}
	}

	// Token: 0x04000490 RID: 1168
	public GameObject PointerPrefab;

	// Token: 0x04000491 RID: 1169
	public float Offset = 0.5f;

	// Token: 0x04000492 RID: 1170
	private Transform markerTransform;
}
