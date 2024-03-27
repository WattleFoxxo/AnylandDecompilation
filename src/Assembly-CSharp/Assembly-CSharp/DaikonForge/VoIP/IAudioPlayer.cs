using System;

namespace DaikonForge.VoIP
{
	// Token: 0x02000015 RID: 21
	public interface IAudioPlayer
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000061 RID: 97
		bool PlayingSound { get; }

		// Token: 0x06000062 RID: 98
		void SetSampleRate(int sampleRate);

		// Token: 0x06000063 RID: 99
		void BufferAudio(BigArray<float> audioData);
	}
}
