using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class AttachmentPoint : MonoBehaviour
{
	// Token: 0x060006E9 RID: 1769 RVA: 0x0001FEC0 File Offset: 0x0001E2C0
	public void AttachThing(GameObject thing)
	{
		if (this.attachedThing != null)
		{
			Log.Info("AttachmentPoint.AttachThing called when something already attached", false);
			global::UnityEngine.Object.DestroyImmediate(this.attachedThing);
		}
		this.attachedThing = thing;
		Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(base.gameObject);
		if (personThisObjectIsOf != null && personThisObjectIsOf.transform.localScale.x != 1f)
		{
			thing.transform.localScale = Misc.GetUniformVector3(personThisObjectIsOf.transform.localScale.x);
		}
		thing.transform.parent = base.transform;
		thing.GetComponent<Thing>().OnAttached();
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x0001FF74 File Offset: 0x0001E374
	public void DetachAndDestroyAttachedThing()
	{
		if (this.attachedThing != null)
		{
			Thing component = this.attachedThing.GetComponent<Thing>();
			if (component)
			{
				component.OnDeleteAttachment();
			}
		}
		global::UnityEngine.Object.Destroy(this.attachedThing);
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x0001FFBC File Offset: 0x0001E3BC
	public AttachmentData GetAttachmentData()
	{
		AttachmentData attachmentData = null;
		if (this.attachedThing != null)
		{
			Thing component = this.attachedThing.GetComponent<Thing>();
			attachmentData = new AttachmentData(component.thingId, this.attachedThing.transform.localPosition, this.attachedThing.transform.localEulerAngles);
		}
		else
		{
			Debug.Log("GetAttachmentData - no attached thing");
		}
		return attachmentData;
	}

	// Token: 0x0400050B RID: 1291
	public AttachmentPointId id;

	// Token: 0x0400050C RID: 1292
	public GameObject attachedThing;
}
