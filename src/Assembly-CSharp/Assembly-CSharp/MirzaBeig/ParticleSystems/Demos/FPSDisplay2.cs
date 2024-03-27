using System;
using UnityEngine;
using UnityEngine.UI;

namespace MirzaBeig.ParticleSystems.Demos
{
	// Token: 0x02000042 RID: 66
	[ExecuteInEditMode]
	[Serializable]
	public class FPSDisplay2 : MonoBehaviour
	{
		// Token: 0x0600028C RID: 652 RVA: 0x0000B0B7 File Offset: 0x000094B7
		private void Awake()
		{
			if (FPSDisplay2.instance && !Application.isEditor)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				FPSDisplay2.instance = this;
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000B0E8 File Offset: 0x000094E8
		private void Start()
		{
			this.fpsText = base.GetComponent<Text>();
			this.m_FpsNextPeriod = Time.realtimeSinceStartup + this.fpsMeasurePeriod;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000B108 File Offset: 0x00009508
		private void Update()
		{
			this.m_FpsAccumulator++;
			if (Time.realtimeSinceStartup > this.m_FpsNextPeriod)
			{
				this.m_CurrentFps = (int)((float)this.m_FpsAccumulator / this.fpsMeasurePeriod);
				this.m_FpsAccumulator = 0;
				this.m_FpsNextPeriod += this.fpsMeasurePeriod;
				this.fpsText.text = this.m_CurrentFps.ToString(this.textFormat);
			}
		}

		// Token: 0x04000169 RID: 361
		private static FPSDisplay2 instance;

		// Token: 0x0400016A RID: 362
		public float fpsMeasurePeriod = 0.5f;

		// Token: 0x0400016B RID: 363
		private int m_FpsAccumulator;

		// Token: 0x0400016C RID: 364
		private float m_FpsNextPeriod;

		// Token: 0x0400016D RID: 365
		private int m_CurrentFps;

		// Token: 0x0400016E RID: 366
		private Text fpsText;

		// Token: 0x0400016F RID: 367
		public string textFormat = "FPS (X/Xs-AVG): 00.00";
	}
}
