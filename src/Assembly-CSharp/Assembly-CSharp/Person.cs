using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using ZenFulcrum.EmbeddedBrowser;

// Token: 0x02000207 RID: 519
[RequireComponent(typeof(PhotonView))]
public class Person : MonoBehaviour
{
	// Token: 0x17000256 RID: 598
	// (get) Token: 0x060014B8 RID: 5304 RVA: 0x000B7ADB File Offset: 0x000B5EDB
	// (set) Token: 0x060014B9 RID: 5305 RVA: 0x000B7AE3 File Offset: 0x000B5EE3
	public string userId { get; set; }

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x060014BA RID: 5306 RVA: 0x000B7AEC File Offset: 0x000B5EEC
	public string screenName
	{
		get
		{
			if (this.photonPlayer == null)
			{
				Log.Warning("photonPlayer was null when trying to get person screenName");
			}
			return (this.photonPlayer == null) ? "unknown" : Managers.personManager.GetScreenNameWithDiscloser(this.userId, this.photonPlayer.NickName);
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x060014BB RID: 5307 RVA: 0x000B7B3E File Offset: 0x000B5F3E
	public bool isMasterClient
	{
		get
		{
			return this.photonPlayer != null && this.photonPlayer.IsMasterClient;
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x060014BC RID: 5308 RVA: 0x000B7B59 File Offset: 0x000B5F59
	// (set) Token: 0x060014BD RID: 5309 RVA: 0x000B7B61 File Offset: 0x000B5F61
	public string homeAreaId { get; set; }

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x060014BE RID: 5310 RVA: 0x000B7B6A File Offset: 0x000B5F6A
	// (set) Token: 0x060014BF RID: 5311 RVA: 0x000B7B72 File Offset: 0x000B5F72
	public string statusText { get; set; }

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x060014C0 RID: 5312 RVA: 0x000B7B7B File Offset: 0x000B5F7B
	// (set) Token: 0x060014C1 RID: 5313 RVA: 0x000B7B83 File Offset: 0x000B5F83
	public bool isFindable { get; set; }

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x060014C2 RID: 5314 RVA: 0x000B7B8C File Offset: 0x000B5F8C
	// (set) Token: 0x060014C3 RID: 5315 RVA: 0x000B7B94 File Offset: 0x000B5F94
	public int ageInDays { get; set; }

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x060014C4 RID: 5316 RVA: 0x000B7B9D File Offset: 0x000B5F9D
	// (set) Token: 0x060014C5 RID: 5317 RVA: 0x000B7BA5 File Offset: 0x000B5FA5
	public int ageInSecs { get; set; }

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x060014C6 RID: 5318 RVA: 0x000B7BAE File Offset: 0x000B5FAE
	// (set) Token: 0x060014C7 RID: 5319 RVA: 0x000B7BB6 File Offset: 0x000B5FB6
	public bool isHardBanned { get; set; }

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x060014C8 RID: 5320 RVA: 0x000B7BBF File Offset: 0x000B5FBF
	// (set) Token: 0x060014C9 RID: 5321 RVA: 0x000B7BC7 File Offset: 0x000B5FC7
	public bool isSoftBanned { get; set; }

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x060014CA RID: 5322 RVA: 0x000B7BD0 File Offset: 0x000B5FD0
	// (set) Token: 0x060014CB RID: 5323 RVA: 0x000B7BD8 File Offset: 0x000B5FD8
	public bool showFlagWarning { get; set; }

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x060014CC RID: 5324 RVA: 0x000B7BE1 File Offset: 0x000B5FE1
	// (set) Token: 0x060014CD RID: 5325 RVA: 0x000B7BE9 File Offset: 0x000B5FE9
	public string[] flagTags { get; set; }

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x060014CE RID: 5326 RVA: 0x000B7BF2 File Offset: 0x000B5FF2
	// (set) Token: 0x060014CF RID: 5327 RVA: 0x000B7BFA File Offset: 0x000B5FFA
	public int areaCount { get; set; }

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x060014D0 RID: 5328 RVA: 0x000B7C03 File Offset: 0x000B6003
	// (set) Token: 0x060014D1 RID: 5329 RVA: 0x000B7C0B File Offset: 0x000B600B
	public int thingTagCount { get; set; }

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x060014D2 RID: 5330 RVA: 0x000B7C14 File Offset: 0x000B6014
	// (set) Token: 0x060014D3 RID: 5331 RVA: 0x000B7C1C File Offset: 0x000B601C
	public bool allThingsClonable { get; set; }

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x060014D4 RID: 5332 RVA: 0x000B7C25 File Offset: 0x000B6025
	// (set) Token: 0x060014D5 RID: 5333 RVA: 0x000B7C2D File Offset: 0x000B602D
	public bool hasEditTools { get; set; }

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x060014D6 RID: 5334 RVA: 0x000B7C36 File Offset: 0x000B6036
	// (set) Token: 0x060014D7 RID: 5335 RVA: 0x000B7C3E File Offset: 0x000B603E
	public bool hasEditToolsPermanently { get; set; }

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x060014D8 RID: 5336 RVA: 0x000B7C47 File Offset: 0x000B6047
	// (set) Token: 0x060014D9 RID: 5337 RVA: 0x000B7C4F File Offset: 0x000B604F
	public string editToolsExpiryDate { get; set; }

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x060014DA RID: 5338 RVA: 0x000B7C58 File Offset: 0x000B6058
	// (set) Token: 0x060014DB RID: 5339 RVA: 0x000B7C60 File Offset: 0x000B6060
	public bool isInEditToolsTrial { get; set; }

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x060014DC RID: 5340 RVA: 0x000B7C69 File Offset: 0x000B6069
	// (set) Token: 0x060014DD RID: 5341 RVA: 0x000B7C71 File Offset: 0x000B6071
	public bool wasEditToolsTrialEverActivated { get; set; }

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x060014DE RID: 5342 RVA: 0x000B7C7A File Offset: 0x000B607A
	// (set) Token: 0x060014DF RID: 5343 RVA: 0x000B7C82 File Offset: 0x000B6082
	public int timesEditToolsPurchased { get; set; }

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x060014E0 RID: 5344 RVA: 0x000B7C8B File Offset: 0x000B608B
	// (set) Token: 0x060014E1 RID: 5345 RVA: 0x000B7C93 File Offset: 0x000B6093
	public bool? isEditorHere { get; set; }

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x060014E2 RID: 5346 RVA: 0x000B7C9C File Offset: 0x000B609C
	// (set) Token: 0x060014E3 RID: 5347 RVA: 0x000B7CA4 File Offset: 0x000B60A4
	public GameObject Head { get; private set; }

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x060014E4 RID: 5348 RVA: 0x000B7CAD File Offset: 0x000B60AD
	// (set) Token: 0x060014E5 RID: 5349 RVA: 0x000B7CB5 File Offset: 0x000B60B5
	public GameObject AttachmentPointHead { get; private set; }

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x060014E6 RID: 5350 RVA: 0x000B7CBE File Offset: 0x000B60BE
	private bool useSmoothRiding
	{
		get
		{
			return !this.isOurPerson || CrossDevice.desktopMode || Our.useSmoothRiding;
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x060014E7 RID: 5351 RVA: 0x000B7CDD File Offset: 0x000B60DD
	// (set) Token: 0x060014E8 RID: 5352 RVA: 0x000B7CE5 File Offset: 0x000B60E5
	public bool amplifySpeech { get; private set; }

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x060014E9 RID: 5353 RVA: 0x000B7CEE File Offset: 0x000B60EE
	// (set) Token: 0x060014EA RID: 5354 RVA: 0x000B7CF6 File Offset: 0x000B60F6
	public int lastBehaviorScriptMessagesPerSecond { get; private set; }

	// Token: 0x060014EB RID: 5355 RVA: 0x000B7CFF File Offset: 0x000B60FF
	public void Awake()
	{
		this.SetGameObjectReferences();
		this.SetAttachmentPointReferencesById();
	}

	// Token: 0x060014EC RID: 5356 RVA: 0x000B7D0D File Offset: 0x000B610D
	public void Start()
	{
		this.photonView = base.GetComponent<PhotonView>();
		this.SetGameObjectReferences();
		this.SetAttachmentPointReferencesById();
		base.InvokeRepeating("HandleNameTagStatsCollectionAndDisplay", 1f, 1f);
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x000B7D3C File Offset: 0x000B613C
	public void AlertOthersOfMyBirthIfAppropriate()
	{
		if (this.isOurPerson && this.ageInSecs <= 600 && Managers.achievementManager != null && !Managers.achievementManager.DidAchieve(Achievement.AlertedOthersOfMyBirth))
		{
			base.Invoke("DoAlertOthersOfMyBirthIfAppropriate", 180f);
		}
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x000B7D98 File Offset: 0x000B6198
	private void SetGameObjectReferences()
	{
		this.Rig = base.gameObject;
		this.Head = Misc.FindObject(this.Rig, "HeadCore");
		this.LeftHand = Misc.FindObject(this.Rig, "HandCoreLeft");
		this.RightHand = Misc.FindObject(this.Rig, "HandCoreRight");
		this.Torso = Misc.FindObject(this.Rig, "Torso");
		string text = "HandSecondaryDot" + ((!this.isOurPerson) ? "Other" : string.Empty);
		if (this.LeftHand != null)
		{
			this.leftHandSecondaryDot = this.LeftHand.transform.Find(text);
		}
		if (this.RightHand != null)
		{
			this.rightHandSecondaryDot = this.RightHand.transform.Find(text);
		}
		this.AttachmentPointHead = Misc.FindObject(this.Rig, "HeadAttachmentPoint");
		this.AttachmentPointHeadTop = Misc.FindObject(this.Rig, "HeadTopAttachmentPoint");
		this.AttachmentPointTorsoLower = Misc.FindObject(this.Rig, "LowerTorsoAttachmentPoint");
		this.AttachmentPointTorsoUpper = Misc.FindObject(this.Rig, "UpperTorsoAttachmentPoint");
		this.AttachmentPointLegLeft = Misc.FindObject(this.Rig, "LegLeftAttachmentPoint");
		this.AttachmentPointLegRight = Misc.FindObject(this.Rig, "LegRightAttachmentPoint");
		this.AttachmentPointHandLeft = Misc.FindObject(this.Rig, "HandLeftAttachmentPoint");
		this.AttachmentPointHandRight = Misc.FindObject(this.Rig, "HandRightAttachmentPoint");
		this.AttachmentPointArmLeft = Misc.FindObject(this.Rig, "ArmLeftAttachmentPoint");
		this.AttachmentPointArmRight = Misc.FindObject(this.Rig, "ArmRightAttachmentPoint");
		Misc.AssertAllNotNull(new object[] { this.AttachmentPointHead, this.AttachmentPointHeadTop, this.AttachmentPointTorsoLower, this.AttachmentPointTorsoUpper, this.AttachmentPointLegLeft, this.AttachmentPointLegRight, this.AttachmentPointHandLeft, this.AttachmentPointHandRight, this.AttachmentPointArmLeft, this.AttachmentPointHandRight }, "An attachment point reference was null");
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x000B7FC8 File Offset: 0x000B63C8
	private void DoAlertOthersOfMyBirthIfAppropriate()
	{
		bool flag = Managers.achievementManager.DidAchieve(Achievement.MetPerson) && Managers.achievementManager.DidAchieve(Achievement.VisitedOtherArea);
		bool flag2 = Managers.areaManager.currentAreaIsHomeArea && Managers.areaManager.isPrivate;
		if (!flag && !flag2 && !Managers.achievementManager.DidAchieve(Achievement.AlertedOthersOfMyBirth))
		{
			bool flag3 = Managers.areaManager.currentAreaIsHomeArea && !Managers.browserManager.GuideIsShowing();
			if (flag3)
			{
				Managers.achievementManager.RegisterAchievement(Achievement.AlertedOthersOfMyBirth);
				Managers.personManager.RequestWelcome(delegate
				{
				});
			}
			else
			{
				base.Invoke("DoAlertOthersOfMyBirthIfAppropriate", 60f);
			}
		}
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x000B80A0 File Offset: 0x000B64A0
	private void SetAttachmentPointReferencesById()
	{
		this.AttachmentPointsById = new Dictionary<AttachmentPointId, GameObject>();
		this.AttachmentPointsById.Add(AttachmentPointId.Head, this.AttachmentPointHead);
		this.AttachmentPointsById.Add(AttachmentPointId.HeadTop, this.AttachmentPointHeadTop);
		this.AttachmentPointsById.Add(AttachmentPointId.TorsoLower, this.AttachmentPointTorsoLower);
		this.AttachmentPointsById.Add(AttachmentPointId.TorsoUpper, this.AttachmentPointTorsoUpper);
		this.AttachmentPointsById.Add(AttachmentPointId.LegLeft, this.AttachmentPointLegLeft);
		this.AttachmentPointsById.Add(AttachmentPointId.LegRight, this.AttachmentPointLegRight);
		this.AttachmentPointsById.Add(AttachmentPointId.ArmLeft, this.AttachmentPointArmLeft);
		this.AttachmentPointsById.Add(AttachmentPointId.ArmRight, this.AttachmentPointArmRight);
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x000B814C File Offset: 0x000B654C
	public void ConstructAttachments(AttachmentDataSet attachmentsData)
	{
		foreach (KeyValuePair<AttachmentPointId, AttachmentData> keyValuePair in attachmentsData)
		{
			AttachmentData value = keyValuePair.Value;
			base.StartCoroutine(this.AttachNewThing(keyValuePair.Key, value.thingId, value.position, Quaternion.Euler(value.rotation)));
		}
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x000B81D0 File Offset: 0x000B65D0
	public bool HasHeadAttachment()
	{
		return Misc.GetChildWithTag(this.AttachmentPointHead.transform, "Attachment") != null;
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x000B81ED File Offset: 0x000B65ED
	public bool HasTorsoUpperAttachment()
	{
		return Misc.GetChildWithTag(this.AttachmentPointTorsoUpper.transform, "Attachment") != null;
	}

	// Token: 0x060014F4 RID: 5364 RVA: 0x000B820C File Offset: 0x000B660C
	public void ConstructHeldThings(HeldThingsDataSet heldThingsData)
	{
		if (heldThingsData != null)
		{
			if (heldThingsData.leftHeldThingData != null)
			{
				Log.Info("ConstructHeldThings : Left Hand adding : " + heldThingsData.leftHeldThingData.thingId, false);
				if (!string.IsNullOrEmpty(heldThingsData.leftHeldThingData.thingId))
				{
					base.StartCoroutine(this.HoldNewThing(TopographyId.Left, heldThingsData.leftHeldThingData.thingId, heldThingsData.leftHeldThingData.position, heldThingsData.leftHeldThingData.rotation, null));
				}
				else
				{
					Log.Debug("Left hand thingId unusually empty during ConstructHeldThings, so ignoring");
				}
			}
			else
			{
				Log.Info("ConstructHeldThings : Left Hand empty", false);
			}
			if (heldThingsData.rightHeldThingData != null)
			{
				Log.Info("ConstructHeldThings : Right Hand adding : " + heldThingsData.rightHeldThingData.thingId, false);
				if (!string.IsNullOrEmpty(heldThingsData.rightHeldThingData.thingId))
				{
					base.StartCoroutine(this.HoldNewThing(TopographyId.Right, heldThingsData.rightHeldThingData.thingId, heldThingsData.rightHeldThingData.position, heldThingsData.rightHeldThingData.rotation, null));
				}
				else
				{
					Log.Debug("Right hand thingId unusually empty during ConstructHeldThings, so ignoring");
				}
			}
			else
			{
				Log.Info("ConstructHeldThings : Right Hand empty", false);
			}
		}
	}

	// Token: 0x060014F5 RID: 5365 RVA: 0x000B8348 File Offset: 0x000B6748
	public Hand GetAHand()
	{
		Hand hand = null;
		TopographyId topographyId = ((Our.lastPreferentialHandSide == TopographyId.None) ? TopographyId.Left : Our.lastPreferentialHandSide);
		GameObject handByTopographyId = this.GetHandByTopographyId(topographyId);
		if (handByTopographyId != null)
		{
			hand = handByTopographyId.GetComponent<Hand>();
		}
		return hand;
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x000B838A File Offset: 0x000B678A
	public void CloseAllDialogs()
	{
		this.CloseAllDialogsAtHand(TopographyId.Left);
		this.CloseAllDialogsAtHand(TopographyId.Right);
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x000B839C File Offset: 0x000B679C
	private void CloseAllDialogsAtHand(TopographyId topographyId)
	{
		GameObject handByTopographyId = this.GetHandByTopographyId(topographyId);
		if (handByTopographyId != null)
		{
			Hand component = handByTopographyId.GetComponent<Hand>();
			component.SwitchToNewDialog(DialogType.Start, string.Empty);
		}
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x000B83D2 File Offset: 0x000B67D2
	public void ResetPositionAndRotation()
	{
		base.transform.rotation = Quaternion.identity;
		this.ResetPosition();
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x000B83EA File Offset: 0x000B67EA
	public void ResetPosition()
	{
		base.transform.position = Vector3.zero;
		CrossDevice.rigPositionIsAuthority = true;
		if (this.isOurPerson)
		{
			Managers.personManager.CachePhotonRigLocation();
		}
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x000B8418 File Offset: 0x000B6818
	private void SetToPreciseCenterIndependentOfInRigPosition()
	{
		if (this.Rig != null && this.Head != null)
		{
			Vector3 position = base.transform.position;
			position.x += this.Rig.transform.position.x - this.Head.transform.position.x;
			position.z += this.Rig.transform.position.z - this.Head.transform.position.z;
			base.transform.position = position;
			CrossDevice.rigPositionIsAuthority = true;
			Managers.personManager.CachePhotonRigLocation();
		}
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x000B84F0 File Offset: 0x000B68F0
	public void InitializeFromPhotonPlayer(PhotonPlayer p)
	{
		this.photonPlayer = p;
		Log.Info("InitializeFromPhotonPlayer.  " + p.NickName, false);
		this.userId = p.userId;
		LocationData photonCachedRigLocation = this.GetPhotonCachedRigLocation();
		if (photonCachedRigLocation != null)
		{
			base.transform.localPosition = photonCachedRigLocation.position;
			base.transform.localEulerAngles = photonCachedRigLocation.rotation;
		}
		else
		{
			Log.Warning("Could not get initial cached rig location for player " + p.NickName);
		}
		float num = 1f;
		object obj;
		if (this.photonPlayer.customProperties.TryGetValue(PhotonCacheKeys.RigScale, out obj))
		{
			num = (float)obj;
		}
		this.Rig.transform.localScale = Misc.GetUniformVector3(num);
		object obj2;
		if (this.photonPlayer.customProperties.TryGetValue(PhotonCacheKeys.RidingBeacon, out obj2) && !string.IsNullOrEmpty((string)obj2))
		{
			RidingBeaconCache ridingBeaconCache = new RidingBeaconCache();
			ridingBeaconCache.InitFromJson((string)obj2);
			this.DoAddRidingBeacon_Remote(ridingBeaconCache.thingAttachedToSpecifierType, ridingBeaconCache.thingAttachedToSpecifierId, ridingBeaconCache.beaconPosition, ridingBeaconCache.beaconPositionOffset, ridingBeaconCache.beaconRotationOffset);
		}
		AttachmentDataSet photonCachedAttachmentDataSet = this.GetPhotonCachedAttachmentDataSet();
		this.ConstructAttachments(photonCachedAttachmentDataSet);
		HeldThingsDataSet photonCachedHeldThingsDataSet = this.GetPhotonCachedHeldThingsDataSet();
		this.ConstructHeldThings(photonCachedHeldThingsDataSet);
		Color? photonCachedHandColor = this.GetPhotonCachedHandColor();
		if (photonCachedHandColor != null)
		{
			this.SetHandsColor(photonCachedHandColor.Value);
		}
		object obj3;
		if (this.photonPlayer.customProperties.TryGetValue(PhotonCacheKeys.IsInvisibleWhereAllowed, out obj3))
		{
			this.isInvisibleWhereAllowed = (bool)obj3;
		}
		object obj4;
		if (this.photonPlayer.customProperties.TryGetValue(PhotonCacheKeys.AmplifySpeech, out obj4) && (bool)obj4)
		{
			this.SetAmplifySpeech(true);
		}
		if (!this.isOurPerson && this.Head != null)
		{
			this.speechIndicatorParticleSystem = this.Head.GetComponent<ParticleSystem>();
		}
		this.Initialized = true;
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x000B86E4 File Offset: 0x000B6AE4
	public AttachmentDataSet GetPhotonCachedAttachmentDataSet()
	{
		AttachmentDataSet attachmentDataSet = null;
		object obj;
		this.photonPlayer.customProperties.TryGetValue(PhotonCacheKeys.Attachments, out obj);
		if (obj != null)
		{
			string text = (string)obj;
			attachmentDataSet = new AttachmentDataSet(text);
			Log.Info("Got attachmentDataSetJSON:", false);
			Log.Info(text, false);
		}
		else
		{
			Log.Info("Got empty attachmentDataSetJSON", false);
		}
		return attachmentDataSet;
	}

	// Token: 0x060014FD RID: 5373 RVA: 0x000B8744 File Offset: 0x000B6B44
	public HeldThingsDataSet GetPhotonCachedHeldThingsDataSet()
	{
		HeldThingsDataSet heldThingsDataSet = null;
		object obj;
		this.photonPlayer.customProperties.TryGetValue(PhotonCacheKeys.HeldThings, out obj);
		if (obj != null)
		{
			string text = (string)obj;
			Log.Info("Got heldThingsDataSetJSON:", false);
			Log.Info(text, false);
			heldThingsDataSet = new HeldThingsDataSet(text);
		}
		else
		{
			Log.Info("Got empty heldThingsDataSetJSON", false);
		}
		return heldThingsDataSet;
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x000B87A4 File Offset: 0x000B6BA4
	public Color? GetPhotonCachedHandColor()
	{
		Color? color = null;
		object obj;
		this.photonPlayer.customProperties.TryGetValue(PhotonCacheKeys.HandColour, out obj);
		if (obj != null)
		{
			string text = (string)obj;
			Log.Info("Got handColorJSON:", false);
			Log.Info(text, false);
			color = new Color?(JsonUtility.FromJson<Color>(text));
		}
		else
		{
			Log.Info("Got empty handColorJSON", false);
		}
		return color;
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x000B8810 File Offset: 0x000B6C10
	public LocationData GetPhotonCachedRigLocation()
	{
		LocationData locationData = null;
		object obj;
		this.photonPlayer.customProperties.TryGetValue(PhotonCacheKeys.RigLocation, out obj);
		if (obj != null)
		{
			string text = (string)obj;
			Log.Info("Got rig location json:", false);
			Log.Info(text, false);
			locationData = JsonUtility.FromJson<LocationData>(text);
		}
		else
		{
			Log.Info("Got empty rig location json", false);
		}
		return locationData;
	}

	// Token: 0x06001500 RID: 5376 RVA: 0x000B8870 File Offset: 0x000B6C70
	public void VerifyPhotonCacheTestItem()
	{
		object obj;
		this.photonPlayer.customProperties.TryGetValue(PhotonCacheKeys.Test, out obj);
		if (obj != null)
		{
			string text = (string)obj;
			if (text != "x")
			{
				Log.Error("VerifyPhotonCacheTestItem failed! (wrong test string value)");
			}
			else
			{
				Log.Info("VerifyPhotonCacheTestItem passed", false);
			}
		}
		else
		{
			Log.Error("VerifyPhotonCacheTestItem failed! (null response for test key)");
		}
	}

	// Token: 0x06001501 RID: 5377 RVA: 0x000B88DC File Offset: 0x000B6CDC
	public HeldThingsDataSet GetHeldThingsDataSet()
	{
		GameObject handByTopographyId = this.GetHandByTopographyId(TopographyId.Left);
		GameObject handByTopographyId2 = this.GetHandByTopographyId(TopographyId.Right);
		GameObject thingInHand = this.GetThingInHand(handByTopographyId, false);
		GameObject thingInHand2 = this.GetThingInHand(handByTopographyId2, false);
		return new HeldThingsDataSet(thingInHand, thingInHand2);
	}

	// Token: 0x06001502 RID: 5378 RVA: 0x000B8914 File Offset: 0x000B6D14
	public void SetComponentNetworkViewIds(int[] viewIds)
	{
		GameObject gameObject = base.gameObject;
		GameObject gameObject2 = Misc.FindObject(gameObject, "HeadCore");
		GameObject gameObject3 = Misc.FindObject(gameObject, "HandCoreLeft");
		GameObject gameObject4 = Misc.FindObject(gameObject, "HandCoreRight");
		GameObject gameObject5 = Misc.FindObject(gameObject, "LegLeftAttachmentPoint");
		GameObject gameObject6 = Misc.FindObject(gameObject, "LegRightAttachmentPoint");
		int num = 0;
		gameObject.GetComponent<PhotonView>().viewID = viewIds[num++];
		gameObject2.GetComponent<PhotonView>().viewID = viewIds[num++];
		gameObject3.GetComponent<PhotonView>().viewID = viewIds[num++];
		gameObject4.GetComponent<PhotonView>().viewID = viewIds[num++];
		gameObject5.GetComponent<PhotonView>().viewID = viewIds[num++];
		gameObject6.GetComponent<PhotonView>().viewID = viewIds[num++];
	}

	// Token: 0x06001503 RID: 5379 RVA: 0x000B89E4 File Offset: 0x000B6DE4
	public int[] GetComponentNetworkViewIds()
	{
		GameObject gameObject = base.gameObject;
		GameObject gameObject2 = Misc.FindObject(gameObject, "HeadCore");
		GameObject gameObject3 = Misc.FindObject(gameObject, "HandCoreLeft");
		GameObject gameObject4 = Misc.FindObject(gameObject, "HandCoreRight");
		GameObject gameObject5 = Misc.FindObject(gameObject, "LegLeftAttachmentPoint");
		GameObject gameObject6 = Misc.FindObject(gameObject, "LegRightAttachmentPoint");
		int[] array = new int[6];
		int num = 0;
		array[num++] = gameObject.GetComponent<PhotonView>().viewID;
		array[num++] = gameObject2.GetComponent<PhotonView>().viewID;
		array[num++] = gameObject3.GetComponent<PhotonView>().viewID;
		array[num++] = gameObject4.GetComponent<PhotonView>().viewID;
		array[num++] = gameObject5.GetComponent<PhotonView>().viewID;
		array[num++] = gameObject6.GetComponent<PhotonView>().viewID;
		return array;
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x000B8AC4 File Offset: 0x000B6EC4
	public void ClearComponentNetworkViewIds()
	{
		Log.Info("Clearing Component Netowrk View Ids", false);
		GameObject gameObject = base.gameObject;
		GameObject gameObject2 = Misc.FindObject(gameObject, "HeadCore");
		GameObject gameObject3 = Misc.FindObject(gameObject, "HandCoreLeft");
		GameObject gameObject4 = Misc.FindObject(gameObject, "HandCoreRight");
		GameObject gameObject5 = Misc.FindObject(gameObject, "LegLeftAttachmentPoint");
		GameObject gameObject6 = Misc.FindObject(gameObject, "LegRightAttachmentPoint");
		if (gameObject != null)
		{
			gameObject.GetComponent<PhotonView>().viewID = 1;
		}
		if (gameObject3 != null)
		{
			gameObject3.GetComponent<PhotonView>().viewID = 2;
		}
		if (gameObject4 != null)
		{
			gameObject4.GetComponent<PhotonView>().viewID = 3;
		}
		if (gameObject2 != null)
		{
			gameObject2.GetComponent<PhotonView>().viewID = 4;
		}
		if (gameObject5 != null)
		{
			gameObject5.GetComponent<PhotonView>().viewID = 5;
		}
		if (gameObject6 != null)
		{
			gameObject6.GetComponent<PhotonView>().viewID = 6;
		}
	}

	// Token: 0x06001505 RID: 5381 RVA: 0x000B8BB8 File Offset: 0x000B6FB8
	public IEnumerator AttachNewThing(AttachmentPointId attachmentPointId, string thingId, Vector3 position, Quaternion rotation)
	{
		GameObject attachmentPoint = this.GetAttachmentPointById(attachmentPointId, false);
		GameObject thing = null;
		int layer = ((!this.isOurPerson || attachmentPointId != AttachmentPointId.Head) ? (-1) : LayerMask.NameToLayer("InvisibleToOurPerson"));
		ThingManager thingManager = Managers.thingManager;
		ThingRequestContext thingRequestContext = ThingRequestContext.PersonClassAttachNewThing;
		Action<GameObject> action = delegate(GameObject returnThing)
		{
			thing = returnThing;
		};
		int num = layer;
		yield return base.StartCoroutine(thingManager.InstantiateThingViaCache(thingRequestContext, thingId, action, false, false, num, null));
		if (thing == null)
		{
			Log.Error("AttachNewThing got null thing from thingManager");
		}
		else
		{
			if (this.isOurPerson)
			{
				Thing component = thing.GetComponent<Thing>();
				if (component != null && component.invisibleToUsWhenAttached)
				{
					component.SetInvisibleToOurPerson(true);
				}
				Component[] componentsInChildren = thing.GetComponentsInChildren<ThingPart>();
				foreach (ThingPart thingPart in componentsInChildren)
				{
					if (thingPart.invisibleToUsWhenAttached)
					{
						thingPart.SetInvisibleToOurPerson(true);
					}
				}
			}
			attachmentPoint.GetComponent<AttachmentPoint>().AttachThing(thing);
			thing.transform.localPosition = position;
			thing.transform.localRotation = rotation;
		}
		yield break;
	}

	// Token: 0x06001506 RID: 5382 RVA: 0x000B8BF0 File Offset: 0x000B6FF0
	public IEnumerator HoldNewThing(TopographyId handTopographyId, string thingId, Vector3 position, Quaternion rotation, EditModes? editMode = null)
	{
		GameObject hand = this.GetHandByTopographyId(handTopographyId);
		GameObject thing = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.PersonClassHoldNewThing, thingId, delegate(GameObject returnThing)
		{
			thing = returnThing;
		}, false, false, -1, null));
		if (thing == null)
		{
			Log.Error("HoldNewThing got null thing from thingManager");
		}
		else
		{
			thing.transform.parent = hand.transform;
			thing.transform.localPosition = position;
			thing.transform.localRotation = rotation;
			Thing component = thing.GetComponent<Thing>();
			if (editMode != null)
			{
				component.SetEditMode(editMode.Value, null);
			}
			component.OnHold(false);
		}
		yield break;
	}

	// Token: 0x06001507 RID: 5383 RVA: 0x000B8C30 File Offset: 0x000B7030
	public void AddNameTagIfNeeded(float secondsToDisplayNameTag = 30f)
	{
		if (this.nameTagWrapper == null)
		{
			this.nameTagWrapper = this.AttachmentPointTorsoLower.transform.Find("NameTagWrapper").gameObject;
			GameObject gameObject = this.nameTagWrapper.transform.Find("NameTag").gameObject;
			this.nameTagTextMesh = gameObject.GetComponent<TextMesh>();
			if (this.mainCamera == null)
			{
				this.mainCamera = GameObject.FindWithTag("MainCamera");
			}
			this.UpdateNameTagContent();
		}
		this.UpdateNameTagColor();
		if (!this.nameTagWrapper.activeSelf)
		{
			this.nameTagWrapper.SetActive(true);
		}
		if (!Managers.areaManager.isTransportInProgress && Managers.areaManager.didFinishLoadingPlacements)
		{
			this.StartNameTagRemovalCountdown(secondsToDisplayNameTag);
		}
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x000B8D04 File Offset: 0x000B7104
	private void UpdateNameTagColor()
	{
		if (this.nameTagTextMesh != null)
		{
			Renderer component = this.nameTagTextMesh.GetComponent<Renderer>();
			component.material.color = ((!(Managers.optimizationManager != null) || !Managers.optimizationManager.findOptimizations) ? Misc.GetGray(0.75f) : Color.white);
		}
	}

	// Token: 0x06001509 RID: 5385 RVA: 0x000B8D6C File Offset: 0x000B716C
	private void HandleNameTagStatsCollectionAndDisplay()
	{
		this.lastBehaviorScriptMessagesPerSecond = this.behaviorScriptMessagesPerSecond;
		this.behaviorScriptMessagesPerSecond = 0;
		if (!this.isOurPerson)
		{
			this.UpdateNameTagContent();
		}
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x000B8D92 File Offset: 0x000B7192
	public void IncreaseBehaviorScriptMessagesPerSecond()
	{
		this.behaviorScriptMessagesPerSecond++;
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x000B8DA2 File Offset: 0x000B71A2
	public void StartNameTagRemovalCountdown(float secondsToDisplayNameTag = 30f)
	{
		base.CancelInvoke("RemoveNameTag");
		base.Invoke("RemoveNameTag", secondsToDisplayNameTag);
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x000B8DBB File Offset: 0x000B71BB
	public void RemoveNameTag()
	{
		base.CancelInvoke("RemoveNameTag");
		base.CancelInvoke("UpdateNameTagContent");
		if (this.nameTagWrapper.activeSelf)
		{
			this.nameTagWrapper.SetActive(false);
		}
	}

	// Token: 0x0600150D RID: 5389 RVA: 0x000B8DF0 File Offset: 0x000B71F0
	public void UpdateNameTagContent()
	{
		if (this.nameTagWrapper != null && this.nameTagWrapper.activeSelf && this.nameTagTextMesh != null)
		{
			string text = this.screenName.Trim();
			bool flag = Managers.optimizationManager != null && Managers.optimizationManager.findOptimizations;
			int num = ((!flag) ? 2 : 1);
			text = Misc.WrapWithNewlines(text, 20, num);
			if (flag)
			{
				int bodyAndHeldThingPartCount = this.GetBodyAndHeldThingPartCount();
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					Environment.NewLine,
					Misc.AddThousandSeparatorComma(bodyAndHeldThingPartCount),
					" ",
					Misc.GetPluralOrSingular("part", (float)bodyAndHeldThingPartCount),
					" | ",
					Misc.AddThousandSeparatorComma(this.lastBehaviorScriptMessagesPerSecond),
					" msgs/s"
				});
				if (this.isMasterClient)
				{
					text = text + Environment.NewLine + "authority";
				}
			}
			text = text.ToUpper();
			this.nameTagTextMesh.text = text;
		}
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x000B8F08 File Offset: 0x000B7308
	public void SetHandsColor(Color color)
	{
		GameObject gameObject = Misc.FindObject(this.AttachmentPointHandLeft, "AttachmentThing");
		GameObject gameObject2 = Misc.FindObject(gameObject, "HandModelLeftThingPart");
		Renderer component = gameObject2.GetComponent<Renderer>();
		component.material.color = color;
		GameObject gameObject3 = Misc.FindObject(this.AttachmentPointHandRight, "AttachmentThing");
		GameObject gameObject4 = Misc.FindObject(gameObject3, "HandModelRightThingPart");
		Renderer component2 = gameObject4.GetComponent<Renderer>();
		component2.material.color = color;
	}

	// Token: 0x0600150F RID: 5391 RVA: 0x000B8F79 File Offset: 0x000B7379
	private void Update()
	{
		if (this.isOurPerson)
		{
			if (this.isMasterClient)
			{
				this.PeriodicallyInformOthersOfStates();
			}
			this.PeriodicallySaveMyBehaviorScriptVariablesToFile();
		}
		this.RotateNeededThingsTowardsUs();
		this.HandleRidingBeaconIfNeeded();
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x000B8FAC File Offset: 0x000B73AC
	private void PeriodicallyInformOthersOfStates()
	{
		if (this.lastHandledInformedOthersOfStatesTime == -1f)
		{
			this.lastHandledInformedOthersOfStatesTime = Time.time;
		}
		if (Time.time >= this.lastHandledInformedOthersOfStatesTime + 15f)
		{
			if (Managers.areaManager.didRPCFinalizeLoadedAllPlacements && Managers.personManager.GetCurrentAreaPersonCount() >= 2)
			{
				Managers.personManager.DoInformOfBehaviorScriptVariablesAndThingStates(true);
			}
			else
			{
				this.lastHandledInformedOthersOfStatesTime = Time.time;
			}
		}
	}

	// Token: 0x06001511 RID: 5393 RVA: 0x000B9024 File Offset: 0x000B7424
	private void PeriodicallySaveMyBehaviorScriptVariablesToFile()
	{
		if (!Managers.areaManager)
		{
			return;
		}
		if (this.lastHandledSaveMyBehaviorScriptVariables == -1f)
		{
			this.lastHandledSaveMyBehaviorScriptVariables = Time.time;
		}
		if (Time.time >= this.lastHandledSaveMyBehaviorScriptVariables + 16f)
		{
			this.lastHandledSaveMyBehaviorScriptVariables = Time.time;
			if (Managers.areaManager.didRPCFinalizeLoadedAllPlacements && !Managers.areaManager.isTransportInProgress)
			{
				this.SaveMyBehaviorScriptVariablesToFile();
			}
		}
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x000B90A4 File Offset: 0x000B74A4
	public void SaveMyBehaviorScriptVariablesToFile()
	{
		if (!string.IsNullOrEmpty(Managers.areaManager.currentAreaId))
		{
			string text = this.GetMyBehaviorScriptVariablesString();
			if (string.IsNullOrEmpty(text))
			{
				string myBehaviorScriptVariablesPath = this.GetMyBehaviorScriptVariablesPath(false);
				if (File.Exists(myBehaviorScriptVariablesPath))
				{
					File.Delete(myBehaviorScriptVariablesPath);
				}
			}
			else
			{
				string myBehaviorScriptVariablesPath2 = this.GetMyBehaviorScriptVariablesPath(true);
				text = Misc.SimpleEncrypt(text);
				File.WriteAllText(myBehaviorScriptVariablesPath2, text);
			}
		}
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x000B910C File Offset: 0x000B750C
	public void LoadMyBehaviorScriptVariablesFromFile()
	{
		if (this.isOurPerson)
		{
			string myBehaviorScriptVariablesPath = this.GetMyBehaviorScriptVariablesPath(false);
			if (File.Exists(myBehaviorScriptVariablesPath))
			{
				string text = File.ReadAllText(myBehaviorScriptVariablesPath);
				text = Misc.SimpleDecrypt(text);
				this.SetMyBehaviorScriptVariablesFromString(text);
			}
		}
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x000B914C File Offset: 0x000B754C
	public string GetMyBehaviorScriptVariablesString()
	{
		string text = string.Empty;
		foreach (KeyValuePair<string, float> keyValuePair in this.behaviorScriptVariables)
		{
			if (text != string.Empty)
			{
				text += "/";
			}
			string text2 = keyValuePair.Key;
			text2 = text2.Replace("person.", string.Empty);
			string text3 = text;
			text = string.Concat(new object[] { text3, text2, ",", keyValuePair.Value });
		}
		return text;
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x000B920C File Offset: 0x000B760C
	public void SetMyBehaviorScriptVariablesFromString(string s)
	{
		this.behaviorScriptVariables = new Dictionary<string, float>();
		if (!string.IsNullOrEmpty(s))
		{
			string[] array = Misc.Split(s, "/", StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				string[] array3 = Misc.Split(text, ",", StringSplitOptions.RemoveEmptyEntries);
				if (array3.Length == 2)
				{
					string text2 = "person." + array3[0];
					float num;
					if (float.TryParse(array3[1], out num))
					{
						this.SetBehaviorScriptVariable(text2, num);
					}
				}
			}
		}
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x000B9298 File Offset: 0x000B7698
	private string GetMyBehaviorScriptVariablesPath(bool createIfNonExisting = false)
	{
		string text = Application.persistentDataPath + "/data/" + this.userId + "/areas";
		if (createIfNonExisting)
		{
			Directory.CreateDirectory(text);
		}
		return text + "/" + Managers.areaManager.currentAreaId + ".dat";
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x000B92E8 File Offset: 0x000B76E8
	private void RotateNeededThingsTowardsUs()
	{
		if (!this.isOurPerson && this.Head != null && this.mainCamera != null)
		{
			if (this.nameTagWrapper.activeSelf)
			{
				this.RotateTowardsUs(this.nameTagWrapper);
			}
			if (this.defaultTextChatReceiver != null)
			{
				this.RotateTowardsUs(this.defaultTextChatReceiver.gameObject);
			}
		}
	}

	// Token: 0x06001518 RID: 5400 RVA: 0x000B9360 File Offset: 0x000B7760
	public void HandleRidingBeaconIfNeeded()
	{
		if (this.ridingBeacon != null)
		{
			this.Rig.transform.position = this.ridingBeacon.position - this.ridingBeaconPositionOffset;
			if (this.isOurPerson && Vector3.Distance(this.ridingBeaconLastDistanceOptimizedPosition, this.ridingBeacon.position) >= 20f)
			{
				this.ridingBeaconLastDistanceOptimizedPosition = this.ridingBeacon.position;
				Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
			}
		}
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x000B93F0 File Offset: 0x000B77F0
	private void HandleAdaptRigToRidingBeaconRotation()
	{
		if (Time.time >= this.timeWhenRotationAdaptsToRidingBeacon)
		{
			Vector3 vector = this.ridingBeacon.eulerAngles - this.ridingBeaconRotationOffset;
			if (Vector3.Distance(this.Rig.transform.eulerAngles, vector) >= 45f)
			{
				Transform transform = this.Head.transform;
				Vector3 position = transform.position;
				this.timeWhenRotationAdaptsToRidingBeacon = Time.time + 0.25f;
				this.Rig.transform.eulerAngles = vector;
				Vector3 position2 = this.Rig.transform.position;
				position2.x += position.x - transform.position.x;
				position2.y += position.y - transform.position.y;
				position2.z += position.z - transform.position.z;
				this.Rig.transform.position = position2;
				Managers.personManager.DoUpdateRidingBeaconOffset();
			}
		}
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x000B9514 File Offset: 0x000B7914
	public void AddRidingBeacon(GameObject targetObject, Vector3 beaconPosition, bool beaconPositionIsLocal)
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		gameObject.tag = "RidingBeacon";
		gameObject.transform.localScale = Misc.GetUniformVector3(0.5f);
		gameObject.transform.parent = targetObject.transform;
		gameObject.transform.eulerAngles = Vector3.zero;
		if (beaconPositionIsLocal)
		{
			gameObject.transform.localPosition = beaconPosition;
		}
		else
		{
			gameObject.transform.position = beaconPosition;
		}
		this.ridingBeaconLastDistanceOptimizedPosition = beaconPosition;
		Renderer component = gameObject.GetComponent<Renderer>();
		component.enabled = false;
		if (this.ridingBeacon != null)
		{
			Misc.Destroy(this.ridingBeacon.gameObject);
		}
		this.ridingBeacon = gameObject.transform;
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x000B95CE File Offset: 0x000B79CE
	public void RemoveRidingBeacon()
	{
		if (this.ridingBeacon != null)
		{
			Misc.Destroy(this.ridingBeacon.gameObject);
			this.ridingBeacon = null;
			this.ridingBeaconCache = new RidingBeaconCache();
		}
	}

	// Token: 0x0600151C RID: 5404 RVA: 0x000B9604 File Offset: 0x000B7A04
	private void RotateTowardsUs(GameObject wrapper)
	{
		Vector3 vector = -(this.mainCamera.transform.position - this.Head.transform.position);
		if (vector != Vector3.zero)
		{
			wrapper.transform.rotation = Quaternion.LookRotation(vector);
		}
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x000B965D File Offset: 0x000B7A5D
	public GameObject GetHandBySide(Side side)
	{
		return this.GetHandByTopographyId((side != Side.Left) ? TopographyId.Right : TopographyId.Left);
	}

	// Token: 0x0600151E RID: 5406 RVA: 0x000B9678 File Offset: 0x000B7A78
	public GameObject GetHandByTopographyId(TopographyId topographyId)
	{
		GameObject gameObject;
		if (topographyId == TopographyId.Left)
		{
			gameObject = this.LeftHand;
		}
		else
		{
			if (topographyId != TopographyId.Right)
			{
				throw new Exception("GetHandByTopographyId - bad topographyId: " + topographyId.ToString());
			}
			gameObject = this.RightHand;
		}
		return gameObject;
	}

	// Token: 0x0600151F RID: 5407 RVA: 0x000B96CD File Offset: 0x000B7ACD
	[PunRPC]
	public void DoAttachThing_Remote(AttachmentPointId attachmentPointId, string thingId, Vector3 position, Quaternion rotation)
	{
		base.StartCoroutine(this.AttachNewThing(attachmentPointId, thingId, position, rotation));
	}

	// Token: 0x06001520 RID: 5408 RVA: 0x000B96E4 File Offset: 0x000B7AE4
	[PunRPC]
	public void DoRemoveAttachedThing_LocalOrRemote(AttachmentPointId attachmentPointId)
	{
		GameObject attachmentPointById = this.GetAttachmentPointById(attachmentPointId, false);
		attachmentPointById.GetComponent<AttachmentPoint>().DetachAndDestroyAttachedThing();
	}

	// Token: 0x06001521 RID: 5409 RVA: 0x000B9705 File Offset: 0x000B7B05
	[PunRPC]
	public void DoHoldThing_Remote(TopographyId topographyId, string thingId, Vector3 position, Quaternion rotation, EditModes editMode)
	{
		this.ClearWhatHandHoldsIfNeeded(topographyId);
		base.StartCoroutine(this.HoldNewThing(topographyId, thingId, position, rotation, new EditModes?(editMode)));
	}

	// Token: 0x06001522 RID: 5410 RVA: 0x000B9728 File Offset: 0x000B7B28
	[PunRPC]
	public IEnumerator DoThrowThing_Remote(string thrownId, TopographyId topographyId, string thingId, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
	{
		GameObject hand = this.GetHandByTopographyId(topographyId);
		GameObject thing = this.GetThingInHand(hand, false);
		if (thing != null)
		{
			Thing component = thing.GetComponent<Thing>();
			if (!(component != null) && !(component.thingId == thingId))
			{
				thing = null;
			}
		}
		else
		{
			Log.Info("GetThingInHand could not get thing of topographyId " + topographyId + ", so instatiating now", false);
		}
		if (thing == null)
		{
			yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoThrowThing_Remote, thingId, delegate(GameObject returnThing)
			{
				thing = returnThing;
			}, false, false, -1, null));
		}
		if (thing != null)
		{
			thing.transform.position = position;
			thing.transform.rotation = rotation;
			thing.GetComponent<Thing>().ThrowMe(velocity, angularVelocity, thrownId);
		}
		else
		{
			Log.Error("DoThrowThing_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x06001523 RID: 5411 RVA: 0x000B9778 File Offset: 0x000B7B78
	[PunRPC]
	public IEnumerator DoEditHold_Remote(string placementId, TopographyId topographyId, string thingId, Vector3 position, Quaternion rotation)
	{
		this.ClearWhatHandHoldsIfNeeded(topographyId);
		GameObject thing = Managers.thingManager.GetPlacementById(placementId, false);
		if (thing == null)
		{
			Log.Info("DoEditHold_Remote didn't find placement of id " + placementId + ", so instantiating now", false);
			yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoEditHold_Remote, thingId, delegate(GameObject returnThing)
			{
				thing = returnThing;
			}, false, false, -1, null));
			thing.GetComponent<Thing>().placementId = placementId;
		}
		if (thing != null)
		{
			GameObject handByTopographyId = this.GetHandByTopographyId(topographyId);
			thing.transform.parent = handByTopographyId.transform;
			thing.transform.localPosition = position;
			thing.transform.localRotation = rotation;
			thing.GetComponent<Thing>().OnEditHold(false);
		}
		else
		{
			Log.Error("DoEditHold_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x06001524 RID: 5412 RVA: 0x000B97B8 File Offset: 0x000B7BB8
	[PunRPC]
	public IEnumerator DoPlaceThingFromHand_Remote(string placementId, TopographyId topographyId, string thingId, Vector3 position, Quaternion rotation, float scale, bool isPlacementUpdate)
	{
		if (!this.ConfirmIsEditorHere())
		{
			yield break;
		}
		GameObject hand = this.GetHandByTopographyId(topographyId);
		GameObject thing = this.GetThingInHand(hand, false);
		if (!isPlacementUpdate)
		{
			Misc.Destroy(thing);
			thing = null;
		}
		if (thing == null)
		{
			yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoPlaceThingFromHand_Remote, thingId, delegate(GameObject returnThing)
			{
				thing = returnThing;
			}, false, true, -1, null));
		}
		if (thing != null)
		{
			thing.transform.parent = Managers.thingManager.placements.transform;
			thing.transform.position = position;
			thing.transform.rotation = rotation;
			thing.transform.localScale = Misc.GetUniformVector3(scale);
			Thing component = thing.GetComponent<Thing>();
			component.placementId = placementId;
			component.OnPlacedFromHand(false, false);
		}
		else
		{
			Log.Error("DoPlaceThingFromHand_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x06001525 RID: 5413 RVA: 0x000B9808 File Offset: 0x000B7C08
	[PunRPC]
	public IEnumerator DoPlaceJustCreatedThing_Remote(string placementId, string thingId, Vector3 position, Quaternion rotation)
	{
		if (!this.ConfirmIsEditorHere())
		{
			yield break;
		}
		GameObject thing = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoPlaceJustCreatedThing_Remote, thingId, delegate(GameObject returnThing)
		{
			thing = returnThing;
		}, false, true, -1, null));
		if (thing != null)
		{
			GameObject placementById = Managers.thingManager.GetPlacementById(placementId, false);
			if (placementById != null)
			{
				Thing component = placementById.GetComponent<Thing>();
				if (component != null)
				{
					component.DeleteMe(true);
				}
			}
			thing.GetComponent<Thing>().placementId = placementId;
			thing.transform.parent = Managers.thingManager.placements.transform;
			thing.transform.position = position;
			thing.transform.rotation = rotation;
			thing.GetComponent<Thing>().OnPlacedJustCreated(false);
		}
		else
		{
			Log.Error("DoPlaceJustCreatedThing_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x06001526 RID: 5414 RVA: 0x000B9840 File Offset: 0x000B7C40
	[PunRPC]
	public IEnumerator DoPlaceRecreatedPlacedSubThing_Remote(string placementId, string thingId, Vector3 position, Quaternion rotation)
	{
		GameObject thing = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoPlaceRecreatedPlacedSubThing_Remote, thingId, delegate(GameObject returnThing)
		{
			thing = returnThing;
		}, false, true, -1, null));
		if (thing != null)
		{
			thing.GetComponent<Thing>().placementId = placementId;
			thing.transform.parent = Managers.thingManager.placements.transform;
			thing.transform.position = position;
			thing.transform.rotation = rotation;
			thing.GetComponent<Thing>().OnPlacedRecreatedPlacedSubThing(false);
		}
		else
		{
			Log.Error("DoPlaceRecreatedPlacedSubThing_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x06001527 RID: 5415 RVA: 0x000B9878 File Offset: 0x000B7C78
	[PunRPC]
	public IEnumerator DoHoldFromHand_Remote(TopographyId oldTopographyId, string thingId, Vector3 position, Quaternion rotation)
	{
		TopographyId newTopographyId = this.GetMirroredTopographyId(oldTopographyId);
		this.ClearWhatHandHoldsIfNeeded(newTopographyId);
		GameObject newHand = this.GetHandByTopographyId(newTopographyId);
		GameObject oldHand = this.GetHandByTopographyId(oldTopographyId);
		GameObject thing = this.GetThingInHand(oldHand, false);
		if (thing == null)
		{
			Log.Info("DoHoldFromHand_Remote didn't find held thing of id " + thingId + ", so instantiating now", false);
			yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoHoldFromHand_Remote, thingId, delegate(GameObject returnThing)
			{
				thing = returnThing;
			}, false, false, -1, null));
		}
		if (thing != null)
		{
			thing.transform.parent = newHand.transform;
			thing.transform.localPosition = position;
			thing.transform.localRotation = rotation;
			thing.GetComponent<Thing>().OnHoldFromHand(false);
		}
		else
		{
			Log.Error("DoHoldFromHand_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x06001528 RID: 5416 RVA: 0x000B98B0 File Offset: 0x000B7CB0
	[PunRPC]
	public IEnumerator DoHoldThrownThing_Remote(string thrownId, TopographyId topographyId, string thingId, Vector3 position, Quaternion rotation)
	{
		this.ClearWhatHandHoldsIfNeeded(topographyId);
		GameObject hand = this.GetHandByTopographyId(topographyId);
		GameObject thrownThing = Managers.thingManager.GetThingByThrownId(thrownId);
		if (thrownThing != null)
		{
			global::UnityEngine.Object.Destroy(thrownThing);
		}
		else
		{
			Log.Info("DoHoldThrownThing_Remote didn't find thrown id " + thrownId + " to destroy", false);
		}
		GameObject thing = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoHoldThrownThing_Remote, thingId, delegate(GameObject returnThing)
		{
			thing = returnThing;
		}, false, false, -1, null));
		if (thing != null)
		{
			thing.transform.parent = hand.transform;
			thing.transform.localPosition = position;
			thing.transform.localRotation = rotation;
			thing.GetComponent<Thing>().OnHoldFromThrownThing(false);
		}
		else
		{
			Log.Error("DoHoldThrownThing_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x06001529 RID: 5417 RVA: 0x000B98F0 File Offset: 0x000B7CF0
	[PunRPC]
	public void DoDeletePlacement_Remote(string placementId)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		GameObject placementById = Managers.thingManager.GetPlacementById(placementId, false);
		if (placementById != null)
		{
			placementById.GetComponent<Thing>().DeleteMe(false);
		}
		else
		{
			Log.Error("DoDeletePlacement_Remote thing for placementId " + placementId + " was null");
		}
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x000B9948 File Offset: 0x000B7D48
	[PunRPC]
	public void DoBehaviorScriptLine_Remote(ThingSpecifierType specifierType, string specifierId, int thingPartIndex, int scriptLine, int stateI, int currentState, Vector3 currentWorldPosition, Quaternion currentWorldRotation)
	{
		this.behaviorScriptMessagesPerSecond++;
		GameObject thingBySpecifier = this.GetThingBySpecifier(specifierType, specifierId);
		if (thingBySpecifier != null)
		{
			GameObject thingPartByIndex = this.GetThingPartByIndex(thingBySpecifier, thingPartIndex);
			if (thingPartByIndex != null)
			{
				ThingPart component = thingPartByIndex.GetComponent<ThingPart>();
				if (component != null && stateI < component.states.Count && currentState < component.states.Count)
				{
					component.currentState = currentState;
					if (scriptLine < component.states[stateI].listeners.Count)
					{
						Vector3 position = thingBySpecifier.transform.position;
						Quaternion rotation = thingBySpecifier.transform.rotation;
						thingBySpecifier.transform.position = currentWorldPosition;
						thingBySpecifier.transform.rotation = currentWorldRotation;
						component.ExecuteCommands(component.states[stateI].listeners[scriptLine], true);
						thingBySpecifier.transform.position = position;
						thingBySpecifier.transform.rotation = rotation;
						if (Managers.optimizationManager != null)
						{
							Managers.optimizationManager.IndicateScriptActivityHere(component.transform.position);
						}
					}
					else
					{
						Log.Debug("DoBehaviorScriptLine_Remote script line " + scriptLine + " not found");
					}
				}
				else
				{
					Log.Debug("DoBehaviorScriptLine_Remote thingPart, stateI or currentState not found");
				}
			}
			else
			{
				Log.Debug(string.Concat(new object[]
				{
					"DoBehaviorScriptLine_Remote(1) thingPart not found for type ",
					specifierType.ToString(),
					", id ",
					specifierId,
					", index ",
					thingPartIndex
				}));
			}
		}
		else
		{
			Log.Debug(string.Concat(new object[]
			{
				"DoBehaviorScriptLine_Remote(2) thingPart not found for type ",
				specifierType.ToString(),
				", id ",
				specifierId,
				", index ",
				thingPartIndex
			}));
		}
	}

	// Token: 0x0600152B RID: 5419 RVA: 0x000B9B3E File Offset: 0x000B7F3E
	[PunRPC]
	public void DoSetEnvironmentChanger_Remote(string changerName, Vector3 rotation, float scale, float red, float green, float blue)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		Managers.areaManager.ApplyEnvironmentChanger(changerName, rotation, scale, new Color(red, green, blue));
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x000B9B64 File Offset: 0x000B7F64
	[PunRPC]
	public void DoSetEnvironmentType_Remote(EnvironmentType environmentType)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		Managers.areaManager.ApplyEnvironmentType(environmentType);
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x000B9B80 File Offset: 0x000B7F80
	[PunRPC]
	public void DoSetAreaPrivate_Remote(bool isPrivate)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		Managers.areaManager.SetIsPrivate_Local(isPrivate);
		if (isPrivate && !Managers.areaManager.weAreEditorOfCurrentArea && !Managers.areaManager.currentAreaIsHomeArea)
		{
			Managers.areaManager.ForceLoadHomeArea();
		}
	}

	// Token: 0x0600152E RID: 5422 RVA: 0x000B9BD2 File Offset: 0x000B7FD2
	[PunRPC]
	public void DoSetAreaZeroGravity_Remote(bool isZeroGravity)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		Managers.areaManager.SetIsZeroGravity_Local(isZeroGravity);
	}

	// Token: 0x0600152F RID: 5423 RVA: 0x000B9BEB File Offset: 0x000B7FEB
	[PunRPC]
	public void DoSetAreaFloatingDust_Remote(bool hasFloatingDust)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		Managers.areaManager.SetHasFloatingDust_Local(hasFloatingDust);
	}

	// Token: 0x06001530 RID: 5424 RVA: 0x000B9C04 File Offset: 0x000B8004
	[PunRPC]
	public void DoSetAreaOnlyOwnerSetsLocks_Remote(bool onlyOwnerSetsLocks)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		Managers.areaManager.SetOnlyOwnerSetsLocks_Local(onlyOwnerSetsLocks);
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x000B9C1D File Offset: 0x000B801D
	[PunRPC]
	public void DoSetHandsColor_Remote(float red, float green, float blue)
	{
		this.SetHandsColor(new Color(red, green, blue));
	}

	// Token: 0x06001532 RID: 5426 RVA: 0x000B9C30 File Offset: 0x000B8030
	[PunRPC]
	public IEnumerator DoAddToHand_Remote(TopographyId topographyId, string thingId, Vector3 position, Quaternion rotation, EditModes editMode, EditModes previousEditMode)
	{
		this.ClearWhatHandHoldsIfNeeded(topographyId);
		GameObject thing = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoAddToHand_Remote, thingId, delegate(GameObject returnThing)
		{
			thing = returnThing;
		}, false, false, -1, null));
		if (thing != null)
		{
			GameObject handByTopographyId = this.GetHandByTopographyId(topographyId);
			thing.transform.parent = handByTopographyId.transform;
			thing.transform.localPosition = position;
			thing.transform.localRotation = rotation;
			Thing component = thing.GetComponent<Thing>();
			component.SetEditMode(editMode, new EditModes?(previousEditMode));
			component.OnAddToHand(false);
		}
		else
		{
			Log.Error("DoAddToHand_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x000B9C78 File Offset: 0x000B8078
	[PunRPC]
	public IEnumerator DoCloneToHand_Remote(TopographyId topographyId, string thingId, Vector3 position, Quaternion rotation, Vector3 scale)
	{
		this.ClearWhatHandHoldsIfNeeded(topographyId);
		GameObject thing = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoCloneToHand_Remote, thingId, delegate(GameObject returnThing)
		{
			thing = returnThing;
		}, false, false, -1, null));
		if (thing != null)
		{
			GameObject handByTopographyId = this.GetHandByTopographyId(topographyId);
			thing.transform.parent = handByTopographyId.transform;
			thing.transform.localPosition = position;
			thing.transform.localRotation = rotation;
			thing.transform.localScale = scale;
			thing.GetComponent<Thing>().OnCloneToHand(false);
		}
		else
		{
			Log.Error("DoCloneToHand_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x000B9CB8 File Offset: 0x000B80B8
	[PunRPC]
	public void DoClearFromHand_Remote(TopographyId topographyId)
	{
		this.ClearWhatHandHoldsIfNeeded(topographyId);
	}

	// Token: 0x06001535 RID: 5429 RVA: 0x000B9CC4 File Offset: 0x000B80C4
	[PunRPC]
	public IEnumerator DoMovePlacement_Remote(string placementId, string thingId, Vector3 localPosition, Vector3 localRotation)
	{
		if (!this.ConfirmIsEditorHere())
		{
			yield break;
		}
		GameObject thing = Managers.thingManager.GetPlacementById(placementId, false);
		if (thing == null)
		{
			Log.Info("DoMovePlacement_Remote didn't find placement of id " + placementId + ", so instantiating now", false);
			yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoMovePlacement_Remote, thingId, delegate(GameObject returnThing)
			{
				thing = returnThing;
			}, false, false, -1, null));
			thing.GetComponent<Thing>().placementId = placementId;
		}
		if (thing != null)
		{
			Thing component = thing.GetComponent<Thing>();
			component.MemorizePositionAndRotationForUndo();
			thing.transform.parent = Managers.thingManager.placements.transform;
			thing.transform.localPosition = localPosition;
			thing.transform.localEulerAngles = localRotation;
			component.placementId = placementId;
			component.OnMovePlacement(false);
		}
		else
		{
			Log.Error("DoMovePlacement_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x06001536 RID: 5430 RVA: 0x000B9CFC File Offset: 0x000B80FC
	[PunRPC]
	public void DoPlayVideo_Remote(ThingSpecifierType specifierType, string specifierId, string videoId, string videoTitle, string videoStartedByPersonId, string videoStartedByPersonName)
	{
		GameObject thingBySpecifier = this.GetThingBySpecifier(specifierType, specifierId);
		if (thingBySpecifier != null)
		{
			Managers.videoManager.PlayVideoAtThing(thingBySpecifier, videoId, videoTitle, videoStartedByPersonId, videoStartedByPersonName, 0, false);
		}
		else
		{
			Log.Warning("DoPlayVideo_Remote thing by specifier not found, can happen normally as thing may be off distance");
		}
	}

	// Token: 0x06001537 RID: 5431 RVA: 0x000B9D41 File Offset: 0x000B8141
	[PunRPC]
	public void DoSetVideoVolume_Remote(float volume)
	{
		Managers.videoManager.SetVolume(volume);
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x000B9D4E File Offset: 0x000B814E
	[PunRPC]
	public void DoStopVideo_Remote()
	{
		Managers.videoManager.StopVideo(false);
	}

	// Token: 0x06001539 RID: 5433 RVA: 0x000B9D5C File Offset: 0x000B815C
	[PunRPC]
	public IEnumerator DoPlaceThing_Remote(string placementId, string thingId, Vector3 position, Quaternion rotation)
	{
		if (!this.ConfirmIsEditorHere())
		{
			yield break;
		}
		GameObject thing = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoPlaceThing_Remote, thingId, delegate(GameObject returnThing)
		{
			thing = returnThing;
		}, false, true, -1, null));
		if (thing != null)
		{
			thing.transform.parent = Managers.thingManager.placements.transform;
			thing.transform.position = position;
			thing.transform.rotation = rotation;
			Thing component = thing.GetComponent<Thing>();
			component.placementId = placementId;
			component.OnPlaced(false);
		}
		else
		{
			Log.Error("DoPlaceThing_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x0600153A RID: 5434 RVA: 0x000B9D94 File Offset: 0x000B8194
	[PunRPC]
	public void DoFinalizeLoadedAllPlacements_Remote(string personId, string personBehaviorScriptVariablesData, bool isTransit)
	{
		if (Our.IsMasterClient(false))
		{
			Person personById = Managers.personManager.GetPersonById(personId);
			bool flag = personById == null;
			if (flag)
			{
				Managers.personManager.receivedUnassignedPersonBehaviorScriptVariables[personId] = personBehaviorScriptVariablesData;
			}
			else if (!personById.isOurPerson)
			{
				if (Managers.personManager.receivedUnassignedPersonBehaviorScriptVariables.ContainsKey(personId))
				{
					Managers.personManager.receivedUnassignedPersonBehaviorScriptVariables.Remove(personId);
				}
				personById.SetMyBehaviorScriptVariablesFromString(personBehaviorScriptVariablesData);
			}
			Managers.personManager.DoInformOfBehaviorScriptVariablesAndThingStates(false);
		}
		else
		{
			Log.Debug("DoFinalizeLoadedAllPlacements_Remote reached non-master, this shouldn't happen");
		}
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x000B9E30 File Offset: 0x000B8230
	[PunRPC]
	public void DoSetPlacementAttribute_Remote(string placementId, PlacementAttribute attribute, bool state)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		GameObject placementById = Managers.thingManager.GetPlacementById(placementId, false);
		if (placementById != null)
		{
			Thing component = placementById.GetComponent<Thing>();
			if (component != null)
			{
				component.SetPlacementAttribute(attribute, state);
				if (attribute != PlacementAttribute.Locked)
				{
					if (attribute != PlacementAttribute.InvisibleToEditors)
					{
						Managers.thingManager.ReCreatePlacementAfterPlacementAttributeChange(component);
					}
					else if (Managers.areaManager.weAreEditorOfCurrentArea)
					{
						if (component.isInvisibleToEditors && !Our.seeInvisibleAsEditor)
						{
							Misc.SetAllObjectLayers(component.gameObject, "InvisibleToOurPerson");
						}
						else
						{
							Misc.SetAllObjectLayers(component.gameObject, "Default");
						}
					}
				}
			}
		}
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		if (currentNonStartDialog != null && currentNonStartDialog.GetComponent<Dialog>().dialogType == DialogType.Thing)
		{
			ThingDialog component2 = currentNonStartDialog.GetComponent<ThingDialog>();
			if (component2 != null)
			{
				component2.UpdateLockButton();
			}
		}
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x000B9F34 File Offset: 0x000B8334
	[PunRPC]
	public void DoSetAreaCopyable_Remote(bool isCopyable)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		Managers.areaManager.SetIsCopyable_Local(isCopyable);
		GameObject currentNonStartDialog = Our.GetCurrentNonStartDialog();
		bool flag = false;
		if (currentNonStartDialog != null)
		{
			DialogType dialogType = currentNonStartDialog.GetComponent<Dialog>().dialogType;
			flag = dialogType == DialogType.Area || dialogType == DialogType.AreaAttributes;
		}
		if (flag)
		{
			global::UnityEngine.Object.Destroy(currentNonStartDialog);
		}
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x000B9F94 File Offset: 0x000B8394
	[PunRPC]
	public IEnumerator DoReplaceAllOccurrencesOfThingInArea_Remote(string oldThingId, string newThingId)
	{
		if (!this.ConfirmIsEditorHere())
		{
			yield break;
		}
		GameObject thing = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoReplaceAllOccurrencesOfThingInArea_Remote, newThingId, delegate(GameObject returnThing)
		{
			thing = returnThing;
		}, false, false, -1, null));
		if (thing != null)
		{
			Thing component = thing.GetComponent<Thing>();
			Managers.areaManager.ReplaceAllOccurrencesOfThingInArea(oldThingId, component, false);
		}
		Misc.Destroy(thing);
		Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
		yield break;
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x000B9FC0 File Offset: 0x000B83C0
	[PunRPC]
	public void DoAddRidingBeacon_Remote(ThingSpecifierType specifierType, string specifierId, Vector3 beaconPosition, Vector3 _ridingBeaconPositionOffset, Vector3 _ridingBeaconRotationOffset)
	{
		GameObject thingBySpecifier = this.GetThingBySpecifier(specifierType, specifierId);
		if (thingBySpecifier != null)
		{
			this.AddRidingBeacon(thingBySpecifier, beaconPosition, true);
			this.ridingBeaconPositionOffset = _ridingBeaconPositionOffset;
			this.ridingBeaconRotationOffset = _ridingBeaconRotationOffset;
		}
		else
		{
			this.RemoveRidingBeacon();
		}
	}

	// Token: 0x0600153F RID: 5439 RVA: 0x000BA006 File Offset: 0x000B8406
	[PunRPC]
	public void DoRemoveRidingBeacon_Remote()
	{
		this.RemoveRidingBeacon();
	}

	// Token: 0x06001540 RID: 5440 RVA: 0x000BA00E File Offset: 0x000B840E
	[PunRPC]
	public void DoUpdateRidingBeaconOffset_Remote(Vector3 rigRotation, Vector3 _ridingBeaconPositionOffset, Vector3 _ridingBeaconRotationOffset)
	{
		this.Rig.transform.eulerAngles = rigRotation;
		this.ridingBeaconPositionOffset = _ridingBeaconPositionOffset;
		this.ridingBeaconRotationOffset = _ridingBeaconRotationOffset;
	}

	// Token: 0x06001541 RID: 5441 RVA: 0x000BA030 File Offset: 0x000B8430
	[PunRPC]
	public void DoChangeHeldThingPositionRotation_Remote(ThingSpecifierType specifierType, string specifierId, Vector3 localPosition, Quaternion localRotation)
	{
		GameObject thingBySpecifier = this.GetThingBySpecifier(specifierType, specifierId);
		if (thingBySpecifier != null)
		{
			thingBySpecifier.transform.localPosition = localPosition;
			thingBySpecifier.transform.localRotation = localRotation;
			Thing component = thingBySpecifier.GetComponent<Thing>();
			if (component != null)
			{
				component.MemorizeOriginalTransform(false);
			}
		}
	}

	// Token: 0x06001542 RID: 5442 RVA: 0x000BA085 File Offset: 0x000B8485
	[PunRPC]
	public void DoAddTypedText_Remote(string text)
	{
		this.AddTypedText(text);
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x000BA090 File Offset: 0x000B8490
	[PunRPC]
	public void DoUpdatePlacementScale_Remote(string placementId, float scale)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		GameObject placementById = Managers.thingManager.GetPlacementById(placementId, false);
		if (placementById != null)
		{
			if (placementById != CreationHelper.thingBeingEdited)
			{
				TransformTargetFollower transformTargetFollower = placementById.GetComponent<TransformTargetFollower>();
				if (transformTargetFollower == null)
				{
					transformTargetFollower = placementById.AddComponent<TransformTargetFollower>();
				}
				transformTargetFollower.targetScale = new Vector3?(Misc.GetUniformVector3(scale));
			}
		}
		else
		{
			Log.Info("DoUpdatePlacementScale_Remote id " + placementId + " not found", false);
		}
	}

	// Token: 0x06001544 RID: 5444 RVA: 0x000BA118 File Offset: 0x000B8518
	[PunRPC]
	public void DoUpdatePlacementShowAt_Remote(string placementId, float? distanceToShow)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		GameObject placementById = Managers.thingManager.GetPlacementById(placementId, false);
		if (placementById != null)
		{
			Thing component = placementById.GetComponent<Thing>();
			if (component != null)
			{
				component.distanceToShow = distanceToShow;
				Managers.optimizationManager.SetPlacementsActiveBasedOnDistance(string.Empty);
			}
		}
		else
		{
			Log.Info("DoUpdatePlacementShowAt_Remote id " + placementId + " not found", false);
		}
	}

	// Token: 0x06001545 RID: 5445 RVA: 0x000BA190 File Offset: 0x000B8590
	[PunRPC]
	public void DoUpdateViaAreaSettingsJson_Remote(string json)
	{
		try
		{
			global::SimpleJSON.JSONNode jsonnode = JSON.Parse(json);
			if (jsonnode != null && Managers.filterManager != null)
			{
				Managers.filterManager.SetByJson(jsonnode);
				Managers.filterManager.ApplySettings(false);
			}
		}
		catch (Exception ex)
		{
			Log.Info("Invalid Json in DoUpdateViaAreaSettingsJson_Remote: " + json, false);
		}
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x000BA204 File Offset: 0x000B8604
	[PunRPC]
	public IEnumerator DoAddJustCreatedTemporaryThing_Remote(string thingId, Vector3 position, Quaternion rotation, string thrownId)
	{
		GameObject thingObject = null;
		yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoAddJustCreatedTemporaryThing_Remote, thingId, delegate(GameObject returnThing)
		{
			thingObject = returnThing;
		}, false, false, -1, null));
		if (thingObject != null)
		{
			Thing component = thingObject.GetComponent<Thing>();
			component.transform.position = position;
			component.transform.rotation = rotation;
			component.thrownId = thrownId;
			component.OnAddJustCreatedTemporary(false);
		}
		else
		{
			Log.Error("DoAddJustCreatedTemporaryThing_Remote thing was null");
		}
		yield break;
	}

	// Token: 0x06001547 RID: 5447 RVA: 0x000BA23C File Offset: 0x000B863C
	[PunRPC]
	public void DoInformOfAreaBehaviorScriptVariableChange_Remote(string variableName, float variableValue)
	{
		this.behaviorScriptMessagesPerSecond++;
		if (Managers.areaManager != null)
		{
			variableName = "area." + variableName;
			Managers.areaManager.SetBehaviorScriptVariable(variableName, variableValue);
		}
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x000BA278 File Offset: 0x000B8678
	[PunRPC]
	public void DoInformOfThingBehaviorScriptVariableChange_Remote(ThingSpecifierType specifierType, string specifierId, string variableName, float variableValue)
	{
		this.behaviorScriptMessagesPerSecond++;
		Thing thingScriptBySpecifier = this.GetThingScriptBySpecifier(specifierType, specifierId);
		if (thingScriptBySpecifier != null)
		{
			thingScriptBySpecifier.SetBehaviorScriptVariable(variableName, variableValue);
			if (Managers.optimizationManager != null)
			{
				Managers.optimizationManager.IndicateScriptActivityHere(thingScriptBySpecifier.transform.position);
			}
		}
		else
		{
			Log.Debug(string.Concat(new string[]
			{
				"DoInformOfThingBehaviorScriptVariableChange_Remote didn't find Thing of specifierType \"",
				specifierType.ToString(),
				"\" + id \"",
				specifierId,
				"\""
			}));
		}
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x000BA318 File Offset: 0x000B8718
	[PunRPC]
	public void DoInformOfPersonBehaviorScriptVariableChange_Remote(string personId, string variableName, float variableValue)
	{
		this.behaviorScriptMessagesPerSecond++;
		Person personById = Managers.personManager.GetPersonById(personId);
		if (personById != null)
		{
			variableName = "person." + variableName;
			personById.SetBehaviorScriptVariable(variableName, variableValue);
		}
		else
		{
			Log.Debug("DoInformOfPersonBehaviorScriptVariableChange_Remote didn't find Person, so ignoring.");
		}
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x000BA370 File Offset: 0x000B8770
	[PunRPC]
	public IEnumerator DoInformOfBehaviorScriptVariablesAndThingStates_Remote(float originClientTime, string thingPartStatesString, string thingPhysicsString, string areaBehaviorScriptVariablesString, string thingBehaviorScriptVariablesString, string personsBehaviorScriptVariablesString, string slideshowUrlsString, string thingAttributesString, string temporarilyDestroyedThingsStrings, string movableByEveryoneThingsString)
	{
		this.behaviorScriptMessagesPerSecond++;
		float clientTime = Time.time;
		try
		{
			string[] array = Misc.Split(thingPartStatesString, "\n", StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				string[] array3 = Misc.Split(text, "|", StringSplitOptions.None);
				if (array3.Length == 16)
				{
					int num = 0;
					ThingSpecifierType syncingUncompressedSpecifierType = PersonManager.GetSyncingUncompressedSpecifierType(array3[num++]);
					string text2 = array3[num++];
					int num2 = int.Parse(array3[num++]);
					int num3 = int.Parse(array3[num++]);
					int syncingUncompressedInt = PersonManager.GetSyncingUncompressedInt(array3[num++]);
					int syncingUncompressedInt2 = PersonManager.GetSyncingUncompressedInt(array3[num++]);
					float num4 = PersonManager.GetSyncingUncompressedFloat(array3[num++]);
					float syncingUncompressedFloat = PersonManager.GetSyncingUncompressedFloat(array3[num++]);
					float num5 = PersonManager.GetSyncingUncompressedFloat(array3[num++]);
					string text3 = array3[num++];
					string text4 = PersonManager.UnencodeSyncingString(array3[num++]);
					TweenType tweenType = (TweenType)int.Parse(array3[num++]);
					bool syncingUncompressedBool = PersonManager.GetSyncingUncompressedBool(array3[num++]);
					bool syncingUncompressedBool2 = PersonManager.GetSyncingUncompressedBool(array3[num++]);
					string syncingUncompressedString = PersonManager.GetSyncingUncompressedString(array3[num++]);
					float num6 = float.Parse(array3[num++]);
					if (num4 != -1f)
					{
						num4 = Misc.ClampMin(num4 - originClientTime + clientTime, 0f);
					}
					if (num5 != -1f)
					{
						num5 = Misc.ClampMin(num5 - originClientTime + clientTime, 0f);
					}
					num6 = Mathf.Clamp(num6, -1000f, 1000f);
					ThingPart thingPartBySpecifier = this.GetThingPartBySpecifier(syncingUncompressedSpecifierType, text2, num2);
					if (thingPartBySpecifier != null)
					{
						thingPartBySpecifier.SetViaSyncingToAreaNewcomersDataString(num3, syncingUncompressedInt, syncingUncompressedInt2, num4, syncingUncompressedFloat, num5, text3, text4, tweenType, syncingUncompressedBool, syncingUncompressedBool2, syncingUncompressedString, num6);
					}
				}
			}
			Managers.areaManager.behaviorScriptVariables = new Dictionary<string, float>();
			string[] array4 = Misc.Split(areaBehaviorScriptVariablesString, "\n", StringSplitOptions.RemoveEmptyEntries);
			foreach (string text5 in array4)
			{
				string[] array6 = Misc.Split(text5, "|", StringSplitOptions.None);
				if (array6.Length == 2)
				{
					string text6 = "area." + array6[0];
					Managers.areaManager.behaviorScriptVariables[text6] = float.Parse(array6[1]);
				}
			}
			Managers.behaviorScriptManager.ResetAllThingBehaviorScriptVariables();
			string[] array7 = Misc.Split(thingBehaviorScriptVariablesString, "\n", StringSplitOptions.RemoveEmptyEntries);
			foreach (string text7 in array7)
			{
				string[] array9 = Misc.Split(text7, "|", StringSplitOptions.None);
				if (array9.Length == 4)
				{
					ThingSpecifierType syncingUncompressedSpecifierType2 = PersonManager.GetSyncingUncompressedSpecifierType(array9[0]);
					string text8 = array9[1];
					Thing thingScriptBySpecifier = this.GetThingScriptBySpecifier(syncingUncompressedSpecifierType2, text8);
					if (thingScriptBySpecifier != null)
					{
						thingScriptBySpecifier.behaviorScriptVariables[array9[2]] = float.Parse(array9[3]);
					}
				}
			}
			string[] array10 = Misc.Split(personsBehaviorScriptVariablesString, "\n", StringSplitOptions.RemoveEmptyEntries);
			foreach (string text9 in array10)
			{
				string[] array12 = Misc.Split(text9, "|", StringSplitOptions.None);
				if (array12.Length == 2)
				{
					string text10 = array12[0];
					string text11 = array12[1];
					Person personById = Managers.personManager.GetPersonById(text10);
					if (personById != null)
					{
						if (!personById.isOurPerson)
						{
							personById.SetMyBehaviorScriptVariablesFromString(text11);
						}
					}
					else
					{
						Log.Debug("Parsing personsBehaviorScriptVariablesString didn't find Person " + text10);
					}
				}
			}
			if (!string.IsNullOrEmpty(thingAttributesString))
			{
				string[] array13 = Misc.Split(thingAttributesString, "\n", StringSplitOptions.RemoveEmptyEntries);
				bool flag = Managers.areaManager != null && Managers.areaManager.weAreEditorOfCurrentArea;
				bool flag2 = flag && Our.seeInvisibleAsEditor;
				bool flag3 = flag && Our.touchUncollidableAsEditor;
				foreach (string text12 in array13)
				{
					string[] array15 = Misc.Split(text12, "|", StringSplitOptions.None);
					if (array15.Length == 4)
					{
						int num7 = 0;
						ThingSpecifierType syncingUncompressedSpecifierType3 = PersonManager.GetSyncingUncompressedSpecifierType(array15[num7++]);
						string text13 = array15[num7++];
						Thing thingScriptBySpecifier2 = this.GetThingScriptBySpecifier(syncingUncompressedSpecifierType3, text13);
						if (thingScriptBySpecifier2 != null)
						{
							thingScriptBySpecifier2.invisible = PersonManager.GetSyncingUncompressedBool(array15[num7++]);
							thingScriptBySpecifier2.uncollidable = PersonManager.GetSyncingUncompressedBool(array15[num7++]);
							thingScriptBySpecifier2.persistentAttributeChangingCommandWasApplied = true;
							thingScriptBySpecifier2.UpdateAllVisibilityAndCollision(flag2, flag3);
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(movableByEveryoneThingsString))
			{
				string[] array16 = Misc.Split(movableByEveryoneThingsString, "\n", StringSplitOptions.RemoveEmptyEntries);
				foreach (string text14 in array16)
				{
					string[] array18 = Misc.Split(text14, "|", StringSplitOptions.None);
					if (array18.Length == 3)
					{
						int num9 = 0;
						string text15 = array18[num9++];
						Vector3 syncingUncompressedVector = PersonManager.GetSyncingUncompressedVector3(array18[num9++]);
						Quaternion syncingUncompressedQuaternion = PersonManager.GetSyncingUncompressedQuaternion(array18[num9++]);
						GameObject placementById = Managers.thingManager.GetPlacementById(text15, false);
						if (placementById != null)
						{
							placementById.transform.localPosition = syncingUncompressedVector;
							placementById.transform.localRotation = syncingUncompressedQuaternion;
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Debug("We received invalid data for DoInformOfBehaviorScriptVariablesAndThingStates_Remote");
		}
		string[] thingPhysicsStringItems = Misc.Split(thingPhysicsString, "\n", StringSplitOptions.RemoveEmptyEntries);
		foreach (string item in thingPhysicsStringItems)
		{
			string[] parts = Misc.Split(item, "|", StringSplitOptions.None);
			if (parts.Length == 8)
			{
				int i = 0;
				string[] array20 = parts;
				int num11;
				i = (num11 = i) + 1;
				ThingSpecifierType specifierType = PersonManager.GetSyncingUncompressedSpecifierType(array20[num11]);
				string[] array21 = parts;
				i = (num11 = i) + 1;
				string specifierId = array21[num11];
				string[] array22 = parts;
				i = (num11 = i) + 1;
				string thingId = array22[num11];
				string[] array23 = parts;
				i = (num11 = i) + 1;
				Vector3 position = PersonManager.GetSyncingUncompressedVector3(array23[num11]);
				string[] array24 = parts;
				i = (num11 = i) + 1;
				Quaternion rotation = PersonManager.GetSyncingUncompressedQuaternion(array24[num11]);
				string[] array25 = parts;
				i = (num11 = i) + 1;
				Vector3 velocity = PersonManager.GetSyncingUncompressedVector3(array25[num11]);
				string[] array26 = parts;
				i = (num11 = i) + 1;
				Vector3 angularVelocity = PersonManager.GetSyncingUncompressedVector3(array26[num11]);
				string[] array27 = parts;
				i = (num11 = i) + 1;
				float destroyMeInTime = PersonManager.GetSyncingUncompressedFloat(array27[num11]);
				if (destroyMeInTime != -1f)
				{
					destroyMeInTime = Misc.ClampMin(destroyMeInTime - originClientTime + clientTime, 0f);
				}
				GameObject thingObject = this.GetThingBySpecifier(specifierType, specifierId);
				if (thingObject != null)
				{
					Thing component = thingObject.GetComponent<Thing>();
					if (component != null)
					{
						component.transform.position = position;
						component.transform.rotation = rotation;
						if (component.rigidbody != null)
						{
							component.rigidbody.velocity = velocity;
							component.rigidbody.angularVelocity = angularVelocity;
						}
						else
						{
							component.ThrowMe(velocity, angularVelocity, specifierId);
						}
						component.destroyMeInTime = destroyMeInTime;
					}
				}
				else
				{
					yield return base.StartCoroutine(Managers.thingManager.InstantiateThingViaCache(ThingRequestContext.DoInformOfBehaviorScriptVariablesAndThingStates_Remote, thingId, delegate(GameObject returnThing)
					{
						thingObject = returnThing;
					}, false, false, -1, null));
					Thing thing = thingObject.GetComponent<Thing>();
					if (thing != null)
					{
						thing.transform.position = position;
						thing.transform.rotation = rotation;
						thing.ThrowMe(velocity, angularVelocity, specifierId);
						thing.destroyMeInTime = destroyMeInTime;
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(slideshowUrlsString))
		{
			string[] array28 = Misc.Split(slideshowUrlsString, "\n", StringSplitOptions.RemoveEmptyEntries);
			foreach (string text16 in array28)
			{
				string[] array30 = Misc.Split(text16, "|", StringSplitOptions.None);
				if (array30.Length == 7)
				{
					int num13 = 0;
					ThingSpecifierType syncingUncompressedSpecifierType4 = PersonManager.GetSyncingUncompressedSpecifierType(array30[num13++]);
					string text17 = array30[num13++];
					int num14 = int.Parse(array30[num13++]);
					List<string> list = new List<string>(Misc.Split(array30[num13++], " ", StringSplitOptions.RemoveEmptyEntries));
					int num15 = int.Parse(array30[num13++]);
					string text18 = array30[num13++];
					bool syncingUncompressedBool3 = PersonManager.GetSyncingUncompressedBool(array30[num13++]);
					ThingPart thingPartBySpecifier2 = this.GetThingPartBySpecifier(syncingUncompressedSpecifierType4, text17, num14);
					if (thingPartBySpecifier2 != null && thingPartBySpecifier2.offersSlideshowScreen)
					{
						ThingPartSlideshow orAddSlideshow = this.GetOrAddSlideshow(thingPartBySpecifier2);
						orAddSlideshow.SetUrls(list, text18, num15);
						if (syncingUncompressedBool3)
						{
							orAddSlideshow.Play();
						}
						else
						{
							orAddSlideshow.MasterClientSwitchedToThisImage(num15);
						}
					}
				}
			}
		}
		Managers.temporarilyDestroyedThingsManager.SetFromMasterClientSyncString(temporarilyDestroyedThingsStrings);
		yield break;
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x000BA3D8 File Offset: 0x000B87D8
	[PunRPC]
	public void DoInformOfThingPhysics_Remote(ThingSpecifierType specifierType, string specifierId, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
	{
		Thing thingScriptBySpecifier = this.GetThingScriptBySpecifier(specifierType, specifierId);
		if (thingScriptBySpecifier != null)
		{
			if (thingScriptBySpecifier.rigidbody != null)
			{
				thingScriptBySpecifier.transform.position = position;
				thingScriptBySpecifier.transform.rotation = rotation;
				thingScriptBySpecifier.rigidbody.velocity = velocity;
				thingScriptBySpecifier.rigidbody.angularVelocity = angularVelocity;
			}
		}
		else
		{
			Log.Debug("DoInformOfThingPhysics_Remote didn't find Thing, so ignoring.");
		}
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x000BA450 File Offset: 0x000B8850
	[PunRPC]
	public void DoSendTestCall_Remote(string senderName, string info)
	{
		Log.Debug("-- DoSendTestCall_Remote --");
		Log.Debug("This person object name: " + this.screenName);
		Log.Debug("We are: " + Managers.personManager.ourPerson.screenName);
		Log.Debug("- Sender name: " + senderName);
		Log.Debug("- Info: " + info);
		Log.Debug("This person object isOurPerson: " + this.isOurPerson);
		Log.Debug("This person object isMasterClient: " + this.isMasterClient);
		Log.Debug("We are masterClient: " + Managers.personManager.ourPerson.isMasterClient);
		Log.Debug("---------------------------");
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x000BA51C File Offset: 0x000B891C
	[PunRPC]
	public void DoWeSwitchedToThisSlideshowImage_Remote(ThingSpecifierType specifierType, string specifierId, int indexWithinThing, int urlIndex)
	{
		ThingPart thingPartBySpecifier = this.GetThingPartBySpecifier(specifierType, specifierId, indexWithinThing);
		if (thingPartBySpecifier != null)
		{
			ThingPartSlideshow orAddSlideshow = this.GetOrAddSlideshow(thingPartBySpecifier);
			orAddSlideshow.MasterClientSwitchedToThisImage(urlIndex);
		}
		else
		{
			Log.Debug("DoWeSwitchedToThisSlideshowImage_Remote didn't find ThingPart");
		}
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x000BA560 File Offset: 0x000B8960
	[PunRPC]
	public void DoSlideshowControl_SetUrls_Remote(ThingSpecifierType specifierType, string specifierId, int indexWithinThing, string[] urlsArray, string searchText)
	{
		ThingPart thingPartBySpecifier = this.GetThingPartBySpecifier(specifierType, specifierId, indexWithinThing);
		if (thingPartBySpecifier != null)
		{
			ThingPartSlideshow orAddSlideshow = this.GetOrAddSlideshow(thingPartBySpecifier);
			List<string> list = new List<string>(urlsArray);
			orAddSlideshow.SetUrls(list, searchText, 0);
		}
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x000BA5A0 File Offset: 0x000B89A0
	[PunRPC]
	public void DoSlideshowControl_Play_Remote(ThingSpecifierType specifierType, string specifierId, int indexWithinThing)
	{
		ThingPart thingPartBySpecifier = this.GetThingPartBySpecifier(specifierType, specifierId, indexWithinThing);
		if (thingPartBySpecifier != null)
		{
			ThingPartSlideshow orAddSlideshow = this.GetOrAddSlideshow(thingPartBySpecifier);
			orAddSlideshow.Play();
		}
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x000BA5D4 File Offset: 0x000B89D4
	[PunRPC]
	public void DoSlideshowControl_Pause_Remote(ThingSpecifierType specifierType, string specifierId, int indexWithinThing)
	{
		ThingPart thingPartBySpecifier = this.GetThingPartBySpecifier(specifierType, specifierId, indexWithinThing);
		if (thingPartBySpecifier != null)
		{
			ThingPartSlideshow orAddSlideshow = this.GetOrAddSlideshow(thingPartBySpecifier);
			orAddSlideshow.Pause();
		}
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x000BA608 File Offset: 0x000B8A08
	[PunRPC]
	public void DoSlideshowControl_Stop_Remote(ThingSpecifierType specifierType, string specifierId, int indexWithinThing)
	{
		ThingPart thingPartBySpecifier = this.GetThingPartBySpecifier(specifierType, specifierId, indexWithinThing);
		if (thingPartBySpecifier != null)
		{
			ThingPartSlideshow component = thingPartBySpecifier.GetComponent<ThingPartSlideshow>();
			global::UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x000BA638 File Offset: 0x000B8A38
	[PunRPC]
	public void DoInformOfThingInvisibleUncollidableChange_Remote(ThingSpecifierType specifierType, string specifierId, bool invisible, bool uncollidable)
	{
		Thing thingScriptBySpecifier = this.GetThingScriptBySpecifier(specifierType, specifierId);
		if (thingScriptBySpecifier != null && thingScriptBySpecifier.gameObject != CreationHelper.thingBeingEdited)
		{
			thingScriptBySpecifier.invisible = invisible;
			thingScriptBySpecifier.uncollidable = uncollidable;
			thingScriptBySpecifier.UpdateAllVisibilityAndCollision(false, false);
		}
	}

	// Token: 0x06001553 RID: 5459 RVA: 0x000BA688 File Offset: 0x000B8A88
	[PunRPC]
	public void DoInformOfThingPartInvisibleUncollidableChange_Remote(ThingSpecifierType specifierType, string specifierId, int indexWithinThing, bool invisible, bool uncollidable)
	{
		ThingPart thingPartBySpecifier = this.GetThingPartBySpecifier(specifierType, specifierId, indexWithinThing);
		if (thingPartBySpecifier != null && !thingPartBySpecifier.GetIsOfThingBeingEdited())
		{
			thingPartBySpecifier.invisible = invisible;
			thingPartBySpecifier.uncollidable = uncollidable;
			thingPartBySpecifier.ApplyCurrentInvisibleAndCollidable();
		}
		else
		{
			Log.Debug("Didn't find thing part");
		}
	}

	// Token: 0x06001554 RID: 5460 RVA: 0x000BA6DC File Offset: 0x000B8ADC
	[PunRPC]
	public void DoRedundantlyInformAboutThingDestruction_Remote(string placementId, bool burst, float burstVelocity, int maxParts, float growth, bool bouncy, bool slidy, float hidePartsInSeconds, float restoreInSeconds, bool gravity, bool collides, bool collidesWithSiblings)
	{
		GameObject placementById = Managers.thingManager.GetPlacementById(placementId, false);
		Thing thing = ((!(placementById != null)) ? null : placementById.GetComponent<Thing>());
		if (thing != null)
		{
			ThingDestruction thingDestruction = new ThingDestruction();
			thingDestruction.burst = burst;
			thingDestruction.burstVelocity = burstVelocity;
			thingDestruction.maxParts = maxParts;
			thingDestruction.growth = growth;
			thingDestruction.bouncy = bouncy;
			thingDestruction.slidy = slidy;
			thingDestruction.hidePartsInSeconds = hidePartsInSeconds;
			thingDestruction.restoreInSeconds = ((restoreInSeconds != 0f) ? new float?(restoreInSeconds) : null);
			thingDestruction.gravity = gravity;
			thingDestruction.collides = collides;
			thingDestruction.collidesWithSiblings = collidesWithSiblings;
			if (thingDestruction.burst)
			{
				Effects.BreakIntoPieces(thing, thingDestruction);
			}
			Managers.temporarilyDestroyedThingsManager.AddPlacement(thing, thingDestruction.restoreInSeconds);
		}
	}

	// Token: 0x06001555 RID: 5461 RVA: 0x000BA7BC File Offset: 0x000B8BBC
	[PunRPC]
	public void DoHoldAsMovableByEveryone_Remote(string placementId, Side handSide, Vector3 position, Quaternion rotation)
	{
		GameObject gameObject = Managers.thingManager.GetPlacementById(placementId, false);
		if (gameObject == null)
		{
			GameObject handBySide = this.GetHandBySide(Misc.GetOppositeSide(handSide));
			if (handBySide != null)
			{
				gameObject = this.GetThingInHand(handBySide, false);
				Thing component = gameObject.GetComponent<Thing>();
				if (component == null || component.placementId != placementId)
				{
					gameObject = null;
				}
			}
		}
		if (gameObject != null)
		{
			GameObject handBySide2 = this.GetHandBySide(handSide);
			if (handBySide2 != null)
			{
				gameObject.transform.parent = handBySide2.transform;
				gameObject.transform.localPosition = position;
				gameObject.transform.localRotation = rotation;
				Thing component2 = gameObject.GetComponent<Thing>();
				gameObject.tag = component2.GetCurrentlyHeldTag();
				if (component2 != null)
				{
					component2.TriggerEvent(StateListener.EventType.OnTaken, string.Empty, false, null);
				}
				Managers.soundManager.Play("pickUp", handBySide2.transform, 0.3f, false, false);
			}
		}
		else
		{
			Log.Debug("DoHoldAsMovableByEveryone_Remote placementId " + placementId + " not found");
		}
	}

	// Token: 0x06001556 RID: 5462 RVA: 0x000BA8E0 File Offset: 0x000B8CE0
	[PunRPC]
	public void DoPlaceAsMovableByEveryone_Remote(string placementId, Side handSide, Vector3 position, Quaternion rotation)
	{
		GameObject handBySide = this.GetHandBySide(handSide);
		GameObject gameObject = this.GetThingInHand(handBySide, false);
		bool flag = gameObject == null;
		if (flag)
		{
			gameObject = Managers.thingManager.GetPlacementById(placementId, false);
		}
		if (gameObject != null)
		{
			gameObject.transform.parent = Managers.thingManager.placements.transform;
			gameObject.transform.position = position;
			gameObject.transform.rotation = rotation;
			gameObject.tag = "Thing";
			Thing component = gameObject.GetComponent<Thing>();
			if (component != null)
			{
				component.TriggerEvent(StateListener.EventType.OnLetGo, string.Empty, false, null);
			}
			Managers.soundManager.Play("putDown", handBySide.transform, 0.3f, false, false);
		}
		else
		{
			Log.Debug("DoPlaceAsMovableByEveryone_Remote placementId " + placementId + " not found placed or in hand");
		}
	}

	// Token: 0x06001557 RID: 5463 RVA: 0x000BA9BC File Offset: 0x000B8DBC
	[PunRPC]
	public void DoPlaySound_Remote(string soundName, Vector3 position, float volume)
	{
		Managers.soundManager.Play(soundName, position, volume, false, false);
	}

	// Token: 0x06001558 RID: 5464 RVA: 0x000BA9D0 File Offset: 0x000B8DD0
	[PunRPC]
	public void DoInformOnMovableByEveryoneGroupTransforms_Remote(string[] items, Side holdingHandSide, MovableByEveryoneGroupInformType informType)
	{
		Transform transform = Managers.thingManager.placements.transform;
		GameObject handBySide = this.GetHandBySide(holdingHandSide);
		foreach (string text in items)
		{
			string[] array = Misc.Split(text, "|", StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 3)
			{
				int num = 0;
				string text2 = array[num++];
				Vector3 syncingUncompressedVector = PersonManager.GetSyncingUncompressedVector3(array[num++]);
				Quaternion syncingUncompressedQuaternion = PersonManager.GetSyncingUncompressedQuaternion(array[num++]);
				if (informType == MovableByEveryoneGroupInformType.PutDown)
				{
					IEnumerator enumerator = handBySide.transform.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Transform transform2 = (Transform)obj;
							if (transform2.CompareTag("Thing"))
							{
								transform2.parent = transform;
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
				GameObject gameObject = null;
				if (informType == MovableByEveryoneGroupInformType.Shuffle)
				{
					IEnumerator enumerator2 = handBySide.transform.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							Transform transform3 = (Transform)obj2;
							if (transform3.CompareTag("Thing"))
							{
								Thing component = transform3.GetComponent<Thing>();
								if (component.placementId == text2)
								{
									gameObject = component.gameObject;
									break;
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
				}
				else
				{
					gameObject = Managers.thingManager.GetPlacementById(text2, false);
				}
				if (gameObject != null)
				{
					if (informType == MovableByEveryoneGroupInformType.PickUp)
					{
						gameObject.transform.parent = handBySide.transform;
					}
					gameObject.transform.localPosition = syncingUncompressedVector;
					gameObject.transform.localRotation = syncingUncompressedQuaternion;
					if (informType == MovableByEveryoneGroupInformType.PickUp || informType == MovableByEveryoneGroupInformType.PutDown)
					{
						Thing component2 = gameObject.GetComponent<Thing>();
						if (component2 != null)
						{
							ThingMovableByEveryone component3 = component2.GetComponent<ThingMovableByEveryone>();
							if (component3 != null)
							{
								if (informType == MovableByEveryoneGroupInformType.PickUp)
								{
									component3.OnPickUp(false);
								}
								else if (informType == MovableByEveryoneGroupInformType.PutDown)
								{
									component3.OnPutDown(false);
								}
							}
						}
					}
				}
				else
				{
					Log.Debug("DoInformOnMovableByEveryoneGroupTransforms_Remote placementId " + text2 + " not found");
				}
			}
			else
			{
				Log.Debug("DoInformOnMovableByEveryoneGroupTransforms_Remote received false data structure");
			}
		}
		Vector3 position = handBySide.transform.position;
		if (informType != MovableByEveryoneGroupInformType.PickUp)
		{
			if (informType != MovableByEveryoneGroupInformType.PutDown)
			{
				if (informType == MovableByEveryoneGroupInformType.Shuffle)
				{
					Managers.soundManager.Play("shuffle", position, 0.3f, false, false);
				}
			}
			else
			{
				Managers.soundManager.Play("putDown", position, 0.35f, false, false);
			}
		}
		else
		{
			Managers.soundManager.Play("pickUp", position, 0.3f, false, false);
		}
	}

	// Token: 0x06001559 RID: 5465 RVA: 0x000BACCC File Offset: 0x000B90CC
	[PunRPC]
	public void DoSetBrowserUrl_Remote(ThingSpecifierType specifierType, string specifierId, string url, bool isSetByThisPerson)
	{
		Thing thingScriptBySpecifier = this.GetThingScriptBySpecifier(specifierType, specifierId);
		if (thingScriptBySpecifier != null)
		{
			if (Managers.browserManager.IsValidUrlProtocol(url) && thingScriptBySpecifier.gameObject != CreationHelper.thingBeingEdited)
			{
				Browser browser = (Browser)thingScriptBySpecifier.GetComponentInChildren(typeof(Browser), true);
				if (browser == null)
				{
					Component[] componentsInChildren = thingScriptBySpecifier.GetComponentsInChildren(typeof(ThingPart), true);
					foreach (ThingPart thingPart in componentsInChildren)
					{
						if (thingPart.offersScreen)
						{
							BrowserSettings browserSettings = new BrowserSettings();
							browserSettings.url = url;
							browser = Managers.browserManager.TryAttachBrowser(thingPart, browserSettings, true);
							break;
						}
					}
				}
				else
				{
					ThingPart browserThingPart = Managers.browserManager.GetBrowserThingPart(browser);
					if (browserThingPart != null && !Managers.browserManager.UrlIsBlockedInThisCase(browserThingPart, url) && browser.Url != url)
					{
						browser.lastRemotePersonReceivedUrl = url;
						browser.Url = url;
					}
				}
				if (browser != null && isSetByThisPerson)
				{
					browser.AddUrlSetByPersonId(url, this.userId);
				}
			}
		}
		else
		{
			Log.Debug("Didn't find thing in DoSetBrowserUrl_Remote: " + specifierType.ToString() + ", " + specifierId);
		}
	}

	// Token: 0x0600155A RID: 5466 RVA: 0x000BAE34 File Offset: 0x000B9234
	[PunRPC]
	public void DoCloseBrowser_Remote(ThingSpecifierType specifierType, string specifierId)
	{
		Thing thingScriptBySpecifier = this.GetThingScriptBySpecifier(specifierType, specifierId);
		if (thingScriptBySpecifier != null)
		{
			Component[] componentsInChildren = thingScriptBySpecifier.GetComponentsInChildren(typeof(Browser), true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				global::UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
				componentsInChildren[i] = null;
			}
		}
		else
		{
			Log.Debug("Didn't find thing in DoCloseBrowser_Remote");
		}
	}

	// Token: 0x0600155B RID: 5467 RVA: 0x000BAE9C File Offset: 0x000B929C
	[PunRPC]
	public void DoReloadBrowserPage_Remote(ThingSpecifierType specifierType, string specifierId, string url)
	{
		Thing thingScriptBySpecifier = this.GetThingScriptBySpecifier(specifierType, specifierId);
		if (thingScriptBySpecifier != null)
		{
			Browser browser = (Browser)thingScriptBySpecifier.GetComponentInChildren(typeof(Browser), true);
			if (browser != null)
			{
				browser.Reload(true);
				base.StartCoroutine(Managers.browserManager.RegisterBrowserFunctionsDelayed(browser));
			}
		}
		else
		{
			Log.Debug("Didn't find thing in DoReloadBrowserPage_Remote");
		}
	}

	// Token: 0x0600155C RID: 5468 RVA: 0x000BAF09 File Offset: 0x000B9309
	[PunRPC]
	public void DoSetAmplifySpeech_Remote(bool state)
	{
		this.SetAmplifySpeech(state);
	}

	// Token: 0x0600155D RID: 5469 RVA: 0x000BAF14 File Offset: 0x000B9314
	[PunRPC]
	public void DoIndicateIsSpeaking_Remote()
	{
		if (this.speechIndicatorParticleSystem != null)
		{
			ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
			this.speechIndicatorParticleSystem.Emit(emitParams, 1);
		}
	}

	// Token: 0x0600155E RID: 5470 RVA: 0x000BAF47 File Offset: 0x000B9347
	[PunRPC]
	public void DoSetAreaIsExcluded_Remote(bool isExcluded)
	{
		if (!this.ConfirmIsEditorHere())
		{
			return;
		}
		Managers.areaManager.SetIsExcluded_Local(isExcluded);
	}

	// Token: 0x0600155F RID: 5471 RVA: 0x000BAF60 File Offset: 0x000B9360
	private ThingPartSlideshow GetOrAddSlideshow(ThingPart thingPart)
	{
		ThingPartSlideshow thingPartSlideshow = thingPart.GetComponent<ThingPartSlideshow>();
		if (thingPartSlideshow == null)
		{
			thingPartSlideshow = thingPart.gameObject.AddComponent<ThingPartSlideshow>();
		}
		return thingPartSlideshow;
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x000BAF90 File Offset: 0x000B9390
	private ThingPart GetThingPartBySpecifier(ThingSpecifierType specifierType, string specifierId, int thingPartIndex)
	{
		ThingPart thingPart = null;
		GameObject thingBySpecifier = this.GetThingBySpecifier(specifierType, specifierId);
		if (thingBySpecifier != null)
		{
			thingPart = this.GetThingPartScriptByIndex(thingBySpecifier, thingPartIndex);
		}
		return thingPart;
	}

	// Token: 0x06001561 RID: 5473 RVA: 0x000BAFC0 File Offset: 0x000B93C0
	private void ClearWhatHandHoldsIfNeeded(TopographyId topographyId)
	{
		GameObject handByTopographyId = this.GetHandByTopographyId(topographyId);
		GameObject thingInHand = this.GetThingInHand(handByTopographyId, false);
		if (thingInHand != null)
		{
			global::UnityEngine.Object.Destroy(thingInHand);
		}
	}

	// Token: 0x06001562 RID: 5474 RVA: 0x000BAFF0 File Offset: 0x000B93F0
	private GameObject GetThingPartByIndex(GameObject thing, int index)
	{
		GameObject gameObject = null;
		Component[] componentsInChildren = thing.GetComponentsInChildren(typeof(ThingPart));
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.indexWithinThing == index)
			{
				gameObject = thingPart.gameObject;
				break;
			}
		}
		return gameObject;
	}

	// Token: 0x06001563 RID: 5475 RVA: 0x000BB050 File Offset: 0x000B9450
	private ThingPart GetThingPartScriptByIndex(GameObject thing, int index)
	{
		ThingPart thingPart = null;
		Component[] componentsInChildren = thing.GetComponentsInChildren(typeof(ThingPart));
		foreach (ThingPart thingPart2 in componentsInChildren)
		{
			if (thingPart2.indexWithinThing == index)
			{
				thingPart = thingPart2;
				break;
			}
		}
		return thingPart;
	}

	// Token: 0x06001564 RID: 5476 RVA: 0x000BB0A8 File Offset: 0x000B94A8
	private Thing GetThingScriptBySpecifier(ThingSpecifierType specifierType, string id)
	{
		Thing thing = null;
		GameObject thingBySpecifier = this.GetThingBySpecifier(specifierType, id);
		if (thingBySpecifier != null)
		{
			thing = thingBySpecifier.GetComponent<Thing>();
		}
		return thing;
	}

	// Token: 0x06001565 RID: 5477 RVA: 0x000BB0D4 File Offset: 0x000B94D4
	private GameObject GetThingBySpecifier(ThingSpecifierType specifierType, string id)
	{
		GameObject gameObject = null;
		string text = null;
		if (!string.IsNullOrEmpty(id) && id.Contains(";"))
		{
			string[] array = Misc.Split(id, ";", StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 2)
			{
				id = array[0];
				text = array[1];
			}
		}
		Person person = this;
		string text2 = null;
		if (!string.IsNullOrEmpty(id) && id.Contains("~") && Managers.personManager != null)
		{
			string[] array2 = Misc.Split(id, "~", StringSplitOptions.None);
			if (array2.Length >= 2)
			{
				string text3 = array2[0];
				id = array2[1];
				if (array2.Length == 3)
				{
					text2 = array2[2];
				}
				if (!string.IsNullOrEmpty(text3) && text3 != person.userId)
				{
					Person personById = Managers.personManager.GetPersonById(text3);
					if (personById != null)
					{
						person = personById;
					}
				}
			}
		}
		switch (specifierType)
		{
		case ThingSpecifierType.Hand:
		{
			TopographyId topographyId = (TopographyId)Enum.Parse(typeof(TopographyId), id);
			GameObject handByTopographyId = person.GetHandByTopographyId(topographyId);
			gameObject = this.GetThingInHand(handByTopographyId, false);
			break;
		}
		case ThingSpecifierType.Thrown:
			gameObject = Managers.thingManager.GetThingByThrownId(id);
			break;
		case ThingSpecifierType.Attachment:
		{
			AttachmentPointId attachmentPointId = (AttachmentPointId)Enum.Parse(typeof(AttachmentPointId), id);
			gameObject = person.GetThingOnAttachmentPointById(attachmentPointId);
			break;
		}
		case ThingSpecifierType.Placement:
			gameObject = Managers.thingManager.GetPlacementById(id, false);
			break;
		case ThingSpecifierType.HandMovableByEveryone:
		{
			TopographyId topographyId2 = (TopographyId)Enum.Parse(typeof(TopographyId), id);
			GameObject handByTopographyId2 = person.GetHandByTopographyId(topographyId2);
			gameObject = this.GetMovableByEveryoneInHand(handByTopographyId2, text2);
			break;
		}
		}
		if (gameObject != null && !string.IsNullOrEmpty(text))
		{
			Component[] componentsInChildren = gameObject.gameObject.GetComponentsInChildren(typeof(Thing), true);
			foreach (Component component in componentsInChildren)
			{
				Thing component2 = component.GetComponent<Thing>();
				if (component2 != null && component2.crossClientSubThingId == text)
				{
					gameObject = component2.gameObject;
					break;
				}
			}
		}
		return gameObject;
	}

	// Token: 0x06001566 RID: 5478 RVA: 0x000BB314 File Offset: 0x000B9714
	private TopographyId GetMirroredTopographyId(TopographyId otherTopographyId)
	{
		TopographyId topographyId = TopographyId.None;
		if (otherTopographyId != TopographyId.Left)
		{
			if (otherTopographyId == TopographyId.Right)
			{
				topographyId = TopographyId.Left;
			}
		}
		else
		{
			topographyId = TopographyId.Right;
		}
		return topographyId;
	}

	// Token: 0x06001567 RID: 5479 RVA: 0x000BB34C File Offset: 0x000B974C
	public GameObject GetAttachmentPointById(AttachmentPointId id, bool allowReturningNull = false)
	{
		if (this.AttachmentPointsById == null)
		{
			this.SetAttachmentPointReferencesById();
		}
		if (this.AttachmentPointsById == null)
		{
			throw new Exception("GetAttachmentPointById called before AttachmentPointsById have been initialized (in SetAttachmentPointReferencesById)");
		}
		GameObject gameObject;
		this.AttachmentPointsById.TryGetValue(id, out gameObject);
		if (!allowReturningNull)
		{
		}
		return gameObject;
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x000BB398 File Offset: 0x000B9798
	public GameObject GetThingInHand(GameObject hand, bool restrictToFindingHoldables = false)
	{
		GameObject gameObject = null;
		if (hand != null)
		{
			IEnumerator enumerator = hand.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.gameObject.CompareTag("CurrentlyHeldLeft") || transform.gameObject.CompareTag("CurrentlyHeldRight") || transform.gameObject.CompareTag("Thing"))
					{
						if (!restrictToFindingHoldables)
						{
							gameObject = transform.gameObject;
							break;
						}
						Thing component = transform.gameObject.GetComponent<Thing>();
						if (component != null)
						{
							gameObject = transform.gameObject;
							break;
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
		return gameObject;
	}

	// Token: 0x06001569 RID: 5481 RVA: 0x000BB484 File Offset: 0x000B9884
	public GameObject GetMovableByEveryoneInHand(GameObject hand, string placementId)
	{
		GameObject gameObject = null;
		if (hand != null)
		{
			IEnumerator enumerator = hand.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					Thing component = transform.GetComponent<Thing>();
					if (component != null && component.movableByEveryone && component.placementId == placementId)
					{
						gameObject = transform.gameObject;
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
		return gameObject;
	}

	// Token: 0x0600156A RID: 5482 RVA: 0x000BB530 File Offset: 0x000B9930
	private int GetBodyAndHeldThingPartCount()
	{
		return base.GetComponentsInChildren<ThingPart>().Length;
	}

	// Token: 0x0600156B RID: 5483 RVA: 0x000BB53C File Offset: 0x000B993C
	public int GetBodyThingPartAndStrictSyncCount(out int strictSyncCount)
	{
		int num = 0;
		strictSyncCount = 0;
		foreach (KeyValuePair<AttachmentPointId, GameObject> keyValuePair in this.AttachmentPointsById)
		{
			GameObject value = keyValuePair.Value;
			if (value != null)
			{
				num += Managers.thingManager.GetThingPartCountFullDepthWithStrictSyncCount(value, out strictSyncCount);
			}
		}
		return num;
	}

	// Token: 0x0600156C RID: 5484 RVA: 0x000BB5BC File Offset: 0x000B99BC
	public GameObject GetThingOnAttachmentPointById(AttachmentPointId attachmentPointId)
	{
		GameObject attachmentPointById = this.GetAttachmentPointById(attachmentPointId, false);
		return attachmentPointById.GetComponent<AttachmentPoint>().attachedThing;
	}

	// Token: 0x0600156D RID: 5485 RVA: 0x000BB5DF File Offset: 0x000B99DF
	public bool CanWeMoveForward(float maxDistance = 0.25f)
	{
		return this.WeHaveGroundToWalkOn() && this.DirectionIsFree(this.Torso.transform.forward, maxDistance);
	}

	// Token: 0x0600156E RID: 5486 RVA: 0x000BB606 File Offset: 0x000B9A06
	public bool CanWeMoveBackward(float maxDistance = 0.25f)
	{
		return this.WeHaveGroundToWalkOn() && this.DirectionIsFree(-this.Torso.transform.forward, maxDistance);
	}

	// Token: 0x0600156F RID: 5487 RVA: 0x000BB632 File Offset: 0x000B9A32
	public void MoveUsForward(float speedFactor = 1f)
	{
		this.Rig.transform.Translate(this.Torso.transform.forward * (Time.deltaTime * speedFactor), Space.World);
	}

	// Token: 0x06001570 RID: 5488 RVA: 0x000BB661 File Offset: 0x000B9A61
	public void MoveUsBackward(float speedFactor = 1f)
	{
		this.Rig.transform.Translate(-(this.Torso.transform.forward * (Time.deltaTime * speedFactor)), Space.World);
	}

	// Token: 0x06001571 RID: 5489 RVA: 0x000BB695 File Offset: 0x000B9A95
	public void MoveUsLeft(float speedFactor = 1f)
	{
		this.Rig.transform.Translate(-(this.Torso.transform.right * (Time.deltaTime * speedFactor)), Space.World);
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x000BB6C9 File Offset: 0x000B9AC9
	public void MoveUsRight(float speedFactor = 1f)
	{
		this.Rig.transform.Translate(this.Torso.transform.right * (Time.deltaTime * speedFactor), Space.World);
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x000BB6F8 File Offset: 0x000B9AF8
	public void MoveUsUpward(float speedFactor = 1f)
	{
		this.Rig.transform.Translate(this.Torso.transform.up * (Time.deltaTime * speedFactor), Space.World);
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x000BB727 File Offset: 0x000B9B27
	public void MoveUsDownward(float speedFactor = 1f)
	{
		this.Rig.transform.Translate(-(this.Torso.transform.up * (Time.deltaTime * speedFactor)), Space.World);
	}

	// Token: 0x06001575 RID: 5493 RVA: 0x000BB75C File Offset: 0x000B9B5C
	private bool WeHaveGroundToWalkOn()
	{
		bool flag = false;
		Ray ray = new Ray(this.Torso.transform.position, -this.Torso.transform.up);
		float num = 2f;
		foreach (RaycastHit raycastHit in (from h in Physics.RaycastAll(ray, num)
			orderby h.distance
			select h).ToArray<RaycastHit>())
		{
			Transform transform = raycastHit.collider.transform;
			if (transform != null && transform.parent != null)
			{
				Thing component = transform.parent.GetComponent<Thing>();
				ThingPart component2 = transform.GetComponent<ThingPart>();
				if (component != null && component2 != null)
				{
					bool flag2 = raycastHit.normal.y >= 0.5f;
					bool flag3 = !string.IsNullOrEmpty(component.placementId);
					if (!component.isPassable && !component2.isLiquid)
					{
						if (flag3 && flag2 && !component.isUnwalkable)
						{
							flag = true;
							break;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06001576 RID: 5494 RVA: 0x000BB8C0 File Offset: 0x000B9CC0
	private bool DirectionIsFree(Vector3 direction, float maxDistance)
	{
		bool flag = true;
		Ray ray = new Ray(this.Torso.transform.position, direction);
		foreach (RaycastHit raycastHit in (from h in Physics.RaycastAll(ray, maxDistance)
			orderby h.distance
			select h).ToArray<RaycastHit>())
		{
			Transform transform = raycastHit.collider.transform;
			if (transform != null && transform.parent != null)
			{
				Thing component = transform.parent.GetComponent<Thing>();
				ThingPart component2 = transform.GetComponent<ThingPart>();
				if (component != null && component2 != null)
				{
					bool flag2 = !string.IsNullOrEmpty(component.placementId);
					if (!component.isPassable && !component2.isLiquid)
					{
						if (flag2)
						{
							flag = false;
							break;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06001577 RID: 5495 RVA: 0x000BB9D4 File Offset: 0x000B9DD4
	public void StartControlControllable(Thing thingOriginal)
	{
		this.controlledControllable = global::UnityEngine.Object.Instantiate<GameObject>(thingOriginal.gameObject, thingOriginal.transform.position, thingOriginal.transform.rotation);
		Managers.thingManager.MakeDeepThingClone(thingOriginal.gameObject, this.controlledControllable, true, false, false);
		if (thingOriginal.IsPlacement())
		{
			thingOriginal.SetTemporarilyInactiveAsControllableIsSpawned();
		}
		Thing component = this.controlledControllable.GetComponent<Thing>();
		component.MakeControllable();
		if (this.isOurPerson)
		{
			Our.SetMode(EditModes.None, false);
		}
	}

	// Token: 0x06001578 RID: 5496 RVA: 0x000BBA58 File Offset: 0x000B9E58
	public void StopControlControllable(Thing thingOriginal)
	{
		if (this.isOurPerson)
		{
		}
		if (this.controlledControllable != null && thingOriginal.gameObject == this.controlledControllable)
		{
			Misc.Destroy(this.controlledControllable);
		}
		this.controlledControllable = null;
	}

	// Token: 0x06001579 RID: 5497 RVA: 0x000BBAAC File Offset: 0x000B9EAC
	public void AddTypedText(string text)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = Misc.GetImgurImageUrl(text) != null;
		Component[] componentsInChildren = this.Rig.GetComponentsInChildren(typeof(ThingPart), true);
		if (flag3)
		{
			foreach (ThingPart thingPart in componentsInChildren)
			{
				if (thingPart.isImagePasteScreen)
				{
					flag = true;
					thingPart.AddTypedText(text, false);
				}
			}
		}
		if (!flag)
		{
			foreach (ThingPart thingPart2 in componentsInChildren)
			{
				if (thingPart2.isText)
				{
					thingPart2.AddTypedText(text, false);
					flag2 = true;
				}
				else
				{
					thingPart2.AddTypedText(text, true);
				}
			}
		}
		if (!flag && !flag2)
		{
			this.SetTextOfDefaultTextChatReceiver(text);
		}
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x000BBB8C File Offset: 0x000B9F8C
	private void SetTextOfDefaultTextChatReceiver(string text)
	{
		if (!this.isOurPerson)
		{
			if (this.defaultTextChatReceiver == null)
			{
				this.AddDefaultTextChatReceiver();
			}
			if (this.defaultTextChatReceiver != null)
			{
				this.defaultTextChatReceiver.text = text;
				base.Invoke("ClearDefaultChatTextReceiver", 15f);
			}
		}
	}

	// Token: 0x0600157B RID: 5499 RVA: 0x000BBBE8 File Offset: 0x000B9FE8
	private void AddDefaultTextChatReceiver()
	{
		GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load("Prefabs/TextChatWrapper", typeof(GameObject))) as GameObject;
		gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
		gameObject.transform.parent = this.AttachmentPointTorsoLower.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		this.defaultTextChatReceiver = gameObject.GetComponentInChildren<TextMesh>();
		Renderer component = this.defaultTextChatReceiver.GetComponent<Renderer>();
		component.material.color = Misc.GetGray(0.75f);
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x000BBC88 File Offset: 0x000BA088
	private void ClearDefaultChatTextReceiver()
	{
		if (this != null && this.defaultTextChatReceiver != null)
		{
			this.defaultTextChatReceiver.text = string.Empty;
		}
	}

	// Token: 0x0600157D RID: 5501 RVA: 0x000BBCB8 File Offset: 0x000BA0B8
	public void ResetBodyStatesAndVariables()
	{
		Component[] componentsInChildren = base.transform.GetComponentsInChildren(typeof(Thing), true);
		foreach (Thing thing in componentsInChildren)
		{
			thing.behaviorScriptVariables = new Dictionary<string, float>();
			thing.ResetStates();
		}
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x000BBD0C File Offset: 0x000BA10C
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

	// Token: 0x0600157F RID: 5503 RVA: 0x000BBD35 File Offset: 0x000BA135
	public void SetBehaviorScriptVariable(string variableName, float value)
	{
		this.behaviorScriptVariables[variableName] = value;
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x000BBD44 File Offset: 0x000BA144
	private void OnDestroy()
	{
		if (Managers.areaManager == null)
		{
			return;
		}
		if (!Managers.areaManager.isTransportInProgress)
		{
			this.DropMovableByEveryoneThings(this.LeftHand);
			this.DropMovableByEveryoneThings(this.RightHand);
		}
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x000BBD80 File Offset: 0x000BA180
	private void DropMovableByEveryoneThings(GameObject hand)
	{
		if (hand != null)
		{
			IEnumerator enumerator = hand.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					Thing component = transform.GetComponent<Thing>();
					if (component != null && component.movableByEveryone)
					{
						transform.parent = Managers.thingManager.placements.transform;
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
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x000BBE20 File Offset: 0x000BA220
	public void SetAmplifySpeech(bool amplifySpeech)
	{
		this.amplifySpeech = amplifySpeech;
		if (this.Head != null)
		{
			AudioSource component = this.Head.GetComponent<AudioSource>();
			component.maxDistance = (float)((!this.amplifySpeech) ? 12 : 2500);
		}
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x000BBE70 File Offset: 0x000BA270
	public bool IsWearingSomethingVisible()
	{
		bool flag = false;
		foreach (KeyValuePair<AttachmentPointId, GameObject> keyValuePair in this.AttachmentPointsById)
		{
			if (keyValuePair.Value != null)
			{
				GameObject value = keyValuePair.Value;
				Component[] componentsInChildren = value.GetComponentsInChildren(typeof(ThingPart), false);
				foreach (ThingPart thingPart in componentsInChildren)
				{
					if (thingPart.transform.parent != null && thingPart.transform.parent.parent != null && thingPart.transform.parent.parent.gameObject == value)
					{
						MeshRenderer component = thingPart.GetComponent<MeshRenderer>();
						if (component != null && component.enabled)
						{
							return true;
						}
					}
				}
			}
		}
		return flag;
	}

	// Token: 0x06001584 RID: 5508 RVA: 0x000BBFA0 File Offset: 0x000BA3A0
	public void SetAttachmentActive(AttachmentPointId attachmentPointId, bool isActive)
	{
		GameObject attachmentPointById = this.GetAttachmentPointById(attachmentPointId, false);
		if (attachmentPointById != null)
		{
			Thing componentInChildren = attachmentPointById.GetComponentInChildren<Thing>();
			if (componentInChildren != null)
			{
				Component[] componentsInChildren = componentInChildren.gameObject.GetComponentsInChildren(typeof(ThingPart), true);
				foreach (ThingPart thingPart in componentsInChildren)
				{
					Renderer component = thingPart.GetComponent<Renderer>();
					if (component != null)
					{
						if (isActive)
						{
							bool? invisibleStateToReset = thingPart.invisibleStateToReset;
							if (invisibleStateToReset != null)
							{
								ThingPart thingPart2 = thingPart;
								bool? invisibleStateToReset2 = thingPart.invisibleStateToReset;
								thingPart2.invisible = invisibleStateToReset2.Value;
								thingPart.invisibleStateToReset = null;
							}
						}
						else
						{
							thingPart.invisibleStateToReset = new bool?(thingPart.invisible);
							thingPart.invisible = true;
						}
						component.enabled = !thingPart.invisible;
					}
					Collider component2 = thingPart.GetComponent<Collider>();
					if (component2 != null)
					{
						if (isActive)
						{
							bool? uncollidableStateToReset = thingPart.uncollidableStateToReset;
							if (uncollidableStateToReset != null)
							{
								ThingPart thingPart3 = thingPart;
								bool? uncollidableStateToReset2 = thingPart.uncollidableStateToReset;
								thingPart3.uncollidable = uncollidableStateToReset2.Value;
								thingPart.uncollidableStateToReset = null;
							}
						}
						else
						{
							thingPart.uncollidableStateToReset = new bool?(thingPart.uncollidable);
							thingPart.uncollidable = true;
						}
						component2.enabled = !thingPart.uncollidable;
					}
				}
			}
		}
	}

	// Token: 0x06001585 RID: 5509 RVA: 0x000BC118 File Offset: 0x000BA518
	private bool ConfirmIsEditorHere()
	{
		bool flag = true;
		if (this.isOurPerson)
		{
			if (Managers.areaManager != null)
			{
				flag = Managers.areaManager.weAreEditorOfCurrentArea;
			}
		}
		else if (this.isEditorHere != null)
		{
			flag = this.isEditorHere.Value;
			if (!flag)
			{
				Log.Debug("Received editor-only RPC from non-editor \"" + this.screenName + "\". They may be hacking, or it's racing after someone toggled their editor status. Ignoring.");
			}
		}
		return flag;
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x000BC195 File Offset: 0x000BA595
	public void MakeMasterClientManually()
	{
		if (this.photonPlayer != null && !this.isMasterClient)
		{
			PhotonNetwork.SetMasterClient(this.photonPlayer);
		}
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x000BC1BC File Offset: 0x000BA5BC
	public void SetRingsGlow(bool doGlow)
	{
		float num = ((!doGlow) ? 0f : 1f);
		foreach (GlowTransition glowTransition in this.rings)
		{
			glowTransition.targetGlow = num;
		}
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x000BC208 File Offset: 0x000BA608
	public void ResetLegsPositionRotationToUniversalDefault(Side? onlyThisSide = null)
	{
		Vector3 vector = new Vector3(-0.15f, -1f, -0.11f);
		Vector3 vector2 = new Vector3(vector.x * -1f, vector.y, vector.z);
		if (onlyThisSide == null || onlyThisSide == Side.Left)
		{
			this.AttachmentPointLegLeft.transform.localPosition = vector;
			this.AttachmentPointLegLeft.transform.localRotation = Quaternion.identity;
		}
		if (onlyThisSide == null || onlyThisSide == Side.Right)
		{
			this.AttachmentPointLegRight.transform.localPosition = vector2;
			this.AttachmentPointLegRight.transform.localRotation = Quaternion.identity;
		}
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x000BC2EC File Offset: 0x000BA6EC
	public void ResetLegsPositionRotationToBodyOrUniversalDefault()
	{
		bool flag = false;
		bool flag2 = false;
		Thing componentInChildren = this.AttachmentPointHead.GetComponentInChildren<Thing>();
		if (componentInChildren != null)
		{
			string empty = string.Empty;
			if (Managers.thingManager.thingDefinitionCache.level1Cache.TryGetValue(componentInChildren.thingId, out empty) && !string.IsNullOrEmpty(empty))
			{
				global::SimpleJSON.JSONNode jsonnode = JSON.Parse(empty);
				global::SimpleJSON.JSONNode jsonnode2 = jsonnode["bod"];
				if (jsonnode2 != null)
				{
					global::SimpleJSON.JSONNode jsonnode3 = jsonnode2["ll"];
					if (jsonnode3 != null && jsonnode3["ap"] != null && jsonnode3["ar"] != null)
					{
						this.AttachmentPointLegLeft.transform.localPosition = JsonHelper.GetVector3(jsonnode3["ap"]);
						this.AttachmentPointLegLeft.transform.localEulerAngles = JsonHelper.GetVector3(jsonnode3["ar"]);
						flag = true;
					}
					global::SimpleJSON.JSONNode jsonnode4 = jsonnode2["lr"];
					if (jsonnode4 != null && jsonnode4["ap"] != null && jsonnode4["ar"] != null)
					{
						this.AttachmentPointLegRight.transform.localPosition = JsonHelper.GetVector3(jsonnode4["ap"]);
						this.AttachmentPointLegRight.transform.localEulerAngles = JsonHelper.GetVector3(jsonnode4["ar"]);
						flag2 = true;
					}
				}
			}
		}
		if (!flag)
		{
			this.ResetLegsPositionRotationToUniversalDefault(new Side?(Side.Left));
		}
		if (!flag2)
		{
			this.ResetLegsPositionRotationToUniversalDefault(new Side?(Side.Right));
		}
	}

	// Token: 0x04001268 RID: 4712
	public bool isOurPerson;

	// Token: 0x0400127D RID: 4733
	public GameObject Rig;

	// Token: 0x0400127F RID: 4735
	private GameObject LeftHand;

	// Token: 0x04001280 RID: 4736
	private GameObject RightHand;

	// Token: 0x04001281 RID: 4737
	public GameObject Torso;

	// Token: 0x04001282 RID: 4738
	public Transform leftHandSecondaryDot;

	// Token: 0x04001283 RID: 4739
	public Transform rightHandSecondaryDot;

	// Token: 0x04001285 RID: 4741
	private GameObject AttachmentPointHeadTop;

	// Token: 0x04001286 RID: 4742
	private GameObject AttachmentPointTorsoLower;

	// Token: 0x04001287 RID: 4743
	private GameObject AttachmentPointTorsoUpper;

	// Token: 0x04001288 RID: 4744
	public GameObject AttachmentPointLegLeft;

	// Token: 0x04001289 RID: 4745
	public GameObject AttachmentPointLegRight;

	// Token: 0x0400128A RID: 4746
	private GameObject AttachmentPointHandLeft;

	// Token: 0x0400128B RID: 4747
	private GameObject AttachmentPointHandRight;

	// Token: 0x0400128C RID: 4748
	private GameObject AttachmentPointArmLeft;

	// Token: 0x0400128D RID: 4749
	private GameObject AttachmentPointArmRight;

	// Token: 0x0400128E RID: 4750
	public bool isInvisibleWhereAllowed;

	// Token: 0x0400128F RID: 4751
	private GameObject nameTagWrapper;

	// Token: 0x04001290 RID: 4752
	private GameObject mainCamera;

	// Token: 0x04001291 RID: 4753
	private TextMesh nameTagTextMesh;

	// Token: 0x04001292 RID: 4754
	public Transform ridingBeacon;

	// Token: 0x04001293 RID: 4755
	public Vector3 ridingBeaconPositionOffset = Vector3.zero;

	// Token: 0x04001294 RID: 4756
	public Vector3 ridingBeaconRotationOffset = Vector3.zero;

	// Token: 0x04001295 RID: 4757
	public Vector3 ridingBeaconLastDistanceOptimizedPosition = Vector3.zero;

	// Token: 0x04001296 RID: 4758
	public RidingBeaconCache ridingBeaconCache;

	// Token: 0x04001297 RID: 4759
	private float timeWhenRotationAdaptsToRidingBeacon = -1f;

	// Token: 0x04001298 RID: 4760
	public Dictionary<AttachmentPointId, GameObject> AttachmentPointsById;

	// Token: 0x04001299 RID: 4761
	public PhotonView photonView;

	// Token: 0x0400129A RID: 4762
	public PhotonPlayer photonPlayer;

	// Token: 0x0400129B RID: 4763
	public bool Initialized;

	// Token: 0x0400129C RID: 4764
	public GameObject controlledControllable;

	// Token: 0x0400129D RID: 4765
	public TextMesh defaultTextChatReceiver;

	// Token: 0x0400129E RID: 4766
	public float smallestScaleObservedSoFar = 1f;

	// Token: 0x0400129F RID: 4767
	public float lastHandledInformedOthersOfStatesTime = -1f;

	// Token: 0x040012A0 RID: 4768
	public float lastHandledSaveMyBehaviorScriptVariables = -1f;

	// Token: 0x040012A1 RID: 4769
	public Dictionary<string, float> behaviorScriptVariables = new Dictionary<string, float>();

	// Token: 0x040012A2 RID: 4770
	public const string subThingIdSeparator = ";";

	// Token: 0x040012A3 RID: 4771
	public const string personIdSeparator = "~";

	// Token: 0x040012A5 RID: 4773
	private ParticleSystem speechIndicatorParticleSystem;

	// Token: 0x040012A6 RID: 4774
	public GlowTransition[] rings;

	// Token: 0x040012A7 RID: 4775
	private int behaviorScriptMessagesPerSecond;
}
