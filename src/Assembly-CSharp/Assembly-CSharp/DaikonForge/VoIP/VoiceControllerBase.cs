using System;
using UnityEngine;

namespace DaikonForge.VoIP
{
	// Token: 0x02000011 RID: 17
	public abstract class VoiceControllerBase : MonoBehaviour
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000049 RID: 73
		public abstract bool IsLocal { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002E2E File Offset: 0x0000122E
		public AudioInputDeviceBase AudioInputDevice
		{
			get
			{
				return this.microphone;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002E36 File Offset: 0x00001236
		public IAudioPlayer AudioOutputDevice
		{
			get
			{
				return this.speaker;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002E40 File Offset: 0x00001240
		protected virtual void Awake()
		{
			this.codec = this.GetCodec();
			this.microphone = base.GetComponent<AudioInputDeviceBase>();
			this.speaker = base.GetComponent(typeof(IAudioPlayer)) as IAudioPlayer;
			if (this.microphone == null)
			{
				Debug.LogError("No audio input component attached to speaker", this);
				return;
			}
			if (this.speaker == null)
			{
				Debug.LogError("No audio output component attached to speaker", this);
				return;
			}
			if (this.IsLocal)
			{
				this.microphone.OnAudioBufferReady += this.OnMicrophoneDataReady;
				this.microphone.StartRecording();
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002EE4 File Offset: 0x000012E4
		protected virtual void OnDestroy()
		{
			if (this.IsLocal && this.microphone != null)
			{
				this.microphone.OnAudioBufferReady -= this.OnMicrophoneDataReady;
				this.microphone.StopRecording();
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002F30 File Offset: 0x00001330
		protected virtual void OnAudioDataEncoded(VoicePacketWrapper encodedFrame)
		{
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002F34 File Offset: 0x00001334
		protected virtual IAudioCodec GetCodec()
		{
			bool flag = true;
			if (flag)
			{
				AudioUtils.FrequencyProvider = new OpusCodec.FrequencyProvider();
				return new OpusCodec(64000);
			}
			AudioUtils.FrequencyProvider = new SpeexCodec.FrequencyProvider();
			return new SpeexCodec(true);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002F6E File Offset: 0x0000136E
		protected void SkipFrame()
		{
			this.nextExpectedIndex += 1UL;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002F80 File Offset: 0x00001380
		protected virtual void ReceiveAudioData(VoicePacketWrapper encodedFrame)
		{
			if (!this.IsLocal || this.DebugAudio)
			{
				if (encodedFrame.Index < this.nextExpectedIndex)
				{
					return;
				}
				if (this.Mute)
				{
					this.nextExpectedIndex = encodedFrame.Index + 1UL;
					return;
				}
				this.speaker.SetSampleRate((int)encodedFrame.Frequency * 1000);
				if (this.nextExpectedIndex != 0UL && encodedFrame.Index != this.nextExpectedIndex && this.speaker.PlayingSound)
				{
					int num = (int)(encodedFrame.Index - this.nextExpectedIndex);
					for (int i = 0; i < num; i++)
					{
						BigArray<float> bigArray = this.codec.GenerateMissingFrame((int)encodedFrame.Frequency);
						this.speaker.BufferAudio(bigArray);
					}
				}
				BigArray<float> bigArray2 = this.codec.DecodeFrame(encodedFrame);
				this.speaker.BufferAudio(bigArray2);
				this.nextExpectedIndex = encodedFrame.Index + 1UL;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003084 File Offset: 0x00001484
		protected virtual void OnMicrophoneDataReady(BigArray<float> newData, int frequency)
		{
			if (!this.IsLocal)
			{
				return;
			}
			this.codec.OnAudioAvailable(newData);
			for (VoicePacketWrapper? voicePacketWrapper = this.codec.GetNextEncodedFrame(frequency); voicePacketWrapper != null; voicePacketWrapper = this.codec.GetNextEncodedFrame(frequency))
			{
				VoicePacketWrapper value = voicePacketWrapper.Value;
				ulong num;
				this.nextFrameIndex = (num = this.nextFrameIndex) + 1UL;
				value.Index = num;
				this.OnAudioDataEncoded(new VoicePacketWrapper?(value).Value);
			}
		}

		// Token: 0x0400002F RID: 47
		public bool DebugAudio;

		// Token: 0x04000030 RID: 48
		public bool Mute;

		// Token: 0x04000031 RID: 49
		protected AudioInputDeviceBase microphone;

		// Token: 0x04000032 RID: 50
		protected IAudioPlayer speaker;

		// Token: 0x04000033 RID: 51
		protected IAudioCodec codec;

		// Token: 0x04000034 RID: 52
		protected ulong nextFrameIndex;

		// Token: 0x04000035 RID: 53
		protected ulong nextExpectedIndex;
	}
}
