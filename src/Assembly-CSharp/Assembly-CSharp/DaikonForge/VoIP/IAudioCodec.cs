using System;

namespace DaikonForge.VoIP
{
	// Token: 0x02000008 RID: 8
	public interface IAudioCodec
	{
		// Token: 0x0600002B RID: 43
		void OnAudioAvailable(BigArray<float> rawPCM);

		// Token: 0x0600002C RID: 44
		VoicePacketWrapper? GetNextEncodedFrame(int frequency);

		// Token: 0x0600002D RID: 45
		BigArray<float> DecodeFrame(VoicePacketWrapper data);

		// Token: 0x0600002E RID: 46
		BigArray<float> GenerateMissingFrame(int frequency);
	}
}
