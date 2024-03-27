using System;
using UnityEngine;

// Token: 0x02000262 RID: 610
public class PartTrail : MonoBehaviour
{
	// Token: 0x06001682 RID: 5762 RVA: 0x000CA700 File Offset: 0x000C8B00
	private void Start()
	{
		this.trailObject = global::UnityEngine.Object.Instantiate<GameObject>(Resources.Load("Prefabs/Trail") as GameObject, Vector3.zero, Quaternion.identity);
		this.trailObject.transform.parent = base.transform;
		this.trailObject.transform.localPosition = Vector3.zero;
		this.trailObject.transform.localRotation = Quaternion.identity;
		this.trailObject.name = Misc.RemoveCloneFromName(this.trailObject.name);
		Renderer component = base.gameObject.GetComponent<Renderer>();
		TrailRenderer component2 = this.trailObject.GetComponent<TrailRenderer>();
		component2.materials = component.sharedMaterials;
		component2.time = this.settings.durationSeconds;
		AnimationCurve animationCurve = new AnimationCurve();
		float num = Misc.GetLargestValueOfVector(base.transform.localScale) * 10f;
		animationCurve.AddKey(0f, (!this.settings.thickEnd) ? 0f : num);
		animationCurve.AddKey(0.5f, num);
		animationCurve.AddKey(1f, (!this.settings.thickStart) ? 0f : num);
		component2.widthCurve = animationCurve;
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x000CA840 File Offset: 0x000C8C40
	private void OnDestroy()
	{
		if (this.trailObject != null)
		{
			this.trailObject.transform.parent = Effects.effectsContainerTransform;
		}
	}

	// Token: 0x04001439 RID: 5177
	public PartTrailSettings settings = new PartTrailSettings();

	// Token: 0x0400143A RID: 5178
	private GameObject trailObject;
}
