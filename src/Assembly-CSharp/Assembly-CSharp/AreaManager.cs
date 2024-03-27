using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020001E1 RID: 481
public class AreaManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06000F0F RID: 3855 RVA: 0x000839D6 File Offset: 0x00081DD6
	// (set) Token: 0x06000F10 RID: 3856 RVA: 0x000839DE File Offset: 0x00081DDE
	public ManagerStatus status { get; private set; }

	// Token: 0x170001B2 RID: 434
	// (get) Token: 0x06000F11 RID: 3857 RVA: 0x000839E7 File Offset: 0x00081DE7
	// (set) Token: 0x06000F12 RID: 3858 RVA: 0x000839EF File Offset: 0x00081DEF
	public string failMessage { get; private set; }

	// Token: 0x170001B3 RID: 435
	// (get) Token: 0x06000F13 RID: 3859 RVA: 0x000839F8 File Offset: 0x00081DF8
	// (set) Token: 0x06000F14 RID: 3860 RVA: 0x00083A00 File Offset: 0x00081E00
	public string currentAreaId { get; set; }

	// Token: 0x170001B4 RID: 436
	// (get) Token: 0x06000F15 RID: 3861 RVA: 0x00083A09 File Offset: 0x00081E09
	// (set) Token: 0x06000F16 RID: 3862 RVA: 0x00083A11 File Offset: 0x00081E11
	public string currentAreaName { get; private set; }

	// Token: 0x170001B5 RID: 437
	// (get) Token: 0x06000F17 RID: 3863 RVA: 0x00083A1A File Offset: 0x00081E1A
	// (set) Token: 0x06000F18 RID: 3864 RVA: 0x00083A22 File Offset: 0x00081E22
	public string currentAreaKey { get; private set; }

	// Token: 0x170001B6 RID: 438
	// (get) Token: 0x06000F19 RID: 3865 RVA: 0x00083A2B File Offset: 0x00081E2B
	// (set) Token: 0x06000F1A RID: 3866 RVA: 0x00083A33 File Offset: 0x00081E33
	public string currentAreaUrlName { get; private set; }

	// Token: 0x170001B7 RID: 439
	// (get) Token: 0x06000F1B RID: 3867 RVA: 0x00083A3C File Offset: 0x00081E3C
	// (set) Token: 0x06000F1C RID: 3868 RVA: 0x00083A44 File Offset: 0x00081E44
	public string currentAreaCreatorId { get; private set; }

	// Token: 0x170001B8 RID: 440
	// (get) Token: 0x06000F1D RID: 3869 RVA: 0x00083A4D File Offset: 0x00081E4D
	// (set) Token: 0x06000F1E RID: 3870 RVA: 0x00083A55 File Offset: 0x00081E55
	public bool weAreEditorOfCurrentArea { get; private set; }

	// Token: 0x170001B9 RID: 441
	// (get) Token: 0x06000F1F RID: 3871 RVA: 0x00083A5E File Offset: 0x00081E5E
	// (set) Token: 0x06000F20 RID: 3872 RVA: 0x00083A66 File Offset: 0x00081E66
	public bool weAreListEditorOfCurrentArea { get; private set; }

	// Token: 0x170001BA RID: 442
	// (get) Token: 0x06000F21 RID: 3873 RVA: 0x00083A6F File Offset: 0x00081E6F
	// (set) Token: 0x06000F22 RID: 3874 RVA: 0x00083A77 File Offset: 0x00081E77
	public bool weAreOwnerOfCurrentArea { get; private set; }

	// Token: 0x170001BB RID: 443
	// (get) Token: 0x06000F23 RID: 3875 RVA: 0x00083A80 File Offset: 0x00081E80
	// (set) Token: 0x06000F24 RID: 3876 RVA: 0x00083A88 File Offset: 0x00081E88
	public bool canCreateAreas { get; set; }

	// Token: 0x170001BC RID: 444
	// (get) Token: 0x06000F25 RID: 3877 RVA: 0x00083A91 File Offset: 0x00081E91
	// (set) Token: 0x06000F26 RID: 3878 RVA: 0x00083A99 File Offset: 0x00081E99
	public bool currentAreaIsHomeArea { get; set; }

	// Token: 0x170001BD RID: 445
	// (get) Token: 0x06000F27 RID: 3879 RVA: 0x00083AA2 File Offset: 0x00081EA2
	// (set) Token: 0x06000F28 RID: 3880 RVA: 0x00083AAA File Offset: 0x00081EAA
	public bool isPrivate { get; private set; }

	// Token: 0x170001BE RID: 446
	// (get) Token: 0x06000F29 RID: 3881 RVA: 0x00083AB3 File Offset: 0x00081EB3
	// (set) Token: 0x06000F2A RID: 3882 RVA: 0x00083ABB File Offset: 0x00081EBB
	public bool isZeroGravity { get; private set; }

	// Token: 0x170001BF RID: 447
	// (get) Token: 0x06000F2B RID: 3883 RVA: 0x00083AC4 File Offset: 0x00081EC4
	// (set) Token: 0x06000F2C RID: 3884 RVA: 0x00083ACC File Offset: 0x00081ECC
	public bool hasFloatingDust { get; private set; }

	// Token: 0x170001C0 RID: 448
	// (get) Token: 0x06000F2D RID: 3885 RVA: 0x00083AD5 File Offset: 0x00081ED5
	// (set) Token: 0x06000F2E RID: 3886 RVA: 0x00083ADD File Offset: 0x00081EDD
	public bool isCopyable { get; private set; }

	// Token: 0x170001C1 RID: 449
	// (get) Token: 0x06000F2F RID: 3887 RVA: 0x00083AE6 File Offset: 0x00081EE6
	// (set) Token: 0x06000F30 RID: 3888 RVA: 0x00083AEE File Offset: 0x00081EEE
	public bool onlyOwnerSetsLocks { get; private set; }

	// Token: 0x170001C2 RID: 450
	// (get) Token: 0x06000F31 RID: 3889 RVA: 0x00083AF7 File Offset: 0x00081EF7
	// (set) Token: 0x06000F32 RID: 3890 RVA: 0x00083AFF File Offset: 0x00081EFF
	public bool isExcluded { get; private set; }

	// Token: 0x170001C3 RID: 451
	// (get) Token: 0x06000F33 RID: 3891 RVA: 0x00083B08 File Offset: 0x00081F08
	// (set) Token: 0x06000F34 RID: 3892 RVA: 0x00083B10 File Offset: 0x00081F10
	public bool isTransportInProgress { get; private set; }

	// Token: 0x170001C4 RID: 452
	// (get) Token: 0x06000F35 RID: 3893 RVA: 0x00083B19 File Offset: 0x00081F19
	// (set) Token: 0x06000F36 RID: 3894 RVA: 0x00083B21 File Offset: 0x00081F21
	public bool didFinishLoadingPlacements { get; private set; }

	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06000F37 RID: 3895 RVA: 0x00083B2A File Offset: 0x00081F2A
	// (set) Token: 0x06000F38 RID: 3896 RVA: 0x00083B32 File Offset: 0x00081F32
	public string areaToTransportToAfterNextAreaLoad { get; private set; }

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x06000F39 RID: 3897 RVA: 0x00083B3B File Offset: 0x00081F3B
	// (set) Token: 0x06000F3A RID: 3898 RVA: 0x00083B43 File Offset: 0x00081F43
	public string lastDeletionId { get; private set; }

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x06000F3B RID: 3899 RVA: 0x00083B4C File Offset: 0x00081F4C
	// (set) Token: 0x06000F3C RID: 3900 RVA: 0x00083B54 File Offset: 0x00081F54
	public string currentlyCopiedAreaName { get; private set; }

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x06000F3D RID: 3901 RVA: 0x00083B5D File Offset: 0x00081F5D
	// (set) Token: 0x06000F3E RID: 3902 RVA: 0x00083B65 File Offset: 0x00081F65
	public string currentlyCopiedAreaId { get; private set; }

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06000F3F RID: 3903 RVA: 0x00083B6E File Offset: 0x00081F6E
	// (set) Token: 0x06000F40 RID: 3904 RVA: 0x00083B76 File Offset: 0x00081F76
	public bool startedLaunchFadeIn { get; private set; }

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x06000F41 RID: 3905 RVA: 0x00083B80 File Offset: 0x00081F80
	// (set) Token: 0x06000F42 RID: 3906 RVA: 0x00083BCF File Offset: 0x00081FCF
	public bool onlyEditorsSetScreenContent
	{
		get
		{
			string text = ((!Managers.broadcastNetworkManager.inRoom) ? "0" : Managers.broadcastNetworkManager.GetPhotonCustomRoomProperty("onlyEditorsSetScreenContent"));
			return !string.IsNullOrEmpty(text) && text == "1";
		}
		set
		{
			Managers.broadcastNetworkManager.UpdatePhotonCustomRoomProperty("onlyEditorsSetScreenContent", (!value) ? "0" : "1");
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x06000F43 RID: 3907 RVA: 0x00083BF8 File Offset: 0x00081FF8
	// (set) Token: 0x06000F44 RID: 3908 RVA: 0x00083C47 File Offset: 0x00082047
	public bool onlyEditorsCanUseInventory
	{
		get
		{
			string text = ((!Managers.broadcastNetworkManager.inRoom) ? "0" : Managers.broadcastNetworkManager.GetPhotonCustomRoomProperty("onlyEditorsCanUseInventory"));
			return !string.IsNullOrEmpty(text) && text == "1";
		}
		set
		{
			Managers.broadcastNetworkManager.UpdatePhotonCustomRoomProperty("onlyEditorsCanUseInventory", (!value) ? "0" : "1");
		}
	}

	// Token: 0x06000F45 RID: 3909 RVA: 0x00083C70 File Offset: 0x00082070
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.canCreateAreas = true;
		this.weAreEditorOfCurrentArea = false;
		this.timeAreaWasJoined = null;
		Managers.StartupCompleted += this.OnManagersStartupCompleted;
		this.CheckStartupAreaParameter();
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06000F46 RID: 3910 RVA: 0x00083CC0 File Offset: 0x000820C0
	private void CheckStartupAreaParameter()
	{
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		if (commandLineArgs.Length >= 2)
		{
			this.areaToLoadDueToStartupParameter = commandLineArgs[1].ToLower().Trim();
		}
	}

	// Token: 0x06000F47 RID: 3911 RVA: 0x00083CF0 File Offset: 0x000820F0
	private void OnManagersStartupCompleted(object sender, EventArgs e)
	{
		this.areaToLoadForTesting = null;
		if (!string.IsNullOrEmpty(this.areaToLoadForTesting))
		{
			Log.Info("Loading area for testing: " + this.areaToLoadForTesting, false);
			this.TryTransportToAreaByNameOrUrlName(this.areaToLoadForTesting, string.Empty, false);
		}
		else if (!string.IsNullOrEmpty(this.areaToLoadDueToStartupParameter))
		{
			this.TryTransportToAreaByNameOrUrlName(this.areaToLoadDueToStartupParameter, string.Empty, false);
		}
		else if (!string.IsNullOrEmpty(AreaManager.areaToLoadAfterSceneChange))
		{
			SteamVR_Fade.Start(Color.clear, 0f, false);
			this.TryTransportToAreaByNameOrUrlName(AreaManager.areaToLoadAfterSceneChange, string.Empty, false);
			AreaManager.areaToLoadAfterSceneChange = null;
		}
		else
		{
			this.ForceLoadHomeArea();
		}
	}

	// Token: 0x06000F48 RID: 3912 RVA: 0x00083DAC File Offset: 0x000821AC
	public void ReloadCurrentArea()
	{
		Log.Info("Reloading current area.", false);
		this.TryTransportToAreaById(this.currentAreaId);
	}

	// Token: 0x06000F49 RID: 3913 RVA: 0x00083DC8 File Offset: 0x000821C8
	public bool TryTransportToAreaByNameOrUrlName(string areaNameOrAreaUrlName, string _thingNameToSendToAfterTransport = "", bool omitSound = false)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		this.thingNameToSendToAfterTransport = _thingNameToSendToAfterTransport;
		if (!omitSound)
		{
			Managers.soundManager.Play("teleport", Managers.personManager.ourPerson.transform, 1f, false, false);
		}
		string urlNameFromName = this.GetUrlNameFromName(areaNameOrAreaUrlName, ref flag2, ref flag3);
		if (urlNameFromName != null)
		{
			flag = true;
			Managers.pollManager.RemovePingsReceivedFromArea(areaNameOrAreaUrlName);
			this.TryTransportToAreaByUrlName(urlNameFromName);
		}
		return flag;
	}

	// Token: 0x06000F4A RID: 3914 RVA: 0x00083E34 File Offset: 0x00082234
	public void TryTransportToAreaByUrlName(string areaUrlName)
	{
		if (!this.isTransportInProgress)
		{
			this.isTransportInProgress = true;
			this.didFinishLoadingPlacements = false;
			base.StartCoroutine(this.TryTransportToArea(null, areaUrlName));
		}
	}

	// Token: 0x06000F4B RID: 3915 RVA: 0x00083E5E File Offset: 0x0008225E
	public void TryTransportToAreaById(string areaId)
	{
		if (!this.isTransportInProgress)
		{
			this.isTransportInProgress = true;
			this.didFinishLoadingPlacements = false;
			base.StartCoroutine(this.TryTransportToArea(areaId, null));
		}
	}

	// Token: 0x06000F4C RID: 3916 RVA: 0x00083E88 File Offset: 0x00082288
	private IEnumerator TryTransportToArea(string areaId, string areaUrlName)
	{
		Log.Info("Trying to transport to area: " + areaId + areaUrlName, false);
		Log.Info("Clean up before transport.", false);
		this.CleanUpBeforeTransport();
		bool isTransit = !string.IsNullOrEmpty(this.areaToTransportToAfterNextAreaLoad);
		Log.Info("TryTransportToArea - Loading area Data", false);
		Log.StartPerf("LOADING_AREA_DATA");
		LoadArea_Response response = null;
		LoadAreaData data = null;
		yield return base.StartCoroutine(Managers.serverManager.LoadArea(areaId, areaUrlName, isTransit, delegate(LoadArea_Response loadAreaResponse)
		{
			Log.EndPerf("LOADING_AREA_DATA");
			response = loadAreaResponse;
			data = response.data;
		}));
		Log.Info("TryTransportToArea - Handle missing response", false);
		if (response == null)
		{
			Managers.errorManager.ShowCriticalHaltError("Null response when loading area ", true, false, true, false, false);
			this.isTransportInProgress = false;
			yield break;
		}
		Log.Info("TryTransportToArea - Handle server error", false);
		if (!string.IsNullOrEmpty(response.error))
		{
			Log.Info("*** TryTransportToArea - server error : " + response.error, false);
			Managers.dialogManager.ShowInfo("Oops, this area doesn't exist.", false, true, -1, DialogType.Start, 1f, true, TextColor.Default, TextAlignment.Left);
			this.isTransportInProgress = false;
			yield break;
		}
		Log.Info("TryTransportToArea - Handle transport rejection", false);
		if (!response.ok)
		{
			this.HandleTransportDenial(data.reasonDenied);
			this.isTransportInProgress = false;
			yield break;
		}
		Log.Info("Area Data Loaded ok", false);
		this.MemorizeCurrentAsPreviousDataBeforeProcessingNewCurrent();
		this.didTeleportMoveInThisArea = false;
		yield return base.StartCoroutine(this.ProcessAreaLoadData(data, delegate(bool ok)
		{
		}));
		this.isTransportInProgress = false;
		Log.Info("Area Data processed (Things may still be instantiating).", false);
		string roomName = this.currentAreaUrlName + "_" + this.currentAreaKey;
		Managers.broadcastNetworkManager.JoinArea(roomName);
		Managers.pollManager.ForcePoll();
		yield break;
	}

	// Token: 0x06000F4D RID: 3917 RVA: 0x00083EB4 File Offset: 0x000822B4
	private void MemorizeCurrentAsPreviousDataBeforeProcessingNewCurrent()
	{
		if (!this.currentAreaIsTransit)
		{
			this.previousAreaUrlName = this.currentAreaUrlName;
			if (Managers.personManager.ourPerson != null)
			{
				Transform transform = Managers.personManager.ourPerson.transform;
				if (transform != null)
				{
					this.previousAreaPosition = transform.position;
					this.previousAreaRotation = transform.rotation;
				}
			}
		}
	}

	// Token: 0x06000F4E RID: 3918 RVA: 0x00083F24 File Offset: 0x00082324
	private void CleanUpBeforeTransport()
	{
		if (this.cleanUpBeforeTransportCount < 100)
		{
			this.cleanUpBeforeTransportCount++;
		}
		if (this.cleanUpBeforeTransportCount <= 1)
		{
			return;
		}
		this.didFinishLoadingPlacements = false;
		this.didRPCFinalizeLoadedAllPlacements = false;
		if (Managers.optimizationManager != null)
		{
			Managers.optimizationManager.ResetIndicateScriptActivity();
		}
		Managers.personManager.receivedUnassignedPersonBehaviorScriptVariables = new Dictionary<string, string>();
		Hand ahandOfOurs = Misc.GetAHandOfOurs();
		if (ahandOfOurs != null)
		{
			ahandOfOurs.EndAllOngoingLegPuppeteering();
		}
		this.behaviorScriptVariables = new Dictionary<string, float>();
		if (Managers.personManager.ourPerson != null)
		{
			Managers.personManager.ourPerson.SaveMyBehaviorScriptVariablesToFile();
			Managers.personManager.ourPerson.behaviorScriptVariables = new Dictionary<string, float>();
			Managers.personManager.ourPerson.ResetBodyStatesAndVariables();
			Managers.personManager.ourPerson.lastHandledSaveMyBehaviorScriptVariables = Time.time;
		}
		if (Managers.desktopManager != null)
		{
			Managers.desktopManager.ResetToDefaultSpeeds();
		}
		Universe.demoMode = false;
		this.limitVisibilityMeters = null;
		CreationHelper.customSnapAngles = null;
		CrossDevice.rigPositionIsAuthority = true;
		this.timeAreaWasJoined = null;
		Managers.skyManager.ResetToDefault();
		Managers.temporarilyDestroyedThingsManager.ClearAll();
		Effects.ClearAll();
		if (Managers.behaviorScriptManager != null)
		{
			Managers.behaviorScriptManager.StopSpeech();
		}
		Managers.personManager.DoAmplifySpeech(false, true);
		base.CancelInvoke("TransportToNextSetArea");
		base.CancelInvoke("ActivateThingsInCaseOfPotentialPlacementLoadingIssue");
		this.lastDeletionId = null;
		this.containsThingPartWithImage = false;
		if (Managers.personManager.ourPerson != null)
		{
			Managers.personManager.ourPerson.CloseAllDialogs();
		}
		if (Managers.videoManager != null)
		{
			Managers.videoManager.StopVideo(false);
		}
		else
		{
			Log.Error("VideoManager not ready");
		}
		this.DestroyThrownOrEmittedThings();
		this.rights = new AreaRights();
		this.didShowAllowInvisibilityWarning = false;
		Our.areaIdOfInterest = null;
		if (Our.mode == EditModes.Area)
		{
			Managers.personManager.DoClearPlacementIdOfHeldThing(TopographyId.Left);
			Managers.personManager.DoClearPlacementIdOfHeldThing(TopographyId.Right);
		}
		GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
		foreach (GameObject gameObject in rootGameObjects)
		{
			bool flag = gameObject.name == "New Game Object";
			if (flag)
			{
				global::UnityEngine.Object.Destroy(gameObject);
			}
		}
		Managers.browserManager.DestroyAllBrowsers();
		SpeechInput.DisposeListener();
		Managers.filterManager.SetToDefaults();
		Managers.filterManager.ApplySettings(false);
		this.sunOmitsShadow = false;
		this.ApplySunOmitsShadow();
	}

	// Token: 0x06000F4F RID: 3919 RVA: 0x000841D8 File Offset: 0x000825D8
	private void DestroyThrownOrEmittedThings()
	{
		GameObject @object = Managers.treeManager.GetObject("/Universe/ThrownOrEmittedThings");
		Thing[] componentsInChildren = @object.GetComponentsInChildren<Thing>();
		foreach (Thing thing in componentsInChildren)
		{
			Misc.Destroy(thing.gameObject);
		}
	}

	// Token: 0x06000F50 RID: 3920 RVA: 0x00084228 File Offset: 0x00082628
	private IEnumerator ProcessAreaLoadData(LoadAreaData data, Action<bool> callback)
	{
		Log.Info("Area manager - ProcessAreaLoadData", false);
		if (string.IsNullOrEmpty(data.areaId))
		{
			Managers.errorManager.ShowCriticalHaltError("No area id in loaded area data", true, true, true, false, false);
		}
		if (string.IsNullOrEmpty(data.areaName))
		{
			Managers.errorManager.ShowCriticalHaltError("No area name in loaded area data", true, true, true, false, false);
		}
		this.INFO_CurrentAreaId = data.areaId;
		this.currentAreaId = data.areaId;
		this.currentAreaName = data.areaName;
		this.currentAreaKey = data.areaKey;
		this.currentAreaUrlName = this.GetUrlNameFromName(this.currentAreaName);
		this.currentAreaCreatorId = data.areaCreatorId;
		this.currentAreaIsTransit = !string.IsNullOrEmpty(this.areaToTransportToAfterNextAreaLoad);
		this.isPrivate = data.isPrivate;
		this.isZeroGravity = data.isZeroGravity;
		this.hasFloatingDust = data.hasFloatingDust;
		this.isCopyable = data.isCopyable;
		this.onlyOwnerSetsLocks = data.onlyOwnerSetsLocks;
		this.isExcluded = data.isExcluded;
		this.weAreEditorOfCurrentArea = data.requestorIsEditor;
		this.weAreListEditorOfCurrentArea = data.requestorIsListEditor;
		this.weAreOwnerOfCurrentArea = data.requestorIsOwner;
		this.currentAreaIsHomeArea = data.areaId == Managers.personManager.ourPerson.homeAreaId;
		this.environmentChangersToApply = data.environmentChangers;
		this.ApplyEnvironmentType(data.environmentType);
		this.ResetEnvironmentChangers();
		this.ApplySettingsByJson(data.settingsJSON);
		if (Managers.personManager != null && Managers.personManager.ourPerson != null)
		{
			Managers.personManager.ourPerson.LoadMyBehaviorScriptVariablesFromFile();
		}
		yield return base.StartCoroutine(this.ProcessAreaPlacements(data.placements, delegate(bool ok)
		{
			if (ok)
			{
				Log.Info(string.Concat(new object[]
				{
					"**** Area *",
					this.currentAreaName,
					"* Loaded ok (",
					data.placements.Count,
					" placements.)  Editor?:",
					this.weAreEditorOfCurrentArea.ToString()
				}), false);
			}
			else
			{
				Log.Warning("Issue loading area placements");
			}
		}));
		yield break;
	}

	// Token: 0x06000F51 RID: 3921 RVA: 0x0008424C File Offset: 0x0008264C
	private void HandleTransportDenial(TransportDenialReason reason)
	{
		Log.Info("Area transport denied : " + reason.ToString(), false);
		switch (reason)
		{
		case TransportDenialReason.None:
			Log.Warning("HandleTransportDenial with reason=none. Should not happen.");
			Managers.dialogManager.ShowInfo("This area cannot be accessed...", false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
			break;
		case TransportDenialReason.Missing:
			Managers.dialogManager.ShowInfo("Oops, this area doesn't exist.", false, true, -1, DialogType.Start, 1f, true, TextColor.Default, TextAlignment.Left);
			break;
		case TransportDenialReason.Locked:
			Managers.dialogManager.ShowInfo("Oops, editors of this area have locked you from here for now.", false, true, -1, DialogType.Start, 1f, true, TextColor.Default, TextAlignment.Left);
			break;
		case TransportDenialReason.Closed:
			Managers.dialogManager.ShowInfo("Oops, this area cannot be accessed.", false, true, -1, DialogType.Start, 1f, true, TextColor.Default, TextAlignment.Left);
			break;
		case TransportDenialReason.Private:
			Managers.dialogManager.ShowInfo("Oops, that area is set to private and can only be joined by editors.", false, true, -1, DialogType.Start, 1f, true, TextColor.Default, TextAlignment.Left);
			break;
		case TransportDenialReason.Full:
			Managers.dialogManager.ShowInfo("Oops, that area is currently full!", false, true, -1, DialogType.Start, 1f, true, TextColor.Default, TextAlignment.Left);
			break;
		case TransportDenialReason.Banned:
			Managers.dialogManager.ShowInfo("This area cannot be accessed.", false, true, -1, DialogType.Start, 1f, true, TextColor.Default, TextAlignment.Left);
			break;
		}
		if (this.sentHomeAfterDenialCount < 20)
		{
			Log.Info("Transporting home", false);
			this.sentHomeAfterDenialCount++;
			base.Invoke("ForceLoadHomeArea", 5f);
		}
		else
		{
			Log.Warning("Maximum sent home count reached. Not transporting to home");
		}
	}

	// Token: 0x06000F52 RID: 3922 RVA: 0x000843DA File Offset: 0x000827DA
	public void HandlePhotonJoinRoomFailure()
	{
		this.ForceLoadHomeArea();
	}

	// Token: 0x06000F53 RID: 3923 RVA: 0x000843E2 File Offset: 0x000827E2
	public void HandlePhotonConnectionFailure()
	{
		this.ForceReloadCurrentArea();
	}

	// Token: 0x06000F54 RID: 3924 RVA: 0x000843EA File Offset: 0x000827EA
	public void ForceLoadHomeArea()
	{
		this.isTransportInProgress = false;
		this.LoadHomeArea();
	}

	// Token: 0x06000F55 RID: 3925 RVA: 0x000843F9 File Offset: 0x000827F9
	public void ForceReloadCurrentArea()
	{
		this.isTransportInProgress = false;
		this.ReloadCurrentArea();
	}

	// Token: 0x06000F56 RID: 3926 RVA: 0x00084408 File Offset: 0x00082808
	public void LoadHomeArea()
	{
		Log.Info("Loading home area: " + Managers.personManager.ourPerson.homeAreaId, false);
		this.TryTransportToAreaById(Managers.personManager.ourPerson.homeAreaId);
	}

	// Token: 0x06000F57 RID: 3927 RVA: 0x00084440 File Offset: 0x00082840
	public string GetUrlNameFromName(string name)
	{
		bool flag = false;
		bool flag2 = false;
		return this.GetUrlNameFromName(name, ref flag, ref flag2);
	}

	// Token: 0x06000F58 RID: 3928 RVA: 0x0008445C File Offset: 0x0008285C
	public string GetUrlNameFromName(string name, ref bool lengthTooSmall, ref bool lengthTooBig)
	{
		if (name == null)
		{
			throw new Exception("GetUrlNameFromName received null name");
		}
		name = name.ToLower();
		name = name.Trim();
		string text = string.Empty;
		foreach (char c in name)
		{
			if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'z') || c == '-')
			{
				text += c;
			}
		}
		name = text;
		name = Misc.ReplaceAll(name, "--", "-");
		lengthTooSmall = name.Length < 7;
		lengthTooBig = name.Length > 40;
		if (lengthTooSmall || lengthTooBig)
		{
			name = null;
		}
		return name;
	}

	// Token: 0x06000F59 RID: 3929 RVA: 0x00084524 File Offset: 0x00082924
	private IEnumerator ProcessAreaPlacements(List<PlacementData> placements, Action<bool> callback)
	{
		Log.Info("Area manager - ProcessAreaPlacements (" + placements.Count + ")", false);
		DateTime t = DateTime.Now;
		base.CancelInvoke("ActivateThingsInCaseOfPotentialPlacementLoadingIssue");
		Managers.thingManager.UnloadPlacements();
		if (string.IsNullOrEmpty(this.thingNameToSendToAfterTransport) || this.rotationAfterTransport != 0f)
		{
			Managers.personManager.ourPerson.ResetPositionAndRotation();
		}
		else
		{
			Managers.personManager.ourPerson.ResetPosition();
		}
		Log.Info("PRIME CACHE - CALLING", false);
		yield return base.StartCoroutine(Managers.thingManager.PrimeCacheWithThingDefinitionBundleIfNeeded(placements, this.currentAreaId, this.currentAreaKey, delegate(bool ok)
		{
			Log.Info("*** PRIME CACHE - CALLBACK", false);
		}));
		Log.Info("*** PRIME CACHE - AFTER", false);
		this.placementsLoadingCounter = placements.Count;
		base.Invoke("ActivateThingsInCaseOfPotentialPlacementLoadingIssue", 20f);
		foreach (PlacementData placementData in placements)
		{
			if (string.IsNullOrEmpty(placementData.Id))
			{
				Log.Error("ProcessAreaPlacements found placement with missing placementId!:");
			}
			Managers.thingManager.InstantiatePlacedThingViaCache(ThingRequestContext.ProcessAreaPlacements, placementData);
		}
		this.SetAppropriateEditMode();
		this.SetAppropriateOurScale();
		if (Managers.personManager.ourPerson.isHardBanned)
		{
			Managers.errorManager.ShowCriticalHaltError("Oops, this account is locked", false, false, true, false, false);
		}
		Log.Info("ProcessAreaPlacements took " + DateTime.Now.Subtract(t).TotalSeconds + "s", false);
		yield break;
	}

	// Token: 0x06000F5A RID: 3930 RVA: 0x00084546 File Offset: 0x00082946
	public void DoneLoadingThisPlacement()
	{
		this.placementsLoadingCounter--;
		if (this.placementsLoadingCounter == 0)
		{
			this.DoneLoadingAllPlacements();
		}
	}

	// Token: 0x06000F5B RID: 3931 RVA: 0x00084568 File Offset: 0x00082968
	private void DoneLoadingAllPlacements()
	{
		Log.Info("DoneLoadingAllPlacements", false);
		base.CancelInvoke("ActivateThingsInCaseOfPotentialPlacementLoadingIssue");
		bool flag = !string.IsNullOrEmpty(this.areaToTransportToAfterNextAreaLoad);
		Managers.behaviorScriptManager.TriggerEventsRelatedToPosition();
		if (this.environmentChangersToApply != null && this.environmentChangersToApply.environmentChangers.Count > 0)
		{
			foreach (EnvironmentChangerData environmentChangerData in this.environmentChangersToApply.environmentChangers)
			{
				this.ApplyEnvironmentChanger(environmentChangerData);
			}
		}
		this.environmentChangersToApply = null;
		if (!flag && !string.IsNullOrEmpty(this.thingNameToSendToAfterTransport))
		{
			bool flag2 = this.SendOurPersonToThingInArea(this.thingNameToSendToAfterTransport, true);
			this.thingNameToSendToAfterTransport = string.Empty;
			if (!flag2)
			{
				Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
			}
		}
		else
		{
			if (!flag && this.positionToTransportToAfterAreaLoad != Vector3.zero && Managers.personManager.ourPerson != null)
			{
				Transform transform = Managers.personManager.ourPerson.transform;
				if (transform != null)
				{
					transform.position = this.positionToTransportToAfterAreaLoad;
					transform.rotation = this.rotationToTransportToAfterAreaLoad;
				}
				this.positionToTransportToAfterAreaLoad = Vector3.zero;
				this.rotationToTransportToAfterAreaLoad = Quaternion.identity;
			}
			Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
		}
		if (!this.currentAreaIsHomeArea)
		{
			Managers.achievementManager.RegisterAchievement(Achievement.VisitedOtherArea);
		}
		if (flag)
		{
			base.Invoke("TransportToNextSetArea", this.areaToTransportToAfterNextAreaLoadSeconds);
		}
		Managers.thingManager.UpdateAllVisibilityAndCollision(false);
		this.HandleAutoPlayingVideos(null);
		bool flag3 = !string.IsNullOrEmpty(Managers.broadcastNetworkManager.INFO_Current_Room);
		if (flag3)
		{
			Managers.personManager.DoFinalizeLoadedAllPlacements();
		}
		CrossDevice.rigPositionIsAuthority = true;
		this.didFinishLoadingPlacements = true;
		if (this.containsThingPartWithImage)
		{
			this.DeleteImageCacheIfTooBig();
		}
		if (this.areasToSnapshot.Length >= 1)
		{
			base.Invoke("HandleAreaSnapshotting", 0.5f);
		}
		this.HandleLaunchFadeInAndEffects();
		this.ActivateAreaSpeechListener(false);
		this.didAlertVerySmallPersonScaleForThisArea = false;
		this.timeAreaWasJoined = new float?(Time.time);
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x000847C4 File Offset: 0x00082BC4
	public void ActivateAreaSpeechListener(bool ignoreMode = false)
	{
		if (!this.didFinishLoadingPlacements || (!ignoreMode && Our.mode == EditModes.Inventory))
		{
			return;
		}
		List<string> list = new List<string>();
		GameObject @object = Managers.treeManager.GetObject("/Universe/ThrownOrEmittedThings");
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(ThingPart), true);
		Component[] componentsInChildren2 = Managers.personManager.ourPerson.GetComponentsInChildren(typeof(ThingPart), true);
		Component[] componentsInChildren3 = @object.GetComponentsInChildren(typeof(ThingPart), true);
		Component[] array = componentsInChildren3.Concat(componentsInChildren.Concat(componentsInChildren2).ToArray<Component>()).ToArray<Component>();
		foreach (ThingPart thingPart in array)
		{
			if (thingPart.transform.parent.name != Universe.objectNameIfAlreadyDestroyed)
			{
				foreach (ThingPartState thingPartState in thingPart.states)
				{
					foreach (StateListener stateListener in thingPartState.listeners)
					{
						bool flag = stateListener.eventType == StateListener.EventType.OnHears || stateListener.eventType == StateListener.EventType.OnHearsAnywhere;
						if (flag && !string.IsNullOrEmpty(stateListener.whenData) && !list.Contains(stateListener.whenData))
						{
							list.Add(stateListener.whenData);
						}
					}
				}
			}
		}
		this.keywordsToHear = list;
		SpeechInput.InitListener(this.keywordsToHear.ToArray(), new SpeechInput.SpeechRecognizedHandler(this.OnSpeechRecognized));
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x000849BC File Offset: 0x00082DBC
	private void OnSpeechRecognized(SpeechRecognizedEventArgs args)
	{
		string text = args.Text;
		Managers.behaviorScriptManager.TriggerOnHears(text);
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x000849DC File Offset: 0x00082DDC
	private void HandleLaunchFadeInAndEffects()
	{
		bool flag = false;
		if (!this.startedLaunchFadeIn)
		{
			bool flag2 = Managers.personManager && Managers.personManager.ourPerson && (float)Managers.personManager.ourPerson.ageInSecs <= 10f;
			if (flag)
			{
				flag2 = true;
			}
			float num = 5f;
			float num2 = 3f;
			string text = "longWhoosh";
			float num3 = 0.125f;
			if (flag2)
			{
				num = 7.5f;
				num2 = 15f;
				text = "newPersonBorn";
				num3 = 0.15f;
			}
			SteamVR_Fade.Start(Color.clear, num, false);
			if (Managers.soundManager != null && Managers.treeManager != null)
			{
				Transform transform = Managers.treeManager.GetTransform("/OurPersonRig/EnvironmentParticleSystems");
				GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("ParticleSystems/LaunchDust"), transform) as GameObject;
				gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
				ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
				component.main.duration = num2;
				component.Play();
				SoundManager soundManager = Managers.soundManager;
				string text2 = text;
				float num4 = num3;
				soundManager.Play(text2, null, num4, false, false);
			}
			this.startedLaunchFadeIn = true;
		}
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x00084B24 File Offset: 0x00082F24
	public void HandleAutoPlayingVideos(Thing parentThing = null)
	{
		ThingPart avideoAutoPlayingThingPart = this.GetAVideoAutoPlayingThingPart(parentThing);
		if (avideoAutoPlayingThingPart != null && avideoAutoPlayingThingPart.transform.parent != null)
		{
			Managers.videoManager.SetVolume(avideoAutoPlayingThingPart.videoAutoPlayVolume);
			Managers.personManager.DoPlayVideo(avideoAutoPlayingThingPart.transform.parent.gameObject, avideoAutoPlayingThingPart.videoIdToPlayAtAreaStart, "Auto-playing Video");
		}
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x00084B90 File Offset: 0x00082F90
	public ThingPart GetAVideoAutoPlayingThingPart(Thing parentThing = null)
	{
		ThingPart thingPart = null;
		GameObject gameObject = ((!(parentThing != null)) ? Managers.thingManager.placements : parentThing.gameObject);
		Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(ThingPart), true);
		foreach (ThingPart thingPart2 in componentsInChildren)
		{
			if (!string.IsNullOrEmpty(thingPart2.videoIdToPlayAtAreaStart) && thingPart2.transform.parent != null && thingPart2.transform.parent.name != Universe.objectNameIfAlreadyDestroyed)
			{
				thingPart = thingPart2;
			}
		}
		return thingPart;
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x00084C44 File Offset: 0x00083044
	private void HandleAreaSnapshotting()
	{
		Log.Debug(string.Concat(new object[]
		{
			"Taking snapshot ",
			this.areasToSnapshotIndex + 1,
			" of ",
			this.areasToSnapshot.Length,
			": ",
			this.currentAreaName
		}));
		ScreenCapture.CaptureScreenshot("E:\\_temp\\anyland-screenshots\\" + this.currentAreaUrlName + ".png");
		this.areasToSnapshotIndex++;
		if (this.areasToSnapshotIndex < this.areasToSnapshot.Length)
		{
			this.isTransportInProgress = false;
			this.TryTransportToAreaByNameOrUrlName(this.areasToSnapshot[this.areasToSnapshotIndex], string.Empty, false);
		}
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x00084D00 File Offset: 0x00083100
	private void DeleteImageCacheIfTooBig()
	{
		string text = Path.Combine(Application.persistentDataPath, "cache\\images");
		if (Misc.GetDirectorySizeInBytes(text, "*") > 524288000L)
		{
			Directory.Delete(text, true);
		}
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x00084D3C File Offset: 0x0008313C
	public void AssignPlacedSubThings(string onlyThisThingId = "")
	{
		Thing[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren<Thing>();
		foreach (Thing thing in componentsInChildren)
		{
			if (thing.containsPlacedSubThings && thing.transform.parent == Managers.thingManager.placements.transform && thing.CompareTag("Thing") && (onlyThisThingId == string.Empty || thing.thingId == onlyThisThingId))
			{
				ThingPart[] componentsInChildren2 = thing.GetComponentsInChildren<ThingPart>();
				foreach (ThingPart thingPart in componentsInChildren2)
				{
					thingPart.AssignMyPlacedSubThings(string.Empty);
				}
			}
		}
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x00084E0C File Offset: 0x0008320C
	private void TransportToNextSetArea()
	{
		if (!string.IsNullOrEmpty(this.areaToTransportToAfterNextAreaLoad))
		{
			string areaToTransportToAfterNextAreaLoad = this.areaToTransportToAfterNextAreaLoad;
			this.areaToTransportToAfterNextAreaLoad = null;
			this.areaToTransportToAfterNextAreaLoadSeconds = 0f;
			this.TryTransportToAreaByNameOrUrlName(areaToTransportToAfterNextAreaLoad, this.thingNameToSendToAfterTransport, false);
		}
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x00084E51 File Offset: 0x00083251
	public void ActivateThingsInCaseOfPotentialPlacementLoadingIssue()
	{
		Log.Warning("Area placements load took too long, force-activating nearby things now");
		this.placementsLoadingCounter = 0;
		this.DoneLoadingAllPlacements();
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x00084E6C File Offset: 0x0008326C
	public bool SendOurPersonToThingInArea(string thingName, bool omitSound = false)
	{
		bool flag = false;
		if (thingName != string.Empty)
		{
			GameObject closestThingToUs = this.GetClosestThingToUs(thingName);
			if (closestThingToUs != null)
			{
				Hand ahandOfOurs = Misc.GetAHandOfOurs();
				if (ahandOfOurs != null)
				{
					ahandOfOurs.TeleportTo(closestThingToUs, closestThingToUs.transform.position, false, omitSound, false, true, true);
					if (this.rotationAfterTransport != 0f)
					{
						ahandOfOurs.TurnToAngle(this.rotationAfterTransport);
						this.rotationAfterTransport = 0f;
					}
					if (ahandOfOurs.otherHandScript != null)
					{
						ahandOfOurs.otherHandScript.smoothMovementTeleportTarget = null;
					}
					flag = true;
				}
			}
		}
		return flag;
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x00084F18 File Offset: 0x00083318
	private GameObject GetClosestThingToUs(string thingName)
	{
		GameObject gameObject = null;
		if (Managers.personManager == null || Managers.personManager.ourPerson == null || Managers.personManager.ourPerson.Head == null)
		{
			return null;
		}
		Transform transform = Managers.personManager.ourPerson.Head.transform;
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Transform), true);
		float num = -1f;
		foreach (Transform transform2 in componentsInChildren)
		{
			if (transform2.name == thingName)
			{
				float num2 = Vector3.Distance(transform.position, transform2.position);
				if (num == -1f || num2 < num)
				{
					num = num2;
					gameObject = transform2.gameObject;
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x0008500C File Offset: 0x0008340C
	private void SetAppropriateEditMode()
	{
		if (!Managers.personManager.ourPerson.isHardBanned && Managers.personManager.ourPerson.hasEditTools)
		{
			bool flag = this.previousAreaUrlName == null;
			if (flag)
			{
				if (this.weAreEditorOfCurrentArea && Universe.features.changeThings)
				{
					Our.SetMode(EditModes.Area, false);
				}
				else
				{
					Our.SetMode(EditModes.None, false);
				}
			}
			else if (!this.weAreEditorOfCurrentArea)
			{
				Our.SetMode(EditModes.None, false);
			}
		}
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x00085098 File Offset: 0x00083498
	private void SetAppropriateOurScale()
	{
		if (!this.weAreEditorOfCurrentArea)
		{
			int ourScalePercent = Managers.personManager.GetOurScalePercent();
			if (ourScalePercent < 1 || ourScalePercent > 150)
			{
				Managers.personManager.ResetAndCachePhotonRigScale();
			}
		}
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x000850D8 File Offset: 0x000834D8
	public void CreateArea(string areaName)
	{
		base.StartCoroutine(Managers.serverManager.CreateArea(areaName, delegate(CreateArea_Response response)
		{
			if (response.error == null)
			{
				Managers.personManager.ourPerson.areaCount = response.newAreaCount;
				if (response.cantCreate)
				{
					Managers.dialogManager.ShowInfo("Oops, you are not currently able to create areas.", false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
				}
				else if (response.badName)
				{
					Managers.dialogManager.ShowInfo("Oops, this area name can't be chosen.", false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
				}
				else if (response.duplicateName)
				{
					Managers.dialogManager.ShowInfo("Oops, this area name is unavailable.", false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
				}
				else if (response.invalidName)
				{
					Log.Error("Area name validation mismatch for name: " + areaName);
					Managers.dialogManager.ShowInfo("Oops, this area name is invalid.", false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
				}
				else if (response.areaId != null)
				{
					Log.Info("Area created with id: " + response.areaId, false);
					this.TryTransportToAreaById(response.areaId);
					Managers.achievementManager.RegisterAchievement(Achievement.CreatedNewArea);
				}
				else
				{
					Log.Error("CreateArea returned without error, but no areaId");
				}
			}
			else
			{
				Log.Error(response.error);
				Managers.dialogManager.ShowInfo(response.error, false, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
			}
		}));
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x0008511C File Offset: 0x0008351C
	public void SaveThingPlacement(GameObject thing)
	{
		Thing thing2 = (Thing)thing.GetComponent(typeof(Thing));
		string thingId = thing2.thingId;
		Vector3 localPosition = thing.transform.localPosition;
		Vector3 localEulerAngles = thing.transform.localEulerAngles;
		float x = thing.transform.localScale.x;
		float? distanceToShow = thing2.distanceToShow;
		float num;
		if (distanceToShow != null)
		{
			float? distanceToShow2 = thing2.distanceToShow;
			num = distanceToShow2.Value;
		}
		else
		{
			num = 0f;
		}
		PlacementData placementData = new PlacementData(thingId, localPosition, localEulerAngles, x, num);
		thing2.placementId = placementData.Id;
		base.StartCoroutine(Managers.serverManager.NewPlacement(this.currentAreaId, placementData, delegate(Placement_Response response)
		{
			if (response.error == null)
			{
				if (response.ok)
				{
					Log.Info("Placement saved ok", false);
				}
				else
				{
					Log.Error("Placement NOT saved");
				}
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x000851E4 File Offset: 0x000835E4
	public void UpdateThingPlacement(GameObject thing)
	{
		Thing component = thing.GetComponent<Thing>();
		Log.Info("AreaManager.UpdateThingPlacement, placementId : " + component.placementId, false);
		if (string.IsNullOrEmpty(component.placementId))
		{
			throw new Exception("AreaManager.UpdateThingPlacement - thing has no placementId");
		}
		component.MemorizeOriginalTransform(true);
		string placementId = component.placementId;
		string thingId = component.thingId;
		Vector3 localPosition = thing.transform.localPosition;
		Vector3 localEulerAngles = thing.transform.localEulerAngles;
		float x = thing.transform.localScale.x;
		float? distanceToShow = component.distanceToShow;
		float num;
		if (distanceToShow != null)
		{
			float? distanceToShow2 = component.distanceToShow;
			num = distanceToShow2.Value;
		}
		else
		{
			num = 0f;
		}
		PlacementData placementData = new PlacementData(placementId, thingId, localPosition, localEulerAngles, x, num);
		base.StartCoroutine(Managers.serverManager.UpdatePlacement(this.currentAreaId, placementData, delegate(Placement_Response response)
		{
			if (response.error == null)
			{
				if (response.ok)
				{
					Log.Info("Placement updated ok", false);
				}
				else
				{
					Log.Error("Placement NOT updated");
				}
			}
			else
			{
				Managers.errorManager.BeepError();
			}
		}));
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x000852CC File Offset: 0x000836CC
	public void DeleteThingPlacement(GameObject thing)
	{
		Thing component = thing.GetComponent<Thing>();
		string placementId = component.placementId;
		if (string.IsNullOrEmpty(placementId))
		{
			Managers.errorManager.BeepError();
			Log.Error("DeleteThingPlacement called with thing with null placementId");
			return;
		}
		this.lastDeletionId = component.thingId;
		this.lastDeletionPosition = thing.transform.localPosition;
		this.lastDeletionRotation = thing.transform.localRotation;
		base.StartCoroutine(Managers.serverManager.DeletePlacement(this.currentAreaId, placementId, delegate(Placement_Response response)
		{
			if (response.error != null)
			{
				Managers.errorManager.BeepError();
			}
		}));
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x0008536C File Offset: 0x0008376C
	public void SetPlacementAttribute(string placementId, PlacementAttribute attribute, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetPlacementAttribute(this.currentAreaId, placementId, attribute, delegate(ResponseBase response)
		{
			if (response.error == null)
			{
				Managers.personManager.DoSetPlacementAttribute(placementId, attribute, true);
			}
			callback(response.error == null);
		}));
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x000853C4 File Offset: 0x000837C4
	public void ClearPlacementAttribute(string placementId, PlacementAttribute attribute, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.ClearPlacementAttribute(this.currentAreaId, placementId, attribute, delegate(ResponseBase response)
		{
			if (response.error == null)
			{
				Managers.personManager.DoSetPlacementAttribute(placementId, attribute, false);
			}
			callback(response.error == null);
		}));
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x0008541C File Offset: 0x0008381C
	public void GetInfo(Action<AreaInfo> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetAreaInfo(this.currentAreaId, delegate(GetAreaInfo_Response response)
		{
			if (response.error != null)
			{
				Managers.errorManager.BeepError();
			}
			callback(response.areaInfo);
		}));
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x0008545C File Offset: 0x0008385C
	public void SetIsPrivate(bool isPrivate, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetAreaPrivacy(this.currentAreaId, isPrivate, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Managers.errorManager.BeepError();
			}
			else if (response.ok)
			{
				this.SetIsPrivate_Local(isPrivate);
			}
			callback(response.error == null && response.ok);
		}));
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x000854AD File Offset: 0x000838AD
	public void SetIsPrivate_Local(bool _isPrivate)
	{
		this.isPrivate = _isPrivate;
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x000854B8 File Offset: 0x000838B8
	public void SetIsZeroGravity(bool isZeroGravity, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetAreaGravity(this.currentAreaId, isZeroGravity, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Managers.errorManager.BeepError();
			}
			else if (response.ok)
			{
				this.SetIsZeroGravity_Local(isZeroGravity);
			}
			callback(response.error == null && response.ok);
		}));
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x0008550C File Offset: 0x0008390C
	public void SetIsZeroGravity_Local(bool _isZeroGravity)
	{
		this.isZeroGravity = _isZeroGravity;
		GameObject @object = Managers.treeManager.GetObject("/Universe/EnvironmentManager");
		if (@object != null)
		{
			EnvironmentManager component = @object.GetComponent<EnvironmentManager>();
			component.UpdateGravity();
		}
	}

	// Token: 0x06000F75 RID: 3957 RVA: 0x0008554C File Offset: 0x0008394C
	public void SetHasFloatingDust(bool hasFloatingDust, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetAreaFloatingDust(this.currentAreaId, hasFloatingDust, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Managers.errorManager.BeepError();
			}
			else if (response.ok)
			{
				this.SetHasFloatingDust_Local(hasFloatingDust);
			}
			callback(response.error == null && response.ok);
		}));
	}

	// Token: 0x06000F76 RID: 3958 RVA: 0x000855A0 File Offset: 0x000839A0
	public void SaveSettings(Action<bool> callback)
	{
		string settingsJson = this.GetSettingsJson(false);
		base.StartCoroutine(Managers.serverManager.UpdateAreaSettings(this.currentAreaId, settingsJson, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Managers.errorManager.BeepError();
			}
			else if (response.ok)
			{
			}
			callback(response.error == null && response.ok);
		}));
	}

	// Token: 0x06000F77 RID: 3959 RVA: 0x000855E8 File Offset: 0x000839E8
	private string GetSettingsJson(bool isForLocalExport = false)
	{
		List<string> list = new List<string>();
		string text = Managers.filterManager.GetJson();
		if (text != string.Empty)
		{
			text = "\"filters\":" + text;
			list.Add(text);
		}
		if (this.sunOmitsShadow)
		{
			list.Add("\"sunOmitsShadow\":true");
		}
		if (isForLocalExport)
		{
			list.Add("\"sunDirection\":" + JsonHelper.GetJson(this.environmentLight.transform.eulerAngles));
			if (this.hasFloatingDust)
			{
				list.Add("\"hasFloatingDust\":true");
			}
		}
		return "{" + string.Join(",", list.ToArray()) + "}";
	}

	// Token: 0x06000F78 RID: 3960 RVA: 0x000856A0 File Offset: 0x00083AA0
	public void ApplySettingsByJson(string json)
	{
		if (json == "{}")
		{
			json = string.Empty;
		}
		if (!string.IsNullOrEmpty(json))
		{
			JSONNode jsonnode = JSON.Parse(json);
			if (jsonnode != null)
			{
				Managers.filterManager.SetByJson(jsonnode["filters"]);
				Managers.filterManager.ApplySettings(false);
				if (jsonnode["sunOmitsShadow"] != null)
				{
					this.sunOmitsShadow = true;
				}
				this.ApplySunOmitsShadow();
			}
		}
	}

	// Token: 0x06000F79 RID: 3961 RVA: 0x00085725 File Offset: 0x00083B25
	public void ApplySunOmitsShadow()
	{
		this.environmentLight.shadows = ((!this.sunOmitsShadow) ? LightShadows.Soft : LightShadows.None);
	}

	// Token: 0x06000F7A RID: 3962 RVA: 0x00085744 File Offset: 0x00083B44
	public void ApplyCustomRoomProperties()
	{
		string photonCustomRoomProperty = Managers.broadcastNetworkManager.GetPhotonCustomRoomProperty("gravity");
		if (!string.IsNullOrEmpty(photonCustomRoomProperty))
		{
			Vector3? spaceSeparatedStringToVector = Misc.GetSpaceSeparatedStringToVector3(photonCustomRoomProperty, 1000f);
			if (spaceSeparatedStringToVector != null)
			{
				Physics.gravity = spaceSeparatedStringToVector.Value;
			}
		}
	}

	// Token: 0x06000F7B RID: 3963 RVA: 0x00085790 File Offset: 0x00083B90
	public void SetHasFloatingDust_Local(bool _hasFloatingDust)
	{
		this.hasFloatingDust = _hasFloatingDust;
		GameObject @object = Managers.treeManager.GetObject("/Universe/EnvironmentManager");
		if (@object != null)
		{
			EnvironmentManager component = @object.GetComponent<EnvironmentManager>();
			component.UpdateFloatingDust();
		}
	}

	// Token: 0x06000F7C RID: 3964 RVA: 0x000857D0 File Offset: 0x00083BD0
	public void SetIsCopyable(bool _isCopyable, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetAreaCopyable(this.currentAreaId, _isCopyable, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Managers.errorManager.BeepError();
			}
			else if (response.ok)
			{
				this.SetIsCopyable_Local(_isCopyable);
			}
			callback(response.error == null && response.ok);
		}));
	}

	// Token: 0x06000F7D RID: 3965 RVA: 0x00085821 File Offset: 0x00083C21
	public void SetIsCopyable_Local(bool _isCopyable)
	{
		this.isCopyable = _isCopyable;
	}

	// Token: 0x06000F7E RID: 3966 RVA: 0x0008582C File Offset: 0x00083C2C
	public void SetOnlyOwnerSetsLocks(bool _onlyOwnerSetsLocks, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetAreaOnlyOwnerSetsLocks(this.currentAreaId, _onlyOwnerSetsLocks, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Managers.errorManager.BeepError();
			}
			else if (response.ok)
			{
				this.SetOnlyOwnerSetsLocks_Local(_onlyOwnerSetsLocks);
			}
			callback(response.error == null && response.ok);
		}));
	}

	// Token: 0x06000F7F RID: 3967 RVA: 0x0008587D File Offset: 0x00083C7D
	public void SetOnlyOwnerSetsLocks_Local(bool onlyOwnerSetsLocks)
	{
		this.onlyOwnerSetsLocks = onlyOwnerSetsLocks;
	}

	// Token: 0x06000F80 RID: 3968 RVA: 0x00085886 File Offset: 0x00083C86
	public bool WeCanChangeLocks()
	{
		return Universe.features.changeThings && (this.weAreOwnerOfCurrentArea || (this.weAreEditorOfCurrentArea && !this.onlyOwnerSetsLocks));
	}

	// Token: 0x06000F81 RID: 3969 RVA: 0x000858C0 File Offset: 0x00083CC0
	public void SetIsExcluded(bool _isExcluded, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetAreaIsExcluded(this.currentAreaId, _isExcluded, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Managers.errorManager.BeepError();
			}
			else if (response.ok)
			{
				this.SetIsExcluded_Local(_isExcluded);
			}
			callback(response.error == null && response.ok);
		}));
	}

	// Token: 0x06000F82 RID: 3970 RVA: 0x00085911 File Offset: 0x00083D11
	public void SetIsExcluded_Local(bool isExcluded)
	{
		this.isExcluded = isExcluded;
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x0008591C File Offset: 0x00083D1C
	public void SetDescription(string description, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetAreaDescription(this.currentAreaId, description, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			callback(response.ok);
		}));
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x0008595C File Offset: 0x00083D5C
	public void SetParentArea(string childAreaId, string parentAreaId, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetParentArea(childAreaId, parentAreaId, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			callback(response.ok);
		}));
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x00085998 File Offset: 0x00083D98
	public void GetSubAreasOfArea(string areaId, Action<List<AreaOverview>> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetSubAreas(areaId, delegate(GetSubAreas_Response response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			callback(response.subAreas);
		}));
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x000859D0 File Offset: 0x00083DD0
	public void SetEditor(string areaId, string personId, bool isEditor, Action callback)
	{
		base.StartCoroutine(Managers.serverManager.SetAreaEditor(areaId, personId, isEditor, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			else
			{
				Managers.broadcastNetworkManager.RaiseEvent_ReloadArea(personId);
				callback();
			}
		}));
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x00085A18 File Offset: 0x00083E18
	public void SetListEditor(string areaId, string personId, bool isListEditor, Action callback)
	{
		base.StartCoroutine(Managers.serverManager.SetAreaListEditor(areaId, personId, isListEditor, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			else
			{
				Managers.broadcastNetworkManager.RaiseEvent_ReloadArea(personId);
				callback();
			}
		}));
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x00085A60 File Offset: 0x00083E60
	public void LockPersonFromArea(string areaId, string personId, string reason, int? minutesOrPermanentIfNull, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.LockPersonFromArea(areaId, personId, reason, minutesOrPermanentIfNull, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			bool isPrivate = this.isPrivate;
			if (isPrivate)
			{
				Managers.broadcastNetworkManager.RaiseEvent_ReloadArea();
			}
			else
			{
				Managers.broadcastNetworkManager.RaiseEvent_ReloadArea(personId);
			}
			callback(response.ok);
		}));
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x00085AB0 File Offset: 0x00083EB0
	public void UnlockPersonFromArea(string areaId, string personId, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.UnlockPersonFromArea(areaId, personId, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			callback(response.ok);
		}));
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x00085AE9 File Offset: 0x00083EE9
	public void PersistEnvironmentChanger(EnvironmentChangerData changer)
	{
		base.StartCoroutine(Managers.serverManager.UpdateAreaEnvironmentChanger(this.currentAreaId, changer, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x00085B20 File Offset: 0x00083F20
	public void PersistEnvironmentType(EnvironmentType envType)
	{
		base.StartCoroutine(Managers.serverManager.SetAreaEnvironmentType(this.currentAreaId, envType, delegate(ExtendedServerResponse response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x00085B58 File Offset: 0x00083F58
	public void ApplyEnvironmentChanger(string changerName, Vector3 rotation, float scale, Color color)
	{
		string[] array = new string[] { "sun", "night", "ambientLight", "fog", "clouds" };
		if (array.Contains(changerName))
		{
			GameObject @object = Managers.treeManager.GetObject("/Universe/EnvironmentChangers/" + changerName);
			if (@object != null)
			{
				@object.transform.localEulerAngles = rotation;
				@object.transform.localScale = new Vector3(scale, scale, scale);
				Renderer component = @object.GetComponent<Renderer>();
				if (component)
				{
					component.material.color = color;
				}
			}
		}
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x00085C00 File Offset: 0x00084000
	public void ApplyEnvironmentChanger(EnvironmentChangerData changer)
	{
		this.ApplyEnvironmentChanger(changer.Name, changer.Rotation, changer.Scale, changer.Color);
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x00085C20 File Offset: 0x00084020
	public void ResetEnvironmentChangers()
	{
		GameObject @object = Managers.treeManager.GetObject("/Universe/EnvironmentChangers");
		Component[] componentsInChildren = @object.GetComponentsInChildren(typeof(EnvironmentChanger), true);
		foreach (EnvironmentChanger environmentChanger in componentsInChildren)
		{
			environmentChanger.SetToDefault();
		}
	}

	// Token: 0x06000F8F RID: 3983 RVA: 0x00085C7C File Offset: 0x0008407C
	public void ApplyEnvironmentType(EnvironmentType environmentType)
	{
		GameObject @object = Managers.treeManager.GetObject("/Universe/EnvironmentManager");
		if (@object != null)
		{
			EnvironmentManager component = @object.GetComponent<EnvironmentManager>();
			component.SetToType(environmentType);
		}
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x00085CB4 File Offset: 0x000840B4
	public void GetAreaLists(Action<AreaListSet> callback)
	{
		int num = 30;
		int num2 = 300;
		base.StartCoroutine(Managers.serverManager.GetAreaLists(num, num2, delegate(GetAreaLists_Response response)
		{
			if (response.error == null)
			{
				callback(response.areas);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x00085CF8 File Offset: 0x000840F8
	public void SearchAreas(string term, string byCreatorName, string byCreatorId, Action<AreaList> callback)
	{
		base.StartCoroutine(Managers.serverManager.SearchAreas(term, byCreatorName, byCreatorId, delegate(SearchAreas_Response response)
		{
			if (response.error == null)
			{
				callback(response.areas);
			}
			else
			{
				Log.Error(response.error);
			}
		}));
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x00085D34 File Offset: 0x00084134
	public void GetAreaFlagStatus(string areaId, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.GetAreaFlagStatus(areaId, delegate(FlagStatus_Response response)
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

	// Token: 0x06000F93 RID: 3987 RVA: 0x00085D6C File Offset: 0x0008416C
	public void ToggleAreaFlag(string areaId, string reason, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.ToggleAreaFlag(areaId, reason, delegate(FlagStatus_Response response)
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

	// Token: 0x06000F94 RID: 3988 RVA: 0x00085DA8 File Offset: 0x000841A8
	public void RenameArea(string areaId, string newName, Action<bool, string> callback)
	{
		base.StartCoroutine(Managers.serverManager.RenameArea(areaId, newName, delegate(RenameArea_Response response)
		{
			if (response.error == null)
			{
				if (string.IsNullOrEmpty(response.reasonFailed))
				{
					this.currentAreaName = newName;
					this.currentAreaUrlName = this.GetUrlNameFromName(newName);
				}
				callback(response.ok, response.reasonFailed);
			}
			else
			{
				callback(false, response.error);
			}
		}));
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x00085DF4 File Offset: 0x000841F4
	public void SetFavoriteArea(string areaId, bool isFavorited, Action<bool> callback)
	{
		base.StartCoroutine(Managers.serverManager.SetFavoriteArea(areaId, isFavorited, delegate(SetFavoriteArea_Response response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
				callback(false);
			}
			else
			{
				callback(response.ok);
			}
		}));
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x00085E30 File Offset: 0x00084230
	public void SetHomeArea(Action<bool> callback)
	{
		this.currentAreaIsHomeArea = true;
		Managers.personManager.ourPerson.homeAreaId = this.currentAreaId;
		base.StartCoroutine(Managers.serverManager.SetHomeArea(this.currentAreaId, delegate(ResponseBase response)
		{
			callback(response.error == null);
		}));
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x00085E8C File Offset: 0x0008428C
	public void GetThingIdsInAreaByOtherCreators(Action<bool, List<string>> callback)
	{
		this.currentAreaIsHomeArea = true;
		base.StartCoroutine(Managers.serverManager.GetThingIdsInAreaByOtherCreators(this.currentAreaId, delegate(GetThingIdsInAreaByOtherCreators_Response response)
		{
			callback(response.error == null, response.thingIds);
		}));
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x00085ED0 File Offset: 0x000842D0
	public int GetGameObjectCount()
	{
		GameObject[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		return array.Length;
	}

	// Token: 0x06000F99 RID: 3993 RVA: 0x00085EF5 File Offset: 0x000842F5
	public void TransportToRandomArea()
	{
		base.StartCoroutine(Managers.serverManager.GetRandomArea(delegate(GetRandomArea_Response response)
		{
			if (response.error == null)
			{
				if (!string.IsNullOrEmpty(response.areaId))
				{
					this.TryTransportToAreaById(response.areaId);
				}
				else
				{
					Log.Error("GetRandomArea returned null areaId");
					Managers.errorManager.BeepError();
				}
			}
			else
			{
				Managers.errorManager.BeepError();
			}
		}));
	}

	// Token: 0x06000F9A RID: 3994 RVA: 0x00085F14 File Offset: 0x00084314
	public void UndoLastDeletion()
	{
		if (this.weAreEditorOfCurrentArea && !string.IsNullOrEmpty(this.lastDeletionId))
		{
			string lastDeletionId = this.lastDeletionId;
			this.lastDeletionId = null;
			Managers.soundManager.Play("putDown", this.lastDeletionPosition, 0.35f, false, false);
			base.StartCoroutine(Managers.personManager.DoPlaceThing(lastDeletionId, this.lastDeletionPosition, this.lastDeletionRotation));
		}
		else
		{
			Managers.errorManager.BeepError();
		}
	}

	// Token: 0x06000F9B RID: 3995 RVA: 0x00085F93 File Offset: 0x00084393
	public void SetAreaToTransportToAfterNextAreaLoad(string areaName, float seconds)
	{
		this.areaToTransportToAfterNextAreaLoad = areaName;
		this.areaToTransportToAfterNextAreaLoadSeconds = seconds;
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x00085FA3 File Offset: 0x000843A3
	public void ClearAreaToTransportToAfterNextAreaLoad()
	{
		this.areaToTransportToAfterNextAreaLoad = null;
		this.areaToTransportToAfterNextAreaLoadSeconds = 0f;
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x00085FB8 File Offset: 0x000843B8
	public void SetPositionAndRotationToTransportToToCurrent()
	{
		if (Managers.personManager.ourPerson != null)
		{
			Transform transform = Managers.personManager.ourPerson.transform;
			if (transform != null)
			{
				this.positionToTransportToAfterAreaLoad = transform.position;
				this.rotationToTransportToAfterAreaLoad = transform.rotation;
			}
		}
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x0008600E File Offset: 0x0008440E
	public void SetPositionAndRotationToTransportToToPrevious()
	{
		this.positionToTransportToAfterAreaLoad = this.previousAreaPosition;
		this.rotationToTransportToAfterAreaLoad = this.previousAreaRotation;
	}

	// Token: 0x06000F9F RID: 3999 RVA: 0x00086028 File Offset: 0x00084428
	public AreaOverview GetAndRemoveFirstDistinctAreaOverview(List<AreaOverview> areaOverviews, List<AreaOverview> existingList)
	{
		AreaOverview areaOverview = null;
		while (areaOverviews.Count >= 1)
		{
			AreaOverview areaOverview2 = areaOverviews[0];
			areaOverview = new AreaOverview();
			areaOverview.id = areaOverview2.id;
			areaOverview.name = areaOverview2.name;
			areaOverview.playerCount = areaOverview2.playerCount;
			areaOverviews.RemoveAt(0);
			if (!this.AreaOverviewExistsInList(areaOverview, existingList))
			{
				break;
			}
			areaOverview = null;
		}
		return areaOverview;
	}

	// Token: 0x06000FA0 RID: 4000 RVA: 0x0008609C File Offset: 0x0008449C
	private bool AreaOverviewExistsInList(AreaOverview area, List<AreaOverview> existingList)
	{
		bool flag = false;
		foreach (AreaOverview areaOverview in existingList)
		{
			if (areaOverview.id == area.id)
			{
				flag = true;
				break;
			}
		}
		return flag;
	}

	// Token: 0x06000FA1 RID: 4001 RVA: 0x0008610C File Offset: 0x0008450C
	public void CopyCurrentArea()
	{
		this.currentlyCopiedAreaId = this.currentAreaId;
		this.currentlyCopiedAreaName = this.currentAreaName;
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x00086128 File Offset: 0x00084528
	public void PasteCurrentlyCopiedArea()
	{
		Log.Info("Copying currentlyCopiedArea placement to current area", false);
		base.StartCoroutine(Managers.serverManager.CopyPlacements(this.currentlyCopiedAreaId, this.currentAreaId, delegate(AcknowledgeOperation_Response response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			else
			{
				Managers.broadcastNetworkManager.RaiseEvent_ReloadArea();
			}
		}));
	}

	// Token: 0x06000FA3 RID: 4003 RVA: 0x0008617C File Offset: 0x0008457C
	public void PermanentlyDeleteAllPlacementsInArea()
	{
		Log.Info("Deleting all area placements", false);
		base.StartCoroutine(Managers.serverManager.DeleteAllPlacements(this.currentAreaId, delegate(AcknowledgeOperation_Response response)
		{
			if (response.error != null)
			{
				Log.Error(response.error);
			}
			else
			{
				Managers.broadcastNetworkManager.RaiseEvent_ReloadArea();
			}
		}));
	}

	// Token: 0x06000FA4 RID: 4004 RVA: 0x000861C8 File Offset: 0x000845C8
	public void ReplaceAllOccurrencesOfThingInArea(string originalThingId, Thing newThing, bool isAuthority = false)
	{
		Log.Info("ReplaceAllOccurrencesOfThingInArea", false);
		if (isAuthority)
		{
			base.StartCoroutine(Managers.serverManager.ReplaceAllOccurrencesOfThing(this.currentAreaId, originalThingId, newThing.thingId, delegate(AcknowledgeOperation_Response response)
			{
				if (response.error != null)
				{
					Log.Error(response.error);
				}
			}));
			Managers.personManager.DoReplaceAllOccurrencesOfThingInArea(originalThingId, newThing.thingId);
		}
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Thing), true);
		foreach (Thing thing in componentsInChildren)
		{
			if (thing.thingId == originalThingId && thing != newThing && thing.gameObject.name != Universe.objectNameIfAlreadyDestroyed)
			{
				Managers.thingManager.ReplaceThing(thing, newThing);
			}
		}
	}

	// Token: 0x06000FA5 RID: 4005 RVA: 0x000862AF File Offset: 0x000846AF
	private void StartStopWatch()
	{
		Log.Debug("-- Starting StopWatch. --");
		this.stopWatch = new Stopwatch();
		this.stopWatch.Start();
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x000862D4 File Offset: 0x000846D4
	private void StopAndOutputStopWatch()
	{
		if (this.stopWatch != null)
		{
			Log.Debug("-- Stopping StopWatch. Took " + (float)this.stopWatch.ElapsedMilliseconds / 1000f + " --");
			this.stopWatch.Stop();
			this.stopWatch = null;
		}
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x0008632C File Offset: 0x0008472C
	public float? GetSecondsSinceJoiningArea()
	{
		float? num = null;
		float? num2 = this.timeAreaWasJoined;
		if (num2 != null)
		{
			float time = Time.time;
			float? num3 = this.timeAreaWasJoined;
			num = new float?(time - num3.Value);
		}
		return num;
	}

	// Token: 0x06000FA8 RID: 4008 RVA: 0x00086371 File Offset: 0x00084771
	public void HandleConnectionFailure()
	{
		this.isTransportInProgress = false;
		this.ReloadCurrentArea();
	}

	// Token: 0x06000FA9 RID: 4009 RVA: 0x00086380 File Offset: 0x00084780
	public void AlertVerySmallPersonScaleIfNeeded(float otherPersonScale)
	{
		if (otherPersonScale < 0.1f && !this.didAlertVerySmallPersonScaleForThisArea && !Managers.personManager.WeAreResized())
		{
			this.didAlertVerySmallPersonScaleForThisArea = true;
			string text = "Please note some in this area shrunk below " + ((int)Mathf.Round(10f)).ToString() + "%";
			Managers.dialogManager.ShowInfo(text, true, true, -1, DialogType.Start, 1f, false, TextColor.Default, TextAlignment.Left);
		}
	}

	// Token: 0x06000FAA RID: 4010 RVA: 0x000863FC File Offset: 0x000847FC
	public float GetBehaviorScriptVariable(string variableName)
	{
		float num = 0f;
		float num2;
		if (this.behaviorScriptVariables.TryGetValue(variableName, out num2))
		{
			num = num2;
		}
		return num;
	}

	// Token: 0x06000FAB RID: 4011 RVA: 0x00086425 File Offset: 0x00084825
	public void SetBehaviorScriptVariable(string variableName, float value)
	{
		this.behaviorScriptVariables[variableName] = value;
	}

	// Token: 0x06000FAC RID: 4012 RVA: 0x00086434 File Offset: 0x00084834
	public void LimitVisibilityIfNeeded()
	{
		float? num = this.limitVisibilityMeters;
		if (num != null)
		{
			GameObject placements = Managers.thingManager.placements;
			Transform transform = placements.transform;
			Transform transform2 = Managers.personManager.ourPerson.Head.transform;
			Component[] componentsInChildren = placements.GetComponentsInChildren<Thing>();
			foreach (Thing thing in componentsInChildren)
			{
				if (thing.transform.parent == transform && !thing.benefitsFromShowingAtDistance && Vector3.Distance(transform2.position, thing.transform.position) > this.limitVisibilityMeters)
				{
					thing.gameObject.SetActive(false);
				}
			}
		}
	}

	// Token: 0x06000FAD RID: 4013 RVA: 0x0008651C File Offset: 0x0008491C
	public string GetAreaJsonForExport()
	{
		string text = string.Empty;
		text += "{";
		text += "\"placements\":[";
		Component[] componentsInChildren = Managers.thingManager.placements.GetComponentsInChildren(typeof(Thing), true);
		int num = 0;
		foreach (Thing thing in componentsInChildren)
		{
			if (thing.IsPlacement())
			{
				if (++num > 1)
				{
					text += ",";
				}
				text += "{";
				text = text + "\"i\":" + JsonHelper.GetJson(thing.thingId);
				text = text + ",\"p\":" + JsonHelper.GetJson(thing.originalPlacementPosition);
				text = text + ",\"r\":" + JsonHelper.GetJson(thing.originalPlacementRotation);
				if (thing.transform.localScale != Vector3.one)
				{
					text = text + ",\"s\":" + JsonHelper.GetJson(thing.transform.localScale);
				}
				if (thing.isLocked)
				{
					text = text + ",\"locked\":" + JsonHelper.GetJson(thing.isLocked);
				}
				if (thing.isInvisibleToEditors)
				{
					text = text + ",\"invisibleToEditors\":" + JsonHelper.GetJson(thing.isInvisibleToEditors);
				}
				float? distanceToShow = thing.distanceToShow;
				if (distanceToShow != null)
				{
					object obj = text;
					object obj2 = ",\"distanceToShow\":";
					float? distanceToShow2 = thing.distanceToShow;
					text = obj + obj2 + distanceToShow2.Value;
				}
				if (thing.suppressScriptsAndStates)
				{
					text = text + ",\"suppressScriptsAndStates\":" + JsonHelper.GetJson(thing.suppressScriptsAndStates);
				}
				if (thing.suppressCollisions)
				{
					text = text + ",\"suppressCollisions\":" + JsonHelper.GetJson(thing.suppressCollisions);
				}
				if (thing.suppressLights)
				{
					text = text + ",\"suppressLights\":" + JsonHelper.GetJson(thing.suppressLights);
				}
				if (thing.suppressParticles)
				{
					text = text + ",\"suppressParticles\":" + JsonHelper.GetJson(thing.suppressParticles);
				}
				if (thing.suppressHoldable)
				{
					text = text + ",\"suppressHoldable\":" + JsonHelper.GetJson(thing.suppressHoldable);
				}
				if (thing.suppressShowAtDistance)
				{
					text = text + ",\"suppressShowAtDistance\":" + JsonHelper.GetJson(thing.suppressShowAtDistance);
				}
				text += "}";
			}
		}
		text += "]";
		string settingsJson = this.GetSettingsJson(true);
		if (!string.IsNullOrEmpty(settingsJson) && settingsJson != "{}")
		{
			text = text + ",\"settings\":" + settingsJson;
		}
		return text + "}";
	}

	// Token: 0x04000FD4 RID: 4052
	private bool currentAreaIsTransit;

	// Token: 0x04000FE2 RID: 4066
	public bool didTeleportMoveInThisArea;

	// Token: 0x04000FE4 RID: 4068
	private float areaToTransportToAfterNextAreaLoadSeconds;

	// Token: 0x04000FE5 RID: 4069
	private Vector3 positionToTransportToAfterAreaLoad = Vector3.zero;

	// Token: 0x04000FE6 RID: 4070
	private Quaternion rotationToTransportToAfterAreaLoad = Quaternion.identity;

	// Token: 0x04000FE7 RID: 4071
	public string previousAreaUrlName;

	// Token: 0x04000FE8 RID: 4072
	private Vector3 previousAreaPosition = Vector3.zero;

	// Token: 0x04000FE9 RID: 4073
	private Quaternion previousAreaRotation = Quaternion.identity;

	// Token: 0x04000FEA RID: 4074
	public string areaToLoadForTesting;

	// Token: 0x04000FEB RID: 4075
	private string areaToLoadDueToStartupParameter;

	// Token: 0x04000FEC RID: 4076
	public int sentHomeAfterDenialCount;

	// Token: 0x04000FED RID: 4077
	public const int MAX_SENT_HOME_COUNT = 20;

	// Token: 0x04000FEE RID: 4078
	public const float SENT_HOME_PAUSE_SECS = 5f;

	// Token: 0x04000FEF RID: 4079
	public Dictionary<string, float> behaviorScriptVariables = new Dictionary<string, float>();

	// Token: 0x04000FF0 RID: 4080
	public const int maxAllowedRenames = 10;

	// Token: 0x04000FF1 RID: 4081
	private const int areaUrlNameMinLength = 7;

	// Token: 0x04000FF2 RID: 4082
	private const int areaUrlNameMaxLength = 40;

	// Token: 0x04000FF3 RID: 4083
	public int placementsLoadingCounter;

	// Token: 0x04000FF4 RID: 4084
	public AreaRights rights = new AreaRights();

	// Token: 0x04000FF5 RID: 4085
	public bool didShowAllowInvisibilityWarning;

	// Token: 0x04000FF6 RID: 4086
	private string thingNameToSendToAfterTransport = string.Empty;

	// Token: 0x04000FF7 RID: 4087
	public bool containsThingPartWithImage;

	// Token: 0x04000FF8 RID: 4088
	private EnvironmentChangerDataCollection environmentChangersToApply;

	// Token: 0x04000FFA RID: 4090
	private Vector3 lastDeletionPosition;

	// Token: 0x04000FFB RID: 4091
	private Quaternion lastDeletionRotation;

	// Token: 0x04000FFC RID: 4092
	public string INFO_CurrentAreaId;

	// Token: 0x04000FFF RID: 4095
	private string[] areasToSnapshot = new string[0];

	// Token: 0x04001000 RID: 4096
	private int areasToSnapshotIndex = -1;

	// Token: 0x04001001 RID: 4097
	public float rotationAfterTransport;

	// Token: 0x04001002 RID: 4098
	private Stopwatch stopWatch;

	// Token: 0x04001004 RID: 4100
	private float? timeAreaWasJoined;

	// Token: 0x04001005 RID: 4101
	private List<string> keywordsToHear = new List<string>();

	// Token: 0x04001006 RID: 4102
	private bool didAlertVerySmallPersonScaleForThisArea;

	// Token: 0x04001007 RID: 4103
	public const string gravityKey = "gravity";

	// Token: 0x04001008 RID: 4104
	public bool didRPCFinalizeLoadedAllPlacements;

	// Token: 0x04001009 RID: 4105
	public static string areaToLoadAfterSceneChange;

	// Token: 0x0400100A RID: 4106
	public float? limitVisibilityMeters;

	// Token: 0x0400100B RID: 4107
	private int cleanUpBeforeTransportCount;

	// Token: 0x0400100C RID: 4108
	public Light environmentLight;

	// Token: 0x0400100D RID: 4109
	public bool sunOmitsShadow;
}
