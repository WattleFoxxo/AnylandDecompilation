using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AF RID: 431
public class Effects
{
	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000D46 RID: 3398 RVA: 0x00077041 File Offset: 0x00075441
	// (set) Token: 0x06000D47 RID: 3399 RVA: 0x00077048 File Offset: 0x00075448
	public static GameObject effectsContainer { get; private set; } = new GameObject("Effects");

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000D48 RID: 3400 RVA: 0x00077050 File Offset: 0x00075450
	// (set) Token: 0x06000D49 RID: 3401 RVA: 0x00077057 File Offset: 0x00075457
	public static Transform effectsContainerTransform { get; private set; } = Effects.effectsContainer.transform;

	// Token: 0x06000D4A RID: 3402 RVA: 0x0007705F File Offset: 0x0007545F
	private static bool EffectEntitiesReachedLimit()
	{
		return Effects.effectsContainerTransform != null && Effects.effectsContainerTransform.childCount >= 100;
	}

	// Token: 0x06000D4B RID: 3403 RVA: 0x00077088 File Offset: 0x00075488
	public static void AddSplash(Vector3 position, Color color)
	{
		if (!Universe.showEffects || Effects.EffectEntitiesReachedLimit())
		{
			return;
		}
		GameObject @object = Managers.treeManager.GetObject("/Universe/MiscellaneousSourceObjects");
		GameObject gameObject = Misc.FindObject(@object, "WaterSplash");
		GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
		gameObject2.transform.parent = Effects.effectsContainerTransform;
		gameObject2.transform.localPosition = position;
		gameObject2.SetActive(true);
		ParticleSystem component = gameObject2.GetComponent<ParticleSystem>();
		component.startColor = color;
		Sound sound = new Sound();
		sound.name = "water splash";
		sound.volume = 0.25f;
		Managers.soundLibraryManager.Play(position, sound, false, false, false, -1f);
	}

	// Token: 0x06000D4C RID: 3404 RVA: 0x00077134 File Offset: 0x00075534
	public static void SpawnCrumbles(GameObject objectToCrumble)
	{
		if (!Universe.showEffects || Effects.EffectEntitiesReachedLimit())
		{
			return;
		}
		int num = 6;
		float num2 = 0.8f;
		GameObject[] array;
		if (objectToCrumble.CompareTag("Thing") || objectToCrumble.GetComponent<Thing>() != null)
		{
			array = Misc.GetChildrenAsArray(objectToCrumble.transform);
			num = 12;
			num2 = 0.3f;
		}
		else
		{
			array = new GameObject[] { objectToCrumble };
		}
		for (int i = 0; i < num; i++)
		{
			int num3 = array.Length - 1;
			GameObject gameObject = array[global::UnityEngine.Random.Range(0, num3 + 1)];
			Renderer component = gameObject.GetComponent<Renderer>();
			if (component != null)
			{
				GameObject gameObject2 = ((!Misc.Chance(50f)) ? GameObject.CreatePrimitive(PrimitiveType.Cube) : GameObject.CreatePrimitive(PrimitiveType.Sphere));
				gameObject2.transform.parent = Effects.effectsContainerTransform;
				EffectParticle effectParticle = gameObject2.AddComponent<EffectParticle>();
				Vector3 vector = gameObject.transform.position;
				if (vector == Vector3.zero)
				{
					vector = objectToCrumble.transform.position;
				}
				Vector3 localScale = gameObject.transform.localScale;
				Vector3 vector2 = new Vector3(vector.x + global::UnityEngine.Random.Range(-localScale.x * num2, localScale.x * num2), vector.y + global::UnityEngine.Random.Range(-localScale.y * num2, localScale.y * num2), vector.z + global::UnityEngine.Random.Range(-localScale.z * num2, localScale.z * num2));
				Color color = ((!component.material.HasProperty("_Color")) ? Color.black : component.material.color);
				effectParticle.BeCrumble(vector2, color);
			}
		}
	}

	// Token: 0x06000D4D RID: 3405 RVA: 0x000772F8 File Offset: 0x000756F8
	public static void SpawnNewCreationSparkles(GameObject newThing)
	{
		GameObject @object = Managers.treeManager.GetObject("/Universe/MiscellaneousSourceObjects/Sparkle");
		GameObject[] childrenAsArray = Misc.GetChildrenAsArray(newThing.transform);
		for (int i = 0; i < 12; i++)
		{
			GameObject gameObject = childrenAsArray[global::UnityEngine.Random.Range(0, childrenAsArray.Length - 1)];
			GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(@object);
			gameObject2.SetActive(true);
			EffectParticle effectParticle = gameObject2.AddComponent<EffectParticle>();
			Vector3 position = gameObject.transform.position;
			Vector3 localScale = gameObject.transform.localScale;
			Vector3 vector = new Vector3(position.x + global::UnityEngine.Random.Range(-localScale.x * 1.5f, localScale.x * 1.5f), position.y + global::UnityEngine.Random.Range(-localScale.y * 1.5f, localScale.y * 1.5f), position.z + global::UnityEngine.Random.Range(-localScale.z * 1.5f, localScale.z * 1.5f));
			effectParticle.BeSparkle(vector);
		}
	}

