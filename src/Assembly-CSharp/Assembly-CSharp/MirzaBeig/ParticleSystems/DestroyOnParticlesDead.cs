using System;
using UnityEngine;

namespace MirzaBeig.ParticleSystems
{
	// Token: 0x02000049 RID: 73
	[Serializable]
	public class DestroyOnParticlesDead : ParticleSystems
	{
		// Token: 0x060002BC RID: 700 RVA: 0x0000BDAB File Offset: 0x0000A1AB
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000BDB3 File Offset: 0x0000A1B3
		protected override void Start()
		{
			base.Start();
			base.onParticleSystemsDeadEvent += this.onParticleSystemsDead;
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000BDCD File Offset: 0x0000A1CD
		private void onParticleSystemsDead()
		{
			global::UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000BDDA File Offset: 0x0000A1DA
		protected override void Update()
		{
			base.Update();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000BDE2 File Offset: 0x0000A1E2
		protected override void LateUpdate()
		{
			base.LateUpdate();
		}
	}
}
