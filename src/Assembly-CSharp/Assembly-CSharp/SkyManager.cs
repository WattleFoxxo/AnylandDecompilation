using System;
using UnityEngine;

// Token: 0x020001FB RID: 507
public class SkyManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000231 RID: 561
	// (get) Token: 0x0600131D RID: 4893 RVA: 0x000A6619 File Offset: 0x000A4A19
	// (set) Token: 0x0600131E RID: 4894 RVA: 0x000A6621 File Offset: 0x000A4A21
	public ManagerStatus status { get; private set; }

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x0600131F RID: 4895 RVA: 0x000A662A File Offset: 0x000A4A2A
	// (set) Token: 0x06001320 RID: 4896 RVA: 0x000A6632 File Offset: 0x000A4A32
	public string failMessage { get; private set; }

	// Token: 0x06001321 RID: 4897 RVA: 0x000A663B File Offset: 0x000A4A3B
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001322 RID: 4898 RVA: 0x000A664C File Offset: 0x000A4A4C
	private void Update()
	{
		if (this.status != ManagerStatus.Started)
		{
			return;
		}
		if (this.skydomeActive && (this.currentThingPart == null || !this.currentThingPart.useTextureAsSky))
		{
			this.ResetToDefault();
			this.AutoFindThingPartMatchIfNeeded();
		}
	}

	// Token: 0x06001323 RID: 4899 RVA: 0x000A66A0 File Offset: 0x000A4AA0
	public void SetThingPart(ThingPart thingPart)
	{
		if (thingPart == null)
		{
			return;
		}
		this.ResetToDefault();
		Renderer component = thingPart.GetComponent<Renderer>();
		if (component != null && component.materials != null)
		{
			this.currentThingPart = thingPart;
			RenderSettings.skybox = null;
			this.camera.backgroundColor = Color.black;
			if (thingPart.stretchSkydomeSeam)
			{
				this.skydomeSeamStretch.SetActive(true);
				this.skydomeDefault.SetActive(false);
				this.skydomeSeamStretch.GetComponent<Renderer>().sharedMaterials = component.sharedMaterials;
			}
			else
			{
				this.skydomeDefault.SetActive(true);
				this.skydomeSeamStretch.SetActive(false);
				this.skydomeDefault.GetComponent<Renderer>().sharedMaterials = component.sharedMaterials;
			}
			this.skydomeActive = true;
		}
	}

	// Token: 0x06001324 RID: 4900 RVA: 0x000A676E File Offset: 0x000A4B6E
	public void ResetToDefault()
	{
		this.skydomeActive = false;
		this.currentThingPart = null;
		this.skydomeDefault.SetActive(false);
		this.skydomeSeamStretch.SetActive(false);
		RenderSettings.skybox = this.skyboxMaterial;
	}

	// Token: 0x06001325 RID: 4901 RVA: 0x000A67A4 File Offset: 0x000A4BA4
	public void AutoFindThingPartMatchIfNeeded()
	{
		if (!this.skydomeActive)
		{
			ThingPart otherThingPartMatchInPlacements = this.GetOtherThingPartMatchInPlacements(null);
			if (otherThingPartMatchInPlacements != null)
			{
				this.SetThingPart(otherThingPartMatchInPlacements);
			}
		}
	}

	// Token: 0x06001326 RID: 4902 RVA: 0x000A67D8 File Offset: 0x000A4BD8
	private ThingPart GetOtherThingPartMatchInPlacements(ThingPart thingPartToExclude = null)
	{
		ThingPart thingPart = null;
		if (!Managers.areaManager.isTransportInProgress)
		{
			Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(ThingPart), true);
			for (int i = componentsInChildren.Length - 1; i >= 0; i--)
			{
				ThingPart thingPart2 = (ThingPart)componentsInChildren[i];
				if (thingPart2.transform.parent != null && thingPart2.transform.parent.name != Universe.objectNameIfAlreadyDestroyed && thingPart2.transform.parent.gameObject.activeSelf && thingPart2.useTextureAsSky && thingPart2 != thingPartToExclude)
				{
					thingPart = thingPart2;
					break;
				}
			}
		}
		return thingPart;
	}

	// Token: 0x06001327 RID: 4903 RVA: 0x000A68A0 File Offset: 0x000A4CA0
	public void RemoveThingPartIfCurrent(ThingPart thingPart)
	{
		if (this.currentThingPart == thingPart)
		{
			ThingPart otherThingPartMatchInPlacements = this.GetOtherThingPartMatchInPlacements(thingPart);
			this.currentThingPart = null;
			if (otherThingPartMatchInPlacements != null)
			{
				this.SetThingPart(otherThingPartMatchInPlacements);
			}
		}
	}

	// Token: 0x04001189 RID: 4489
	public Material skyboxMaterial;

	// Token: 0x0400118A RID: 4490
	public GameObject skydomeDefault;

	// Token: 0x0400118B RID: 4491
	public GameObject skydomeSeamStretch;

	// Token: 0x0400118C RID: 4492
	public Camera camera;

	// Token: 0x0400118D RID: 4493
	private bool skydomeActive;

	// Token: 0x0400118E RID: 4494
	private ThingPart currentThingPart;
}
