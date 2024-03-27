using System;
using UnityEngine;

// Token: 0x020000A3 RID: 163
[RequireComponent(typeof(PhotonView))]
[AddComponentMenu("Photon Networking/Photon Transform View")]
public class PhotonTransformView : MonoBehaviour, IPunObservable
{
	// Token: 0x060005E1 RID: 1505 RVA: 0x0001B2C4 File Offset: 0x000196C4
	private void Awake()
	{
		this.m_PhotonView = base.GetComponent<PhotonView>();
		this.m_PositionControl = new PhotonTransformViewPositionControl(this.m_PositionModel);
		this.m_RotationControl = new PhotonTransformViewRotationControl(this.m_RotationModel);
		this.m_ScaleControl = new PhotonTransformViewScaleControl(this.m_ScaleModel);
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x0001B310 File Offset: 0x00019710
	private void OnEnable()
	{
		this.m_firstTake = true;
	}

	// Token: 0x060005E3 RID: 1507 RVA: 0x0001B319 File Offset: 0x00019719
	private void Update()
	{
		if (this.m_PhotonView == null || this.m_PhotonView.isMine || !PhotonNetwork.connected)
		{
			return;
		}
		this.UpdatePosition();
		this.UpdateRotation();
		this.UpdateScale();
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0001B359 File Offset: 0x00019759
	private void UpdatePosition()
	{
		if (!this.m_PositionModel.SynchronizeEnabled || !this.m_ReceivedNetworkUpdate)
		{
			return;
		}
		base.transform.localPosition = this.m_PositionControl.UpdatePosition(base.transform.localPosition);
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0001B398 File Offset: 0x00019798
	private void UpdateRotation()
	{
		if (!this.m_RotationModel.SynchronizeEnabled || !this.m_ReceivedNetworkUpdate)
		{
			return;
		}
		base.transform.localRotation = this.m_RotationControl.GetRotation(base.transform.localRotation);
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0001B3D7 File Offset: 0x000197D7
	private void UpdateScale()
	{
		if (!this.m_ScaleModel.SynchronizeEnabled || !this.m_ReceivedNetworkUpdate)
		{
			return;
		}
		base.transform.localScale = this.m_ScaleControl.GetScale(base.transform.localScale);
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0001B416 File Offset: 0x00019816
	public void SetSynchronizedValues(Vector3 speed, float turnSpeed)
	{
		this.m_PositionControl.SetSynchronizedValues(speed, turnSpeed);
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0001B428 File Offset: 0x00019828
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		this.m_PositionControl.OnPhotonSerializeView(base.transform.localPosition, stream, info);
		this.m_RotationControl.OnPhotonSerializeView(base.transform.localRotation, stream, info);
		this.m_ScaleControl.OnPhotonSerializeView(base.transform.localScale, stream, info);
		if (!this.m_PhotonView.isMine && this.m_PositionModel.DrawErrorGizmo)
		{
			this.DoDrawEstimatedPositionError();
		}
		if (stream.isReading)
		{
			this.m_ReceivedNetworkUpdate = true;
			if (this.m_firstTake)
			{
				this.m_firstTake = false;
				if (this.m_PositionModel.SynchronizeEnabled)
				{
					base.transform.localPosition = this.m_PositionControl.GetNetworkPosition();
				}
				if (this.m_RotationModel.SynchronizeEnabled)
				{
					base.transform.localRotation = this.m_RotationControl.GetNetworkRotation();
				}
				if (this.m_ScaleModel.SynchronizeEnabled)
				{
					base.transform.localScale = this.m_ScaleControl.GetNetworkScale();
				}
			}
		}
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0001B53C File Offset: 0x0001993C
	private void DoDrawEstimatedPositionError()
	{
		Vector3 vector = this.m_PositionControl.GetNetworkPosition();
		if (base.transform.parent != null)
		{
			vector = base.transform.parent.position + vector;
		}
		Debug.DrawLine(vector, base.transform.position, Color.red, 2f);
		Debug.DrawLine(base.transform.position, base.transform.position + Vector3.up, Color.green, 2f);
		Debug.DrawLine(vector, vector + Vector3.up, Color.red, 2f);
	}

	// Token: 0x04000431 RID: 1073
	[SerializeField]
	private PhotonTransformViewPositionModel m_PositionModel = new PhotonTransformViewPositionModel();

	// Token: 0x04000432 RID: 1074
	[SerializeField]
	private PhotonTransformViewRotationModel m_RotationModel = new PhotonTransformViewRotationModel();

	// Token: 0x04000433 RID: 1075
	[SerializeField]
	private PhotonTransformViewScaleModel m_ScaleModel = new PhotonTransformViewScaleModel();

	// Token: 0x04000434 RID: 1076
	private PhotonTransformViewPositionControl m_PositionControl;

	// Token: 0x04000435 RID: 1077
	private PhotonTransformViewRotationControl m_RotationControl;

	// Token: 0x04000436 RID: 1078
	private PhotonTransformViewScaleControl m_ScaleControl;

	// Token: 0x04000437 RID: 1079
	private PhotonView m_PhotonView;

	// Token: 0x04000438 RID: 1080
	private bool m_ReceivedNetworkUpdate;

	// Token: 0x04000439 RID: 1081
	private bool m_firstTake;
}
