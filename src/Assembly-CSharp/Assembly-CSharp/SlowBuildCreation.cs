using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000267 RID: 615
public class SlowBuildCreation : MonoBehaviour
{
	// Token: 0x0600168E RID: 5774 RVA: 0x000CAAFC File Offset: 0x000C8EFC
	private void Start()
	{
		this.thing = base.GetComponent<Thing>();
		this.AddSeparateThingPartsClone();
		global::UnityEngine.Object.Destroy(this);
	}

	// Token: 0x0600168F RID: 5775 RVA: 0x000CAB18 File Offset: 0x000C8F18
	private void AddSeparateThingPartsClone()
	{
		bool suppressScriptsAndStates = this.thing.suppressScriptsAndStates;
		this.thing.suppressScriptsAndStates = true;
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.thing.gameObject, this.thing.transform.position, this.thing.transform.rotation);
		gameObject.transform.localScale = this.thing.transform.localScale;
		gameObject.transform.parent = Managers.thingManager.placements.transform;
		Managers.thingManager.MakeDeepThingClone(this.thing.gameObject, gameObject, false, false, true);
		global::UnityEngine.Object.Destroy(gameObject.GetComponent<Thing>());
		global::UnityEngine.Object.Destroy(gameObject.GetComponent<SlowBuildCreation>());
		GameObject gameObject2 = gameObject;
		gameObject2.name += " (Slow Build)";
		gameObject.transform.parent = Effects.effectsContainerTransform;
		this.thing.suppressScriptsAndStates = suppressScriptsAndStates;
		this.thing.transform.parent = Effects.effectsContainerTransform;
		this.CopyOverSubThingParts(this.thing, gameObject);
		this.ReplaceThingPartsWithSlowBuildParts(gameObject);
		this.thing.gameObject.SetActive(false);
		GameObject combinedColliderForObject = this.GetCombinedColliderForObject(this.thing.gameObject);
		combinedColliderForObject.transform.parent = gameObject.transform;
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x000CAC64 File Offset: 0x000C9064
	private GameObject GetCombinedColliderForObject(GameObject thisObject)
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(thisObject, thisObject.transform.position, thisObject.transform.rotation);
		gameObject.name = "CombinedColliders";
		Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(Component), true);
		foreach (Component component in componentsInChildren)
		{
			string text = component.GetType().ToString().ToLower();
			if (!text.Contains("transform") && !text.Contains("collider"))
			{
				global::UnityEngine.Object.Destroy(component);
			}
			else
			{
				component.gameObject.tag = "SlowBuildCreationCollider";
			}
		}
		gameObject.SetActive(true);
		return gameObject;
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x000CAD28 File Offset: 0x000C9128
	private void CopyOverSubThingParts(Thing thing, GameObject target)
	{
		Component[] componentsInChildren = thing.GetComponentsInChildren<Thing>();
		foreach (Thing thing2 in componentsInChildren)
		{
			if (thing2 != thing)
			{
				Component[] componentsInChildren2 = thing2.GetComponentsInChildren<ThingPart>();
				foreach (ThingPart thingPart in componentsInChildren2)
				{
					GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(thingPart.gameObject, target.transform, true);
					GameObject gameObject2 = gameObject;
					gameObject2.name += " (Via Sub-Thing)";
				}
			}
		}
	}

	// Token: 0x06001692 RID: 5778 RVA: 0x000CADC4 File Offset: 0x000C91C4
	private void ReplaceThingPartsWithSlowBuildParts(GameObject clonedThing)
	{
		Component[] componentsInChildren = clonedThing.GetComponentsInChildren(typeof(ThingPart), true);
		int num = 0;
		SlowBuildPart slowBuildPart = null;
		foreach (ThingPart thingPart in componentsInChildren)
		{
			Renderer component = thingPart.GetComponent<Renderer>();
			if ((component && !component.enabled) || thingPart.invisible || !thingPart.gameObject.activeSelf)
			{
				global::UnityEngine.Object.Destroy(thingPart.gameObject);
			}
			else if (thingPart.gameObject.name != Universe.objectNameIfAlreadyDestroyed)
			{
				slowBuildPart = thingPart.gameObject.AddComponent<SlowBuildPart>();
				slowBuildPart.Init(num++, SlowBuildCreationDialog.startDelaySeconds, SlowBuildCreationDialog.secondsPerPart);
				global::UnityEngine.Object.Destroy(thingPart);
			}
			Collider component2 = thingPart.GetComponent<Collider>();
			global::UnityEngine.Object.Destroy(component2);
		}
		if (slowBuildPart != null)
		{
			slowBuildPart.thingToRestoreAfter = this.thing;
		}
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x000CAEC4 File Offset: 0x000C92C4
	private void RemoveChildren()
	{
		IEnumerator enumerator = base.transform.GetEnumerator();
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

	// Token: 0x06001694 RID: 5780 RVA: 0x000CAF30 File Offset: 0x000C9330
	private void RestoreOriginal()
	{
		global::UnityEngine.Object.Destroy(this);
		this.thing.gameObject.SetActive(true);
	}

	// Token: 0x0400144D RID: 5197
	private Thing thing;
}
