using System;
using UnityEngine;

namespace MirzaBeig.Shaders.ImageEffects
{
	// Token: 0x02000051 RID: 81
	[ExecuteInEditMode]
	[Serializable]
	public class IEBase : MonoBehaviour
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000BF41 File Offset: 0x0000A341
		protected Material material
		{
			get
			{
				if (!this._material)
				{
					this._material = new Material(this.shader);
					this._material.hideFlags = HideFlags.HideAndDontSave;
				}
				return this._material;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000BF77 File Offset: 0x0000A377
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000BF7F File Offset: 0x0000A37F
		protected Shader shader { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000BF88 File Offset: 0x0000A388
		protected Camera camera
		{
			get
			{
				if (!this._camera)
				{
					this._camera = base.GetComponent<Camera>();
				}
				return this._camera;
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000BFAC File Offset: 0x0000A3AC
		private void Awake()
		{
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000BFAE File Offset: 0x0000A3AE
		private void Start()
		{
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000BFB0 File Offset: 0x0000A3B0
		private void Update()
		{
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000BFB2 File Offset: 0x0000A3B2
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000BFB4 File Offset: 0x0000A3B4
		protected void blit(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, this.material);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000BFC3 File Offset: 0x0000A3C3
		private void OnDisable()
		{
			if (this._material)
			{
				global::UnityEngine.Object.DestroyImmediate(this._material);
			}
		}

		// Token: 0x0400018F RID: 399
		private Material _material;

		// Token: 0x04000191 RID: 401
		private Camera _camera;
	}
}
