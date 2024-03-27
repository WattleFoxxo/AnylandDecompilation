using System;
using UnityEngine;

// Token: 0x020001B4 RID: 436
public class GlowTransition : MonoBehaviour
{
	// Token: 0x06000D67 RID: 3431 RVA: 0x00077B64 File Offset: 0x00075F64
	private void Start()
	{
		this.renderer = base.GetComponent<Renderer>();
	}

	// Token: 0x06000D68 RID: 3432 RVA: 0x00077B74 File Offset: 0x00075F74
	private void Update()
	{
		if (this.glow != this.targetGlow)
		{
			this.glow = Mathf.MoveTowards(this.glow, this.targetGlow, 2f * Time.deltaTime);
			if (this.renderer != null)
			{
				Color gray = Misc.GetGray(this.glow * 0.75f);
				this.renderer.material.SetColor("_EmissionColor", gray);
			}
		}
	}

	// Token: 0x04000EF5 RID: 3829
	public float targetGlow;

	// Token: 0x04000EF6 RID: 3830
	private float glow;

	// Token: 0x04000EF7 RID: 3831
	private Renderer renderer;
}
