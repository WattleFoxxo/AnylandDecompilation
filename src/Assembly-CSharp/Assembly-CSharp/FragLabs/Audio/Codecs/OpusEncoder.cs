using System;
using FragLabs.Audio.Codecs.Opus;

namespace FragLabs.Audio.Codecs
{
	// Token: 0x02000007 RID: 7
	public class OpusEncoder : IDisposable
	{
		// Token: 0x06000018 RID: 24 RVA: 0x000022C9 File Offset: 0x000006C9
		private OpusEncoder(IntPtr encoder, int inputSamplingRate, int inputChannels, Application application)
		{
			this._encoder = encoder;
			this.InputSamplingRate = inputSamplingRate;
			this.InputChannels = inputChannels;
			this.Application = application;
			this.MaxDataBytes = 4000;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000022FC File Offset: 0x000006FC
		public static OpusEncoder Create(int inputSamplingRate, int inputChannels, Application application)
		{
			if (inputSamplingRate != 8000 && inputSamplingRate != 12000 && inputSamplingRate != 16000 && inputSamplingRate != 24000 && inputSamplingRate != 48000)
			{
				throw new ArgumentOutOfRangeException("inputSamplingRate");
			}
			if (inputChannels != 1 && inputChannels != 2)
			{
				throw new ArgumentOutOfRangeException("inputChannels");
			}
			IntPtr intPtr2;
			IntPtr intPtr = API.opus_encoder_create(inputSamplingRate, inputChannels, (int)application, out intPtr2);
			if ((int)intPtr2 != 0)
			{
				throw new Exception("Exception occured while creating encoder");
			}
			return new OpusEncoder(intPtr, inputSamplingRate, inputChannels, application);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002390 File Offset: 0x00000790
		public unsafe byte[] Encode(byte[] inputPcmSamples, int sampleLength, out int encodedLength)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException("OpusEncoder");
			}
			int num = this.FrameCount(inputPcmSamples);
			byte[] array = new byte[this.MaxDataBytes];
			int num2;
			fixed (byte* ptr = (ref array != null && array.Length != 0 ? ref array[0] : ref *null))
			{
				IntPtr intPtr = new IntPtr((void*)ptr);
				num2 = API.opus_encode(this._encoder, inputPcmSamples, num, intPtr, sampleLength);
			}
			encodedLength = num2;
			if (num2 < 0)
			{
				string text = "Encoding failed - ";
				Errors errors = (Errors)num2;
				throw new Exception(text + errors.ToString());
			}
			return array;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002430 File Offset: 0x00000830
		public int FrameCount(byte[] pcmSamples)
		{
			int num = 16;
			int num2 = num / 8 * this.InputChannels;
			return pcmSamples.Length / num2;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002450 File Offset: 0x00000850
		public int FrameByteCount(int frameCount)
		{
			int num = 16;
			int num2 = num / 8 * this.InputChannels;
			return frameCount * num2;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000246E File Offset: 0x0000086E
		// (set) Token: 0x0600001E RID: 30 RVA: 0x00002476 File Offset: 0x00000876
		public int InputSamplingRate { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000247F File Offset: 0x0000087F
		// (set) Token: 0x06000020 RID: 32 RVA: 0x00002487 File Offset: 0x00000887
		public int InputChannels { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000021 RID: 33 RVA: 0x00002490 File Offset: 0x00000890
		// (set) Token: 0x06000022 RID: 34 RVA: 0x00002498 File Offset: 0x00000898
		public Application Application { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000024A1 File Offset: 0x000008A1
		// (set) Token: 0x06000024 RID: 36 RVA: 0x000024A9 File Offset: 0x000008A9
		public int MaxDataBytes { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000024B4 File Offset: 0x000008B4
		// (set) Token: 0x06000026 RID: 38 RVA: 0x00002514 File Offset: 0x00000914
		public int Bitrate
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException("OpusEncoder");
				}
				int num2;
				int num = API.opus_encoder_ctl(this._encoder, Ctl.GetBitrateRequest, out num2);
				if (num < 0)
				{
					string text = "Encoder error - ";
					Errors errors = (Errors)num;
					throw new Exception(text + errors.ToString());
				}
				return num2;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException("OpusEncoder");
				}
				int num = API.opus_encoder_ctl(this._encoder, Ctl.SetBitrateRequest, value);
				if (num < 0)
				{
					string text = "Encoder error - ";
					Errors errors = (Errors)num;
					throw new Exception(text + errors.ToString());
				}
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002570 File Offset: 0x00000970
		// (set) Token: 0x06000028 RID: 40 RVA: 0x000025DC File Offset: 0x000009DC
		public bool ForwardErrorCorrection
		{
			get
			{
				if (this._encoder == IntPtr.Zero)
				{
					throw new ObjectDisposedException("OpusEncoder");
				}
				int num2;
				int num = API.opus_encoder_ctl(this._encoder, Ctl.GetInbandFECRequest, out num2);
				if (num < 0)
				{
					string text = "Encoder error - ";
					Errors errors = (Errors)num;
					throw new Exception(text + errors.ToString());
				}
				return num2 > 0;
			}
			set
			{
				if (this._encoder == IntPtr.Zero)
				{
					throw new ObjectDisposedException("OpusEncoder");
				}
				int num = API.opus_encoder_ctl(this._encoder, Ctl.SetInbandFECRequest, (!value) ? 0 : 1);
				if (num < 0)
				{
					string text = "Encoder error - ";
					Errors errors = (Errors)num;
					throw new Exception(text + errors.ToString());
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002650 File Offset: 0x00000A50
		~OpusEncoder()
		{
			this.Dispose();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002680 File Offset: 0x00000A80
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			GC.SuppressFinalize(this);
			if (this._encoder != IntPtr.Zero)
			{
				API.opus_encoder_destroy(this._encoder);
				this._encoder = IntPtr.Zero;
			}
			this.disposed = true;
		}

		// Token: 0x0400001A RID: 26
		private IntPtr _encoder;

		// Token: 0x0400001F RID: 31
		private bool disposed;
	}
}
