using System;
using UnityEngine;

// Token: 0x020000A2 RID: 162
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
[AddComponentMenu("Photon Networking/Photon Rigidbody View")]
public class PhotonRigidbodyView : MonoBehaviour, IPunObservable
{
	// Token: 0x060005DE RID: 1502 RVA: 0x0001B1E7 File Offset: 0x000195E7
	private void Awake()
	{
		this.m_Body = base.GetComponent<Rigidbody>();
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x0001B1F8 File Offset: 0x000195F8
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (this.m_SynchronizeVelocity)
			{
				stream.SendNext(this.m_Body.velocity);
			}
			if (this.m_SynchronizeAngularVelocity)
			{
				stream.SendNext(this.m_Body.angularVelocity);
			}
		}
		else
		{
			if (this.m_SynchronizeVelocity)
			{
				this.m_Body.velocity = (Vector3)stream.ReceiveNext();
			}
			if (this.m_SynchronizeAngularVelocity)
			{
				this.m_Body.angularVelocity = (Vector3)stream.ReceiveNext();
			}
		}
	}

	// Token: 0x0400042E RID: 1070
	[SerializeField]
	private bool m_SynchronizeVelocity = true;

	// Token: 0x0400042F RID: 1071
	[SerializeField]
	private bool m_SynchronizeAngularVelocity = true;

	// Token: 0x04000430 RID: 1072
	private Rigidbody m_Body;
}
