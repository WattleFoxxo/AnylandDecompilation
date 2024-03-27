using System;
using System.Runtime.InteropServices;
using POpusCodec.Enums;

namespace POpusCodec
{
	// Token: 0x02000193 RID: 403
	public class OpusDecoder : IDisposable
	{
		// Token: 0x0600056C RID: 1388 RVA: 0x00004AB0 File Offset: 0x00002CB0
		public OpusDecoder(SamplingRate outputSamplingRateHz, Channels numChannels)
		{
			if (outputSamplingRateHz != SamplingRate.Sampling08000 && outputSamplingRateHz != SamplingRate.Sampling12000 && outputSamplingRateHz != SamplingRate.Sampling16000 && outputSamplingRateHz != SamplingRate.Sampling24000 && outputSamplingRateHz != SamplingRate.Sampling48000)
			{
				throw new ArgumentOutOfRangeException("outputSamplingRateHz", "Must use one of the pre-defined sampling rates (" + outputSamplingRateHz + ")");
			}
			if (numChannels != Channels.Mono && numChannels != Channels.Stereo)
			{
				throw new ArgumentOutOfRangeException("numChannels", "Must be Mono or Stereo");
			}
			this._channelCount = (int)numChannels;
			this._handle = Wrapper.opus_decoder_create(outputSamplingRateHz, numChannels);
			this._version = Marshal.PtrToStringAnsi(Wrapper.opus_get_version_string());
			if (this._handle == IntPtr.Zero)
			{
				throw new OpusException(OpusStatusCode.AllocFail, "Memory was not allocated for the encoder");
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x00004B9B File Offset: 0x00002D9B
		public string Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x00004BA3 File Offset: 0x00002DA3
		public Bandwidth? PreviousPacketBandwidth
		{
			get
			{
				return this._previousPacketBandwidth;
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00004BAC File Offset: 0x00002DAC
		public short[] DecodePacketLost()
		{
			this._previousPacketInvalid = true;
			int num = Wrapper.get_opus_decoder_ctl(this._handle, OpusCtlGetRequest.LastPacketDurationRequest);
			short[] array = new short[num * this._channelCount];
			int num2 = Wrapper.opus_decode(this._handle, null, array, 0, this._channelCount);
			if (num2 == 0)
			{
				return new short[0];
			}
			short[] array2 = new short[num2 * this._channelCount];
			Buffer.BlockCopy(array, 0, array2, 0, array2.Length * 2);
			return array2;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00004C20 File Offset: 0x00002E20
		public float[] DecodePacketLostFloat()
		{
			this._previousPacketInvalid = true;
			int num = Wrapper.get_opus_decoder_ctl(this._handle, OpusCtlGetRequest.LastPacketDurationRequest);
			if (this.lostDataFloats == null || this.lostDataFloats.Length != num * this._channelCount)
			{
				this.lostDataFloats = new float[num * this._channelCount];
			}
			int num2 = Wrapper.opus_decode(this._handle, null, this.lostDataFloats, 0, this._channelCount);
			if (num2 == 0)
			{
				return new float[0];
			}
			if (this.pcm == null || this.pcm.Length != num2 * this._channelCount)
			{
				this.pcm = new float[num2 * this._channelCount];
			}
			Buffer.BlockCopy(this.lostDataFloats, 0, this.pcm, 0, this.pcm.Length * 4);
			return this.pcm;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00004CF8 File Offset: 0x00002EF8
		public short[] DecodePacket(byte[] packetData)
		{
			short[] array = new short[5760 * this._channelCount];
			int num = Wrapper.opus_packet_get_bandwidth(packetData);
			int num2;
			if (num == -4)
			{
				num2 = Wrapper.opus_decode(this._handle, null, array, 0, this._channelCount);
				this._previousPacketInvalid = true;
			}
			else
			{
				this._previousPacketBandwidth = new Bandwidth?((Bandwidth)num);
				num2 = Wrapper.opus_decode(this._handle, packetData, array, 0, this._channelCount);
				this._previousPacketInvalid = false;
			}
			if (num2 == 0)
			{
				return new short[0];
			}
			short[] array2 = new short[num2 * this._channelCount];
			Buffer.BlockCopy(array, 0, array2, 0, array2.Length * 2);
			return array2;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00004D9C File Offset: 0x00002F9C
		public float[] DecodePacketFloat(byte[] packetData)
		{
			if (this.tempData == null || this.tempData.Length != 5760 * this._channelCount)
			{
				this.tempData = new float[5760 * this._channelCount];
			}
			int num = Wrapper.opus_packet_get_bandwidth(packetData);
			int num2;
			if (num == -4)
			{
				num2 = Wrapper.opus_decode(this._handle, null, this.tempData, 0, this._channelCount);
				this._previousPacketInvalid = true;
			}
			else
			{
				this._previousPacketBandwidth = new Bandwidth?((Bandwidth)num);
				num2 = Wrapper.opus_decode(this._handle, packetData, this.tempData, 0, this._channelCount);
				this._previousPacketInvalid = false;
			}
			if (num2 == 0)
			{
				return new float[0];
			}
			if (this.pcm == null || this.pcm.Length != num2 * this._channelCount)
			{
				this.pcm = new float[num2 * this._channelCount];
			}
			Buffer.BlockCopy(this.tempData, 0, this.pcm, 0, this.pcm.Length * 4);
			return this.pcm;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00004EAB File Offset: 0x000030AB
		public void Dispose()
		{
			if (this._handle != IntPtr.Zero)
			{
				Wrapper.opus_decoder_destroy(this._handle);
				this._handle = IntPtr.Zero;
			}
		}

		// Token: 0x04000550 RID: 1360
		private IntPtr _handle = IntPtr.Zero;

		// Token: 0x04000551 RID: 1361
		private string _version = string.Empty;

		// Token: 0x04000552 RID: 1362
		private const int MaxFrameSize = 5760;

		// Token: 0x04000553 RID: 1363
		private bool _previousPacketInvalid;

		// Token: 0x04000554 RID: 1364
		private int _channelCount = 2;

		// Token: 0x04000555 RID: 1365
		private Bandwidth? _previousPacketBandwidth;

		// Token: 0x04000556 RID: 1366
		private float[] lostDataFloats;

		// Token: 0x04000557 RID: 1367
		private float[] tempData;

		// Token: 0x04000558 RID: 1368
		private float[] pcm;
	}
}
