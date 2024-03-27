using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000204 RID: 516
public class VideoManager : MonoBehaviour, IGameManager
{
	// Token: 0x1700024C RID: 588
	// (get) Token: 0x060013F8 RID: 5112 RVA: 0x000B4410 File Offset: 0x000B2810
	// (set) Token: 0x060013F9 RID: 5113 RVA: 0x000B4418 File Offset: 0x000B2818
	public ManagerStatus status { get; private set; }

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x060013FA RID: 5114 RVA: 0x000B4421 File Offset: 0x000B2821
	// (set) Token: 0x060013FB RID: 5115 RVA: 0x000B4429 File Offset: 0x000B2829
	public string failMessage { get; private set; }

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x060013FC RID: 5116 RVA: 0x000B4432 File Offset: 0x000B2832
	// (set) Token: 0x060013FD RID: 5117 RVA: 0x000B443A File Offset: 0x000B283A
	public string currentVideoId { get; private set; }

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x060013FE RID: 5118 RVA: 0x000B4443 File Offset: 0x000B2843
	// (set) Token: 0x060013FF RID: 5119 RVA: 0x000B444B File Offset: 0x000B284B
	public string currentVideoTitle { get; private set; }

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06001400 RID: 5120 RVA: 0x000B4454 File Offset: 0x000B2854
	// (set) Token: 0x06001401 RID: 5121 RVA: 0x000B445C File Offset: 0x000B285C
	public string currentVideoStartedByPersonId { get; private set; }

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06001402 RID: 5122 RVA: 0x000B4465 File Offset: 0x000B2865
	// (set) Token: 0x06001403 RID: 5123 RVA: 0x000B446D File Offset: 0x000B286D
	public string currentVideoStartedByPersonName { get; private set; }

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06001404 RID: 5124 RVA: 0x000B4476 File Offset: 0x000B2876
	// (set) Token: 0x06001405 RID: 5125 RVA: 0x000B447E File Offset: 0x000B287E
	public float volume { get; private set; }

	// Token: 0x06001406 RID: 5126 RVA: 0x000B4487 File Offset: 0x000B2887
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.volume = 1f;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001407 RID: 5127 RVA: 0x000B44A2 File Offset: 0x000B28A2
	public bool IsCurrentlyShowingSomething()
	{
		return this.currentVideoScreen != null;
	}

	// Token: 0x06001408 RID: 5128 RVA: 0x000B44B0 File Offset: 0x000B28B0
	public bool WePlayVideoAtNearestThingPartScreen(string youTubeVideoOrCameraId, string videoTitle, string videoThingName = null, Transform centerTransform = null)
	{
		GameObject nearestThingWithThingPartScreen = this.GetNearestThingWithThingPartScreen(videoThingName, centerTransform);
		if (nearestThingWithThingPartScreen != null)
		{
			Managers.personManager.DoPlayVideo(nearestThingWithThingPartScreen, youTubeVideoOrCameraId, videoTitle);
		}
		return nearestThingWithThingPartScreen != null;
	}

	// Token: 0x06001409 RID: 5129 RVA: 0x000B44E8 File Offset: 0x000B28E8
	private GameObject GetNearestThingWithThingPartScreen(string videoThingName = null, Transform centerTransform = null)
	{
		if (centerTransform == null)
		{
			centerTransform = Managers.treeManager.GetObject("/OurPersonRig/HeadCore").transform;
		}
		ThingPart[] array = global::UnityEngine.Object.FindObjectsOfType(typeof(ThingPart)) as ThingPart[];
		array = array.OrderBy((ThingPart x) => Vector3.Distance(centerTransform.position, x.transform.position)).ToArray<ThingPart>();
		foreach (ThingPart thingPart in array)
		{
			if (thingPart.offersScreen)
			{
				Thing parentThing = thingPart.GetParentThing();
				if (parentThing != null && !parentThing.isInInventoryOrDialog && parentThing.name != Universe.objectNameIfAlreadyDestroyed && (string.IsNullOrEmpty(videoThingName) || parentThing.gameObject.name == videoThingName))
				{
					return parentThing.gameObject;
				}
			}
		}
		return null;
	}

