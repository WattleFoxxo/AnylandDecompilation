using System;
using System.Runtime.InteropServices;
using POpusCodec.Enums;

namespace POpusCodec
{
	// Token: 0x02000194 RID: 404
	public class OpusEncoder : IDisposable
	{
		// Token: 0x06000574 RID: 1396 RVA: 0x00004ED8 File Offset: 0x000030D8
		public OpusEncoder(SamplingRate inputSamplingRateHz, Channels numChannels, int bitrate, OpusApplicationType applicationType, Delay encoderDelay)
		{
			if (inputSamplingRateHz != SamplingRate.Sampling08000 && inputSamplingRateHz != SamplingRate.Sampling12000 && inputSamplingRateHz != SamplingRate.Sampling16000 && inputSamplingRateHz != SamplingRate.Sampling24000 && inputSamplingRateHz != SamplingRate.Sampling48000)
			{
				throw new ArgumentOutOfRangeException("inputSamplingRateHz", "Must use one of the pre-defined sampling rates(" + inputSamplingRateHz + ")");
			}
			if (numChannels != Channels.Mono && numChannels != Channels.Stereo)
			{
				throw new ArgumentOutOfRangeException("numChannels", "Must be Mono or Stereo");
			}
			if (applicationType != OpusApplicationType.Audio && applicationType != OpusApplicationType.RestrictedLowDelay && applicationType != OpusApplicationType.Voip)
			{
				throw new ArgumentOutOfRangeException("applicationType", "Must use one of the pre-defined application types (" + applicationType + ")");
			}
			if (encoderDelay != Delay.Delay10ms && encoderDelay != Delay.Delay20ms && encoderDelay != Delay.Delay2dot5ms && encoderDelay != Delay.Delay40ms && encoderDelay != Delay.Delay5ms && encoderDelay != Delay.Delay60ms)
			{
				throw new ArgumentOutOfRangeException("encoderDelay", "Must use one of the pre-defined delay values (" + encoderDelay + ")");
			}
			this._inputSamplingRate = inputSamplingRateHz;
			this._inputChannels = numChannels;
			this._handle = Wrapper.opus_encoder_create(inputSamplingRateHz, numChannels, applicationType);
			this._version = Marshal.PtrToStringAnsi(Wrapper.opus_get_version_string());
			if (this._handle == IntPtr.Zero)
			{
				throw new OpusException(OpusStatusCode.AllocFail, "Memory was not allocated for the encoder");
			}
			this.EncoderDelay = encoderDelay;
			this.Bitrate = bitrate;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x000050A4 File Offset: 0x000032A4
		public SamplingRate InputSamplingRate
		{
			get
			{
				return this._inputSamplingRate;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x000050AC File Offset: 0x000032AC
		public Channels InputChannels
		{
			get
			{
				return this._inputChannels;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x000050B4 File Offset: 0x000032B4
		public string Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x000050FC File Offset: 0x000032FC
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x000050BC File Offset: 0x000032BC
		public Delay EncoderDelay
		{
			get
			{
				return this._encoderDelay;
			}
			set
			{
				this._encoderDelay = value;
				this._frameSizePerChannel = (int)((int)(this._inputSamplingRate / (SamplingRate)1000) * (int)this._encoderDelay / 2m);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x00005104 File Offset: 0x00003304
		public int FrameSizePerChannel
		{
			get
			{
				return this._frameSizePerChannel;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0000510C File Offset: 0x0000330C
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x0000511E File Offset: 0x0000331E
		public int Bitrate
		{
			get
			{
				return Wrapper.get_opus_encoder_ctl(this._handle, OpusCtlGetRequest.Bitrate);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(this._handle, OpusCtlSetRequest.Bitrate, value);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00005131 File Offset: 0x00003331
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x00005143 File Offset: 0x00003343
		public Bandwidth MaxBandwidth
		{
			get
			{
				return (Bandwidth)Wrapper.get_opus_encoder_ctl(this._handle, OpusCtlGetRequest.MaxBandwidth);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(this._handle, OpusCtlSetRequest.MaxBandwidth, (int)value);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x00005156 File Offset: 0x00003356
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x00005168 File Offset: 0x00003368
		public Complexity Complexity
		{
			get
			{
				return (Complexity)Wrapper.get_opus_encoder_ctl(this._handle, OpusCtlGetRequest.Complexity);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(this._handle, OpusCtlSetRequest.Complexity, (int)value);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x0000517B File Offset: 0x0000337B
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x0000518D File Offset: 0x0000338D
		public int ExpectedPacketLossPercentage
		{
			get
			{
				return Wrapper.get_opus_encoder_ctl(this._handle, OpusCtlGetRequest.PacketLossPercentage);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(this._handle, OpusCtlSetRequest.PacketLossPercentage, value);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x000051A0 File Offset: 0x000033A0
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x000051B2 File Offset: 0x000033B2
		public SignalHint SignalHint
		{
			get
			{
				return (SignalHint)Wrapper.get_opus_encoder_ctl(this._handle, OpusCtlGetRequest.Signal);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(this._handle, OpusCtlSetRequest.Signal, (int)value);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x000051C5 File Offset: 0x000033C5
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x000051D7 File Offset: 0x000033D7
		public ForceChannels ForceChannels
		{
			get
			{
				return (ForceChannels)Wrapper.get_opus_encoder_ctl(this._handle, OpusCtlGetRequest.ForceChannels);
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(this._handle, OpusCtlSetRequest.ForceChannels, (int)value);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x000051EA File Offset: 0x000033EA
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x000051FF File Offset: 0x000033FF
		public bool UseInbandFEC
		{
			get
			{
				return Wrapper.get_opus_encoder_ctl(this._handle, OpusCtlGetRequest.InbandFec) == 1;
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(this._handle, OpusCtlSetRequest.InbandFec, (!value) ? 0 : 1);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0000521E File Offset: 0x0000341E
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x00005233 File Offset: 0x00003433
		public bool UseUnconstrainedVBR
		{
			get
			{
				return Wrapper.get_opus_encoder_ctl(this._handle, OpusCtlGetRequest.VBRConstraint) == 0;
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(this._handle, OpusCtlSetRequest.VBRConstraint, (!value) ? 1 : 0);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x00005252 File Offset: 0x00003452
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x00005267 File Offset: 0x00003467
		public bool DtxEnabled
		{
			get
			{
				return Wrapper.get_opus_encoder_ctl(this._handle, OpusCtlGetRequest.Dtx) == 1;
			}
			set
			{
				Wrapper.set_opus_encoder_ctl(this._handle, OpusCtlSetRequest.Dtx, (!value) ? 0 : 1);
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00005288 File Offset: 0x00003488
		public byte[] Encode(short[] pcmSamples)
		{
			int num = Wrapper.opus_encode(this._handle, pcmSamples, this._frameSizePerChannel, this.writePacket);
			if (num <= 1)
			{
				return new byte[0];
			}
			byte[] array = new byte[num];
			Buffer.BlockCopy(this.writePacket, 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x000052D8 File Offset: 0x000034D8
		public byte[] Encode(float[] pcmSamples)
		{
			int num = Wrapper.opus_encode(this._handle, pcmSamples, this._frameSizePerChannel, this.writePacket);
			if (num <= 1)
			{
				return new byte[0];
			}
			byte[] array = new byte[num];
			Buffer.BlockCopy(this.writePacket, 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00005325 File Offset: 0x00003525
		public void Dispose()
		{
			if (this._handle != IntPtr.Zero)
			{
				Wrapper.opus_encoder_destroy(this._handle);
				this._handle = IntPtr.Zero;
			}
		}

		// Token: 0x04000559 RID: 1369
		public const int BitrateMax = -1;

		// Token: 0x0400055A RID: 1370
		private IntPtr _handle = IntPtr.Zero;

		// Token: 0x0400055B RID: 1371
		private string _version = string.Empty;

		// Token: 0x0400055C RID: 1372
		private const int RecommendedMaxPacketSize = 4000;

		// Token: 0x0400055D RID: 1373
		private int _frameSizePerChannel = 960;

		// Token: 0x0400055E RID: 1374
		private SamplingRate _inputSamplingRate = SamplingRate.Sampling48000;

		// Token: 0x0400055F RID: 1375
		private Channels _inputChannels = Channels.Stereo;

		// Token: 0x04000560 RID: 1376
		private byte[] writePacket = new byte[4000];

		// Token: 0x04000561 RID: 1377
		private Delay _encoderDelay = Delay.Delay20ms;
	}
}
