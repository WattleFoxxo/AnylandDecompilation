using System;
using System.Diagnostics;
using UnityEngine;

namespace MirzaBeig.ParticleSystems
{
	// Token: 0x0200004C RID: 76
	[Serializable]
	public class ParticleSystems : MonoBehaviour
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0000BA52 File Offset: 0x00009E52
		// (set) Token: 0x060002CB RID: 715 RVA: 0x0000BA5A File Offset: 0x00009E5A
		public ParticleSystem[] particleSystems { get; set; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060002CC RID: 716 RVA: 0x0000BA64 File Offset: 0x00009E64
		// (remove) Token: 0x060002CD RID: 717 RVA: 0x0000BA9C File Offset: 0x00009E9C
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event ParticleSystems.onParticleSystemsDeadEventHandler onParticleSystemsDeadEvent;

		// Token: 0x060002CE RID: 718 RVA: 0x0000BAD2 File Offset: 0x00009ED2
		protected virtual void Awake()
		{
			this.particleSystems = base.GetComponentsInChildren<ParticleSystem>();
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000BAE0 File Offset: 0x00009EE0
		protected virtual void Start()
		{
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000BAE2 File Offset: 0x00009EE2
		protected virtual void Update()
		{
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000BAE4 File Offset: 0x00009EE4
		protected virtual void LateUpdate()
		{
			if (!this.isAlive() && this.onParticleSystemsDeadEvent != null)
			{
				this.onParticleSystemsDeadEvent();
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000BB08 File Offset: 0x00009F08
		public void reset()
		{
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				this.particleSystems[i].time = 0f;
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000BB40 File Offset: 0x00009F40
		public void play()
		{
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				this.particleSystems[i].Play(false);
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000BB74 File Offset: 0x00009F74
		public void pause()
		{
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				this.particleSystems[i].Pause(false);
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000BBA8 File Offset: 0x00009FA8
		public void stop()
		{
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				this.particleSystems[i].Stop(false);
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000BBDC File Offset: 0x00009FDC
		public void clear()
		{
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				this.particleSystems[i].Clear(false);
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000BC10 File Offset: 0x0000A010
		public void setLoop(bool loop)
		{
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				this.particleSystems[i].loop = loop;
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000BC44 File Offset: 0x0000A044
		public void setPlaybackSpeed(float speed)
		{
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				this.particleSystems[i].playbackSpeed = speed;
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000BC78 File Offset: 0x0000A078
		public void simulate(float time, bool reset = false)
		{
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				this.particleSystems[i].Simulate(time, false, reset);
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000BCB0 File Offset: 0x0000A0B0
		public bool isAlive()
		{
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				if (this.particleSystems[i] && this.particleSystems[i].IsAlive())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000BD00 File Offset: 0x0000A100
		public bool isPlaying(bool checkAll = false)
		{
			if (this.particleSystems.Length == 0)
			{
				return false;
			}
			if (!checkAll)
			{
				return this.particleSystems[0].isPlaying;
			}
			for (int i = 0; i < 0; i++)
			{
				if (!this.particleSystems[i].isPlaying)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000BD58 File Offset: 0x0000A158
		public int getParticleCount()
		{
			int num = 0;
			for (int i = 0; i < this.particleSystems.Length; i++)
			{
				if (this.particleSystems[i])
				{
					num += this.particleSystems[i].particleCount;
				}
			}
			return num;
		}

		// Token: 0x0200004D RID: 77
		// (Invoke) Token: 0x060002DE RID: 734
		public delegate void onParticleSystemsDeadEventHandler();
	}
}
