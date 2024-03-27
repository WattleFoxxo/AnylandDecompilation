using System;
using UnityEngine;

namespace MirzaBeig.ParticleSystems
{
	// Token: 0x02000050 RID: 80
	[Serializable]
	public class TrailRenderers : MonoBehaviour
	{
		// Token: 0x060002EA RID: 746 RVA: 0x0000BDF2 File Offset: 0x0000A1F2
		protected virtual void Awake()
		{
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000BDF4 File Offset: 0x0000A1F4
		protected virtual void Start()
		{
			this.trailRenderers = base.GetComponentsInChildren<TrailRenderer>();
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000BE02 File Offset: 0x0000A202
		protected virtual void Update()
		{
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000BE04 File Offset: 0x0000A204
		public void setAutoDestruct(bool value)
		{
			for (int i = 0; i < this.trailRenderers.Length; i++)
			{
				this.trailRenderers[i].autodestruct = value;
			}
		}

		// Token: 0x0400018E RID: 398
		[HideInInspector]
		public TrailRenderer[] trailRenderers;
	}
}
