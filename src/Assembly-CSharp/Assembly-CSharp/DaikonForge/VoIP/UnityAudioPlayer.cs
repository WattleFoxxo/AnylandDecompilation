using System;
using UnityEngine;

namespace DaikonForge.VoIP
{
	// Token: 0x02000017 RID: 23
	[AddComponentMenu("DFVoice/Unity Audio Player")]
	[RequireComponent(typeof(AudioSource))]
	public class UnityAudioPlayer : MonoBehaviour, IAudioPlayer
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000038CD File Offset: 0x00001CCD
		public bool PlayingSound
		{
			get
			{
				return base.GetComponent<AudioSource>().isPlaying;
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000038DC File Offset: 0x00001CDC
		private void Start()
		{
			this.playClip = AudioClip.Create("vc", this.frequency * 10, 1, this.frequency, false);
			if (base.GetComponent<AudioSource>() == null)
			{
				base.gameObject.AddComponent<AudioSource>();
			}
			base.GetComponent<AudioSource>().clip = this.playClip;
			base.GetComponent<AudioSource>().Stop();
			base.GetComponent<AudioSource>().loop = true;
			base.GetComponent<AudioSource>().spatialBlend = ((!this.IsThreeDimensional) ? 0f : 1f);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003974 File Offset: 0x00001D74
		private void Update()
		{
			if (base.GetComponent<AudioSource>().isPlaying)
			{
				if (this.lastTime > base.GetComponent<AudioSource>().timeSamples)
				{
					this.played += base.GetComponent<AudioSource>().clip.samples;
				}
				this.lastTime = base.GetComponent<AudioSource>().timeSamples;
				this.currentGain = Mathf.MoveTowards(this.currentGain, this.targetGain, Time.deltaTime * this.EqualizeSpeed);
				if (this.played + base.GetComponent<AudioSource>().timeSamples >= this.totalWritten)
				{
					base.GetComponent<AudioSource>().Pause();
					this.delayForFrames = 2;
				}
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003A27 File Offset: 0x00001E27
		private void OnDestroy()
		{
			global::UnityEngine.Object.Destroy(base.GetComponent<AudioSource>().clip);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003A3C File Offset: 0x00001E3C
		public void SetSampleRate(int sampleRate)
		{
			if (base.GetComponent<AudioSource>() == null)
			{
				return;
			}
			if (base.GetComponent<AudioSource>().clip != null && base.GetComponent<AudioSource>().clip.frequency == sampleRate)
			{
				return;
			}
			this.frequency = sampleRate;
			if (base.GetComponent<AudioSource>().clip != null)
			{
				global::UnityEngine.Object.Destroy(base.GetComponent<AudioSource>().clip);
			}
			this.playClip = AudioClip.Create("vc", this.frequency * 10, 1, this.frequency, false);
			base.GetComponent<AudioSource>().clip = this.playClip;
			base.GetComponent<AudioSource>().Stop();
			base.GetComponent<AudioSource>().loop = true;
			this.writeHead = 0;
			this.totalWritten = 0;
			this.delayForFrames = 0;
			this.lastTime = 0;
			this.played = 0;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003B24 File Offset: 0x00001F24
		public void BufferAudio(BigArray<float> audioData)
		{
			if (base.GetComponent<AudioSource>() == null)
			{
				return;
			}
			float[] array = TempArray<float>.Obtain(audioData.Length);
			audioData.CopyTo(0, array, 0, audioData.Length * 4);
			if (this.Equalize)
			{
				float maxAmplitude = AudioUtils.GetMaxAmplitude(array);
				this.targetGain = this.TargetEqualizeVolume / maxAmplitude;
				if (this.targetGain > this.MaxEqualization)
				{
					this.targetGain = this.MaxEqualization;
				}
				if (this.targetGain < this.currentGain)
				{
					this.currentGain = this.targetGain;
				}
				AudioUtils.ApplyGain(array, this.currentGain);
			}
			this.playClip.SetData(array, this.writeHead);
			TempArray<float>.Release(array);
			this.writeHead += audioData.Length;
			this.totalWritten += audioData.Length;
			this.writeHead %= this.playClip.samples;
			if (!base.GetComponent<AudioSource>().isPlaying)
			{
				this.delayForFrames--;
				if (this.delayForFrames <= 0)
				{
					base.GetComponent<AudioSource>().Play();
				}
			}
		}

		// Token: 0x04000049 RID: 73
		public bool IsThreeDimensional;

		// Token: 0x0400004A RID: 74
		public bool Equalize;

		// Token: 0x0400004B RID: 75
		public float EqualizeSpeed = 1f;

		// Token: 0x0400004C RID: 76
		public float TargetEqualizeVolume = 0.75f;

		// Token: 0x0400004D RID: 77
		public float MaxEqualization = 5f;

		// Token: 0x0400004E RID: 78
		private int frequency = 16000;

		// Token: 0x0400004F RID: 79
		private int writeHead;

		// Token: 0x04000050 RID: 80
		private int totalWritten;

		// Token: 0x04000051 RID: 81
		private AudioClip playClip;

		// Token: 0x04000052 RID: 82
		private int delayForFrames;

		// Token: 0x04000053 RID: 83
		private int lastTime;

		// Token: 0x04000054 RID: 84
		private int played;

		// Token: 0x04000055 RID: 85
		private float currentGain = 1f;

		// Token: 0x04000056 RID: 86
		private float targetGain = 1f;
	}
}
