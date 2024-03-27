using System;
using UnityEngine;

// Token: 0x020000EA RID: 234
public class MouthCollisionSphere : MonoBehaviour
{
	// Token: 0x060007CE RID: 1998 RVA: 0x0002B380 File Offset: 0x00029780
	private void Start()
	{
		this.voiceController = base.transform.parent.GetComponent<VoiceController>();
	}

	// Token: 0x060007CF RID: 1999 RVA: 0x0002B398 File Offset: 0x00029798
	private void OnTriggerEnter(Collider other)
	{
		ThingPart appropriateThingPart = this.GetAppropriateThingPart(other);
		if (appropriateThingPart != null)
		{
			bool flag = appropriateThingPart.TriggerEventAsStateAuthority(StateListener.EventType.OnConsumed, string.Empty);
			if (flag)
			{
				Thing parentThing = appropriateThingPart.GetParentThing();
				if (parentThing != null)
				{
					if (parentThing.isHeldAsHoldableByOurPerson)
					{
						appropriateThingPart.TriggerHapticPulse(Universe.mediumHapticPulse);
					}
					if (parentThing.amplifySpeech)
					{
						base.CancelInvoke("AlertOfEmphasizedSpeech");
						base.Invoke("AlertOfEmphasizedSpeech", 0.25f);
					}
				}
				Managers.behaviorScriptManager.TriggerTellBodyEventToAttachments(Managers.personManager.ourPerson, "consumed", true);
			}
			appropriateThingPart.TriggerEventAsStateAuthority(StateListener.EventType.OnTouches, "face");
		}
	}

	// Token: 0x060007D0 RID: 2000 RVA: 0x0002B444 File Offset: 0x00029844
	private void AlertOfEmphasizedSpeech()
	{
		string text = ((!(Managers.areaManager.rights.amplifiedSpeech == true)) ? "Oops, amplified speech is disallowed here." : "Everyone here hears you now (you can disable that via Me dialog, or switching areas).");
		Managers.dialogManager.ShowInfo(text, false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
	}

	// Token: 0x060007D1 RID: 2001 RVA: 0x0002B4A4 File Offset: 0x000298A4
	private void OnTriggerStay(Collider other)
	{
		ThingPart appropriateThingPart = this.GetAppropriateThingPart(other);
		if (appropriateThingPart != null && (this.timeOfLastInfrequentEventsCheck == -1f || this.timeOfLastInfrequentEventsCheck + this.infrequentEventsCheckDelay <= Time.time))
		{
			this.timeOfLastInfrequentEventsCheck = Time.time;
			if (this.voiceController != null && this.voiceController.isOurPerson && this.voiceController.isTalking)
			{
				appropriateThingPart.TriggerEventAsStateAuthority(StateListener.EventType.OnBlownAt, string.Empty);
			}
		}
	}

	// Token: 0x060007D2 RID: 2002 RVA: 0x0002B538 File Offset: 0x00029938
	private ThingPart GetAppropriateThingPart(Collider other)
	{
		ThingPart thingPart = null;
		if (other.gameObject.CompareTag("ThingPart") && other.transform.parent != null && !other.transform.parent.CompareTag("Attachment"))
		{
			thingPart = other.gameObject.GetComponent<ThingPart>();
		}
		return thingPart;
	}

	// Token: 0x040005BC RID: 1468
	private VoiceController voiceController;

	// Token: 0x040005BD RID: 1469
	private float infrequentEventsCheckDelay = 0.1f;

	// Token: 0x040005BE RID: 1470
	private float timeOfLastInfrequentEventsCheck = -1f;
}
