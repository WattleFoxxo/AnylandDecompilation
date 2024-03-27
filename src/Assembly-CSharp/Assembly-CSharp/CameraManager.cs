using System;
using System.Linq;
using UnityEngine;

// Token: 0x020001E6 RID: 486
public class CameraManager : MonoBehaviour, IGameManager
{
	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x06001040 RID: 4160 RVA: 0x0008B864 File Offset: 0x00089C64
	// (set) Token: 0x06001041 RID: 4161 RVA: 0x0008B86C File Offset: 0x00089C6C
	public ManagerStatus status { get; private set; }

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x06001042 RID: 4162 RVA: 0x0008B875 File Offset: 0x00089C75
	// (set) Token: 0x06001043 RID: 4163 RVA: 0x0008B87D File Offset: 0x00089C7D
	public string failMessage { get; private set; }

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x06001044 RID: 4164 RVA: 0x0008B886 File Offset: 0x00089C86
	// (set) Token: 0x06001045 RID: 4165 RVA: 0x0008B88E File Offset: 0x00089C8E
	public bool isStreamingToDesktop { get; private set; }

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x06001046 RID: 4166 RVA: 0x0008B897 File Offset: 0x00089C97
	// (set) Token: 0x06001047 RID: 4167 RVA: 0x0008B89F File Offset: 0x00089C9F
	public bool isStreamingToVideoScreen { get; private set; }

	// Token: 0x06001048 RID: 4168 RVA: 0x0008B8A8 File Offset: 0x00089CA8
	public void Startup()
	{
		this.status = ManagerStatus.Initializing;
		this.status = ManagerStatus.Started;
	}

	// Token: 0x06001049 RID: 4169 RVA: 0x0008B8B8 File Offset: 0x00089CB8
	private void CheckCullingMaskValue()
	{
	}

	// Token: 0x0600104A RID: 4170 RVA: 0x0008B8BA File Offset: 0x00089CBA
	private void Update()
	{
		if (this.isStreamingToDesktop && CrossDevice.desktopMode && Input.GetKeyDown(KeyCode.Escape))
		{
			this.StopStreamToDesktop();
		}
	}

	// Token: 0x0600104B RID: 4171 RVA: 0x0008B8E4 File Offset: 0x00089CE4
	public GameObject GetNearestCameraThingPart()
	{
		GameObject @object = Managers.treeManager.GetObject("/OurPersonRig");
		Transform ourTransform = @object.transform;
		GameObject[] array = Misc.GetChildrenAsArray(Managers.thingManager.placements.transform);
		array = array.OrderBy((GameObject x) => Vector3.Distance(ourTransform.position, x.transform.position)).ToArray<GameObject>();
		foreach (GameObject gameObject in array)
		{
			Component[] componentsInChildren = gameObject.GetComponentsInChildren<ThingPart>();
			foreach (ThingPart thingPart in componentsInChildren)
			{
				if (thingPart.isCamera)
				{
					return thingPart.gameObject;
				}
			}
		}
		return null;
	}

	// Token: 0x0600104C RID: 4172 RVA: 0x0008B9A8 File Offset: 0x00089DA8
	public void StartStreamToVideoScreen(GameObject cameraThingPart, string thingNameMustContain = null)
	{
		this.StopStreamToDesktop();
		this.StopStreamToVideoScreen();
		this.isStreamingToVideoScreen = true;
		this.isStreamingToDesktop = false;
		this.AddCameraComponent(cameraThingPart, false);
		string cameraCrossClientId = this.GetCameraCrossClientId(cameraThingPart);
		if (cameraCrossClientId != string.Empty)
		{
			GameObject thingObjectMatchInSameThing = this.GetThingObjectMatchInSameThing(cameraThingPart, thingNameMustContain);
			if (thingObjectMatchInSameThing != null)
			{
				Managers.personManager.DoPlayVideo(thingObjectMatchInSameThing, cameraCrossClientId, "camera");
			}
			else if (!Managers.videoManager.WePlayVideoAtNearestThingPartScreen(cameraCrossClientId, "camera", thingNameMustContain, null))
			{
				Managers.soundManager.Play("no", base.transform, 0.2f, false, false);
			}
		}
	}

	// Token: 0x0600104D RID: 4173 RVA: 0x0008BA50 File Offset: 0x00089E50
	private GameObject GetThingObjectMatchInSameThing(GameObject cameraThingPart, string videoThingName)
	{
		GameObject gameObject = null;
		ThingPart component = cameraThingPart.GetComponent<ThingPart>();
		if (component != null)
		{
			Thing parentThing = component.GetParentThing();
			if (parentThing != null && !parentThing.isInInventoryOrDialog && (string.IsNullOrEmpty(videoThingName) || parentThing.gameObject.name == videoThingName))
			{
				Component[] componentsInChildren = parentThing.gameObject.GetComponentsInChildren<ThingPart>();
				foreach (ThingPart thingPart in componentsInChildren)
				{
					if (thingPart.offersScreen)
					{
						return parentThing.gameObject;
					}
				}
			}
		}
		return gameObject;
	}

	// Token: 0x0600104E RID: 4174 RVA: 0x0008BAF9 File Offset: 0x00089EF9
	public void AddCameraComponentIfNoneForRemoteReceive(GameObject thingPart)
	{
		if (this.currentCameraComponent == null && thingPart != null)
		{
			this.isStreamingToVideoScreen = true;
			this.isStreamingToDesktop = false;
			this.AddCameraComponent(thingPart, false);
		}
	}

