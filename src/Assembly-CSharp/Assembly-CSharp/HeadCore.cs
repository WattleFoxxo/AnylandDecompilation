using System;
using UnityEngine;

// Token: 0x020000E6 RID: 230
public class HeadCore : MonoBehaviour
{
	// Token: 0x060007B7 RID: 1975 RVA: 0x0002AD8F File Offset: 0x0002918F
	private void Start()
	{
		this.voiceController = base.gameObject.GetComponent<VoiceController>();
		this.layerOfAllButInvisible = ~(1 << LayerMask.NameToLayer("InvisibleToOurPerson"));
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x0002ADB8 File Offset: 0x000291B8
	private void Update()
	{
		this.HandleInfrequentEventChecks();
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x0002ADC0 File Offset: 0x000291C0
	private void HandleInfrequentEventChecks()
	{
		if (this.timeOfLastInfrequentEventsCheck == -1f || this.timeOfLastInfrequentEventsCheck + this.infrequentEventsCheckDelay <= Time.time)
		{
			this.timeOfLastInfrequentEventsCheck = Time.time;
			this.CheckForLookedAtAndTalkedToEvent();
		}
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x0002ADFC File Offset: 0x000291FC
	private void CheckForLookedAtAndTalkedToEvent()
	{
		Ray ray = new Ray(base.transform.position + base.transform.forward * 0.05f, base.transform.forward);
		float num = 10f;
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit, num, this.layerOfAllButInvisible))
		{
			GameObject gameObject = raycastHit.collider.gameObject;
			if (gameObject.tag == "ThingPart")
			{
				ThingPart component = gameObject.GetComponent<ThingPart>();
				if (component != null)
				{
					component.TriggerEventAsStateAuthority(StateListener.EventType.OnLookedAt, string.Empty);
				}
				if (gameObject.transform.parent != null && this.voiceController != null && this.voiceController.isOurPerson && this.voiceController.isTalking)
				{
					Thing component2 = gameObject.transform.parent.GetComponent<Thing>();
					component2.TriggerEventAsStateAuthority(StateListener.EventType.OnTalkedTo, string.Empty);
				}
			}
		}
	}

	// Token: 0x040005B1 RID: 1457
	private float infrequentEventsCheckDelay = 0.5f;

	// Token: 0x040005B2 RID: 1458
	private float timeOfLastInfrequentEventsCheck = -1f;

	// Token: 0x040005B3 RID: 1459
	private VoiceController voiceController;

	// Token: 0x040005B4 RID: 1460
	private int layerOfAllButInvisible = -1;
}
