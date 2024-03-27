using System;
using UnityEngine;

namespace DaikonForge.VoIP
{
	// Token: 0x02000016 RID: 22
	[AddComponentMenu("DFVoice/Microphone Input Device")]
	public class MicrophoneInputDevice : AudioInputDeviceBase
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000034D6 File Offset: 0x000018D6
		public string ActiveDevice
		{
			get
			{
				return this.device;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000034DE File Offset: 0x000018DE
		// (set) Token: 0x06000067 RID: 103 RVA: 0x000034E6 File Offset: 0x000018E6
		public bool didJustMeetAmplitudeThreshold { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000034EF File Offset: 0x000018EF
		// (set) Token: 0x06000069 RID: 105 RVA: 0x000034F7 File Offset: 0x000018F7
		public float lastMaxAmplitude { get; private set; }

		// Token: 0x0600006A RID: 106 RVA: 0x00003500 File Offset: 0x00001900
		public override void StartRecording()
		{
			if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
			{
				Debug.LogWarning("StartRecording: Webplayer microphone access denied");
				return;
			}
			this.device = MicrophoneInputDevice.DefaultMicrophone;
			bool flag = Microphone.devices != null && Microphone.devices.Length >= 1;
			this.prevReadPosition = 0;
			this.recordingFrequency = AudioUtils.GetFrequency(this.Mode);
			int num = 0;
			int num2 = 0;
			Microphone.GetDeviceCaps(this.device, out num, out num2);
			if (num2 == 0)
			{
				num2 = 48000;
			}
			int num3 = Mathf.Clamp(this.recordingFrequency, num, num2);
			this.resampleBuffer = new BigArray<float>(320, 0);
			this.recordedAudio = Microphone.Start(this.device, true, 5, num3);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000035B3 File Offset: 0x000019B3
		public override void StopRecording()
		{
			Microphone.End(this.device);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000035C0 File Offset: 0x000019C0
		public void ChangeMicrophoneDevice(string newDevice)
		{
			this.StopRecording();
			MicrophoneInputDevice.DefaultMicrophone = newDevice;
			this.StartRecording();
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000035D4 File Offset: 0x000019D4
		private void Update()
		{
			if (!Microphone.IsRecording(this.device) || this.recordedAudio == null)
			{
				return;
			}
			float[] array = TempArray<float>.Obtain(320);
			bool flag = this.resampleBuffer == null;
			if (flag)
			{
				this.resampleBuffer = new BigArray<float>(320, 0);
			}
			int i = Microphone.GetPosition(this.device);
			if (i >= this.prevReadPosition + 320)
			{
				while (i >= this.prevReadPosition + 320)
				{
					this.recordedAudio.GetData(array, this.prevReadPosition);
					bool flag2;
					if (this.exceedsVolumeThreshold(array, out flag2) || (flag2 && this.RecentlyExceededVolumeThreshold()))
					{
						this.resample(array);
						base.bufferReady(this.resampleBuffer, this.recordingFrequency);
					}
					this.prevReadPosition += 320;
				}
			}
			else if (this.prevReadPosition > i)
			{
				int num = i + this.recordedAudio.samples;
				for (int j = num - this.prevReadPosition; j >= 320; j = num - this.prevReadPosition)
				{
					this.recordedAudio.GetData(array, this.prevReadPosition);
					bool flag3;
					if (this.exceedsVolumeThreshold(array, out flag3) || (flag3 && this.RecentlyExceededVolumeThreshold()))
					{
						this.resample(array);
						base.bufferReady(this.resampleBuffer, this.recordingFrequency);
					}
					this.prevReadPosition += 320;
					if (this.prevReadPosition >= this.recordedAudio.samples)
					{
						this.prevReadPosition -= this.recordedAudio.samples;
						break;
					}
					num = i + this.recordedAudio.samples;
				}
			}
			TempArray<float>.Release(array);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000037B0 File Offset: 0x00001BB0
		private bool exceedsVolumeThreshold(float[] data, out bool hadSomeVolume)
		{
			this.lastMaxAmplitude = Mathf.Max(data);
			this.didJustMeetAmplitudeThreshold = this.lastMaxAmplitude >= 0.005f;
			if (this.didJustMeetAmplitudeThreshold)
			{
				this.lastTimeVolumeThresholdWasExceeded = Time.time;
			}
			hadSomeVolume = this.lastMaxAmplitude > 0f;
			return this.didJustMeetAmplitudeThreshold;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000380C File Offset: 0x00001C0C
		private bool RecentlyExceededVolumeThreshold()
		{
			float num = Time.time - this.lastTimeVolumeThresholdWasExceeded;
			return num <= 2f;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003835 File Offset: 0x00001C35
		private void resample(float[] tempArray)
		{
			this.resampleBuffer.Resize(tempArray.Length);
			this.resampleBuffer.CopyFrom(tempArray, 0, 0, tempArray.Length * 4);
			AudioUtils.Resample(this.resampleBuffer, this.recordedAudio.frequency, this.recordingFrequency);
		}

		// Token: 0x04000039 RID: 57
		public static string DefaultMicrophone;

		// Token: 0x0400003A RID: 58
		private const float amplitudeThreshold = 0.005f;

		// Token: 0x0400003B RID: 59
		public const int ChunkSize = 320;

		// Token: 0x0400003C RID: 60
		private const bool smoothAudioChunksToAvoidClickSounds = true;

		// Token: 0x0400003D RID: 61
		private const float secondsToExtendToSmoothAudioAndAvoidClickSounds = 2f;

		// Token: 0x0400003E RID: 62
		public FrequencyMode Mode = FrequencyMode.Wide;

		// Token: 0x0400003F RID: 63
		public KeyCode PushToTalk;

		// Token: 0x04000042 RID: 66
		private AudioClip recordedAudio;

		// Token: 0x04000043 RID: 67
		private int prevReadPosition;

		// Token: 0x04000044 RID: 68
		private BigArray<float> resampleBuffer;

		// Token: 0x04000045 RID: 69
		private string device;

		// Token: 0x04000046 RID: 70
		private int recordingFrequency;

		// Token: 0x04000047 RID: 71
		private float pushToTalkTimer;

		// Token: 0x04000048 RID: 72
		private float lastTimeVolumeThresholdWasExceeded = -1f;
	}
}
