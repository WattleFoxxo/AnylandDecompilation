using System;
using UnityEngine;

// Token: 0x02000255 RID: 597
[RequireComponent(typeof(PhotonView))]
public class PersonLegStateSync : MonoBehaviour
{
	// Token: 0x0600161D RID: 5661 RVA: 0x000C1634 File Offset: 0x000BFA34
	private void Start()
	{
		this.photonView = base.GetComponent<PhotonView>();
		if (this.photonView == null)
		{
			Log.Error("Could not find PhotonView");
		}
	}

	// Token: 0x0600161E RID: 5662 RVA: 0x000C1660 File Offset: 0x000BFA60
	private void Update()
	{
		if (this.isOurPerson)
		{
			this.localPositionToSend = base.transform.localPosition;
			this.localRotationToSend = base.transform.localRotation;
		}
		else if (this.haveStartedToReceiveData)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, this.lastReceivedLocalPosition, 0.25f);
			base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, this.lastReceivedLocalRotation, 0.25f);
		}
	}

	// Token: 0x0600161F RID: 5663 RVA: 0x000C16F8 File Offset: 0x000BFAF8
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (this.isOurPerson && Managers.personManager && Managers.personManager.PartMovementIsRelevantEnoughToTransmit(true, this.localPositionToSend, this.localRotationToSend, ref this.lastLocalPositionSent, ref this.lastLocalRotationSent, ref this.timeOfLastForceSent))
			{
				stream.SendNext(this.localPositionToSend);
				stream.SendNext(this.localRotationToSend);
			}
		}
		else if (!this.isOurPerson)
		{
			this.lastReceivedMessageTimeStamp = info.timestamp;
			this.lastReceivedLocalPosition = (Vector3)stream.ReceiveNext();
			this.lastReceivedLocalRotation = (Quaternion)stream.ReceiveNext();
			if (!this.haveStartedToReceiveData)
			{
				base.transform.localPosition = this.lastReceivedLocalPosition;
				base.transform.localRotation = this.lastReceivedLocalRotation;
				this.haveStartedToReceiveData = true;
			}
		}
	}

	// Token: 0x040013EA RID: 5098
	public bool isOurPerson;

	// Token: 0x040013EB RID: 5099
	private PhotonView photonView;

	// Token: 0x040013EC RID: 5100
	private const float lerpValue = 0.25f;

	// Token: 0x040013ED RID: 5101
	private Vector3 lastReceivedLocalPosition;

	// Token: 0x040013EE RID: 5102
	private Quaternion lastReceivedLocalRotation;

	// Token: 0x040013EF RID: 5103
	private double lastReceivedMessageTimeStamp;

	// Token: 0x040013F0 RID: 5104
	private Vector3 localPositionToSend;

	// Token: 0x040013F1 RID: 5105
	private Quaternion localRotationToSend;

	// Token: 0x040013F2 RID: 5106
	private Vector3 lastLocalPositionSent;

	// Token: 0x040013F3 RID: 5107
	private Quaternion lastLocalRotationSent;

	// Token: 0x040013F4 RID: 5108
	private bool haveStartedToReceiveData;

	// Token: 0x040013F5 RID: 5109
	private float timeOfLastForceSent = -1f;
}
