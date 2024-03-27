using System;
using UnityEngine;

// Token: 0x020000AB RID: 171
public class PhotonTransformViewScaleControl
{
	// Token: 0x060005F9 RID: 1529 RVA: 0x0001BC53 File Offset: 0x0001A053
	public PhotonTransformViewScaleControl(PhotonTransformViewScaleModel model)
	{
		this.m_Model = model;
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x0001BC6D File Offset: 0x0001A06D
	public Vector3 GetNetworkScale()
	{
		return this.m_NetworkScale;
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x0001BC78 File Offset: 0x0001A078
	public Vector3 GetScale(Vector3 currentScale)
	{
		switch (this.m_Model.InterpolateOption)
		{
		default:
			return this.m_NetworkScale;
		case PhotonTransformViewScaleModel.InterpolateOptions.MoveTowards:
			return Vector3.MoveTowards(currentScale, this.m_NetworkScale, this.m_Model.InterpolateMoveTowardsSpeed * Time.deltaTime);
		case PhotonTransformViewScaleModel.InterpolateOptions.Lerp:
			return Vector3.Lerp(currentScale, this.m_NetworkScale, this.m_Model.InterpolateLerpSpeed * Time.deltaTime);
		}
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x0001BCEC File Offset: 0x0001A0EC
	public void OnPhotonSerializeView(Vector3 currentScale, PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.m_Model.SynchronizeEnabled)
		{
			return;
		}
		if (stream.isWriting)
		{
			stream.SendNext(currentScale);
			this.m_NetworkScale = currentScale;
		}
		else
		{
			this.m_NetworkScale = (Vector3)stream.ReceiveNext();
		}
	}

	// Token: 0x04000465 RID: 1125
	private PhotonTransformViewScaleModel m_Model;

	// Token: 0x04000466 RID: 1126
	private Vector3 m_NetworkScale = Vector3.one;
}
