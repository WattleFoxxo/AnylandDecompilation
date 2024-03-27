using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200027C RID: 636
public static class ThingToJsonConverter
{
	// Token: 0x06001820 RID: 6176 RVA: 0x000DD348 File Offset: 0x000DB748
	public static string GetJson(GameObject thisThingGameObject, ref int vertexCount, ref int changedVerticesIndicesCount)
	{
		string text = string.Empty;
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		changedVerticesIndicesCount = 0;
		Thing component = thisThingGameObject.GetComponent<Thing>();
		text += "{";
		if (!string.IsNullOrEmpty(component.givenName) && component.givenName != CreationHelper.thingDefaultName)
		{
			text = text + "\"n\":" + JsonHelper.GetJson(component.givenName) + ",";
		}
		if (component.version > 9)
		{
			component.version = 9;
		}
		string text2 = text;
		text = string.Concat(new object[] { text2, "\"v\":", component.version, "," });
		if (!string.IsNullOrEmpty(component.description))
		{
			text = text + "\"d\":" + JsonHelper.GetJson(component.description) + ",";
		}
		string attributesAsIntList = ThingToJsonConverter.GetAttributesAsIntList(component);
		if (attributesAsIntList != string.Empty)
		{
			text = text + "\"a\":[" + attributesAsIntList + "],";
		}
		if (component.includedNameIds.Count >= 1)
		{
			text = text + "\"inc\":[" + JsonHelper.GetStringDictionaryAsArray(component.includedNameIds) + "],";
		}
		if (component.addBodyWhenAttached || component.addBodyWhenAttachedNonClearing)
		{
			text = text + "\"bod\":" + ThingToJsonConverter.GetOurCurrentBodyAttachmentsAsJson(false) + ",";
		}
		text += ThingToJsonConverter.GetThingPhysicsJson(component);
		ThingToJsonConverter.RemoveUnneededThingPartGuids(component);
		text += "\"p\":[";
		int num = 0;
		IEnumerator enumerator = thisThingGameObject.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.CompareTag("ThingPart"))
				{
					ThingPart component2 = transform.gameObject.GetComponent<ThingPart>();
					if (num >= 1)
					{
						text += ",";
					}
					text += "{";
					if (component2.baseType != ThingPartBase.Cube)
					{
						text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"\"b\":",
							(int)component2.baseType,
							","
						});
					}
					if (component2.materialType != MaterialTypes.None)
					{
						text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"\"t\":",
							(int)component2.materialType,
							","
						});
					}
					string thingPartAttributeAsIntList = ThingToJsonConverter.GetThingPartAttributeAsIntList(component2);
					if (thingPartAttributeAsIntList != string.Empty)
					{
						text = text + "\"a\":[" + thingPartAttributeAsIntList + "],";
					}
					if (!string.IsNullOrEmpty(component2.guid))
					{
						text = text + "\"id\":" + JsonHelper.GetJson(component2.guid) + ",";
					}
					if (!string.IsNullOrEmpty(component2.givenName))
					{
						text = text + "\"n\":" + JsonHelper.GetJson(component2.givenName) + ",";
					}
					if (component2.isText)
					{
						string text3 = component2.GetComponent<TextMesh>().text;
						text = text + "\"e\":" + JsonHelper.GetJson(text3) + ",";
						if (component2.textLineHeight != 1f)
						{
							text2 = text;
							text = string.Concat(new object[] { text2, "\"lh\":", component2.textLineHeight, "," });
						}
					}
					text += ThingToJsonConverter.GetIncludedSubThingsJson(component, component2);
					text += ThingToJsonConverter.GetPlacedSubThingsJson(component2);
					text += ThingToJsonConverter.GetControllableJson(component2);
					if (component2.imageUrl != string.Empty)
					{
						text = text + "\"im\":" + JsonHelper.GetJson(component2.imageUrl) + ",";
						if (component2.imageType == ImageType.Png)
						{
							text2 = text;
							text = string.Concat(new object[]
							{
								text2,
								"\"imt\":",
								(int)component2.imageType,
								","
							});
						}
					}
					if (component2.particleSystemType != ParticleSystemType.None)
					{
						text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"\"pr\":",
							(int)component2.particleSystemType,
							","
						});
					}
					if (component2.textureTypes[0] != TextureType.None)
					{
						text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"\"t1\":",
							(int)component2.textureTypes[0],
							","
						});
					}
					if (component2.textureTypes[1] != TextureType.None)
					{
						text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"\"t2\":",
							(int)component2.textureTypes[1],
							","
						});
					}
					if (component2.autoContinuation != null)
					{
						string json = component2.autoContinuation.GetJson();
						if (json != string.Empty)
						{
							text = text + json + ",";
						}
					}
					if (component2.changedVertices != null)
					{
						int num2 = 0;
						string changedVerticesJson = ThingToJsonConverter.GetChangedVerticesJson(component2, dictionary, num, out num2);
						changedVerticesIndicesCount += num2;
						if (changedVerticesJson != string.Empty)
						{
							text = text + changedVerticesJson + ",";
						}
					}
					int? smoothingAngle = component2.smoothingAngle;
					if (smoothingAngle != null)
					{
						int num3 = 0;
						Managers.thingManager.smoothingAngles.TryGetValue(component2.baseType, out num3);
						int? smoothingAngle2 = component2.smoothingAngle;
						if (smoothingAngle2.Value != num3)
						{
							string text4 = text;
							string text5 = "\"sa\":";
							int? smoothingAngle3 = component2.smoothingAngle;
							text = text4 + text5 + smoothingAngle3.Value.ToString() + ",";
						}
					}
					bool? convex = component2.convex;
					if (convex != null)
					{
						string text6 = text;
						string text7 = "\"cx\":";
						bool? convex2 = component2.convex;
						text = text6 + text7 + ((!convex2.Value) ? "0" : "1") + ",";
					}
					text += "\"s\":[";
					text += ThingToJsonConverter.GetStateJson(transform, component2);
					text += "]";
					text += "}";
					MeshFilter component3 = component2.gameObject.GetComponent<MeshFilter>();
					if (component3 != null)
					{
						vertexCount += component3.sharedMesh.vertexCount;
					}
					else
					{
						vertexCount += 50;
					}
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
		text += "]";
		text += "}";
		text = text.Replace("\r\n", "\n");
		text = text.Replace("\r", "\n");
		text = text.Replace("\n", "\\n");
		return text;
	}

	// Token: 0x06001821 RID: 6177 RVA: 0x000DDA44 File Offset: 0x000DBE44
	private static void RemoveUnneededThingPartGuids(Thing thing)
	{
		Component[] componentsInChildren = thing.gameObject.GetComponentsInChildren(typeof(ThingPart));
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (!string.IsNullOrEmpty(thingPart.guid) && !ThingToJsonConverter.ThingPartGuidIsReferenced(thing, thingPart.guid, thingPart))
			{
				thingPart.guid = null;
			}
		}
	}

	// Token: 0x06001822 RID: 6178 RVA: 0x000DDAB0 File Offset: 0x000DBEB0
	private static bool ThingPartGuidIsReferenced(Thing thing, string guid, ThingPart thingPartToIgnore)
	{
		bool flag = false;
		Component[] componentsInChildren = thing.gameObject.GetComponentsInChildren(typeof(ThingPart));
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart != thingPartToIgnore && thingPart.autoContinuation != null && thingPart.autoContinuation.fromPartGuid == guid)
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x000DDB30 File Offset: 0x000DBF30
	private static string GetIncludedSubThingsJson(Thing thing, ThingPart thingPart)
	{
		string text = string.Empty;
		if (thingPart.includedSubThings != null)
		{
			foreach (IncludedSubThing includedSubThing in thingPart.includedSubThings)
			{
				if (text != string.Empty)
				{
					text += ",";
				}
				text += "{";
				text = text + "\"t\":" + JsonHelper.GetJson(includedSubThing.thingId) + ",";
				text = text + "\"p\":" + JsonHelper.GetJson(includedSubThing.originalRelativePosition) + ",";
				text = text + "\"r\":" + JsonHelper.GetJson(includedSubThing.originalRelativeRotation);
				if (includedSubThing.nameOverride != null && includedSubThing.nameOverride != thing.givenName)
				{
					text = text + ",\"n\":" + JsonHelper.GetJson(includedSubThing.nameOverride);
				}
				string subThingAttributeInvertsAsIntList = ThingToJsonConverter.GetSubThingAttributeInvertsAsIntList(includedSubThing);
				if (subThingAttributeInvertsAsIntList != string.Empty)
				{
					text = text + ",\"a\":[" + subThingAttributeInvertsAsIntList + "]";
				}
				text += "}";
			}
			if (text != string.Empty)
			{
				text = "\"i\":[" + text + "],";
			}
		}
		return text;
	}

	// Token: 0x06001824 RID: 6180 RVA: 0x000DDCA8 File Offset: 0x000DC0A8
	private static string GetPlacedSubThingsJson(ThingPart thingPart)
	{
		string text = string.Empty;
		foreach (KeyValuePair<string, ThingIdPositionRotation> keyValuePair in thingPart.placedSubThingIdsWithOriginalInfo)
		{
			string key = keyValuePair.Key;
			ThingIdPositionRotation value = keyValuePair.Value;
			if (text != string.Empty)
			{
				text += ",";
			}
			text += "{";
			text = text + "\"i\":" + JsonHelper.GetJson(key) + ",";
			text = text + "\"t\":" + JsonHelper.GetJson(value.thingId) + ",";
			text = text + "\"p\":" + JsonHelper.GetJson(value.position) + ",";
			text = text + "\"r\":" + JsonHelper.GetJson(value.rotation);
			text += "}";
		}
		if (text != string.Empty)
		{
			text = "\"su\":[" + text + "],";
		}
		return text;
	}

	// Token: 0x06001825 RID: 6181 RVA: 0x000DDDD4 File Offset: 0x000DC1D4
	private static string GetControllableJson(ThingPart thingPart)
	{
		string text = string.Empty;
		if (thingPart.controllableBodySlidiness != 0f)
		{
			string text2 = text;
			text = string.Concat(new object[] { text2, "\"m_bs\": ", thingPart.controllableBodySlidiness, "," });
		}
		if (thingPart.controllableBodyBounciness != 0f)
		{
			string text2 = text;
			text = string.Concat(new object[] { text2, "\"m_bb\": ", thingPart.controllableBodyBounciness, "," });
		}
		if (thingPart.isControllableWheel)
		{
			text += "\"m_w\": true,";
		}
		if (thingPart.joystickToControllablePart != null && !thingPart.joystickToControllablePart.IsAllDefault())
		{
			text = text + thingPart.joystickToControllablePart.GetJson() + ",";
		}
		return text;
	}

	// Token: 0x06001826 RID: 6182 RVA: 0x000DDEB4 File Offset: 0x000DC2B4
	private static string GetThingPhysicsJson(Thing thing)
	{
		string text = string.Empty;
		float? mass = thing.mass;
		if (mass != null)
		{
			string text2 = text;
			object[] array = new object[4];
			array[0] = text2;
			array[1] = "\"tp_m\": ";
			int num = 2;
			float? mass2 = thing.mass;
			array[num] = mass2.Value;
			array[3] = ",";
			text = string.Concat(array);
		}
		float? drag = thing.drag;
		if (drag != null)
		{
			string text2 = text;
			object[] array2 = new object[4];
			array2[0] = text2;
			array2[1] = "\"tp_d\": ";
			int num2 = 2;
			float? drag2 = thing.drag;
			array2[num2] = drag2.Value;
			array2[3] = ",";
			text = string.Concat(array2);
		}
		float? angularDrag = thing.angularDrag;
		if (angularDrag != null)
		{
			string text2 = text;
			object[] array3 = new object[4];
			array3[0] = text2;
			array3[1] = "\"tp_ad\": ";
			int num3 = 2;
			float? angularDrag2 = thing.angularDrag;
			array3[num3] = angularDrag2.Value;
			array3[3] = ",";
			text = string.Concat(array3);
		}
		if (!thing.lockPhysicsPosition.IsAllDefault())
		{
			text += text + "\"tp_lp\": " + JsonHelper.GetJson(thing.lockPhysicsPosition) + ",";
		}
		if (!thing.lockPhysicsRotation.IsAllDefault())
		{
			text += text + "\"tp_lr\": " + JsonHelper.GetJson(thing.lockPhysicsRotation) + ",";
		}
		return text;
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x000DE018 File Offset: 0x000DC418
	private static string GetOurCurrentBodyAttachmentsAsJson(bool headOnly = false)
	{
		string text = string.Empty;
		text += ThingToJsonConverter.GetOurCurrentBodyAttachmentAsJsonForPart(text, "HeadCore/HeadAttachmentPoint", "h", true, false);
		if (!headOnly)
		{
			text += ThingToJsonConverter.GetOurCurrentBodyAttachmentAsJsonForPart(text, "HeadCore/HeadTopAttachmentPoint", "ht", false, false);
			text += ThingToJsonConverter.GetOurCurrentBodyAttachmentAsJsonForPart(text, "HandCoreLeft/ArmLeftAttachmentPoint", "al", false, false);
			text += ThingToJsonConverter.GetOurCurrentBodyAttachmentAsJsonForPart(text, "HandCoreRight/ArmRightAttachmentPoint", "ar", false, false);
			text += ThingToJsonConverter.GetOurCurrentBodyAttachmentAsJsonForPart(text, "Torso/UpperTorsoAttachmentPoint", "ut", false, false);
			text += ThingToJsonConverter.GetOurCurrentBodyAttachmentAsJsonForPart(text, "Torso/LowerTorsoAttachmentPoint", "lt", false, false);
			text += ThingToJsonConverter.GetOurCurrentBodyAttachmentAsJsonForPart(text, "Torso/LegLeftAttachmentPoint", "ll", false, true);
			text += ThingToJsonConverter.GetOurCurrentBodyAttachmentAsJsonForPart(text, "Torso/LegRightAttachmentPoint", "lr", false, true);
		}
		return "{" + text + "}";
	}

	// Token: 0x06001828 RID: 6184 RVA: 0x000DE10C File Offset: 0x000DC50C
	private static string GetOurCurrentBodyAttachmentAsJsonForPart(string currentJson, string treePath, string shortName, bool onlyIfCloneOfCurrentHead = false, bool includingAttachmentPointPositions = false)
	{
		string text = string.Empty;
		treePath = "/OurPersonRig/" + treePath;
		Transform transform = Managers.treeManager.GetTransform(treePath);
		GameObject childWithTag = Misc.GetChildWithTag(transform, "Attachment");
		if (childWithTag != null)
		{
			Thing component = childWithTag.GetComponent<Thing>();
			if (component != null)
			{
				bool flag = true;
				if (onlyIfCloneOfCurrentHead)
				{
					flag = false;
					if (CreationHelper.thingThatWasClonedFrom != null)
					{
						Thing component2 = CreationHelper.thingThatWasClonedFrom.GetComponent<Thing>();
						flag = component2 != null && component2.thingId != string.Empty && component2.thingId == component.thingId;
					}
				}
				if (flag)
				{
					if (currentJson != string.Empty)
					{
						text += ",";
					}
					text = text + "\"" + shortName + "\": {";
					if (!onlyIfCloneOfCurrentHead)
					{
						text = text + "\"i\":\"" + component.thingId + "\",";
					}
					text = text + "\"p\":" + JsonHelper.GetJson(component.transform.localPosition) + ",";
					text = text + "\"r\":" + JsonHelper.GetJson(component.transform.localEulerAngles);
					if (includingAttachmentPointPositions)
					{
						text += ",";
						text = text + "\"ap\":" + JsonHelper.GetJson(component.transform.parent.localPosition) + ",";
						text = text + "\"ar\":" + JsonHelper.GetJson(component.transform.parent.localEulerAngles);
					}
					text += "}";
				}
			}
		}
		return text;
	}

	// Token: 0x06001829 RID: 6185 RVA: 0x000DE2BC File Offset: 0x000DC6BC
	private static string GetAttributesAsIntList(Thing thing)
	{
		List<int> list = new List<int>();
		if (thing.isClonable)
		{
			list.Add(1);
		}
		if (thing.isHoldable)
		{
			list.Add(2);
		}
		if (thing.remainsHeld)
		{
			list.Add(3);
		}
		if (thing.isClimbable)
		{
			list.Add(4);
		}
		if (thing.isPassable)
		{
			list.Add(6);
		}
		if (thing.isUnwalkable)
		{
			list.Add(5);
		}
		if (thing.doSnapAngles)
		{
			list.Add(7);
		}
		if (thing.doSoftSnapAngles)
		{
			list.Add(22);
		}
		if (thing.isBouncy)
		{
			list.Add(9);
		}
		if (thing.doShowDirection)
		{
			list.Add(12);
		}
		if (thing.keepPreciseCollider)
		{
			list.Add(13);
		}
		if (thing.doesFloat)
		{
			list.Add(14);
		}
		if (thing.doesShatter)
		{
			list.Add(15);
		}
		if (thing.isSticky)
		{
			list.Add(16);
		}
		if (thing.isSlidy)
		{
			list.Add(17);
		}
		if (thing.doSnapPosition)
		{
			list.Add(18);
		}
		if (thing.amplifySpeech)
		{
			list.Add(19);
		}
		if (thing.benefitsFromShowingAtDistance)
		{
			list.Add(20);
		}
		if (thing.scaleAllParts)
		{
			list.Add(21);
		}
		if (thing.doAlwaysMergeParts)
		{
			list.Add(23);
		}
		if (thing.addBodyWhenAttached)
		{
			list.Add(24);
		}
		if (thing.hasSurroundSound)
		{
			list.Add(25);
		}
		if (thing.canGetEventsWhenStateChanging)
		{
			list.Add(26);
		}
		if (thing.replacesHandsWhenAttached)
		{
			list.Add(27);
		}
		if (thing.mergeParticleSystems)
		{
			list.Add(28);
		}
		if (thing.isSittable)
		{
			list.Add(29);
		}
		if (thing.smallEditMovements)
		{
			list.Add(30);
		}
		if (thing.scaleEachPartUniformly)
		{
			list.Add(31);
		}
		if (thing.snapAllPartsToGrid)
		{
			list.Add(32);
		}
		if (thing.invisibleToUsWhenAttached)
		{
			list.Add(33);
		}
		if (thing.replaceInstancesInArea)
		{
			list.Add(34);
		}
		if (thing.addBodyWhenAttachedNonClearing)
		{
			list.Add(35);
		}
		if (thing.avoidCastShadow)
		{
			list.Add(36);
		}
		if (thing.avoidReceiveShadow)
		{
			list.Add(37);
		}
		if (thing.omitAutoSounds)
		{
			list.Add(38);
		}
		if (thing.omitAutoHapticFeedback)
		{
			list.Add(39);
		}
		if (thing.keepSizeInInventory)
		{
			list.Add(40);
		}
		if (thing.autoAddReflectionPartsSideways)
		{
			list.Add(41);
		}
		if (thing.autoAddReflectionPartsVertical)
		{
			list.Add(42);
		}
		if (thing.autoAddReflectionPartsDepth)
		{
			list.Add(43);
		}
		if (thing.activeEvenInInventory)
		{
			list.Add(44);
		}
		if (thing.stricterPhysicsSyncing)
		{
			list.Add(45);
		}
		if (thing.removeOriginalWhenGrabbed)
		{
			list.Add(46);
		}
		if (thing.persistWhenThrownOrEmitted)
		{
			list.Add(47);
		}
		if (thing.invisible)
		{
			list.Add(48);
		}
		if (thing.uncollidable)
		{
			list.Add(49);
		}
		if (thing.movableByEveryone)
		{
			list.Add(50);
		}
		if (thing.isNeverClonable)
		{
			list.Add(51);
		}
		if (thing.floatsOnLiquid)
		{
			list.Add(52);
		}
		if (thing.invisibleToDesktopCamera)
		{
			list.Add(53);
		}
		if (thing.personalExperience)
		{
			list.Add(54);
		}
		return ThingToJsonConverter.GetCommaSeparatedIntList(list);
	}

	// Token: 0x0600182A RID: 6186 RVA: 0x000DE698 File Offset: 0x000DCA98
	private static string GetSubThingAttributeInvertsAsIntList(IncludedSubThing subThing)
	{
		List<int> list = new List<int>();
		if (subThing.invert_isHoldable)
		{
			list.Add(2);
		}
		if (subThing.invert_invisible)
		{
			list.Add(48);
		}
		if (subThing.invert_uncollidable)
		{
			list.Add(49);
		}
		return ThingToJsonConverter.GetCommaSeparatedIntList(list);
	}

	// Token: 0x0600182B RID: 6187 RVA: 0x000DE6EC File Offset: 0x000DCAEC
	private static string GetThingPartAttributeAsIntList(ThingPart thingPart)
	{
		List<int> list = new List<int>();
		if (thingPart.offersScreen)
		{
			list.Add(1);
		}
		if (thingPart.videoScreenHasSurroundSound)
		{
			list.Add(4);
		}
		if (thingPart.videoScreenLoops)
		{
			list.Add(27);
		}
		if (thingPart.videoScreenIsDirectlyOnMesh)
		{
			list.Add(28);
		}
		if (thingPart.scalesUniformly)
		{
			list.Add(3);
		}
		if (thingPart.isLiquid)
		{
			list.Add(5);
		}
		if (thingPart.offersSlideshowScreen)
		{
			list.Add(6);
		}
		if (thingPart.isCamera)
		{
			list.Add(8);
		}
		if (thingPart.isFishEyeCamera)
		{
			list.Add(10);
		}
		if (thingPart.useUnsoftenedAnimations)
		{
			list.Add(11);
		}
		if (thingPart.invisible)
		{
			list.Add(12);
		}
		if (thingPart.uncollidable)
		{
			list.Add(37);
		}
		if (thingPart.isUnremovableCenter)
		{
			list.Add(13);
		}
		if (thingPart.omitAutoSounds)
		{
			list.Add(14);
		}
		if (thingPart.doSnapTextureAngles)
		{
			list.Add(15);
		}
		if (thingPart.textureScalesUniformly)
		{
			list.Add(16);
		}
		if (thingPart.avoidCastShadow)
		{
			list.Add(17);
		}
		if (thingPart.looselyCoupledParticles)
		{
			list.Add(18);
		}
		if (thingPart.textAlignCenter)
		{
			list.Add(19);
		}
		if (thingPart.textAlignRight)
		{
			list.Add(20);
		}
		if (thingPart.isAngleLocker)
		{
			list.Add(21);
		}
		if (thingPart.isPositionLocker)
		{
			list.Add(22);
		}
		if (thingPart.isLocked)
		{
			list.Add(23);
		}
		if (thingPart.avoidReceiveShadow)
		{
			list.Add(24);
		}
		if (thingPart.isImagePasteScreen)
		{
			list.Add(25);
		}
		if (thingPart.allowBlackImageBackgrounds)
		{
			list.Add(26);
		}
		if (thingPart.useTextureAsSky)
		{
			list.Add(29);
		}
		if (thingPart.stretchSkydomeSeam)
		{
			list.Add(30);
		}
		if (thingPart.subThingsFollowDelayed)
		{
			list.Add(31);
		}
		if (thingPart.hasReflectionPartSideways)
		{
			list.Add(32);
		}
		if (thingPart.hasReflectionPartVertical)
		{
			list.Add(33);
		}
		if (thingPart.hasReflectionPartDepth)
		{
			list.Add(34);
		}
		if (thingPart.videoScreenFlipsX)
		{
			list.Add(35);
		}
		if (thingPart.persistStates)
		{
			list.Add(36);
		}
		if (thingPart.isDedicatedCollider)
		{
			list.Add(38);
		}
		if (thingPart.personalExperience)
		{
			list.Add(39);
		}
		if (thingPart.invisibleToUsWhenAttached)
		{
			list.Add(40);
		}
		if (thingPart.lightOmitsShadow)
		{
			list.Add(41);
		}
		if (thingPart.showDirectionArrowsWhenEditing)
		{
			list.Add(42);
		}
		return ThingToJsonConverter.GetCommaSeparatedIntList(list);
	}

	// Token: 0x0600182C RID: 6188 RVA: 0x000DE9E4 File Offset: 0x000DCDE4
	public static string GetCommaSeparatedIntList(List<int> list)
	{
		string text = string.Empty;
		foreach (int num in list)
		{
			if (text != string.Empty)
			{
				text += ",";
			}
			text += num.ToString();
		}
		return text;
	}

	// Token: 0x0600182D RID: 6189 RVA: 0x000DEA6C File Offset: 0x000DCE6C
	public static string GetCommaSeparatedStringList(List<string> list)
	{
		string text = string.Empty;
		foreach (string text2 in list)
		{
			if (text != string.Empty)
			{
				text += "\",\"";
			}
			text += text2;
		}
		if (text != string.Empty)
		{
			text = "\"" + text + "\"";
		}
		return text;
	}

	// Token: 0x0600182E RID: 6190 RVA: 0x000DEB08 File Offset: 0x000DCF08
	private static string GetStateJson(Transform transform, ThingPart thingPart)
	{
		string text = string.Empty;
		int num = 0;
		foreach (ThingPartState thingPartState in thingPart.states)
		{
			if (++num >= 2)
			{
				text += ",";
			}
			text += "{";
			text = text + "\"p\":" + JsonHelper.GetJson(thingPartState.position) + ",";
			text = text + "\"r\":" + JsonHelper.GetJson(thingPartState.rotation) + ",";
			text = text + "\"s\":" + JsonHelper.GetJson(thingPartState.scale) + ",";
			text = text + "\"c\":" + JsonHelper.GetJson(thingPartState.color);
			if (thingPartState.scriptLines.Count >= 1)
			{
				text += ",\"b\":[";
				for (int i = 0; i < thingPartState.scriptLines.Count; i++)
				{
					if (i != 0)
					{
						text += ",";
					}
					text += JsonHelper.GetJson(thingPartState.scriptLines[i]);
				}
				text += "]";
			}
			if (thingPartState.textureProperties != null && thingPartState.textureProperties[0] != null && thingPart.textureTypes[0] != TextureType.None)
			{
				text = text + ",\"t1\":{" + ThingToJsonConverter.GetStateTextureJson(thingPart, thingPartState, 0) + "}";
			}
			if (thingPartState.textureProperties != null && thingPartState.textureProperties[1] != null && thingPart.textureTypes[1] != TextureType.None)
			{
				text = text + ",\"t2\":{" + ThingToJsonConverter.GetStateTextureJson(thingPart, thingPartState, 1) + "}";
			}
			if (thingPartState.particleSystemProperty != null)
			{
				text = text + ",\"pr\":{" + ThingToJsonConverter.GetStateParticleSystemJson(thingPart, thingPartState) + "}";
			}
			text += "}";
		}
		return text;
	}

	// Token: 0x0600182F RID: 6191 RVA: 0x000DED20 File Offset: 0x000DD120
	private static string GetStateTextureJson(ThingPart thingPart, ThingPartState state, int index)
	{
		string text = string.Empty;
		text = text + "\"c\":" + JsonHelper.GetJson(state.textureColors[index]);
		bool flag = Managers.thingManager.IsTextureTypeWithOnlyAlphaSetting(thingPart.textureTypes[index]);
		foreach (KeyValuePair<TextureProperty, string> keyValuePair in Managers.thingManager.texturePropertyAbbreviations)
		{
			if (!flag || keyValuePair.Key == TextureProperty.Strength)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					",\"",
					keyValuePair.Value,
					"\":",
					state.textureProperties[index][keyValuePair.Key]
				});
			}
		}
		return text;
	}

	// Token: 0x06001830 RID: 6192 RVA: 0x000DEE14 File Offset: 0x000DD214
	private static string GetStateParticleSystemJson(ThingPart thingPart, ThingPartState state)
	{
		string text = string.Empty;
		text = text + "\"c\":" + JsonHelper.GetJson(state.particleSystemColor);
		bool flag = Managers.thingManager.IsParticleSystemTypeWithOnlyAlphaSetting(thingPart.particleSystemType);
		foreach (KeyValuePair<ParticleSystemProperty, string> keyValuePair in Managers.thingManager.particleSystemPropertyAbbreviations)
		{
			if (!flag || keyValuePair.Key == ParticleSystemProperty.Alpha)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					",\"",
					keyValuePair.Value,
					"\":",
					state.particleSystemProperty[keyValuePair.Key]
				});
			}
		}
		return text;
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x000DEEF8 File Offset: 0x000DD2F8
	private static string GetChangedVerticesJson(ThingPart thingPart, Dictionary<string, int> changedVerticesIndexReference, int indexWithinThing, out int indicesCount)
	{
		string text = string.Empty;
		indicesCount = 0;
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Dictionary<int, Vector3> dictionary2 = null;
		if (thingPart.changedVertices != null)
		{
			dictionary2 = new Dictionary<int, Vector3>();
			foreach (KeyValuePair<int, Vector3> keyValuePair in thingPart.changedVertices)
			{
				dictionary2.Add(keyValuePair.Key, Misc.ReduceVector3Digits(keyValuePair.Value, 3));
			}
		}
		foreach (KeyValuePair<int, Vector3> keyValuePair2 in dictionary2)
		{
			int key = keyValuePair2.Key;
			Vector3 value = keyValuePair2.Value;
			string jsonNoBrackets = JsonHelper.GetJsonNoBrackets(value);
			if (!dictionary.ContainsKey(jsonNoBrackets))
			{
				string text2 = string.Empty;
				int num = 0;
				foreach (KeyValuePair<int, Vector3> keyValuePair3 in dictionary2)
				{
					int key2 = keyValuePair3.Key;
					Vector3 value2 = keyValuePair3.Value;
					if (JsonHelper.GetJsonNoBrackets(value2) == jsonNoBrackets)
					{
						if (text2 != string.Empty)
						{
							text2 += ",";
						}
						text2 += (key2 - num).ToString();
						num = key2;
					}
				}
				dictionary[jsonNoBrackets] = text2;
			}
		}
		foreach (KeyValuePair<string, string> keyValuePair4 in dictionary)
		{
			if (text != string.Empty)
			{
				text += ",";
			}
			string text3 = text;
			text = string.Concat(new string[] { text3, "[", keyValuePair4.Key, ",", keyValuePair4.Value, "]" });
		}
		if (text != string.Empty)
		{
			text = "\"c\":[" + text + "]";
			string text4 = text;
			int? smoothingAngle = thingPart.smoothingAngle;
			if (smoothingAngle != null)
			{
				string text5 = text4;
				int? smoothingAngle2 = thingPart.smoothingAngle;
				text4 = text5 + smoothingAngle2.Value.ToString();
			}
			text4 += "_";
			bool? convex = thingPart.convex;
			if (convex != null)
			{
				text4 += ((!(thingPart.convex == true)) ? "0" : "1");
			}
			int num2;
			if (changedVerticesIndexReference.TryGetValue(text4, out num2))
			{
				text = "\"v\":" + num2;
				thingPart.smoothingAngle = null;
				thingPart.convex = null;
			}
			else
			{
				changedVerticesIndexReference[text4] = indexWithinThing;
			}
		}
		indicesCount = dictionary.Count;
		return text;
	}
}
