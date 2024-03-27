using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DC RID: 220
public class AttachmentPointsMemory
{
	// Token: 0x060006ED RID: 1773 RVA: 0x0002002C File Offset: 0x0001E42C
	public void Memorize()
	{
		Person ourPerson = Managers.personManager.ourPerson;
		this.clipboards = new Dictionary<AttachmentPointId, TransformClipboard>();
		IEnumerator enumerator = Enum.GetValues(typeof(AttachmentPointId)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				AttachmentPointId attachmentPointId = (AttachmentPointId)obj;
				if (attachmentPointId != AttachmentPointId.None)
				{
					GameObject attachmentPointById = ourPerson.GetAttachmentPointById(attachmentPointId, true);
					if (attachmentPointById != null)
					{
						AttachmentPoint component = attachmentPointById.GetComponent<AttachmentPoint>();
						if (component != null && component.attachedThing != null)
						{
							TransformClipboard transformClipboard = new TransformClipboard();
							transformClipboard.useLocal = true;
							transformClipboard.SetFromTransform(component.attachedThing.transform);
							this.clipboards.Add(attachmentPointId, transformClipboard);
						}
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x060006EE RID: 1774 RVA: 0x00020120 File Offset: 0x0001E520
	public void SnapAttachmentIfMemorized(AttachmentPointId id, GameObject attachment)
	{
		TransformClipboard transformClipboard;
		if (this.clipboards != null && this.clipboards.TryGetValue(id, out transformClipboard))
		{
			transformClipboard.ApplyToTransform(attachment.transform);
		}
	}

	// Token: 0x060006EF RID: 1775 RVA: 0x00020157 File Offset: 0x0001E557
	public void Clear()
	{
		this.clipboards = null;
	}

	// Token: 0x060006F0 RID: 1776 RVA: 0x00020160 File Offset: 0x0001E560
	public bool HasMemorized()
	{
		return this.clipboards != null;
	}

	// Token: 0x0400050D RID: 1293
	private Dictionary<AttachmentPointId, TransformClipboard> clipboards;
}
