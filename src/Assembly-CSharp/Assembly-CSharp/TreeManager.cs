using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000201 RID: 513
public class TreeManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000241 RID: 577
	// (get) Token: 0x060013D1 RID: 5073 RVA: 0x000B3D9F File Offset: 0x000B219F
	// (set) Token: 0x060013D2 RID: 5074 RVA: 0x000B3DA7 File Offset: 0x000B21A7
	public ManagerStatus status { get; private set; }

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x060013D3 RID: 5075 RVA: 0x000B3DB0 File Offset: 0x000B21B0
	// (set) Token: 0x060013D4 RID: 5076 RVA: 0x000B3DB8 File Offset: 0x000B21B8
	public string failMessage { get; private set; }

	// Token: 0x060013D5 RID: 5077 RVA: 0x000B3DC4 File Offset: 0x000B21C4
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.cachedObjects.Add("/DialogParts", this.dialogParts_preCached);
		this.cachedObjects.Add("/Universe", this.universe_preCached);
		this.cachedObjects.Add("/Universe/EnvironmentManager", this.environmentManager_preCached);
		this.cachedObjects.Add("/Universe/EnvironmentChangers", this.environmentChangers_preCached);
		this.cachedObjects.Add("/Universe/EnvironmentChangers/sun", this.sunEnvironmentChanger_preCached);
		this.cachedObjects.Add("/Universe/EnvironmentChangers/night", this.nightEnvironmentChanger_preCached);
		this.cachedObjects.Add("/Universe/EnvironmentChangers/ambientLight", this.ambientLightEnvironmentChanger_preCached);
		this.cachedObjects.Add("/Universe/EnvironmentChangers/fog", this.fogEnvironmentChanger_preCached);
		this.cachedObjects.Add("/Universe/EnvironmentChangers/clouds", this.cloudsEnvironmentChanger_preCached);
		this.cachedObjects.Add("/Universe/MiscellaneousSourceObjects", this.miscellaneousSourceObjects_preCached);
		this.cachedObjects.Add("/OurPersonRig/EnvironmentParticleSystems", this.environmentParticleSystems_preCached);
		this.cachedObjects.Add("/Universe/ThrownOrEmittedThings", this.thrownOrEmittedThings_preCached);
		this.cachedObjects.Add("/OurPersonRig", this.ourPersonRig_preCached);
		this.cachedObjects.Add("/OurPersonRig/HeadCore", this.headCore_preCached);
		this.cachedObjects.Add("/OurPersonRig/HandCoreLeft", this.handCoreLeft_preCached);
		this.cachedObjects.Add("/OurPersonRig/HandCoreRight", this.handCoreRight_preCached);
		this.cachedObjects.Add("/OurPersonRig/HandCoreLeft/HandDot", this.handDotLeft_preCached);
		this.cachedObjects.Add("/OurPersonRig/HandCoreRight/HandDot", this.handDotRight_preCached);
		this.cachedObjects.Add("/Universe/FollowerCamera", this.followerCamera_preCached);
		this.cachedObjects.Add("/OurPersonRig/FollowerCameraTarget", this.followerCameraTarget_preCached);
		this.status = ManagerStatus.Started;
	}

	// Token: 0x060013D6 RID: 5078 RVA: 0x000B3F98 File Offset: 0x000B2398
	public GameObject GetObject(string absoluteTreePath)
	{
		GameObject gameObject = null;
		this.cachedObjects.TryGetValue(absoluteTreePath, out gameObject);
		if (gameObject == null)
		{
			gameObject = GameObject.Find(absoluteTreePath);
			if (gameObject != null)
			{
				this.cachedObjects.Add(absoluteTreePath, gameObject);
			}
		}
		return gameObject;
	}

	// Token: 0x060013D7 RID: 5079 RVA: 0x000B3FE4 File Offset: 0x000B23E4
	public Transform GetTransform(string absoluteTreePath)
	{
		Transform transform = null;
		GameObject @object = this.GetObject(absoluteTreePath);
		if (@object != null)
		{
			transform = @object.transform;
		}
		return transform;
	}

	// Token: 0x040011F3 RID: 4595
	public GameObject dialogParts_preCached;

	// Token: 0x040011F4 RID: 4596
	public GameObject environmentManager_preCached;

	// Token: 0x040011F5 RID: 4597
	public GameObject environmentChangers_preCached;

	// Token: 0x040011F6 RID: 4598
	public GameObject sunEnvironmentChanger_preCached;

	// Token: 0x040011F7 RID: 4599
	public GameObject nightEnvironmentChanger_preCached;

	// Token: 0x040011F8 RID: 4600
	public GameObject ambientLightEnvironmentChanger_preCached;

	// Token: 0x040011F9 RID: 4601
	public GameObject fogEnvironmentChanger_preCached;

	// Token: 0x040011FA RID: 4602
	public GameObject cloudsEnvironmentChanger_preCached;

	// Token: 0x040011FB RID: 4603
	public GameObject miscellaneousSourceObjects_preCached;

	// Token: 0x040011FC RID: 4604
	public GameObject universe_preCached;

	// Token: 0x040011FD RID: 4605
	public GameObject headCore_preCached;

	// Token: 0x040011FE RID: 4606
	public GameObject handCoreLeft_preCached;

	// Token: 0x040011FF RID: 4607
	public GameObject handCoreRight_preCached;

	// Token: 0x04001200 RID: 4608
	public GameObject handDotLeft_preCached;

	// Token: 0x04001201 RID: 4609
	public GameObject handDotRight_preCached;

	// Token: 0x04001202 RID: 4610
	public GameObject ourPersonRig_preCached;

	// Token: 0x04001203 RID: 4611
	public GameObject followerCamera_preCached;

	// Token: 0x04001204 RID: 4612
	public GameObject followerCameraTarget_preCached;

	// Token: 0x04001205 RID: 4613
	public GameObject environmentParticleSystems_preCached;

	// Token: 0x04001206 RID: 4614
	public GameObject thrownOrEmittedThings_preCached;

	// Token: 0x04001207 RID: 4615
	private Dictionary<string, GameObject> cachedObjects = new Dictionary<string, GameObject>();
}
