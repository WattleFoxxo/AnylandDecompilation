using System;
using UnityEngine;

// Token: 0x020000A8 RID: 168
public class PhotonTransformViewRotationControl
{
	// Token: 0x060005F4 RID: 1524 RVA: 0x0001BB51 File Offset: 0x00019F51
	public PhotonTransformViewRotationControl(PhotonTransformViewRotationModel model)
	{
		this.m_Model = model;
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0001BB60 File Offset: 0x00019F60
	public Quaternion GetNetworkRotation()
	{
		return this.m_NetworkRotation;
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0001BB68 File Offset: 0x00019F68
	public Quaternion GetRotation(Quaternion currentRotation)
	{
		switch (this.m_Model.InterpolateOption)
		{
		default:
			return this.m_NetworkRotation;
		case PhotonTransformViewRotationModel.InterpolateOptions.RotateTowards:
			return Quaternion.RotateTowards(currentRotation, this.m_NetworkRotation, this.m_Model.InterpolateRotateTowardsSpeed * Time.deltaTime);
		case PhotonTransformViewRotationModel.InterpolateOptions.Lerp:
			return Quaternion.Lerp(currentRotation, this.m_NetworkRotation, this.m_Model.InterpolateLerpSpeed * Time.deltaTime);
		}
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0001BBDC File Offset: 0x00019FDC
	public void OnPhotonSerializeView(Quaternion currentRotation, PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.m_Model.SynchronizeEnabled)
		{
			return;
		}
		if (stream.isWriting)
		{
			stream.SendNext(currentRotation);
			this.m_NetworkRotation = currentRotation;
		}
		else
		{
			this.m_NetworkRotation = (Quaternion)stream.ReceiveNext();
		}
	}

	// Token: 0x0400045B RID: 1115
	private PhotonTransformViewRotationModel m_Model;

	// Token: 0x0400045C RID: 1116
	private Quaternion m_NetworkRotation;
}
