using System;
using UnityEngine;

// Token: 0x020000A1 RID: 161
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody2D))]
[AddComponentMenu("Photon Networking/Photon Rigidbody 2D View")]
public class PhotonRigidbody2DView : MonoBehaviour, IPunObservable
{
	// Token: 0x060005DB RID: 1499 RVA: 0x0001B11F File Offset: 0x0001951F
	private void Awake()
	{
		this.m_Body = base.GetComponent<Rigidbody2D>();
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x0001B130 File Offset: 0x00019530
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
				this.m_Body.velocity = (Vector2)stream.ReceiveNext();
			}
			if (this.m_SynchronizeAngularVelocity)
			{
				this.m_Body.angularVelocity = (float)stream.ReceiveNext();
			}
		}
	}

	// Token: 0x0400042B RID: 1067
	[SerializeField]
	private bool m_SynchronizeVelocity = true;

	// Token: 0x0400042C RID: 1068
	[SerializeField]
	private bool m_SynchronizeAngularVelocity = true;

	// Token: 0x0400042D RID: 1069
	private Rigidbody2D m_Body;
}
