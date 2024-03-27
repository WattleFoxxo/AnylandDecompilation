using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200019F RID: 415
public sealed class ColorReductionRenderer : PostProcessEffectRenderer<ColorReductionFilter>
{
	// Token: 0x06000D15 RID: 3349 RVA: 0x00075BFC File Offset: 0x00073FFC
	public override void Render(PostProcessRenderContext context)
	{
		PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("Hidden/Filter/ColorReduction"));
		propertySheet.properties.SetFloat("_intensity", base.settings.intensity.value);
		context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0, false);
	}
}
