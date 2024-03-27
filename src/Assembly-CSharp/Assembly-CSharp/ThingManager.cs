using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000200 RID: 512
public class ThingManager : MonoBehaviour, IGameManager
{
	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06001369 RID: 4969 RVA: 0x000ADE54 File Offset: 0x000AC254
	// (set) Token: 0x0600136A RID: 4970 RVA: 0x000ADE5C File Offset: 0x000AC25C
	public ManagerStatus status { get; private set; }

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x0600136B RID: 4971 RVA: 0x000ADE65 File Offset: 0x000AC265
	// (set) Token: 0x0600136C RID: 4972 RVA: 0x000ADE6D File Offset: 0x000AC26D
	public string failMessage { get; private set; }

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x0600136D RID: 4973 RVA: 0x000ADE76 File Offset: 0x000AC276
	// (set) Token: 0x0600136E RID: 4974 RVA: 0x000ADE7E File Offset: 0x000AC27E
	public GameObject placements { get; private set; }

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x0600136F RID: 4975 RVA: 0x000ADE87 File Offset: 0x000AC287
	// (set) Token: 0x06001370 RID: 4976 RVA: 0x000ADE8F File Offset: 0x000AC28F
	public Transform controllables { get; private set; }

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06001371 RID: 4977 RVA: 0x000ADE98 File Offset: 0x000AC298
	// (set) Token: 0x06001372 RID: 4978 RVA: 0x000ADEA0 File Offset: 0x000AC2A0
	public Transform thrownOrEmittedThingsParent { get; private set; }

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06001373 RID: 4979 RVA: 0x000ADEA9 File Offset: 0x000AC2A9
	// (set) Token: 0x06001374 RID: 4980 RVA: 0x000ADEB1 File Offset: 0x000AC2B1
	public Dictionary<ThingPartBase, int> smoothingAngles { get; private set; }

