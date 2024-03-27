using System;
using UnityEngine;

namespace MirzaBeig.ParticleSystems.Demos
{
	// Token: 0x02000043 RID: 67
	[Serializable]
	public class FPSTest : MonoBehaviour
	{
		// Token: 0x06000290 RID: 656 RVA: 0x0000B196 File Offset: 0x00009596
		private void Awake()
		{
		}

		// Token: 0x06000291 RID: 657 RVA: 0x0000B198 File Offset: 0x00009598
		private void Start()
		{
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000B19C File Offset: 0x0000959C
		private void Update()
		{
			if (Input.GetKey(KeyCode.Space))
			{
				Application.targetFrameRate = this.targetFPS2;
				this.previousVSyncCount = QualitySettings.vSyncCount;
				QualitySettings.vSyncCount = 0;
			}
			else if (Input.GetKeyUp(KeyCode.Space))
			{
				Application.targetFrameRate = this.targetFPS1;
				QualitySettings.vSyncCount = this.previousVSyncCount;
			}
		}

		// Token: 0x04000170 RID: 368
		public int targetFPS1 = 60;

		// Token: 0x04000171 RID: 369
		public int targetFPS2 = 10;

		// Token: 0x04000172 RID: 370
		private int previousVSyncCount;
	}
}
