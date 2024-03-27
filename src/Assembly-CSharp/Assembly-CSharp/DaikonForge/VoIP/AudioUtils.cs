using System;
using UnityEngine;

namespace DaikonForge.VoIP
{
	// Token: 0x0200001B RID: 27
	public class AudioUtils
	{
		// Token: 0x06000080 RID: 128 RVA: 0x00003DE7 File Offset: 0x000021E7
		public static int GetFrequency(FrequencyMode mode)
		{
			return AudioUtils.FrequencyProvider.GetFrequency(mode);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003DF4 File Offset: 0x000021F4
		public static void Resample(BigArray<float> samples, int oldFrequency, int newFrequency)
		{
			if (oldFrequency == newFrequency)
			{
				return;
			}
			AudioUtils.temp.Clear();
			float num = (float)oldFrequency / (float)newFrequency;
			int num2 = 0;
			for (;;)
			{
				int num3 = (int)((float)num2++ * num);
				if (num3 >= samples.Length)
				{
					break;
				}
				AudioUtils.temp.Add(samples[num3]);
			}
			samples.Resize(AudioUtils.temp.Count);
			samples.CopyFrom(AudioUtils.temp.Items, 0, 0, AudioUtils.temp.Count * 4);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003E80 File Offset: 0x00002280
		public static void ApplyGain(float[] samples, float gain)
		{
			for (int i = 0; i < samples.Length; i++)
			{
				samples[i] *= gain;
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003EB0 File Offset: 0x000022B0
		public static float GetMaxAmplitude(float[] samples)
		{
			float num = 0f;
			for (int i = 0; i < samples.Length; i++)
			{
				num = Mathf.Max(num, Mathf.Abs(samples[i]));
			}
			return num;
		}

		// Token: 0x0400005F RID: 95
		public static IFrequencyProvider FrequencyProvider = new SpeexCodec.FrequencyProvider();

		// Token: 0x04000060 RID: 96
		private static FastList<float> temp = new FastList<float>();
	}
}
