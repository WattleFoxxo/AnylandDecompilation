using System;
using UnityEngine;

namespace MirzaBeig.ParticleSystems
{
	// Token: 0x0200004B RID: 75
	[RequireComponent(typeof(ParticleSystems))]
	[Serializable]
	public class IgnoreTimeScale : MonoBehaviour
	{
		// Token: 0x060002C6 RID: 710 RVA: 0x0000BEB1 File Offset: 0x0000A2B1
		private void Awake()
		{
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000BEB3 File Offset: 0x0000A2B3
		private void Start()
		{
			this.particleSystems = base.GetComponent<ParticleSystems>();
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000BEC1 File Offset: 0x0000A2C1
		private void Update()
		{
			this.particleSystems.simulate(Time.unscaledDeltaTime, false);
		}

		// Token: 0x04000188 RID: 392
		private ParticleSystems particleSystems;
	}
}
