using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000100 RID: 256
public class AreaFiltersDialog : Dialog
{
	// Token: 0x06000902 RID: 2306 RVA: 0x00036618 File Offset: 0x00034A18
	public void Start()
	{
		base.Init(base.gameObject, false, false, false);
		base.AddFundament();
		base.AddBackButton();
		base.AddCloseButton();
		base.AddHeadline("Filters", -370, -460, TextColor.Default, TextAlignment.Left, false);
		this.AddSliders();
		if (!Managers.optimizationManager.extraEffectsEvenInVR)
		{
			string text = "While this dialog shows, edge shade & bloom show in VR too";
			base.AddLabel(text, 0, 425, 0.625f, false, TextColor.Gray, false, TextAlignment.Center, -1, 1f, false, TextAnchor.MiddleLeft);
		}
		Managers.filterManager.ApplySettings(true);
		base.AddDefaultPagingButtons(80, 350, "Page", false, 0, 0.85f, false);
		this.AddBacksideButtons();
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x000366C5 File Offset: 0x00034AC5
	private void AddSliders()
	{
		base.StartCoroutine(this.DoAddSliders());
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x000366D4 File Offset: 0x00034AD4
	private IEnumerator DoAddSliders()
	{
		if (this.wrapper != null)
		{
			global::UnityEngine.Object.Destroy(this.wrapper);
			yield return false;
		}
		this.wrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.wrapper);
		int y = -270;
		if (this.page == 0)
		{
			int num = y;
			float num2 = Managers.filterManager.bloom;
			this.bloomSlider = base.AddSlider("Light Bloom", string.Empty, 0, num, 0f, 100f, true, num2, new Action<float>(this.OnBloomChange), false, 0.85f);
			y += 150;
			float num3 = Managers.filterManager.saturation * 0.5f + 50f;
			num = y;
			num2 = num3;
			this.saturationSlider = base.AddSlider("Saturation", string.Empty, 0, num, 0f, 100f, true, num2, new Action<float>(this.OnSaturationChange), false, 0.85f);
			y += 150;
			float num4 = Managers.filterManager.contrast * 1f;
			num = y;
			num2 = num4;
			this.contrastSlider = base.AddSlider("Contrast", string.Empty, 0, num, 0f, 100f, true, num2, new Action<float>(this.OnContrastChange), false, 0.85f);
			y += 150;
			float num5 = Managers.filterManager.brightness * 13.333333f + 50f;
			num = y;
			num2 = num5;
			this.brightnessSlider = base.AddSlider("Brightness", string.Empty, 0, num, 0f, 100f, true, num2, new Action<float>(this.OnBrightnessChange), false, 0.85f);
			y += 150;
		}
		else if (this.page == 1)
		{
			float num6 = Managers.filterManager.colorReduction * 20f;
			int num = y;
			float num2 = num6;
			this.colorReductionSlider = base.AddSlider("Color Groups", string.Empty, 0, num, 0f, 100f, true, num2, new Action<float>(this.OnColorReductionChange), false, 0.85f);
			y += 150;
			float num7 = Managers.filterManager.hueShift * 0.5555556f + 50f;
			num = y;
			num2 = num7;
			this.hueShiftSlider = base.AddSlider("Hue shift", string.Empty, 0, num, 0f, 100f, true, num2, new Action<float>(this.OnHueShiftChange), false, 0.85f);
			y += 150;
			float num8 = Managers.filterManager.distanceTint * 100f;
			num = y;
			num2 = num8;
			this.distanceTintSlider = base.AddSlider("Distance tint", string.Empty, 0, num, 0f, 100f, true, num2, new Action<float>(this.OnDistanceTintChange), false, 0.85f);
			y += 150;
			float num9 = Managers.filterManager.inversion * 100f;
			num = y;
			num2 = num9;
			this.inversionSlider = base.AddSlider("Inversion", string.Empty, 0, num, 0f, 100f, true, num2, new Action<float>(this.OnInversionChange), false, 0.85f);
			y += 150;
		}
		else if (this.page == 2)
		{
			float num10 = Managers.filterManager.heatMap * 100f;
			int num = y;
			float num2 = num10;
			this.heatMapSlider = base.AddSlider("Heat Vision", string.Empty, 0, num, 0f, 100f, true, num2, new Action<float>(this.OnHeatMapChange), false, 0.85f);
			y += 150;
		}
		this.UpdateSliderSuffixes();
		base.SetUiWrapper(base.gameObject);
		yield break;
	}

	// Token: 0x06000905 RID: 2309 RVA: 0x000366EF File Offset: 0x00034AEF
	private void OnBloomChange(float value)
	{
		Managers.filterManager.bloom = value;
		this.FinalizeValueChange();
	}

	// Token: 0x06000906 RID: 2310 RVA: 0x00036702 File Offset: 0x00034B02
	private void OnSaturationChange(float value)
	{
		Managers.filterManager.saturation = (value - 50f) * 2f;
		this.FinalizeValueChange();
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x00036721 File Offset: 0x00034B21
	private void OnContrastChange(float value)
	{
		Managers.filterManager.contrast = value * 1f;
		this.FinalizeValueChange();
	}

	// Token: 0x06000908 RID: 2312 RVA: 0x0003673A File Offset: 0x00034B3A
	private void OnBrightnessChange(float value)
	{
		Managers.filterManager.brightness = (value - 50f) * 0.075f;
		this.FinalizeValueChange();
	}

	// Token: 0x06000909 RID: 2313 RVA: 0x00036759 File Offset: 0x00034B59
	private void OnColorReductionChange(float value)
	{
		Managers.filterManager.colorReduction = value * 0.05f;
		this.FinalizeValueChange();
	}

	// Token: 0x0600090A RID: 2314 RVA: 0x00036772 File Offset: 0x00034B72
	private void OnHueShiftChange(float value)
	{
		Managers.filterManager.hueShift = (value - 50f) * 1.8f;
		this.FinalizeValueChange();
	}

	// Token: 0x0600090B RID: 2315 RVA: 0x00036791 File Offset: 0x00034B91
	private void OnDistanceTintChange(float value)
	{
		Managers.filterManager.distanceTint = value * 0.01f;
		this.FinalizeValueChange();
	}

	// Token: 0x0600090C RID: 2316 RVA: 0x000367AA File Offset: 0x00034BAA
	private void OnInversionChange(float value)
	{
		Managers.filterManager.inversion = value * 0.01f;
		this.FinalizeValueChange();
	}

	// Token: 0x0600090D RID: 2317 RVA: 0x000367C3 File Offset: 0x00034BC3
	private void OnHeatMapChange(float value)
	{
		Managers.filterManager.heatMap = value * 0.01f;
		this.FinalizeValueChange();
	}

	// Token: 0x0600090E RID: 2318 RVA: 0x000367DC File Offset: 0x00034BDC
	private void FinalizeValueChange()
	{
		Managers.filterManager.ApplySettings(true);
		this.UpdateSliderSuffixes();
		base.CancelInvoke("SyncToOthers");
		base.Invoke("SyncToOthers", 0.25f);
		base.CancelInvoke("SaveSettingsToServer");
		base.Invoke("SaveSettingsToServer", 2.5f);
	}

	// Token: 0x0600090F RID: 2319 RVA: 0x00036830 File Offset: 0x00034C30
	private void SaveSettingsToServer()
	{
		Managers.areaManager.SaveSettings(delegate(bool success)
		{
		});
	}

	// Token: 0x06000910 RID: 2320 RVA: 0x00036859 File Offset: 0x00034C59
	private void SyncToOthers()
	{
		Managers.personManager.DoUpdateViaAreaSettingsJson();
	}

	// Token: 0x06000911 RID: 2321 RVA: 0x00036868 File Offset: 0x00034C68
	private void UpdateSliderSuffixes()
	{
		if (this.bloomSlider != null)
		{
			this.bloomSlider.valueSuffix = ((Managers.filterManager.bloom != 5f) ? string.Empty : " (Default)".ToUpper());
		}
		if (this.saturationSlider != null)
		{
			this.saturationSlider.valueSuffix = ((Managers.filterManager.saturation != 0f) ? string.Empty : " (Default)".ToUpper());
		}
		if (this.contrastSlider != null)
		{
			this.contrastSlider.valueSuffix = ((Managers.filterManager.contrast != 0f) ? string.Empty : " (Default)".ToUpper());
		}
		if (this.brightnessSlider != null)
		{
			this.brightnessSlider.valueSuffix = ((Managers.filterManager.brightness != 0f) ? string.Empty : " (Default)".ToUpper());
		}
		if (this.colorReductionSlider != null)
		{
			this.colorReductionSlider.valueSuffix = ((Managers.filterManager.colorReduction != 0f) ? string.Empty : " (Default)".ToUpper());
		}
		if (this.hueShiftSlider != null)
		{
			this.hueShiftSlider.valueSuffix = ((Managers.filterManager.hueShift != 0f) ? string.Empty : " (Default)".ToUpper());
		}
		if (this.distanceTintSlider != null)
		{
			this.distanceTintSlider.valueSuffix = ((Managers.filterManager.distanceTint != 0f) ? string.Empty : " (Default)".ToUpper());
		}
		if (this.inversionSlider != null)
		{
			this.inversionSlider.valueSuffix = ((Managers.filterManager.inversion != 0f) ? string.Empty : " (Default)".ToUpper());
		}
		if (this.heatMapSlider != null)
		{
			this.heatMapSlider.valueSuffix = ((Managers.filterManager.heatMap != 0f) ? string.Empty : " (Default)".ToUpper());
		}
	}

	// Token: 0x06000912 RID: 2322 RVA: 0x00036ADC File Offset: 0x00034EDC
	private void AddBacksideButtons()
	{
		this.backsideWrapper = base.GetUiWrapper();
		base.SetUiWrapper(this.backsideWrapper);
		base.AddButton("resetAll", null, "Reset all", "ButtonCompactNoIcon", 0, 0, null, false, 1f, TextColor.Default, 1f, 0f, 0f, string.Empty, false, false, TextAlignment.Left, false, false);
		base.RotateBacksideWrapper();
		base.SetUiWrapper(base.gameObject);
	}

	// Token: 0x06000913 RID: 2323 RVA: 0x00036B4D File Offset: 0x00034F4D
	private void OnDestroy()
	{
		Managers.filterManager.ApplySettings(false);
		base.CancelInvoke("SaveSettingsToServer");
		this.SaveSettingsToServer();
	}

	// Token: 0x06000914 RID: 2324 RVA: 0x00036B6C File Offset: 0x00034F6C
	private void ResetAll()
	{
		base.CancelInvoke("SyncToOthers");
		Managers.filterManager.SetToDefaults();
		Managers.filterManager.ApplySettings(false);
		Managers.soundManager.Play("success", null, 0.15f, false, false);
		this.SyncToOthers();
		this.AddSliders();
	}

	// Token: 0x06000915 RID: 2325 RVA: 0x00036BBC File Offset: 0x00034FBC
	public override void OnClick(string contextName, string contextId, bool state, GameObject thisButton)
	{
		if (contextName != null)
		{
			if (!(contextName == "previousPage"))
			{
				if (!(contextName == "nextPage"))
				{
					if (!(contextName == "resetAll"))
					{
						if (!(contextName == "back"))
						{
							if (contextName == "close")
							{
								base.CloseDialog();
							}
						}
						else
						{
							base.SwitchTo(DialogType.AreaAttributes, string.Empty);
						}
					}
					else if (!CrossDevice.desktopMode || Managers.desktopManager.showDialogBackside)
					{
						this.ResetAll();
					}
				}
				else
				{
					if (++this.page > 2)
					{
						this.page = 0;
					}
					this.AddSliders();
				}
			}
			else
			{
				if (--this.page < 0)
				{
					this.page = 2;
				}
				this.AddSliders();
			}
		}
	}

	// Token: 0x040006C0 RID: 1728
	private const float saturationOffset = 50f;

	// Token: 0x040006C1 RID: 1729
	private const float saturationFactor = 2f;

	// Token: 0x040006C2 RID: 1730
	private const float contrastOffset = 0f;

	// Token: 0x040006C3 RID: 1731
	private const float contrastFactor = 1f;

	// Token: 0x040006C4 RID: 1732
	private const float brightnessOffset = 50f;

	// Token: 0x040006C5 RID: 1733
	private const float brightnessFactor = 0.075f;

	// Token: 0x040006C6 RID: 1734
	private const float colorReductionOffset = 0f;

	// Token: 0x040006C7 RID: 1735
	private const float colorReductionFactor = 0.05f;

	// Token: 0x040006C8 RID: 1736
	private const float hueShiftOffset = 50f;

	// Token: 0x040006C9 RID: 1737
	private const float hueShiftFactor = 1.8f;

	// Token: 0x040006CA RID: 1738
	private const float distanceTintOffset = 0f;

	// Token: 0x040006CB RID: 1739
	private const float distanceTintFactor = 0.01f;

	// Token: 0x040006CC RID: 1740
	private const float inversionOffset = 0f;

	// Token: 0x040006CD RID: 1741
	private const float inversionFactor = 0.01f;

	// Token: 0x040006CE RID: 1742
	private const float heatMapOffset = 0f;

	// Token: 0x040006CF RID: 1743
	private const float heatMapFactor = 0.01f;

	// Token: 0x040006D0 RID: 1744
	private DialogSlider bloomSlider;

	// Token: 0x040006D1 RID: 1745
	private DialogSlider saturationSlider;

	// Token: 0x040006D2 RID: 1746
	private DialogSlider contrastSlider;

	// Token: 0x040006D3 RID: 1747
	private DialogSlider brightnessSlider;

	// Token: 0x040006D4 RID: 1748
	private DialogSlider colorReductionSlider;

	// Token: 0x040006D5 RID: 1749
	private DialogSlider hueShiftSlider;

	// Token: 0x040006D6 RID: 1750
	private DialogSlider distanceTintSlider;

	// Token: 0x040006D7 RID: 1751
	private DialogSlider inversionSlider;

	// Token: 0x040006D8 RID: 1752
	private DialogSlider heatMapSlider;

	// Token: 0x040006D9 RID: 1753
	private GameObject resetButton;

	// Token: 0x040006DA RID: 1754
	private const string suffixIfDefault = " (Default)";

	// Token: 0x040006DB RID: 1755
	private int page;

	// Token: 0x040006DC RID: 1756
	private const int maxPages = 3;
}
