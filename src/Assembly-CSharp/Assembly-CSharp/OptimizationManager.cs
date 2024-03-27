using System;
using System.Linq;
using UnityEngine;

// Token: 0x020001F5 RID: 501
public class OptimizationManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000214 RID: 532
	// (get) Token: 0x06001195 RID: 4501 RVA: 0x0009565F File Offset: 0x00093A5F
	// (set) Token: 0x06001196 RID: 4502 RVA: 0x00095667 File Offset: 0x00093A67
	public ManagerStatus status { get; private set; }

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x06001197 RID: 4503 RVA: 0x00095670 File Offset: 0x00093A70
	// (set) Token: 0x06001198 RID: 4504 RVA: 0x00095678 File Offset: 0x00093A78
	public string failMessage { get; private set; }

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x06001199 RID: 4505 RVA: 0x00095681 File Offset: 0x00093A81
	// (set) Token: 0x0600119A RID: 4506 RVA: 0x00095689 File Offset: 0x00093A89
	public bool doOptimizeSpeed { get; private set; }

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x0600119B RID: 4507 RVA: 0x00095692 File Offset: 0x00093A92
	// (set) Token: 0x0600119C RID: 4508 RVA: 0x0009569A File Offset: 0x00093A9A
	public bool extraEffectsEvenInVR { get; private set; }

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x0600119D RID: 4509 RVA: 0x000956A3 File Offset: 0x00093AA3
	// (set) Token: 0x0600119E RID: 4510 RVA: 0x000956AB File Offset: 0x00093AAB
	public int maxThingsAround { get; private set; }

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x0600119F RID: 4511 RVA: 0x000956B4 File Offset: 0x00093AB4
	// (set) Token: 0x060011A0 RID: 4512 RVA: 0x000956BC File Offset: 0x00093ABC
	public int maxLightsAround { get; private set; }

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x060011A1 RID: 4513 RVA: 0x000956C5 File Offset: 0x00093AC5
	// (set) Token: 0x060011A2 RID: 4514 RVA: 0x000956CD File Offset: 0x00093ACD
	public int maxLightsToThrowShadows { get; private set; }

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x060011A3 RID: 4515 RVA: 0x000956D6 File Offset: 0x00093AD6
	// (set) Token: 0x060011A4 RID: 4516 RVA: 0x000956DE File Offset: 0x00093ADE
	public int maxBaseLayerParticleSystemsAround { get; private set; }

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x060011A5 RID: 4517 RVA: 0x000956E7 File Offset: 0x00093AE7
	// (set) Token: 0x060011A6 RID: 4518 RVA: 0x000956EF File Offset: 0x00093AEF
	public int maxParticleSystemsAround { get; private set; }

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x060011A7 RID: 4519 RVA: 0x000956F8 File Offset: 0x00093AF8
	// (set) Token: 0x060011A8 RID: 4520 RVA: 0x00095700 File Offset: 0x00093B00
	public int maxThrownOrEmittedThingsForEmitting { get; private set; }

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x060011A9 RID: 4521 RVA: 0x00095709 File Offset: 0x00093B09
	// (set) Token: 0x060011AA RID: 4522 RVA: 0x00095711 File Offset: 0x00093B11
	public int maxImagePartsAroundCount { get; private set; }

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x060011AB RID: 4523 RVA: 0x0009571A File Offset: 0x00093B1A
	// (set) Token: 0x060011AC RID: 4524 RVA: 0x00095722 File Offset: 0x00093B22
	public bool hideTextures { get; private set; }

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x060011AD RID: 4525 RVA: 0x0009572B File Offset: 0x00093B2B
	// (set) Token: 0x060011AE RID: 4526 RVA: 0x00095733 File Offset: 0x00093B33
	public bool findOptimizations { get; private set; }

	// Token: 0x060011AF RID: 4527 RVA: 0x0009573C File Offset: 0x00093B3C
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.doOptimizeSpeed = PlayerPrefs.GetInt("doOptimizeSpeed", 0) == 1;
		this.extraEffectsEvenInVR = PlayerPrefs.GetInt("extraEffectsEvenInVR", 0) == 1;
		this.hideTextures = false;
		this.UpdateSettings();
		this.status = ManagerStatus.Started;
	}

	// Token: 0x060011B0 RID: 4528 RVA: 0x0009578C File Offset: 0x00093B8C
	private void Update()
	{
		if (this.findOptimizations && Misc.Chance(1f))
		{
			this.lagIndicator.transform.position = this.farAway;
		}
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x000957C0 File Offset: 0x00093BC0
	public void SetDoOptimizeSpeed(bool state, bool doRefreshScene = false, bool ignoreUpdateSettings = false)
	{
		if (state && this.extraEffectsEvenInVR)
		{
			this.SetExtraEffectsEvenInVR(false, true);
			Managers.filterManager.ApplySettings(false);
		}
		this.doOptimizeSpeed = state;
		PlayerPrefs.SetInt("doOptimizeSpeed", (!this.doOptimizeSpeed) ? 0 : 1);
		if (!ignoreUpdateSettings)
		{
			this.UpdateSettings();
			Managers.filterManager.ApplySettings(false);
		}
		if (doRefreshScene)
		{
			this.SetPlacementsActiveBasedOnDistance(string.Empty);
		}
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x0009583C File Offset: 0x00093C3C
	public void SetFindOptimizations(bool state)
	{
		this.findOptimizations = state;
		if (this.findOptimizations)
		{
			if (this.lagIndicator == null)
			{
				this.lagIndicator = global::UnityEngine.Object.Instantiate<GameObject>(this.lagIndicatorPrefab);
				Misc.RemoveCloneFromName(this.lagIndicator);
				this.lagIndicator.transform.position = this.farAway;
			}
		}
		else
		{
			global::UnityEngine.Object.Destroy(this.lagIndicator);
			this.lagIndicator = null;
		}
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x000958B6 File Offset: 0x00093CB6
	public void IndicateScriptActivityHere(Vector3 position)
	{
		if (this.findOptimizations && this.lagIndicator != null && Misc.Chance(5f))
		{
			this.lagIndicator.transform.position = position;
		}
	}

	// Token: 0x060011B4 RID: 4532 RVA: 0x000958F4 File Offset: 0x00093CF4
	public void ResetIndicateScriptActivity()
	{
		if (this.findOptimizations && this.lagIndicator != null)
		{
			ParticleSystem component = this.lagIndicator.GetComponent<ParticleSystem>();
			component.Clear();
			this.lagIndicator.transform.position = this.farAway;
		}
	}

	// Token: 0x060011B5 RID: 4533 RVA: 0x00095948 File Offset: 0x00093D48
	public void SetExtraEffectsEvenInVR(bool state, bool ignoreUpdateSettings = false)
	{
		if (state && this.doOptimizeSpeed)
		{
			this.SetDoOptimizeSpeed(false, false, true);
		}
		this.extraEffectsEvenInVR = state;
		PlayerPrefs.SetInt("extraEffectsEvenInVR", (!this.extraEffectsEvenInVR) ? 0 : 1);
		if (!ignoreUpdateSettings)
		{
			this.UpdateSettings();
			Managers.filterManager.ApplySettings(false);
		}
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x000959AC File Offset: 0x00093DAC
	public void UpdateSettings()
	{
		int? num = this.maxThingsAroundOverride;
		int num3;
		if (num != null)
		{
			int? num2 = this.maxThingsAroundOverride;
			num3 = num2.Value;
		}
		else
		{
			num3 = 250;
		}
		this.maxThingsAround = num3;
		int? num4 = this.maxLightsAroundOverride;
		int num6;
		if (num4 != null)
		{
			int? num5 = this.maxLightsAroundOverride;
			num6 = num5.Value;
		}
		else
		{
			num6 = 3;
		}
		this.maxLightsAround = num6;
		int? num7 = this.maxLightsToThrowShadowsOverride;
		int num9;
		if (num7 != null)
		{
			int? num8 = this.maxLightsToThrowShadowsOverride;
			num9 = num8.Value;
		}
		else
		{
			num9 = 1;
		}
		this.maxLightsToThrowShadows = num9;
		this.maxParticleSystemsAround = 15;
		this.maxBaseLayerParticleSystemsAround = 4;
		this.maxThrownOrEmittedThingsForEmitting = 150;
		int num10 = 0;
		this.maxImagePartsAroundCount = 50;
		int num11 = 2;
		bool flag = false;
		if (CrossDevice.desktopMode)
		{
			this.maxLightsAround += 2;
			num11 = 3;
			this.maxLightsToThrowShadows++;
			flag = true;
		}
		this.cookieLight.SetActive(flag);
		if (this.doOptimizeSpeed)
		{
			this.maxThingsAround = 175;
			num11 = 1;
			this.maxLightsAround = 2;
			this.maxLightsToThrowShadows = 0;
			this.maxBaseLayerParticleSystemsAround = 2;
			this.maxParticleSystemsAround = 5;
			num10 = 1;
		}
		if (num10 != QualitySettings.GetQualityLevel())
		{
			QualitySettings.SetQualityLevel(num10, true);
		}
		QualitySettings.pixelLightCount = num11;
	}

	// Token: 0x060011B7 RID: 4535 RVA: 0x00095B00 File Offset: 0x00093F00
	public void SetPlacementsActiveBasedOnDistance(string placementIdToAlwaysShow = "")
	{
		if (Managers.personManager == null || Managers.personManager.ourPerson == null || Managers.personManager.ourPerson.Head == null || Managers.areaManager == null)
		{
			return;
		}
		Transform ourTransform = Managers.personManager.ourPerson.Head.transform;
		GameObject[] array = Misc.GetChildrenAsArray(Managers.thingManager.placements.transform);
		array = array.OrderBy((GameObject x) => Vector3.Distance(ourTransform.position, x.transform.position)).ToArray<GameObject>();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		foreach (GameObject gameObject in array)
		{
			if (gameObject != CreationHelper.thingThatWasClonedFrom && gameObject != CreationHelper.thingBeingEdited && gameObject.name != Universe.objectNameIfAlreadyDestroyed)
			{
				Thing component = gameObject.GetComponent<Thing>();
				if ((string.IsNullOrEmpty(placementIdToAlwaysShow) || !(component.placementId == placementIdToAlwaysShow)) && !component.isHighlighted)
				{
					bool flag = false;
					bool flag2 = false;
					float num7 = Vector3.Distance(ourTransform.position, gameObject.transform.position);
					float? distanceToShow = component.distanceToShow;
					if (distanceToShow != null && !component.isHighlighted)
					{
						float num8 = num7;
						float? distanceToShow2 = component.distanceToShow;
						if (num8 <= distanceToShow2.Value)
						{
							flag = true;
						}
						else
						{
							flag2 = true;
						}
					}
					bool flag3 = false;
					bool flag4 = false;
					if (!component.suppressShowAtDistance)
					{
						flag4 = component.thingPartCount <= 15 && (component.isVeryBig || component.requiresWiderReach || component.hasSurroundSound);
						if (component.benefitsFromShowingAtDistance || component.temporarilyBenefitsFromShowingAtDistance || num7 <= 2.5f)
						{
							flag4 = true;
						}
					}
					if ((num < this.maxThingsAround || flag4 || flag) && !flag2)
					{
						num++;
						float num9 = 0f;
						if (!component.containsBehaviorScript || num7 > 35f)
						{
							if (num7 >= 100f)
							{
								num9 = 15f;
							}
							else if (num7 >= 75f)
							{
								num9 = 6f;
							}
							else if (num7 >= 50f)
							{
								num9 = 2f;
							}
							else if (num7 >= 25f)
							{
								num9 = 1f;
							}
						}
						bool flag5 = num7 >= 50f && component.thingPartCount >= 25;
						if (flag4 || (component.biggestSize >= num9 && !flag5) || flag)
						{
							component.gameObject.SetActive(true);
							flag3 = true;
							if (component.containsBaseLayerParticleSystem)
							{
								num2++;
								bool flag6 = num2 > Managers.optimizationManager.maxBaseLayerParticleSystemsAround && !component.benefitsFromShowingAtDistance;
								this.AdjustThingParticleSystemsOptimization(component.gameObject, flag6);
							}
							if (component.containsParticleSystem)
							{
								num3++;
								bool flag7 = num3 > Managers.optimizationManager.maxParticleSystemsAround && !component.benefitsFromShowingAtDistance;
								component.SetParticleSystemsStopPlay(!flag7);
							}
							if (component.containsLight)
							{
								num4++;
								component.AdjustLightsOptimization(num4 > Managers.optimizationManager.maxLightsAround && !flag);
								if (num5 < Managers.optimizationManager.maxLightsToThrowShadows)
								{
									int? num10 = new int?(Managers.optimizationManager.maxLightsToThrowShadows - num5);
									int num11 = component.SetLightShadows(true, num10);
									num5 += num11;
								}
								else
								{
									component.SetLightShadows(false, null);
								}
							}
							if (component.allPartsImageCount > 0)
							{
								num6 += component.allPartsImageCount;
								bool flag8 = num6 <= Managers.optimizationManager.maxImagePartsAroundCount || component.IsPlacedSubThing() || component.movableByEveryone;
								component.SetImagePartsActive(flag8);
							}
						}
					}
					if (!flag3)
					{
						component.gameObject.SetActive(false);
					}
				}
			}
		}
		Managers.areaManager.LimitVisibilityIfNeeded();
	}

	// Token: 0x060011B8 RID: 4536 RVA: 0x00095F7C File Offset: 0x0009437C
	private void AdjustThingParticleSystemsOptimization(GameObject thing, bool doOptimize)
	{
		Component[] componentsInChildren = thing.gameObject.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (doOptimize)
			{
				if (thingPart.materialType == MaterialTypes.Particles || thingPart.materialType == MaterialTypes.ParticlesBig)
				{
					thingPart.materialTypeBeforeOptimization = thingPart.materialType;
					thingPart.materialType = MaterialTypes.None;
					thingPart.ResetStates();
				}
			}
			else if (thingPart.materialTypeBeforeOptimization == MaterialTypes.Particles || thingPart.materialTypeBeforeOptimization == MaterialTypes.ParticlesBig)
			{
				thingPart.materialType = thingPart.materialTypeBeforeOptimization;
				thingPart.materialTypeBeforeOptimization = MaterialTypes.None;
				thingPart.ResetStates();
			}
		}
	}

	// Token: 0x04001141 RID: 4417
	public GameObject cookieLight;

	// Token: 0x04001142 RID: 4418
	private const string optimizeSpeed_keyName = "doOptimizeSpeed";

	// Token: 0x04001143 RID: 4419
	private const string extraEffectsEvenInVR_keyName = "extraEffectsEvenInVR";

	// Token: 0x04001144 RID: 4420
	private const int defaultQualityIndex = 0;

	// Token: 0x04001145 RID: 4421
	private const int optimizedForSpeedQualityIndex = 1;

	// Token: 0x04001146 RID: 4422
	public const int maxThingsAroundDefault = 250;

	// Token: 0x04001147 RID: 4423
	public const int maxLightsAroundDefault = 3;

	// Token: 0x04001148 RID: 4424
	public const int maxLightsToThrowShadowsDefault = 1;

	// Token: 0x04001149 RID: 4425
	public int? maxThingsAroundOverride;

	// Token: 0x0400114A RID: 4426
	public int? maxLightsAroundOverride;

	// Token: 0x0400114B RID: 4427
	public int? maxLightsToThrowShadowsOverride;

	// Token: 0x0400114D RID: 4429
	[SerializeField]
	private GameObject lagIndicatorPrefab;

	// Token: 0x0400114E RID: 4430
	private GameObject lagIndicator;

	// Token: 0x0400114F RID: 4431
	private Vector3 farAway = new Vector3(0f, 10000f, 0f);
}
