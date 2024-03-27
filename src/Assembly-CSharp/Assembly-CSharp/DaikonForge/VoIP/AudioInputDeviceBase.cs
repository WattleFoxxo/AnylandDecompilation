using System;
using System.Diagnostics;
using UnityEngine;

namespace DaikonForge.VoIP
{
	// Token: 0x02000014 RID: 20
	public abstract class AudioInputDeviceBase : MonoBehaviour
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600005C RID: 92 RVA: 0x000031E0 File Offset: 0x000015E0
		// (remove) Token: 0x0600005D RID: 93 RVA: 0x00003218 File Offset: 0x00001618
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event AudioBufferReadyHandler OnAudioBufferReady;

		// Token: 0x0600005E RID: 94
		public abstract void StartRecording();

		// Token: 0x0600005F RID: 95
		public abstract void StopRecording();

		// Token: 0x06000060 RID: 96 RVA: 0x0000324E File Offset: 0x0000164E
		protected void bufferReady(BigArray<float> newData, int frequency)
		{
			if (this.OnAudioBufferReady != null)
			{
				this.OnAudioBufferReady(newData, frequency);
			}
		}
	}
}
