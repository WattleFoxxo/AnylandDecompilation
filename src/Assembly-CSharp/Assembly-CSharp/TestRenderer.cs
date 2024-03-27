using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x020001A7 RID: 423
public sealed class TestRenderer : PostProcessEffectRenderer<TestFilter>
{
	// Token: 0x06000D25 RID: 3365 RVA: 0x00075EEC File Offset: 0x000742EC
	public override void Render(PostProcessRenderContext context)
	{
		PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("Hidden/Filter/Test"));
		propertySheet.properties.SetFloat("_intensity", base.settings.intensity.value);
		context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0, false);
	}
}
