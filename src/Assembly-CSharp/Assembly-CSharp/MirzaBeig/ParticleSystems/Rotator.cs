using System;
using UnityEngine;

namespace MirzaBeig.ParticleSystems
{
	// Token: 0x0200004F RID: 79
	[Serializable]
	public class Rotator : MonoBehaviour
	{
		// Token: 0x060002E6 RID: 742 RVA: 0x0000BEFB File Offset: 0x0000A2FB
		private void Awake()
		{
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000BEFD File Offset: 0x0000A2FD
		private void Start()
		{
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000BEFF File Offset: 0x0000A2FF
		private void Update()
		{
			base.transform.Rotate(this.localRotationSpeed * Time.deltaTime, Space.Self);
			base.transform.Rotate(this.worldRotationSpeed * Time.deltaTime, Space.World);
		}

		// Token: 0x0400018C RID: 396
		public Vector3 localRotationSpeed;

		// Token: 0x0400018D RID: 397
		public Vector3 worldRotationSpeed;
	}
}
