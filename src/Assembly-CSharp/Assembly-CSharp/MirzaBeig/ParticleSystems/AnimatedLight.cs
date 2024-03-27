using System;
using UnityEngine;

namespace MirzaBeig.ParticleSystems
{
	// Token: 0x02000047 RID: 71
	[RequireComponent(typeof(Light))]
	[Serializable]
	public class AnimatedLight : MonoBehaviour
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x0000B802 File Offset: 0x00009C02
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x0000B80A File Offset: 0x00009C0A
		public float time { get; set; }

		// Token: 0x060002B2 RID: 690 RVA: 0x0000B813 File Offset: 0x00009C13
		private void Awake()
		{
			this.light = base.GetComponent<Light>();
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000B824 File Offset: 0x00009C24
		private void Start()
		{
			this.startColour = this.light.color;
			this.startIntensity = this.light.intensity;
			this.light.color = this.startColour * this.colourOverLifetime.Evaluate(0f);
			this.light.intensity = this.startIntensity * this.intensityOverLifetime.Evaluate(0f);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000B89B File Offset: 0x00009C9B
		private void OnEnable()
		{
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000B8A0 File Offset: 0x00009CA0
		private void OnDisable()
		{
			this.light.color = this.startColour;
			this.light.intensity = this.startIntensity;
			this.time = 0f;
			this.evaluating = true;
			this.light.color = this.startColour * this.colourOverLifetime.Evaluate(0f);
			this.light.intensity = this.startIntensity * this.intensityOverLifetime.Evaluate(0f);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000B92C File Offset: 0x00009D2C
		private void Update()
		{
			if (this.evaluating)
			{
				if (this.time < this.duration)
				{
					this.time += Time.deltaTime;
					if (this.time > this.duration)
					{
						if (this.autoDestruct)
						{
							global::UnityEngine.Object.Destroy(base.gameObject);
						}
						else if (this.loop)
						{
							this.time = 0f;
						}
						else
						{
							this.time = this.duration;
							this.evaluating = false;
						}
					}
				}
				if (this.time <= this.duration)
				{
					float num = this.time / this.duration;
					this.light.color = this.startColour * this.colourOverLifetime.Evaluate(num);
					this.light.intensity = this.startIntensity * this.intensityOverLifetime.Evaluate(num);
				}
			}
		}

		// Token: 0x0400017D RID: 381
		private Light light;

		// Token: 0x0400017F RID: 383
		public float duration = 1f;

		// Token: 0x04000180 RID: 384
		private bool evaluating = true;

		// Token: 0x04000181 RID: 385
		public Gradient colourOverLifetime;

		// Token: 0x04000182 RID: 386
		public AnimationCurve intensityOverLifetime = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000183 RID: 387
		public bool loop = true;

		// Token: 0x04000184 RID: 388
		public bool autoDestruct;

		// Token: 0x04000185 RID: 389
		private Color startColour;

		// Token: 0x04000186 RID: 390
		private float startIntensity;
	}
}
