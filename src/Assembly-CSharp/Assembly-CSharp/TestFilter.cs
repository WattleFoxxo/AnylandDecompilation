using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x020001A6 RID: 422
[PostProcess(typeof(TestRenderer), PostProcessEvent.AfterStack, "Filter/Test", true)]
[Serializable]
public sealed class TestFilter : PostProcessEffectSettings
{
	// Token: 0x06000D23 RID: 3363 RVA: 0x00075EBB File Offset: 0x000742BB
	public override bool IsEnabledAndSupported(PostProcessRenderContext context)
	{
		return this.enabled.value && this.intensity.value > 0f;
	}

	// Token: 0x04000EC7 RID: 3783
	[Range(0f, 1f)]
	public FloatParameter intensity = new FloatParameter
	{
		value = 0f
	};
}
