using System;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class AttachmentPointSphereVisibility : MonoBehaviour
{
	// Token: 0x060006F8 RID: 1784 RVA: 0x00020328 File Offset: 0x0001E728
	private void Start()
	{
		this.renderer = base.gameObject.GetComponent<MeshRenderer>();
		float num = 0.5f + global::UnityEngine.Random.Range(-0.01f, 0.01f);
		base.InvokeRepeating("AdjustVisibility", num, num);
	}

	// Token: 0x060006F9 RID: 1785 RVA: 0x0002036C File Offset: 0x0001E76C
	private void AdjustVisibility()
	{
		bool flag = true;
		if (Managers.personManager == null || Managers.areaManager == null)
		{
			return;
		}
		Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(base.gameObject);
		if (personThisObjectIsOf == null)
		{
			return;
		}
		if (personThisObjectIsOf.isOurPerson && Our.mode == EditModes.Body)
		{
			flag = true;
		}
		else if (Managers.areaManager.rights.invisibility == true)
		{
			flag = false;
		}
		else
		{
			string name = base.transform.parent.name;
			bool flag2 = name == "HeadAttachmentPoint" || name == "UpperTorsoAttachmentPoint";
			if (flag2)
			{
				flag = true;
			}
			else if (Misc.GetChildWithTag(base.transform.parent, "Attachment") != null)
			{
				flag = false;
			}
			else if (name != null)
			{
				if (!(name == "HeadTopAttachmentPoint"))
				{
					if (!(name == "LowerTorsoAttachmentPoint"))
					{
						if (name == "LegLeftAttachmentPoint" || name == "LegRightAttachmentPoint")
						{
							flag = false;
						}
					}
					else
					{
						flag = !personThisObjectIsOf.HasTorsoUpperAttachment();
					}
				}
				else
				{
					flag = !personThisObjectIsOf.HasHeadAttachment();
				}
			}
		}
		if (this.renderer.enabled != flag)
		{
			this.renderer.enabled = flag;
		}
	}

	// Token: 0x04000511 RID: 1297
	private MeshRenderer renderer;
}
