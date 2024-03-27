using System;

namespace DaikonForge.VoIP
{
	// Token: 0x0200001D RID: 29
	public class ChunkBuffer
	{
		// Token: 0x0600008F RID: 143 RVA: 0x00003FDC File Offset: 0x000023DC
		public void AddSamples(BigArray<float> incomingSamples)
		{
			int num = this.samples.Count * 4;
			int num2 = this.samples.Count + incomingSamples.Length;
			int num3 = incomingSamples.Length * 4;
			this.samples.EnsureCapacity(num2);
			this.samples.ForceCount(num2);
			incomingSamples.CopyTo(0, this.samples.Items, num, num3);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004040 File Offset: 0x00002440
		public bool RetrieveChunk(float[] destination)
		{
			if (this.samples.Count < destination.Length)
			{
				return false;
			}
			for (int i = 0; i < destination.Length; i++)
			{
				destination[i] = this.samples[i];
			}
			this.samples.RemoveRange(0, destination.Length);
			return true;
		}

		// Token: 0x04000063 RID: 99
		private FastList<float> samples = new FastList<float>();
	}
}
