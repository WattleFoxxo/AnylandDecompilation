using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x020001A0 RID: 416
[PostProcess(typeof(DistanceTintRenderer), PostProcessEvent.AfterStack, "Filter/DistanceTint", true)]
[Serializable]
public sealed class DistanceTintFilter : PostProcessEffectSettings
{
	// Token: 0x06000D17 RID: 3351 RVA: 0x00075C87 File Offset: 0x00074087
	public override bool IsEnabledAndSupported(PostProcessRenderContext context)
	{
		return this.enabled.value && this.intensity.value > 0f;
	}

	// Token: 0x04000EC4 RID: 3780
	[Range(0f, 1f)]
	public FloatParameter intensity = new FloatParameter
	{
		value = 0f
	};
}
