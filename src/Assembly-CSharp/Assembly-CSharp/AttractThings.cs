using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000257 RID: 599
public class AttractThings : MonoBehaviour
{
	// Token: 0x06001625 RID: 5669 RVA: 0x000C1AB4 File Offset: 0x000BFEB4
	private void Start()
	{
		this.FindAllThingRigidbodiesToWatch();
		if (this.settings.forwardOnly)
		{
			this.largestBoundsExtent = this.GetLargestBoundsExtent();
		}
	}

	// Token: 0x06001626 RID: 5670 RVA: 0x000C1AD8 File Offset: 0x000BFED8
	private void Update()
	{
		this.ApplyForceToWatchedBodies();
	}

	// Token: 0x06001627 RID: 5671 RVA: 0x000C1AE0 File Offset: 0x000BFEE0
	private void ApplyForceToWatchedBodies()
	{
		if (this.settings.strength != 0f && this.watchedBodies.Count >= 1 && this.UntargetedRequirementOk())
		{
			float num = Mathf.Abs(this.settings.strength);
			float num2 = num;
			foreach (Rigidbody rigidbody in this.watchedBodies)
			{
				float num3;
				if (rigidbody != null && this.PointIsInForceScope(rigidbody.position, num2, out num3))
				{
					Vector3 normalized = (base.transform.position - rigidbody.position).normalized;
					Vector3 vector = num * normalized;
					float num4 = 1f - num3 / num2;
					vector *= num4;
					if (this.settings.strength < 0f)
					{
						vector *= -1f;
					}
					rigidbody.AddForce(vector * Time.deltaTime * 90f);
				}
			}
		}
	}

	// Token: 0x06001628 RID: 5672 RVA: 0x000C1C1C File Offset: 0x000C001C
	private bool UntargetedRequirementOk()
	{
		bool flag = true;
		if (Managers.areaManager.rights.untargetedAttractThings == false)
		{
			flag = this.settings.thingNameFilter != null && this.settings.thingNameFilter.Length >= 3;
		}
		return flag;
	}

	// Token: 0x06001629 RID: 5673 RVA: 0x000C1C80 File Offset: 0x000C0080
	private bool PointIsInForceScope(Vector3 point, float maxRadius, out float distance)
	{
		distance = Vector3.Distance(base.transform.position, point);
		bool flag = distance < maxRadius;
		if (flag && this.settings.forwardOnly)
		{
			flag = false;
			Vector3 position = base.transform.position;
			Vector3 vector = position + base.transform.forward * maxRadius;
			float num = this.DistancePointToLine(point, position, vector);
			if (num <= this.largestBoundsExtent)
			{
				Vector3 normalized = (point - base.transform.position).normalized;
				float num2 = Vector3.Dot(normalized, base.transform.forward);
				bool flag2 = num2 >= 0f;
				flag = flag2;
			}
		}
		return flag;
	}

	// Token: 0x0600162A RID: 5674 RVA: 0x000C1D3C File Offset: 0x000C013C
	private float GetLargestBoundsExtent()
	{
		float num = 0.5f;
		Collider component = base.gameObject.GetComponent<Collider>();
		if (component != null)
		{
			num = Misc.GetLargestValueOfVector(component.bounds.extents);
		}
		return num;
	}

	// Token: 0x0600162B RID: 5675 RVA: 0x000C1D7C File Offset: 0x000C017C
	private void FindAllThingRigidbodiesToWatch()
	{
		Component[] allRigidbodies = Managers.thingManager.GetAllRigidbodies();
		foreach (Rigidbody rigidbody in allRigidbodies)
		{
			if (this.ThingNameIsOk(rigidbody) && rigidbody.transform != base.transform)
			{
				this.watchedBodies.Add(rigidbody);
			}
		}
	}

	// Token: 0x0600162C RID: 5676 RVA: 0x000C1DE4 File Offset: 0x000C01E4
	public void WatchThingRigidbody(Rigidbody thingRigidbody)
	{
		if (!this.watchedBodies.Contains(thingRigidbody) && this.ThingNameIsOk(thingRigidbody) && thingRigidbody.transform != base.transform)
		{
			this.watchedBodies.Add(thingRigidbody);
		}
	}

	// Token: 0x0600162D RID: 5677 RVA: 0x000C1E30 File Offset: 0x000C0230
	private bool ThingNameIsOk(Rigidbody body)
	{
		bool flag = true;
		if (this.settings.thingNameFilter != null && body != null)
		{
			Thing component = body.GetComponent<Thing>();
			flag = component != null && component.givenName.Contains(this.settings.thingNameFilter);
		}
		return flag;
	}

	// Token: 0x0600162E RID: 5678 RVA: 0x000C1E89 File Offset: 0x000C0289
	private float DistancePointToLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		return Vector3.Magnitude(this.ProjectPointLine(point, lineStart, lineEnd) - point);
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x000C1EA0 File Offset: 0x000C02A0
	private Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
	{
		Vector3 vector = point - lineStart;
		Vector3 vector2 = lineEnd - lineStart;
		float magnitude = vector2.magnitude;
		Vector3 vector3 = vector2;
		if (magnitude > 1E-06f)
		{
			vector3 /= magnitude;
		}
		float num = Mathf.Clamp(Vector3.Dot(vector3, vector), 0f, magnitude);
		return lineStart + vector3 * num;
	}

	// Token: 0x04001406 RID: 5126
	public AttractThingsSettings settings = new AttractThingsSettings();

	// Token: 0x04001407 RID: 5127
	private List<Rigidbody> watchedBodies = new List<Rigidbody>();

	// Token: 0x04001408 RID: 5128
	private float largestBoundsExtent;
}
