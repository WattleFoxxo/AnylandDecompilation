using System;
using UnityEngine;

// Token: 0x020000DD RID: 221
public class AttachmentPointSphere : MonoBehaviour
{
	// Token: 0x060006F2 RID: 1778 RVA: 0x00020176 File Offset: 0x0001E576
	private void Start()
	{
		this.renderer = base.gameObject.GetComponent<Renderer>();
	}

	// Token: 0x060006F3 RID: 1779 RVA: 0x0002018C File Offset: 0x0001E58C
	private void Update()
	{
		if (this.collidingThing != null && this.collidingThing.tag != "Attachment" && this.collidingThing.transform.parent != null)
		{
			Hand component = this.collidingThing.transform.parent.gameObject.GetComponent<Hand>();
			if (component != null && (!(base.gameObject.name == "HeadAttachmentPointSphere") || !Managers.personManager.ourPerson.HasHeadAttachment()))
			{
				component.TriggerHapticPulse(Universe.lowHapticPulse);
			}
		}
		if (this.doHighlight)
		{
			Color color = new Color(Mathf.PingPong(Time.time, 0.75f), Mathf.PingPong(Time.time, 0.75f), Mathf.PingPong(Time.time, 0.75f));
			this.renderer.material.SetColor("_EmissionColor", color);
		}
	}

	// Token: 0x060006F4 RID: 1780 RVA: 0x00020294 File Offset: 0x0001E694
	public void SetDoHighlight(bool _doHighlight)
	{
		this.doHighlight = _doHighlight;
		if (!this.doHighlight)
		{
			this.renderer.material.SetColor("_EmissionColor", Color.black);
		}
	}

	// Token: 0x060006F5 RID: 1781 RVA: 0x000202C2 File Offset: 0x0001E6C2
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "ThingPart" && this.collidingThing == null)
		{
			this.collidingThing = other.transform.parent.gameObject;
		}
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x00020300 File Offset: 0x0001E700
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "ThingPart")
		{
			this.collidingThing = null;
		}
	}

	// Token: 0x0400050E RID: 1294
	public GameObject collidingThing;

	// Token: 0x0400050F RID: 1295
	private Renderer renderer;

	// Token: 0x04000510 RID: 1296
	private bool doHighlight;
}
