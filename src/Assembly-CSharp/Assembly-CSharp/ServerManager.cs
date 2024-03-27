using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020001F9 RID: 505
public class ServerManager : MonoBehaviour, IGameManager
{
	// Token: 0x1700022B RID: 555
	// (get) Token: 0x0600128D RID: 4749 RVA: 0x0009CADC File Offset: 0x0009AEDC
	// (set) Token: 0x0600128E RID: 4750 RVA: 0x0009CAE4 File Offset: 0x0009AEE4
	public ManagerStatus status { get; private set; }

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x0600128F RID: 4751 RVA: 0x0009CAED File Offset: 0x0009AEED
	// (set) Token: 0x06001290 RID: 4752 RVA: 0x0009CAF5 File Offset: 0x0009AEF5
	public string failMessage { get; private set; }

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06001291 RID: 4753 RVA: 0x0009CAFE File Offset: 0x0009AEFE
	private bool doUseCDN
	{
		get
		{
			return this.UseCDN && !this.UseLocalServer;
		}
	}

	// Token: 0x06001292 RID: 4754 RVA: 0x0009CB17 File Offset: 0x0009AF17
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.UseLocalServer = false;
		if (this.UseLocalServer)
		{
			this.serverBaseUrl = "http://127.0.0.1:3000";
		}
		else
		{
			this.serverBaseUrl = this.RemoteServerBaseUrl;
		}
		this.StartAuthentication();
	}

	// Token: 0x06001293 RID: 4755 RVA: 0x0009CB54 File Offset: 0x0009AF54
	private WWW getAuthenticatedWWWRequest(string relUrl, HTTPVerb httpVerb, byte[] rawPostBodyData)
	{
		if (string.IsNullOrEmpty(this.ServerAuthSessionToken))
		{
			Log.Warning("getAuthenticatedWWWRequest called when no ServerAuthSessionToken set.  URL: " + relUrl);
		}
		if (httpVerb == HTTPVerb.GET)
		{
			rawPostBodyData = null;
		}
		WWWForm wwwform = new WWWForm();
		Dictionary<string, string> headers = wwwform.headers;
		string text = "s=" + this.ServerAuthSessionToken;
		headers["cookie"] = text;
		string text2 = this.serverBaseUrl + relUrl;
		return new WWW(text2, rawPostBodyData, headers);
	}

	// Token: 0x06001294 RID: 4756 RVA: 0x0009CBD0 File Offset: 0x0009AFD0
	private WWW getNonAuthenticatedWWWRequest(string absoluteUrl, HTTPVerb httpVerb, byte[] rawPostBodyData)
	{
		if (httpVerb == HTTPVerb.GET)
		{
			rawPostBodyData = null;
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		return new WWW(absoluteUrl, rawPostBodyData, dictionary);
	}

	// Token: 0x06001295 RID: 4757 RVA: 0x0009CBF8 File Offset: 0x0009AFF8
	private byte[] getPostBody(Dictionary<string, string> keyValuePairs)
	{
		WWWForm wwwform = new WWWForm();
		foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
		{
			wwwform.AddField(keyValuePair.Key, keyValuePair.Value);
		}
		return wwwform.data;
	}

	// Token: 0x06001296 RID: 4758 RVA: 0x0009CC68 File Offset: 0x0009B068
	private T DeserialiseResponse<T>(WWW www) where T : ResponseBase, new()
	{
		T t = new T();
		if (!string.IsNullOrEmpty(www.text))
		{
			t = JsonUtility.FromJson<T>(www.text);
			if (t == null)
			{
				t = new T();
			}
			t.SetFromWWW(www);
			if (!string.IsNullOrEmpty(t.error))
			{
				Log.Error(t.error);
			}
		}
		return t;
	}

	// Token: 0x06001297 RID: 4759 RVA: 0x0009CCDC File Offset: 0x0009B0DC
	public IEnumerator RegisterAchievement(Achievement achievement, Action<ExtendedServerResponse> callback)
	{
		int num = (int)achievement;
		string achievementIdString = num.ToString();
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "id", achievementIdString } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.RegisterAchievement, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001298 RID: 4760 RVA: 0x0009CD08 File Offset: 0x0009B108
	public IEnumerator LoadArea(string areaId, string areaUrlName, bool isTransit, Action<LoadArea_Response> callback)
	{
		Log.Info("Request - LoadArea", false);
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		if (!string.IsNullOrEmpty(areaId))
		{
			keyValuePairs.Add("areaId", areaId);
		}
		if (!string.IsNullOrEmpty(areaUrlName))
		{
			keyValuePairs.Add("areaUrlName", areaUrlName);
		}
		keyValuePairs.Add("isPrivate", isTransit.ToString());
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.LoadArea, HTTPVerb.POST, body);
		yield return www;
		LoadArea_Response response = new LoadArea_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001299 RID: 4761 RVA: 0x0009CD40 File Offset: 0x0009B140
	public IEnumerator GetAreaInfo(string areaId, Action<GetAreaInfo_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "areaId", areaId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetAreaInfo, HTTPVerb.POST, body);
		yield return www;
		GetAreaInfo_Response response = new GetAreaInfo_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x0600129A RID: 4762 RVA: 0x0009CD6C File Offset: 0x0009B16C
	public IEnumerator GetRandomArea(Action<GetRandomArea_Response> callback)
	{
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetRandomArea, HTTPVerb.GET, null);
		yield return www;
		GetRandomArea_Response response = new GetRandomArea_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x0600129B RID: 4763 RVA: 0x0009CD90 File Offset: 0x0009B190
	public IEnumerator CreateArea(string name, Action<CreateArea_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "name", name } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.CreateArea, HTTPVerb.POST, body);
		yield return www;
		CreateArea_Response response = new CreateArea_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x0600129C RID: 4764 RVA: 0x0009CDBC File Offset: 0x0009B1BC
	public IEnumerator UpdateAreaEnvironmentChanger(string areaId, EnvironmentChangerData environmentChanger, Action<ExtendedServerResponse> callback)
	{
		string environmentChangerJSON = JsonUtility.ToJson(environmentChanger);
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "environmentChanger", environmentChangerJSON }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateAreaSettings, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x0600129D RID: 4765 RVA: 0x0009CDEC File Offset: 0x0009B1EC
	public IEnumerator SetAreaEnvironmentType(string areaId, EnvironmentType environmentType, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{
				"environmentType",
				environmentType.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateAreaSettings, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x0600129E RID: 4766 RVA: 0x0009CE1C File Offset: 0x0009B21C
	public IEnumerator SetAreaPrivacy(string areaId, bool isPrivate, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{
				"isPrivate",
				isPrivate.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateAreaSettings, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x0600129F RID: 4767 RVA: 0x0009CE4C File Offset: 0x0009B24C
	public IEnumerator SetAreaGravity(string areaId, bool isZeroGravity, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{
				"isZeroGravity",
				isZeroGravity.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateAreaSettings, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012A0 RID: 4768 RVA: 0x0009CE7C File Offset: 0x0009B27C
	public IEnumerator SetAreaFloatingDust(string areaId, bool hasFloatingDust, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{
				"hasFloatingDust",
				hasFloatingDust.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateAreaSettings, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012A1 RID: 4769 RVA: 0x0009CEAC File Offset: 0x0009B2AC
	public IEnumerator SetAreaCopyable(string areaId, bool isCopyable, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{
				"isCopyable",
				isCopyable.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateAreaSettings, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012A2 RID: 4770 RVA: 0x0009CEDC File Offset: 0x0009B2DC
	public IEnumerator SetAreaOnlyOwnerSetsLocks(string areaId, bool onlyOwnerSetsLocks, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{
				"onlyOwnerSetsLocks",
				onlyOwnerSetsLocks.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateAreaSettings, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012A3 RID: 4771 RVA: 0x0009CF0C File Offset: 0x0009B30C
	public IEnumerator SetAreaIsExcluded(string areaId, bool isExcluded, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{
				"isExcluded",
				isExcluded.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateAreaSettings, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x0009CF3C File Offset: 0x0009B33C
	public IEnumerator UpdateAreaSettings(string areaId, string settingsJSON, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "settings", settingsJSON }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateAreaSettings, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012A5 RID: 4773 RVA: 0x0009CF6C File Offset: 0x0009B36C
	public IEnumerator SetAreaDescription(string areaId, string description, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "description", description }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateAreaSettings, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012A6 RID: 4774 RVA: 0x0009CF9C File Offset: 0x0009B39C
	public IEnumerator SetParentArea(string areaId, string parentAreaId, Action<ExtendedServerResponse> callback)
	{
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("areaId", areaId);
		if (!string.IsNullOrEmpty(parentAreaId))
		{
			keyValuePairs.Add("parentAreaId", parentAreaId);
		}
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetParentArea, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012A7 RID: 4775 RVA: 0x0009CFCC File Offset: 0x0009B3CC
	public IEnumerator GetSubAreas(string areaId, Action<GetSubAreas_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "areaId", areaId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetSubAreas, HTTPVerb.POST, body);
		yield return www;
		GetSubAreas_Response response = this.DeserialiseResponse<GetSubAreas_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x0009CFF8 File Offset: 0x0009B3F8
	public IEnumerator SetAreaEditor(string areaId, string personId, bool isEditor, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "userId", personId },
			{
				"isEditor",
				isEditor.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetAreaEditor, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x0009D030 File Offset: 0x0009B430
	public IEnumerator SetAreaListEditor(string areaId, string personId, bool isListEditor, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "userId", personId },
			{
				"isListEditor",
				isListEditor.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetAreaListEditor, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x0009D068 File Offset: 0x0009B468
	public IEnumerator LockPersonFromArea(string areaId, string personId, string reason, int? minutesOrPermanentIfNull, Action<ExtendedServerResponse> callback)
	{
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("areaId", areaId);
		keyValuePairs.Add("userId", personId);
		keyValuePairs.Add("reason", reason);
		if (minutesOrPermanentIfNull != null)
		{
			keyValuePairs.Add("minutes", minutesOrPermanentIfNull.ToString());
		}
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.LockPersonFromArea, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x0009D0A8 File Offset: 0x0009B4A8
	public IEnumerator UnlockPersonFromArea(string areaId, string personId, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "userId", personId }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UnlockPersonFromArea, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x0009D0D8 File Offset: 0x0009B4D8
	public IEnumerator GetAreaLists(int subsetSize, int setSize, Action<GetAreaLists_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{
				"subsetsize",
				subsetSize.ToString()
			},
			{
				"setsize",
				setSize.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetAreaLists, HTTPVerb.POST, body);
		yield return www;
		GetAreaLists_Response response = new GetAreaLists_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x0009D108 File Offset: 0x0009B508
	public IEnumerator SearchAreas(string term, string byCreatorName, string byCreatorId, Action<SearchAreas_Response> callback)
	{
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("term", term);
		if (!string.IsNullOrEmpty(byCreatorName))
		{
			keyValuePairs.Add("byCreatorName", byCreatorName);
		}
		else if (!string.IsNullOrEmpty(byCreatorId))
		{
			keyValuePairs.Add("byCreatorId", byCreatorId);
		}
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SearchAreas, HTTPVerb.POST, body);
		yield return www;
		SearchAreas_Response response = new SearchAreas_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012AE RID: 4782 RVA: 0x0009D140 File Offset: 0x0009B540
	public IEnumerator GetAreaFlagStatus(string areaId, Action<FlagStatus_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "id", areaId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetAreaFlagStatus, HTTPVerb.POST, body);
		yield return www;
		FlagStatus_Response response = new FlagStatus_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012AF RID: 4783 RVA: 0x0009D16C File Offset: 0x0009B56C
	public IEnumerator ToggleAreaFlag(string areaId, string reason, Action<FlagStatus_Response> callback)
	{
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("id", areaId);
		if (!string.IsNullOrEmpty(reason))
		{
			keyValuePairs.Add("reason", reason);
		}
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.ToggleAreaFlag, HTTPVerb.POST, body);
		yield return www;
		FlagStatus_Response response = new FlagStatus_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012B0 RID: 4784 RVA: 0x0009D19C File Offset: 0x0009B59C
	public IEnumerator RenameArea(string areaId, string newName, Action<RenameArea_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "name", newName }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.RenameArea, HTTPVerb.POST, body);
		yield return www;
		RenameArea_Response response = new RenameArea_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012B1 RID: 4785 RVA: 0x0009D1CC File Offset: 0x0009B5CC
	public IEnumerator SetFavoriteArea(string areaId, bool isFavorited, Action<SetFavoriteArea_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{
				"favorited",
				isFavorited.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetFavoriteArea, HTTPVerb.POST, body);
		yield return www;
		SetFavoriteArea_Response response = this.DeserialiseResponse<SetFavoriteArea_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012B2 RID: 4786 RVA: 0x0009D1FC File Offset: 0x0009B5FC
	public IEnumerator SetHomeArea(string areaId, Action<ResponseBase> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "areaId", areaId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetHomeArea, HTTPVerb.POST, body);
		yield return www;
		ResponseBase response = this.DeserialiseResponse<ResponseBase>(www);
		callback(response);
		yield break;
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x060012B3 RID: 4787 RVA: 0x0009D225 File Offset: 0x0009B625
	// (set) Token: 0x060012B4 RID: 4788 RVA: 0x0009D22D File Offset: 0x0009B62D
	public string SteamAuthSessionTicket { get; private set; }

	// Token: 0x060012B5 RID: 4789 RVA: 0x0009D236 File Offset: 0x0009B636
	public void StartAuthentication()
	{
		this.SetSteamAuthSessionTicket();
		base.StartCoroutine(this.StartAuthenticatedSession(delegate(bool completedOk, StartInfo startInfo)
		{
			if (completedOk)
			{
				this.processStartInfo(startInfo);
			}
			else
			{
				Debug.LogError("StartAuthenticatedSession_Steam called back NOT ok");
				this.failMessage = "Couldn't start server session [Authentication issue]";
				this.status = ManagerStatus.Failed;
			}
		}));
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x0009D258 File Offset: 0x0009B658
	private void processStartInfo(StartInfo startInfo)
	{
		if (Misc.ForceUpdateIsNeeded(startInfo.versionMajorServerAndClient))
		{
			Misc.ForceUpdate();
		}
		Universe.versionMinorServerOnly = startInfo.versionMinorServerOnly;
		Log.Info("Universe server version " + Universe.GetServerVersionDisplay(), false);
		Managers.personManager.InitializeOurPerson(startInfo);
		Managers.achievementManager.InitializeAchievements(startInfo.achievements);
		this.status = ManagerStatus.Started;
		Managers.pollManager.StartPolling();
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x0009D2C6 File Offset: 0x0009B6C6
	public void TryReauthenticate()
	{
		Managers.pollManager.StopPolling();
		this.SetSteamAuthSessionTicket();
		base.StartCoroutine(this.StartAuthenticatedSession(delegate(bool completedOk, StartInfo startInfo)
		{
			if (completedOk)
			{
				Debug.Log("Reauthentication successful");
				Managers.pollManager.StartPolling();
			}
			else
			{
				Debug.LogError("Reauthentication failed");
				Managers.errorManager.ShowCriticalHaltError("Can't re-start server session", true, false, true, false, false);
			}
		}));
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x0009D304 File Offset: 0x0009B704
	public void CancelAuthentication()
	{
		if (this.authProvider == AuthenticationProvider.STEAM)
		{
			this.CancelAuthentication_Steam();
		}
		else
		{
			if (this.authProvider != AuthenticationProvider.ANDROID)
			{
				throw new Exception("Unsuported authentication provider : " + this.authProvider.ToString());
			}
			this.CancelAuthentication_Android();
		}
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x0009D35F File Offset: 0x0009B75F
	public void CancelAuthentication_Steam()
	{
		this.ServerAuthSessionToken = null;
		this.SteamAuthSessionTicket = null;
		this.authenticated = false;
	}

	// Token: 0x060012BA RID: 4794 RVA: 0x0009D376 File Offset: 0x0009B776
	public void CancelAuthentication_Android()
	{
		this.ServerAuthSessionToken = null;
		this.SteamAuthSessionTicket = null;
		this.authenticated = false;
	}

	// Token: 0x060012BB RID: 4795 RVA: 0x0009D390 File Offset: 0x0009B790
	private void SetSteamAuthSessionTicket()
	{
		if (Misc.ShouldBypassAuth() || (Application.isEditor && this.doBypassAuth))
		{
			this.SteamAuthSessionTicket = "bypass";
		}
		else
		{
			this.SteamAuthSessionTicket = Managers.steamManager.GetAuthSessionTicket();
		}
		if (this.SteamAuthSessionTicket == null)
		{
			Managers.errorManager.ShowCriticalHaltError("Can't start server session [Autentication session ticket error]", true, false, true, false, false);
		}
	}

	// Token: 0x060012BC RID: 4796 RVA: 0x0009D3FC File Offset: 0x0009B7FC
	private IEnumerator StartAuthenticatedSession(Action<bool, StartInfo> callback)
	{
		WWWForm wwwForm = new WWWForm();
		wwwForm.AddField("authProvider", this.authProvider.ToString());
		wwwForm.AddField("ast", this.SteamAuthSessionTicket ?? string.Empty);
		wwwForm.AddField("os", SystemInfo.operatingSystem);
		wwwForm.AddField("vrModelName", (XRDevice.model == null) ? string.Empty : XRDevice.model);
		wwwForm.AddField("clientVersion", Universe.GetClientVersionDisplay());
		Dictionary<string, string> headers = wwwForm.headers;
		byte[] rawBodyData = wwwForm.data;
		string url = this.serverBaseUrl + ServerURLs.StartAuthenticatedSession;
		WWW www = new WWW(url, rawBodyData, headers);
		Log.Info("Calling auth", false);
		yield return www;
		if (www.error != null)
		{
			Managers.steamManager.CancelCurrentAuthSessionTicket();
			int responseCode = Misc.getResponseCode(www);
			if (Misc.isServerDown(responseCode))
			{
				Managers.errorManager.ShowMaintenanceMessage();
			}
			else if (responseCode == 0)
			{
				Managers.errorManager.ShowCriticalHaltError("Unable to contact server.  This may be a problem with the servers, or your internet connection.", true, false, true, false, false);
			}
			else
			{
				Managers.errorManager.ShowCriticalHaltError("Can't start server session [HTTP Error " + responseCode + "]", true, false, true, false, false);
			}
		}
		else
		{
			StartAuthenticatedSession_Response startAuthenticatedSession_Response = new StartAuthenticatedSession_Response(www);
			if (startAuthenticatedSession_Response.isHardBanned)
			{
				Managers.errorManager.ShowCriticalHaltError("Sorry, this account is locked for terms or etiquette reasons. Please have a look at anyland.com/info and contact us if needed. Thanks!", true, true, true, false, false);
			}
			else if (!string.IsNullOrEmpty(startAuthenticatedSession_Response.steamErrorCode))
			{
				if (startAuthenticatedSession_Response.steamErrorCode == "100")
				{
					Managers.errorManager.ShowCriticalHaltError("Steam reports 'User Offline'. Please check that you are online with Steam (or try logging out then logging back in again).", true, false, true, false, false);
				}
				else if (startAuthenticatedSession_Response.steamErrorCode == "comms_err")
				{
					Managers.errorManager.ShowCriticalHaltError("Could not communicate with steam servers during authentication", true, false, true, false, false);
				}
				else
				{
					Managers.errorManager.ShowCriticalHaltError("Steam authentication request failed [" + startAuthenticatedSession_Response.steamErrorCode + "]", true, false, true, false, false);
				}
			}
			else
			{
				this.ProcessSessionCookie(www.responseHeaders);
				this.authenticated = true;
				callback(true, startAuthenticatedSession_Response.startInfo);
			}
		}
		yield break;
	}

	// Token: 0x060012BD RID: 4797 RVA: 0x0009D420 File Offset: 0x0009B820
	public IEnumerator EndAuthenticatedSession()
	{
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.EndAuthenticatedSession, HTTPVerb.GET, null);
		yield return www;
		if (www.error != null)
		{
			throw new Exception(www.error);
		}
		Debug.Log("Ended authenticated session");
		yield break;
	}

	// Token: 0x060012BE RID: 4798 RVA: 0x0009D43C File Offset: 0x0009B83C
	private void ProcessSessionCookie(Dictionary<string, string> responseHeaders)
	{
		if (!responseHeaders.ContainsKey("SET-COOKIE"))
		{
			Managers.errorManager.ShowCriticalHaltError("Can't start server session [Missing header]", true, false, true, false, false);
		}
		string text = responseHeaders["SET-COOKIE"];
		this.ServerAuthSessionToken = text.Split(new char[] { '=', ';' })[1];
		if (string.IsNullOrEmpty(this.ServerAuthSessionToken))
		{
			Managers.errorManager.ShowCriticalHaltError("Can't start server session [Empty header]", true, false, true, false, false);
		}
		Log.Info("ServerAuthSessionToken is SET: " + this.ServerAuthSessionToken, false);
	}

	// Token: 0x060012BF RID: 4799 RVA: 0x0009D4D4 File Offset: 0x0009B8D4
	public IEnumerator StartEditToolsTrial(Action<StartEditToolsTrial_Response> callback)
	{
		byte[] body = new byte[1];
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.StartEditToolsTrial, HTTPVerb.POST, body);
		yield return www;
		StartEditToolsTrial_Response response = this.DeserialiseResponse<StartEditToolsTrial_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012C0 RID: 4800 RVA: 0x0009D4F8 File Offset: 0x0009B8F8
	public IEnumerator StartEditToolsPurchase(Action<StartEditToolsPurchase_Response> callback)
	{
		byte[] body = new byte[1];
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.StartEditToolsPurchase, HTTPVerb.POST, body);
		yield return www;
		StartEditToolsPurchase_Response response = this.DeserialiseResponse<StartEditToolsPurchase_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012C1 RID: 4801 RVA: 0x0009D51C File Offset: 0x0009B91C
	public IEnumerator StartPurchase(TransactionType transactionType, string objectReferenceId, Action<StartPurchase_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{
				"transactionType",
				transactionType.ToString()
			},
			{ "objectReferenceId", objectReferenceId }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.StartPurchase, HTTPVerb.POST, body);
		yield return www;
		StartPurchase_Response response = this.DeserialiseResponse<StartPurchase_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012C2 RID: 4802 RVA: 0x0009D54C File Offset: 0x0009B94C
	public IEnumerator CompletePurchase(string ourSteamTransactionId, Action<CompletePurchase_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "transactionId", ourSteamTransactionId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.CompletePurchase, HTTPVerb.POST, body);
		yield return www;
		CompletePurchase_Response response = this.DeserialiseResponse<CompletePurchase_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x0009D578 File Offset: 0x0009B978
	public IEnumerator CancelPurchase(string ourSteamTransactionId, Action<CancelPurchase_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "transactionId", ourSteamTransactionId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.CancelPurchase, HTTPVerb.POST, body);
		yield return www;
		CancelPurchase_Response response = this.DeserialiseResponse<CancelPurchase_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x0009D5A4 File Offset: 0x0009B9A4
	public IEnumerator SubmitGift(string toUserId, string thingId, Vector3 position, Vector3 rotation, bool isPrivate, Action<SubmitGift_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "toUserId", toUserId },
			{ "thingId", thingId },
			{
				"pX",
				position.x.ToString()
			},
			{
				"pY",
				position.y.ToString()
			},
			{
				"pZ",
				position.z.ToString()
			},
			{
				"rX",
				rotation.x.ToString()
			},
			{
				"rY",
				rotation.y.ToString()
			},
			{
				"rZ",
				rotation.z.ToString()
			},
			{
				"isPrivate",
				isPrivate.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SubmitGift, HTTPVerb.POST, body);
		yield return www;
		SubmitGift_Response response = this.DeserialiseResponse<SubmitGift_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x0009D5EC File Offset: 0x0009B9EC
	public IEnumerator GetReceivedGifts(string userId, Action<GetReceivedGifts_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "userId", userId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetReceivedGifts, HTTPVerb.POST, body);
		yield return www;
		GetReceivedGifts_Response response = this.DeserialiseResponse<GetReceivedGifts_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012C6 RID: 4806 RVA: 0x0009D618 File Offset: 0x0009BA18
	public IEnumerator ToggleGiftPrivacy(string giftId, Action<ToggleGiftPrivacy_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "giftId", giftId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.ToggleGiftPrivacy, HTTPVerb.POST, body);
		yield return www;
		ToggleGiftPrivacy_Response response = this.DeserialiseResponse<ToggleGiftPrivacy_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012C7 RID: 4807 RVA: 0x0009D644 File Offset: 0x0009BA44
	public IEnumerator MarkGiftSeen(string giftId, Action<AcknowledgeOperation_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "giftId", giftId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.MarkGiftSeen, HTTPVerb.POST, body);
		yield return www;
		AcknowledgeOperation_Response response = this.DeserialiseResponse<AcknowledgeOperation_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012C8 RID: 4808 RVA: 0x0009D670 File Offset: 0x0009BA70
	public IEnumerator CreateForum(string forumName, string forumDescription, Action<CreateForum_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "name", forumName },
			{ "description", forumDescription }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.CreateForum, HTTPVerb.POST, body);
		yield return www;
		CreateForum_Response response = this.DeserialiseResponse<CreateForum_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012C9 RID: 4809 RVA: 0x0009D6A0 File Offset: 0x0009BAA0
	public IEnumerator GetForum(string forumId, Action<GetForum_Response> callback)
	{
		string relUrl = ServerURLs.GetForum + "/" + forumId;
		WWW www = this.getAuthenticatedWWWRequest(relUrl, HTTPVerb.GET, null);
		yield return www;
		GetForum_Response response = this.DeserialiseResponse<GetForum_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012CA RID: 4810 RVA: 0x0009D6CC File Offset: 0x0009BACC
	public IEnumerator GetForumInfo(string forumId, Action<GetForumInfo_Response> callback)
	{
		string relUrl = ServerURLs.GetForumInfo + "/" + forumId;
		WWW www = this.getAuthenticatedWWWRequest(relUrl, HTTPVerb.GET, null);
		yield return www;
		GetForumInfo_Response response = this.DeserialiseResponse<GetForumInfo_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012CB RID: 4811 RVA: 0x0009D6F8 File Offset: 0x0009BAF8
	public IEnumerator GetForumIdFromName(string forumName, Action<GetForumIdFromName_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "forumName", forumName } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetForumIdFromName, HTTPVerb.POST, body);
		yield return www;
		GetForumIdFromName_Response response = this.DeserialiseResponse<GetForumIdFromName_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012CC RID: 4812 RVA: 0x0009D724 File Offset: 0x0009BB24
	public IEnumerator GetForumThread(string threadId, Action<GetForumThread_Response> callback)
	{
		string relUrl = ServerURLs.GetForumThread + "/" + threadId;
		WWW www = this.getAuthenticatedWWWRequest(relUrl, HTTPVerb.GET, null);
		yield return www;
		GetForumThread_Response response = this.DeserialiseResponse<GetForumThread_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012CD RID: 4813 RVA: 0x0009D750 File Offset: 0x0009BB50
	public IEnumerator GetForumThreadInfo(string threadId, Action<GetForumThreadInfo_Response> callback)
	{
		string relUrl = ServerURLs.GetForumThreadInfo + "/" + threadId;
		WWW www = this.getAuthenticatedWWWRequest(relUrl, HTTPVerb.GET, null);
		yield return www;
		GetForumThreadInfo_Response response = this.DeserialiseResponse<GetForumThreadInfo_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012CE RID: 4814 RVA: 0x0009D77C File Offset: 0x0009BB7C
	public IEnumerator AddForumThread(string forumId, string threadTitle, string commentText, string optionalThingId, Action<AddForumThread_Response> callback)
	{
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("forumId", forumId);
		keyValuePairs.Add("threadTitle", threadTitle);
		keyValuePairs.Add("commentText", commentText);
		if (!string.IsNullOrEmpty(optionalThingId))
		{
			keyValuePairs.Add("thingId", optionalThingId);
		}
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.AddForumThread, HTTPVerb.POST, body);
		yield return www;
		AddForumThread_Response response = this.DeserialiseResponse<AddForumThread_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012CF RID: 4815 RVA: 0x0009D7BC File Offset: 0x0009BBBC
	public IEnumerator RemoveForumThread(string threadId, Action<RemoveForumThread_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "threadId", threadId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.RemoveForumThread, HTTPVerb.POST, body);
		yield return www;
		RemoveForumThread_Response response = this.DeserialiseResponse<RemoveForumThread_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012D0 RID: 4816 RVA: 0x0009D7E8 File Offset: 0x0009BBE8
	public IEnumerator AddForumComment(string threadId, string commentText, string optionalThingId, Action<AddForumComment_Response> callback)
	{
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("threadId", threadId);
		keyValuePairs.Add("commentText", commentText);
		if (!string.IsNullOrEmpty(optionalThingId))
		{
			keyValuePairs.Add("thingId", optionalThingId);
		}
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.AddForumComment, HTTPVerb.POST, body);
		yield return www;
		AddForumComment_Response response = this.DeserialiseResponse<AddForumComment_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012D1 RID: 4817 RVA: 0x0009D820 File Offset: 0x0009BC20
	public IEnumerator EditForumComment(string threadId, string commentDate, string newCommentText, Action<EditForumComment_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "threadId", threadId },
			{ "commentDate", commentDate },
			{ "newCommentText", newCommentText }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.EditForumComment, HTTPVerb.POST, body);
		yield return www;
		EditForumComment_Response response = this.DeserialiseResponse<EditForumComment_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012D2 RID: 4818 RVA: 0x0009D858 File Offset: 0x0009BC58
	public IEnumerator RemoveForumComment(string threadId, string commentUserId, string commentDate, Action<RemoveForumComment_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "threadId", threadId },
			{ "commentUserId", commentUserId },
			{ "commentDate", commentDate }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.RemoveForumComment, HTTPVerb.POST, body);
		yield return www;
		RemoveForumComment_Response response = this.DeserialiseResponse<RemoveForumComment_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012D3 RID: 4819 RVA: 0x0009D890 File Offset: 0x0009BC90
	public IEnumerator LikeForumComment(string threadId, string commentUserId, string commentDate, Action<AcknowledgeOperation_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "threadId", threadId },
			{ "commentUserId", commentUserId },
			{ "commentDate", commentDate }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.LikeForumComment, HTTPVerb.POST, body);
		yield return www;
		AcknowledgeOperation_Response response = this.DeserialiseResponse<AcknowledgeOperation_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012D4 RID: 4820 RVA: 0x0009D8C8 File Offset: 0x0009BCC8
	public IEnumerator ToggleForumThreadSticky(string threadId, Action<ToggleForumThreadSticky_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "threadId", threadId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.ToggleForumThreadSticky, HTTPVerb.POST, body);
		yield return www;
		ToggleForumThreadSticky_Response response = this.DeserialiseResponse<ToggleForumThreadSticky_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012D5 RID: 4821 RVA: 0x0009D8F4 File Offset: 0x0009BCF4
	public IEnumerator ToggleForumThreadLocked(string threadId, Action<ToggleForumThreadLocked_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "threadId", threadId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.ToggleForumThreadLocked, HTTPVerb.POST, body);
		yield return www;
		ToggleForumThreadLocked_Response response = this.DeserialiseResponse<ToggleForumThreadLocked_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012D6 RID: 4822 RVA: 0x0009D920 File Offset: 0x0009BD20
	public IEnumerator ClarifyForumThreadTitle(string threadId, string titleClarification, Action<ClarifyForumThreadTitle_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "threadId", threadId },
			{ "clarification", titleClarification }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.ClarifyForumThreadTitle, HTTPVerb.POST, body);
		yield return www;
		ClarifyForumThreadTitle_Response response = this.DeserialiseResponse<ClarifyForumThreadTitle_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012D7 RID: 4823 RVA: 0x0009D950 File Offset: 0x0009BD50
	public IEnumerator GetFavoriteForums(Action<GetFavoriteForums_Response> callback)
	{
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetFavoriteForums, HTTPVerb.GET, null);
		yield return www;
		GetFavoriteForums_Response response = this.DeserialiseResponse<GetFavoriteForums_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012D8 RID: 4824 RVA: 0x0009D974 File Offset: 0x0009BD74
	public IEnumerator ToggleFavoriteForum(string forumId, Action<ToggleFavoriteForum_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "forumId", forumId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.ToggleFavoriteForum, HTTPVerb.POST, body);
		yield return www;
		ToggleFavoriteForum_Response response = this.DeserialiseResponse<ToggleFavoriteForum_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012D9 RID: 4825 RVA: 0x0009D9A0 File Offset: 0x0009BDA0
	public IEnumerator SetForumProtectionLevel(string forumId, int protectionLevel, Action<SetForumProtectionLevel_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "forumId", forumId },
			{
				"protectionLevel",
				protectionLevel.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetForumProtectionLevel, HTTPVerb.POST, body);
		yield return www;
		SetForumProtectionLevel_Response response = this.DeserialiseResponse<SetForumProtectionLevel_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012DA RID: 4826 RVA: 0x0009D9D0 File Offset: 0x0009BDD0
	public IEnumerator EditForumInfo(string forumId, string description, Action<EditForumInfo_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "forumId", forumId },
			{ "description", description }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.EditForumInfo, HTTPVerb.POST, body);
		yield return www;
		EditForumInfo_Response response = this.DeserialiseResponse<EditForumInfo_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012DB RID: 4827 RVA: 0x0009DA00 File Offset: 0x0009BE00
	public IEnumerator SetForumDialog(string forumId, string thingId, string color, Action<SetForumDialog_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "forumId", forumId },
			{ "thingId", thingId },
			{ "color", color }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetForumDialog, HTTPVerb.POST, body);
		yield return www;
		SetForumDialog_Response response = this.DeserialiseResponse<SetForumDialog_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012DC RID: 4828 RVA: 0x0009DA38 File Offset: 0x0009BE38
	public IEnumerator SearchForums(string searchTerm, Action<SearchForums_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "searchTerm", searchTerm } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SearchForums, HTTPVerb.POST, body);
		yield return www;
		SearchForums_Response response = this.DeserialiseResponse<SearchForums_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012DD RID: 4829 RVA: 0x0009DA64 File Offset: 0x0009BE64
	public IEnumerator SearchForumThreads(string forumId, string searchTerm, Action<SearchForumThreads_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "forumId", forumId },
			{ "searchTerm", searchTerm }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SearchForumThreads, HTTPVerb.POST, body);
		yield return www;
		SearchForumThreads_Response response = this.DeserialiseResponse<SearchForumThreads_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012DE RID: 4830 RVA: 0x0009DA94 File Offset: 0x0009BE94
	public IEnumerator PollServer(string currentAreaId, Action<PollServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "areaId", currentAreaId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.PollServer, HTTPVerb.POST, body);
		yield return www;
		PollServerResponse response = new PollServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x0009DAC0 File Offset: 0x0009BEC0
	public IEnumerator LoadInventory(int page, Action<LoadInventory_Response> callback)
	{
		string relUrl = ServerURLs.LoadInventory + "/" + page;
		WWW www = this.getAuthenticatedWWWRequest(relUrl, HTTPVerb.GET, null);
		yield return www;
		LoadInventory_Response response = new LoadInventory_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x0009DAEC File Offset: 0x0009BEEC
	public IEnumerator SaveThingToInventory(int page, InventoryItemData inventoryItem, Action<ExtendedServerResponse> callback)
	{
		string inventoryItemJSON = JsonUtility.ToJson(inventoryItem);
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{
				"page",
				page.ToString()
			},
			{ "inventoryItem", inventoryItemJSON }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SaveInventoryItem, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012E1 RID: 4833 RVA: 0x0009DB1C File Offset: 0x0009BF1C
	public IEnumerator UpdateThingInInventory(int page, InventoryItemData inventoryItem, Action<ExtendedServerResponse> callback)
	{
		string inventoryItemJSON = JsonUtility.ToJson(inventoryItem);
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{
				"page",
				page.ToString()
			},
			{ "inventoryItem", inventoryItemJSON }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateInventoryItem, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x0009DB4C File Offset: 0x0009BF4C
	public IEnumerator DeleteThingFromInventory(int page, string thingId, Action<ServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{
				"page",
				page.ToString()
			},
			{ "thingId", thingId }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.DeleteInventoryItem, HTTPVerb.POST, body);
		yield return www;
		ServerResponse response = new ServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x0009DB7C File Offset: 0x0009BF7C
	public IEnumerator AddFriend(string personId, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "id", personId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.AddFriend, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x0009DBA8 File Offset: 0x0009BFA8
	public IEnumerator RemoveFriend(string personId, Action<ServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "id", personId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.RemoveFriend, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012E5 RID: 4837 RVA: 0x0009DBD4 File Offset: 0x0009BFD4
	public IEnumerator IncreaseFriendshipStrength(string personId, Action<ServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "id", personId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.IncreaseFriendshipStrength, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x0009DC00 File Offset: 0x0009C000
	public IEnumerator GetFriends(Action<GetFriends_Response> callback)
	{
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetFriends, HTTPVerb.GET, null);
		yield return www;
		GetFriends_Response response = new GetFriends_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012E7 RID: 4839 RVA: 0x0009DC24 File Offset: 0x0009C024
	public IEnumerator GetFriendsByStrength(Action<GetFriendsByStrength_Response> callback)
	{
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetFriendsByStrength, HTTPVerb.GET, null);
		yield return www;
		GetFriendsByStrength_Response response = new GetFriendsByStrength_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x0009DC48 File Offset: 0x0009C048
	public IEnumerator UpdatePersonalSetting(string settingName, string settingValue, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "name", settingName },
			{ "value", settingValue }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdatePersonalSetting, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012E9 RID: 4841 RVA: 0x0009DC78 File Offset: 0x0009C078
	public IEnumerator UpdateAttachment(AttachmentPointId attachmentPointId, AttachmentData attachmentData, Action<ServerResponse> callback)
	{
		string attachmentDataJSON = JsonUtility.ToJson(attachmentData);
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		Dictionary<string, string> dictionary = keyValuePairs;
		string text = "id";
		int num = (int)attachmentPointId;
		dictionary.Add(text, num.ToString());
		keyValuePairs.Add("data", attachmentDataJSON);
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdateAttachment, HTTPVerb.POST, body);
		yield return www;
		ServerResponse response = new ServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012EA RID: 4842 RVA: 0x0009DCA8 File Offset: 0x0009C0A8
	public IEnumerator GetPersonInfo(string personId, string areaId, Action<GetPersonInfo_Response> callback)
	{
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("userId", personId);
		if (!string.IsNullOrEmpty(areaId))
		{
			keyValuePairs.Add("areaId", areaId);
		}
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetPersonInfo, HTTPVerb.POST, body);
		yield return www;
		GetPersonInfo_Response response = new GetPersonInfo_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012EB RID: 4843 RVA: 0x0009DCD8 File Offset: 0x0009C0D8
	public IEnumerator GetPersonInfoBasic(string personId, string areaId, Action<GetPersonInfoBasic_Response> callback)
	{
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("userId", personId);
		if (!string.IsNullOrEmpty(areaId))
		{
			keyValuePairs.Add("areaId", areaId);
		}
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetPersonInfoBasic, HTTPVerb.POST, body);
		yield return www;
		GetPersonInfoBasic_Response response = new GetPersonInfoBasic_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012EC RID: 4844 RVA: 0x0009DD08 File Offset: 0x0009C108
	public IEnumerator SetHandColor(Color handColor, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{
				"r",
				handColor.r.ToString()
			},
			{
				"g",
				handColor.g.ToString()
			},
			{
				"b",
				handColor.b.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetHandColor, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012ED RID: 4845 RVA: 0x0009DD34 File Offset: 0x0009C134
	public IEnumerator GetPersonFlagStatus(string personId, Action<FlagStatus_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "id", personId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetPersonFlagStatus, HTTPVerb.POST, body);
		yield return www;
		FlagStatus_Response response = new FlagStatus_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012EE RID: 4846 RVA: 0x0009DD60 File Offset: 0x0009C160
	public IEnumerator TogglePersonFlag(string personId, string reason, Action<FlagStatus_Response> callback)
	{
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("id", personId);
		if (!string.IsNullOrEmpty(reason))
		{
			keyValuePairs.Add("reason", reason);
		}
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.TogglePersonFlag, HTTPVerb.POST, body);
		yield return www;
		FlagStatus_Response response = new FlagStatus_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012EF RID: 4847 RVA: 0x0009DD90 File Offset: 0x0009C190
	public IEnumerator PingPerson(string userId, string areaId, Action<ExtendedServerResponse> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "userId", userId },
			{ "areaId", areaId }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.PingPerson, HTTPVerb.POST, body);
		yield return www;
		ExtendedServerResponse response = new ExtendedServerResponse(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012F0 RID: 4848 RVA: 0x0009DDC0 File Offset: 0x0009C1C0
	public IEnumerator RegisterUsageMode(bool inDesktopMode, Action<ResponseBase> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { 
		{
			"inDesktopMode",
			inDesktopMode.ToString()
		} });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.RegisterUsageMode, HTTPVerb.POST, body);
		yield return www;
		ResponseBase response = this.DeserialiseResponse<ResponseBase>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012F1 RID: 4849 RVA: 0x0009DDEC File Offset: 0x0009C1EC
	public IEnumerator RegisterHold(string thingId, Side handSide, HoldGeometryData optionalHoldGeometryData, bool inDesktopMode, Action<ResponseBase> callback)
	{
		string holdGeometryDataJSON = JsonUtility.ToJson(optionalHoldGeometryData);
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("thingId", thingId);
		Dictionary<string, string> dictionary = keyValuePairs;
		string text = "handSide";
		int num = (int)handSide;
		dictionary.Add(text, num.ToString());
		keyValuePairs.Add("geometry", holdGeometryDataJSON);
		keyValuePairs.Add("inDesktopMode", inDesktopMode.ToString());
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.RegisterHold, HTTPVerb.POST, body);
		yield return www;
		ResponseBase response = this.DeserialiseResponse<ResponseBase>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012F2 RID: 4850 RVA: 0x0009DE2C File Offset: 0x0009C22C
	public IEnumerator GetHoldGeometry(string thingId, Side handSide, Action<GetHoldGeometry_Response> callback)
	{
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("thingId", thingId);
		Dictionary<string, string> dictionary = keyValuePairs;
		string text = "handSide";
		int num = (int)handSide;
		dictionary.Add(text, num.ToString());
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetHoldGeometry, HTTPVerb.POST, body);
		yield return www;
		GetHoldGeometry_Response response = this.DeserialiseResponse<GetHoldGeometry_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012F3 RID: 4851 RVA: 0x0009DE5C File Offset: 0x0009C25C
	public IEnumerator GetQuickEquipList(Action<GetQuickEquipList_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "x", "x" } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetQuickEquipList, HTTPVerb.POST, body);
		yield return www;
		GetQuickEquipList_Response response = this.DeserialiseResponse<GetQuickEquipList_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012F4 RID: 4852 RVA: 0x0009DE80 File Offset: 0x0009C280
	public IEnumerator RequestWelcome(Action<ResponseBase> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "x", "x" } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.RequestWelcome, HTTPVerb.POST, body);
		yield return www;
		ResponseBase response = this.DeserialiseResponse<ResponseBase>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012F5 RID: 4853 RVA: 0x0009DEA4 File Offset: 0x0009C2A4
	public IEnumerator SetCustomSearchWords(string words, Action<ResponseBase> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "words", words } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetCustomSearchWords, HTTPVerb.POST, body);
		yield return www;
		ResponseBase response = this.DeserialiseResponse<ResponseBase>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012F6 RID: 4854 RVA: 0x0009DED0 File Offset: 0x0009C2D0
	public IEnumerator NewPlacement(string areaId, PlacementData placement, Action<Placement_Response> callback)
	{
		string placementJSON = JsonUtility.ToJson(placement);
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "placement", placementJSON }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.NewPlacement, HTTPVerb.POST, body);
		yield return www;
		Placement_Response response = new Placement_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012F7 RID: 4855 RVA: 0x0009DF00 File Offset: 0x0009C300
	public IEnumerator UpdatePlacement(string areaId, PlacementData placement, Action<Placement_Response> callback)
	{
		string placementJSON = JsonUtility.ToJson(placement);
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "placement", placementJSON }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.UpdatePlacement, HTTPVerb.POST, body);
		yield return www;
		Placement_Response response = new Placement_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012F8 RID: 4856 RVA: 0x0009DF30 File Offset: 0x0009C330
	public IEnumerator DeletePlacement(string areaId, string placementId, Action<Placement_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "placementId", placementId }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.DeletePlacement, HTTPVerb.POST, body);
		yield return www;
		Placement_Response response = new Placement_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012F9 RID: 4857 RVA: 0x0009DF60 File Offset: 0x0009C360
	public IEnumerator DeleteAllPlacements(string areaId, Action<AcknowledgeOperation_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "areaId", areaId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.DeleteAllPlacements, HTTPVerb.POST, body);
		yield return www;
		AcknowledgeOperation_Response response = this.DeserialiseResponse<AcknowledgeOperation_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012FA RID: 4858 RVA: 0x0009DF8C File Offset: 0x0009C38C
	public IEnumerator ReplaceAllOccurrencesOfThing(string areaId, string originalThingId, string newThingId, Action<AcknowledgeOperation_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "originalThingId", originalThingId },
			{ "newThingId", newThingId }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.ReplaceAllOccurrencesOfThing, HTTPVerb.POST, body);
		yield return www;
		AcknowledgeOperation_Response response = this.DeserialiseResponse<AcknowledgeOperation_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x0009DFC4 File Offset: 0x0009C3C4
	public IEnumerator GetPlacementInfo(string areaId, string placementId, Action<GetPlacementInfo_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "placementId", placementId }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetPlacementInfo, HTTPVerb.POST, body);
		yield return www;
		GetPlacementInfo_Response response = new GetPlacementInfo_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x0009DFF4 File Offset: 0x0009C3F4
	public IEnumerator SetPlacementAttribute(string areaId, string placementId, PlacementAttribute attribute, Action<ResponseBase> callback)
	{
		int num = (int)attribute;
		string attributeIdString = num.ToString();
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "placementId", placementId },
			{ "attribute", attributeIdString }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetPlacementAttribute, HTTPVerb.POST, body);
		yield return www;
		ResponseBase response = this.DeserialiseResponse<ResponseBase>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x0009E02C File Offset: 0x0009C42C
	public IEnumerator ClearPlacementAttribute(string areaId, string placementId, PlacementAttribute attribute, Action<ResponseBase> callback)
	{
		int num = (int)attribute;
		string attributeIdString = num.ToString();
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "areaId", areaId },
			{ "placementId", placementId },
			{ "attribute", attributeIdString }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.ClearPlacementAttribute, HTTPVerb.POST, body);
		yield return www;
		ResponseBase response = this.DeserialiseResponse<ResponseBase>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x0009E064 File Offset: 0x0009C464
	public IEnumerator GetThingIdsInAreaByOtherCreators(string areaId, Action<GetThingIdsInAreaByOtherCreators_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "areaId", areaId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetThingIdsInAreaByOtherCreators, HTTPVerb.POST, body);
		yield return www;
		GetThingIdsInAreaByOtherCreators_Response response = this.DeserialiseResponse<GetThingIdsInAreaByOtherCreators_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x0009E090 File Offset: 0x0009C490
	public IEnumerator CopyPlacements(string sourceAreaId, string destinationAreaId, Action<AcknowledgeOperation_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "sourceAreaId", sourceAreaId },
			{ "destinationAreaId", destinationAreaId }
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.CopyPlacements, HTTPVerb.POST, body);
		yield return www;
		AcknowledgeOperation_Response response = this.DeserialiseResponse<AcknowledgeOperation_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x0009E0C0 File Offset: 0x0009C4C0
	public IEnumerator GetThingDefinition(string id, Action<GetThingDefinition_Response> callback)
	{
		WWW www;
		if (this.doUseCDN)
		{
			string text = ServerURLs.GetThingDefinition_CDN + "/" + id;
			www = this.getNonAuthenticatedWWWRequest(text, HTTPVerb.GET, null);
		}
		else
		{
			string text2 = ServerURLs.GetThingDefinition + "/" + id;
			www = this.getAuthenticatedWWWRequest(text2, HTTPVerb.GET, null);
		}
		yield return www;
		GetThingDefinition_Response response = new GetThingDefinition_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x0009E0EC File Offset: 0x0009C4EC
	public IEnumerator GetThingDefinitionAreaBundle(string areaId, string key, Action<GetThingDefinitionAreaBundle_Response> callback)
	{
		if (string.IsNullOrEmpty(key))
		{
			key = "0";
		}
		WWW www;
		if (this.doUseCDN)
		{
			string text = string.Concat(new string[]
			{
				ServerURLs.GetThingDefinitionAreaBundle_CDN,
				"/",
				areaId,
				"/",
				key
			});
			www = this.getNonAuthenticatedWWWRequest(text, HTTPVerb.GET, null);
		}
		else
		{
			string text2 = string.Concat(new string[]
			{
				ServerURLs.GetThingDefinitionAreaBundle,
				"/",
				areaId,
				"/",
				key
			});
			www = this.getAuthenticatedWWWRequest(text2, HTTPVerb.GET, null);
		}
		yield return www;
		GetThingDefinitionAreaBundle_Response response = this.DeserialiseResponse<GetThingDefinitionAreaBundle_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x0009E11C File Offset: 0x0009C51C
	public IEnumerator GetThingInfo(string thingId, Action<GetThingInfo_Response> callback)
	{
		string relUrl = ServerURLs.GetThingInfo + "/" + thingId;
		WWW www = this.getAuthenticatedWWWRequest(relUrl, HTTPVerb.GET, null);
		yield return www;
		GetThingInfo_Response response = new GetThingInfo_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x0009E148 File Offset: 0x0009C548
	public IEnumerator SaveThing(string thingDefinitionJSON, int vertexCount, string clonedFromId, Action<SaveThing_Response> callback)
	{
		Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
		keyValuePairs.Add("definition", thingDefinitionJSON);
		keyValuePairs.Add("vertexCount", vertexCount.ToString());
		if (clonedFromId != null)
		{
			keyValuePairs.Add("clonedFromId", clonedFromId);
		}
		byte[] body = this.getPostBody(keyValuePairs);
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SaveThing, HTTPVerb.POST, body);
		yield return www;
		SaveThing_Response response = new SaveThing_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001304 RID: 4868 RVA: 0x0009E180 File Offset: 0x0009C580
	public IEnumerator GetRecentlyDeletedThingIds(Action<GetRecentlyDeletedThingIds_Response> callback)
	{
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetRecentlyDeletedThingIds, HTTPVerb.GET, null);
		yield return www;
		GetRecentlyDeletedThingIds_Response response = new GetRecentlyDeletedThingIds_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001305 RID: 4869 RVA: 0x0009E1A4 File Offset: 0x0009C5A4
	public IEnumerator GetTopThingIdsCreatedByPerson(string personId, int numberToGet, Action<GetTopThingIdsCreatedByPerson_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "id", personId },
			{
				"limit",
				numberToGet.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetTopThingIdsCreatedByPerson, HTTPVerb.POST, body);
		yield return www;
		GetTopThingIdsCreatedByPerson_Response response = new GetTopThingIdsCreatedByPerson_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001306 RID: 4870 RVA: 0x0009E1D4 File Offset: 0x0009C5D4
	public IEnumerator GetThingFlag(string thingId, Action<FlagStatus_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "id", thingId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetThingFlagStatus, HTTPVerb.POST, body);
		yield return www;
		FlagStatus_Response response = new FlagStatus_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001307 RID: 4871 RVA: 0x0009E200 File Offset: 0x0009C600
	public IEnumerator ToggleThingFlag(string thingId, Action<FlagStatus_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "id", thingId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.ToggleThingFlag, HTTPVerb.POST, body);
		yield return www;
		FlagStatus_Response response = new FlagStatus_Response(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001308 RID: 4872 RVA: 0x0009E22C File Offset: 0x0009C62C
	public IEnumerator SearchThings(string term, Action<SearchThings_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "term", term } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SearchThings, HTTPVerb.POST, body);
		yield return www;
		SearchThings_Response response = this.DeserialiseResponse<SearchThings_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x06001309 RID: 4873 RVA: 0x0009E258 File Offset: 0x0009C658
	public IEnumerator ReportMissingThing(ThingRequestContext thingRequestContext, string thingId, Action<ResponseBase> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "thingId", thingId },
			{
				"context",
				thingRequestContext.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.ReportMissingThing, HTTPVerb.POST, body);
		yield return www;
		ResponseBase response = this.DeserialiseResponse<ResponseBase>(www);
		callback(response);
		yield break;
	}

	// Token: 0x0600130A RID: 4874 RVA: 0x0009E288 File Offset: 0x0009C688
	public IEnumerator SetThingUnlisted(string thingId, bool isUnlisted, Action<ResponseBase> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "thingId", thingId },
			{
				"isUnlisted",
				isUnlisted.ToString()
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetThingUnlisted, HTTPVerb.POST, body);
		yield return www;
		ResponseBase response = this.DeserialiseResponse<ResponseBase>(www);
		callback(response);
		yield break;
	}

	// Token: 0x0600130B RID: 4875 RVA: 0x0009E2B8 File Offset: 0x0009C6B8
	public IEnumerator SetThingTags(string thingId, List<string> tagsToAdd, List<string> tagsToRemove, Action<ResponseBase> callback)
	{
		TagListWrapper tagsToAddWrapped = new TagListWrapper(tagsToAdd);
		TagListWrapper tagsToRemoveWrapped = new TagListWrapper(tagsToRemove);
		byte[] body = this.getPostBody(new Dictionary<string, string>
		{
			{ "thingId", thingId },
			{
				"tagsToAdd",
				JsonUtility.ToJson(tagsToAddWrapped)
			},
			{
				"tagsToRemove",
				JsonUtility.ToJson(tagsToRemoveWrapped)
			}
		});
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.SetThingTags, HTTPVerb.POST, body);
		yield return www;
		ResponseBase response = this.DeserialiseResponse<ResponseBase>(www);
		callback(response);
		yield break;
	}

	// Token: 0x0600130C RID: 4876 RVA: 0x0009E2F0 File Offset: 0x0009C6F0
	public IEnumerator GetThingTags(string thingId, Action<GetThingTags_Response> callback)
	{
		byte[] body = this.getPostBody(new Dictionary<string, string> { { "thingId", thingId } });
		WWW www = this.getAuthenticatedWWWRequest(ServerURLs.GetThingTags, HTTPVerb.POST, body);
		yield return www;
		GetThingTags_Response response = this.DeserialiseResponse<GetThingTags_Response>(www);
		callback(response);
		yield break;
	}

	// Token: 0x0400117B RID: 4475
	public bool UseLocalServer;

	// Token: 0x0400117C RID: 4476
	public bool UseCDN = true;

	// Token: 0x0400117D RID: 4477
	private string serverBaseUrl;

	// Token: 0x0400117E RID: 4478
	public string RemoteServerBaseUrl = "http://anyland.com";

	// Token: 0x0400117F RID: 4479
	private AuthenticationProvider authProvider;

	// Token: 0x04001181 RID: 4481
	private string ServerAuthSessionToken;

	// Token: 0x04001182 RID: 4482
	private bool authenticated;

	// Token: 0x04001183 RID: 4483
	private bool doBypassAuth;
}