	// Token: 0x0600140A RID: 5130 RVA: 0x000B45E8 File Offset: 0x000B29E8
	public void PlayVideoAtThing(GameObject thingObject, string videoId, string title, string startedByPersonId, string startedByPersonName, int startSeconds = 0, bool ignoreVideoCacheUpdating = false)
	{
		if (this.currentVideoScreen != null)
		{
			Misc.Destroy(this.currentVideoScreen);
		}
		this.currentVideoId = videoId;
		this.currentVideoTitle = title;
		this.currentVideoStartedByPersonId = startedByPersonId;
		this.currentVideoStartedByPersonName = startedByPersonName;
		Component[] componentsInChildren = thingObject.GetComponentsInChildren<ThingPart>();
		foreach (ThingPart thingPart in componentsInChildren)
		{
			if (thingPart.offersScreen)
			{
				this.currentVideoScreen = this.AddVideoScreenMesh(thingPart.gameObject, this.currentVideoId, thingPart.videoScreenHasSurroundSound, startSeconds, thingPart.videoScreenLoops, thingPart.videoScreenIsDirectlyOnMesh);
				thingPart.videoScreen = this.currentVideoScreen;
				this.UpdateVolume();
				if (!ignoreVideoCacheUpdating)
				{
					this.UpdateRoomVideoInfoCache(false);
				}
				if (thingPart.browser != null)
				{
					global::UnityEngine.Object.Destroy(thingPart.browser.gameObject);
				}
				break;
			}
		}
	}

	// Token: 0x0600140B RID: 5131 RVA: 0x000B46D4 File Offset: 0x000B2AD4
	public void StopVideo(bool weAreAuthority = false)
	{
		if (this.currentVideoScreen != null)
		{
			Transform parent = this.currentVideoScreen.transform.parent;
			if (parent != null)
			{
				Renderer component = parent.GetComponent<Renderer>();
				if (component != null)
				{
					component.enabled = true;
				}
			}
			Misc.Destroy(this.currentVideoScreen);
		}
		if (this.VideoIsOfCamera(this.currentVideoId))
		{
			Managers.cameraManager.StopStreamToDesktop();
		}
		this.currentVideoId = null;
		this.currentVideoTitle = null;
		this.currentVideoStartedByPersonId = null;
		this.currentVideoStartedByPersonName = null;
		this.hasSurroundSound = false;
		if (weAreAuthority)
		{
			Managers.personManager.DoStopVideo();
			this.UpdateRoomVideoInfoCache(false);
		}
		Misc.DeleteCookiesFileIfExists();
	}

	// Token: 0x0600140C RID: 5132 RVA: 0x000B4790 File Offset: 0x000B2B90
	public GameObject GetCurrentScreenThingObject(GameObject thingObjectToCompare)
	{
		GameObject gameObject = null;
		if (this.currentVideoScreen != null)
		{
			Transform transform = this.currentVideoScreen.transform;
			if (transform.parent != null && transform.parent.parent != null && transform.parent.parent.gameObject == thingObjectToCompare)
			{
				gameObject = thingObjectToCompare;
			}
		}
		return gameObject;
	}

	// Token: 0x0600140D RID: 5133 RVA: 0x000B4804 File Offset: 0x000B2C04
	public ThingPart GetCurrentScreenThingPart()
	{
		ThingPart thingPart = null;
		if (this.currentVideoScreen != null)
		{
			thingPart = this.currentVideoScreen.transform.parent.GetComponent<ThingPart>();
		}
		return thingPart;
	}

	// Token: 0x0600140E RID: 5134 RVA: 0x000B483B File Offset: 0x000B2C3B
	private bool VideoIsOfCamera(string videoId)
	{
		return videoId != null && videoId.IndexOf("camera_") == 0;
	}

	// Token: 0x0600140F RID: 5135 RVA: 0x000B4854 File Offset: 0x000B2C54
	public void LoadSearchResults()
	{
		base.StartCoroutine(this.DoLoadSearchResults());
	}

