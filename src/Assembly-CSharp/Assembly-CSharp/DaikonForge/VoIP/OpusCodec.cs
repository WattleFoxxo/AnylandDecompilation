using System;
using System.Collections.Generic;
using FragLabs.Audio.Codecs;
using FragLabs.Audio.Codecs.Opus;

namespace DaikonForge.VoIP
{
	// Token: 0x02000009 RID: 9
	public class OpusCodec : IAudioCodec
	{
		// Token: 0x0600002F RID: 47 RVA: 0x000026D4 File Offset: 0x00000AD4
		public OpusCodec(int bitrate)
		{
			this.encoders = new Dictionary<int, OpusCodec.codecWrapper>
			{
				{
					8000,
					new OpusCodec.codecWrapper(8000, bitrate)
				},
				{
					16000,
					new OpusCodec.codecWrapper(16000, bitrate)
				},
				{
					48000,
					new OpusCodec.codecWrapper(48000, bitrate)
				}
			};
			this.chunkBuffer = new ChunkBuffer();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002752 File Offset: 0x00000B52
		public void OnAudioAvailable(BigArray<float> rawPCM)
		{
			this.chunkBuffer.AddSamples(rawPCM);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002760 File Offset: 0x00000B60
		public VoicePacketWrapper? GetNextEncodedFrame(int frequency)
		{
			OpusCodec.codecWrapper codecWrapper = this.encoders[frequency];
			int num = codecWrapper.BytesPerSegment / 2;
			float[] array = TempArray<float>.Obtain(num);
			if (!this.chunkBuffer.RetrieveChunk(array))
			{
				TempArray<float>.Release(array);
				return null;
			}
			VoicePacketWrapper voicePacketWrapper = default(VoicePacketWrapper);
			voicePacketWrapper.Frequency = (byte)(frequency / 1000);
			byte[] array2 = TempArray<byte>.Obtain(num * 2);
			for (int i = 0; i < array2.Length; i += 2)
			{
				short num2 = (short)(array[i / 2] * 32767f);
				array2[i] = (byte)(num2 & 255);
				array2[i + 1] = (byte)((num2 >> 8) & 255);
			}
			TempArray<float>.Release(array);
			int num3;
			byte[] array3 = codecWrapper.encoder.Encode(array2, array2.Length, out num3);
			TempArray<byte>.Release(array2);
			voicePacketWrapper.RawData = new byte[num3];
			Buffer.BlockCopy(array3, 0, voicePacketWrapper.RawData, 0, num3);
			return new VoicePacketWrapper?(voicePacketWrapper);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002860 File Offset: 0x00000C60
		public BigArray<float> DecodeFrame(VoicePacketWrapper data)
		{
			OpusCodec.codecWrapper codecWrapper = this.encoders[(int)data.Frequency * 1000];
			int num;
			byte[] array = codecWrapper.decoder.Decode(data.RawData, data.RawData.Length, out num);
			int num2 = num / 2;
			if (this.tempOutputArray.Length != num2)
			{
				this.tempOutputArray.Resize(num2);
			}
			for (int i = 0; i < num2; i++)
			{
				byte b = array[i * 2];
				byte b2 = array[i * 2 + 1];
				short num3 = (short)((int)b | ((int)b2 << 8));
				float num4 = (float)num3;
				num4 /= 32767f;
				this.tempOutputArray[i] = num4;
			}
			return this.tempOutputArray;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000291C File Offset: 0x00000D1C
		public BigArray<float> GenerateMissingFrame(int frequency)
		{
			OpusCodec.codecWrapper codecWrapper = this.encoders[frequency * 1000];
			int num;
			byte[] array = codecWrapper.decoder.Decode(null, 0, out num);
			int num2 = num / 2;
			if (this.tempOutputArray.Length != num2)
			{
				this.tempOutputArray.Resize(num2);
			}
			for (int i = 0; i < num2; i++)
			{
				byte b = array[i * 2];
				byte b2 = array[i * 2 + 1];
				float num3 = (float)((int)b & ((int)b2 << 8));
				num3 /= 32767f;
				this.tempOutputArray[i] = num3;
			}
			return this.tempOutputArray;
		}

		// Token: 0x04000020 RID: 32
		private Dictionary<int, OpusCodec.codecWrapper> encoders;

		// Token: 0x04000021 RID: 33
		private ChunkBuffer chunkBuffer;

		// Token: 0x04000022 RID: 34
		private BigArray<float> tempOutputArray = new BigArray<float>(1024, 0);

		// Token: 0x0200000A RID: 10
		public class FrequencyProvider : IFrequencyProvider
		{
			// Token: 0x06000035 RID: 53 RVA: 0x000029C6 File Offset: 0x00000DC6
			public int GetFrequency(FrequencyMode mode)
			{
				switch (mode)
				{
				case FrequencyMode.Narrow:
					return 8000;
				case FrequencyMode.Wide:
					return 16000;
				case FrequencyMode.UltraWide:
					return 48000;
				default:
					return 16000;
				}
			}
		}

		// Token: 0x0200000B RID: 11
		private class codecWrapper
		{
			// Token: 0x06000036 RID: 54 RVA: 0x000029F8 File Offset: 0x00000DF8
			public codecWrapper(int sampleRate, int bitrate)
			{
				this.encoder = OpusEncoder.Create(sampleRate, 1, Application.Voip);
				this.encoder.Bitrate = bitrate;
				this.decoder = OpusDecoder.Create(sampleRate, 1);
				this.SegmentFrames = 960;
				this.BytesPerSegment = this.encoder.FrameByteCount(this.SegmentFrames);
			}

			// Token: 0x04000023 RID: 35
			public OpusEncoder encoder;

			// Token: 0x04000024 RID: 36
			public OpusDecoder decoder;

			// Token: 0x04000025 RID: 37
			public int BytesPerSegment;

			// Token: 0x04000026 RID: 38
			public int SegmentFrames;
		}
	}
}
