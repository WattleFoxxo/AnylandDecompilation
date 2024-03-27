using System;
using UnityEngine;

// Token: 0x02000256 RID: 598
[RequireComponent(typeof(PhotonView))]
public class PersonRigStateSync : MonoBehaviour
{
	// Token: 0x06001621 RID: 5665 RVA: 0x000C1800 File Offset: 0x000BFC00
	private void Start()
	{
		this.photonView = base.GetComponent<PhotonView>();
		if (this.photonView == null)
		{
			Log.Error("Could not find PhotonView Component");
		}
		this.person = base.GetComponent<Person>();
	}

	// Token: 0x06001622 RID: 5666 RVA: 0x000C1838 File Offset: 0x000BFC38
	private void Update()
	{
		if (this.IsOurPerson)
		{
			this.positionToSend = base.transform.position;
			this.rotationToSend = base.transform.rotation;
			this.scaleToSend = base.transform.localScale.x;
		}
		else if (this.haveStartedToReceiveData)
		{
			if (this.person.ridingBeacon == null)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, this.lastReceivedPosition, 0.25f);
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this.lastReceivedRotation, 0.25f);
			}
			base.transform.localScale = Vector3.Lerp(base.transform.localScale, Misc.GetUniformVector3(this.lastReceivedScale), 0.25f);
			if (Managers.areaManager != null && base.transform.localScale.x < this.person.smallestScaleObservedSoFar)
			{
				this.person.smallestScaleObservedSoFar = base.transform.localScale.x;
				Managers.areaManager.AlertVerySmallPersonScaleIfNeeded(base.transform.localScale.x);
			}
		}
	}

	// Token: 0x06001623 RID: 5667 RVA: 0x000C1998 File Offset: 0x000BFD98
	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			if ((Managers.personManager && Managers.personManager.PartMovementIsRelevantEnoughToTransmit(true, this.positionToSend, this.rotationToSend, ref this.lastPositionSent, ref this.lastRotationSent, ref this.timeOfLastForceSent)) || this.lastScaleSent != this.scaleToSend)
			{
				this.lastScaleSent = this.scaleToSend;
				stream.SendNext(this.positionToSend);
				stream.SendNext(this.rotationToSend);
				stream.SendNext(base.transform.localScale.x);
			}
		}
		else
		{
			this.lastReceivedMessageTimeStamp = info.timestamp;
			this.lastReceivedPosition = (Vector3)stream.ReceiveNext();
			this.lastReceivedRotation = (Quaternion)stream.ReceiveNext();
			this.lastReceivedScale = (float)stream.ReceiveNext();
			this.haveStartedToReceiveData = true;
		}
	}

	// Token: 0x040013F6 RID: 5110
	public bool IsOurPerson;

	// Token: 0x040013F7 RID: 5111
	private PhotonView photonView;

	// Token: 0x040013F8 RID: 5112
	private const float lerpValue = 0.25f;

	// Token: 0x040013F9 RID: 5113
	private Vector3 lastReceivedPosition;

	// Token: 0x040013FA RID: 5114
	private Quaternion lastReceivedRotation;

	// Token: 0x040013FB RID: 5115
	private float lastReceivedScale;

	// Token: 0x040013FC RID: 5116
	private double lastReceivedMessageTimeStamp;

	// Token: 0x040013FD RID: 5117
	private Vector3 positionToSend;

	// Token: 0x040013FE RID: 5118
	private Quaternion rotationToSend;

	// Token: 0x040013FF RID: 5119
	private float scaleToSend;

	// Token: 0x04001400 RID: 5120
	private Vector3 lastPositionSent;

	// Token: 0x04001401 RID: 5121
	private Quaternion lastRotationSent;

	// Token: 0x04001402 RID: 5122
	private float lastScaleSent;

	// Token: 0x04001403 RID: 5123
	private bool haveStartedToReceiveData;

	// Token: 0x04001404 RID: 5124
	private float timeOfLastForceSent = -1f;

	// Token: 0x04001405 RID: 5125
	private Person person;
}