	// Token: 0x06001410 RID: 5136 RVA: 0x000B4864 File Offset: 0x000B2C64
	public IEnumerator DoLoadSearchResults()
	{
		string url = "http://www.youtube.com/results?search_query=" + WWW.EscapeURL(this.searchText) + "&hl=en";
		WWW www = new WWW(url);
		yield return www;
		if (string.IsNullOrEmpty(www.error))
		{
			this.ParseSearchResultHtmlIntoDictionary(www.text);
			Hand ahandOfOurs = Misc.GetAHandOfOurs();
			ahandOfOurs.SwitchToNewDialog(DialogType.VideoControl, string.Empty);
		}
		else
		{
			Log.Debug("DoLoadSearchResults results error: " + www.error);
		}
		yield break;
	}

	// Token: 0x06001411 RID: 5137 RVA: 0x000B4880 File Offset: 0x000B2C80
	private void ParseSearchResultHtmlIntoDictionary(string html)
	{
		this.searchResultIdTitles = new Dictionary<string, string>();
		List<string> textsBetween = Misc.GetTextsBetween(html, "\"watchEndpoint\":{\"videoId\":\"", "\"");
		List<string> textsBetween2 = Misc.GetTextsBetween(html, "\"title\":{\"runs\":[{\"text\":\"", "\"");
		int num = 0;
		while (num < textsBetween.Count && num < 100)
		{
			string text = "Unnamed";
			string text2 = textsBetween[num];
			if (textsBetween2.Count > num)
			{
				text = textsBetween2[num];
			}
			text = Misc.HtmlDecode(text);
			if (!this.searchResultIdTitles.ContainsKey(text2))
			{
				this.searchResultIdTitles.Add(text2, text);
			}
			num++;
		}
	}

	// Token: 0x06001412 RID: 5138 RVA: 0x000B4924 File Offset: 0x000B2D24
	public GameObject AddVideoScreenMesh(GameObject parentObject, string youTubeVideoOrCameraId, bool _hasSurroundSound = false, int startSeconds = 0, bool loop = false, bool videoScreenIsDirectlyOnMesh = false)
	{
		if (this.VideoIsOfCamera(youTubeVideoOrCameraId))
		{
			return this.AddVideoScreenCameraStreamMesh(parentObject, youTubeVideoOrCameraId, videoScreenIsDirectlyOnMesh);
		}
		Transform transform = parentObject.transform;
		this.hasSurroundSound = _hasSurroundSound;
		string text = "VideoQuad";
		bool flag = Misc.GetLargestValueOfVector(transform.localScale) >= 5f;
		if (this.hasSurroundSound)
		{
			text = "SurroundSoundVideoQuad";
		}
		else if (flag)
		{
			text = "WideAudioReachVideoQuad";
		}
		text = "Prefabs/VideoPlayers/" + text;
		GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load(text, typeof(GameObject))) as GameObject;
		gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
		gameObject.transform.parent = transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.Rotate(0f, 180f, 0f);
		float num = ((!flag) ? 0.0025f : 0.013f);
		gameObject.transform.Translate(Vector3.up * (transform.localScale.y / 2f + num));
		this.ResizeScreenProportionally(gameObject.transform);
		ThingPart component = parentObject.GetComponent<ThingPart>();
		if (component != null && component.videoScreenFlipsX)
		{
			Vector3 localScale = gameObject.transform.localScale;
			localScale.x *= -1f;
			gameObject.transform.localScale = localScale;
		}
		this.videoPlayer = gameObject.GetComponent<YoutubePlayer>();
		this.videoPlayer.videoControllerCanvas = gameObject;
		this.videoPlayer.youtubeUrl = "https://www.youtube.com/watch?v=" + youTubeVideoOrCameraId;
		startSeconds = 0;
		if (startSeconds > 0)
		{
			this.videoPlayer.startFromSecond = true;
			this.videoPlayer.startFromSecondTime = startSeconds;
		}
		else
		{
			this.videoPlayer.startFromSecond = false;
			this.videoPlayer.startFromSecondTime = 0;
		}
		if (this.videoPlayer.videoPlayer != null)
		{
			this.videoPlayer.videoPlayer.isLooping = loop;
		}
		this.videoPlayer.videoQuality = YoutubePlayer.YoutubeVideoQuality.HD;
		this.videoPlayer.autoPlayOnStart = true;
		if (videoScreenIsDirectlyOnMesh)
		{
			this.UseParentMeshForScreen(gameObject.transform, component);
		}
		return gameObject;
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x000B4B78 File Offset: 0x000B2F78
	private void UseParentMeshForScreen(Transform screen, ThingPart thingPart)
	{
		Transform parent = screen.parent;
		if (parent != null && thingPart != null)
		{
			MeshFilter component = screen.GetComponent<MeshFilter>();
			MeshFilter component2 = parent.GetComponent<MeshFilter>();
			Renderer component3 = screen.GetComponent<Renderer>();
			Renderer component4 = parent.GetComponent<Renderer>();
			if (component != null && component2 != null && component3 != null && component4 != null)
			{
				screen.position = parent.position;
				screen.rotation = parent.rotation;
				screen.localScale = Vector3.one;
				component.sharedMesh = component2.sharedMesh;
				if (thingPart.materialType != MaterialTypes.Glow)
				{
					component3.sharedMaterials = component4.sharedMaterials;
				}
				component4.enabled = false;
			}
		}
	}

