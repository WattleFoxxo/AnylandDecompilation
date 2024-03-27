using System;
using UnityEngine;

namespace DaikonForge.VoIP
{
	// Token: 0x0200000F RID: 15
	public class TestLocalVoiceController : VoiceControllerBase
	{
		// Token: 0x06000040 RID: 64 RVA: 0x0000311C File Offset: 0x0000151C
		protected override void OnAudioDataEncoded(VoicePacketWrapper encodedFrame)
		{
			if (global::UnityEngine.Random.Range(0f, 1f) <= this.PacketLoss)
			{
				return;
			}
			this.ReceiveAudioData(encodedFrame);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003140 File Offset: 0x00001540
		public override bool IsLocal
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0400002E RID: 46
		public float PacketLoss = 0.1f;
	}
}
