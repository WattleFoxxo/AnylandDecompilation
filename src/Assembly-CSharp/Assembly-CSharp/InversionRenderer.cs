using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x020001A5 RID: 421
public sealed class InversionRenderer : PostProcessEffectRenderer<InversionFilter>
{
	// Token: 0x06000D21 RID: 3361 RVA: 0x00075E30 File Offset: 0x00074230
	public override void Render(PostProcessRenderContext context)
	{
		PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("Hidden/Filter/Inversion"));
		propertySheet.properties.SetFloat("_intensity", base.settings.intensity.value);
		context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0, false);
	}
}
