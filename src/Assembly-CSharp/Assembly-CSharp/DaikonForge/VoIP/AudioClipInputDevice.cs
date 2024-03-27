using System;
using System.Collections;
using UnityEngine;

namespace DaikonForge.VoIP
{
	// Token: 0x02000012 RID: 18
	public class AudioClipInputDevice : AudioInputDeviceBase
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00003278 File Offset: 0x00001678
		public override void StartRecording()
		{
			float[] array = new float[this.testClip.samples * this.testClip.channels];
			this.testClip.GetData(array, 0);
			BigArray<float> bigArray = new BigArray<float>(array.Length, 0);
			bigArray.Resize(array.Length);
			bigArray.CopyFrom(array, 0, 0, array.Length * 4);
			base.StartCoroutine(this.yieldChunks(bigArray, this.testClip.frequency, 1f));
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000032F0 File Offset: 0x000016F0
		private IEnumerator yieldChunks(BigArray<float> data, int chunkSize, float chunkDuration)
		{
			int readHead = 0;
			while (readHead < data.Length)
			{
				int remainder = chunkSize;
				if (readHead + chunkSize >= data.Length)
				{
					remainder = data.Length - readHead;
				}
				BigArray<float> temp = new BigArray<float>(remainder, 0);
				temp.Resize(remainder);
				temp.CopyFrom(data.Items, readHead * 4, 0, remainder * 4);
				AudioUtils.Resample(temp, this.testClip.frequency, AudioUtils.GetFrequency(this.ResampleFrequency));
				base.bufferReady(temp, AudioUtils.GetFrequency(this.ResampleFrequency));
				readHead += remainder;
				yield return new WaitForSeconds(chunkDuration);
			}
			yield break;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003320 File Offset: 0x00001720
		public override void StopRecording()
		{
		}

		// Token: 0x04000036 RID: 54
		public AudioClip testClip;

		// Token: 0x04000037 RID: 55
		public FrequencyMode ResampleFrequency = FrequencyMode.Wide;
	}
}
