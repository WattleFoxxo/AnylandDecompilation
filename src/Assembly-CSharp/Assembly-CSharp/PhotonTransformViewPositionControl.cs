using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A4 RID: 164
public class PhotonTransformViewPositionControl
{
	// Token: 0x060005EA RID: 1514 RVA: 0x0001B5E7 File Offset: 0x000199E7
	public PhotonTransformViewPositionControl(PhotonTransformViewPositionModel model)
	{
		this.m_Model = model;
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0001B614 File Offset: 0x00019A14
	private Vector3 GetOldestStoredNetworkPosition()
	{
		Vector3 vector = this.m_NetworkPosition;
		if (this.m_OldNetworkPositions.Count > 0)
		{
			vector = this.m_OldNetworkPositions.Peek();
		}
		return vector;
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0001B646 File Offset: 0x00019A46
	public void SetSynchronizedValues(Vector3 speed, float turnSpeed)
	{
		this.m_SynchronizedSpeed = speed;
		this.m_SynchronizedTurnSpeed = turnSpeed;
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0001B658 File Offset: 0x00019A58
	public Vector3 UpdatePosition(Vector3 currentPosition)
	{
		Vector3 vector = this.GetNetworkPosition() + this.GetExtrapolatedPositionOffset();
		switch (this.m_Model.InterpolateOption)
		{
		case PhotonTransformViewPositionModel.InterpolateOptions.Disabled:
			if (!this.m_UpdatedPositionAfterOnSerialize)
			{
				currentPosition = vector;
				this.m_UpdatedPositionAfterOnSerialize = true;
			}
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.FixedSpeed:
			currentPosition = Vector3.MoveTowards(currentPosition, vector, Time.deltaTime * this.m_Model.InterpolateMoveTowardsSpeed);
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.EstimatedSpeed:
			if (this.m_OldNetworkPositions.Count != 0)
			{
				float num = Vector3.Distance(this.m_NetworkPosition, this.GetOldestStoredNetworkPosition()) / (float)this.m_OldNetworkPositions.Count * (float)PhotonNetwork.sendRateOnSerialize;
				currentPosition = Vector3.MoveTowards(currentPosition, vector, Time.deltaTime * num);
			}
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues:
			if (this.m_SynchronizedSpeed.magnitude == 0f)
			{
				currentPosition = vector;
			}
			else
			{
				currentPosition = Vector3.MoveTowards(currentPosition, vector, Time.deltaTime * this.m_SynchronizedSpeed.magnitude);
			}
			break;
		case PhotonTransformViewPositionModel.InterpolateOptions.Lerp:
			currentPosition = Vector3.Lerp(currentPosition, vector, Time.deltaTime * this.m_Model.InterpolateLerpSpeed);
			break;
		}
		if (this.m_Model.TeleportEnabled && Vector3.Distance(currentPosition, this.GetNetworkPosition()) > this.m_Model.TeleportIfDistanceGreaterThan)
		{
			currentPosition = this.GetNetworkPosition();
		}
		return currentPosition;
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0001B7BB File Offset: 0x00019BBB
	public Vector3 GetNetworkPosition()
	{
		return this.m_NetworkPosition;
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0001B7C4 File Offset: 0x00019BC4
	public Vector3 GetExtrapolatedPositionOffset()
	{
		float num = (float)(PhotonNetwork.time - this.m_LastSerializeTime);
		if (this.m_Model.ExtrapolateIncludingRoundTripTime)
		{
			num += (float)PhotonNetwork.GetPing() / 1000f;
		}
		Vector3 vector = Vector3.zero;
		PhotonTransformViewPositionModel.ExtrapolateOptions extrapolateOption = this.m_Model.ExtrapolateOption;
		if (extrapolateOption != PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues)
		{
			if (extrapolateOption != PhotonTransformViewPositionModel.ExtrapolateOptions.FixedSpeed)
			{
				if (extrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.EstimateSpeedAndTurn)
				{
					Vector3 vector2 = (this.m_NetworkPosition - this.GetOldestStoredNetworkPosition()) * (float)PhotonNetwork.sendRateOnSerialize;
					vector = vector2 * num;
				}
			}
			else
			{
				Vector3 normalized = (this.m_NetworkPosition - this.GetOldestStoredNetworkPosition()).normalized;
				vector = normalized * this.m_Model.ExtrapolateSpeed * num;
			}
		}
		else
		{
			Quaternion quaternion = Quaternion.Euler(0f, this.m_SynchronizedTurnSpeed * num, 0f);
			vector = quaternion * (this.m_SynchronizedSpeed * num);
		}
		return vector;
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0001B8C4 File Offset: 0x00019CC4
	public void OnPhotonSerializeView(Vector3 currentPosition, PhotonStream stream, PhotonMessageInfo info)
	{
		if (!this.m_Model.SynchronizeEnabled)
		{
			return;
		}
		if (stream.isWriting)
		{
			this.SerializeData(currentPosition, stream, info);
		}
		else
		{
			this.DeserializeData(stream, info);
		}
		this.m_LastSerializeTime = PhotonNetwork.time;
		this.m_UpdatedPositionAfterOnSerialize = false;
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x0001B918 File Offset: 0x00019D18
	private void SerializeData(Vector3 currentPosition, PhotonStream stream, PhotonMessageInfo info)
	{
		stream.SendNext(currentPosition);
		this.m_NetworkPosition = currentPosition;
		if (this.m_Model.ExtrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues || this.m_Model.InterpolateOption == PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues)
		{
			stream.SendNext(this.m_SynchronizedSpeed);
			stream.SendNext(this.m_SynchronizedTurnSpeed);
		}
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0001B97C File Offset: 0x00019D7C
	private void DeserializeData(PhotonStream stream, PhotonMessageInfo info)
	{
		Vector3 vector = (Vector3)stream.ReceiveNext();
		if (this.m_Model.ExtrapolateOption == PhotonTransformViewPositionModel.ExtrapolateOptions.SynchronizeValues || this.m_Model.InterpolateOption == PhotonTransformViewPositionModel.InterpolateOptions.SynchronizeValues)
		{
			this.m_SynchronizedSpeed = (Vector3)stream.ReceiveNext();
			this.m_SynchronizedTurnSpeed = (float)stream.ReceiveNext();
		}
		if (this.m_OldNetworkPositions.Count == 0)
		{
			this.m_NetworkPosition = vector;
		}
		this.m_OldNetworkPositions.Enqueue(this.m_NetworkPosition);
		this.m_NetworkPosition = vector;
		while (this.m_OldNetworkPositions.Count > this.m_Model.ExtrapolateNumberOfStoredPositions)
		{
			this.m_OldNetworkPositions.Dequeue();
		}
	}

	// Token: 0x0400043A RID: 1082
	private PhotonTransformViewPositionModel m_Model;

	// Token: 0x0400043B RID: 1083
	private float m_CurrentSpeed;

	// Token: 0x0400043C RID: 1084
	private double m_LastSerializeTime;

	// Token: 0x0400043D RID: 1085
	private Vector3 m_SynchronizedSpeed = Vector3.zero;

	// Token: 0x0400043E RID: 1086
	private float m_SynchronizedTurnSpeed;

	// Token: 0x0400043F RID: 1087
	private Vector3 m_NetworkPosition;

	// Token: 0x04000440 RID: 1088
	private Queue<Vector3> m_OldNetworkPositions = new Queue<Vector3>();

	// Token: 0x04000441 RID: 1089
	private bool m_UpdatedPositionAfterOnSerialize = true;
}