	// Token: 0x06001375 RID: 4981 RVA: 0x000ADEBC File Offset: 0x000AC2BC
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.CacheShaders();
		this.InitSettings();
		this.thingDefinitionCache = base.gameObject.AddComponent<ThingDefinitionCache>();
		this.heldThingsRegistrar = base.gameObject.AddComponent<HeldThingsRegistrar>();
		this.bouncyMaterial = (PhysicMaterial)Resources.Load("Materials/BouncyMaterial");
		this.slidyMaterial = (PhysicMaterial)Resources.Load("Materials/SlidyMaterial");
		this.bouncySlidyMaterial = (PhysicMaterial)Resources.Load("Materials/BouncySlidyMaterial");
		this.controllableMaterial = (PhysicMaterial)Resources.Load("Materials/ControllableMaterial");
		this.placements = new GameObject("Placements");
		this.placements.tag = "Placements";
		this.controllables = new GameObject("Controllables").transform;
		this.thrownOrEmittedThingsParent = Managers.treeManager.GetTransform("/Universe/ThrownOrEmittedThings");
		this.thingGameObject = (GameObject)Resources.Load("Prefabs/Thing", typeof(GameObject));
		Thing component = this.thingGameObject.GetComponent<Thing>();
		component.version = 9;
		this.LoadThingPartBases();
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PassableObjects"), LayerMask.NameToLayer("IgnorePassableObjects"));
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PassThroughEachOther"), LayerMask.NameToLayer("PassThroughEachOther"));
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x000AE00E File Offset: 0x000AC40E
	private void ShowThingPartBasesInfo()
	{
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x000AE010 File Offset: 0x000AC410
	private void CacheShaders()
	{
		this.shader_standard = Shader.Find("Standard");
		this.shader_customGlow = Shader.Find("Custom/Glow");
		this.shader_customUnshaded = Shader.Find("Custom/Unshaded");
		this.shader_customInversion = Shader.Find("Custom/Inversion");
		this.shader_customBrightness = Shader.Find("Custom/Brightness");
		this.shader_customTransparentGlow = Shader.Find("Custom/AlphaSelfIllum");
		this.shader_textLit = Shader.Find("GUI/3D Text Shader - Lit");
		this.shader_textEmissive = Shader.Find("GUI/3D Text Shader - Cull Back");
	}

	// Token: 0x06001378 RID: 4984 RVA: 0x000AE0A0 File Offset: 0x000AC4A0
	private void LoadThingPartBases()
	{
		this.thingPartBases = new GameObject[this.GetHighestThingPartBaseIndex() + 1];
		IEnumerator enumerator = Enum.GetValues(typeof(ThingPartBase)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				ThingPartBase thingPartBase = (ThingPartBase)obj;
				string text = "ThingPartBases/" + thingPartBase.ToString();
				this.thingPartBases[(int)thingPartBase] = (GameObject)Resources.Load(text, typeof(GameObject));
				ThingPart component = this.thingPartBases[(int)thingPartBase].GetComponent<ThingPart>();
				component.baseType = thingPartBase;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06001379 RID: 4985 RVA: 0x000AE168 File Offset: 0x000AC568
	private void ShowThingPartBasesVertexCount()
	{
		IEnumerator enumerator = Enum.GetValues(typeof(ThingPartBase)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				ThingPartBase thingPartBase = (ThingPartBase)obj;
				MeshFilter component = this.thingPartBases[(int)thingPartBase].GetComponent<MeshFilter>();
				if (component != null)
				{
					int vertexCount = component.sharedMesh.vertexCount;
					if (vertexCount >= 250)
					{
						Log.Debug(thingPartBase.ToString() + " meshFilter vertices: " + vertexCount);
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x0600137A RID: 4986 RVA: 0x000AE224 File Offset: 0x000AC624
	private void ShowThingPartBasesColliderVertexCount()
	{
		IEnumerator enumerator = Enum.GetValues(typeof(ThingPartBase)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				ThingPartBase thingPartBase = (ThingPartBase)obj;
				MeshCollider component = this.thingPartBases[(int)thingPartBase].GetComponent<MeshCollider>();
				if (component != null)
				{
					int vertexCount = component.sharedMesh.vertexCount;
					if (vertexCount >= 250)
					{
						Log.Debug(thingPartBase.ToString() + " vertices: " + vertexCount);
					}
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x0600137B RID: 4987 RVA: 0x000AE2E0 File Offset: 0x000AC6E0
	private void ShowThingPartBasesWithOptimizedColliders()
	{
		IEnumerator enumerator = Enum.GetValues(typeof(ThingPartBase)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				ThingPartBase thingPartBase = (ThingPartBase)obj;
				GameObject gameObject = this.thingPartBases[(int)thingPartBase];
				MeshFilter component = gameObject.GetComponent<MeshFilter>();
				MeshCollider component2 = gameObject.GetComponent<MeshCollider>();
				if (component != null && component2 != null && component.sharedMesh.vertexCount != component2.sharedMesh.vertexCount)
				{
					string text = string.Concat(new object[]
					{
						thingPartBase.ToString(),
						" has different collider count: ",
						component.sharedMesh.vertexCount,
						" -> ",
						component2.sharedMesh.vertexCount
					});
					if (component2.sharedMesh.vertexCount > component.sharedMesh.vertexCount)
					{
						text += "---> Collider has more vertices!";
					}
					Log.Debug(text);
				}
				else if (component != null && component2 == null)
				{
					string text2 = ">>> " + thingPartBase.ToString() + " has different collider type: ";
					BoxCollider component3 = gameObject.GetComponent<BoxCollider>();
					SphereCollider component4 = gameObject.GetComponent<SphereCollider>();
					if (component3 != null)
					{
						text2 += "Box";
					}
					else if (component4 != null)
					{
						text2 += "Sphere";
					}
					Log.Debug(text2);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x0600137C RID: 4988 RVA: 0x000AE4B8 File Offset: 0x000AC8B8
	private int GetHighestThingPartBaseIndex()
	{
		int num = 0;
		IEnumerator enumerator = Enum.GetValues(typeof(ThingPartBase)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				ThingPartBase thingPartBase = (ThingPartBase)obj;
				if (thingPartBase > (ThingPartBase)num)
				{
					num = (int)thingPartBase;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		return num;
	}

	// Token: 0x0600137D RID: 4989 RVA: 0x000AE52C File Offset: 0x000AC92C
	public Vector3 GetBaseShapeDropUpScale(ThingPartBase thingPartBase)
	{
		Vector3 vector = Vector3.zero;
		float? num = null;
		switch (thingPartBase)
		{
		case ThingPartBase.JitterCubeSoft:
		case ThingPartBase.JitterSphereSoft:
			break;
		case ThingPartBase.LowJitterCube:
		case ThingPartBase.LowJitterCubeSoft:
			num = new float?(0.04f);
			goto IL_3D3;
		case ThingPartBase.JitterCone:
		case ThingPartBase.JitterConeSoft:
			num = new float?(0.07f);
			goto IL_3D3;
		case ThingPartBase.JitterHalfCone:
		case ThingPartBase.JitterHalfConeSoft:
			goto IL_19A;
		case ThingPartBase.JitterChamferCylinder:
		case ThingPartBase.JitterChamferCylinderSoft:
			num = new float?(0.09f);
			goto IL_3D3;
		default:
			switch (thingPartBase)
			{
			case ThingPartBase.SharpBent:
			case ThingPartBase.Pipe:
			case ThingPartBase.Pipe2:
			case ThingPartBase.Pipe3:
			case ThingPartBase.ShrinkDisk:
			case ThingPartBase.ShrinkDisk2:
				break;
			case ThingPartBase.Tetrahedron:
				goto IL_19A;
			case ThingPartBase.DirectionIndicator:
				goto IL_1CA;
			default:
				switch (thingPartBase)
				{
				case ThingPartBase.Ramp:
				case ThingPartBase.HalfSphere:
					break;
				case ThingPartBase.JitterCube:
				case ThingPartBase.ChamferCube:
				case ThingPartBase.Spike:
				case ThingPartBase.JitterSphere:
					goto IL_16A;
				default:
					switch (thingPartBase)
					{
					case ThingPartBase.Icosahedron:
						num = new float?(0.23f);
						goto IL_3D3;
					case ThingPartBase.Cubeoctahedron:
						num = new float?(0.19f);
						goto IL_3D3;
					case ThingPartBase.Dodecahedron:
						num = new float?(0.22f);
						goto IL_3D3;
					case ThingPartBase.Icosidodecahedron:
						num = new float?(0.24f);
						goto IL_3D3;
					case ThingPartBase.Octahedron:
						num = new float?(0.24f);
						goto IL_3D3;
					default:
						switch (thingPartBase)
						{
						case ThingPartBase.QuarterBowlCube:
						case ThingPartBase.CubeHole:
						case ThingPartBase.QuarterBowlCubeSoft:
							break;
						default:
							switch (thingPartBase)
							{
							case ThingPartBase.RoundCube:
								num = new float?(0.1f);
								goto IL_3D3;
							case ThingPartBase.Capsule:
								break;
							default:
								if (thingPartBase == ThingPartBase.Cube)
								{
									num = new float?(0.175f);
									goto IL_3D3;
								}
								if (thingPartBase == ThingPartBase.Cylinder)
								{
									goto IL_1DA;
								}
								if (thingPartBase != ThingPartBase.BigDialog)
								{
									string text = thingPartBase.ToString();
									if (text.StartsWith("Text"))
									{
										num = new float?(0.003f);
									}
									else if (text.StartsWith("Gear"))
									{
										num = new float?(0.1f);
									}
									else if (text.StartsWith("Wheel"))
									{
										num = new float?(0.09f);
									}
									else if (text.StartsWith("Bowl"))
									{
										num = new float?(0.075f);
									}
									else if (text.StartsWith("Rocky"))
									{
										num = new float?(0.045f);
									}
									else if (text.StartsWith("Spikes"))
									{
										num = new float?(0.045f);
									}
									else if (text.StartsWith("WavyWall"))
									{
										num = new float?(0.045f);
									}
									else if (text.StartsWith("Drop"))
									{
										num = new float?(0.06f);
									}
									else
									{
										num = new float?(0.125f);
									}
									goto IL_3D3;
								}
								vector = new Vector3(2f, 1f, 2f);
								goto IL_3D3;
							case ThingPartBase.HighPolySphere:
								num = new float?(0.225f);
								goto IL_3D3;
							}
							break;
						case ThingPartBase.HalfBowlSoft:
							goto IL_19A;
						}
						num = new float?(0.075f);
						goto IL_3D3;
					}
					break;
				}
				IL_1DA:
				num = new float?(0.1f);
				goto IL_3D3;
			}
			break;
		case ThingPartBase.Branch:
			num = new float?(0.03f);
			goto IL_3D3;
		case ThingPartBase.Bubbles:
			num = new float?(0.045f);
			goto IL_3D3;
		case ThingPartBase.HoleWall:
		case ThingPartBase.JaggyWall:
			goto IL_1CA;
		case ThingPartBase.Quad:
			num = new float?(0.215f);
			goto IL_3D3;
		}
		IL_16A:
		num = new float?(0.06f);
		goto IL_3D3;
		IL_19A:
		num = new float?(0.08f);
		goto IL_3D3;
		IL_1CA:
		num = new float?(0.045f);
		IL_3D3:
		if (num != null)
		{
			vector = Misc.GetUniformVector3(num.Value);
		}
		return vector;
	}

	// Token: 0x0600137E RID: 4990 RVA: 0x000AE928 File Offset: 0x000ACD28
	public IEnumerator PrimeCacheWithThingDefinitionBundleIfNeeded(List<PlacementData> placements, string areaId, string key, Action<bool> callback)
	{
		yield return this.thingDefinitionCache.PrimeCacheWithThingDefinitionBundleIfNeeded(placements, areaId, key, callback);
		yield break;
	}

	// Token: 0x0600137F RID: 4991 RVA: 0x000AE960 File Offset: 0x000ACD60
	public void InstantiatePlacedThingViaCache(ThingRequestContext thingRequestContext, PlacementData placement)
	{
		if (string.IsNullOrEmpty(placement.Id))
		{
			throw new Exception("InstantiatePlacedThingViaCache called with placement missing placementId");
		}
		string areaIdAtStart = Managers.areaManager.currentAreaId;
		base.StartCoroutine(this.thingDefinitionCache.GetThingDefinition(thingRequestContext, placement.Tid, delegate(string errorString, string thingDefinitionJSON)
		{
			bool flag = areaIdAtStart != Managers.areaManager.currentAreaId;
			if (flag)
			{
				return;
			}
			if (errorString != null)
			{
				Log.Error(errorString);
			}
			else
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.thingGameObject);
				gameObject.SetActive(false);
				Thing component = gameObject.GetComponent<Thing>();
				component.thingId = placement.thingId;
				component.placementId = placement.Id;
				component.isLocked = this.GetPlacementAttributeValue(placement, PlacementAttribute.Locked);
				component.isInvisibleToEditors = this.GetPlacementAttributeValue(placement, PlacementAttribute.InvisibleToEditors);
				component.suppressScriptsAndStates = this.GetPlacementAttributeValue(placement, PlacementAttribute.SuppressScriptsAndStates);
				component.suppressCollisions = this.GetPlacementAttributeValue(placement, PlacementAttribute.SuppressCollisions);
				component.suppressLights = this.GetPlacementAttributeValue(placement, PlacementAttribute.SuppressLights);
				component.suppressParticles = this.GetPlacementAttributeValue(placement, PlacementAttribute.SuppressParticles);
				component.suppressHoldable = this.GetPlacementAttributeValue(placement, PlacementAttribute.SuppressHoldable);
				component.suppressShowAtDistance = this.GetPlacementAttributeValue(placement, PlacementAttribute.SuppressShowAtDistance);
				if (placement.distanceToShow != 0f)
				{
					component.distanceToShow = new float?(placement.distanceToShow);
				}
				JsonToThingConverter.SetThing(gameObject, thingDefinitionJSON, false, true, new Vector3?(placement.position), new Vector3?(placement.rotation));
				component.MemorizeOriginalTransform(true);
				gameObject.transform.parent = this.placements.transform;
				gameObject.transform.localScale = ((placement.scale == 0f) ? Vector3.one : Misc.GetUniformVector3(placement.scale));
				if (component.isInvisibleToEditors && Managers.areaManager.weAreEditorOfCurrentArea && !Our.seeInvisibleAsEditor)
				{
					Misc.SetAllObjectLayers(component.gameObject, "InvisibleToOurPerson");
				}
			}
			Managers.areaManager.DoneLoadingThisPlacement();
		}));
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x000AE9DB File Offset: 0x000ACDDB
	private bool GetPlacementAttributeValue(PlacementData placement, PlacementAttribute thisAttribute)
	{
		return placement.A != null && placement.A.Contains((int)thisAttribute);
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x000AE9F8 File Offset: 0x000ACDF8
	public IEnumerator InstantiateThingViaCache(ThingRequestContext thingRequestContext, string thingId, Action<GameObject> callback, bool alwaysKeepThingPartsSeparate = false, bool isForPlacement = false, int layer = -1, Thing inheritSuppressAttributesThing = null)
	{
		string thingDefJSON = null;
		string getThingDefErrorString = null;
		GameObject thingObject = null;
		yield return base.StartCoroutine(this.thingDefinitionCache.GetThingDefinition(thingRequestContext, thingId, delegate(string errorString, string definitionJSON)
		{
			thingDefJSON = definitionJSON;
			getThingDefErrorString = errorString;
		}));
		if (!string.IsNullOrEmpty(getThingDefErrorString))
		{
			Debug.LogError(getThingDefErrorString);
		}
		else
		{
			thingObject = global::UnityEngine.Object.Instantiate<GameObject>(this.thingGameObject);
			Thing component = thingObject.GetComponent<Thing>();
			component.thingId = thingId;
			if (inheritSuppressAttributesThing != null)
			{
				component.suppressScriptsAndStates = inheritSuppressAttributesThing.suppressScriptsAndStates;
				component.suppressCollisions = inheritSuppressAttributesThing.suppressCollisions;
				component.suppressLights = inheritSuppressAttributesThing.suppressLights;
				component.suppressParticles = inheritSuppressAttributesThing.suppressParticles;
				component.suppressHoldable = inheritSuppressAttributesThing.suppressHoldable;
				component.suppressShowAtDistance = inheritSuppressAttributesThing.suppressShowAtDistance;
			}
			JsonToThingConverter.SetThing(thingObject, thingDefJSON, alwaysKeepThingPartsSeparate, isForPlacement, null, null);
			if (layer > 0)
			{
				ThingManager.SetLayerForThingAndParts(component, layer);
			}
		}
		if (thingObject == null)
		{
			Log.Warning("InstantiateThingViaCache returning null thing");
		}
		callback(thingObject);
		yield break;
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x000AEA48 File Offset: 0x000ACE48
	public void InstantiateInventoryItemViaCache(ThingRequestContext thingRequestContext, InventoryItemData inventoryItem, Transform inventoryBoxTransform, bool isSearchResult = false, bool useFallingEffect = false)
	{
		base.StartCoroutine(this.thingDefinitionCache.GetThingDefinition(thingRequestContext, inventoryItem.thingId, delegate(string errorString, string thingDefinitionJSON)
		{
			if (errorString != null)
			{
				Debug.LogError(errorString);
			}
			else
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.thingGameObject);
				Thing component = gameObject.GetComponent<Thing>();
				component.thingId = inventoryItem.thingId;
				component.isInInventoryOrDialog = true;
				component.isInInventory = true;
				JsonToThingConverter.SetThing(gameObject, thingDefinitionJSON, false, false, null, null);
				float num = ((!isSearchResult) ? 0.1f : 0.075f);
				gameObject.transform.localScale = ((!component.keepSizeInInventory || isSearchResult) ? this.GetAppropriateDownScaleForThing(gameObject, num, false) : Vector3.one);
				gameObject.transform.parent = inventoryBoxTransform;
				gameObject.transform.localPosition = inventoryItem.position;
				gameObject.transform.localEulerAngles = inventoryItem.rotation;
				if (useFallingEffect)
				{
					component.localTarget = new Vector3?(inventoryItem.position);
					float num2 = global::UnityEngine.Random.Range(5f, 20f);
					gameObject.transform.localPosition = new Vector3(inventoryItem.position.x, inventoryItem.position.y + num2, inventoryItem.position.z + num2);
				}
				gameObject.GetComponent<Thing>().MemorizeOriginalTransform(false);
			}
		}));
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x000AEAAC File Offset: 0x000ACEAC
	public void InstantiateThingOnDialogViaCache(ThingRequestContext thingRequestContext, string thingId, Transform fundament, Vector3 position, float scale = 0.035f, bool allowGrabbing = false, bool useDefaultRotation = false, float rotationX = 0f, float rotationY = 0f, float rotationZ = 0f, bool isGift = false, bool isNewGift = false)
	{
		base.StartCoroutine(this.thingDefinitionCache.GetThingDefinition(thingRequestContext, thingId, delegate(string errorString, string thingDefinitionJSON)
		{
			if (errorString != null)
			{
				Debug.LogError(errorString);
			}
			else
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.thingGameObject);
				Thing component = gameObject.GetComponent<Thing>();
				component.thingId = thingId;
				component.isInInventoryOrDialog = true;
				component.isGiftInDialog = isGift;
				JsonToThingConverter.SetThing(gameObject, thingDefinitionJSON, false, false, null, null);
				gameObject.transform.parent = fundament;
				gameObject.transform.localPosition = position;
				if (scale == 1f)
				{
					gameObject.transform.localScale = Vector3.one;
				}
				else
				{
					gameObject.transform.localScale = this.GetAppropriateDownScaleForThing(gameObject, scale, isGift);
				}
				gameObject.transform.localRotation = Quaternion.identity;
				if (rotationX != 0f || rotationY != 0f || rotationZ != 0f)
				{
					gameObject.transform.localEulerAngles = new Vector3(rotationX, rotationY, rotationZ);
				}
				else if (!useDefaultRotation)
				{
					gameObject.transform.Rotate(new Vector3(90f, 0f, 0f));
					gameObject.transform.Rotate(new Vector3(0f, -45f, 0f));
				}
				gameObject.GetComponent<Thing>().MemorizeOriginalTransform(false);
				if (allowGrabbing)
				{
					component.tag = "GrabbableDialogThingThumb";
				}
				else
				{
					component.tag = "DialogThingThumb";
					component.isHoldable = false;
					component.remainsHeld = false;
					if (!isGift)
					{
						Component[] componentsInChildren = component.GetComponentsInChildren(typeof(Collider), true);
						foreach (Collider collider in componentsInChildren)
						{
							collider.enabled = false;
						}
					}
				}
				if (isNewGift)
				{
					Effects.SpawnNewCreationSparkles(gameObject);
					Managers.soundManager.Play("seeingNewGift", gameObject.transform, 0.4f, false, false);
				}
				else if (isGift)
				{
					Sound sound = new Sound();
					sound.name = "goblet ding sparkle";
					sound.volume = 0.03f;
					Managers.soundLibraryManager.Play(gameObject.transform.position, sound, false, false, false, -1f);
				}
			}
		}));
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x000AEB44 File Offset: 0x000ACF44
	public void GetThingInfo(string thingId, Action<ThingInfo> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetThingInfo(thingId, delegate(GetThingInfo_Response response)
		{
			if (response.error == null)
			{
				if (response.thingInfo != null && string.IsNullOrEmpty(response.thingInfo.name))
				{
					response.thingInfo.name = CreationHelper.thingDefaultName;
				}
				callback(response.thingInfo);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x000AEB7C File Offset: 0x000ACF7C
	public void GetPlacementInfo(string areaId, string placementId, Action<PlacementInfo> callback)
	{
		if (string.IsNullOrEmpty(placementId))
		{
			Managers.errorManager.BeepError();
			Log.Error("GetPlacementInfo called with null placementId");
			callback(new PlacementInfo());
			return;
		}
		base.StartCoroutine(Managers.serverManager.GetPlacementInfo(areaId, placementId, delegate(GetPlacementInfo_Response response)
		{
			if (response.error == null)
			{
				callback(response.placementInfo);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x000AEBE8 File Offset: 0x000ACFE8
	public void GetThingFlagStatus(string thingId, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetThingFlag(thingId, delegate(FlagStatus_Response response)
		{
			if (response.error == null)
			{
				callback(response.isFlagged);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x000AEC20 File Offset: 0x000AD020
	public void ToggleThingFlag(string thingId, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.ToggleThingFlag(thingId, delegate(FlagStatus_Response response)
		{
			if (response.error == null)
			{
				callback(response.isFlagged);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x000AEC58 File Offset: 0x000AD058
	public void SetThingUnlisted(string thingId, bool isUnlisted, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetThingUnlisted(thingId, isUnlisted, delegate(ResponseBase response)
		{
			if (response.error != null)
			{
				Log.Error("response.error");
			}
			callback(response.error == null);
		}));
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x000AEC94 File Offset: 0x000AD094
	public void SetThingTags(string thingId, List<string> tagsToAdd, List<string> tagsToRemove, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetThingTags(thingId, tagsToAdd, tagsToRemove, delegate(ResponseBase response)
		{
			if (response.error == null)
			{
				if (Managers.personManager.ourPerson != null)
				{
					Managers.personManager.ourPerson.thingTagCount += tagsToAdd.Count;
					Managers.personManager.ourPerson.thingTagCount -= tagsToRemove.Count;
				}
			}
			else
			{
				Log.Error("response.error");
			}
			callback(response.error == null);
		}));
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x000AECE8 File Offset: 0x000AD0E8
	public void GetThingTags(string thingId, Action<List<ThingTagInfo>> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetThingTags(thingId, delegate(GetThingTags_Response response)
		{
			if (response.error != null)
			{
				Log.Error("response.error");
			}
			callback(response.tags);
		}));
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x000AED20 File Offset: 0x000AD120
	public void UnloadPlacements()
	{
		IEnumerator enumerator = this.placements.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				Misc.Destroy(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		this.sharedMaterials = new Dictionary<string, Material>();
		this.sharedTextureMaterials = new Dictionary<string, Material>();
		this.sharedMeshes = new Dictionary<string, Mesh>();
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x000AEDB0 File Offset: 0x000AD1B0
	public float GetShapeToReferenceShapeScaleFactor(ThingPartBase baseType)
	{
		float num = 1f;
		if (baseType != ThingPartBase.LowPolySphere)
		{
			Transform transform = this.thingPartBases[10].transform;
			Transform transform2 = this.thingPartBases[(int)baseType].transform;
			num = transform.localScale.x / transform2.localScale.x;
		}
		return num;
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x000AEE08 File Offset: 0x000AD208
	private IEnumerator AttachHead(string thingId)
	{
		GameObject thingObject = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.ApproveBodyDialogHead, thingId, delegate(GameObject returnThingObject)
		{
			thingObject = returnThingObject;
		}, false, false, -1, null));
		if (thingObject != null)
		{
			Thing component = thingObject.GetComponent<Thing>();
			GameObject attachmentPointHead = Managers.personManager.ourPerson.AttachmentPointHead;
			component.transform.position = attachmentPointHead.transform.position;
			component.transform.rotation = attachmentPointHead.transform.rotation;
			Managers.personManager.DoAttachThing(attachmentPointHead, component.gameObject, false);
			base.StartCoroutine(Managers.thingManager.SetOurCurrentBodyAttachmentsByThing(ThingRequestContext.LocalTest, component.thingId, false, null));
		}
		yield break;
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x000AEE2C File Offset: 0x000AD22C
	public IEnumerator SetOurCurrentBodyAttachmentsByThing(ThingRequestContext thingRequestContext, string thingId, bool clearSpotsLeftEmpty, Thing headToAttach = null)
	{
		string thingDefJSON = null;
		string getThingDefErrorString = null;
		yield return base.StartCoroutine(this.thingDefinitionCache.GetThingDefinition(thingRequestContext, thingId, delegate(string errorString, string definitionJSON)
		{
			thingDefJSON = definitionJSON;
			getThingDefErrorString = errorString;
		}));
		if (!string.IsNullOrEmpty(getThingDefErrorString))
		{
			Debug.LogError(getThingDefErrorString);
		}
		else
		{
			JSONNode jsonnode = JSON.Parse(thingDefJSON);
			JSONNode jsonnode2 = jsonnode["bod"];
			if (jsonnode2 != null && (jsonnode2["h"] != null || headToAttach == null))
			{
				Debug.Log("Body data found");
				if (jsonnode2["h"] != null)
				{
					if (headToAttach != null)
					{
						GameObject attachmentPointHead = Managers.personManager.ourPerson.AttachmentPointHead;
						GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(headToAttach.gameObject, headToAttach.transform.position, headToAttach.transform.rotation);
						this.MakeDeepThingClone(headToAttach.gameObject, gameObject, true, false, false);
						Managers.personManager.DoAttachThing(attachmentPointHead, gameObject, false);
						Managers.soundManager.Play("putDown", attachmentPointHead.transform, 1f, false, false);
					}
					base.StartCoroutine(this.SetOurCurrentBodyAttachment(thingRequestContext, jsonnode2, "HeadCore/HeadAttachmentPoint", "h", true, false));
				}
				JSONNode jsonnode3 = jsonnode2;
				string text = "HeadCore/HeadTopAttachmentPoint";
				string text2 = "ht";
				base.StartCoroutine(this.SetOurCurrentBodyAttachment(thingRequestContext, jsonnode3, text, text2, false, clearSpotsLeftEmpty));
				jsonnode3 = jsonnode2;
				text2 = "HandCoreLeft/ArmLeftAttachmentPoint";
				text = "al";
				base.StartCoroutine(this.SetOurCurrentBodyAttachment(thingRequestContext, jsonnode3, text2, text, false, clearSpotsLeftEmpty));
				jsonnode3 = jsonnode2;
				text = "HandCoreRight/ArmRightAttachmentPoint";
				text2 = "ar";
				base.StartCoroutine(this.SetOurCurrentBodyAttachment(thingRequestContext, jsonnode3, text, text2, false, clearSpotsLeftEmpty));
				jsonnode3 = jsonnode2;
				text2 = "Torso/UpperTorsoAttachmentPoint";
				text = "ut";
				base.StartCoroutine(this.SetOurCurrentBodyAttachment(thingRequestContext, jsonnode3, text2, text, false, clearSpotsLeftEmpty));
				jsonnode3 = jsonnode2;
				text = "Torso/LowerTorsoAttachmentPoint";
				text2 = "lt";
				base.StartCoroutine(this.SetOurCurrentBodyAttachment(thingRequestContext, jsonnode3, text, text2, false, clearSpotsLeftEmpty));
				jsonnode3 = jsonnode2;
				text2 = "Torso/LegLeftAttachmentPoint";
				text = "ll";
				base.StartCoroutine(this.SetOurCurrentBodyAttachment(thingRequestContext, jsonnode3, text2, text, false, clearSpotsLeftEmpty));
				jsonnode3 = jsonnode2;
				text = "Torso/LegRightAttachmentPoint";
				text2 = "lr";
				base.StartCoroutine(this.SetOurCurrentBodyAttachment(thingRequestContext, jsonnode3, text, text2, false, clearSpotsLeftEmpty));
			}
		}
		yield break;
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x000AEE64 File Offset: 0x000AD264
	private IEnumerator SetOurCurrentBodyAttachment(ThingRequestContext thingRequestContext, JSONNode body, string treePath, string shortName, bool merelyAdjustCurrent = false, bool clearSpotsLeftEmpty = false)
	{
		treePath = "/OurPersonRig/" + treePath;
		GameObject attachmentPoint = Managers.treeManager.GetObject(treePath);
		if (attachmentPoint != null)
		{
			JSONNode data = body[shortName];
			string oldId = string.Empty;
			if (merelyAdjustCurrent)
			{
				GameObject childWithTag = Misc.GetChildWithTag(attachmentPoint.transform, "Attachment");
				if (childWithTag != null)
				{
					oldId = childWithTag.GetComponent<Thing>().thingId;
				}
			}
			if (data != null || clearSpotsLeftEmpty)
			{
				Managers.personManager.DoRemoveAttachedThing(attachmentPoint);
			}
			if (data != null)
			{
				yield return new WaitForSeconds(0.35f);
				GameObject attachmentThing = null;
				string thingId = ((!merelyAdjustCurrent) ? data["i"] : oldId);
				if (!string.IsNullOrEmpty(thingId))
				{
					yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(thingRequestContext, thingId, delegate(GameObject returnThing)
					{
						attachmentThing = returnThing;
					}, false, false, -1, null));
					if (attachmentThing != null)
					{
						attachmentThing.transform.parent = attachmentPoint.transform;
						attachmentThing.transform.localPosition = JsonHelper.GetVector3(data["p"]);
						attachmentThing.transform.localEulerAngles = JsonHelper.GetVector3(data["r"]);
						bool flag = data["ap"] != null && data["ar"] != null;
						if (flag)
						{
							attachmentPoint.transform.localPosition = JsonHelper.GetVector3(data["ap"]);
							attachmentPoint.transform.localEulerAngles = JsonHelper.GetVector3(data["ar"]);
						}
						Managers.personManager.DoAttachThing(attachmentPoint, attachmentThing, false);
					}
				}
				Managers.personManager.SaveOurLegAttachmentPointPositions();
			}
		}
		yield break;
	}

	// Token: 0x06001390 RID: 5008 RVA: 0x000AEEAC File Offset: 0x000AD2AC
	public string GetThingJsonFromCache(Thing thing)
	{
		string text = null;
		if (thing != null)
		{
			this.thingDefinitionCache.level1Cache.TryGetValue(thing.thingId, out text);
		}
		return text;
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x000AEEE4 File Offset: 0x000AD2E4
	public void MakeDeepThingClone(GameObject sourceThing, GameObject targetThing, bool alsoCloneIfNotContainingScript = false, bool isForPlacement = false, bool alwaysKeepThingPartsSeparate = false)
	{
		Thing component = sourceThing.GetComponent<Thing>();
		if (component.containsBehaviorScript || alsoCloneIfNotContainingScript || alwaysKeepThingPartsSeparate)
		{
			IEnumerator enumerator = targetThing.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					string tag = transform.tag;
					if (tag == "ThingPart" || tag == "IncludedSubThings" || tag == "PlacedSubThings" || tag == "ReflectionPartDuringEditing" || tag == "ContinuationPartDuringEditing")
					{
						Misc.Destroy(transform.gameObject);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
			string text = string.Empty;
			if (this.thingDefinitionCache.level1Cache.TryGetValue(component.thingId, out text))
			{
				string text2 = text;
				JsonToThingConverter.SetThing(targetThing, text2, alwaysKeepThingPartsSeparate, isForPlacement, null, null);
			}
			else
			{
				int num = 0;
				int num2 = 0;
				text = ThingToJsonConverter.GetJson(sourceThing, ref num, ref num2);
				string text2 = text;
				JsonToThingConverter.SetThing(targetThing, text2, alwaysKeepThingPartsSeparate, isForPlacement, null, null);
			}
			Component[] componentsInChildren = targetThing.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart in componentsInChildren)
			{
				if (thingPart.name != Universe.objectNameIfAlreadyDestroyed)
				{
					thingPart.SetStatePropertiesByTransform(false);
				}
			}
			Thing component2 = targetThing.GetComponent<Thing>();
			if (component2 != null)
			{
				if (isForPlacement)
				{
					component2.lastPositionForUndo = component.lastPositionForUndo;
					component2.lastRotationForUndo = component.lastRotationForUndo;
					component2.timeOfLastMemorizationForUndo = component.timeOfLastMemorizationForUndo;
					component2.distanceToShow = component.distanceToShow;
				}
				component2.isHighlighted = false;
			}
		}
		if (targetThing != null)
		{
			targetThing.name = Misc.RemoveCloneFromName(targetThing.name);
		}
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x000AF11C File Offset: 0x000AD51C
	public void EmitThingFromOrigin(ThingRequestContext thingRequestContext, Transform origin, string thingId, float velocityPercent, bool isGravityFree, bool omitSound = false)
	{
		if (this.thrownOrEmittedThingsParent.childCount < Managers.optimizationManager.maxThrownOrEmittedThingsForEmitting)
		{
			Thing component = origin.parent.GetComponent<Thing>();
			if (component != null && component.thingId != null)
			{
				string originParentThingId = component.thingId;
				Vector3 originPosition = origin.position;
				Vector3 originEulerAngles = origin.eulerAngles;
				Vector3 originForward = origin.forward;
				base.StartCoroutine(this.thingDefinitionCache.GetThingDefinition(thingRequestContext, thingId, delegate(string errorString, string thingDefinitionJSON)
				{
					if (errorString == null)
					{
						GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.thingGameObject);
						Thing component2 = gameObject.GetComponent<Thing>();
						gameObject.transform.parent = this.thrownOrEmittedThingsParent;
						component2.thingId = thingId;
						JsonToThingConverter.SetThing(gameObject, thingDefinitionJSON, false, false, null, null);
						component2.emittedByThingId = originParentThingId;
						component2.EmitMeFromOrigin(origin, originPosition, originEulerAngles, originForward, velocityPercent, isGravityFree, omitSound);
					}
					else
					{
						Log.Error(errorString);
					}
				}));
			}
		}
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x000AF210 File Offset: 0x000AD610
	public GameObject GetPlacementById(string placementId, bool ignorePlacedSubThings = false)
	{
		GameObject gameObject = null;
		if (!string.IsNullOrEmpty(placementId))
		{
			Component[] componentsInChildren = this.placements.GetComponentsInChildren(typeof(Thing), true);
			foreach (Thing thing in componentsInChildren)
			{
				if (thing.placementId == placementId && thing.CompareTag("Thing") && thing.name != Universe.objectNameIfAlreadyDestroyed)
				{
					bool flag = thing.transform.parent.CompareTag("PlacedSubThings");
					if (!flag || !ignorePlacedSubThings)
					{
						gameObject = thing.gameObject;
						break;
					}
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06001394 RID: 5012 RVA: 0x000AF2CC File Offset: 0x000AD6CC
	public GameObject GetThingByThrownId(string thrownId)
	{
		GameObject gameObject = null;
		Component[] componentsInChildren = this.thrownOrEmittedThingsParent.GetComponentsInChildren<Thing>();
		foreach (Thing thing in componentsInChildren)
		{
			if (thing.isThrownOrEmitted && thing.thrownId == thrownId)
			{
				gameObject = thing.gameObject;
				break;
			}
		}
		bool flag = gameObject == null;
		if (flag)
		{
			Component[] componentsInChildren2 = this.placements.GetComponentsInChildren<Thing>();
			foreach (Thing thing2 in componentsInChildren2)
			{
				if (thing2.isThrownOrEmitted && thing2.thrownId == thrownId)
				{
					gameObject = thing2.gameObject;
					Log.Debug("Found as sticky");
					break;
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06001395 RID: 5013 RVA: 0x000AF3B0 File Offset: 0x000AD7B0
	public void GetTopThingIdsCreatedByPerson(string personId, Action<List<string>> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetTopThingIdsCreatedByPerson(personId, 4, delegate(GetTopThingIdsCreatedByPerson_Response response)
		{
			if (response.error == null)
			{
				callback(response.Ids);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06001396 RID: 5014 RVA: 0x000AF3EC File Offset: 0x000AD7EC
	public Vector3 GetAppropriateDownScaleForThing(GameObject thing, float maxScale = 0.1f, bool avoidUpscalingSmallThings = false)
	{
		Transform parent = thing.transform.parent;
		Vector3 localScale = thing.transform.localScale;
		thing.transform.parent = null;
		thing.transform.localScale = Vector3.one;
		Component[] componentsInChildren = thing.GetComponentsInChildren(typeof(ParticleSystem), true);
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			particleSystem.gameObject.SetActive(false);
		}
		Vector3 combinedBoundsSizeOfThingParts = ThingManager.GetCombinedBoundsSizeOfThingParts(thing);
		float num = maxScale / Misc.GetLargestValueOfVector(combinedBoundsSizeOfThingParts);
		if (avoidUpscalingSmallThings && num > 1f)
		{
			num = 1f;
		}
		foreach (ParticleSystem particleSystem2 in componentsInChildren)
		{
			particleSystem2.gameObject.SetActive(true);
		}
		thing.transform.parent = parent;
		thing.transform.localScale = localScale;
		return Misc.GetUniformVector3(num);
	}

	// Token: 0x06001397 RID: 5015 RVA: 0x000AF4FC File Offset: 0x000AD8FC
	public static Vector3 GetCombinedBoundsSizeOfThingParts(GameObject parentObject)
	{
		Bounds bounds = new Bounds(parentObject.transform.position, Vector3.zero);
		foreach (Renderer renderer in parentObject.GetComponentsInChildren(typeof(Renderer), true))
		{
			if (renderer.gameObject.CompareTag("ThingPart"))
			{
				bounds.Encapsulate(renderer.bounds);
			}
		}
		return bounds.size;
	}

	// Token: 0x06001398 RID: 5016 RVA: 0x000AF578 File Offset: 0x000AD978
	public void UpdateStats()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
		this.statsThingsAroundPosition = 0;
		this.statsThingPartsAroundPosition = 0;
		this.statsThingsInArea = this.placements.transform.childCount;
		IEnumerator enumerator = this.placements.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.gameObject.activeSelf)
				{
					Thing component = transform.GetComponent<Thing>();
					string thingId = component.thingId;
					int thingPartCount = this.GetThingPartCount(transform.gameObject);
					if (!dictionary.ContainsKey(thingId))
					{
						dictionary.Add(thingId, 0);
						dictionary2.Add(thingId, component.givenName);
					}
					Dictionary<string, int> dictionary3;
					string text;
					(dictionary3 = dictionary)[text = thingId] = dictionary3[text] + thingPartCount;
					this.statsThingsAroundPosition++;
					this.statsThingPartsAroundPosition += thingPartCount;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x06001399 RID: 5017 RVA: 0x000AF698 File Offset: 0x000ADA98
	public bool GetPlacementsReachedLimit(out string info, GameObject dialogFundament = null)
	{
		info = string.Empty;
		this.UpdateStats();
		bool flag = this.statsThingsInArea >= 10000;
		if (flag)
		{
			info = "Sorry, your area capacity has reached the limit of " + 10000 + " things. You may want to set up transports to other areas you make to combine into a bigger location.";
		}
		return flag;
	}

	// Token: 0x0600139A RID: 5018 RVA: 0x000AF6E8 File Offset: 0x000ADAE8
	public int GetThingPartCount(GameObject thing)
	{
		int num = 0;
		IEnumerator enumerator = thing.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.gameObject.CompareTag("ThingPart"))
				{
					num++;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		return num;
	}

	// Token: 0x0600139B RID: 5019 RVA: 0x000AF764 File Offset: 0x000ADB64
	public int GetThingPartCountFullDepthWithStrictSyncCount(GameObject thing, out int strictSyncCount)
	{
		Component[] componentsInChildren = thing.GetComponentsInChildren<ThingPart>();
		strictSyncCount = 0;
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.IsStrictSyncingToAreaNewcomersNeeded(true))
			{
				strictSyncCount++;
			}
		}
		return componentsInChildren.Length;
	}

	// Token: 0x0600139C RID: 5020 RVA: 0x000AF7AF File Offset: 0x000ADBAF
	public void LoadMaterial(GameObject gameObject, string assetPath)
	{
		base.StartCoroutine(this.LoadMaterialAsync(gameObject, assetPath));
	}

	// Token: 0x0600139D RID: 5021 RVA: 0x000AF7C0 File Offset: 0x000ADBC0
	private IEnumerator LoadMaterialAsync(GameObject gameObject, string assetPath)
	{
		ResourceRequest request = Resources.LoadAsync(assetPath);
		yield return request;
		if (!request.isDone)
		{
			Log.Debug("undone object after yield! " + assetPath);
		}
		if (gameObject != null)
		{
			Renderer component = gameObject.GetComponent<Renderer>();
			Material[] materials = component.materials;
			materials[0] = (Material)request.asset;
			if (materials[0] != null)
			{
				component.materials = materials;
			}
			else
			{
				Log.Debug("Material was null on load: " + assetPath);
			}
		}
		yield break;
	}

	// Token: 0x0600139E RID: 5022 RVA: 0x000AF7E4 File Offset: 0x000ADBE4
	public string ExportThing(Thing thing)
	{
		string text = Application.persistentDataPath + "/things";
		Directory.CreateDirectory(text);
		Vector3 position = thing.transform.position;
		thing.transform.position = Vector3.zero;
		OBJExporter objexporter = new OBJExporter();
		objexporter.applyPosition = true;
		Component[] componentsInChildren = thing.gameObject.GetComponentsInChildren<ThingPart>();
		GameObject[] array = new GameObject[componentsInChildren.Length];
		int num = 0;
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.material.name.IndexOf("(Instance)") >= 0)
			{
				thingPart.material.name = JsonToThingConverter.GetNormalizedColorString(thingPart.material.color);
			}
			array[num++] = thingPart.gameObject;
		}
		string text2 = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss");
		string text3 = thing.givenName.Replace(" ", "-").ToLower();
		string text4 = "abcdefghijklmnopqrstuvwxyz0123456789-";
		if (Validator.ContainsOnly(text3, text4))
		{
			text2 = text3 + "-" + text2;
		}
		string text5 = text + "/" + text2 + ".obj";
		objexporter.Export(text5, array);
		string text6 = text + "/" + text2 + ".json";
		string text7 = string.Empty;
		if (this.thingDefinitionCache.level1Cache.TryGetValue(thing.thingId, out text7))
		{
			File.WriteAllText(text6, text7);
		}
		else
		{
			int num2 = 0;
			int num3 = 0;
			text7 = ThingToJsonConverter.GetJson(thing.gameObject, ref num2, ref num3);
			File.WriteAllText(text6, text7);
		}
		thing.transform.position = position;
		return text;
	}

	// Token: 0x0600139F RID: 5023 RVA: 0x000AF9A4 File Offset: 0x000ADDA4
	public string ExportAllThings(List<string> thingIdsByOtherCreators)
	{
		string text = Application.persistentDataPath + "/areas";
		Directory.CreateDirectory(text);
		string text2 = DateTime.UtcNow.ToString("yyyy-MM-dd-HH-mm-ss");
		string text3 = Managers.areaManager.currentAreaName.Replace(" ", "-").ToLower();
		text3 = text3.Replace("'", string.Empty);
		string text4 = "abcdefghijklmnopqrstuvwxyz0123456789-";
		if (Validator.ContainsOnly(text3, text4))
		{
			text2 = text3 + "-" + text2;
		}
		string text5 = text + "/" + text2 + "-things";
		Directory.CreateDirectory(text5);
		OBJExporter objexporter = new OBJExporter();
		objexporter.applyPosition = true;
		Component[] componentsInChildren = this.placements.GetComponentsInChildren(typeof(ThingPart), true);
		GameObject[] array = new GameObject[componentsInChildren.Length];
		List<string> list = new List<string>();
		int num = 0;
		foreach (Component component in componentsInChildren)
		{
			if (component.transform.parent != null)
			{
				Thing component2 = component.transform.parent.GetComponent<Thing>();
				if (component2 != null)
				{
					ThingPart component3 = component.GetComponent<ThingPart>();
					if (!thingIdsByOtherCreators.Contains(component2.thingId) || component2.isClonable)
					{
						if (!component3.invisible)
						{
							array[num++] = component.gameObject;
						}
						if (!list.Contains(component2.thingId))
						{
							list.Add(component2.thingId);
							string text6 = text5 + "/" + component2.thingId + ".json";
							string json;
							if (!this.thingDefinitionCache.level1Cache.TryGetValue(component2.thingId, out json))
							{
								int num2 = 0;
								int num3 = 0;
								json = ThingToJsonConverter.GetJson(component2.gameObject, ref num2, ref num3);
							}
							File.WriteAllText(text6, json);
						}
					}
				}
			}
		}
		string text7 = text + "/" + text2 + ".obj";
		objexporter.Export(text7, array);
		string text8 = text + "/" + text2 + ".json";
		string areaJsonForExport = Managers.areaManager.GetAreaJsonForExport();
		File.WriteAllText(text8, areaJsonForExport);
		return text;
	}

	// Token: 0x060013A0 RID: 5024 RVA: 0x000AFBEC File Offset: 0x000ADFEC
	public void ResetTriggeredOnSomeoneNewInVicinity()
	{
		Component[] componentsInChildren = this.placements.GetComponentsInChildren(typeof(Thing), true);
		foreach (Thing thing in componentsInChildren)
		{
			thing.triggeredOnSomeoneNewInVicinity = false;
		}
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x000AFC38 File Offset: 0x000AE038
	public string GetUnremovableCenterColorString(Thing thing)
	{
		string text = string.Empty;
		Component[] componentsInChildren = thing.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.isUnremovableCenter && thingPart.material != null)
			{
				Color32 color = thingPart.material.color;
				text = string.Concat(new object[] { color.r, ",", color.g, ",", color.b });
			}
		}
		return text;
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x000AFCF0 File Offset: 0x000AE0F0
	public void PlacePlacedSubThingsAsTheyWereOriginallyPositioned(GameObject thingObject)
	{
		Component[] componentsInChildren = thingObject.GetComponentsInChildren<ThingPart>();
		int num = 0;
		foreach (ThingPart thingPart in componentsInChildren)
		{
			Dictionary<string, ThingIdPositionRotation> placedSubThingIdsWithOriginalInfoClone = this.GetPlacedSubThingIdsWithOriginalInfoClone(thingPart.placedSubThingIdsWithOriginalInfo);
			foreach (KeyValuePair<string, ThingIdPositionRotation> keyValuePair in placedSubThingIdsWithOriginalInfoClone)
			{
				ThingIdPositionRotation value = keyValuePair.Value;
				this.PlaceBasedOnOldThingIdPositionRotation(ThingRequestContext.PlaceOriginalPlacedSubThings, thingPart, value);
				if (++num >= 20)
				{
					return;
				}
			}
		}
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x000AFDA4 File Offset: 0x000AE1A4
	private Dictionary<string, ThingIdPositionRotation> GetPlacedSubThingIdsWithOriginalInfoClone(Dictionary<string, ThingIdPositionRotation> original)
	{
		Dictionary<string, ThingIdPositionRotation> dictionary = new Dictionary<string, ThingIdPositionRotation>();
		foreach (KeyValuePair<string, ThingIdPositionRotation> keyValuePair in original)
		{
			dictionary.Add(keyValuePair.Key, keyValuePair.Value);
		}
		return dictionary;
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x000AFE10 File Offset: 0x000AE210
	private void PlaceBasedOnOldThingIdPositionRotation(ThingRequestContext thingRequestContext, ThingPart thingPart, ThingIdPositionRotation thingIdPositionRotation)
	{
		base.StartCoroutine(this.thingDefinitionCache.GetThingDefinition(thingRequestContext, thingIdPositionRotation.thingId, delegate(string errorString, string thingDefinitionJSON)
		{
			if (errorString == null)
			{
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.thingGameObject);
				Thing component = gameObject.GetComponent<Thing>();
				component.thingId = thingIdPositionRotation.thingId;
				JsonToThingConverter.SetThing(gameObject, thingDefinitionJSON, false, true, null, null);
				Vector3 localScale = thingPart.transform.localScale;
				thingPart.transform.localScale = Vector3.one;
				gameObject.transform.parent = thingPart.transform;
				gameObject.transform.localPosition = thingIdPositionRotation.position;
				gameObject.transform.localEulerAngles = thingIdPositionRotation.rotation;
				gameObject.transform.parent = this.placements.transform;
				thingPart.transform.localScale = localScale;
				component.MemorizeOriginalTransform(true);
				string text = Managers.personManager.DoPlaceRecreatedPlacedSubThing(gameObject);
				thingPart.placedSubThingIdsWithOriginalInfo.Add(text, thingIdPositionRotation);
			}
			else
			{
				Log.Error(errorString);
			}
		}));
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x000AFE64 File Offset: 0x000AE264
	public void UpdatePlacedSubThingsInfo(Thing thing)
	{
		ThingPart[] componentsInChildren = thing.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			Dictionary<string, ThingIdPositionRotation> placedSubThingIdsWithOriginalInfoClone = this.GetPlacedSubThingIdsWithOriginalInfoClone(thingPart.placedSubThingIdsWithOriginalInfo);
			thingPart.placedSubThingIdsWithOriginalInfo = new Dictionary<string, ThingIdPositionRotation>();
			foreach (KeyValuePair<string, ThingIdPositionRotation> keyValuePair in placedSubThingIdsWithOriginalInfoClone)
			{
				string key = keyValuePair.Key;
				ThingIdPositionRotation value = keyValuePair.Value;
				GameObject placementById = this.GetPlacementById(key, false);
				if (placementById != null)
				{
					Thing component = placementById.GetComponent<Thing>();
					if (component != null)
					{
						thingPart.ResetStates();
						Transform parent = placementById.transform.parent;
						Vector3 localScale = thingPart.transform.localScale;
						thingPart.transform.localScale = Vector3.one;
						placementById.transform.parent = thingPart.transform;
						thingPart.AddConfirmedNonExistingPlacedSubThingId(key, component.thingId, placementById.transform.localPosition, placementById.transform.localEulerAngles);
						placementById.transform.parent = parent;
						thingPart.transform.localScale = localScale;
					}
				}
			}
		}
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x000AFFB8 File Offset: 0x000AE3B8
	public void StoreThingJsonInAllCaches(string thingId, string json)
	{
		this.thingDefinitionCache.StoreInAllCaches(thingId, json);
	}

	// Token: 0x060013A7 RID: 5031 RVA: 0x000AFFC8 File Offset: 0x000AE3C8
	public Dictionary<TextureProperty, float>[] CloneTextureProperties(Dictionary<TextureProperty, float>[] originalTextureProperties)
	{
		Dictionary<TextureProperty, float>[] array = null;
		if (originalTextureProperties != null)
		{
			array = new Dictionary<TextureProperty, float>[2];
			for (int i = 0; i < originalTextureProperties.Length; i++)
			{
				array[i] = this.CloneTextureProperty(originalTextureProperties[i]);
			}
		}
		return array;
	}

	// Token: 0x060013A8 RID: 5032 RVA: 0x000B0008 File Offset: 0x000AE408
	public Dictionary<TextureProperty, float> CloneTextureProperty(Dictionary<TextureProperty, float> originalTextureProperty)
	{
		Dictionary<TextureProperty, float> dictionary = null;
		if (originalTextureProperty != null)
		{
			dictionary = new Dictionary<TextureProperty, float>();
			foreach (KeyValuePair<TextureProperty, float> keyValuePair in originalTextureProperty)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		return dictionary;
	}

	// Token: 0x060013A9 RID: 5033 RVA: 0x000B007C File Offset: 0x000AE47C
	public void SetTexturePropertiesToDefault(Dictionary<TextureProperty, float> textureProperty, TextureType textureType)
	{
		IEnumerator enumerator = Enum.GetValues(typeof(TextureProperty)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				TextureProperty textureProperty2 = (TextureProperty)obj;
				if (textureProperty.ContainsKey(textureProperty2))
				{
					textureProperty[textureProperty2] = this.texturePropertyDefault[textureProperty2];
				}
				else
				{
					textureProperty.Add(textureProperty2, this.texturePropertyDefault[textureProperty2]);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		float num = 1f;
		float num2;
		if (this.textureAlphaCaps.TryGetValue(textureType, out num2))
		{
			num = num2;
		}
		textureProperty[TextureProperty.Strength] = Mathf.Lerp(0.5f, 0.5f + num * 0.5f, 0.785f);
		if (this.IsAlgorithmTextureType(textureType))
		{
			textureProperty[TextureProperty.ScaleX] = 0.17f;
			textureProperty[TextureProperty.ScaleY] = textureProperty[TextureProperty.ScaleX];
			textureProperty[TextureProperty.OffsetX] = 0.5f;
			textureProperty[TextureProperty.OffsetY] = 0.5f;
		}
		switch (textureType)
		{
		case TextureType.Vertex_Scatter:
		case TextureType.Vertex_Expand:
		case TextureType.Vertex_Slice:
			textureProperty[TextureProperty.Strength] = 0.5f;
			break;
		default:
			if (textureType != TextureType.VoronoiDots)
			{
				if (textureType == TextureType.SideGlow)
				{
					textureProperty[TextureProperty.Strength] = 0f;
				}
			}
			else
			{
				textureProperty[TextureProperty.Param2] = 0.25f;
			}
			break;
		case TextureType.Wireframe:
			textureProperty[TextureProperty.Strength] = 0.75f;
			break;
		case TextureType.Outline:
			textureProperty[TextureProperty.Strength] = 0.05f;
			break;
		}
	}

	// Token: 0x060013AA RID: 5034 RVA: 0x000B0224 File Offset: 0x000AE624
	public void AdditionallyModulateTexturePropertiesByType(Dictionary<TextureProperty, float> textureProperty, TextureType textureType)
	{
	}

	// Token: 0x060013AB RID: 5035 RVA: 0x000B0228 File Offset: 0x000AE628
	public void SetParticleSystemPropertiesToDefault(Dictionary<ParticleSystemProperty, float> particleSystemProperty, ParticleSystemType particleSystemType)
	{
		IEnumerator enumerator = Enum.GetValues(typeof(ParticleSystemProperty)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				ParticleSystemProperty particleSystemProperty2 = (ParticleSystemProperty)obj;
				if (particleSystemProperty.ContainsKey(particleSystemProperty2))
				{
					particleSystemProperty[particleSystemProperty2] = this.particleSystemPropertyDefault[particleSystemProperty2];
				}
				else
				{
					particleSystemProperty.Add(particleSystemProperty2, this.particleSystemPropertyDefault[particleSystemProperty2]);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		switch (particleSystemType)
		{
		case ParticleSystemType.AreaEmbers:
			particleSystemProperty[ParticleSystemProperty.Shape] = 0.75f;
			break;
		default:
			if (particleSystemType != ParticleSystemType.FireMore)
			{
				if (particleSystemType != ParticleSystemType.OrganicSplatter)
				{
					if (particleSystemType != ParticleSystemType.TwisterLines)
					{
						if (particleSystemType == ParticleSystemType.TwistedSmoke)
						{
							particleSystemProperty[ParticleSystemProperty.Alpha] = 0.05f;
						}
					}
					else
					{
						particleSystemProperty[ParticleSystemProperty.Size] = 0.025f;
					}
				}
				else
				{
					particleSystemProperty[ParticleSystemProperty.Size] = 0.4f;
				}
			}
			else
			{
				particleSystemProperty[ParticleSystemProperty.Shape] = 0.5f;
				particleSystemProperty[ParticleSystemProperty.Size] = 0.5f;
			}
			break;
		case ParticleSystemType.CenteredElectric:
			particleSystemProperty[ParticleSystemProperty.Speed] = 0f;
			break;
		case ParticleSystemType.Smoke:
			particleSystemProperty[ParticleSystemProperty.Shape] = 0.5f;
			particleSystemProperty[ParticleSystemProperty.Size] = 0.5f;
			break;
		case ParticleSystemType.CircularSmoke:
			particleSystemProperty[ParticleSystemProperty.Alpha] = 0.1f;
			break;
		case ParticleSystemType.Shards:
		case ParticleSystemType.RoughShards:
			particleSystemProperty[ParticleSystemProperty.Size] = 0.1f;
			particleSystemProperty[ParticleSystemProperty.Shape] = 0.6f;
			particleSystemProperty[ParticleSystemProperty.Gravity] = 0.1f;
			particleSystemProperty[ParticleSystemProperty.Speed] = 0.35f;
			break;
		case ParticleSystemType.FireThrow:
			particleSystemProperty[ParticleSystemProperty.Size] = 0.35f;
			particleSystemProperty[ParticleSystemProperty.Shape] = 0.5f;
			break;
		case ParticleSystemType.SoftFire:
			particleSystemProperty[ParticleSystemProperty.Shape] = 0.5f;
			particleSystemProperty[ParticleSystemProperty.Size] = 0.5f;
			particleSystemProperty[ParticleSystemProperty.Alpha] = 0.5f;
			break;
		case ParticleSystemType.SpiralSmoke:
			particleSystemProperty[ParticleSystemProperty.Amount] = 0.4f;
			particleSystemProperty[ParticleSystemProperty.Size] = 0.5f;
			particleSystemProperty[ParticleSystemProperty.Shape] = 0.5f;
			break;
		case ParticleSystemType.TwisterSmoke:
			particleSystemProperty[ParticleSystemProperty.Amount] = 0.4f;
			particleSystemProperty[ParticleSystemProperty.Size] = 0.5f;
			particleSystemProperty[ParticleSystemProperty.Shape] = 0.5f;
			break;
		case ParticleSystemType.ShrinkSmoke:
		case ParticleSystemType.ThickSmoke:
		case ParticleSystemType.PlopSmoke:
			particleSystemProperty[ParticleSystemProperty.Size] = 0.5f;
			particleSystemProperty[ParticleSystemProperty.Shape] = 0.5f;
			break;
		case ParticleSystemType.LightStreaks:
			particleSystemProperty[ParticleSystemProperty.Size] = 0.7f;
			break;
		case ParticleSystemType.SoftSmoke:
			particleSystemProperty[ParticleSystemProperty.Size] = 0.5f;
			particleSystemProperty[ParticleSystemProperty.Shape] = 0.5f;
			particleSystemProperty[ParticleSystemProperty.Speed] = 0f;
			break;
		}
	}

	// Token: 0x060013AC RID: 5036 RVA: 0x000B0524 File Offset: 0x000AE924
	public Dictionary<ParticleSystemProperty, float> CloneParticleSystemProperty(Dictionary<ParticleSystemProperty, float> originalParticleSystemProperty)
	{
		Dictionary<ParticleSystemProperty, float> dictionary = null;
		if (originalParticleSystemProperty != null)
		{
			dictionary = new Dictionary<ParticleSystemProperty, float>();
			foreach (KeyValuePair<ParticleSystemProperty, float> keyValuePair in originalParticleSystemProperty)
			{
				dictionary.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
		return dictionary;
	}

	// Token: 0x060013AD RID: 5037 RVA: 0x000B0598 File Offset: 0x000AE998
	public List<TextureProperty> GetTexturePropertiesList(TextureType textureType)
	{
		List<TextureProperty> list = new List<TextureProperty>();
		if (textureType == TextureType.Vertex_Scatter)
		{
			list.Add(TextureProperty.Strength);
			list.Add(TextureProperty.Glow);
			list.Add(TextureProperty.Param1);
			list.Add(TextureProperty.Param2);
		}
		else if (textureType == TextureType.Vertex_Expand)
		{
			list.Add(TextureProperty.Strength);
			list.Add(TextureProperty.Glow);
		}
		else if (textureType == TextureType.Vertex_Slice)
		{
			list.Add(TextureProperty.ScaleY);
			list.Add(TextureProperty.ScaleX);
			list.Add(TextureProperty.Strength);
			list.Add(TextureProperty.Glow);
		}
		else
		{
			int num = 0;
			if (this.textureExtraParamsNumber.ContainsKey(textureType))
			{
				num = this.textureExtraParamsNumber[textureType];
			}
			IEnumerator enumerator = Enum.GetValues(typeof(TextureProperty)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					TextureProperty textureProperty = (TextureProperty)obj;
					switch (textureProperty)
					{
					case TextureProperty.Param1:
						if (num >= 1)
						{
							list.Add(textureProperty);
						}
						break;
					case TextureProperty.Param2:
						if (num >= 2)
						{
							list.Add(textureProperty);
						}
						break;
					case TextureProperty.Param3:
						if (num >= 3)
						{
							list.Add(textureProperty);
						}
						break;
					default:
						list.Add(textureProperty);
						break;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}
		return list;
	}

	// Token: 0x060013AE RID: 5038 RVA: 0x000B06FC File Offset: 0x000AEAFC
	public List<ParticleSystemProperty> GetParticleSystemPropertiesList(ParticleSystemType particleSystemType)
	{
		List<ParticleSystemProperty> list = new List<ParticleSystemProperty>();
		IEnumerator enumerator = Enum.GetValues(typeof(ParticleSystemProperty)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				ParticleSystemProperty particleSystemProperty = (ParticleSystemProperty)obj;
				list.Add(particleSystemProperty);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = enumerator as IDisposable) != null)
			{
				disposable.Dispose();
			}
		}
		return list;
	}

	// Token: 0x060013AF RID: 5039 RVA: 0x000B0774 File Offset: 0x000AEB74
	public void ReplaceThing(Thing oldThing, Thing newThing)
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.thingGameObject);
		Thing component = gameObject.GetComponent<Thing>();
		gameObject.SetActive(oldThing.gameObject.activeSelf);
		Vector3 localScale = oldThing.transform.localScale;
		component.thingId = newThing.thingId;
		Managers.thingManager.MakeDeepThingClone(newThing.gameObject, gameObject, true, true, false);
		component.placementId = oldThing.placementId;
		gameObject.transform.parent = this.placements.transform;
		component.transform.position = oldThing.transform.position;
		component.transform.rotation = oldThing.transform.rotation;
		component.transform.localScale = localScale;
		component.MemorizeOriginalTransform(true);
		Misc.Destroy(oldThing.gameObject);
	}

	// Token: 0x060013B0 RID: 5040 RVA: 0x000B0840 File Offset: 0x000AEC40
	public bool IsClosestSurfaceNearbyOurPerson(ThingPart sourceThingPart, float maxDistance)
	{
		bool flag = false;
		if (sourceThingPart.transform.parent != null)
		{
			Thing component = sourceThingPart.transform.parent.GetComponent<Thing>();
			if (component != null)
			{
				float? num = null;
				ThingPart thingPart = null;
				Vector3[] sphereDirections = this.GetSphereDirections(16);
				foreach (Vector3 vector in sphereDirections)
				{
					RaycastHit raycastHit;
					if (Physics.Raycast(sourceThingPart.transform.position, vector, out raycastHit, 2f) && (num == null || raycastHit.distance < num.Value))
					{
						ThingPart component2 = raycastHit.collider.gameObject.GetComponent<ThingPart>();
						if (component2 != null && component2 != sourceThingPart && component2.transform.parent != null)
						{
							Thing component3 = component2.transform.parent.GetComponent<Thing>();
							if (component3 != null && component3 != component && !component3.uncollidable)
							{
								num = new float?(raycastHit.distance);
								thingPart = component2;
							}
						}
					}
				}
				if (thingPart != null && Managers.personManager.GetIsThisObjectOfOurPerson(thingPart.gameObject, false))
				{
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x060013B1 RID: 5041 RVA: 0x000B09BC File Offset: 0x000AEDBC
	public Vector3[] GetSphereDirections(int numDirections = 16)
	{
		Vector3[] array = new Vector3[numDirections];
		double num = 3.141592653589793 * (3.0 - Math.Sqrt(5.0));
		float num2 = 2f / (float)numDirections;
		foreach (int num3 in Enumerable.Range(0, numDirections))
		{
			float num4 = (float)num3 * num2 - 1f + num2 / 2f;
			double num5 = Math.Sqrt((double)(1f - num4 * num4));
			double num6 = (double)num3 * num;
			float num7 = (float)(Math.Cos(num6) * num5);
			float num8 = (float)(Math.Sin(num6) * num5);
			array[num3] = new Vector3(num7, num4, num8);
		}
		return array;
	}

	// Token: 0x060013B2 RID: 5042 RVA: 0x000B0AA8 File Offset: 0x000AEEA8
	public string GetVersionInfo(int version)
	{
		string text = string.Empty;
		switch (version)
		{
		case 1:
			text = "To emulate an old bug-used-as-feature-by-people behavior, for downwards-compatibility reasons, \"send nearby\" commands of things saved at this version are treated to mean \"send one nearby\" when part of emitted or thrown items stuck to someone.";
			break;
		case 2:
			text = "If the thingPart includes an image, the material will be forced to become default and white (and black during loading). In version 3+ it will be left as is, and e.g. glowing becomes a glowing image, andthingPart colors are being respected.";
			break;
		case 3:
			text = "The default font material in version 4+ is non-glowing. Version 3- fonts will take on the glow material.";
			break;
		case 4:
			text = "In version 4-, bouncy & slidy for thrown/ emitted things were both selectable, but mutually exclusive in effect (defaulting on bouncy). Since v5+ they mix.";
			break;
		case 5:
			text = "In version 5-, \"tell web\" and \"tell any web\" didn't exist as special tell scope commands, so they will be understood as being tell/ tell any with \"web\" as data.";
			break;
		case 6:
			text = "In version 6-, one unit of the \"set constant rotation\" command equals 10 rotation degree (instead of 1 in later version).";
			break;
		case 7:
			text = "In version 8, the \"tell in front\" and \"tell first front\" commands were added. In version 7-, \"in front\"/ \"first in front\" are considered normal tell data text.";
			break;
		case 8:
			text = "As of version 9, sounds played via the Loop command adhere to the Thing's Surround Sound attribute. In version 8-, that setting was ignored.";
			break;
		case 9:
			text = "The current version.";
			break;
		}
		return text;
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x000B0B50 File Offset: 0x000AEF50
	public GameObject GetClosestThingOfNameIn(GameObject originObject, List<GameObject> parentNodesToSearchThrough, string targetName)
	{
		GameObject gameObject = null;
		float? num = null;
		Vector3 position = originObject.transform.position;
		foreach (GameObject gameObject2 in parentNodesToSearchThrough)
		{
			Component[] componentsInChildren = gameObject2.GetComponentsInChildren(typeof(Thing), true);
			foreach (Thing thing in componentsInChildren)
			{
				GameObject gameObject3 = thing.gameObject;
				if (gameObject3.name == targetName && gameObject3 != CreationHelper.thingBeingEdited && gameObject3 != originObject && !thing.isInInventoryOrDialog && !thing.isGiftInDialog)
				{
					float num2 = Vector3.Distance(position, thing.transform.position);
					if (num == null || num2 < num.Value)
					{
						num = new float?(num2);
						gameObject = gameObject3;
					}
				}
			}
		}
		return gameObject;
	}

	// Token: 0x060013B4 RID: 5044 RVA: 0x000B0C88 File Offset: 0x000AF088
	public bool IsVertexTexture(TextureType textureType)
	{
		return textureType == TextureType.Vertex_Scatter || textureType == TextureType.Vertex_Expand || textureType == TextureType.Vertex_Slice;
	}

	// Token: 0x060013B5 RID: 5045 RVA: 0x000B0CAC File Offset: 0x000AF0AC
	public void SearchThings(string term, Action<List<string>> callback)
	{
		base.StartCoroutine(Managers.serverManager.SearchThings(term, delegate(SearchThings_Response response)
		{
			if (response.error == null)
			{
				callback(response.thingIds);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x060013B6 RID: 5046 RVA: 0x000B0CE4 File Offset: 0x000AF0E4
	public bool IsParticleSystemTypeWithOnlyAlphaSetting(ParticleSystemType type)
	{
		return this.particleSystemTypeWithOnlyAlphaSetting.ContainsKey(type);
	}

	// Token: 0x060013B7 RID: 5047 RVA: 0x000B0CF2 File Offset: 0x000AF0F2
	public bool IsTextureTypeWithOnlyAlphaSetting(TextureType type)
	{
		return this.textureTypeWithOnlyAlphaSetting.ContainsKey(type);
	}

	// Token: 0x060013B8 RID: 5048 RVA: 0x000B0D00 File Offset: 0x000AF100
	public bool IsAlgorithmTextureType(TextureType type)
	{
		return this.algorithmTextureTypes.ContainsKey(type);
	}

	// Token: 0x060013B9 RID: 5049 RVA: 0x000B0D10 File Offset: 0x000AF110
	public Material GetSharedOrDistinctTextureMaterial(ThingPart thingPart, int textureIndex, bool isBeingEdited)
	{
		Material material = null;
		string text = "Textures/" + thingPart.textureTypes[textureIndex].ToString().Replace("_", "/");
		ThingPartState thingPartState = thingPart.states[0];
		if (isBeingEdited || thingPart.states.Count >= 2 || thingPart.textureTypes[textureIndex] == TextureType.None)
		{
			material = global::UnityEngine.Object.Instantiate<Material>(Resources.Load(text) as Material);
			if (thingPart.textureTypes[textureIndex] != TextureType.None && thingPart.states.Count >= 2)
			{
				thingPart.ApplyTextureColorByThis(thingPartState.textureColors[textureIndex], material);
				Dictionary<TextureProperty, float> dictionary = this.CloneTextureProperty(thingPartState.textureProperties[textureIndex]);
				if (dictionary != null)
				{
					thingPart.ModulateTheseTextureProperties(dictionary, thingPart.textureTypes[textureIndex]);
					thingPart.ApplyTexturePropertiesToMaterial(dictionary, material);
				}
			}
		}
		else
		{
			string sharedTextureId = this.GetSharedTextureId(thingPart, textureIndex);
			if (!string.IsNullOrEmpty(sharedTextureId))
			{
				Material material2 = null;
				if (this.sharedTextureMaterials.TryGetValue(sharedTextureId, out material2))
				{
					material = global::UnityEngine.Object.Instantiate<Material>(material2);
				}
				else
				{
					try
					{
						material = global::UnityEngine.Object.Instantiate<Material>(Resources.Load(text) as Material);
					}
					catch (Exception ex)
					{
						Log.Debug("Material " + text + " not found, switching to None");
						material = global::UnityEngine.Object.Instantiate<Material>(Resources.Load("Textures/None") as Material);
					}
					material.name = sharedTextureId;
					thingPart.ApplyTextureColorByThis(thingPartState.textureColors[textureIndex], material);
					Dictionary<TextureProperty, float> dictionary2 = this.CloneTextureProperty(thingPartState.textureProperties[textureIndex]);
					thingPart.ModulateTheseTextureProperties(dictionary2, thingPart.textureTypes[textureIndex]);
					thingPart.ApplyTexturePropertiesToMaterial(dictionary2, material);
					this.sharedTextureMaterials.Add(sharedTextureId, material);
				}
			}
		}
		return material;
	}

	// Token: 0x060013BA RID: 5050 RVA: 0x000B0EE4 File Offset: 0x000AF2E4
	private string GetSharedTextureId(ThingPart thingPart, int textureIndex)
	{
		string text = string.Empty;
		ThingPartState thingPartState = thingPart.states[0];
		if (thingPartState.textureProperties != null && thingPartState.textureProperties[textureIndex] != null)
		{
			string text2 = text;
			int num = (int)thingPart.textureTypes[textureIndex];
			text = text2 + num.ToString() + "_";
			Color color = thingPartState.textureColors[textureIndex];
			text = text + JsonToThingConverter.GetNormalizedColorString(color) + "_";
			bool flag = this.IsVertexTexture(thingPart.textureTypes[textureIndex]);
			if (flag)
			{
				text = text + "_vertex" + ++this.vertexTextureCounter;
			}
			foreach (KeyValuePair<TextureProperty, float> keyValuePair in thingPartState.textureProperties[textureIndex])
			{
				text = text + keyValuePair.Value.ToString().Replace("0.", ".") + "_";
			}
		}
		return text;
	}

	// Token: 0x060013BB RID: 5051 RVA: 0x000B101C File Offset: 0x000AF41C
	public ThingPart GetRandomThingPartForTesting()
	{
		return null;
	}

	// Token: 0x060013BC RID: 5052 RVA: 0x000B102C File Offset: 0x000AF42C
	public void AddOutlineHighlightMaterial(Thing thing, bool useInnerGlow = false)
	{
		bool flag = !useInnerGlow;
		if (this.outlineHighlightMaterial == null)
		{
			this.outlineHighlightMaterial = Resources.Load("Materials/HighlightAndAlwaysVisible", typeof(Material)) as Material;
			this.innerGlowHighlightMaterial = Resources.Load("Materials/InnerGlowHighlight", typeof(Material)) as Material;
		}
		if (!thing.isHighlighted)
		{
			thing.isHighlighted = true;
			Component[] componentsInChildren = thing.GetComponentsInChildren(typeof(ThingPart), true);
			foreach (ThingPart thingPart in componentsInChildren)
			{
				if (!thingPart.isText || flag)
				{
					Renderer component = thingPart.gameObject.GetComponent<MeshRenderer>();
					Material[] materials = component.materials;
					Material[] array2 = new Material[materials.Length + 1];
					for (int j = 0; j < materials.Length; j++)
					{
						array2[j] = materials[j];
					}
					array2[array2.Length - 1] = ((!useInnerGlow) ? this.outlineHighlightMaterial : this.innerGlowHighlightMaterial);
					component.materials = array2;
				}
			}
		}
	}

	// Token: 0x060013BD RID: 5053 RVA: 0x000B1158 File Offset: 0x000AF558
	public void RemoveOutlineHighlightMaterial(Thing thing, bool useInnerGlow = false)
	{
		bool flag = !useInnerGlow;
		if (thing.isHighlighted)
		{
			thing.isHighlighted = false;
			Component[] componentsInChildren = thing.transform.GetComponentsInChildren(typeof(ThingPart), true);
			foreach (ThingPart thingPart in componentsInChildren)
			{
				if (!thingPart.isText || flag)
				{
					Renderer component = thingPart.gameObject.GetComponent<MeshRenderer>();
					Material[] materials = component.materials;
					Material[] array2 = new Material[materials.Length - 1];
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j] = materials[j];
					}
					component.materials = array2;
				}
			}
		}
	}

	// Token: 0x060013BE RID: 5054 RVA: 0x000B1218 File Offset: 0x000AF618
	public GameObject GetIncludedSubThingTopMasterThingPart(Transform transformToCheck)
	{
		GameObject gameObject = null;
		Transform transform = transformToCheck;
		while (transform != null)
		{
			IncludedSubThingsWrapper component = transform.GetComponent<IncludedSubThingsWrapper>();
			if (component != null && component.masterThingPart)
			{
				gameObject = component.masterThingPart.gameObject;
			}
			transform = transform.parent;
		}
		return gameObject;
	}

	// Token: 0x060013BF RID: 5055 RVA: 0x000B1274 File Offset: 0x000AF674
	public GameObject GetIncludedSubThingDirectMasterThingPart(Transform transformToCheck)
	{
		GameObject gameObject = null;
		Transform transform = transformToCheck;
		while (transform != null)
		{
			IncludedSubThingsWrapper component = transform.GetComponent<IncludedSubThingsWrapper>();
			if (component != null && component.masterThingPart)
			{
				gameObject = component.masterThingPart.gameObject;
				break;
			}
			transform = transform.parent;
		}
		return gameObject;
	}

	// Token: 0x060013C0 RID: 5056 RVA: 0x000B12D2 File Offset: 0x000AF6D2
	public bool ThingPartBaseSupportsReflectionPart(ThingPartBase thingPartBase)
	{
		return this.thingPartBasesSupportingReflectionParts.Contains(thingPartBase);
	}

	// Token: 0x060013C1 RID: 5057 RVA: 0x000B12E0 File Offset: 0x000AF6E0
	public bool ThingPartBaseSupportsLimitedReflectionPart(ThingPartBase thingPartBase)
	{
		return this.thingPartBasesSupportingLimitedReflectionParts.Contains(thingPartBase);
	}

	// Token: 0x060013C2 RID: 5058 RVA: 0x000B12F0 File Offset: 0x000AF6F0
	public int FindAndReplaceInScripts(ThingPart thingPart, string find, string replace, bool forSingleState)
	{
		int num = 0;
		Thing component = thingPart.transform.parent.GetComponent<Thing>();
		int num2 = 0;
		int num3 = thingPart.states.Count - 1;
		if (forSingleState)
		{
			num2 = thingPart.currentState;
			num3 = thingPart.currentState;
		}
		string[] array = new string[] { find };
		for (int i = num2; i <= num3; i++)
		{
			ThingPartState thingPartState = thingPart.states[i];
			for (int j = 0; j < thingPartState.scriptLines.Count; j++)
			{
				string text = thingPartState.scriptLines[j];
				string[] array2 = text.Split(array, StringSplitOptions.None);
				int num4 = array2.Length - 1;
				if (num4 > 0)
				{
					thingPartState.scriptLines[j] = text.Replace(find, replace);
					num += num4;
					thingPartState.ParseScriptLinesIntoListeners(component, thingPart, false);
				}
			}
		}
		thingPart.SetStatePropertiesByTransform(false);
		return num;
	}

	// Token: 0x060013C3 RID: 5059 RVA: 0x000B13E4 File Offset: 0x000AF7E4
	public Component[] GetAllThings()
	{
		GameObject @object = Managers.treeManager.GetObject("/Universe/ThrownOrEmittedThings");
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Thing), true);
		Component[] componentsInChildren2 = @object.GetComponentsInChildren(typeof(Thing), true);
		Component[] componentsInChildren3 = Managers.personManager.ourPerson.GetComponentsInChildren(typeof(Thing), true);
		Component[] componentsInChildren4 = Managers.personManager.People.GetComponentsInChildren(typeof(Thing), true);
		Component[] array = new Component[componentsInChildren.Length + componentsInChildren2.Length + componentsInChildren3.Length + componentsInChildren4.Length];
		int num = 0;
		componentsInChildren.CopyTo(array, num);
		num += componentsInChildren.Length;
		componentsInChildren2.CopyTo(array, num);
		num += componentsInChildren2.Length;
		componentsInChildren3.CopyTo(array, num);
		num += componentsInChildren3.Length;
		componentsInChildren4.CopyTo(array, num);
		num += componentsInChildren4.Length;
		return array;
	}

	// Token: 0x060013C4 RID: 5060 RVA: 0x000B14CC File Offset: 0x000AF8CC
	public Component[] GetAllAttractors()
	{
		GameObject @object = Managers.treeManager.GetObject("/Universe/ThrownOrEmittedThings");
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(AttractThings), true);
		Component[] componentsInChildren2 = @object.GetComponentsInChildren(typeof(AttractThings), true);
		Component[] componentsInChildren3 = Managers.personManager.ourPerson.GetComponentsInChildren(typeof(AttractThings), true);
		Component[] componentsInChildren4 = Managers.personManager.People.GetComponentsInChildren(typeof(AttractThings), true);
		Component[] array = new Component[componentsInChildren.Length + componentsInChildren2.Length + componentsInChildren3.Length + componentsInChildren4.Length];
		int num = 0;
		componentsInChildren.CopyTo(array, num);
		num += componentsInChildren.Length;
		componentsInChildren2.CopyTo(array, num);
		num += componentsInChildren2.Length;
		componentsInChildren3.CopyTo(array, num);
		num += componentsInChildren3.Length;
		componentsInChildren4.CopyTo(array, num);
		num += componentsInChildren4.Length;
		return array;
	}

	// Token: 0x060013C5 RID: 5061 RVA: 0x000B15B4 File Offset: 0x000AF9B4
	public Component[] GetAllRigidbodies()
	{
		GameObject @object = Managers.treeManager.GetObject("/Universe/ThrownOrEmittedThings");
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Rigidbody), true);
		Component[] componentsInChildren2 = @object.GetComponentsInChildren(typeof(Rigidbody), true);
		Component[] componentsInChildren3 = Managers.personManager.ourPerson.GetComponentsInChildren(typeof(Rigidbody), true);
		Component[] componentsInChildren4 = Managers.personManager.People.GetComponentsInChildren(typeof(Rigidbody), true);
		Component[] array = new Component[componentsInChildren.Length + componentsInChildren2.Length + componentsInChildren3.Length + componentsInChildren4.Length];
		int num = 0;
		componentsInChildren.CopyTo(array, num);
		num += componentsInChildren.Length;
		componentsInChildren2.CopyTo(array, num);
		num += componentsInChildren2.Length;
		componentsInChildren3.CopyTo(array, num);
		num += componentsInChildren3.Length;
		componentsInChildren4.CopyTo(array, num);
		num += componentsInChildren4.Length;
		return array;
	}

	// Token: 0x060013C6 RID: 5062 RVA: 0x000B169C File Offset: 0x000AFA9C
	public void UpdateAllVisibilityAndCollision(bool contextLaserIsOn = false)
	{
		bool flag = Managers.areaManager != null && Managers.areaManager.weAreEditorOfCurrentArea;
		bool flag2 = flag && Our.seeInvisibleAsEditor;
		bool flag3 = (flag && Our.touchUncollidableAsEditor) || contextLaserIsOn || Our.mode == EditModes.Inventory;
		Component[] allThings = this.GetAllThings();
		foreach (Thing thing in allThings)
		{
			if (thing.containsInvisibleOrUncollidable || thing.subThingMasterPart != null)
			{
				thing.UpdateAllVisibilityAndCollision(flag2, flag3);
			}
		}
	}

	// Token: 0x060013C7 RID: 5063 RVA: 0x000B174E File Offset: 0x000AFB4E
	public static void SetLayerForThingAndParts(Thing thing, string layerName)
	{
		ThingManager.SetLayerForThingAndParts(thing, LayerMask.NameToLayer(layerName));
	}

	// Token: 0x060013C8 RID: 5064 RVA: 0x000B175C File Offset: 0x000AFB5C
	public static void SetLayerForThingAndParts(Thing thing, int layer)
	{
		thing.gameObject.layer = layer;
		Component[] componentsInChildren = thing.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			thingPart.gameObject.layer = layer;
		}
	}

	// Token: 0x060013C9 RID: 5065 RVA: 0x000B17AC File Offset: 0x000AFBAC
	public Thing ReCreatePlacementAfterPlacementAttributeChange(Thing thing)
	{
		if (thing == null)
		{
			return null;
		}
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(thing.gameObject, thing.transform.position, thing.transform.rotation, this.placements.transform);
		Thing thing2 = gameObject.GetComponent<Thing>();
		thing2 = thing.GetComponent<Thing>();
		thing2.placementId = thing.placementId;
		thing2.isLocked = thing.isLocked;
		thing2.isInvisibleToEditors = thing.isInvisibleToEditors;
		thing2.suppressScriptsAndStates = thing.suppressScriptsAndStates;
		thing2.suppressCollisions = thing.suppressCollisions;
		thing2.suppressLights = thing.suppressLights;
		thing2.suppressParticles = thing.suppressParticles;
		thing2.suppressHoldable = thing.suppressHoldable;
		thing2.suppressShowAtDistance = thing.suppressShowAtDistance;
		thing2.distanceToShow = thing.distanceToShow;
		Managers.thingManager.MakeDeepThingClone(thing.gameObject, gameObject, true, true, false);
		GameObject gameObject2 = thing.gameObject;
		string placementId = thing.placementId;
		thing = gameObject.GetComponent<Thing>();
		thing.placementId = placementId;
		thing.MemorizeOriginalTransform(true);
		thing.AutoUpdateAllVisibilityAndCollision();
		global::UnityEngine.Object.Destroy(gameObject2);
		Managers.soundManager.Play("success", null, 0.2f, false, false);
		return thing;
	}

	// Token: 0x060013CA RID: 5066 RVA: 0x000B18D8 File Offset: 0x000AFCD8
	public ThingPartBase? GetSubdividableGroup(ThingPartBase baseType)
	{
		ThingPartBase? thingPartBase = null;
		if (this.IsSubdividableCube(baseType))
		{
			thingPartBase = new ThingPartBase?(ThingPartBase.Cube);
		}
		else if (this.IsSubdividableQuad(baseType))
		{
			thingPartBase = new ThingPartBase?(ThingPartBase.Quad);
		}
		return thingPartBase;
	}

	// Token: 0x060013CB RID: 5067 RVA: 0x000B1920 File Offset: 0x000AFD20
	public void StartCreateThingViaJson(string json)
	{
		if (json[0] == '{' && CreationHelper.thingBeingEdited == null && (Our.mode == EditModes.Area || Our.mode == EditModes.None))
		{
			string text;
			if (!Managers.thingManager.GetPlacementsReachedLimit(out text, null))
			{
				Our.SetMode(EditModes.Thing, false);
				CreationHelper.thingBeingEdited = global::UnityEngine.Object.Instantiate<GameObject>(Managers.thingManager.thingGameObject);
				CreationHelper.thingBeingEdited.name = CreationHelper.thingDefaultName;
				Transform transform = CreationHelper.thingBeingEdited.transform;
				transform.parent = Managers.thingManager.placements.transform;
				transform.rotation = Quaternion.identity;
				transform.localScale = Vector3.one;
				GameObject @object = Managers.treeManager.GetObject("/OurPersonRig/HeadCore");
				transform.position = @object.transform.position + @object.transform.forward * 1f;
				try
				{
					JsonToThingConverter.SetThing(CreationHelper.thingBeingEdited, json, true, false, null, null);
					Managers.soundManager.Play("putDown", null, 1f, false, false);
					Managers.dialogManager.SwitchToNewDialog(DialogType.Create, null, string.Empty);
				}
				catch
				{
					Managers.soundManager.Play("no", null, 1f, false, false);
					Our.SetPreviousMode();
					global::UnityEngine.Object.Destroy(CreationHelper.thingBeingEdited);
					CreationHelper.thingBeingEdited = null;
					Managers.dialogManager.ShowInfo("Creation import cancelled as thing definition was invalid. See anyland.com/info/thing-format.html", false, true, -1, DialogType.Start, 1f, false, TextColor.Red, TextAlignment.Left);
				}
			}
			else
			{
				Managers.dialogManager.ShowInfo(text, false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
			}
		}
	}

	// Token: 0x060013CC RID: 5068 RVA: 0x000B1AD8 File Offset: 0x000AFED8
	public void UpdateShowThingPartDirectionArrows(Thing thing, bool doShow)
	{
		Component[] componentsInChildren = thing.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			DirectionArrows component = thingPart.GetComponent<DirectionArrows>();
			if (thingPart.showDirectionArrowsWhenEditing && doShow)
			{
				if (component == null)
				{
					thingPart.gameObject.AddComponent<DirectionArrows>();
				}
			}
			else if (component != null)
			{
				global::UnityEngine.Object.Destroy(component.arrows);
				global::UnityEngine.Object.Destroy(component);
			}
		}
	}

	// Token: 0x060013CD RID: 5069 RVA: 0x000B1B64 File Offset: 0x000AFF64
	public bool IsSubdividableCube(ThingPartBase baseType)
	{
		ThingPartBase[] array = new ThingPartBase[]
		{
			ThingPartBase.Cube,
			ThingPartBase.Cube3x2,
			ThingPartBase.Cube4x2,
			ThingPartBase.Cube5x2,
			ThingPartBase.Cube6x2,
			ThingPartBase.Cube2x3,
			ThingPartBase.Cube3x3,
			ThingPartBase.Cube4x3,
			ThingPartBase.Cube5x3,
			ThingPartBase.Cube6x3,
			ThingPartBase.Cube2x4,
			ThingPartBase.Cube3x4,
			ThingPartBase.Cube4x4,
			ThingPartBase.Cube5x4,
			ThingPartBase.Cube6x4,
			ThingPartBase.Cube2x5,
			ThingPartBase.Cube3x5,
			ThingPartBase.Cube4x5,
			ThingPartBase.Cube5x5,
			ThingPartBase.Cube6x5,
			ThingPartBase.Cube6x6,
			ThingPartBase.Cube5x6deprecated
		};
		return Array.IndexOf<ThingPartBase>(array, baseType) >= 0;
	}

	// Token: 0x060013CE RID: 5070 RVA: 0x000B1B94 File Offset: 0x000AFF94
	public bool IsSubdividableQuad(ThingPartBase baseType)
	{
		ThingPartBase[] array = new ThingPartBase[]
		{
			ThingPartBase.Quad,
			ThingPartBase.Quad3x2,
			ThingPartBase.Quad4x2,
			ThingPartBase.Quad5x2,
			ThingPartBase.Quad6x2,
			ThingPartBase.Quad2x3,
			ThingPartBase.Quad3x3,
			ThingPartBase.Quad4x3,
			ThingPartBase.Quad5x3,
			ThingPartBase.Quad6x3,
			ThingPartBase.Quad2x4,
			ThingPartBase.Quad3x4,
			ThingPartBase.Quad4x4,
			ThingPartBase.Quad5x4,
			ThingPartBase.Quad6x4,
			ThingPartBase.Quad2x5,
			ThingPartBase.Quad3x5,
			ThingPartBase.Quad4x5,
			ThingPartBase.Quad5x5,
			ThingPartBase.Quad6x5,
			ThingPartBase.Quad6x6
		};
		return Array.IndexOf<ThingPartBase>(array, baseType) >= 0;
	}

	// Token: 0x060013CF RID: 5071 RVA: 0x000B1BC4 File Offset: 0x000AFFC4
	private void InitSettings()
	{
		this.texturePropertyAbbreviations = new Dictionary<TextureProperty, string>
		{
			{
				TextureProperty.ScaleX,
				"x"
			},
			{
				TextureProperty.ScaleY,
				"y"
			},
			{
				TextureProperty.Strength,
				"a"
			},
			{
				TextureProperty.OffsetX,
				"m"
			},
			{
				TextureProperty.OffsetY,
				"n"
			},
			{
				TextureProperty.Rotation,
				"r"
			},
			{
				TextureProperty.Glow,
				"g"
			},
			{
				TextureProperty.Param1,
				"o"
			},
			{
				TextureProperty.Param2,
				"t"
			},
			{
				TextureProperty.Param3,
				"e"
			}
		};
		this.texturePropertyDefault = new Dictionary<TextureProperty, float>
		{
			{
				TextureProperty.ScaleX,
				0.5f
			},
			{
				TextureProperty.ScaleY,
				0.5f
			},
			{
				TextureProperty.OffsetX,
				0f
			},
			{
				TextureProperty.OffsetY,
				0f
			},
			{
				TextureProperty.Strength,
				0.5f
			},
			{
				TextureProperty.Rotation,
				0f
			},
			{
				TextureProperty.Glow,
				0f
			},
			{
				TextureProperty.Param1,
				0.5f
			},
			{
				TextureProperty.Param2,
				0.5f
			},
			{
				TextureProperty.Param3,
				0.5f
			}
		};
		this.textureTypeWithOnlyAlphaSetting = new Dictionary<TextureType, bool>
		{
			{
				TextureType.SideGlow,
				true
			},
			{
				TextureType.Wireframe,
				true
			},
			{
				TextureType.Outline,
				true
			}
		};
		this.algorithmTextureTypes = new Dictionary<TextureType, bool>
		{
			{
				TextureType.Gradient,
				true
			},
			{
				TextureType.PerlinNoise1,
				true
			},
			{
				TextureType.QuasiCrystal,
				true
			},
			{
				TextureType.VoronoiDots,
				true
			},
			{
				TextureType.VoronoiPolys,
				true
			},
			{
				TextureType.WavyLines,
				true
			},
			{
				TextureType.WoodGrain,
				true
			},
			{
				TextureType.PlasmaNoise,
				true
			},
			{
				TextureType.Pool,
				true
			},
			{
				TextureType.Bio,
				true
			},
			{
				TextureType.FractalNoise,
				true
			},
			{
				TextureType.LightSquares,
				true
			},
			{
				TextureType.Machine,
				true
			},
			{
				TextureType.SweptNoise,
				true
			},
			{
				TextureType.Abstract,
				true
			},
			{
				TextureType.Dashes,
				true
			},
			{
				TextureType.LayeredNoise,
				true
			},
			{
				TextureType.SquareRegress,
				true
			},
			{
				TextureType.Swirly,
				true
			}
		};
		this.textureAlphaCaps = new Dictionary<TextureType, float>
		{
			{
				TextureType.QuasiCrystal,
				0.5f
			},
			{
				TextureType.VoronoiDots,
				0.5f
			},
			{
				TextureType.VoronoiPolys,
				0.5f
			},
			{
				TextureType.WavyLines,
				0.5f
			},
			{
				TextureType.WoodGrain,
				0.5f
			},
			{
				TextureType.Machine,
				0.5f
			},
			{
				TextureType.Dashes,
				0.5f
			},
			{
				TextureType.SquareRegress,
				0.5f
			},
			{
				TextureType.Swirly,
				0.5f
			}
		};
		this.textureExtraParamsNumber = new Dictionary<TextureType, int>
		{
			{
				TextureType.Gradient,
				1
			},
			{
				TextureType.PerlinNoise1,
				1
			},
			{
				TextureType.QuasiCrystal,
				1
			},
			{
				TextureType.VoronoiDots,
				3
			},
			{
				TextureType.VoronoiPolys,
				3
			},
			{
				TextureType.WavyLines,
				2
			},
			{
				TextureType.WoodGrain,
				3
			},
			{
				TextureType.PlasmaNoise,
				3
			},
			{
				TextureType.Pool,
				3
			},
			{
				TextureType.Bio,
				2
			},
			{
				TextureType.FractalNoise,
				3
			},
			{
				TextureType.LightSquares,
				3
			},
			{
				TextureType.SweptNoise,
				3
			},
			{
				TextureType.Abstract,
				2
			},
			{
				TextureType.LayeredNoise,
				2
			},
			{
				TextureType.SquareRegress,
				3
			},
			{
				TextureType.Swirly,
				3
			}
		};
		this.particleSystemPropertyAbbreviations = new Dictionary<ParticleSystemProperty, string>
		{
			{
				ParticleSystemProperty.Amount,
				"m"
			},
			{
				ParticleSystemProperty.Alpha,
				"a"
			},
			{
				ParticleSystemProperty.Speed,
				"s"
			},
			{
				ParticleSystemProperty.Size,
				"z"
			},
			{
				ParticleSystemProperty.Gravity,
				"g"
			},
			{
				ParticleSystemProperty.Shape,
				"h"
			}
		};
		this.particleSystemPropertyDefault = new Dictionary<ParticleSystemProperty, float>
		{
			{
				ParticleSystemProperty.Amount,
				0.25f
			},
			{
				ParticleSystemProperty.Alpha,
				0.25f
			},
			{
				ParticleSystemProperty.Speed,
				0.15f
			},
			{
				ParticleSystemProperty.Size,
				0.25f
			},
			{
				ParticleSystemProperty.Gravity,
				0.5f
			},
			{
				ParticleSystemProperty.Shape,
				0.25f
			}
		};
		this.particleSystemTypeWithOnlyAlphaSetting = new Dictionary<ParticleSystemType, bool>
		{
			{
				ParticleSystemType.NoisyWater,
				true
			},
			{
				ParticleSystemType.GroundSmoke,
				true
			},
			{
				ParticleSystemType.Rain,
				true
			},
			{
				ParticleSystemType.Fog,
				true
			},
			{
				ParticleSystemType.TwistedSmoke,
				true
			},
			{
				ParticleSystemType.Embers,
				true
			},
			{
				ParticleSystemType.Beams,
				true
			},
			{
				ParticleSystemType.Rays,
				true
			},
			{
				ParticleSystemType.CircularSmoke,
				true
			},
			{
				ParticleSystemType.PopSmoke,
				true
			},
			{
				ParticleSystemType.WaterFlow,
				true
			},
			{
				ParticleSystemType.WaterFlowSoft,
				true
			},
			{
				ParticleSystemType.Sparks,
				true
			},
			{
				ParticleSystemType.Flame,
				true
			}
		};
		this.thingPartBasesSupportingReflectionParts = new ThingPartBase[]
		{
			ThingPartBase.Cube,
			ThingPartBase.Pyramid,
			ThingPartBase.LowPolySphere,
			ThingPartBase.Icosphere,
			ThingPartBase.Ramp,
			ThingPartBase.JitterCube,
			ThingPartBase.Cone,
			ThingPartBase.HalfSphere,
			ThingPartBase.Trapeze,
			ThingPartBase.Sphere,
			ThingPartBase.Cylinder,
			ThingPartBase.Spike,
			ThingPartBase.JitterSphere,
			ThingPartBase.LowPolyCylinder,
			ThingPartBase.ChamferCube,
			ThingPartBase.CubeBevel1,
			ThingPartBase.Ring1,
			ThingPartBase.Ring2,
			ThingPartBase.Ring3,
			ThingPartBase.Ring4,
			ThingPartBase.Ring5,
			ThingPartBase.Ring6,
			ThingPartBase.CubeBevel2,
			ThingPartBase.CubeBevel3,
			ThingPartBase.CubeRotated,
			ThingPartBase.CurvedRamp,
			ThingPartBase.Hexagon,
			ThingPartBase.HexagonBevel,
			ThingPartBase.Capsule,
			ThingPartBase.HalfCylinder,
			ThingPartBase.RoundCube,
			ThingPartBase.QuarterSphereRotated,
			ThingPartBase.Octagon,
			ThingPartBase.HighPolySphere,
			ThingPartBase.BowlCube,
			ThingPartBase.BowlCubeSoft,
			ThingPartBase.Wheel,
			ThingPartBase.WheelVariant,
			ThingPartBase.Wheel2,
			ThingPartBase.Wheel2Variant,
			ThingPartBase.Wheel3,
			ThingPartBase.Wheel4,
			ThingPartBase.Bowl1Soft,
			ThingPartBase.Bowl1,
			ThingPartBase.Bowl2,
			ThingPartBase.Bowl3,
			ThingPartBase.Bowl4,
			ThingPartBase.Bowl5,
			ThingPartBase.Bowl6,
			ThingPartBase.CubeHole,
			ThingPartBase.HalfCubeHole,
			ThingPartBase.LowJitterCube,
			ThingPartBase.LowJitterCubeSoft,
			ThingPartBase.JitterChamferCylinder,
			ThingPartBase.JitterChamferCylinderSoft,
			ThingPartBase.JitterHalfCone,
			ThingPartBase.JitterHalfConeSoft,
			ThingPartBase.JitterCone,
			ThingPartBase.JitterConeSoft,
			ThingPartBase.Gear,
			ThingPartBase.GearVariant,
			ThingPartBase.GearVariant2,
			ThingPartBase.GearSoft,
			ThingPartBase.GearVariantSoft,
			ThingPartBase.GearVariant2Soft,
			ThingPartBase.Rocky,
			ThingPartBase.RockySoft,
			ThingPartBase.RockyVerySoft,
			ThingPartBase.Spikes,
			ThingPartBase.SpikesSoft,
			ThingPartBase.SpikesVerySoft,
			ThingPartBase.HoleWall,
			ThingPartBase.JaggyWall,
			ThingPartBase.WavyWall,
			ThingPartBase.JitterCubeSoft,
			ThingPartBase.JitterSphereSoft
		};
		this.thingPartBasesSupportingLimitedReflectionParts = new ThingPartBase[]
		{
			ThingPartBase.Drop,
			ThingPartBase.Drop2,
			ThingPartBase.Drop3,
			ThingPartBase.DropSharp,
			ThingPartBase.DropSharp2,
			ThingPartBase.DropSharp3,
			ThingPartBase.Drop3Flat,
			ThingPartBase.DropSharp3Flat,
			ThingPartBase.DropCut,
			ThingPartBase.DropSharpCut,
			ThingPartBase.DropRing,
			ThingPartBase.DropRingFlat,
			ThingPartBase.DropPear,
			ThingPartBase.DropPear2,
			ThingPartBase.Drop3Jitter,
			ThingPartBase.SharpBent,
			ThingPartBase.Tetrahedron,
			ThingPartBase.Pipe,
			ThingPartBase.Pipe2,
			ThingPartBase.Pipe3,
			ThingPartBase.ShrinkDisk,
			ThingPartBase.ShrinkDisk2,
			ThingPartBase.DirectionIndicator,
			ThingPartBase.Quad,
			ThingPartBase.WavyWallVariant,
			ThingPartBase.WavyWallVariantSoft
		};
		this.smoothingAngles = new Dictionary<ThingPartBase, int>
		{
			{
				ThingPartBase.Bowl1,
				80
			},
			{
				ThingPartBase.Bowl2,
				80
			},
			{
				ThingPartBase.Bowl3,
				80
			},
			{
				ThingPartBase.Bowl4,
				80
			},
			{
				ThingPartBase.Bowl5,
				80
			},
			{
				ThingPartBase.Bowl6,
				80
			},
			{
				ThingPartBase.Bowl1Soft,
				140
			},
			{
				ThingPartBase.BowlCube,
				50
			},
			{
				ThingPartBase.BowlCubeSoft,
				80
			},
			{
				ThingPartBase.CubeHole,
				80
			},
			{
				ThingPartBase.Octagon,
				10
			},
			{
				ThingPartBase.HalfBowlSoft,
				140
			},
			{
				ThingPartBase.HalfCubeHole,
				80
			},
			{
				ThingPartBase.HalfCylinder,
				80
			},
			{
				ThingPartBase.Heptagon,
				10
			},
			{
				ThingPartBase.Pentagon,
				10
			},
			{
				ThingPartBase.QuarterBowlCube,
				50
			},
			{
				ThingPartBase.QuarterBowlCubeSoft,
				80
			},
			{
				ThingPartBase.QuarterBowlSoft,
				140
			},
			{
				ThingPartBase.QuarterCylinder,
				80
			},
			{
				ThingPartBase.QuarterPipe1,
				80
			},
			{
				ThingPartBase.QuarterPipe2,
				80
			},
			{
				ThingPartBase.QuarterPipe3,
				80
			},
			{
				ThingPartBase.QuarterPipe4,
				80
			},
			{
				ThingPartBase.QuarterPipe5,
				80
			},
			{
				ThingPartBase.QuarterPipe6,
				80
			},
			{
				ThingPartBase.QuarterSphere,
				60
			},
			{
				ThingPartBase.Ring1,
				180
			},
			{
				ThingPartBase.Ring2,
				180
			},
			{
				ThingPartBase.Ring3,
				180
			},
			{
				ThingPartBase.Ring4,
				180
			},
			{
				ThingPartBase.Ring5,
				180
			},
			{
				ThingPartBase.Ring6,
				180
			},
			{
				ThingPartBase.SphereEdge,
				80
			},
			{
				ThingPartBase.QuarterTorus1,
				180
			},
			{
				ThingPartBase.QuarterTorus2,
				180
			},
			{
				ThingPartBase.QuarterTorus3,
				180
			},
			{
				ThingPartBase.QuarterTorus4,
				180
			},
			{
				ThingPartBase.QuarterTorus5,
				180
			},
			{
				ThingPartBase.QuarterTorus6,
				180
			},
			{
				ThingPartBase.QuarterTorusRotated1,
				180
			},
			{
				ThingPartBase.QuarterTorusRotated2,
				180
			},
			{
				ThingPartBase.QuarterTorusRotated3,
				180
			},
			{
				ThingPartBase.QuarterTorusRotated4,
				180
			},
			{
				ThingPartBase.QuarterTorusRotated5,
				180
			},
			{
				ThingPartBase.QuarterTorusRotated6,
				180
			},
			{
				ThingPartBase.QuarterSphereRotated,
				80
			},
			{
				ThingPartBase.Branch,
				120
			},
			{
				ThingPartBase.FineSphere,
				140
			},
			{
				ThingPartBase.GearSoft,
				120
			},
			{
				ThingPartBase.GearVariantSoft,
				120
			},
			{
				ThingPartBase.GearVariant2Soft,
				60
			},
			{
				ThingPartBase.HalfSphere,
				60
			},
			{
				ThingPartBase.JitterChamferCylinderSoft,
				120
			},
			{
				ThingPartBase.JitterConeSoft,
				120
			},
			{
				ThingPartBase.JitterCubeSoft,
				120
			},
			{
				ThingPartBase.JitterHalfConeSoft,
				90
			},
			{
				ThingPartBase.JitterSphereSoft,
				120
			},
			{
				ThingPartBase.LowJitterCubeSoft,
				120
			},
			{
				ThingPartBase.Pipe,
				80
			},
			{
				ThingPartBase.Pipe2,
				80
			},
			{
				ThingPartBase.Pipe3,
				80
			},
			{
				ThingPartBase.Wheel,
				20
			},
			{
				ThingPartBase.Wheel2,
				20
			},
			{
				ThingPartBase.Wheel3,
				15
			},
			{
				ThingPartBase.Wheel4,
				60
			},
			{
				ThingPartBase.Bubbles,
				120
			},
			{
				ThingPartBase.HoleWall,
				60
			},
			{
				ThingPartBase.JaggyWall,
				10
			},
			{
				ThingPartBase.WavyWall,
				25
			},
			{
				ThingPartBase.WavyWallVariantSoft,
				30
			},
			{
				ThingPartBase.Spikes,
				15
			},
			{
				ThingPartBase.SpikesSoft,
				90
			},
			{
				ThingPartBase.SpikesVerySoft,
				160
			},
			{
				ThingPartBase.Rocky,
				15
			},
			{
				ThingPartBase.RockySoft,
				30
			},
			{
				ThingPartBase.RockyVerySoft,
				120
			},
			{
				ThingPartBase.Drop,
				60
			},
			{
				ThingPartBase.Drop2,
				80
			},
			{
				ThingPartBase.Drop3,
				80
			},
			{
				ThingPartBase.Drop3Jitter,
				35
			},
			{
				ThingPartBase.DropBent,
				80
			},
			{
				ThingPartBase.DropBent2,
				80
			},
			{
				ThingPartBase.DropCut,
				80
			},
			{
				ThingPartBase.DropPear,
				80
			},
			{
				ThingPartBase.DropPear2,
				80
			},
			{
				ThingPartBase.DropRing,
				80
			},
			{
				ThingPartBase.DropSharp,
				80
			},
			{
				ThingPartBase.DropSharp2,
				80
			},
			{
				ThingPartBase.DropSharp3,
				80
			},
			{
				ThingPartBase.DropSharpCut,
				80
			},
			{
				ThingPartBase.SharpBent,
				80
			},
			{
				ThingPartBase.ShrinkDisk,
				60
			},
			{
				ThingPartBase.ShrinkDisk2,
				60
			},
			{
				ThingPartBase.Sphere,
				60
			}
		};
	}

	// Token: 0x040011BE RID: 4542
	public const int currentThingVersion = 9;

	// Token: 0x040011BF RID: 4543
	public const int maxThingsInArea = 10000;

	// Token: 0x040011C0 RID: 4544
	public const float subThingsSmoothDelay = 0.05f;

	// Token: 0x040011C1 RID: 4545
	public GameObject thingGameObject;

	// Token: 0x040011C4 RID: 4548
	public PhysicMaterial bouncyMaterial;

	// Token: 0x040011C5 RID: 4549
	public PhysicMaterial slidyMaterial;

	// Token: 0x040011C6 RID: 4550
	public PhysicMaterial bouncySlidyMaterial;

	// Token: 0x040011C7 RID: 4551
	public PhysicMaterial controllableMaterial;

	// Token: 0x040011C8 RID: 4552
	public GameObject[] thingPartBases;

	// Token: 0x040011C9 RID: 4553
	public Dictionary<string, Material> sharedMaterials = new Dictionary<string, Material>();

	// Token: 0x040011CA RID: 4554
	public Dictionary<string, Material> sharedTextureMaterials = new Dictionary<string, Material>();

	// Token: 0x040011CB RID: 4555
	public Dictionary<string, Mesh> sharedMeshes = new Dictionary<string, Mesh>();

	// Token: 0x040011CD RID: 4557
	private ThingPartBase[] thingPartBasesSupportingReflectionParts;

	// Token: 0x040011CE RID: 4558
	private ThingPartBase[] thingPartBasesSupportingLimitedReflectionParts;

	// Token: 0x040011CF RID: 4559
	public ThingDefinitionCache thingDefinitionCache;

	// Token: 0x040011D0 RID: 4560
	public HeldThingsRegistrar heldThingsRegistrar;

	// Token: 0x040011D1 RID: 4561
	public int statsThingsInArea;

	// Token: 0x040011D2 RID: 4562
	public int statsThingsAroundPosition;

	// Token: 0x040011D3 RID: 4563
	public int statsThingPartsAroundPosition;

	// Token: 0x040011D4 RID: 4564
	public bool mergeThings = true;

	// Token: 0x040011D5 RID: 4565
	public const string steamScreenshotPrefix = "https://steamuserimages-a.akamaihd.net/ugc/";

	// Token: 0x040011D6 RID: 4566
	public const string steamScreenshotPrefixHttp = "http://steamuserimages-a.akamaihd.net/ugc/";

	// Token: 0x040011D7 RID: 4567
	public const string coverImagePrefix = "http://www.coverbrowser.com/image/";

	// Token: 0x040011D8 RID: 4568
	public const string coverCacheImagePrefix = "http://cache.coverbrowser.com/image/";

	// Token: 0x040011D9 RID: 4569
	public const int minAllowedPolygonCountForConvex = 3;

	// Token: 0x040011DA RID: 4570
	public const int maxAllowedPolygonCountForConvex = 255;

	// Token: 0x040011DB RID: 4571
	private int vertexTextureCounter;

	// Token: 0x040011DC RID: 4572
	private ThingPartAttribute[] partAttributesWhichCanBeMerged;

	// Token: 0x040011DD RID: 4573
	public Dictionary<TextureProperty, string> texturePropertyAbbreviations;

	// Token: 0x040011DE RID: 4574
	private Dictionary<TextureProperty, float> texturePropertyDefault;

	// Token: 0x040011DF RID: 4575
	private Dictionary<TextureType, bool> textureTypeWithOnlyAlphaSetting;

	// Token: 0x040011E0 RID: 4576
	private Dictionary<TextureType, bool> algorithmTextureTypes;

	// Token: 0x040011E1 RID: 4577
	private Dictionary<TextureType, int> textureExtraParamsNumber;

	// Token: 0x040011E2 RID: 4578
	public Dictionary<TextureType, float> textureAlphaCaps;

	// Token: 0x040011E3 RID: 4579
	public Dictionary<ParticleSystemProperty, string> particleSystemPropertyAbbreviations;

	// Token: 0x040011E4 RID: 4580
	private Dictionary<ParticleSystemProperty, float> particleSystemPropertyDefault;

	// Token: 0x040011E5 RID: 4581
	private Dictionary<ParticleSystemType, bool> particleSystemTypeWithOnlyAlphaSetting;

	// Token: 0x040011E7 RID: 4583
	private Material outlineHighlightMaterial;

	// Token: 0x040011E8 RID: 4584
	private Material innerGlowHighlightMaterial;

	// Token: 0x040011E9 RID: 4585
	public Shader shader_standard;

	// Token: 0x040011EA RID: 4586
	public Shader shader_customGlow;

	// Token: 0x040011EB RID: 4587
	public Shader shader_customUnshaded;

	// Token: 0x040011EC RID: 4588
	public Shader shader_customInversion;

	// Token: 0x040011ED RID: 4589
	public Shader shader_customBrightness;

	// Token: 0x040011EE RID: 4590
	public Shader shader_customTransparentGlow;

	// Token: 0x040011EF RID: 4591
	public Shader shader_textLit;

	// Token: 0x040011F0 RID: 4592
	public Shader shader_textEmissive;
}
