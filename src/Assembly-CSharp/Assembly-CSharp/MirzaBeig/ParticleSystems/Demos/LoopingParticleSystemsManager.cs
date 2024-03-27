using System;

namespace MirzaBeig.ParticleSystems.Demos
{
	// Token: 0x02000044 RID: 68
	[Serializable]
	public class LoopingParticleSystemsManager : ParticleManager
	{
		// Token: 0x06000294 RID: 660 RVA: 0x0000B481 File Offset: 0x00009881
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000B489 File Offset: 0x00009889
		protected override void Start()
		{
			base.Start();
			this.particlePrefabs[this.currentParticlePrefab].gameObject.SetActive(true);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000B4AD File Offset: 0x000098AD
		public override void next()
		{
			this.particlePrefabs[this.currentParticlePrefab].gameObject.SetActive(false);
			base.next();
			this.particlePrefabs[this.currentParticlePrefab].gameObject.SetActive(true);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000B4ED File Offset: 0x000098ED
		public override void previous()
		{
			this.particlePrefabs[this.currentParticlePrefab].gameObject.SetActive(false);
			base.previous();
			this.particlePrefabs[this.currentParticlePrefab].gameObject.SetActive(true);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000B52D File Offset: 0x0000992D
		protected override void Update()
		{
			base.Update();
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000B535 File Offset: 0x00009935
		public override int getParticleCount()
		{
			return this.particlePrefabs[this.currentParticlePrefab].getParticleCount();
		}
	}
}
