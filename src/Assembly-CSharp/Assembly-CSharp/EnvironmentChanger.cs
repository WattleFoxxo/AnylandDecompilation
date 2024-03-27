using System;
using UnityEngine;

// Token: 0x02000199 RID: 409
public class EnvironmentChanger : MonoBehaviour
{
	// Token: 0x06000D02 RID: 3330 RVA: 0x000753C0 File Offset: 0x000737C0
	private void Awake()
	{
		this.manager = (SkyDriver)this.environmentLight.GetComponent(typeof(SkyDriver));
		this.material = base.gameObject.GetComponent<Renderer>().material;
		this.environmentLightComponent = this.environmentLight.GetComponent<Light>();
		if (base.gameObject.name == "clouds")
		{
			this.cloudsParticleSystem = this.cloudsParticleSystemGameObject.GetComponent<ParticleSystem>();
		}
		this.SetToDefault();
	}

	// Token: 0x06000D03 RID: 3331 RVA: 0x00075448 File Offset: 0x00073848
	public void SetToDefault()
	{
		string name = base.gameObject.name;
		if (name != null)
		{
			if (!(name == "sun"))
			{
				if (!(name == "night"))
				{
					if (!(name == "ambientLight"))
					{
						if (!(name == "fog"))
						{
							if (name == "clouds")
							{
								float num = 0.01f;
								base.transform.localScale = new Vector3(num, num, num);
								this.material.color = Color.white;
							}
						}
						else
						{
							float num = 0.03f;
							base.transform.localScale = new Vector3(num, num, num);
							float num2 = 0.1f;
							this.material.color = new Color(num2, num2, num2);
						}
					}
					else
					{
						float num = 0.1f;
						base.transform.localScale = new Vector3(num, num, num);
						this.material.color = new Color(0.1f, 0.099f, 0.07f);
					}
				}
				else
				{
					float num = 0.05f;
					base.transform.localScale = new Vector3(num, num, num);
					this.material.color = new Color(0.04f, 0.05f, 0.1f);
				}
			}
			else
			{
				float num = 0.05f;
				base.transform.localScale = new Vector3(num, num, num);
				base.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
				this.material.color = Color.white;
			}
		}
	}

	// Token: 0x06000D04 RID: 3332 RVA: 0x000755E8 File Offset: 0x000739E8
	private void Update()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		Vector3 localScale = base.transform.localScale;
		if (eulerAngles != this.lastAngles || localScale != this.lastScale || this.material.color != this.lastColor)
		{
			float num = 0.03f;
			float num2 = 0.15f;
			float num3 = Mathf.Clamp((localScale.x - num) / (num2 - num), 0f, 1f);
			Vector3 vector = eulerAngles;
			vector.x -= 180f;
			string name = base.gameObject.name;
			if (name != null)
			{
				if (!(name == "sun"))
				{
					if (!(name == "night"))
					{
						if (!(name == "ambientLight"))
						{
							if (!(name == "fog"))
							{
								if (name == "clouds")
								{
									this.SetClouds(num3, this.material.color);
								}
							}
							else if (num3 == 0f)
							{
								RenderSettings.fog = false;
							}
							else
							{
								RenderSettings.fog = true;
								RenderSettings.fogColor = this.material.color;
								RenderSettings.fogDensity = Mathf.Pow(num3, 3f);
							}
						}
						else
						{
							RenderSettings.ambientLight = this.material.color;
						}
					}
					else
					{
						this.manager.nightColor = this.material.color;
						this.manager.nightStrength = 2f - num3 * 2f;
					}
				}
				else
				{
					this.manager.sunSize = num3 * 5f;
					this.manager.tint = this.material.color;
					this.environmentLight.transform.eulerAngles = vector;
					this.environmentLightComponent.color = this.material.color;
				}
			}
			this.lastAngles = eulerAngles;
			this.lastScale = localScale;
			this.lastColor = this.material.color;
		}
	}

	// Token: 0x06000D05 RID: 3333 RVA: 0x00075814 File Offset: 0x00073C14
	private void SetClouds(float scale, Color color)
	{
		float num = scale * 2.65f;
		Gradient gradient = new Gradient();
		gradient.SetKeys(new GradientColorKey[]
		{
			new GradientColorKey(color, 0f),
			new GradientColorKey(color, 1f)
		}, new GradientAlphaKey[]
		{
			new GradientAlphaKey(num, 0f),
			new GradientAlphaKey(num, 1f)
		});
		this.cloudsParticleSystem.colorOverLifetime.color = gradient;
	}

	// Token: 0x04000EAE RID: 3758
	public GameObject environmentLight;

	// Token: 0x04000EAF RID: 3759
	public GameObject cloudsParticleSystemGameObject;

	// Token: 0x04000EB0 RID: 3760
	public const float scaleMin = 0.03f;

	// Token: 0x04000EB1 RID: 3761
	public const float scaleMax = 0.15f;

	// Token: 0x04000EB2 RID: 3762
	private SkyDriver manager;

	// Token: 0x04000EB3 RID: 3763
	private Material material;

	// Token: 0x04000EB4 RID: 3764
	private Light environmentLightComponent;

	// Token: 0x04000EB5 RID: 3765
	private Vector3 lastAngles;

	// Token: 0x04000EB6 RID: 3766
	private Vector3 lastScale;

	// Token: 0x04000EB7 RID: 3767
	private Color lastColor;

	// Token: 0x04000EB8 RID: 3768
	private ParticleSystem cloudsParticleSystem;
}
