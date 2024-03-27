using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200019E RID: 414
[PostProcess(typeof(ColorReductionRenderer), PostProcessEvent.AfterStack, "Filter/ColorReduction", true)]
[Serializable]
public sealed class ColorReductionFilter : PostProcessEffectSettings
{
	// Token: 0x06000D13 RID: 3347 RVA: 0x00075BCB File Offset: 0x00073FCB
	public override bool IsEnabledAndSupported(PostProcessRenderContext context)
	{
		return this.enabled.value && this.intensity.value > 0f;
	}

	// Token: 0x04000EC3 RID: 3779
	[Range(0f, 10f)]
	public FloatParameter intensity = new FloatParameter
	{
		value = 0f
	};
}
