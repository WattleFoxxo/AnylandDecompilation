using System;
using UnityEngine;

// Token: 0x0200029E RID: 670
public class SteamVR_IK : MonoBehaviour
{
	// Token: 0x06001943 RID: 6467 RVA: 0x000E563C File Offset: 0x000E3A3C
	private void LateUpdate()
	{
		if (this.blendPct < 0.001f)
		{
			return;
		}
		Vector3 vector = ((!this.upVector) ? Vector3.Cross(this.end.position - this.start.position, this.joint.position - this.start.position).normalized : this.upVector.up);
		Vector3 position = this.target.position;
		Quaternion rotation = this.target.rotation;
		Vector3 position2 = this.joint.position;
		Vector3 vector2;
		Vector3 vector3;
		SteamVR_IK.Solve(this.start.position, position, this.poleVector.position, (this.joint.position - this.start.position).magnitude, (this.end.position - this.joint.position).magnitude, ref position2, out vector2, out vector3);
		if (vector3 == Vector3.zero)
		{
			return;
		}
		Vector3 position3 = this.start.position;
		Vector3 position4 = this.joint.position;
		Vector3 position5 = this.end.position;
		Quaternion localRotation = this.start.localRotation;
		Quaternion localRotation2 = this.joint.localRotation;
		Quaternion localRotation3 = this.end.localRotation;
		Transform parent = this.start.parent;
		Transform parent2 = this.joint.parent;
		Transform parent3 = this.end.parent;
		Vector3 localScale = this.start.localScale;
		Vector3 localScale2 = this.joint.localScale;
		Vector3 localScale3 = this.end.localScale;
		if (this.startXform == null)
		{
			this.startXform = new GameObject("startXform").transform;
			this.startXform.parent = base.transform;
		}
		this.startXform.position = position3;
		this.startXform.LookAt(this.joint, vector);
		this.start.parent = this.startXform;
		if (this.jointXform == null)
		{
			this.jointXform = new GameObject("jointXform").transform;
			this.jointXform.parent = this.startXform;
		}
		this.jointXform.position = position4;
		this.jointXform.LookAt(this.end, vector);
		this.joint.parent = this.jointXform;
		if (this.endXform == null)
		{
			this.endXform = new GameObject("endXform").transform;
			this.endXform.parent = this.jointXform;
		}
		this.endXform.position = position5;
		this.end.parent = this.endXform;
		this.startXform.LookAt(position2, vector3);
		this.jointXform.LookAt(position, vector3);
		this.endXform.rotation = rotation;
		this.start.parent = parent;
		this.joint.parent = parent2;
		this.end.parent = parent3;
		this.end.rotation = rotation;
		if (this.blendPct < 1f)
		{
			this.start.localRotation = Quaternion.Slerp(localRotation, this.start.localRotation, this.blendPct);
			this.joint.localRotation = Quaternion.Slerp(localRotation2, this.joint.localRotation, this.blendPct);
			this.end.localRotation = Quaternion.Slerp(localRotation3, this.end.localRotation, this.blendPct);
		}
		this.start.localScale = localScale;
		this.joint.localScale = localScale2;
		this.end.localScale = localScale3;
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x000E5A1C File Offset: 0x000E3E1C
	public static bool Solve(Vector3 start, Vector3 end, Vector3 poleVector, float jointDist, float targetDist, ref Vector3 result, out Vector3 forward, out Vector3 up)
	{
		float num = jointDist + targetDist;
		Vector3 vector = end - start;
		Vector3 normalized = (poleVector - start).normalized;
		float magnitude = vector.magnitude;
		result = start;
		if (magnitude < 0.001f)
		{
			result += normalized * jointDist;
			forward = Vector3.Cross(normalized, Vector3.up);
			up = Vector3.Cross(forward, normalized).normalized;
		}
		else
		{
			forward = vector * (1f / magnitude);
			up = Vector3.Cross(forward, normalized).normalized;
			if (magnitude + 0.001f < num)
			{
				float num2 = (num + magnitude) * 0.5f;
				if (num2 > jointDist + 0.001f && num2 > targetDist + 0.001f)
				{
					float num3 = Mathf.Sqrt(num2 * (num2 - jointDist) * (num2 - targetDist) * (num2 - magnitude));
					float num4 = 2f * num3 / magnitude;
					float num5 = Mathf.Sqrt(jointDist * jointDist - num4 * num4);
					Vector3 vector2 = Vector3.Cross(up, forward);
					result += forward * num5 + vector2 * num4;
					return true;
				}
				result += normalized * jointDist;
			}
			else
			{
				result += forward * jointDist;
			}
		}
		return false;
	}

	// Token: 0x0400176B RID: 5995
	public Transform target;

	// Token: 0x0400176C RID: 5996
	public Transform start;

	// Token: 0x0400176D RID: 5997
	public Transform joint;

	// Token: 0x0400176E RID: 5998
	public Transform end;

	// Token: 0x0400176F RID: 5999
	public Transform poleVector;

	// Token: 0x04001770 RID: 6000
	public Transform upVector;

	// Token: 0x04001771 RID: 6001
	public float blendPct = 1f;

	// Token: 0x04001772 RID: 6002
	[HideInInspector]
	public Transform startXform;

	// Token: 0x04001773 RID: 6003
	[HideInInspector]
	public Transform jointXform;

	// Token: 0x04001774 RID: 6004
	[HideInInspector]
	public Transform endXform;
}
