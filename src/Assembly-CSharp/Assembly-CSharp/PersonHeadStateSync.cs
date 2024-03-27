using System;
using UnityEngine;

// Token: 0x02000254 RID: 596
[RequireComponent(typeof(PhotonView))]
public class PersonHeadStateSync : MonoBehaviour
{
	// Token: 0x06001619 RID: 5657 RVA: 0x000C1468 File Offset: 0x000BF868
	private void Start()
	{
		this.photonView = base.GetComponent<PhotonView>();
		if (this.photonView == null)
		{
			Log.Error("Could not find PhotonView");
		}
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x000C1494 File Offset: 0x000BF894
	private void Update()
	{
		if (this.IsOurPerson)
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

	// Token: 0x0600161B RID: 5659 RVA: 0x000C152C File Offset: 0x000BF92C
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (this.IsOurPerson && Managers.personManager && Managers.personManager.PartMovementIsRelevantEnoughToTransmit(true, this.localPositionToSend, this.localRotationToSend, ref this.lastLocalPositionSent, ref this.lastLocalRotationSent, ref this.timeOfLastForceSent))
			{
				stream.SendNext(this.localPositionToSend);
				stream.SendNext(this.localRotationToSend);
			}
		}
		else if (!this.IsOurPerson)
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

	// Token: 0x040013DE RID: 5086
	public bool IsOurPerson;

	// Token: 0x040013DF RID: 5087
	private PhotonView photonView;

	// Token: 0x040013E0 RID: 5088
	private const float lerpValue = 0.25f;

	// Token: 0x040013E1 RID: 5089
	private Vector3 lastReceivedLocalPosition;

	// Token: 0x040013E2 RID: 5090
	private Quaternion lastReceivedLocalRotation;

	// Token: 0x040013E3 RID: 5091
	private double lastReceivedMessageTimeStamp;

	// Token: 0x040013E4 RID: 5092
	private Vector3 localPositionToSend;

	// Token: 0x040013E5 RID: 5093
	private Quaternion localRotationToSend;

	// Token: 0x040013E6 RID: 5094
	private Vector3 lastLocalPositionSent;

	// Token: 0x040013E7 RID: 5095
	private Quaternion lastLocalRotationSent;

	// Token: 0x040013E8 RID: 5096
	private bool haveStartedToReceiveData;

	// Token: 0x040013E9 RID: 5097
	private float timeOfLastForceSent = -1f;
}
