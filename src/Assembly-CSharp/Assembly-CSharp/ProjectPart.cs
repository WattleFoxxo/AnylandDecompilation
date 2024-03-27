using System;
using System.Linq;
using UnityEngine;

// Token: 0x02000264 RID: 612
public class ProjectPart : MonoBehaviour
{
	// Token: 0x06001686 RID: 5766 RVA: 0x000CA8A4 File Offset: 0x000C8CA4
	private void Start()
	{
		this.MemorizeOriginalTransform();
	}

	// Token: 0x06001687 RID: 5767 RVA: 0x000CA8AC File Offset: 0x000C8CAC
	private void Update()
	{
		Transform parent = base.transform.parent;
		if (parent != null)
		{
			Vector3 forward = parent.forward;
			Ray ray = new Ray(parent.position, forward);
			RaycastHit[] array = (from h in Physics.RaycastAll(ray, this.settings.maxDistance)
				orderby h.distance
				select h).ToArray<RaycastHit>();
			bool flag = false;
			foreach (RaycastHit raycastHit in array)
			{
				GameObject gameObject = raycastHit.collider.gameObject;
				if (raycastHit.transform.parent != parent && gameObject.CompareTag("ThingPart"))
				{
					flag = true;
					base.transform.position = Mathfx.LerpWithOvershoot(parent.position, raycastHit.point, this.settings.relativeReach);
					base.transform.localPosition += this.originalLocalPosition;
					if (this.settings.align == ProjectPartAlignment.TowardsSurface)
					{
						base.transform.rotation = Quaternion.FromToRotation(Vector3.forward, -raycastHit.normal);
					}
					else if (this.settings.align == ProjectPartAlignment.AwayFromSurface)
					{
						base.transform.rotation = Quaternion.FromToRotation(Vector3.forward, raycastHit.normal);
					}
					break;
				}
			}
			if (!flag)
			{
				this.RestoreOriginalTransform();
				if (this.settings.defaultDistance != 0f)
				{
					base.transform.position += this.settings.defaultDistance * parent.forward;
				}
			}
		}
	}

	// Token: 0x06001688 RID: 5768 RVA: 0x000CAA7D File Offset: 0x000C8E7D
	private void OnDestroy()
	{
		this.RestoreOriginalTransform();
	}

	// Token: 0x06001689 RID: 5769 RVA: 0x000CAA85 File Offset: 0x000C8E85
	private void MemorizeOriginalTransform()
	{
		this.originalLocalPosition = base.transform.localPosition;
		this.originalRotation = base.transform.localRotation;
	}

	// Token: 0x0600168A RID: 5770 RVA: 0x000CAAA9 File Offset: 0x000C8EA9
	private void RestoreOriginalTransform()
	{
		base.transform.localPosition = this.originalLocalPosition;
		base.transform.localRotation = this.originalRotation;
	}

	// Token: 0x04001441 RID: 5185
	public ProjectPartSettings settings = new ProjectPartSettings();

	// Token: 0x04001442 RID: 5186
	private Vector3 originalLocalPosition = Vector3.zero;

	// Token: 0x04001443 RID: 5187
	private Quaternion originalRotation = Quaternion.identity;
}
