using System;
using System.Collections.Generic;
using NSpeex;

namespace DaikonForge.VoIP
{
	// Token: 0x0200000C RID: 12
	public class SpeexCodec : IAudioCodec
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002A58 File Offset: 0x00000E58
		public SpeexCodec(bool VBR)
		{
			this.encoders = new Dictionary<int, SpeexCodec.codecWrapper>
			{
				{
					8000,
					new SpeexCodec.codecWrapper(BandMode.Narrow, VBR)
				},
				{
					16000,
					new SpeexCodec.codecWrapper(BandMode.Wide, VBR)
				},
				{
					32000,
					new SpeexCodec.codecWrapper(BandMode.UltraWide, VBR)
				}
			};
			this.chunkBuffer = new ChunkBuffer();
			this.tempOutputArray = new BigArray<float>(1024, 0);
			this.tempPacketWrapper = new VoicePacketWrapper(0UL, 16, new byte[0]);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002B1C File Offset: 0x00000F1C
		public void OnAudioAvailable(BigArray<float> rawPCM)
		{
			this.chunkBuffer.AddSamples(rawPCM);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002B2C File Offset: 0x00000F2C
		public VoicePacketWrapper? GetNextEncodedFrame(int frequency)
		{
			int num = this.frameSizes[frequency];
			SpeexCodec.codecWrapper codecWrapper = this.encoders[frequency];
			float[] array = TempArray<float>.Obtain(num);
			if (!this.chunkBuffer.RetrieveChunk(array))
			{
				TempArray<float>.Release(array);
				return null;
			}
			this.tempPacketWrapper = default(VoicePacketWrapper);
			this.tempPacketWrapper.Frequency = (byte)(frequency / 1000);
			short[] array2 = TempArray<short>.Obtain(num);
			for (int i = 0; i < num; i++)
			{
				float num2 = array[i] * 32767f;
				array2[i] = (short)num2;
			}
			TempArray<float>.Release(array);
			byte[] array3 = TempArray<byte>.Obtain(array2.Length * 2);
			int num3 = codecWrapper.encoder.Encode(array2, 0, num, array3, 0, array3.Length);
			TempArray<short>.Release(array2);
			this.tempPacketWrapper.RawData = new byte[num3];
			Buffer.BlockCopy(array3, 0, this.tempPacketWrapper.RawData, 0, num3);
			TempArray<byte>.Release(array3);
			return new VoicePacketWrapper?(this.tempPacketWrapper);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002C40 File Offset: 0x00001040
		public BigArray<float> DecodeFrame(VoicePacketWrapper data)
		{
			int num = this.frameSizes[(int)data.Frequency * 1000];
			SpeexCodec.codecWrapper codecWrapper = this.encoders[(int)data.Frequency * 1000];
			short[] array = TempArray<short>.Obtain(num * 4);
			int num2 = codecWrapper.decoder.Decode(data.RawData, 0, data.RawData.Length, array, 0, false);
			if (this.tempOutputArray.Length != num2)
			{
				this.tempOutputArray.Resize(num2);
			}
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)array[i];
				num3 /= 32767f;
				this.tempOutputArray[i] = num3;
			}
			TempArray<short>.Release(array);
			return this.tempOutputArray;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D08 File Offset: 0x00001108
		public BigArray<float> GenerateMissingFrame(int frequency)
		{
			int num = this.frameSizes[frequency * 1000];
			SpeexCodec.codecWrapper codecWrapper = this.encoders[frequency * 1000];
			short[] array = TempArray<short>.Obtain(num * 4);
			int num2 = codecWrapper.decoder.Decode(null, 0, 0, array, 0, true);
			if (this.tempOutputArray.Length != num2)
			{
				this.tempOutputArray.Resize(num2);
			}
			for (int i = 0; i < num2; i++)
			{
				float num3 = (float)array[i];
				num3 /= 32767f;
				this.tempOutputArray[i] = num3;
			}
			TempArray<short>.Release(array);
			return this.tempOutputArray;
		}

		// Token: 0x04000027 RID: 39
		private Dictionary<int, SpeexCodec.codecWrapper> encoders;

		// Token: 0x04000028 RID: 40
		private Dictionary<int, int> frameSizes = new Dictionary<int, int>
		{
			{ 8000, 160 },
			{ 16000, 320 },
			{ 32000, 640 }
		};

		// Token: 0x04000029 RID: 41
		private ChunkBuffer chunkBuffer;

		// Token: 0x0400002A RID: 42
		private BigArray<float> tempOutputArray;

		// Token: 0x0400002B RID: 43
		private VoicePacketWrapper tempPacketWrapper;

		// Token: 0x0200000D RID: 13
		public class FrequencyProvider : IFrequencyProvider
		{
			// Token: 0x0600003D RID: 61 RVA: 0x00002DBD File Offset: 0x000011BD
			public int GetFrequency(FrequencyMode mode)
			{
				switch (mode)
				{
				case FrequencyMode.Narrow:
					return 8000;
				case FrequencyMode.Wide:
					return 16000;
				case FrequencyMode.UltraWide:
					return 32000;
				default:
					return 16000;
				}
			}
		}

		// Token: 0x0200000E RID: 14
		private class codecWrapper
		{
			// Token: 0x0600003E RID: 62 RVA: 0x00002DED File Offset: 0x000011ED
			public codecWrapper(BandMode mode, bool vbr)
			{
				this.encoder = new SpeexEncoder(mode);
				this.decoder = new SpeexDecoder(mode, false);
				this.encoder.VBR = vbr;
				this.encoder.Quality = 5;
			}

			// Token: 0x0400002C RID: 44
			public SpeexEncoder encoder;

			// Token: 0x0400002D RID: 45
			public SpeexDecoder decoder;
		}
	}
}
