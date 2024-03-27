using System;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class TransformTargetFollower : MonoBehaviour
{
	// Token: 0x06000EC4 RID: 3780 RVA: 0x00082214 File Offset: 0x00080614
	private void Update()
	{
		bool flag = false;
		Vector3? vector = this.targetPosition;
		if (vector != null && base.transform.localPosition != this.targetPosition)
		{
			Transform transform = base.transform;
			Vector3 localPosition = base.transform.localPosition;
			Vector3? vector2 = this.targetPosition;
			transform.localPosition = Vector3.Lerp(localPosition, vector2.Value, this.speed);
			flag = true;
		}
		Vector3? vector3 = this.targetRotation;
		if (vector3 != null && base.transform.localEulerAngles != this.targetRotation)
		{
			Transform transform2 = base.transform;
			Vector3 localEulerAngles = base.transform.localEulerAngles;
			Vector3? vector4 = this.targetRotation;
			transform2.localEulerAngles = Vector3.Lerp(localEulerAngles, vector4.Value, this.speed);
			flag = true;
		}
		Vector3? vector5 = this.targetScale;
		if (vector5 != null && base.transform.localScale != this.targetScale)
		{
			Transform transform3 = base.transform;
			Vector3 localScale = base.transform.localScale;
			Vector3? vector6 = this.targetScale;
			transform3.localScale = Vector3.Lerp(localScale, vector6.Value, this.speed);
			flag = true;
		}
		if (!flag)
		{
			global::UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x04000F91 RID: 3985
	public float speed = 0.25f;

	// Token: 0x04000F92 RID: 3986
	public Vector3? targetPosition;

	// Token: 0x04000F93 RID: 3987
	public Vector3? targetRotation;

	// Token: 0x04000F94 RID: 3988
	public Vector3? targetScale;
}
