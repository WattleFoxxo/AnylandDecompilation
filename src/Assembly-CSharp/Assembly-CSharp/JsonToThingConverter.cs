using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200025D RID: 605
public static class JsonToThingConverter
{
	// Token: 0x06001660 RID: 5728 RVA: 0x000C76A0 File Offset: 0x000C5AA0
	public static void SetThing(GameObject thisThingGameObject, string json, bool alwaysKeepThingPartsSeparate = false, bool isForPlacement = false, Vector3? initialPosition = null, Vector3? initialRotation = null)
	{
		if (string.IsNullOrEmpty(json))
		{
			Log.Warning("SetThingFromJsonString got null json");
			return;
		}
		JSONNode jsonnode = JSON.Parse(json);
		if (jsonnode["p"].Count == 0)
		{
			Log.Warning("SetThingFromJsonString got 0 parts json");
			return;
		}
		string text = ((!(jsonnode["n"] != null) || string.IsNullOrEmpty(jsonnode["n"].Value)) ? CreationHelper.thingDefaultName : jsonnode["n"].Value);
		thisThingGameObject.name = text;
		Thing component = thisThingGameObject.GetComponent<Thing>();
		component.givenName = text;
		component.version = ((!(jsonnode["v"] != null)) ? 1 : jsonnode["v"].AsInt);
		component.description = ((!(jsonnode["d"] != null)) ? null : jsonnode["d"]);
		JsonToThingConverter.ExpandThingAttributeFromJson(component, jsonnode["a"]);
		JsonToThingConverter.ExpandThingIncludedNameIdsFromJson(component, jsonnode["inc"]);
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		component.containsInvisibleOrUncollidable = component.invisible || component.uncollidable || component.suppressCollisions;
		if (jsonnode["tp_m"] != null)
		{
			component.mass = new float?(jsonnode["tp_m"]);
		}
		if (jsonnode["tp_d"] != null)
		{
			component.drag = new float?(jsonnode["tp_d"]);
		}
		if (jsonnode["tp_ad"] != null)
		{
			component.angularDrag = new float?(jsonnode["tp_ad"]);
		}
		if (jsonnode["tp_lp"] != null)
		{
			component.lockPhysicsPosition = JsonHelper.GetBoolVector3(jsonnode["tp_lp"]);
		}
		if (jsonnode["tp_lr"] != null)
		{
			component.lockPhysicsRotation = JsonHelper.GetBoolVector3(jsonnode["tp_lr"]);
		}
		component.thingPartCount = jsonnode["p"].Count;
		for (int i = 0; i < component.thingPartCount; i++)
		{
			JSONNode jsonnode2 = jsonnode["p"][i];
			int num = ((!(jsonnode2["b"] != null)) ? 1 : jsonnode2["b"].AsInt);
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(Managers.thingManager.thingPartBases[num], Vector3.zero, Quaternion.identity);
			gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
			gameObject.tag = "ThingPart";
			gameObject.transform.parent = thisThingGameObject.transform;
			ThingPart component2 = gameObject.GetComponent<ThingPart>();
			component2.thingVersion = component.version;
			component2.indexWithinThing = i;
			component2.isInInventoryOrDialog = component.isInInventoryOrDialog;
			component2.isInInventory = component.isInInventory;
			component2.activeEvenInInventory = component.activeEvenInInventory;
			component2.isGiftInDialog = component.isGiftInDialog;
			if (jsonnode2["id"])
			{
				component2.guid = jsonnode2["id"];
			}
			JsonToThingConverter.ExpandThingPartAttributeFromJson(component2, jsonnode2["a"]);
			if (component.stricterPhysicsSyncing)
			{
				component2.parentPersistsPhysics = true;
			}
			if (jsonnode2["n"] != null && jsonnode2["n"].Value != string.Empty)
			{
				component2.givenName = jsonnode2["n"].Value;
			}
			if (jsonnode2["m_bs"] != null)
			{
				component2.controllableBodySlidiness = jsonnode2["m_bs"].AsFloat;
			}
			if (jsonnode2["m_bb"] != null)
			{
				component2.controllableBodyBounciness = jsonnode2["m_bb"].AsFloat;
			}
			component2.isControllableWheel = jsonnode2["m_w"] != null;
			if (jsonnode2["j"] != null)
			{
				component2.joystickToControllablePart = new JoystickToControllablePart();
				component2.joystickToControllablePart.SetFromJson(jsonnode2["j"]);
			}
			if (jsonnode2["vid_a"] != null)
			{
				component2.videoIdToPlayAtAreaStart = jsonnode2["vid_a"];
			}
			if (jsonnode2["vid_p"] != null)
			{
				component2.videoIdToPlayWhenPressed = jsonnode2["vid_p"];
			}
			if (jsonnode2["vid_v"] != null)
			{
				component2.videoAutoPlayVolume = jsonnode2["vid_v"].AsFloat;
			}
			if (jsonnode2["ac"] != null)
			{
				component2.autoContinuation = new ThingPartAutoContinuation();
				component2.autoContinuation.SetByJson(jsonnode2["ac"]);
			}
			if (component2.isText && jsonnode2["e"] != null)
			{
				string value = jsonnode2["e"].Value;
				if (jsonnode2["lh"] != null)
				{
					component2.textLineHeight = jsonnode2["lh"].AsFloat;
				}
				component2.SetOriginalText(value);
			}
			if (jsonnode2["t"] != null)
			{
				component2.materialType = (MaterialTypes)jsonnode2["t"].AsInt;
				if (component2.materialType == MaterialTypes.InvisibleWhenDone_Deprecated)
				{
					component2.materialType = MaterialTypes.None;
					component2.invisible = true;
				}
				switch (component2.materialType)
				{
				case MaterialTypes.PointLight:
				case MaterialTypes.SpotLight:
					if (component.suppressLights)
					{
						component2.materialType = MaterialTypes.Glow;
					}
					else
					{
						component.containsLight = true;
					}
					break;
				case MaterialTypes.Particles:
				case MaterialTypes.ParticlesBig:
					if (component.suppressParticles)
					{
						component2.materialType = MaterialTypes.None;
					}
					else
					{
						component.containsBaseLayerParticleSystem = true;
					}
					break;
				}
			}
			else if (component2.isText && component.version <= 3)
			{
				component2.materialType = MaterialTypes.Glow;
			}
			JsonToThingConverter.ApplyVertexChangesAndSmoothingAngle(component2, jsonnode2, alwaysKeepThingPartsSeparate, jsonnode["p"]);
			if (jsonnode2["i"] != null && jsonnode2["i"].Count >= 1)
			{
				component2.includedSubThings = new List<IncludedSubThing>();
				for (int j = 0; j < jsonnode2["i"].Count; j++)
				{
					JSONNode jsonnode3 = jsonnode2["i"][j];
					IncludedSubThing includedSubThing = new IncludedSubThing();
					includedSubThing.thingId = jsonnode3["t"];
					includedSubThing.originalRelativePosition = JsonHelper.GetVector3(jsonnode3["p"]);
					includedSubThing.originalRelativeRotation = JsonHelper.GetVector3(jsonnode3["r"]);
					if (jsonnode3["n"] != null)
					{
						includedSubThing.nameOverride = jsonnode3["n"];
					}
					JsonToThingConverter.ExpandIncludedSubThingInvertAttributeFromJson(includedSubThing, jsonnode3["a"]);
					component2.includedSubThings.Add(includedSubThing);
				}
			}
			if (jsonnode2["su"] != null && jsonnode2["su"].Count >= 1)
			{
				component.containsPlacedSubThings = true;
				component.requiresWiderReach = true;
				for (int k = 0; k < jsonnode2["su"].Count; k++)
				{
					JSONNode jsonnode4 = jsonnode2["su"][k];
					if (jsonnode4["i"] != null)
					{
						string text2 = jsonnode4["t"];
						Vector3 vector = JsonHelper.GetVector3(jsonnode4["p"]);
						Vector3 vector2 = JsonHelper.GetVector3(jsonnode4["r"]);
						component2.AddConfirmedNonExistingPlacedSubThingId(jsonnode4["i"], text2, vector, vector2);
					}
				}
			}
			if (jsonnode2["im"] != null)
			{
				Managers.areaManager.containsThingPartWithImage = true;
				component2.imageUrl = jsonnode2["im"];
				if (jsonnode2["imt"] != null)
				{
					component2.imageType = (ImageType)jsonnode2["imt"].AsInt;
				}
				component.allPartsImageCount++;
			}
			bool flag4 = jsonnode2["t1"] == null && jsonnode2["t2"] != null;
			if (!Managers.optimizationManager.hideTextures)
			{
				if (jsonnode2["t1"] != null)
				{
					if (component2.textureTypes == null)
					{
						component2.textureTypes = new TextureType[2];
					}
					component2.textureTypes[0] = (TextureType)jsonnode2["t1"].AsInt;
				}
				if (jsonnode2["t2"] != null)
				{
					if (component2.textureTypes == null)
					{
						component2.textureTypes = new TextureType[2];
					}
					int num2 = ((!flag4) ? 1 : 0);
					component2.textureTypes[num2] = (TextureType)jsonnode2["t2"].AsInt;
				}
			}
			if (jsonnode2["pr"] != null && !component.suppressParticles)
			{
				component2.particleSystemType = (ParticleSystemType)jsonnode2["pr"].AsInt;
				if (component2.particleSystemType != ParticleSystemType.None)
				{
					component.containsParticleSystem = true;
				}
			}
			bool flag5 = false;
			int num3 = ((!component.suppressScriptsAndStates) ? jsonnode2["s"].Count : 1);
			for (int l = 0; l < num3; l++)
			{
				if (l >= 1)
				{
					component2.states.Add(new ThingPartState());
				}
				JSONNode jsonnode5 = jsonnode2["s"][l];
				component2.states[l].position = new Vector3(jsonnode5["p"][0].AsFloat, jsonnode5["p"][1].AsFloat, jsonnode5["p"][2].AsFloat);
				component2.states[l].rotation = new Vector3(jsonnode5["r"][0].AsFloat, jsonnode5["r"][1].AsFloat, jsonnode5["r"][2].AsFloat);
				component2.states[l].scale = new Vector3(jsonnode5["s"][0].AsFloat, jsonnode5["s"][1].AsFloat, jsonnode5["s"][2].AsFloat);
				component2.states[l].color = JsonHelper.GetColor(jsonnode5["c"]);
				component2.states[l].name = component2.givenName;
				if (jsonnode5["b"] != null && !component.suppressScriptsAndStates)
				{
					component.containsBehaviorScript = true;
					flag5 = true;
					for (int m = 0; m < jsonnode5["b"].Count; m++)
					{
						component2.states[l].scriptLines.Add(jsonnode5["b"][m].Value);
					}
					component2.states[l].ParseScriptLinesIntoListeners(component, component2, false);
					if (!component.containsOnAnyListener)
					{
						component.containsOnAnyListener = component2.states[l].ContainsOnAnyListener();
					}
					if (!component2.isGrabbable)
					{
						foreach (StateListener stateListener in component2.states[l].listeners)
						{
							if (stateListener.eventType == StateListener.EventType.OnGrabbed)
							{
								component2.isGrabbable = true;
								break;
							}
							if (stateListener.eventType == StateListener.EventType.OnTriggered && component.isHoldable)
							{
								component.remainsHeld = true;
							}
							else if (stateListener.eventType == StateListener.EventType.OnHearsAnywhere)
							{
								component.requiresWiderReach = true;
							}
						}
					}
					if (!flag3)
					{
						flag3 = JsonToThingConverter.GetContainsCollisionListener(component2.states[l].listeners);
					}
					if (!component2.containsBehaviorScriptVariables)
					{
						component2.containsBehaviorScriptVariables = JsonToThingConverter.GetContainsBehaviorScriptVariables(component2.states[l].listeners);
						if (!component.containsBehaviorScriptVariables && component2.containsBehaviorScriptVariables)
						{
							component.containsBehaviorScriptVariables = true;
						}
					}
					if (!component2.containsTextCommands)
					{
						component2.containsTextCommands = JsonToThingConverter.GetContainsTextCommands(component2.states[l].listeners);
					}
					if (!component.requiresWiderReach)
					{
						component.requiresWiderReach = JsonToThingConverter.GetContainsAttractRepelOrLoopSurroundCommands(component2.states[l].listeners);
					}
					if (!component2.containsTurnCommands)
					{
						component2.containsTurnCommands = JsonToThingConverter.GetContainsTurnCommands(component2.states[l].listeners);
						if (component2.containsTurnCommands)
						{
							component.containsInvisibleOrUncollidable = true;
						}
					}
				}
				if (!component.requiresWiderReach)
				{
					component.requiresWiderReach = component2.videoScreenHasSurroundSound || component2.useTextureAsSky || !string.IsNullOrEmpty(component2.videoIdToPlayAtAreaStart);
				}
				if (!Managers.optimizationManager.hideTextures)
				{
					if (jsonnode5["t1"] != null)
					{
						JsonToThingConverter.SetTextureFromStateProperties(component2, component2.states[l], jsonnode5["t1"], 0);
					}
					if (jsonnode5["t2"] != null)
					{
						int num4 = ((!flag4) ? 1 : 0);
						JsonToThingConverter.SetTextureFromStateProperties(component2, component2.states[l], jsonnode5["t2"], num4);
					}
				}
				if (jsonnode5["pr"] != null)
				{
					JsonToThingConverter.SetParticleSystemFromStateProperties(component2.states[l], jsonnode5["pr"]);
				}
			}
			if (component2.isLiquid)
			{
				flag = true;
			}
			if (!flag2)
			{
				flag2 = component2.isVideoButton || component2.isSlideshowButton || component2.isCameraButton;
			}
			Renderer component3 = component2.GetComponent<Renderer>();
			if (jsonnode2["pr"] != null)
			{
				component2.UpdateParticleSystem();
			}
			if ((jsonnode2["t1"] != null || jsonnode2["t2"] != null) && !Managers.optimizationManager.hideTextures)
			{
				component2.UpdateTextures(false);
			}
			if (flag5 || component2.HasControllableSettings() || (flag3 && !component.doAlwaysMergeParts) || (!isForPlacement && (component.isHoldable || component.remainsHeld)) || (alwaysKeepThingPartsSeparate || component2.materialType == MaterialTypes.SpotLight || component2.materialType == MaterialTypes.PointLight || component2.materialType == MaterialTypes.Particles || component2.materialType == MaterialTypes.ParticlesBig || JsonToThingConverter.ContainsPartAttributesWhichShouldPreventMerging(jsonnode2["a"]) || component2.isLiquid || component2.isText || !string.IsNullOrEmpty(component2.guid) || component2.includedSubThings != null || component2.useTextureAsSky || component2.imageUrl != string.Empty || component2.isImagePasteScreen || flag2 || component2.HasVertexTexture() || (component2.avoidCastShadow && !component.avoidCastShadow)) || (component2.avoidReceiveShadow && !component.avoidReceiveShadow) || (component2.textureTypes[0] != TextureType.None && component2.states.Count >= 2) || (component2.particleSystemType != ParticleSystemType.None && !component.mergeParticleSystems && !component.doAlwaysMergeParts))
			{
				component2.material = component3.material;
			}
			else
			{
				string addSharedMaterialsId = JsonToThingConverter.GetAddSharedMaterialsId(component2, component3);
				component3.sharedMaterial = Managers.thingManager.sharedMaterials[addSharedMaterialsId];
				component2.material = component3.sharedMaterial;
				component2.name = "MergableThingPart";
			}
			if (component2.avoidCastShadow || component.avoidCastShadow)
			{
				component3.shadowCastingMode = ShadowCastingMode.Off;
			}
			if (component2.avoidReceiveShadow || component.avoidReceiveShadow)
			{
				component3.receiveShadows = false;
			}
			JsonToThingConverter.ConvertDeprecatedHideEffectShapes(component, component2);
			if (component2.invisible || component2.uncollidable)
			{
				component.containsInvisibleOrUncollidable = true;
			}
			component2.SetTransformPropertiesByState(false, false);
		}
		component.UpdateIsBigIndicators();
		bool flag6 = (!flag3 && !component.isVeryBig && !flag2 && (isForPlacement || (!component.isHoldable && !component.remainsHeld)) && !flag && component.thingPartCount > 1) || component.suppressScriptsAndStates;
		if (Managers.thingManager.mergeThings && !alwaysKeepThingPartsSeparate && (flag6 || component.doAlwaysMergeParts))
		{
			JsonToThingConverter.AddStaticReflectionAndContinuationParts(component);
			JsonToThingConverter.MergeMeshesToOptimize(thisThingGameObject, component, component.keepPreciseCollider);
		}
		if (component.isPassable)
		{
			ThingManager.SetLayerForThingAndParts(component, "PassableObjects");
		}
		else if (component.invisibleToDesktopCamera)
		{
			ThingManager.SetLayerForThingAndParts(component, "InvisibleToDesktopCamera");
		}
		if (component.movableByEveryone)
		{
			component.gameObject.AddComponent<ThingMovableByEveryone>();
		}
		component.UpdateAllVisibilityAndCollision(alwaysKeepThingPartsSeparate, alwaysKeepThingPartsSeparate);
		if (initialPosition != null)
		{
			component.transform.localPosition = initialPosition.Value;
		}
		if (initialRotation != null)
		{
			component.transform.localEulerAngles = initialRotation.Value;
		}
	}

