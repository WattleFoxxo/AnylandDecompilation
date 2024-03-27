using System;
using UnityEngine;

// Token: 0x020001EA RID: 490
public class EnvironmentManager : MonoBehaviour
{
	// Token: 0x060010C1 RID: 4289 RVA: 0x0009138B File Offset: 0x0008F78B
	private void Start()
	{
		this.SetToType(EnvironmentType.Default);
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x00091394 File Offset: 0x0008F794
	public void UpdateGravity()
	{
		this.SetToType(this.type);
	}

	// Token: 0x060010C3 RID: 4291 RVA: 0x000913A2 File Offset: 0x0008F7A2
	public void UpdateFloatingDust()
	{
		this.SetToType(this.type);
	}

	// Token: 0x060010C4 RID: 4292 RVA: 0x000913B0 File Offset: 0x0008F7B0
	public void SetToType(EnvironmentType newType)
	{
		this.type = newType;
		if (this.currentSound != null)
		{
			this.currentSound.Stop();
			this.currentSound.gameObject.SetActive(false);
			this.currentSound = null;
		}
		if (this.currentParticleSystem != null)
		{
			this.currentParticleSystem.Stop();
			this.currentParticleSystem.Clear();
			global::UnityEngine.Object.Destroy(this.currentParticleSystem.gameObject);
			this.currentParticleSystem = null;
		}
		Physics.gravity = new Vector3(0f, -9.81f, 0f);
		switch (this.type)
		{
		case EnvironmentType.Default:
			this.currentSound = this.defaultSound;
			break;
		case EnvironmentType.Day:
			this.currentSound = this.daySound;
			break;
		case EnvironmentType.Night:
			this.currentSound = this.nightSound;
			break;
		case EnvironmentType.Snow:
			this.currentSound = this.snowSound;
			this.currentParticleSystem = this.GetParticleSystem("Snow");
			break;
		case EnvironmentType.Rain:
			this.currentSound = this.rainSound;
			this.currentParticleSystem = this.GetParticleSystem("Rain");
			break;
		case EnvironmentType.Wind:
			this.currentSound = this.windSound;
			Physics.gravity = new Vector3(-2f, -9.81f, 0f);
			this.currentParticleSystem = this.GetParticleSystem("Wind");
			break;
		case EnvironmentType.Waves:
			this.currentSound = this.wavesSound;
			break;
		case EnvironmentType.Waves2:
			this.currentSound = this.waves2Sound;
			break;
		case EnvironmentType.Waves3:
			this.currentSound = this.waves3Sound;
			break;
		case EnvironmentType.Waves4:
			this.currentSound = this.waves4Sound;
			break;
		case EnvironmentType.Cave:
			this.currentSound = this.caveSound;
			break;
		case EnvironmentType.City:
			this.currentSound = this.citySound;
			break;
		case EnvironmentType.City2:
			this.currentSound = this.city2Sound;
			break;
		case EnvironmentType.City3:
			this.currentSound = this.city3Sound;
			break;
		case EnvironmentType.City4:
			this.currentSound = this.city4Sound;
			break;
		case EnvironmentType.City5:
			this.currentSound = this.city5Sound;
			break;
		case EnvironmentType.Day2:
			this.currentSound = this.day2Sound;
			break;
		case EnvironmentType.Day3:
			this.currentSound = this.day3Sound;
			break;
		case EnvironmentType.Night2:
			this.currentSound = this.night2Sound;
			break;
		case EnvironmentType.Night3:
			this.currentSound = this.night3Sound;
			break;
		case EnvironmentType.Rain2:
			this.currentSound = this.rain2Sound;
			break;
		case EnvironmentType.Wind2:
			this.currentSound = this.wind2Sound;
			break;
		case EnvironmentType.Wind3:
			this.currentSound = this.wind3Sound;
			break;
		case EnvironmentType.Wind4:
			this.currentSound = this.wind4Sound;
			break;
		case EnvironmentType.Wind5:
			this.currentSound = this.wind5Sound;
			break;
		case EnvironmentType.Jungle:
			this.currentSound = this.jungleSound;
			break;
		case EnvironmentType.Jungle2:
			this.currentSound = this.jungle2Sound;
			break;
		case EnvironmentType.Bubbles:
			this.currentSound = this.bubblesSound;
			break;
		case EnvironmentType.Bubbles2:
			this.currentSound = this.bubbles2Sound;
			break;
		case EnvironmentType.Machine:
			this.currentSound = this.machineSound;
			break;
		case EnvironmentType.Machine2:
			this.currentSound = this.machine2Sound;
			break;
		case EnvironmentType.Machine3:
			this.currentSound = this.machine3Sound;
			break;
		case EnvironmentType.Machine4:
			this.currentSound = this.machine4Sound;
			break;
		case EnvironmentType.Machine5:
			this.currentSound = this.machine5Sound;
			break;
		case EnvironmentType.Machine6:
			this.currentSound = this.machine6Sound;
			break;
		case EnvironmentType.Underwater:
			this.currentSound = this.underwaterSound;
			Physics.gravity = new Vector3(0f, -1f, 0f);
			this.currentParticleSystem = this.GetParticleSystem("UnderwaterBubbles");
			break;
		case EnvironmentType.Underwater2:
			this.currentSound = this.underwater2Sound;
			Physics.gravity = new Vector3(0f, -1f, 0f);
			this.currentParticleSystem = this.GetParticleSystem("UnderwaterBubbles");
			break;
		case EnvironmentType.Village:
			this.currentSound = this.villageSound;
			break;
		case EnvironmentType.Village2:
			this.currentSound = this.village2Sound;
			break;
		case EnvironmentType.Village3:
			this.currentSound = this.village3Sound;
			break;
		case EnvironmentType.Horror:
			this.currentSound = this.horrorSound;
			break;
		case EnvironmentType.Horror2:
			this.currentSound = this.horror2Sound;
			break;
		case EnvironmentType.Horror3:
			this.currentSound = this.horror3Sound;
			break;
		case EnvironmentType.Silence:
			this.currentSound = null;
			break;
		}
		if (this.currentSound != null)
		{
			this.currentSound.gameObject.SetActive(true);
		}
		if (Managers.areaManager.isZeroGravity)
		{
			Physics.gravity = Vector3.zero;
		}
		if (Managers.areaManager.hasFloatingDust)
		{
			if (this.floatingDustParticleSystem == null)
			{
				this.floatingDustParticleSystem = this.GetParticleSystem("FloatingDust");
				this.floatingDustParticleSystem.Play();
			}
		}
		else if (this.floatingDustParticleSystem != null)
		{
			this.floatingDustParticleSystem.Clear();
			this.floatingDustParticleSystem.Stop();
			global::UnityEngine.Object.Destroy(this.floatingDustParticleSystem.gameObject);
			this.floatingDustParticleSystem = null;
		}
		if (this.currentSound != null)
		{
			this.currentSound.Play();
		}
		if (this.currentParticleSystem != null)
		{
			this.currentParticleSystem.Play();
		}
	}

	// Token: 0x060010C5 RID: 4293 RVA: 0x0009198C File Offset: 0x0008FD8C
	private ParticleSystem GetParticleSystem(string name)
	{
		Transform transform = Managers.treeManager.GetTransform("/OurPersonRig/EnvironmentParticleSystems");
		string text = "ParticleSystems/Environment/" + name;
		GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load(text), transform) as GameObject;
		gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
		return gameObject.GetComponent<ParticleSystem>();
	}

