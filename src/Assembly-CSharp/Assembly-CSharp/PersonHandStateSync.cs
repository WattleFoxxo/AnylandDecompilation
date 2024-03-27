using System;
using UnityEngine;

// Token: 0x02000253 RID: 595
[RequireComponent(typeof(PhotonView))]
public class PersonHandStateSync : MonoBehaviour
{
	// Token: 0x06001615 RID: 5653 RVA: 0x000C1250 File Offset: 0x000BF650
	private void Start()
	{
		if (this.isOurPerson)
		{
			this.hand = base.GetComponent<Hand>();
			if (this.hand == null)
			{
				Log.Error("Could not find our Hand for PersonHandStateSync");
			}
		}
		this.photonView = base.GetComponent<PhotonView>();
		if (this.photonView == null)
		{
			Log.Error("Could not find PhotonView for PersonHandStateSync");
		}
	}

	// Token: 0x06001616 RID: 5654 RVA: 0x000C12B8 File Offset: 0x000BF6B8
	private void Update()
	{
		if (this.isOurPerson)
		{
			if (!this.hand.isLegPuppeteering)
			{
				this.localPositionToSend = base.transform.localPosition;
				this.localRotationToSend = base.transform.localRotation;
			}
		}
		else if (this.haveStartedToReceiveData)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, this.lastReceivedLocalPosition, 0.25f);
			base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, this.lastReceivedLocalRotation, 0.25f);
		}
	}

	// Token: 0x06001617 RID: 5655 RVA: 0x000C1360 File Offset: 0x000BF760
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

	// Token: 0x040013D1 RID: 5073
	public bool isOurPerson;

	// Token: 0x040013D2 RID: 5074
	private Hand hand;

	// Token: 0x040013D3 RID: 5075
	private PhotonView photonView;

	// Token: 0x040013D4 RID: 5076
	private const float lerpValue = 0.25f;

	// Token: 0x040013D5 RID: 5077
	private Vector3 lastReceivedLocalPosition;

	// Token: 0x040013D6 RID: 5078
	private Quaternion lastReceivedLocalRotation;

	// Token: 0x040013D7 RID: 5079
	private double lastReceivedMessageTimeStamp;

	// Token: 0x040013D8 RID: 5080
	private Vector3 localPositionToSend;

	// Token: 0x040013D9 RID: 5081
	private Quaternion localRotationToSend;

	// Token: 0x040013DA RID: 5082
	private Vector3 lastLocalPositionSent;

	// Token: 0x040013DB RID: 5083
	private Quaternion lastLocalRotationSent;

	// Token: 0x040013DC RID: 5084
	private bool haveStartedToReceiveData;

	// Token: 0x040013DD RID: 5085
	private float timeOfLastForceSent = -1f;
}