	// Token: 0x06001661 RID: 5729 RVA: 0x000C8A58 File Offset: 0x000C6E58
	private static void ApplyVertexChangesAndSmoothingAngle(ThingPart thingPart, JSONNode partNode, bool isForEditing, JSONNode thingNode)
	{
		if (partNode["c"] != null || partNode["v"] != null || partNode["sa"] != null)
		{
			MeshFilter component = thingPart.GetComponent<MeshFilter>();
			if (component == null)
			{
				return;
			}
			Mesh mesh = component.mesh;
			if (partNode["v"] != null)
			{
				int asInt = partNode["v"].AsInt;
				partNode["c"] = thingNode[asInt]["c"];
				if (thingNode[asInt]["sa"] != null)
				{
					partNode["sa"] = thingNode[asInt]["sa"];
				}
				if (thingNode[asInt]["cx"] != null)
				{
					partNode["cx"] = thingNode[asInt]["cx"];
				}
			}
			if (partNode["c"] != null)
			{
				Vector3[] vertices = mesh.vertices;
				int num = mesh.vertices.Length - 1;
				if (isForEditing)
				{
					thingPart.changedVertices = new Dictionary<int, Vector3>();
				}
				int count = partNode["c"].Count;
				for (int i = 0; i < count; i++)
				{
					JSONNode jsonnode = partNode["c"][i];
					Vector3 vector = new Vector3(jsonnode[0].AsFloat, jsonnode[1].AsFloat, jsonnode[2].AsFloat);
					int count2 = jsonnode.Count;
					int num2 = 0;
					for (int j = 3; j < count2; j++)
					{
						int asInt2 = jsonnode[j].AsInt;
						int num3 = num2 + asInt2;
						if (num3 <= num)
						{
							vertices[num3] = vector;
							num2 = num3;
							if (isForEditing)
							{
								thingPart.changedVertices[num3] = vector;
							}
						}
					}
				}
				mesh.vertices = vertices;
			}
			int num4 = 0;
			if (partNode["sa"] != null)
			{
				thingPart.smoothingAngle = new int?(partNode["sa"].AsInt);
				int? smoothingAngle = thingPart.smoothingAngle;
				num4 = smoothingAngle.Value;
			}
			else
			{
				int num5 = 0;
				if (Managers.thingManager.smoothingAngles.TryGetValue(thingPart.baseType, out num5))
				{
					num4 = num5;
				}
			}
			if (partNode["cx"] != null)
			{
				thingPart.convex = new bool?(partNode["cx"].AsInt == 1);
			}
			mesh.RecalculateNormals((float)num4);
			bool flag = mesh.vertices.Length <= 500;
			if (flag)
			{
				JsonToThingConverter.RecreateColliderAndChangeTypeIfNeeded(thingPart, mesh);
			}
			mesh.RecalculateTangents();
		}
		else if (partNode["cx"] != null)
		{
			thingPart.convex = new bool?(partNode["cx"].AsInt == 1);
			MeshCollider component2 = thingPart.GetComponent<MeshCollider>();
			if (component2 != null)
			{
				MeshCollider meshCollider = component2;
				bool? convex = thingPart.convex;
				meshCollider.convex = convex.Value;
			}
		}
	}