	// Token: 0x060010C6 RID: 4294 RVA: 0x000919DE File Offset: 0x0008FDDE
	public void AdjustVolumeByFactor(float factor)
	{
		if (this.currentSound != null)
		{
			this.currentSound.volume *= factor;
		}
	}

	// Token: 0x04001092 RID: 4242
	public AudioSource defaultSound;

	// Token: 0x04001093 RID: 4243
	public AudioSource daySound;

	// Token: 0x04001094 RID: 4244
	public AudioSource day2Sound;

	// Token: 0x04001095 RID: 4245
	public AudioSource day3Sound;

	// Token: 0x04001096 RID: 4246
	public AudioSource nightSound;

	// Token: 0x04001097 RID: 4247
	public AudioSource night2Sound;

	// Token: 0x04001098 RID: 4248
	public AudioSource night3Sound;

	// Token: 0x04001099 RID: 4249
	public AudioSource snowSound;

	// Token: 0x0400109A RID: 4250
	public AudioSource rainSound;

	// Token: 0x0400109B RID: 4251
	public AudioSource rain2Sound;

	// Token: 0x0400109C RID: 4252
	public AudioSource windSound;

	// Token: 0x0400109D RID: 4253
	public AudioSource wind2Sound;

	// Token: 0x0400109E RID: 4254
	public AudioSource wind3Sound;

