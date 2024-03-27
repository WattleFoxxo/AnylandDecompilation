using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MirzaBeig.ParticleSystems.Demos
{
	// Token: 0x02000046 RID: 70
	[Serializable]
	public class ParticleManager : MonoBehaviour
	{
		// Token: 0x060002A6 RID: 678 RVA: 0x0000B21C File Offset: 0x0000961C
		public void init()
		{
			this.particlePrefabs = base.GetComponentsInChildren<ParticleSystems>(true).ToList<ParticleSystems>();
			this.particlePrefabs.AddRange(this.particlePrefabsAppend);
			if (this.disableChildrenAtStart)
			{
				for (int i = 0; i < this.particlePrefabs.Count; i++)
				{
					this.particlePrefabs[i].gameObject.SetActive(false);
				}
			}
			for (int j = 0; j < this.particlePrefabs.Count; j++)
			{
				Light[] componentsInChildren = this.particlePrefabs[j].GetComponentsInChildren<Light>(true);
				for (int k = 0; k < componentsInChildren.Length; k++)
				{
					this.particlePrefabLightGameObjects.Add(componentsInChildren[k].gameObject);
				}
			}
			this.initialized = true;
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000B2E7 File Offset: 0x000096E7
		protected virtual void Awake()
		{
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000B2E9 File Offset: 0x000096E9
		protected virtual void Start()
		{
			if (this.initialized)
			{
				this.init();
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000B2FC File Offset: 0x000096FC
		public virtual void next()
		{
			this.currentParticlePrefab++;
			if (this.currentParticlePrefab > this.particlePrefabs.Count - 1)
			{
				this.currentParticlePrefab = 0;
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000B32B File Offset: 0x0000972B
		public virtual void previous()
		{
			this.currentParticlePrefab--;
			if (this.currentParticlePrefab < 0)
			{
				this.currentParticlePrefab = this.particlePrefabs.Count - 1;
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000B35C File Offset: 0x0000975C
		public string getCurrentPrefabName(bool shorten = false)
		{
			string text = this.particlePrefabs[this.currentParticlePrefab].name;
			if (shorten)
			{
				int num = 0;
				for (int i = 0; i < this.prefabNameUnderscoreCountCutoff; i++)
				{
					num = text.IndexOf("_", num) + 1;
					if (num == 0)
					{
						MonoBehaviour.print("Iteration of underscore not found.");
						break;
					}
				}
				text = text.Substring(num, text.Length - num);
			}
			return string.Concat(new string[]
			{
				"PARTICLE SYSTEM: #",
				(this.currentParticlePrefab + 1).ToString("00"),
				" / ",
				this.particlePrefabs.Count.ToString("00"),
				" (",
				text,
				")"
			});
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000B436 File Offset: 0x00009836
		public virtual int getParticleCount()
		{
			return 0;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000B43C File Offset: 0x0000983C
		public void setLights(bool value)
		{
			for (int i = 0; i < this.particlePrefabLightGameObjects.Count; i++)
			{
				this.particlePrefabLightGameObjects[i].SetActive(value);
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000B477 File Offset: 0x00009877
		protected virtual void Update()
		{
		}

		// Token: 0x04000176 RID: 374
		protected List<ParticleSystems> particlePrefabs;

		// Token: 0x04000177 RID: 375
		protected List<GameObject> particlePrefabLightGameObjects = new List<GameObject>();

		// Token: 0x04000178 RID: 376
		public int currentParticlePrefab;

		// Token: 0x04000179 RID: 377
		public List<ParticleSystems> particlePrefabsAppend;

		// Token: 0x0400017A RID: 378
		public int prefabNameUnderscoreCountCutoff = 4;

		// Token: 0x0400017B RID: 379
		public bool disableChildrenAtStart = true;

		// Token: 0x0400017C RID: 380
		private bool initialized;
	}
}