	// Token: 0x06001414 RID: 5140 RVA: 0x000B4C44 File Offset: 0x000B3044
	private GameObject AddVideoScreenCameraStreamMesh(GameObject parentObject, string cameraId, bool videoScreenIsDirectlyOnMesh = false)
	{
		this.AddCameraComponentIfNoneForRemoteReceive(cameraId);
		Transform transform = parentObject.transform;
		string text = "Prefabs/CameraStreamQuad";
		GameObject gameObject = global::UnityEngine.Object.Instantiate(Resources.Load(text, typeof(GameObject))) as GameObject;
		gameObject.name = Misc.RemoveCloneFromName(gameObject.name);
		gameObject.transform.parent = transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.Rotate(90f, 0f, 180f);
		bool flag = Misc.GetLargestValueOfVector(transform.localScale) >= 5f;
		float num = ((!flag) ? 0.0025f : 0.013f);
		gameObject.transform.Translate(Vector3.forward * -(transform.localScale.y / 2f + num));
		this.ResizeScreenProportionallyForCamera(gameObject.transform);
		ThingPart component = parentObject.GetComponent<ThingPart>();
		if (component != null && component.videoScreenFlipsX)
		{
			Vector3 localScale = gameObject.transform.localScale;
			localScale.x *= -1f;
			gameObject.transform.localScale = localScale;
		}
		return gameObject;
	}