	// Token: 0x0600104F RID: 4175 RVA: 0x0008BB2E File Offset: 0x00089F2E
	public void StopStreamToVideoScreen()
	{
		Managers.videoManager.StopVideo(true);
		this.RemoveCameraComponent();
		this.isStreamingToVideoScreen = false;
		this.isStreamingToDesktop = false;
	}

	// Token: 0x06001050 RID: 4176 RVA: 0x0008BB4F File Offset: 0x00089F4F
	public void StopAllStreaming()
	{
		if (this.isStreamingToVideoScreen)
		{
			this.StopStreamToVideoScreen();
		}
		else if (this.isStreamingToDesktop)
		{
			this.StopStreamToDesktop();
		}
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x0008BB78 File Offset: 0x00089F78
	public void StartStreamToDesktop(GameObject cameraThingPart)
	{
		this.StopStreamToDesktop();
		this.StopStreamToVideoScreen();
		this.isStreamingToVideoScreen = false;
		this.isStreamingToDesktop = true;
		this.AddCameraComponent(cameraThingPart, true);
		if (this.currentCameraComponent != null)
		{
			this.currentCameraComponent.cullingMask = ((!CameraManager.weAreInvisibleToDesktopCameraStream) ? (-2049) : (-18689));
		}
	}

	// Token: 0x06001052 RID: 4178 RVA: 0x0008BBDC File Offset: 0x00089FDC
	public void StopStreamToDesktop()
	{
		this.RemoveCameraComponent();
		this.isStreamingToVideoScreen = false;
		this.isStreamingToDesktop = false;
	}

	// Token: 0x06001053 RID: 4179 RVA: 0x0008BBF4 File Offset: 0x00089FF4
	private void AddCameraComponent(GameObject cameraThingPart, bool isForDesktop)
	{
		this.RemoveCameraComponent();
		Camera componentInChildren = cameraThingPart.GetComponentInChildren<Camera>();
		if (componentInChildren == null)
		{
			GameObject gameObject = global::UnityEngine.Object.Instantiate<GameObject>(this.customCameraPrefab);
			gameObject.transform.parent = cameraThingPart.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			Misc.RemoveCloneFromName(gameObject);
			this.currentCameraComponent = gameObject.GetComponent<Camera>();
			ThingPart component = cameraThingPart.GetComponent<ThingPart>();
			if (component != null && component.isFishEyeCamera)
			{
				this.currentCameraComponent.fieldOfView = 120f;
			}
			else
			{
				this.currentCameraComponent.fieldOfView = 60f;
			}
			if (isForDesktop)
			{
				this.currentCameraComponent.stereoTargetEye = StereoTargetEyeMask.None;
				this.currentCameraComponent.aspect = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
			}
			else
			{
				this.currentCameraComponent.targetTexture = this.renderTexture;
				this.currentCameraComponent.aspect = 1.3333334f;
			}
		}
	}

	// Token: 0x06001054 RID: 4180 RVA: 0x0008BD10 File Offset: 0x0008A110
	private void RemoveCameraComponent()
	{
		if (this.currentCameraComponent != null)
		{
			try
			{
				global::UnityEngine.Object.DestroyImmediate(this.currentCameraComponent.gameObject);
			}
			catch (Exception ex)
			{
				Log.Debug("Using non-immediate destroy");
				global::UnityEngine.Object.Destroy(this.currentCameraComponent.gameObject);
			}
			this.currentCameraComponent = null;
		}
	}

	// Token: 0x06001055 RID: 4181 RVA: 0x0008BD7C File Offset: 0x0008A17C
	public GameObject GetCurrentCameraThingPart()
	{
		return (!(this.currentCameraComponent != null)) ? null : this.currentCameraComponent.gameObject;
	}

	// Token: 0x06001056 RID: 4182 RVA: 0x0008BDA0 File Offset: 0x0008A1A0
	private string GetCameraCrossClientId(GameObject cameraThingPart)
	{
		string text = string.Empty;
		Thing component = cameraThingPart.transform.parent.GetComponent<Thing>();
		if (component != null)
		{
			Person personThisObjectIsOf = Managers.personManager.GetPersonThisObjectIsOf(component.gameObject);
			if (personThisObjectIsOf != null)
			{
				text = text + "camera_person_" + personThisObjectIsOf.userId;
			}
			else if (component.placementId != null)
			{
				text = text + "camera_placement_" + component.placementId;
			}
		}
		return text;
	}

	// Token: 0x04001042 RID: 4162
	public Camera currentCameraComponent;

	// Token: 0x04001045 RID: 4165
	public RenderTexture renderTexture;

	// Token: 0x04001046 RID: 4166
	public const int cullingMask_Everything = -1;

	// Token: 0x04001047 RID: 4167
	public const int cullingMask_EverythingExceptInvisibleToOurPerson = -257;

	// Token: 0x04001048 RID: 4168
	public const int cullingMask_EverythingExceptInvisibleToOurPersonAndDesktopCamera = -2305;

	// Token: 0x04001049 RID: 4169
	public const int cullingMask_EverythingExceptInvisibleToOurPersonAndDesktopCameraAndDialog = -18689;

	// Token: 0x0400104A RID: 4170
	public const int cullingMask_EverythingExceptInvisibleToDesktopCamera = -2049;

	// Token: 0x0400104B RID: 4171
	public static bool weAreInvisibleToDesktopCameraStream;

	// Token: 0x0400104C RID: 4172
	public GameObject customCameraPrefab;
}
