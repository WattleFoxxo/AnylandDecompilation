using System;
using UnityEngine;

namespace MirzaBeig.ParticleSystems
{
	// Token: 0x0200004A RID: 74
	[Serializable]
	public class DestroyOnTrailsDestroyed : TrailRenderers
	{
		// Token: 0x060002C2 RID: 706 RVA: 0x0000BE40 File Offset: 0x0000A240
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000BE48 File Offset: 0x0000A248
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000BE50 File Offset: 0x0000A250
		protected override void Update()
		{
			base.Update();
			bool flag = true;
			for (int i = 0; i < this.trailRenderers.Length; i++)
			{
				if (this.trailRenderers[i] != null)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
