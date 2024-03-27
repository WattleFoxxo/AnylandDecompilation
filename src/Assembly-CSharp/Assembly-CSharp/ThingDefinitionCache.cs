using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200027F RID: 639
public class ThingDefinitionCache : MonoBehaviour
{
	// Token: 0x06001834 RID: 6196 RVA: 0x000DF2D1 File Offset: 0x000DD6D1
	public void Start()
	{
		this.USE_SECOND_LEVEL_CACHE = true;
		if (!this.USE_SECOND_LEVEL_CACHE)
		{
			Log.Warning("ThingDefinitionCache : Second level cache is currently disabled!");
		}
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x000DF2F0 File Offset: 0x000DD6F0
	public IEnumerator GetThingDefinition(ThingRequestContext thingRequestContext, string thingId, Action<string, string> callback)
	{
		if (callback == null)
		{
			throw new Exception("Callback cannot be null");
		}
		if (this.CONTROL_PARALLEL_REQUESTS)
		{
			while (this.IsRequestInProgress(thingId) || this.GetNumberOfRequestsInProgress() >= 12)
			{
				yield return null;
			}
		}
		string thingDefinitionJSON = this.GetFromLevel1(thingId);
		if (thingDefinitionJSON != null)
		{
			callback(null, thingDefinitionJSON);
			yield break;
		}
		if (this.USE_SECOND_LEVEL_CACHE)
		{
			thingDefinitionJSON = this.GetFromLevel2(thingId);
			if (thingDefinitionJSON != null)
			{
				this.StoreInLevel1(thingId, thingDefinitionJSON);
				callback(null, thingDefinitionJSON);
				yield break;
			}
		}
		this.RegisterRequestInProgress(thingId);
		GetThingDefinition_Response response = null;
		yield return base.StartCoroutine(Managers.serverManager.GetThingDefinition(thingId, delegate(GetThingDefinition_Response getThingDefinitionResponse)
		{
			response = getThingDefinitionResponse;
		}));
		this.RegisterRequestCompleted(thingId);
		if (!string.IsNullOrEmpty(response.error))
		{
			callback(response.error, null);
			yield break;
		}
		if (!string.IsNullOrEmpty(response.thingDefinitionJSON))
		{
			thingDefinitionJSON = response.thingDefinitionJSON;
			this.StoreInAllCaches(thingId, thingDefinitionJSON);
			callback(null, thingDefinitionJSON);
			yield break;
		}
		Log.Warning("Empty thing definition for : " + thingId);
		yield return base.StartCoroutine(Managers.serverManager.ReportMissingThing(thingRequestContext, thingId, delegate(ResponseBase res)
		{
		}));
		callback(null, null);
		yield break;
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x000DF320 File Offset: 0x000DD720
	public IEnumerator PrimeCacheWithThingDefinitionBundle(string areaId, string key, Action<bool> callback)
	{
		if (callback == null)
		{
			throw new Exception("Callback cannot be null");
		}
		Log.Info("PrimeCacheWithThingDefinitionBundle - requesting bundle for area : " + areaId, false);
		bool primedOk = true;
		GetThingDefinitionAreaBundle_Response response = null;
		yield return base.StartCoroutine(Managers.serverManager.GetThingDefinitionAreaBundle(areaId, key, delegate(GetThingDefinitionAreaBundle_Response _response)
		{
			response = _response;
		}));
		Log.Info("PrimeCacheWithThingDefinitionBundle - got thingIds : " + response.thingDefinitions.Count, false);
		if (!string.IsNullOrEmpty(response.error))
		{
			primedOk = false;
			Log.Error(response.error);
		}
		else
		{
			foreach (ThingIdAndDefinition thingIdAndDefinition in response.thingDefinitions)
			{
				this.StoreInAllCaches(thingIdAndDefinition.id, thingIdAndDefinition.def);
			}
		}
		callback(primedOk);
		yield break;
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x000DF350 File Offset: 0x000DD750
	public IEnumerator PrimeCacheWithThingDefinitionBundleIfNeeded(List<PlacementData> placements, string areaId, string key, Action<bool> callback)
	{
		Log.Info("*** PRIME CACHE - START", false);
		if (callback == null)
		{
			throw new Exception("Callback cannot be null");
		}
		int numberOfMisses = this.GetNumberOfCacheMissesInPlacements(placements);
		if (!this.USE_THINGDEF_BUNDLES || numberOfMisses < 100)
		{
			Log.Info("PrimeCacheWithThingDefinitionBundleIfNeeded - skipping", false);
			callback(true);
		}
		else
		{
			yield return this.PrimeCacheWithThingDefinitionBundle(areaId, key, callback);
		}
		yield break;
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x000DF388 File Offset: 0x000DD788
	private string GetFromLevel1(string thingId)
	{
		string empty = string.Empty;
		if (this.level1Cache.TryGetValue(thingId, out empty))
		{
			return empty;
		}
		return null;
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x000DF3B4 File Offset: 0x000DD7B4
	private string GetFromLevel2(string thingId)
	{
		string cacheRecordFileDirectory = this.GetCacheRecordFileDirectory(thingId);
		string text = null;
		try
		{
			text = File.ReadAllText(Path.Combine(cacheRecordFileDirectory, thingId));
		}
		catch (DirectoryNotFoundException ex)
		{
		}
		catch (FileNotFoundException ex2)
		{
		}
		return text;
	}

	// Token: 0x0600183A RID: 6202 RVA: 0x000DF408 File Offset: 0x000DD808
	private void StoreInLevel1(string thingId, string thingDefinitionJSON)
	{
		this.level1Cache[thingId] = thingDefinitionJSON;
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x000DF418 File Offset: 0x000DD818
	private void StoreInLevel2(string thingId, string thingDefinitionJSON)
	{
		string cacheRecordFileDirectory = this.GetCacheRecordFileDirectory(thingId);
		Directory.CreateDirectory(cacheRecordFileDirectory);
		File.WriteAllText(Path.Combine(cacheRecordFileDirectory, thingId), thingDefinitionJSON);
	}

	// Token: 0x0600183C RID: 6204 RVA: 0x000DF441 File Offset: 0x000DD841
	public void StoreInAllCaches(string thingId, string thingDefinitionJSON)
	{
		this.StoreInLevel1(thingId, thingDefinitionJSON);
		if (this.USE_SECOND_LEVEL_CACHE)
		{
			this.StoreInLevel2(thingId, thingDefinitionJSON);
		}
	}

	// Token: 0x0600183D RID: 6205 RVA: 0x000DF460 File Offset: 0x000DD860
	public void ClearPersistentCache()
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Application.persistentDataPath, this.PersistentCacheDirectory));
		directoryInfo.Delete(true);
	}

	// Token: 0x0600183E RID: 6206 RVA: 0x000DF48C File Offset: 0x000DD88C
	private string GetCacheRecordFileDirectory(string thingId)
	{
		if (thingId.Length != 24)
		{
			throw new Exception("GetCacheRecordFilePath received bad objectId \"" + thingId + "\"");
		}
		string text = Path.Combine(Application.persistentDataPath, this.PersistentCacheDirectory);
		string text2 = thingId.Substring(thingId.Length - 6);
		string text3 = text2.Substring(0, 3);
		string text4 = text2.Substring(3);
		string text5 = text;
		return string.Concat(new object[]
		{
			text5,
			Path.DirectorySeparatorChar,
			text3,
			Path.DirectorySeparatorChar,
			text4,
			Path.DirectorySeparatorChar
		});
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x000DF534 File Offset: 0x000DD934
	private bool IsThingCachedLocally(string thingId)
	{
		string text = this.GetFromLevel1(thingId);
		if (text == null)
		{
			text = this.GetFromLevel2(thingId);
		}
		return text != null;
	}

	// Token: 0x06001840 RID: 6208 RVA: 0x000DF55E File Offset: 0x000DD95E
	private void RegisterRequestInProgress(string thingId)
	{
		this.inProgressRequests.Add(thingId, null);
	}

	// Token: 0x06001841 RID: 6209 RVA: 0x000DF56D File Offset: 0x000DD96D
	private void RegisterRequestCompleted(string thingId)
	{
		this.inProgressRequests.Remove(thingId);
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x000DF57C File Offset: 0x000DD97C
	private bool IsRequestInProgress(string thingId)
	{
		return this.inProgressRequests.ContainsKey(thingId);
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x000DF58A File Offset: 0x000DD98A
	private int GetNumberOfRequestsInProgress()
	{
		return this.inProgressRequests.Count;
	}

	// Token: 0x06001844 RID: 6212 RVA: 0x000DF598 File Offset: 0x000DD998
	public int GetNumberOfCacheMissesInPlacements(List<PlacementData> placements)
	{
		int num = 0;
		foreach (PlacementData placementData in placements)
		{
			string tid = placementData.Tid;
			if (!this.IsThingCachedLocally(tid))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x04001683 RID: 5763
	private const int MAX_PARALLEL_REQUESTS = 12;

	// Token: 0x04001684 RID: 5764
	private const int NUMBER_OF_MISSES_NEEDED_TO_TRIGGER_AREA_THINGDEF_BUNDLE_LOADING = 100;

	// Token: 0x04001685 RID: 5765
	private bool USE_THINGDEF_BUNDLES = true;

	// Token: 0x04001686 RID: 5766
	private bool USE_SECOND_LEVEL_CACHE = true;

	// Token: 0x04001687 RID: 5767
	private bool CONTROL_PARALLEL_REQUESTS = true;

	// Token: 0x04001688 RID: 5768
	public Dictionary<string, string> level1Cache = new Dictionary<string, string>();

	// Token: 0x04001689 RID: 5769
	private Dictionary<string, string> inProgressRequests = new Dictionary<string, string>();

	// Token: 0x0400168A RID: 5770
	public string PersistentCacheDirectory = "cache" + Path.DirectorySeparatorChar + "thingdefs";
}
