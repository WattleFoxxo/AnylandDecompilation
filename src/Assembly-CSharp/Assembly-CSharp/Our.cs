using System;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000206 RID: 518
public class Our : MonoBehaviour
{
	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06001491 RID: 5265 RVA: 0x000B6FE8 File Offset: 0x000B53E8
	// (set) Token: 0x06001492 RID: 5266 RVA: 0x000B6FEF File Offset: 0x000B53EF
	public static TopographyId lastPreferentialHandSide { get; private set; }

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06001493 RID: 5267 RVA: 0x000B6FF7 File Offset: 0x000B53F7
	// (set) Token: 0x06001494 RID: 5268 RVA: 0x000B6FFE File Offset: 0x000B53FE
	public static bool removeOriginalWhenIncluding { get; private set; }

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06001495 RID: 5269 RVA: 0x000B7006 File Offset: 0x000B5406
	// (set) Token: 0x06001496 RID: 5270 RVA: 0x000B700D File Offset: 0x000B540D
	public static bool showExtraMirrors { get; private set; }

	// Token: 0x06001497 RID: 5271 RVA: 0x000B7018 File Offset: 0x000B5418
	private void Awake()
	{
		Our.lastPreferentialHandSide = ((PlayerPrefs.GetInt("lastPreferentialHandSide", 10) != 10) ? TopographyId.Right : TopographyId.Left);
		Our.gridSize = PlayerPrefs.GetFloat("gridSize", 1f);
		Our.canEditFly = PlayerPrefs.GetInt("canEditFly", 0) == 1;
		Our.seeInvisibleAsEditor = PlayerPrefs.GetInt("seeInvisibleAsEditor", 0) == 1;
		Our.touchUncollidableAsEditor = PlayerPrefs.GetInt("touchUncollidableAsEditor", 0) == 1;
		Our.useSmoothMovement = PlayerPrefs.GetInt("useSmoothMovement", 0) == 1;
		Our.useSmoothRiding = PlayerPrefs.GetInt("useSmoothRiding", 0) == 1;
		Our.teleportLaserAutoTargetsGround = PlayerPrefs.GetInt("teleportLaserAutoTargetsGround", 0) == 1;
		Our.dynamicHands = PlayerPrefs.GetInt("dynamicHands_v3", 1) == 1;
		Our.contextHighlightPlacements = PlayerPrefs.GetInt("contextHighlightPlacements", 1) == 1;
		Our.removeOriginalWhenIncluding = PlayerPrefs.GetInt("removeOriginalWhenIncluding", 0) == 1;
		Our.createThingsInDesktopMode = PlayerPrefs.GetInt("createThingsInDesktopMode", 0) == 1;
		Our.lockLegMovement = PlayerPrefs.GetInt("lockLegMovement", 0) == 1;
		Our.onlyFriendsCanPingUs = PlayerPrefs.GetInt("onlyFriendsCanPingUs", 0) == 1;
		Our.stopPingsAndAlerts = PlayerPrefs.GetInt("stopPingsAndAlerts", 0) == 1;
		if (Universe.features.scriptsAsEditor)
		{
			Our.suppressBehaviorScriptsAsEditor = PlayerPrefs.GetInt("suppressBehaviorScriptsAsEditor", 0) == 1;
		}
		else
		{
			Our.suppressBehaviorScriptsAsEditor = true;
		}
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x000B717E File Offset: 0x000B557E
	public static void SetCanEditFly(bool state)
	{
		Our.canEditFly = state;
		PlayerPrefs.SetInt("canEditFly", (!state) ? 0 : 1);
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x000B719D File Offset: 0x000B559D
	public static void SetGridSize(float size)
	{
		Our.gridSize = size;
		if (Our.gridSize == 1f)
		{
			PlayerPrefs.DeleteKey("gridSize");
		}
		else
		{
			PlayerPrefs.SetFloat("gridSize", Our.gridSize);
		}
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x000B71D2 File Offset: 0x000B55D2
	public static void SetUseSmoothMovement(bool state)
	{
		Our.useSmoothMovement = state;
		PlayerPrefs.SetInt("useSmoothMovement", (!state) ? 0 : 1);
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x000B71F1 File Offset: 0x000B55F1
	public static void SetUseSmoothRiding(bool state)
	{
		Our.useSmoothRiding = state;
		PlayerPrefs.SetInt("useSmoothRiding", (!state) ? 0 : 1);
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x000B7210 File Offset: 0x000B5610
	public static void SetTeleportLaserAutoTargetsGround(bool state)
	{
		Our.teleportLaserAutoTargetsGround = state;
		PlayerPrefs.SetInt("teleportLaserAutoTargetsGround", (!state) ? 0 : 1);
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x000B722F File Offset: 0x000B562F
	public static void SetDynamicHands(bool state)
	{
		Our.dynamicHands = state;
		PlayerPrefs.SetInt("dynamicHands_v3", (!state) ? 0 : 1);
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x000B724E File Offset: 0x000B564E
	public static void SetContextHighlightPlacements(bool state)
	{
		Our.contextHighlightPlacements = state;
		PlayerPrefs.SetInt("contextHighlightPlacements", (!state) ? 0 : 1);
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x000B726D File Offset: 0x000B566D
	public static void SetCreateThingsInDesktopMode(bool state)
	{
		Our.createThingsInDesktopMode = state;
		PlayerPrefs.SetInt("createThingsInDesktopMode", (!state) ? 0 : 1);
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x000B728C File Offset: 0x000B568C
	public static void SetLockLegMovement(bool state)
	{
		Our.lockLegMovement = state;
		PlayerPrefs.SetInt("lockLegMovement", (!state) ? 0 : 1);
	}

	// Token: 0x060014A1 RID: 5281 RVA: 0x000B72AB File Offset: 0x000B56AB
	public static void SetOnlyFriendsCanPingUs(bool state)
	{
		Our.onlyFriendsCanPingUs = state;
		PlayerPrefs.SetInt("onlyFriendsCanPingUs", (!state) ? 0 : 1);
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x000B72CA File Offset: 0x000B56CA
	public static void SetStopPingsAndAlerts(bool state)
	{
		Our.stopPingsAndAlerts = state;
		PlayerPrefs.SetInt("stopPingsAndAlerts", (!state) ? 0 : 1);
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x000B72E9 File Offset: 0x000B56E9
	public static void SetRemoveOriginalWhenIncluding(bool state)
	{
		Our.removeOriginalWhenIncluding = state;
		PlayerPrefs.SetInt("removeOriginalWhenIncluding", (!state) ? 0 : 1);
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x000B7308 File Offset: 0x000B5708
	public static void SetSuppressBehaviorScriptsAsEditor(bool state)
	{
		Our.suppressBehaviorScriptsAsEditor = state;
		PlayerPrefs.SetInt("suppressBehaviorScriptsAsEditor", (!state) ? 0 : 1);
	}

	// Token: 0x060014A5 RID: 5285 RVA: 0x000B7328 File Offset: 0x000B5728
	public static void SetSeeInvisibleAsEditor(bool state)
	{
		Our.seeInvisibleAsEditor = state;
		PlayerPrefs.SetInt("seeInvisibleAsEditor", (!state) ? 0 : 1);
		if (Managers.thingManager != null && Managers.areaManager != null && Managers.areaManager.weAreEditorOfCurrentArea)
		{
			Managers.thingManager.UpdateAllVisibilityAndCollision(false);
		}
	}

	// Token: 0x060014A6 RID: 5286 RVA: 0x000B738C File Offset: 0x000B578C
	public static void SetShowExtraMirrors(bool state)
	{
		Our.showExtraMirrors = state;
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x000B7394 File Offset: 0x000B5794
	public static void SetTouchUncollidableAsEditor(bool state)
	{
		Our.touchUncollidableAsEditor = state;
		PlayerPrefs.SetInt("touchUncollidableAsEditor", (!state) ? 0 : 1);
		if (Managers.thingManager != null && Managers.areaManager != null && Managers.areaManager.weAreEditorOfCurrentArea)
		{
			Managers.thingManager.UpdateAllVisibilityAndCollision(false);
		}
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x000B73F8 File Offset: 0x000B57F8
	public static void SetPreferentialHandSide(Hand hand)
	{
		if (hand != null)
		{
			Our.lastPreferentialHandSide = ((!(hand.name == "HandCoreLeft")) ? TopographyId.Right : TopographyId.Left);
			if (Managers.browserManager != null)
			{
				Managers.browserManager.SetDominantPointerHand(Our.lastPreferentialHandSide);
			}
		}
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x000B7453 File Offset: 0x000B5853
	private void OnApplicationQuit()
	{
		PlayerPrefs.SetInt("lastPreferentialHandSide", (int)Our.lastPreferentialHandSide);
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x000B7464 File Offset: 0x000B5864
	public static void SetLastTransformHandled(Transform thisTransform)
	{
		Our.lastTransformHandled = thisTransform;
		Our.lastTransformHandledStartScale = thisTransform.localScale;
		Our.lastTransformHandledStartPosition = thisTransform.position;
		Our.checkForOneSideExtrusionThisTurn = true;
		Our.oneSideExtrusionDirectionFound = null;
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x000B74A4 File Offset: 0x000B58A4
	public static void AutoSetLastTransformHandledIfOnlySingleThingPart()
	{
		if (CreationHelper.thingBeingEdited != null && CreationHelper.thingBeingEdited.active)
		{
			Component[] componentsInChildren = CreationHelper.thingBeingEdited.GetComponentsInChildren<ThingPart>();
			if (componentsInChildren.Length == 1)
			{
				Our.SetLastTransformHandled(componentsInChildren[0].transform);
			}
		}
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x000B74F4 File Offset: 0x000B58F4
	public static GameObject GetCurrentNonStartDialog()
	{
		GameObject gameObject = null;
		GameObject @object = Managers.treeManager.GetObject("/OurPersonRig/HandCoreLeft");
		GameObject object2 = Managers.treeManager.GetObject("/OurPersonRig/HandCoreRight");
		if (@object != null && object2 != null)
		{
			Hand component = @object.GetComponent<Hand>();
			Hand component2 = object2.GetComponent<Hand>();
			if (component != null && component.currentDialog != null && component.currentDialogType != DialogType.Start)
			{
				gameObject = component.currentDialog;
			}
			else if (component2 != null && component2.currentDialog && component2.currentDialogType != DialogType.Start)
			{
				gameObject = component2.currentDialog;
			}
		}
		return gameObject;
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x000B75B8 File Offset: 0x000B59B8
	public static void SetMode(EditModes newMode, bool forceMode = false)
	{
		if (newMode != Our.mode)
		{
			if (newMode == EditModes.Area && CrossDevice.desktopMode && !forceMode && !Our.createThingsInDesktopMode)
			{
				return;
			}
			if (Our.mode == EditModes.Environment)
			{
				Our.lastTransformHandled = null;
			}
			Our.previousMode = Our.mode;
			Our.mode = newMode;
			Our.UpdateInventoryBoxPosition();
			Our.UpdateModeMarkers();
			if ((Our.previousMode == EditModes.Inventory || newMode == EditModes.Inventory) && Managers.thingManager != null)
			{
				Managers.thingManager.UpdateAllVisibilityAndCollision(false);
			}
		}
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x000B764A File Offset: 0x000B5A4A
	public static void ToggleMode(EditModes modeToToggle)
	{
		if (Our.mode == modeToToggle)
		{
			Our.SetMode(EditModes.None, false);
		}
		else
		{
			Our.SetMode(modeToToggle, false);
		}
	}

	// Token: 0x060014AF RID: 5295 RVA: 0x000B766C File Offset: 0x000B5A6C
	public static void SetPreviousMode()
	{
		if (Our.previousMode != EditModes.Thing && Our.previousMode != EditModes.Area && Our.previousMode != EditModes.Body)
		{
			Our.previousMode = EditModes.None;
		}
		else if (Managers.areaManager != null && !Managers.areaManager.weAreEditorOfCurrentArea && Our.previousMode == EditModes.Area)
		{
			Our.previousMode = EditModes.None;
		}
		Our.SetMode(Our.previousMode, false);
	}

	// Token: 0x060014B0 RID: 5296 RVA: 0x000B76E0 File Offset: 0x000B5AE0
	public static void UpdateInventoryBoxPosition()
	{
		GameObject gameObject = GameObject.FindWithTag("InventoryBiggerCollider");
		if (gameObject != null)
		{
			Vector3 localPosition = gameObject.transform.localPosition;
			localPosition.z = ((Our.mode != EditModes.Body) ? (-0.25f) : (-0.475f));
			gameObject.transform.localPosition = localPosition;
		}
	}

	// Token: 0x060014B1 RID: 5297 RVA: 0x000B773D File Offset: 0x000B5B3D
	public static void UpdateModeMarkers()
	{
		Our.UpdateAttachmentPointSpheres();
		Our.UpdateRings();
	}

	// Token: 0x060014B2 RID: 5298 RVA: 0x000B774C File Offset: 0x000B5B4C
	public static void UpdateAttachmentPointSpheres()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("AttachmentPointSphere");
		bool flag = Managers.personManager.ourPerson.HasHeadAttachment();
		foreach (GameObject gameObject in array)
		{
			bool flag2 = !(gameObject.name == "HeadAttachmentPointSphere") && !(gameObject.name == "ArmAttachmentPointSphere") && !flag;
			bool flag3 = Our.mode == EditModes.Body && !flag2;
			AttachmentPointSphere component = gameObject.GetComponent<AttachmentPointSphere>();
			component.SetDoHighlight(flag3);
			Collider component2 = gameObject.GetComponent<Collider>();
			component2.isTrigger = flag3;
			component2.enabled = flag3;
			Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
			if (flag3)
			{
				if (rigidbody == null)
				{
					rigidbody = gameObject.AddComponent<Rigidbody>();
				}
				rigidbody.useGravity = false;
				rigidbody.isKinematic = true;
				rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			}
			else if (rigidbody != null)
			{
				global::UnityEngine.Object.Destroy(rigidbody);
			}
		}
	}

	// Token: 0x060014B3 RID: 5299 RVA: 0x000B7864 File Offset: 0x000B5C64
	private static void UpdateRings()
	{
		if (Managers.personManager != null && Managers.personManager.ourPerson != null && (Our.mode == EditModes.Area || Our.mode == EditModes.None))
		{
			Managers.personManager.ourPerson.SetRingsGlow(Our.mode == EditModes.Area);
		}
	}

	// Token: 0x060014B4 RID: 5300 RVA: 0x000B78C4 File Offset: 0x000B5CC4
	public static bool WeCanFly()
	{
		return Our.canEditFly && Managers.areaManager != null && Managers.areaManager.weAreEditorOfCurrentArea && (Our.mode == EditModes.Area || Our.mode == EditModes.Thing);
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x000B7914 File Offset: 0x000B5D14
	public static bool IsMasterClient(bool errOnSideOfTrueIfStillJoining = false)
	{
		bool flag = Managers.personManager != null && Managers.personManager.ourPerson != null && Managers.personManager.ourPerson.isMasterClient;
		if (!flag)
		{
			bool flag2 = string.IsNullOrEmpty(Managers.broadcastNetworkManager.INFO_Current_Room);
			if (flag2)
			{
				flag = true;
			}
		}
		return flag;
	}

	// Token: 0x0400122D RID: 4653
	public static EditModes mode = EditModes.None;

	// Token: 0x0400122E RID: 4654
	public static EditModes previousMode = EditModes.None;

	// Token: 0x0400122F RID: 4655
	public static string thingIdOfInterest = null;

	// Token: 0x04001230 RID: 4656
	public static string personIdOfInterest = null;

	// Token: 0x04001231 RID: 4657
	public static string areaIdOfInterest = null;

	// Token: 0x04001232 RID: 4658
	public static string personNameOfInterest = null;

	// Token: 0x04001233 RID: 4659
	public static bool useSmoothRiding = false;

	// Token: 0x04001234 RID: 4660
	public const string useSmoothRiding_key = "useSmoothRiding";

	// Token: 0x04001235 RID: 4661
	public static bool snapThingsToGrid = false;

	// Token: 0x04001236 RID: 4662
	public const float gridSizeDefault = 1f;

	// Token: 0x04001237 RID: 4663
	public static float gridSize = 0f;

	// Token: 0x04001238 RID: 4664
	private const string gridSize_key = "gridSize";

	// Token: 0x04001239 RID: 4665
	public static DialogType dialogToGoBackTo = DialogType.None;

	// Token: 0x0400123A RID: 4666
	public static Transform lastTransformHandled = null;

	// Token: 0x0400123B RID: 4667
	public static Vector3 lastTransformHandledStartScale;

	// Token: 0x0400123C RID: 4668
	public static Vector3 lastTransformHandledStartPosition;

	// Token: 0x0400123D RID: 4669
	public static Vector3 lastThingStartScale;

	// Token: 0x0400123E RID: 4670
	public static Vector3 lastThingStartPosition;

	// Token: 0x0400123F RID: 4671
	public static string lastAreasSearchText = null;

	// Token: 0x04001240 RID: 4672
	public static bool checkForOneSideExtrusionThisTurn = false;

	// Token: 0x04001241 RID: 4673
	public static Vector3? oneSideExtrusionDirectionFound = null;

	// Token: 0x04001242 RID: 4674
	public static int lastSuggestedAreasPage = -1;

	// Token: 0x04001243 RID: 4675
	public static bool suppressBehaviorScriptsAsEditor = false;

	// Token: 0x04001244 RID: 4676
	private const string suppressBehaviorScriptsAsEditor_key = "suppressBehaviorScriptsAsEditor";

	// Token: 0x04001245 RID: 4677
	public static bool canEditFly = false;

	// Token: 0x04001246 RID: 4678
	private const string canEditFly_key = "canEditFly";

	// Token: 0x04001247 RID: 4679
	public static bool seeInvisibleAsEditor = false;

	// Token: 0x04001248 RID: 4680
	private const string seeInvisibleAsEditor_key = "seeInvisibleAsEditor";

	// Token: 0x04001249 RID: 4681
	public static bool touchUncollidableAsEditor = false;

	// Token: 0x0400124A RID: 4682
	private const string touchUncollidableAsEditor_key = "touchUncollidableAsEditor";

	// Token: 0x0400124B RID: 4683
	public static bool useSmoothMovement = false;

	// Token: 0x0400124C RID: 4684
	public const string useSmoothMovement_key = "useSmoothMovement";

	// Token: 0x0400124D RID: 4685
	public static bool teleportLaserAutoTargetsGround = true;

	// Token: 0x0400124E RID: 4686
	private const string teleportLaserAutoTargetsGround_key = "teleportLaserAutoTargetsGround";

	// Token: 0x0400124F RID: 4687
	public static bool createThingsInDesktopMode = false;

	// Token: 0x04001250 RID: 4688
	private const string createThingsInDesktopMode_key = "createThingsInDesktopMode";

	// Token: 0x04001251 RID: 4689
	public static bool lockLegMovement = false;

	// Token: 0x04001252 RID: 4690
	private const string lockLegMovement_key = "lockLegMovement";

	// Token: 0x04001253 RID: 4691
	public static bool contextHighlightPlacements = true;

	// Token: 0x04001254 RID: 4692
	private const string contextHighlightPlacements_key = "contextHighlightPlacements";

	// Token: 0x04001256 RID: 4694
	private const string lastPreferentialHandSide_key = "lastPreferentialHandSide";

	// Token: 0x04001257 RID: 4695
	public static bool onlyFriendsCanPingUs = false;

	// Token: 0x04001258 RID: 4696
	private const string onlyFriendsCanPingUs_key = "onlyFriendsCanPingUs";

	// Token: 0x04001259 RID: 4697
	public static bool stopPingsAndAlerts = false;

	// Token: 0x0400125A RID: 4698
	private const string stopPingsAndAlerts_key = "stopPingsAndAlerts";

	// Token: 0x0400125C RID: 4700
	public const string removeOriginalWhenIncluding_key = "removeOriginalWhenIncluding";

	// Token: 0x0400125D RID: 4701
	public static bool dynamicHands = false;

	// Token: 0x0400125E RID: 4702
	private const string dynamicHands_key = "dynamicHands_v3";

	// Token: 0x0400125F RID: 4703
	public static bool doSnapThingAngles = false;

	// Token: 0x04001260 RID: 4704
	public static bool doSnapThingPosition = false;

	// Token: 0x04001261 RID: 4705
	public static bool disableAllThingSnapping = false;

	// Token: 0x04001262 RID: 4706
	public static bool useHelperLights = true;

	// Token: 0x04001263 RID: 4707
	public static Vector3? lastTeleportHitPoint = null;

	// Token: 0x04001265 RID: 4709
	public static AttachmentPointsMemory attachmentPointsMemory = new AttachmentPointsMemory();

	// Token: 0x04001266 RID: 4710
	public static bool useSlidyMovement = false;

	// Token: 0x04001267 RID: 4711
	private static VRBrowserHand browserPointer = null;
}
