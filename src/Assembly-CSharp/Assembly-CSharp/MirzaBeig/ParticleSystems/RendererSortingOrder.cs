using System;
using UnityEngine;

namespace MirzaBeig.ParticleSystems
{
	// Token: 0x0200004E RID: 78
	[RequireComponent(typeof(Renderer))]
	[Serializable]
	public class RendererSortingOrder : MonoBehaviour
	{
		// Token: 0x060002E2 RID: 738 RVA: 0x0000BEDC File Offset: 0x0000A2DC
		private void Awake()
		{
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000BEDE File Offset: 0x0000A2DE
		private void Start()
		{
			base.GetComponent<Renderer>().sortingOrder = this.sortingOrder;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000BEF1 File Offset: 0x0000A2F1
		private void Update()
		{
		}

		// Token: 0x0400018B RID: 395
		public int sortingOrder;
	}
}
