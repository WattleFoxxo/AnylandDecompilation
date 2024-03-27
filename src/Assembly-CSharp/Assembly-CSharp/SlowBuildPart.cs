using System;
using UnityEngine;

// Token: 0x02000268 RID: 616
public class SlowBuildPart : MonoBehaviour
{
	// Token: 0x06001696 RID: 5782 RVA: 0x000CAF5C File Offset: 0x000C935C
	public void Init(int index, float startDelaySeconds, float durationSeconds)
	{
		this.durationSeconds = durationSeconds;
		this.SetTransformStartAndTarget();
		float num = startDelaySeconds + (float)index * (durationSeconds * 0.5f);
		base.Invoke("StartAnimation", num);
		this.SetRenderer(false);
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x000CAF96 File Offset: 0x000C9396
	public void StartAnimation()
	{
		this.SetRenderer(true);
		this.targetTime = Time.time + this.durationSeconds;
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x000CAFB1 File Offset: 0x000C93B1
	private void Update()
	{
		if (this.targetTime != -1f)
		{
			this.TransformTowardTarget();
		}
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x000CAFCC File Offset: 0x000C93CC
	private void SetRenderer(bool enabled)
	{
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			component.enabled = enabled;
		}
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x000CAFF4 File Offset: 0x000C93F4
	private void TransformTowardTarget()
	{
		float num = this.targetTime - Time.time;
		float num2 = 1f - num / this.durationSeconds;
		num2 = Mathfx.EaseOut(0f, 1f, num2);
		num2 = Mathf.Clamp(num2, 0f, 1f);
		base.transform.localPosition = Vector3.Lerp(this.startPosition, this.targetPosition, num2);
		Quaternion quaternion = Quaternion.Euler(this.startRotation.x, this.startRotation.y, this.startRotation.z);
		Quaternion quaternion2 = Quaternion.Euler(this.targetRotation.x, this.targetRotation.y, this.targetRotation.z);
		base.transform.localRotation = Quaternion.Lerp(quaternion, quaternion2, num2);
		base.transform.localScale = Vector3.Lerp(this.startScale, this.targetScale, num2);
		if (num <= 0f)
		{
			this.DidHitTarget();
		}
	}

	// Token: 0x0600169B RID: 5787 RVA: 0x000CB0EC File Offset: 0x000C94EC
	private void DidHitTarget()
	{
		base.transform.localPosition = this.targetPosition;
		base.transform.localEulerAngles = this.targetRotation;
		base.transform.localScale = this.targetScale;
		this.targetTime = -1f;
		if (this.thingToRestoreAfter != null)
		{
			global::UnityEngine.Object.Destroy(base.transform.parent.gameObject);
		}
	}

	// Token: 0x0600169C RID: 5788 RVA: 0x000CB160 File Offset: 0x000C9560
	private void OnDestroy()
	{
		if (this.thingToRestoreAfter != null && Managers.thingManager != null && Managers.thingManager.placements != null)
		{
			this.thingToRestoreAfter.transform.parent = Managers.thingManager.placements.transform;
			this.thingToRestoreAfter.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600169D RID: 5789 RVA: 0x000CB1D4 File Offset: 0x000C95D4
	private void SetTransformStartAndTarget()
	{
		this.targetPosition = base.transform.localPosition;
		this.targetRotation = base.transform.localEulerAngles;
		this.targetScale = base.transform.localScale;
		base.transform.localPosition = global::UnityEngine.Random.insideUnitSphere.normalized * 1.5f;
		base.transform.localEulerAngles = Misc.GetRandomVector3(360f);
		base.transform.localScale = Vector3.one * 0.01f;
		this.startPosition = base.transform.localPosition;
		this.startRotation = base.transform.localEulerAngles;
		this.startScale = base.transform.localScale;
	}

	// Token: 0x0400144E RID: 5198
	private Vector3 startPosition;

	// Token: 0x0400144F RID: 5199
	private Vector3 startRotation;

	// Token: 0x04001450 RID: 5200
	private Vector3 startScale;

	// Token: 0x04001451 RID: 5201
	private Vector3 targetPosition;

	// Token: 0x04001452 RID: 5202
	private Vector3 targetRotation;

	// Token: 0x04001453 RID: 5203
	private Vector3 targetScale;

	// Token: 0x04001454 RID: 5204
	private float targetTime = -1f;

	// Token: 0x04001455 RID: 5205
	private float durationSeconds;

	// Token: 0x04001456 RID: 5206
	public Thing thingToRestoreAfter;
}
