using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020001FF RID: 511
public class TemporarilyDestroyedThingsManager : MonoBehaviour, IGameManager
{
	// Token: 0x17000239 RID: 569
	// (get) Token: 0x0600135B RID: 4955 RVA: 0x000AD933 File Offset: 0x000ABD33
	// (set) Token: 0x0600135C RID: 4956 RVA: 0x000AD93B File Offset: 0x000ABD3B
	public ManagerStatus status { get; private set; }

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x0600135D RID: 4957 RVA: 0x000AD944 File Offset: 0x000ABD44
	// (set) Token: 0x0600135E RID: 4958 RVA: 0x000AD94C File Offset: 0x000ABD4C
	public string failMessage { get; private set; }

	// Token: 0x0600135F RID: 4959 RVA: 0x000AD955 File Offset: 0x000ABD55
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.wrapper = new GameObject("TemporarilyDestroyedThings");
		this.wrapper.transform.parent = Managers.treeManager.GetTransform("/Universe");
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001360 RID: 4960 RVA: 0x000AD994 File Offset: 0x000ABD94
	private void Update()
	{
		this.RestorePlacementsIfNeeded(false);
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x000AD9A0 File Offset: 0x000ABDA0
	private void RestorePlacementsIfNeeded(bool forceRestore = false)
	{
		if (this.placementsRestoreTime.Count >= 1)
		{
			float time = Time.time;
			KeyValuePair<GameObject, float>[] array = this.placementsRestoreTime.Where((KeyValuePair<GameObject, float> x) => time >= x.Value || forceRestore).ToArray<KeyValuePair<GameObject, float>>();
			foreach (KeyValuePair<GameObject, float> keyValuePair in array)
			{
				GameObject key = keyValuePair.Key;
				key.transform.parent = Managers.thingManager.placements.transform;
				key.SetActive(true);
				this.placementsRestoreTime.Remove(key);
				Thing component = key.GetComponent<Thing>();
				key.name = component.givenName;
				component.TriggerEvent(StateListener.EventType.OnDestroyedRestored, string.Empty, false, null);
			}
		}
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x000ADA90 File Offset: 0x000ABE90
	public void AddPlacement(Thing thing, float? restoreAfterSeconds)
	{
		if (restoreAfterSeconds == null)
		{
			restoreAfterSeconds = new float?(86400f);
		}
		if (Managers.personManager.ourPerson != null && Managers.personManager.ourPerson.isMasterClient)
		{
			thing.TriggerEventAsStateAuthority(StateListener.EventType.OnDestroyed, string.Empty);
		}
		thing.gameObject.SetActive(false);
		thing.behaviorScriptVariables = new Dictionary<string, float>();
		thing.ResetStates();
		thing.transform.parent = this.wrapper.transform;
		thing.gameObject.name = Universe.objectNameIfAlreadyDestroyed;
		this.RemoveExistingPlacementIds(thing.placementId);
		this.placementsRestoreTime[thing.gameObject] = Time.time + restoreAfterSeconds.Value;
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x000ADB5C File Offset: 0x000ABF5C
	private void RemoveExistingPlacementIds(string placementId)
	{
		KeyValuePair<GameObject, float>[] array = this.placementsRestoreTime.Where((KeyValuePair<GameObject, float> x) => x.Key.gameObject.GetComponent<Thing>().placementId == placementId).ToArray<KeyValuePair<GameObject, float>>();
		foreach (KeyValuePair<GameObject, float> keyValuePair in array)
		{
			this.placementsRestoreTime.Remove(keyValuePair.Key);
		}
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x000ADBD0 File Offset: 0x000ABFD0
	public void ClearAll()
	{
		this.placementsRestoreTime = new Dictionary<GameObject, float>();
		IEnumerator enumerator = this.wrapper.transform.GetEnumerator();
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

	// Token: 0x06001365 RID: 4965 RVA: 0x000ADC4C File Offset: 0x000AC04C
	public void RestoreAll()
	{
		this.RestorePlacementsIfNeeded(true);
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x000ADC58 File Offset: 0x000AC058
	public string GetMasterClientSyncString()
	{
		string text = string.Empty;
		if (this.placementsRestoreTime.Count >= 1)
		{
			foreach (KeyValuePair<GameObject, float> keyValuePair in this.placementsRestoreTime)
			{
				GameObject key = keyValuePair.Key;
				float num = keyValuePair.Value - Time.time;
				Thing component = key.GetComponent<Thing>();
				if (text != string.Empty)
				{
					text += "\n";
				}
				string text2 = text;
				text = string.Concat(new object[] { text2, component.placementId, "|", num });
			}
		}
		return text;
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x000ADD30 File Offset: 0x000AC130
	public void SetFromMasterClientSyncString(string s)
	{
		if (!string.IsNullOrEmpty(s))
		{
			string[] array = Misc.Split(s, "\n", StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				string[] array3 = Misc.Split(text, "|", StringSplitOptions.RemoveEmptyEntries);
				string text2 = array3[0];
				GameObject placementById = Managers.thingManager.GetPlacementById(text2, false);
				if (placementById != null)
				{
					Thing component = placementById.GetComponent<Thing>();
					float num;
					if (float.TryParse(array3[1], out num))
					{
						this.AddPlacement(component, new float?(num));
					}
				}
			}
		}
	}

	// Token: 0x040011BA RID: 4538
	private GameObject wrapper;

	// Token: 0x040011BB RID: 4539
	private Dictionary<GameObject, float> placementsRestoreTime = new Dictionary<GameObject, float>();
}
