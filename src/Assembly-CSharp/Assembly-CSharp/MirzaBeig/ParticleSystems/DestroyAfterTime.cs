using System;
using UnityEngine;

namespace MirzaBeig.ParticleSystems
{
	// Token: 0x02000048 RID: 72
	[Serializable]
	public class DestroyAfterTime : MonoBehaviour
	{
		// Token: 0x060002B8 RID: 696 RVA: 0x0000BA33 File Offset: 0x00009E33
		private void Awake()
		{
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000BA35 File Offset: 0x00009E35
		private void Start()
		{
			global::UnityEngine.Object.Destroy(base.gameObject, this.time);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000BA48 File Offset: 0x00009E48
		private void Update()
		{
		}

		// Token: 0x04000187 RID: 391
		public float time = 2f;
	}
}
