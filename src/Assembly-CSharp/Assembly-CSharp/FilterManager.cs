using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x020001EE RID: 494
public class FilterManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x060010EC RID: 4332 RVA: 0x000924D7 File Offset: 0x000908D7
	// (set) Token: 0x060010ED RID: 4333 RVA: 0x000924DF File Offset: 0x000908DF
	public ManagerStatus status { get; private set; }

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x060010EE RID: 4334 RVA: 0x000924E8 File Offset: 0x000908E8
	// (set) Token: 0x060010EF RID: 4335 RVA: 0x000924F0 File Offset: 0x000908F0
	public string failMessage { get; private set; }

	// Token: 0x060010F0 RID: 4336 RVA: 0x000924FC File Offset: 0x000908FC
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		PostProcessVolume component = base.gameObject.GetComponent<PostProcessVolume>();
		component.profile.TryGetSettings<Bloom>(out this.bloomLayer);
		component.profile.TryGetSettings<AmbientOcclusion>(out this.ambientOcclusionLayer);
		component.profile.TryGetSettings<ColorGrading>(out this.colorGradingLayer);
		component.profile.TryGetSettings<ColorReductionFilter>(out this.colorReductionLayer);
		component.profile.TryGetSettings<DistanceTintFilter>(out this.distanceTintLayer);
		component.profile.TryGetSettings<InversionFilter>(out this.inversionLayer);
		component.profile.TryGetSettings<HeatMapFilter>(out this.heatMapLayer);
		this.SetToDefaults();
		this.ApplySettings(false);
		this.status = ManagerStatus.Started;
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x000925B0 File Offset: 0x000909B0
	public void SetToDefaults()
	{
		this.bloom = 5f;
		this.saturation = 0f;
		this.contrast = 0f;
		this.brightness = 0f;
		this.colorReduction = 0f;
		this.hueShift = 0f;
		this.distanceTint = 0f;
		this.inversion = 0f;
		this.heatMap = 0f;
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x00092620 File Offset: 0x00090A20
	public void ApplySettings(bool temporarilyForceExtraEffectsEvenInVR = false)
	{
		bool doOptimizeSpeed = Managers.optimizationManager.doOptimizeSpeed;
		bool flag = false;
		bool flag2 = false;
		if ((Managers.optimizationManager != null && Managers.optimizationManager.extraEffectsEvenInVR) || temporarilyForceExtraEffectsEvenInVR)
		{
			flag = true;
			flag2 = true;
		}
		if ((CrossDevice.desktopMode || flag) && !doOptimizeSpeed)
		{
			this.ambientOcclusionLayer.enabled.value = true;
		}
		else
		{
			this.ambientOcclusionLayer.enabled.value = false;
		}
		if (this.bloom > 0f && (CrossDevice.desktopMode || flag2) && !doOptimizeSpeed)
		{
			this.bloomLayer.enabled.value = true;
			this.bloomLayer.intensity.value = this.bloom * 0.25f;
		}
		else
		{
			this.bloomLayer.enabled.value = false;
		}
		if (this.saturation != 0f || this.contrast != 0f || this.hueShift != 0f || this.brightness != 0f)
		{
			this.colorGradingLayer.enabled.value = true;
			this.colorGradingLayer.saturation.value = this.saturation;
			this.colorGradingLayer.contrast.value = this.contrast;
			this.colorGradingLayer.hueShift.value = this.hueShift;
			this.colorGradingLayer.postExposure.value = this.brightness;
		}
		else
		{
			this.colorGradingLayer.enabled.value = false;
		}
		if (this.colorReduction > 0f)
		{
			this.colorReductionLayer.enabled.value = true;
			this.colorReductionLayer.intensity.value = this.colorReduction;
		}
		else
		{
			this.colorReductionLayer.enabled.value = false;
		}
		if (this.distanceTint > 0f)
		{
			this.distanceTintLayer.enabled.value = true;
			this.distanceTintLayer.intensity.value = this.distanceTint;
		}
		else
		{
			this.distanceTintLayer.enabled.value = false;
		}
		if (this.inversion > 0f)
		{
			this.inversionLayer.enabled.value = true;
			this.inversionLayer.intensity.value = this.inversion;
		}
		else
		{
			this.inversionLayer.enabled.value = false;
		}
		if (this.heatMap > 0f)
		{
			this.heatMapLayer.enabled.value = true;
			this.heatMapLayer.intensity.value = this.heatMap;
		}
		else
		{
			this.heatMapLayer.enabled.value = false;
		}
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x00092900 File Offset: 0x00090D00
	public string GetJson()
	{
		string text = string.Empty;
		if (this.bloom != 5f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				",\"",
				this.jsonKey[FilterType.Bloom],
				"\":",
				this.bloom
			});
		}
		if (this.saturation != 0f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				",\"",
				this.jsonKey[FilterType.Saturation],
				"\":",
				this.saturation
			});
		}
		if (this.contrast != 0f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				",\"",
				this.jsonKey[FilterType.Contrast],
				"\":",
				this.contrast
			});
		}
		if (this.brightness != 0f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				",\"",
				this.jsonKey[FilterType.Brightness],
				"\":",
				this.brightness
			});
		}
		if (this.colorReduction != 0f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				",\"",
				this.jsonKey[FilterType.ColorReduction],
				"\":",
				this.colorReduction
			});
		}
		if (this.hueShift != 0f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				",\"",
				this.jsonKey[FilterType.HueShift],
				"\":",
				this.hueShift
			});
		}
		if (this.distanceTint != 0f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				",\"",
				this.jsonKey[FilterType.DistanceTint],
				"\":",
				this.distanceTint
			});
		}
		if (this.inversion != 0f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				",\"",
				this.jsonKey[FilterType.Inversion],
				"\":",
				this.inversion
			});
		}
		if (this.heatMap != 0f)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				",\"",
				this.jsonKey[FilterType.HeatMap],
				"\":",
				this.heatMap
			});
		}
		if (text != string.Empty)
		{
			text = text.TrimStart(new char[] { ',' });
			text = "{" + text + "}";
		}
		return text;
	}

	// Token: 0x060010F4 RID: 4340 RVA: 0x00092C10 File Offset: 0x00091010
	public void SetByJson(JSONNode node)
	{
		if (node != null)
		{
			if (node[this.jsonKey[FilterType.Bloom]] != null)
			{
				this.bloom = node[this.jsonKey[FilterType.Bloom]].AsFloat;
			}
			if (node[this.jsonKey[FilterType.Saturation]] != null)
			{
				this.saturation = node[this.jsonKey[FilterType.Saturation]].AsFloat;
			}
			if (node[this.jsonKey[FilterType.Contrast]] != null)
			{
				this.contrast = node[this.jsonKey[FilterType.Contrast]].AsFloat;
			}
			if (node[this.jsonKey[FilterType.Brightness]] != null)
			{
				this.brightness = node[this.jsonKey[FilterType.Brightness]].AsFloat;
			}
			if (node[this.jsonKey[FilterType.ColorReduction]] != null)
			{
				this.colorReduction = node[this.jsonKey[FilterType.ColorReduction]].AsFloat;
			}
			if (node[this.jsonKey[FilterType.HueShift]] != null)
			{
				this.hueShift = node[this.jsonKey[FilterType.HueShift]].AsFloat;
			}
			if (node[this.jsonKey[FilterType.DistanceTint]] != null)
			{
				this.distanceTint = node[this.jsonKey[FilterType.DistanceTint]].AsFloat;
			}
			if (node[this.jsonKey[FilterType.Inversion]] != null)
			{
				this.inversion = node[this.jsonKey[FilterType.Inversion]].AsFloat;
			}
			if (node[this.jsonKey[FilterType.HeatMap]] != null)
			{
				this.heatMap = node[this.jsonKey[FilterType.HeatMap]].AsFloat;
			}
		}
	}

	// Token: 0x040010DC RID: 4316
	public const float defaultBloom = 5f;

	// Token: 0x040010DD RID: 4317
	public const float defaultSaturation = 0f;

	// Token: 0x040010DE RID: 4318
	public const float defaultContrast = 0f;

	// Token: 0x040010DF RID: 4319
	public const float defaultBrightness = 0f;

	// Token: 0x040010E0 RID: 4320
	public const float defaultColorReduction = 0f;

	// Token: 0x040010E1 RID: 4321
	public const float defaultHueShift = 0f;

	// Token: 0x040010E2 RID: 4322
	public const float defaultDistanceTint = 0f;

	// Token: 0x040010E3 RID: 4323
	public const float defaultInversion = 0f;

	// Token: 0x040010E4 RID: 4324
	public const float defaultHeatMap = 0f;

	// Token: 0x040010E5 RID: 4325
	public float bloom = 5f;

	// Token: 0x040010E6 RID: 4326
	public float saturation;

	// Token: 0x040010E7 RID: 4327
	public float contrast;

	// Token: 0x040010E8 RID: 4328
	public float brightness;

	// Token: 0x040010E9 RID: 4329
	public float colorReduction;

	// Token: 0x040010EA RID: 4330
	public float hueShift;

	// Token: 0x040010EB RID: 4331
	public float distanceTint;

	// Token: 0x040010EC RID: 4332
	public float inversion;

	// Token: 0x040010ED RID: 4333
	public float heatMap;

	// Token: 0x040010EE RID: 4334
	private AmbientOcclusion ambientOcclusionLayer;

	// Token: 0x040010EF RID: 4335
	private Bloom bloomLayer;

	// Token: 0x040010F0 RID: 4336
	private ColorGrading colorGradingLayer;

	// Token: 0x040010F1 RID: 4337
	private ColorReductionFilter colorReductionLayer;

	// Token: 0x040010F2 RID: 4338
	private DistanceTintFilter distanceTintLayer;

	// Token: 0x040010F3 RID: 4339
	private InversionFilter inversionLayer;

	// Token: 0x040010F4 RID: 4340
	private HeatMapFilter heatMapLayer;

	// Token: 0x040010F5 RID: 4341
	private Dictionary<FilterType, string> jsonKey = new Dictionary<FilterType, string>
	{
		{
			FilterType.Bloom,
			"bl"
		},
		{
			FilterType.Saturation,
			"sa"
		},
		{
			FilterType.Contrast,
			"co"
		},
		{
			FilterType.Brightness,
			"br"
		},
		{
			FilterType.ColorReduction,
			"cr"
		},
		{
			FilterType.HueShift,
			"hs"
		},
		{
			FilterType.DistanceTint,
			"dt"
		},
		{
			FilterType.Inversion,
			"iv"
		},
		{
			FilterType.HeatMap,
			"hm"
		}
	};
}
