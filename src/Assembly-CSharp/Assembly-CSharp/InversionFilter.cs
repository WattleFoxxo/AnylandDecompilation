using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x020001A4 RID: 420
[PostProcess(typeof(InversionRenderer), PostProcessEvent.AfterStack, "Filter/Inversion", true)]
[Serializable]
public sealed class InversionFilter : PostProcessEffectSettings
{
	// Token: 0x06000D1F RID: 3359 RVA: 0x00075DFF File Offset: 0x000741FF
	public override bool IsEnabledAndSupported(PostProcessRenderContext context)
	{
		return this.enabled.value && this.intensity.value > 0f;
	}

	// Token: 0x04000EC6 RID: 3782
	[Range(0f, 1f)]
	public FloatParameter intensity = new FloatParameter
	{
		value = 0f
	};
}