	// Token: 0x06001662 RID: 5730 RVA: 0x000C8DC4 File Offset: 0x000C71C4
	public static void RecreateColliderAndChangeTypeIfNeeded(ThingPart thingPart, Mesh mesh)
	{
		bool flag = true;
		MeshCollider meshCollider = thingPart.GetComponent<MeshCollider>();
		if (meshCollider != null)
		{
			flag = meshCollider.convex;
		}
		global::UnityEngine.Object.Destroy(thingPart.GetComponent<Collider>());
		meshCollider = thingPart.gameObject.AddComponent<MeshCollider>();
		bool? convex = thingPart.convex;
		if (convex != null)
		{
			bool? convex2 = thingPart.convex;
			flag = convex2.Value;
		}
		if (flag)
		{
			MeshFilter component = meshCollider.GetComponent<MeshFilter>();
			if (component != null)
			{
				int num = component.mesh.triangles.Length / 3;
				if (num > 255)
				{
					meshCollider.inflateMesh = true;
				}
			}
		}
		meshCollider.convex = flag;
		mesh.RecalculateBounds();
	}

	// Token: 0x06001663 RID: 5731 RVA: 0x000C8E74 File Offset: 0x000C7274
	private static void ConvertDeprecatedHideEffectShapes(Thing thing, ThingPart thingPart)
	{
		if (thing.hideEffectShapes_deprecated)
		{
			bool flag = thingPart.materialType == MaterialTypes.Particles || thingPart.materialType == MaterialTypes.ParticlesBig || thingPart.materialType == MaterialTypes.PointLight || thingPart.materialType == MaterialTypes.SpotLight || thingPart.particleSystemType != ParticleSystemType.None || thingPart.useTextureAsSky;
			if (flag)
			{
				thingPart.invisible = true;
				thingPart.uncollidable = true;
			}
		}
	}