	// Token: 0x0400109F RID: 4255
	public AudioSource wind4Sound;

	// Token: 0x040010A0 RID: 4256
	public AudioSource wind5Sound;

	// Token: 0x040010A1 RID: 4257
	public AudioSource wavesSound;

	// Token: 0x040010A2 RID: 4258
	public AudioSource waves2Sound;

	// Token: 0x040010A3 RID: 4259
	public AudioSource waves3Sound;

	// Token: 0x040010A4 RID: 4260
	public AudioSource waves4Sound;

	// Token: 0x040010A5 RID: 4261
	public AudioSource caveSound;

	// Token: 0x040010A6 RID: 4262
	public AudioSource citySound;

	// Token: 0x040010A7 RID: 4263
	public AudioSource city2Sound;

	// Token: 0x040010A8 RID: 4264
	public AudioSource city3Sound;

	// Token: 0x040010A9 RID: 4265
	public AudioSource city4Sound;

	// Token: 0x040010AA RID: 4266
	public AudioSource city5Sound;

	// Token: 0x040010AB RID: 4267
	public AudioSource jungleSound;

	// Token: 0x040010AC RID: 4268
	public AudioSource jungle2Sound;

	// Token: 0x040010AD RID: 4269
	public AudioSource bubblesSound;

	// Token: 0x040010AE RID: 4270
	public AudioSource bubbles2Sound;

	// Token: 0x040010AF RID: 4271
	public AudioSource machineSound;

	// Token: 0x040010B0 RID: 4272
	public AudioSource machine2Sound;

	// Token: 0x040010B1 RID: 4273
	public AudioSource machine3Sound;

	// Token: 0x040010B2 RID: 4274
	public AudioSource machine4Sound;

	// Token: 0x040010B3 RID: 4275
	public AudioSource machine5Sound;

	// Token: 0x040010B4 RID: 4276
	public AudioSource machine6Sound;

	// Token: 0x040010B5 RID: 4277
	public AudioSource underwaterSound;

	// Token: 0x040010B6 RID: 4278
	public AudioSource underwater2Sound;

	// Token: 0x040010B7 RID: 4279
	public AudioSource villageSound;

	// Token: 0x040010B8 RID: 4280
	public AudioSource village2Sound;

	// Token: 0x040010B9 RID: 4281
	public AudioSource village3Sound;

	// Token: 0x040010BA RID: 4282
	public AudioSource horrorSound;

	// Token: 0x040010BB RID: 4283
	public AudioSource horror2Sound;

	// Token: 0x040010BC RID: 4284
	public AudioSource horror3Sound;

	// Token: 0x040010BD RID: 4285
	public GameObject snowParticlesObject;

	// Token: 0x040010BE RID: 4286
	public GameObject rainParticlesObject;

	// Token: 0x040010BF RID: 4287
	public GameObject windParticlesObject;

	// Token: 0x040010C0 RID: 4288
	public GameObject underwaterBubblesParticlesObject;

	// Token: 0x040010C1 RID: 4289
	public GameObject floatingDustParticlesObject;

	// Token: 0x040010C2 RID: 4290
	public EnvironmentType type;

	// Token: 0x040010C3 RID: 4291
	private AudioSource currentSound;

	// Token: 0x040010C4 RID: 4292
	private ParticleSystem currentParticleSystem;

	// Token: 0x040010C5 RID: 4293
	private ParticleSystem floatingDustParticleSystem;
}
