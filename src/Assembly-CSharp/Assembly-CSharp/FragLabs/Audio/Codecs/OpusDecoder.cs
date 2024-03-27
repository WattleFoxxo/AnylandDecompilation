using System;
using FragLabs.Audio.Codecs.Opus;

namespace FragLabs.Audio.Codecs
{
	// Token: 0x02000006 RID: 6
	public class OpusDecoder : IDisposable
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002058 File Offset: 0x00000458
		private OpusDecoder(IntPtr decoder, int outputSamplingRate, int outputChannels)
		{
			this._decoder = decoder;
			this.OutputSamplingRate = outputSamplingRate;
			this.OutputChannels = outputChannels;
			this.MaxDataBytes = 4000;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002080 File Offset: 0x00000480
		public static OpusDecoder Create(int outputSampleRate, int outputChannels)
		{
			if (outputSampleRate != 8000 && outputSampleRate != 12000 && outputSampleRate != 16000 && outputSampleRate != 24000 && outputSampleRate != 48000)
			{
				throw new ArgumentOutOfRangeException("inputSamplingRate");
			}
			if (outputChannels != 1 && outputChannels != 2)
			{
				throw new ArgumentOutOfRangeException("inputChannels");
			}
			IntPtr intPtr2;
			IntPtr intPtr = API.opus_decoder_create(outputSampleRate, outputChannels, out intPtr2);
			if ((int)intPtr2 != 0)
			{
				throw new Exception("Exception occured while creating decoder");
			}
			return new OpusDecoder(intPtr, outputSampleRate, outputChannels);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002110 File Offset: 0x00000510
		public unsafe byte[] Decode(byte[] inputOpusData, int dataLength, out int decodedLength)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("OpusDecoder");
			}
			byte[] array = new byte[this.MaxDataBytes];
			int num = this.FrameCount(this.MaxDataBytes);
			int num2;
			fixed (byte* ptr = (ref array != null && array.Length != 0 ? ref array[0] : ref *null))
			{
				IntPtr intPtr = new IntPtr((void*)ptr);
				if (inputOpusData != null)
				{
					num2 = API.opus_decode(this._decoder, inputOpusData, dataLength, intPtr, num, 0);
				}
				else
				{
					num2 = API.opus_decode(this._decoder, null, 0, intPtr, num, (!this.ForwardErrorCorrection) ? 0 : 1);
				}
			}
			decodedLength = num2 * 2;
			if (num2 < 0)
			{
				string text = "Decoding failed - ";
				Errors errors = (Errors)num2;
				throw new Exception(text + errors.ToString());
			}
			return array;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000021E4 File Offset: 0x000005E4
		public int FrameCount(int bufferSize)
		{
			int num = 16;
			int num2 = num / 8 * this.OutputChannels;
			return bufferSize / num2;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002202 File Offset: 0x00000602
		// (set) Token: 0x0600000F RID: 15 RVA: 0x0000220A File Offset: 0x0000060A
		public int OutputSamplingRate { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002213 File Offset: 0x00000613
		// (set) Token: 0x06000011 RID: 17 RVA: 0x0000221B File Offset: 0x0000061B
		public int OutputChannels { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002224 File Offset: 0x00000624
		// (set) Token: 0x06000013 RID: 19 RVA: 0x0000222C File Offset: 0x0000062C
		public int MaxDataBytes { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002235 File Offset: 0x00000635
		// (set) Token: 0x06000015 RID: 21 RVA: 0x0000223D File Offset: 0x0000063D
		public bool ForwardErrorCorrection { get; set; }

		// Token: 0x06000016 RID: 22 RVA: 0x00002248 File Offset: 0x00000648
		~OpusDecoder()
		{
			this.Dispose();
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002278 File Offset: 0x00000678
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			if (this._decoder != IntPtr.Zero)
			{
				API.opus_decoder_destroy(this._decoder);
				this._decoder = IntPtr.Zero;
			}
			this.disposed = true;
		}

		// Token: 0x04000014 RID: 20
		private IntPtr _decoder;

		// Token: 0x04000019 RID: 25
		private bool disposed;
	}
}
