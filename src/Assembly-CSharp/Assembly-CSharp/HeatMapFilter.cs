using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x020001A2 RID: 418
[PostProcess(typeof(HeatMapRenderer), PostProcessEvent.AfterStack, "Filter/HeatMap", true)]
[Serializable]
public sealed class HeatMapFilter : PostProcessEffectSettings
{
	// Token: 0x06000D1B RID: 3355 RVA: 0x00075D43 File Offset: 0x00074143
	public override bool IsEnabledAndSupported(PostProcessRenderContext context)
	{
		return this.enabled.value && this.intensity.value > 0f;
	}

	// Token: 0x04000EC5 RID: 3781
	[Range(0f, 1f)]
	public FloatParameter intensity = new FloatParameter
	{
		value = 0f
	};
}