	// Token: 0x06001664 RID: 5732 RVA: 0x000C8EE8 File Offset: 0x000C72E8
	private static void AddStaticReflectionAndContinuationParts(Thing thing)
	{
		Component[] componentsInChildren = thing.gameObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			thingPart.CreateMyReflectionPartsIfNeeded(thingPart.name);
			thingPart.HandleMyReflectionParts();
			thingPart.CreateMyAutoContinuationPartsIfNeeded(thingPart.name);
			thingPart.HandleMyAutoContinuationParts();
		}
	}

	// Token: 0x06001665 RID: 5733 RVA: 0x000C8F44 File Offset: 0x000C7344
	private static TextureType GetLastTextureType()
	{
		TextureType textureType = TextureType.None;
		IEnumerator enumerator = Enum.GetValues(typeof(TextureType)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				TextureType textureType2 = (TextureType)obj;
				textureType = textureType2;
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
		return textureType;
	}

	// Token: 0x06001666 RID: 5734 RVA: 0x000C8FB0 File Offset: 0x000C73B0
	private static void SetTextureFromStateProperties(ThingPart thingPart, ThingPartState state, JSONNode propertyNode, int index)
	{
		state.textureColors[index] = JsonHelper.GetColor(propertyNode["c"]);
		state.textureProperties[index] = new Dictionary<TextureProperty, float>();
		Managers.thingManager.SetTexturePropertiesToDefault(state.textureProperties[index], thingPart.textureTypes[index]);
		foreach (KeyValuePair<TextureProperty, string> keyValuePair in Managers.thingManager.texturePropertyAbbreviations)
		{
			if (propertyNode[keyValuePair.Value] != null)
			{
				state.textureProperties[index][keyValuePair.Key] = propertyNode[keyValuePair.Value].AsFloat;
			}
		}
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x000C9090 File Offset: 0x000C7490
	private static void SetParticleSystemFromStateProperties(ThingPartState state, JSONNode propertyNode)
	{
		state.particleSystemColor = JsonHelper.GetColor(propertyNode["c"]);
		state.particleSystemProperty = new Dictionary<ParticleSystemProperty, float>();
		foreach (KeyValuePair<ParticleSystemProperty, string> keyValuePair in Managers.thingManager.particleSystemPropertyAbbreviations)
		{
			if (propertyNode[keyValuePair.Value] != null)
			{
				state.particleSystemProperty[keyValuePair.Key] = propertyNode[keyValuePair.Value].AsFloat;
			}
		}
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x000C9148 File Offset: 0x000C7548
	private static bool ContainsPartAttributesWhichShouldPreventMerging(JSONNode attributes)
	{
		bool flag = false;
		if (attributes != null)
		{
			for (int i = 0; i < attributes.Count; i++)
			{
				ThingPartAttribute asInt = (ThingPartAttribute)attributes[i].AsInt;
				if (Array.IndexOf<ThingPartAttribute>(JsonToThingConverter.partAttributesWhichCanBeMerged, asInt) == -1)
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x000C91A0 File Offset: 0x000C75A0
	private static bool GetContainsCollisionListener(List<StateListener> listeners)
	{
		bool flag = false;
		foreach (StateListener stateListener in listeners)
		{
			StateListener.EventType eventType = stateListener.eventType;
			if (eventType == StateListener.EventType.OnTouches || eventType == StateListener.EventType.OnTouchEnds || eventType == StateListener.EventType.OnConsumed || eventType == StateListener.EventType.OnBlownAt || eventType == StateListener.EventType.OnHitting)
			{
				flag = true;
			}
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x0600166A RID: 5738 RVA: 0x000C9238 File Offset: 0x000C7638
	private static bool GetContainsBehaviorScriptVariables(List<StateListener> listeners)
	{
		bool flag = false;
		foreach (StateListener stateListener in listeners)
		{
			flag = stateListener.eventType == StateListener.EventType.OnVariableChange || stateListener.variableOperations != null || stateListener.whenIsData != null;
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x0600166B RID: 5739 RVA: 0x000C92C0 File Offset: 0x000C76C0
	private static bool GetContainsTextCommands(List<StateListener> listeners)
	{
		bool flag = false;
		foreach (StateListener stateListener in listeners)
		{
			flag = stateListener.setText != null;
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x0600166C RID: 5740 RVA: 0x000C932C File Offset: 0x000C772C
	private static bool GetContainsAttractRepelOrLoopSurroundCommands(List<StateListener> listeners)
	{
		bool flag = false;
		foreach (StateListener stateListener in listeners)
		{
			flag = stateListener.attractThingsSettings != null || (stateListener.startLoopSoundName != null && stateListener.loopSpatialBlend < 1f);
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x0600166D RID: 5741 RVA: 0x000C93B4 File Offset: 0x000C77B4
	private static bool GetContainsTurnCommands(List<StateListener> listeners)
	{
		bool flag = false;
		foreach (StateListener stateListener in listeners)
		{
			flag = stateListener.turn != null || stateListener.turnThing != null || stateListener.turnSubThing != null;
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x0600166E RID: 5742 RVA: 0x000C9438 File Offset: 0x000C7838
	private static string GetAddSharedMaterialsId(ThingPart thingPart, Renderer thingPartRenderer)
	{
		string text = string.Empty;
		if (thingPart.materialType != MaterialTypes.None)
		{
			text = text + thingPart.materialType + "_";
		}
		if (thingPart.particleSystemType != ParticleSystemType.None)
		{
			text = text + thingPart.particleSystemType + "_p";
		}
		text = text + JsonToThingConverter.GetNormalizedColorString(thingPart.states[0].color) + "_";
		if (thingPart.textureTypes[0] != TextureType.None)
		{
			ThingPartState thingPartState = thingPart.states[0];
			for (int i = 0; i < thingPartState.textureProperties.Length; i++)
			{
				if (thingPartState.textureProperties[i] != null)
				{
					text = text + JsonToThingConverter.GetNormalizedColorString(thingPartState.textureColors[i]) + "_";
					foreach (KeyValuePair<TextureProperty, float> keyValuePair in thingPartState.textureProperties[i])
					{
						string text2 = Managers.thingManager.texturePropertyAbbreviations[keyValuePair.Key] + keyValuePair.Value.ToString();
						text2 = text2.Replace("0.", ".");
						text = text + text2 + "_";
					}
				}
			}
		}
		if (!Managers.thingManager.sharedMaterials.ContainsKey(text))
		{
			Material material = new Material(thingPartRenderer.material);
			material.color = thingPart.states[0].color;
			material.name = text;
			Managers.thingManager.sharedMaterials.Add(text, material);
		}
		return text;
	}

	// Token: 0x0600166F RID: 5743 RVA: 0x000C9608 File Offset: 0x000C7A08
	public static string GetNormalizedColorString(Color thisColor)
	{
		return string.Concat(new string[]
		{
			JsonToThingConverter.GetNormalizedColorStringPart(thisColor.r),
			"_",
			JsonToThingConverter.GetNormalizedColorStringPart(thisColor.g),
			"_",
			JsonToThingConverter.GetNormalizedColorStringPart(thisColor.b)
		});
	}

	// Token: 0x06001670 RID: 5744 RVA: 0x000C9660 File Offset: 0x000C7A60
	private static string GetNormalizedColorStringPart(float number)
	{
		return ((int)Mathf.Round(Mathf.Clamp(number * 255f, 0f, 255f))).ToString();
	}

	// Token: 0x06001671 RID: 5745 RVA: 0x000C9698 File Offset: 0x000C7A98
	private static string GetAddSharedMeshesId(Thing thing, string materialId, CombineInstance[] combine, int combineVertexCount)
	{
		string text = thing.thingId + "_" + ((!thing.suppressScriptsAndStates) ? string.Empty : "s_") + materialId;
		if (!Managers.thingManager.sharedMeshes.ContainsKey(text))
		{
			Mesh mesh = new Mesh();
			if (combineVertexCount > 64535)
			{
				mesh.indexFormat = IndexFormat.UInt32;
			}
			mesh.CombineMeshes(combine, true, true);
			Managers.thingManager.sharedMeshes.Add(text, mesh);
		}
		return text;
	}

	// Token: 0x06001672 RID: 5746 RVA: 0x000C971C File Offset: 0x000C7B1C
	private static void MergeMeshesToOptimize(GameObject thing, Thing thingScript, bool keepPreciseCollider)
	{
		Vector3 position = thing.transform.position;
		Vector3 localScale = thing.transform.localScale;
		Quaternion rotation = thing.transform.rotation;
		thing.transform.position = Vector3.zero;
		thing.transform.localScale = Vector3.one;
		thing.transform.rotation = Quaternion.identity;
		bool flag = true;
		while (flag)
		{
			flag = false;
			string text = string.Empty;
			TextureType[] array = new TextureType[2];
			Color[] array2 = new Color[]
			{
				Color.white,
				Color.white
			};
			Dictionary<TextureProperty, float>[] array3 = new Dictionary<TextureProperty, float>[2];
			int num = 0;
			IEnumerator enumerator = thing.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.name == "MergableThingPart" && transform.CompareTag("ThingPart"))
					{
						Renderer component = transform.GetComponent<Renderer>();
						if (Managers.thingManager.sharedMaterials.ContainsKey(component.sharedMaterial.name) && (!flag || component.sharedMaterial.name == text))
						{
							if (!flag)
							{
								flag = true;
								text = component.sharedMaterial.name;
								ThingPart component2 = transform.GetComponent<ThingPart>();
								if (component2 != null && component2.textureTypes[0] != TextureType.None)
								{
									ThingPartState thingPartState = component2.states[0];
									for (int i = 0; i < thingPartState.textureProperties.Length; i++)
									{
										array[i] = component2.textureTypes[i];
										array2[i] = new Color(thingPartState.textureColors[i].r, thingPartState.textureColors[i].g, thingPartState.textureColors[i].b);
										array3[i] = Managers.thingManager.CloneTextureProperty(thingPartState.textureProperties[i]);
									}
								}
							}
							num++;
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
			if (flag)
			{
				MaterialTypes materialTypes = MaterialTypes.None;
				Color color = Color.white;
				string text2 = thingScript.thingId + "_" + ((!thingScript.suppressScriptsAndStates) ? string.Empty : "s_") + text;
				bool flag2 = text != string.Empty && Managers.thingManager.sharedMeshes.ContainsKey(text2);
				CombineInstance[] array4 = new CombineInstance[num];
				int num2 = 0;
				int num3 = 0;
				IEnumerator enumerator2 = thing.transform.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						Transform transform2 = (Transform)obj2;
						if (transform2.name == "MergableThingPart" && transform2.CompareTag("ThingPart") && transform2.name != Universe.objectNameIfAlreadyDestroyed)
						{
							Renderer component3 = transform2.GetComponent<Renderer>();
							if (component3.sharedMaterial.name == text)
							{
								ThingPart component4 = transform2.GetComponent<ThingPart>();
								if (component4 != null)
								{
									materialTypes = component4.materialType;
									color = component4.states[0].color;
								}
								MeshFilter component5 = transform2.GetComponent<MeshFilter>();
								if (!flag2)
								{
									array4[num3].mesh = component5.sharedMesh;
									num2 += component5.sharedMesh.vertexCount;
									array4[num3].transform = component5.transform.localToWorldMatrix;
								}
								global::UnityEngine.Object.Destroy(component5);
								Misc.Destroy(transform2.gameObject);
								num3++;
							}
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = enumerator2 as IDisposable) != null)
					{
						disposable2.Dispose();
					}
				}
				GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(Managers.thingManager.thingPartBases[2]);
				gameObject.transform.parent = thing.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.position = Vector3.zero;
				gameObject.transform.rotation = Quaternion.identity;
				gameObject.tag = "ThingPart";
				MeshFilter component6 = gameObject.GetComponent<MeshFilter>();
				string addSharedMeshesId = JsonToThingConverter.GetAddSharedMeshesId(thingScript, text, array4, num2);
				component6.sharedMesh = Managers.thingManager.sharedMeshes[addSharedMeshesId];
				gameObject.name = "Merge_" + addSharedMeshesId + "_" + text;
				MeshCollider component7 = gameObject.GetComponent<MeshCollider>();
				component7.convex = false;
				component7.sharedMesh = Managers.thingManager.sharedMeshes[addSharedMeshesId];
				MeshFilter component8 = component7.GetComponent<MeshFilter>();
				int num4 = 0;
				if (component8 != null)
				{
					num4 = component8.mesh.triangles.Length / 3;
				}
				if (num4 <= 254 && !keepPreciseCollider)
				{
					component7.convex = true;
				}
				Renderer component9 = gameObject.GetComponent<Renderer>();
				ThingPart component10 = gameObject.GetComponent<ThingPart>();
				component10.isInInventoryOrDialog = thingScript.isInInventoryOrDialog;
				component9.sharedMaterial = Managers.thingManager.sharedMaterials[text];
				if (thingScript.avoidCastShadow)
				{
					component9.shadowCastingMode = ShadowCastingMode.Off;
				}
				if (thingScript.avoidReceiveShadow)
				{
					component9.receiveShadows = false;
				}
				component10.material = component9.sharedMaterial;
				component10.materialType = materialTypes;
				component10.states[0].color = color;
				if (array[0] != TextureType.None)
				{
					component10.textureTypes = array;
					ThingPartState thingPartState2 = component10.states[0];
					for (int j = 0; j < thingPartState2.textureProperties.Length; j++)
					{
						thingPartState2.textureColors[j] = array2[j];
						thingPartState2.textureProperties[j] = array3[j];
					}
					component10.UpdateTextures(true);
				}
				component10.UpdateParticleSystem();
			}
		}
		thing.transform.position = position;
		thing.transform.localScale = localScale;
		thing.transform.rotation = rotation;
	}

	// Token: 0x06001673 RID: 5747 RVA: 0x000C9D98 File Offset: 0x000C8198
	private static void ExpandThingIncludedNameIdsFromJson(Thing thing, JSONNode jsonNameIds)
	{
		if (jsonNameIds != null)
		{
			for (int i = 0; i < jsonNameIds.Count; i++)
			{
				thing.includedNameIds.Add(jsonNameIds[i][0], jsonNameIds[i][1]);
			}
		}
	}

	// Token: 0x06001674 RID: 5748 RVA: 0x000C9DF8 File Offset: 0x000C81F8
	private static void ExpandThingAttributeFromJson(Thing thing, JSONNode jsonAttributes)
	{
		if (jsonAttributes != null)
		{
			for (int i = 0; i < jsonAttributes.Count; i++)
			{
				switch (jsonAttributes[i].AsInt)
				{
				case 1:
					thing.isClonable = true;
					break;
				case 2:
					thing.isHoldable = true;
					break;
				case 3:
					thing.remainsHeld = true;
					break;
				case 4:
					thing.isClimbable = true;
					break;
				case 5:
					thing.isUnwalkable = true;
					break;
				case 6:
					thing.isPassable = true;
					break;
				case 7:
					thing.doSnapAngles = true;
					break;
				case 8:
					thing.hideEffectShapes_deprecated = true;
					break;
				case 9:
					thing.isBouncy = true;
					break;
				case 12:
					thing.doShowDirection = true;
					break;
				case 13:
					thing.keepPreciseCollider = true;
					break;
				case 14:
					thing.doesFloat = true;
					break;
				case 15:
					thing.doesShatter = true;
					break;
				case 16:
					thing.isSticky = true;
					break;
				case 17:
					thing.isSlidy = true;
					break;
				case 18:
					thing.doSnapPosition = true;
					break;
				case 19:
					thing.amplifySpeech = true;
					break;
				case 20:
					thing.benefitsFromShowingAtDistance = true;
					break;
				case 21:
					thing.scaleAllParts = true;
					break;
				case 22:
					thing.doSoftSnapAngles = true;
					break;
				case 23:
					thing.doAlwaysMergeParts = true;
					break;
				case 24:
					thing.addBodyWhenAttached = true;
					break;
				case 25:
					thing.hasSurroundSound = true;
					break;
				case 26:
					thing.canGetEventsWhenStateChanging = true;
					break;
				case 27:
					thing.replacesHandsWhenAttached = true;
					break;
				case 28:
					thing.mergeParticleSystems = true;
					break;
				case 29:
					thing.isSittable = true;
					break;
				case 30:
					thing.smallEditMovements = true;
					break;
				case 31:
					thing.scaleEachPartUniformly = true;
					break;
				case 32:
					thing.snapAllPartsToGrid = true;
					break;
				case 33:
					thing.invisibleToUsWhenAttached = true;
					break;
				case 34:
					thing.replaceInstancesInArea = true;
					break;
				case 35:
					thing.addBodyWhenAttachedNonClearing = true;
					break;
				case 36:
					thing.avoidCastShadow = true;
					break;
				case 37:
					thing.avoidReceiveShadow = true;
					break;
				case 38:
					thing.omitAutoSounds = true;
					break;
				case 39:
					thing.omitAutoHapticFeedback = true;
					break;
				case 40:
					thing.keepSizeInInventory = true;
					break;
				case 41:
					thing.autoAddReflectionPartsSideways = true;
					break;
				case 42:
					thing.autoAddReflectionPartsVertical = true;
					break;
				case 43:
					thing.autoAddReflectionPartsDepth = true;
					break;
				case 44:
					thing.activeEvenInInventory = true;
					break;
				case 45:
					thing.stricterPhysicsSyncing = true;
					break;
				case 46:
					thing.removeOriginalWhenGrabbed = true;
					break;
				case 47:
					thing.persistWhenThrownOrEmitted = true;
					break;
				case 48:
					thing.invisible = true;
					break;
				case 49:
					thing.uncollidable = true;
					break;
				case 50:
					thing.movableByEveryone = true;
					break;
				case 51:
					thing.isNeverClonable = true;
					break;
				case 52:
					thing.floatsOnLiquid = true;
					break;
				case 53:
					thing.invisibleToDesktopCamera = true;
					break;
				case 54:
					thing.personalExperience = true;
					break;
				}
			}
			if (thing.suppressHoldable)
			{
				thing.isHoldable = false;
				thing.remainsHeld = false;
			}
		}
	}

	// Token: 0x06001675 RID: 5749 RVA: 0x000CA1A4 File Offset: 0x000C85A4
	private static void ExpandIncludedSubThingInvertAttributeFromJson(IncludedSubThing subThing, JSONNode jsonAttributes)
	{
		if (jsonAttributes != null)
		{
			for (int i = 0; i < jsonAttributes.Count; i++)
			{
				ThingAttribute asInt = (ThingAttribute)jsonAttributes[i].AsInt;
				if (asInt != ThingAttribute.isHoldable)
				{
					if (asInt != ThingAttribute.invisible)
					{
						if (asInt == ThingAttribute.uncollidable)
						{
							subThing.invert_uncollidable = true;
						}
					}
					else
					{
						subThing.invert_invisible = true;
					}
				}
				else
				{
					subThing.invert_isHoldable = true;
				}
			}
		}
	}

	// Token: 0x06001676 RID: 5750 RVA: 0x000CA224 File Offset: 0x000C8624
	private static void ExpandThingPartAttributeFromJson(ThingPart thingPart, JSONNode jsonAttributes)
	{
		if (jsonAttributes != null)
		{
			for (int i = 0; i < jsonAttributes.Count; i++)
			{
				switch (jsonAttributes[i].AsInt)
				{
				case 1:
					thingPart.offersScreen = true;
					break;
				case 2:
					thingPart.isVideoButton = true;
					break;
				case 3:
					thingPart.scalesUniformly = true;
					break;
				case 4:
					thingPart.videoScreenHasSurroundSound = true;
					break;
				case 5:
					thingPart.isLiquid = true;
					break;
				case 6:
					thingPart.offersSlideshowScreen = true;
					break;
				case 7:
					thingPart.isSlideshowButton = true;
					break;
				case 8:
					thingPart.isCamera = true;
					break;
				case 9:
					thingPart.isCameraButton = true;
					break;
				case 10:
					thingPart.isFishEyeCamera = true;
					break;
				case 11:
					thingPart.useUnsoftenedAnimations = true;
					break;
				case 12:
					thingPart.invisible = true;
					break;
				case 13:
					thingPart.isUnremovableCenter = true;
					break;
				case 14:
					thingPart.omitAutoSounds = true;
					break;
				case 15:
					thingPart.doSnapTextureAngles = true;
					break;
				case 16:
					thingPart.textureScalesUniformly = true;
					break;
				case 17:
					thingPart.avoidCastShadow = true;
					break;
				case 18:
					thingPart.looselyCoupledParticles = true;
					break;
				case 19:
					thingPart.textAlignCenter = true;
					break;
				case 20:
					thingPart.textAlignRight = true;
					break;
				case 21:
					thingPart.isAngleLocker = true;
					break;
				case 22:
					thingPart.isPositionLocker = true;
					break;
				case 23:
					thingPart.isLocked = true;
					break;
				case 24:
					thingPart.avoidReceiveShadow = true;
					break;
				case 25:
					thingPart.isImagePasteScreen = true;
					break;
				case 26:
					thingPart.allowBlackImageBackgrounds = true;
					break;
				case 27:
					thingPart.videoScreenLoops = true;
					break;
				case 28:
					thingPart.videoScreenIsDirectlyOnMesh = true;
					break;
				case 29:
					thingPart.useTextureAsSky = true;
					break;
				case 30:
					thingPart.stretchSkydomeSeam = true;
					break;
				case 31:
					thingPart.subThingsFollowDelayed = true;
					break;
				case 32:
					thingPart.hasReflectionPartSideways = true;
					break;
				case 33:
					thingPart.hasReflectionPartVertical = true;
					break;
				case 34:
					thingPart.hasReflectionPartDepth = true;
					break;
				case 35:
					thingPart.videoScreenFlipsX = true;
					break;
				case 36:
					thingPart.persistStates = true;
					break;
				case 37:
					thingPart.uncollidable = true;
					break;
				case 38:
					thingPart.isDedicatedCollider = true;
					break;
				case 39:
					thingPart.personalExperience = true;
					break;
				case 40:
					thingPart.invisibleToUsWhenAttached = true;
					break;
				case 41:
					thingPart.lightOmitsShadow = true;
					break;
				case 42:
					thingPart.showDirectionArrowsWhenEditing = true;
					break;
				}
			}
		}
	}

	// Token: 0x0400142A RID: 5162
	private static ThingPartAttribute[] partAttributesWhichCanBeMerged = new ThingPartAttribute[]
	{
		ThingPartAttribute.scalesUniformly,
		ThingPartAttribute.useUnsoftenedAnimations,
		ThingPartAttribute.omitAutoSounds,
		ThingPartAttribute.doSnapTextureAngles,
		ThingPartAttribute.textureScalesUniformly,
		ThingPartAttribute.isAngleLocker,
		ThingPartAttribute.isPositionLocker,
		ThingPartAttribute.isLocked,
		ThingPartAttribute.hasReflectionPartSideways,
		ThingPartAttribute.hasReflectionPartVertical,
		ThingPartAttribute.hasReflectionPartDepth
	};

	// Token: 0x0400142B RID: 5163
	private const string mergableName = "MergableThingPart";
}
