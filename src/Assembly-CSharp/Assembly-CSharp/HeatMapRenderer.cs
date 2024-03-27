using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x020001A3 RID: 419
public sealed class HeatMapRenderer : PostProcessEffectRenderer<HeatMapFilter>
{
	// Token: 0x06000D1D RID: 3357 RVA: 0x00075D74 File Offset: 0x00074174
	public override void Render(PostProcessRenderContext context)
	{
		PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("Hidden/Filter/HeatMap"));
		propertySheet.properties.SetFloat("_intensity", base.settings.intensity.value);
		context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0, false);
	}
}
