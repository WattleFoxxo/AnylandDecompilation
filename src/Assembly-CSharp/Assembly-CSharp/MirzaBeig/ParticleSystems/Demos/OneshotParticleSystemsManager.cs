using System;
using System.Collections.Generic;
using UnityEngine;

namespace MirzaBeig.ParticleSystems.Demos
{
	// Token: 0x02000045 RID: 69
	[Serializable]
	public class OneshotParticleSystemsManager : ParticleManager
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000B555 File Offset: 0x00009955
		// (set) Token: 0x0600029C RID: 668 RVA: 0x0000B55D File Offset: 0x0000995D
		public bool disableSpawn { get; set; }

		// Token: 0x0600029D RID: 669 RVA: 0x0000B566 File Offset: 0x00009966
		protected override void Awake()
		{
			base.Awake();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000B56E File Offset: 0x0000996E
		protected override void Start()
		{
			base.Start();
			this.disableSpawn = false;
			this.spawnedPrefabs = new List<ParticleSystems>();
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000B588 File Offset: 0x00009988
		private void OnEnable()
		{
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000B58C File Offset: 0x0000998C
		public void clear()
		{
			if (this.spawnedPrefabs != null)
			{
				for (int i = 0; i < this.spawnedPrefabs.Count; i++)
				{
					if (this.spawnedPrefabs[i])
					{
						global::UnityEngine.Object.Destroy(this.spawnedPrefabs[i].gameObject);
					}
				}
				this.spawnedPrefabs.Clear();
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000B5F7 File Offset: 0x000099F7
		protected override void Update()
		{
			base.Update();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000B600 File Offset: 0x00009A00
		public void instantiateParticlePrefab(Vector2 mousePosition, float maxDistance)
		{
			if (this.spawnedPrefabs != null && !this.disableSpawn)
			{
				Vector3 vector = mousePosition;
				vector.z = maxDistance;
				Vector3 vector2 = Camera.main.ScreenToWorldPoint(vector);
				Vector3 vector3 = vector2 - Camera.main.transform.position;
				RaycastHit raycastHit;
				Physics.Raycast(Camera.main.transform.position, vector3, out raycastHit, maxDistance);
				Vector3 vector4;
				if (raycastHit.collider)
				{
					vector4 = raycastHit.point;
				}
				else
				{
					vector4 = vector2;
				}
				ParticleSystems particleSystems = this.particlePrefabs[this.currentParticlePrefab];
				ParticleSystems particleSystems2 = global::UnityEngine.Object.Instantiate<ParticleSystems>(particleSystems, vector4, particleSystems.transform.rotation);
				particleSystems2.gameObject.SetActive(true);
				particleSystems2.transform.parent = base.transform;
				this.spawnedPrefabs.Add(particleSystems2);
			}
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000B6E5 File Offset: 0x00009AE5
		public void randomize()
		{
			this.currentParticlePrefab = global::UnityEngine.Random.Range(0, this.particlePrefabs.Count);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000B700 File Offset: 0x00009B00
		public override int getParticleCount()
		{
			int num = 0;
			if (this.spawnedPrefabs != null)
			{
				for (int i = 0; i < this.spawnedPrefabs.Count; i++)
				{
					if (this.spawnedPrefabs[i])
					{
						num += this.spawnedPrefabs[i].getParticleCount();
					}
					else
					{
						this.spawnedPrefabs.RemoveAt(i);
					}
				}
			}
			return num;
		}

		// Token: 0x04000173 RID: 371
		public LayerMask mouseRaycastLayerMask;

		// Token: 0x04000174 RID: 372
		private List<ParticleSystems> spawnedPrefabs;
	}
}
