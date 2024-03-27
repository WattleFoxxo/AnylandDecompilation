using System;
using System.Collections.Generic;
using DaikonForge.VoIP;
using UnityEngine;

// Token: 0x020000EC RID: 236
public class VoiceController : VoiceControllerBase
{
	// Token: 0x1700013B RID: 315
	// (get) Token: 0x060007D7 RID: 2007 RVA: 0x0002B6B8 File Offset: 0x00029AB8
	public override bool IsLocal
	{
		get
		{
			return this.isOurPerson;
		}
	}

	// Token: 0x1700013C RID: 316
	// (get) Token: 0x060007D8 RID: 2008 RVA: 0x0002B6C0 File Offset: 0x00029AC0
	public bool isTalking
	{
		get
		{
			return Time.realtimeSinceStartup <= this.lastTalkingTime + 0.1f;
		}
	}

	// Token: 0x060007D9 RID: 2009 RVA: 0x0002B6D8 File Offset: 0x00029AD8
	private void Start()
	{
		this.photonView = base.GetComponent<PhotonView>();
		this.microphoneDevice = base.gameObject.GetComponent<MicrophoneInputDevice>();
	}

	// Token: 0x060007DA RID: 2010 RVA: 0x0002B6F8 File Offset: 0x00029AF8
	protected override void OnAudioDataEncoded(VoicePacketWrapper encodedFrame)
	{
		if (Universe.transmitFromMicrophone && PhotonNetwork.inRoom)
		{
			if (Universe.hearEchoOfMyVoice)
			{
				this.ReceiveAudioData(encodedFrame);
			}
			byte[] array = encodedFrame.ObtainHeaders();
			this.photonView.RPC("vc", PhotonTargets.Others, new object[] { array, encodedFrame.RawData });
			encodedFrame.ReleaseHeaders();
			this.lastTalkingTime = Time.realtimeSinceStartup;
			if (this.isOurPerson && this.microphoneDevice.lastMaxAmplitude >= 0.01f)
			{
				this.HandleTriggerOnTalkedFrom();
			}
		}
	}

	// Token: 0x060007DB RID: 2011 RVA: 0x0002B790 File Offset: 0x00029B90
	private void HandleTriggerOnTalkedFrom()
	{
		if (this.lastOnTalkedFromCheck == -1f || Time.realtimeSinceStartup >= this.lastOnTalkedFromCheck + 0.15f)
		{
			this.lastOnTalkedFromCheck = Time.realtimeSinceStartup;
			GameObject childWithTag = Misc.GetChildWithTag(this.headAttachmentPoint.transform, "Attachment");
			Thing thing = ((!(childWithTag != null)) ? null : childWithTag.GetComponent<Thing>());
			if (thing != null)
			{
				thing.TriggerEventAsStateAuthority(StateListener.EventType.OnTalkedFrom, string.Empty);
				if (!this.ContainsOnTalkedFrom(thing))
				{
					Managers.personManager.DoIndicateIsSpeaking();
				}
			}
			else
			{
				Managers.personManager.DoIndicateIsSpeaking();
			}
		}
	}

	// Token: 0x060007DC RID: 2012 RVA: 0x0002B83C File Offset: 0x00029C3C
	private bool ContainsOnTalkedFrom(Thing thing)
	{
		bool flag = false;
		if (!this.thingContainsOnTalkedFrom.TryGetValue(thing.thingId, out flag))
		{
			Component[] componentsInChildren = thing.GetComponentsInChildren(typeof(ThingPart), true);
			foreach (ThingPart thingPart in componentsInChildren)
			{
				for (int j = 0; j < thingPart.states.Count; j++)
				{
					foreach (StateListener stateListener in thingPart.states[j].listeners)
					{
						if (stateListener.eventType == StateListener.EventType.OnTalkedFrom)
						{
							flag = true;
							this.thingContainsOnTalkedFrom[thing.thingId] = flag;
							return flag;
						}
					}
				}
			}
			this.thingContainsOnTalkedFrom[thing.thingId] = flag;
		}
		return flag;
	}

	// Token: 0x060007DD RID: 2013 RVA: 0x0002B950 File Offset: 0x00029D50
	[PunRPC]
	private void vc(byte[] headers, byte[] rawData)
	{
		VoicePacketWrapper voicePacketWrapper = new VoicePacketWrapper(headers, rawData);
		this.ReceiveAudioData(voicePacketWrapper);
	}

	// Token: 0x040005C0 RID: 1472
	public bool isOurPerson;

	// Token: 0x040005C1 RID: 1473
	public GameObject headAttachmentPoint;

	// Token: 0x040005C2 RID: 1474
	private PhotonView photonView;

	// Token: 0x040005C3 RID: 1475
	private float lastTalkingTime = -1f;

	// Token: 0x040005C4 RID: 1476
	private float lastOnTalkedFromCheck = -1f;

	// Token: 0x040005C5 RID: 1477
	private const float onTalkedFromCheckSeconds = 0.15f;

	// Token: 0x040005C6 RID: 1478
	private Dictionary<string, bool> thingContainsOnTalkedFrom = new Dictionary<string, bool>();

	// Token: 0x040005C7 RID: 1479
	private MicrophoneInputDevice microphoneDevice;
}