	// Token: 0x06001415 RID: 5141 RVA: 0x000B4D90 File Offset: 0x000B3190
	private void AddCameraComponentIfNoneForRemoteReceive(string cameraId)
	{
		ThingPart thingPart = null;
		string[] array = new string[] { "_" };
		string[] array2 = cameraId.Split(array, StringSplitOptions.RemoveEmptyEntries);
		if (array2.Length == 3)
		{
			string text = array2[1];
			string text2 = array2[2];
			if (text != null)
			{
				if (!(text == "placement"))
				{
					if (text == "person")
					{
						Person personById = Managers.personManager.GetPersonById(text2);
						if (personById != null)
						{
							GameObject handByTopographyId = personById.GetHandByTopographyId(TopographyId.Left);
							GameObject thingInHand = personById.GetThingInHand(handByTopographyId, false);
							thingPart = this.GetCameraThingPart(thingInHand);
							if (thingPart == null)
							{
								GameObject handByTopographyId2 = personById.GetHandByTopographyId(TopographyId.Right);
								GameObject thingInHand2 = personById.GetThingInHand(handByTopographyId2, false);
								thingPart = this.GetCameraThingPart(thingInHand2);
							}
						}
					}
				}
				else
				{
					GameObject placementById = Managers.thingManager.GetPlacementById(text2, false);
					if (placementById != null)
					{
						thingPart = this.GetCameraThingPart(placementById);
					}
				}
			}
		}
		GameObject gameObject = ((!(thingPart != null)) ? null : thingPart.gameObject);
		if (gameObject == null)
		{
			gameObject = Managers.cameraManager.GetNearestCameraThingPart();
		}
		Managers.cameraManager.AddCameraComponentIfNoneForRemoteReceive(gameObject);
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x000B4ECC File Offset: 0x000B32CC
	private ThingPart GetCameraThingPart(GameObject thing)
	{
		ThingPart thingPart = null;
		if (thing != null)
		{
			Component[] componentsInChildren = thing.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart2 in componentsInChildren)
			{
				if (thingPart2.isCamera)
				{
					thingPart = thingPart2;
					break;
				}
			}
		}
		return thingPart;
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x000B4F28 File Offset: 0x000B3328
	private void ResizeScreenProportionally(Transform screen)
	{
		Transform parent = screen.transform.parent;
		screen.parent = null;
		float x = parent.localScale.x;
		float z = parent.localScale.z;
		float num = x / 1f;
		float num2 = z / 0.5625f;
		float num3 = Math.Min(num, num2);
		float num4 = 0.1f * num3;
		float num5 = 0.056250002f * num3;
		screen.localScale = new Vector3(num4, 1f, num5);
		screen.parent = parent;
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x000B4FB4 File Offset: 0x000B33B4
	private void ResizeScreenProportionallyForCamera(Transform screen)
	{
		Transform parent = screen.transform.parent;
		screen.parent = null;
		float x = parent.localScale.x;
		float z = parent.localScale.z;
		float num = x / 1f;
		float num2 = z / 0.75f;
		float num3 = Math.Min(num, num2);
		float num4 = 1f * num3;
		float num5 = 0.75f * num3;
		screen.localScale = new Vector3(-num4, -num5, 1f);
		screen.parent = parent;
	}

	// Token: 0x06001419 RID: 5145 RVA: 0x000B5041 File Offset: 0x000B3441
	public void SetVolume(float thisVolume)
	{
		if (this.volume >= 0f && this.volume <= 3f)
		{
			this.volume = thisVolume;
			this.UpdateVolume();
			this.UpdateRoomVideoInfoCache(true);
		}
	}

	// Token: 0x0600141A RID: 5146 RVA: 0x000B5078 File Offset: 0x000B3478
	public void UpdateVolume()
	{
		if (this.currentVideoScreen != null)
		{
			AudioSource component = this.currentVideoScreen.GetComponent<AudioSource>();
			if (component != null)
			{
				float num = ((!this.hasSurroundSound) ? 0.4f : 0.6f);
				component.volume = this.volume * num * this.personalVolumeFactor;
			}
		}
	}

	// Token: 0x0600141B RID: 5147 RVA: 0x000B50E0 File Offset: 0x000B34E0
	public string GetVolumeButtonIcon(float thisVolume)
	{
		int num = Array.IndexOf<float>(this.availableButtonVolumes, thisVolume);
		if (num == -1)
		{
			num = 1;
		}
		return "AudioVolumes/" + num;
	}

	// Token: 0x0600141C RID: 5148 RVA: 0x000B5114 File Offset: 0x000B3514
	public float GetToggleVolumeViaButton(float thisVolume)
	{
		int num = Array.IndexOf<float>(this.availableButtonVolumes, thisVolume);
		if (num == -1)
		{
			num = 1;
		}
		num--;
		if (num < 0)
		{
			num = this.availableButtonVolumes.Length - 1;
		}
		return this.availableButtonVolumes[num];
	}

	// Token: 0x0600141D RID: 5149 RVA: 0x000B5158 File Offset: 0x000B3558
	public void InitializeFromRoomVideoInfo()
	{
		if (Managers.areaManager.GetAVideoAutoPlayingThingPart(null) == null)
		{
			AreaVideoInfo roomVideoInfoCache = this.GetRoomVideoInfoCache();
			if (roomVideoInfoCache != null && !string.IsNullOrEmpty(roomVideoInfoCache.urlId) && !string.IsNullOrEmpty(roomVideoInfoCache.videoScreenThingPlacementId) && Managers.thingManager != null)
			{
				GameObject placementById = Managers.thingManager.GetPlacementById(roomVideoInfoCache.videoScreenThingPlacementId, false);
				if (placementById != null)
				{
					int num = (int)Math.Round(PhotonNetwork.time - roomVideoInfoCache.photonTime);
					if (num < 0)
					{
						num = 0;
					}
					this.volume = roomVideoInfoCache.volume;
					this.PlayVideoAtThing(placementById, roomVideoInfoCache.urlId, roomVideoInfoCache.title, roomVideoInfoCache.startedByPersonId, roomVideoInfoCache.startedByPersonName, num, true);
				}
			}
		}
	}

	// Token: 0x0600141E RID: 5150 RVA: 0x000B5220 File Offset: 0x000B3620
	private void UpdateRoomVideoInfoCache(bool useOldPhotonTime = false)
	{
		string text = null;
		int num = global::UnityEngine.Random.Range(1, 10000);
		if (this.currentVideoScreen != null && this.currentVideoScreen.transform.parent != null && this.currentVideoScreen.transform.parent.parent != null)
		{
			Thing component = this.currentVideoScreen.transform.parent.parent.GetComponent<Thing>();
			if (component != null)
			{
				double num2 = PhotonNetwork.time;
				if (useOldPhotonTime)
				{
					AreaVideoInfo roomVideoInfoCache = this.GetRoomVideoInfoCache();
					if (roomVideoInfoCache != null)
					{
						num2 = roomVideoInfoCache.photonTime;
					}
				}
				AreaVideoInfo areaVideoInfo = new AreaVideoInfo(this.currentVideoId, component.placementId, this.currentVideoTitle, this.currentVideoStartedByPersonId, this.currentVideoStartedByPersonName, this.volume, PhotonNetwork.time);
				text = JsonUtility.ToJson(areaVideoInfo);
			}
		}
		Managers.broadcastNetworkManager.UpdatePhotonCustomRoomProperty("vid", text);
		AreaVideoInfo roomVideoInfoCache2 = this.GetRoomVideoInfoCache();
	}

	// Token: 0x0600141F RID: 5151 RVA: 0x000B5324 File Offset: 0x000B3724
	private AreaVideoInfo GetRoomVideoInfoCache()
	{
		AreaVideoInfo areaVideoInfo = null;
		string photonCustomRoomProperty = Managers.broadcastNetworkManager.GetPhotonCustomRoomProperty("vid");
		if (!string.IsNullOrEmpty(photonCustomRoomProperty))
		{
			areaVideoInfo = JsonUtility.FromJson<AreaVideoInfo>(photonCustomRoomProperty);
		}
		return areaVideoInfo;
	}

	// Token: 0x06001420 RID: 5152 RVA: 0x000B5356 File Offset: 0x000B3756
	public void SetNewVideoScreenParent(Transform newParent)
	{
		this.currentVideoScreen.transform.parent = newParent;
	}

	// Token: 0x04001218 RID: 4632
	private GameObject currentVideoScreen;

	// Token: 0x0400121D RID: 4637
	private float[] availableButtonVolumes = new float[] { 0f, 0.05f, 0.15f, 0.4f, 1f, 3f };

	// Token: 0x0400121E RID: 4638
	public const float defaultVolume = 1f;

	// Token: 0x0400121F RID: 4639
	public float personalVolumeFactor = 1f;

	// Token: 0x04001220 RID: 4640
	private bool useTestVideo;

	// Token: 0x04001221 RID: 4641
	public string searchText;

	// Token: 0x04001222 RID: 4642
	public Dictionary<string, string> searchResultIdTitles = new Dictionary<string, string>();

	// Token: 0x04001224 RID: 4644
	private bool hasSurroundSound;

	// Token: 0x04001225 RID: 4645
	private const string roomVideoInfoCacheKey = "vid";

	// Token: 0x04001226 RID: 4646
	private YoutubePlayer videoPlayer;
}
