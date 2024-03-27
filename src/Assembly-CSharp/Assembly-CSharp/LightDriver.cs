using System;
using UnityEngine;

// Token: 0x02000209 RID: 521
[RequireComponent(typeof(SkyDriver))]
[RequireComponent(typeof(Light))]
public class LightDriver : MonoBehaviour
{
	// Token: 0x0600158F RID: 5519 RVA: 0x000BEC0B File Offset: 0x000BD00B
	private void Start()
	{
		this.time = this.startTime;
	}

	// Token: 0x06001590 RID: 5520 RVA: 0x000BEC19 File Offset: 0x000BD019
	public void setSpeed(float v)
	{
		this.timeSpeed = v;
	}

	// Token: 0x06001591 RID: 5521 RVA: 0x000BEC24 File Offset: 0x000BD024
	private void Update()
	{
		if (this.timeSpeed != 0f)
		{
			this.time += this.timeSpeed * Time.deltaTime;
			if (this.time > 1f)
			{
				this.time -= 1f;
			}
			base.transform.eulerAngles = Vector3.right * (this.time - 0.25f) * 360f;
			base.GetComponent<Light>().color = this.sunColor.Evaluate(this.time);
			base.GetComponent<Light>().intensity = this.intensity.Evaluate(this.time);
			base.GetComponent<SkyDriver>().starOffset = Vector2.up * this.time * this.starsSpeed;
		}
	}

	// Token: 0x040012B5 RID: 4789
	public Gradient sunColor;

	// Token: 0x040012B6 RID: 4790
	public AnimationCurve intensity;

	// Token: 0x040012B7 RID: 4791
	public float startTime;

	// Token: 0x040012B8 RID: 4792
	public float timeSpeed;

	// Token: 0x040012B9 RID: 4793
	public float starsSpeed;

	// Token: 0x040012BA RID: 4794
	[HideInInspector]
	public float time;
}
