using System;
using UnityEngine;
using UnityEngine.UI;

namespace MirzaBeig.ParticleSystems.Demos
{
	// Token: 0x02000041 RID: 65
	[ExecuteInEditMode]
	[Serializable]
	public class FPSDisplay : MonoBehaviour
	{
		// Token: 0x06000288 RID: 648 RVA: 0x0000AFDA File Offset: 0x000093DA
		private void Awake()
		{
			if (FPSDisplay.instance)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
			}
			else
			{
				FPSDisplay.instance = this;
			}
			Application.targetFrameRate = this.targetFrameRate;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x0000B00C File Offset: 0x0000940C
		private void Start()
		{
			this.fpsText = base.GetComponent<Text>();
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000B01C File Offset: 0x0000941C
		private void Update()
		{
			this.time += Time.deltaTime;
			this.frames++;
			if (this.time > this.updateTime)
			{
				this.fpsText.text = (1f / (this.time / (float)this.frames)).ToString(this.textFormat);
				this.time = 0f;
				this.frames = 0;
			}
		}

		// Token: 0x04000162 RID: 354
		private static FPSDisplay instance;

		// Token: 0x04000163 RID: 355
		private Text fpsText;

		// Token: 0x04000164 RID: 356
		private int frames;

		// Token: 0x04000165 RID: 357
		private float time;

		// Token: 0x04000166 RID: 358
		public int targetFrameRate = 60;

		// Token: 0x04000167 RID: 359
		public float updateTime = 1f;

		// Token: 0x04000168 RID: 360
		public string textFormat = "FPS (X/Xs-AVG): 00.00";
	}
}