	// Token: 0x06000D4E RID: 3406 RVA: 0x00077400 File Offset: 0x00075800
	public static void SpawnSparklesOnPoints(List<Vector3> points)
	{
		GameObject @object = Managers.treeManager.GetObject("/Universe/MiscellaneousSourceObjects/Sparkle");
		foreach (Vector3 vector in points)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(@object);
			gameObject.SetActive(true);
			EffectParticle effectParticle = gameObject.AddComponent<EffectParticle>();
			effectParticle.BeSparkle(vector);
		}
	}

	// Token: 0x06000D4F RID: 3407 RVA: 0x00077480 File Offset: 0x00075880
	public static void BreakIntoPieces(Thing thingOriginal, ThingDestruction thingDestruction)
	{
		string text;
		if (Managers.thingManager.thingDefinitionCache.level1Cache.TryGetValue(thingOriginal.thingId, out text))
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(Managers.thingManager.thingGameObject);
			JsonToThingConverter.SetThing(gameObject, text, true, false, null, null);
			gameObject.transform.parent = thingOriginal.transform.parent;
			gameObject.transform.position = thingOriginal.transform.position;
			gameObject.transform.rotation = thingOriginal.transform.rotation;
			gameObject.transform.localScale = thingOriginal.transform.localScale;
			IEnumerator enumerator = thingOriginal.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.CompareTag("IncludedSubThings"))
					{
						transform.transform.parent = gameObject.transform;
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
			ThingPart[] componentsInChildren = gameObject.GetComponentsInChildren<ThingPart>();
			Misc.ShuffleArray<ThingPart>(componentsInChildren);
			int num = 0;
			while (num < componentsInChildren.Length && num < thingDestruction.maxParts)
			{
				GameObject gameObject2 = componentsInChildren[num].gameObject;
				if (!componentsInChildren[num].invisible)
				{
					Collider component = gameObject2.GetComponent<Collider>();
					gameObject2.transform.parent = Effects.effectsContainerTransform;
					global::UnityEngine.Object.Destroy(componentsInChildren[num]);
					gameObject2.tag = "Untagged";
					MeshCollider component2 = gameObject2.GetComponent<MeshCollider>();
					if (component2 != null)
					{
						component2.convex = true;
					}
					Rigidbody rigidbody = gameObject2.GetComponent<Rigidbody>();
					if (rigidbody != null)
					{
						global::UnityEngine.Object.Destroy(rigidbody);
					}
					rigidbody = gameObject2.AddComponent<Rigidbody>();
					rigidbody.useGravity = thingDestruction.gravity;
					if (component != null)
					{
						if (thingDestruction.bouncy && thingDestruction.slidy)
						{
							component.material = Managers.thingManager.bouncySlidyMaterial;
						}
						else if (thingDestruction.bouncy)
						{
							component.material = Managers.thingManager.bouncyMaterial;
						}
						else if (thingDestruction.slidy)
						{
							component.material = Managers.thingManager.slidyMaterial;
						}
					}
					if (thingDestruction.burstVelocity != 0f)
					{
						rigidbody.AddForce(Misc.GetRandomVector3(thingDestruction.burstVelocity), ForceMode.Impulse);
					}
					if (thingDestruction.growth != 0f)
					{
						ChangeSizeOverTime changeSizeOverTime = gameObject2.AddComponent<ChangeSizeOverTime>();
						changeSizeOverTime.growth = thingDestruction.growth * 0.01f;
					}
					if (!thingDestruction.collides)
					{
						global::UnityEngine.Object.Destroy(component);
					}
					if (!thingDestruction.collidesWithSiblings)
					{
						gameObject2.layer = LayerMask.NameToLayer("PassThroughEachOther");
					}
					DestroyMeAfterTime destroyMeAfterTime = gameObject2.AddComponent<DestroyMeAfterTime>();
					destroyMeAfterTime.seconds = thingDestruction.hidePartsInSeconds + global::UnityEngine.Random.Range(0f, thingDestruction.hidePartsInSeconds * 0.2f);
				}
				num++;
			}
			Thing[] componentsInChildren2 = gameObject.GetComponentsInChildren<Thing>();
			foreach (Thing thing in componentsInChildren2)
			{
				global::UnityEngine.Object.Destroy(thing);
			}
			Misc.Destroy(gameObject);
		}
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x000777D8 File Offset: 0x00075BD8
	public static void ClearAll()
	{
		if (Effects.effectsContainerTransform != null)
		{
			IEnumerator enumerator = Effects.effectsContainerTransform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					global::UnityEngine.Object.Destroy(transform.gameObject);
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
	}

	// Token: 0x04000EEA RID: 3818
	private const int maxEffectEntities = 100;
}
