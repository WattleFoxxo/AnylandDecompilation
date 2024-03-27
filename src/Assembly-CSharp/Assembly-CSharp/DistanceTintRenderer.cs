using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x020001A1 RID: 417
public sealed class DistanceTintRenderer : PostProcessEffectRenderer<DistanceTintFilter>
{
	// Token: 0x06000D19 RID: 3353 RVA: 0x00075CB8 File Offset: 0x000740B8
	public override void Render(PostProcessRenderContext context)
	{
		PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("Hidden/Filter/DistanceTint"));
		propertySheet.properties.SetFloat("_intensity", base.settings.intensity.value);
		context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0, false);
	}
}
