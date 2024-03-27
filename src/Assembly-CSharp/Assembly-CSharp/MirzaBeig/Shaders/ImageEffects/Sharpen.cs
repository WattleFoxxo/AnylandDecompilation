using System;
using UnityEngine;

namespace MirzaBeig.Shaders.ImageEffects
{
	// Token: 0x02000052 RID: 82
	[ExecuteInEditMode]
	[Serializable]
	public class Sharpen : IEBase
	{
		// Token: 0x060002FA RID: 762 RVA: 0x0000BFFE File Offset: 0x0000A3FE
		private void Awake()
		{
			base.shader = Shader.Find("Hidden/Mirza Beig/Image Effects/Sharpen");
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000C010 File Offset: 0x0000A410
		private void Start()
		{
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000C012 File Offset: 0x0000A412
		private void Update()
		{
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000C014 File Offset: 0x0000A414
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetFloat("_strength", this.strength);
			base.material.SetFloat("_edgeMult", this.edgeMult);
			base.blit(source, destination);
		}

		// Token: 0x04000192 RID: 402
		[Range(-2f, 2f)]
		public float strength = 0.5f;

		// Token: 0x04000193 RID: 403
		[Range(0f, 8f)]
		public float edgeMult = 0.2f;
	}
}
