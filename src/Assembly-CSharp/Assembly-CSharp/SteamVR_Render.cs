using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using Valve.VR;

// Token: 0x020002A5 RID: 677
public class SteamVR_Render : MonoBehaviour
{
	// Token: 0x170002BD RID: 701
	// (get) Token: 0x0600196F RID: 6511 RVA: 0x000E84E7 File Offset: 0x000E68E7
	// (set) Token: 0x06001970 RID: 6512 RVA: 0x000E84EE File Offset: 0x000E68EE
	public static EVREye eye { get; private set; }

	// Token: 0x170002BE RID: 702
	// (get) Token: 0x06001971 RID: 6513 RVA: 0x000E84F8 File Offset: 0x000E68F8
	public static SteamVR_Render instance
	{
		get
		{
			if (SteamVR_Render._instance == null)
			{
				SteamVR_Render._instance = global::UnityEngine.Object.FindObjectOfType<SteamVR_Render>();
				if (SteamVR_Render._instance == null)
				{
					SteamVR_Render._instance = new GameObject("[SteamVR]").AddComponent<SteamVR_Render>();
				}
			}
			return SteamVR_Render._instance;
		}
	}

	// Token: 0x06001972 RID: 6514 RVA: 0x000E8548 File Offset: 0x000E6948
	private void OnDestroy()
	{
		SteamVR_Render._instance = null;
	}

	// Token: 0x06001973 RID: 6515 RVA: 0x000E8550 File Offset: 0x000E6950
	private void OnApplicationQuit()
	{
		SteamVR_Render.isQuitting = true;
		SteamVR.SafeDispose();
	}

	// Token: 0x06001974 RID: 6516 RVA: 0x000E855D File Offset: 0x000E695D
	public static void Add(SteamVR_Camera vrcam)
	{
		if (!SteamVR_Render.isQuitting)
		{
			SteamVR_Render.instance.AddInternal(vrcam);
		}
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x000E8574 File Offset: 0x000E6974
	public static void Remove(SteamVR_Camera vrcam)
	{
		if (!SteamVR_Render.isQuitting && SteamVR_Render._instance != null)
		{
			SteamVR_Render.instance.RemoveInternal(vrcam);
		}
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x000E859B File Offset: 0x000E699B
	public static SteamVR_Camera Top()
	{
		if (!SteamVR_Render.isQuitting)
		{
			return SteamVR_Render.instance.TopInternal();
		}
		return null;
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x000E85B4 File Offset: 0x000E69B4
	private void AddInternal(SteamVR_Camera vrcam)
	{
		Camera component = vrcam.GetComponent<Camera>();
		int num = this.cameras.Length;
		SteamVR_Camera[] array = new SteamVR_Camera[num + 1];
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			Camera component2 = this.cameras[i].GetComponent<Camera>();
			if (i == num2 && component2.depth > component.depth)
			{
				array[num2++] = vrcam;
			}
			array[num2++] = this.cameras[i];
		}
		if (num2 == num)
		{
			array[num2] = vrcam;
		}
		this.cameras = array;
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x000E8648 File Offset: 0x000E6A48
	private void RemoveInternal(SteamVR_Camera vrcam)
	{
		int num = this.cameras.Length;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			SteamVR_Camera steamVR_Camera = this.cameras[i];
			if (steamVR_Camera == vrcam)
			{
				num2++;
			}
		}
		if (num2 == 0)
		{
			return;
		}
		SteamVR_Camera[] array = new SteamVR_Camera[num - num2];
		int num3 = 0;
		for (int j = 0; j < num; j++)
		{
			SteamVR_Camera steamVR_Camera2 = this.cameras[j];
			if (steamVR_Camera2 != vrcam)
			{
				array[num3++] = steamVR_Camera2;
			}
		}
		this.cameras = array;
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x000E86E1 File Offset: 0x000E6AE1
	private SteamVR_Camera TopInternal()
	{
		if (this.cameras.Length > 0)
		{
			return this.cameras[this.cameras.Length - 1];
		}
		return null;
	}

	// Token: 0x170002BF RID: 703
	// (get) Token: 0x0600197A RID: 6522 RVA: 0x000E8704 File Offset: 0x000E6B04
	// (set) Token: 0x0600197B RID: 6523 RVA: 0x000E870C File Offset: 0x000E6B0C
	public static bool pauseRendering
	{
		get
		{
			return SteamVR_Render._pauseRendering;
		}
		set
		{
			SteamVR_Render._pauseRendering = value;
			CVRCompositor compositor = OpenVR.Compositor;
			if (compositor != null)
			{
				compositor.SuspendRendering(value);
			}
		}
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x000E8734 File Offset: 0x000E6B34
	private IEnumerator RenderLoop()
	{
		for (;;)
		{
			yield return new WaitForEndOfFrame();
			if (!SteamVR_Render.pauseRendering)
			{
				CVRCompositor compositor = OpenVR.Compositor;
				if (compositor != null)
				{
					if (!compositor.CanRenderScene())
					{
						continue;
					}
					compositor.SetTrackingSpace(this.trackingSpace);
				}
				SteamVR_Overlay overlay = SteamVR_Overlay.instance;
				if (overlay != null)
				{
					overlay.UpdateOverlay();
				}
				this.RenderExternalCamera();
			}
		}
		yield break;
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x000E8750 File Offset: 0x000E6B50
	private void RenderExternalCamera()
	{
		if (this.externalCamera == null)
		{
			return;
		}
		if (!this.externalCamera.gameObject.activeInHierarchy)
		{
			return;
		}
		int num = (int)Mathf.Max(this.externalCamera.config.frameSkip, 0f);
		if (Time.frameCount % (num + 1) != 0)
		{
			return;
		}
		this.externalCamera.AttachToCamera(this.TopInternal());
		this.externalCamera.RenderNear();
		this.externalCamera.RenderFar();
	}

	// Token: 0x0600197E RID: 6526 RVA: 0x000E87D8 File Offset: 0x000E6BD8
	private void OnInputFocus(params object[] args)
	{
		bool flag = (bool)args[0];
		if (flag)
		{
			if (this.pauseGameWhenDashboardIsVisible)
			{
				Time.timeScale = this.timeScale;
			}
			SteamVR_Camera.sceneResolutionScale = this.sceneResolutionScale;
		}
		else
		{
			if (this.pauseGameWhenDashboardIsVisible)
			{
				this.timeScale = Time.timeScale;
				Time.timeScale = 0f;
			}
			this.sceneResolutionScale = SteamVR_Camera.sceneResolutionScale;
			SteamVR_Camera.sceneResolutionScale = 0.5f;
		}
	}

	// Token: 0x0600197F RID: 6527 RVA: 0x000E884F File Offset: 0x000E6C4F
	private void OnQuit(params object[] args)
	{
		Application.Quit();
	}

	// Token: 0x06001980 RID: 6528 RVA: 0x000E8858 File Offset: 0x000E6C58
	private string GetScreenshotFilename(uint screenshotHandle, EVRScreenshotPropertyFilenames screenshotPropertyFilename)
	{
		EVRScreenshotError evrscreenshotError = EVRScreenshotError.None;
		uint screenshotPropertyFilename2 = OpenVR.Screenshots.GetScreenshotPropertyFilename(screenshotHandle, screenshotPropertyFilename, null, 0U, ref evrscreenshotError);
		if (evrscreenshotError != EVRScreenshotError.None && evrscreenshotError != EVRScreenshotError.BufferTooSmall)
		{
			return null;
		}
		if (screenshotPropertyFilename2 <= 1U)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder((int)screenshotPropertyFilename2);
		OpenVR.Screenshots.GetScreenshotPropertyFilename(screenshotHandle, screenshotPropertyFilename, stringBuilder, screenshotPropertyFilename2, ref evrscreenshotError);
		if (evrscreenshotError != EVRScreenshotError.None)
		{
			return null;
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06001981 RID: 6529 RVA: 0x000E88B8 File Offset: 0x000E6CB8
	private void OnRequestScreenshot(params object[] args)
	{
		VREvent_t vrevent_t = (VREvent_t)args[0];
		uint handle = vrevent_t.data.screenshot.handle;
		EVRScreenshotType type = (EVRScreenshotType)vrevent_t.data.screenshot.type;
		if (type == EVRScreenshotType.StereoPanorama)
		{
			string screenshotFilename = this.GetScreenshotFilename(handle, EVRScreenshotPropertyFilenames.Preview);
			string screenshotFilename2 = this.GetScreenshotFilename(handle, EVRScreenshotPropertyFilenames.VR);
			if (screenshotFilename == null || screenshotFilename2 == null)
			{
				return;
			}
			SteamVR_Utils.TakeStereoScreenshot(handle, new GameObject("screenshotPosition")
			{
				transform = 
				{
					position = SteamVR_Render.Top().transform.position,
					rotation = SteamVR_Render.Top().transform.rotation,
					localScale = SteamVR_Render.Top().transform.lossyScale
				}
			}, 32, 0.064f, ref screenshotFilename, ref screenshotFilename2);
			OpenVR.Screenshots.SubmitScreenshot(handle, type, screenshotFilename, screenshotFilename2);
		}
	}

	// Token: 0x06001982 RID: 6530 RVA: 0x000E899C File Offset: 0x000E6D9C
	private void OnEnable()
	{
		base.StartCoroutine("RenderLoop");
		SteamVR_Utils.Event.Listen("input_focus", new SteamVR_Utils.Event.Handler(this.OnInputFocus));
		SteamVR_Utils.Event.Listen("Quit", new SteamVR_Utils.Event.Handler(this.OnQuit));
		SteamVR_Utils.Event.Listen("RequestScreenshot", new SteamVR_Utils.Event.Handler(this.OnRequestScreenshot));
		if (SteamVR.instance == null)
		{
			base.enabled = false;
			return;
		}
		EVRScreenshotType[] array = new EVRScreenshotType[] { EVRScreenshotType.StereoPanorama };
		OpenVR.Screenshots.HookScreenshot(array);
	}

	// Token: 0x06001983 RID: 6531 RVA: 0x000E8A24 File Offset: 0x000E6E24
	private void OnDisable()
	{
		base.StopAllCoroutines();
		SteamVR_Utils.Event.Remove("RequestScreenshot", new SteamVR_Utils.Event.Handler(this.OnRequestScreenshot));
		SteamVR_Utils.Event.Remove("input_focus", new SteamVR_Utils.Event.Handler(this.OnInputFocus));
		SteamVR_Utils.Event.Remove("Quit", new SteamVR_Utils.Event.Handler(this.OnQuit));
	}

	// Token: 0x06001984 RID: 6532 RVA: 0x000E8A7C File Offset: 0x000E6E7C
	private void Awake()
	{
		if (this.externalCamera == null && File.Exists(this.externalCameraConfigPath))
		{
			GameObject gameObject = Resources.Load<GameObject>("SteamVR_ExternalCamera");
			GameObject gameObject2 = global::UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.gameObject.name = "External Camera";
			this.externalCamera = gameObject2.transform.GetChild(0).GetComponent<SteamVR_ExternalCamera>();
			this.externalCamera.configPath = this.externalCameraConfigPath;
			this.externalCamera.ReadConfig();
		}
	}

	// Token: 0x06001985 RID: 6533 RVA: 0x000E8B00 File Offset: 0x000E6F00
	private void Update()
	{
		if (this.poseUpdater == null)
		{
			this.poseUpdater = new GameObject("poseUpdater")
			{
				transform = 
				{
					parent = base.transform
				}
			}.AddComponent<SteamVR_UpdatePoses>();
		}
		SteamVR_Controller.Update();
		CVRSystem system = OpenVR.System;
		if (system != null)
		{
			VREvent_t vrevent_t = default(VREvent_t);
			uint num = (uint)Marshal.SizeOf(typeof(VREvent_t));
			for (int i = 0; i < 64; i++)
			{
				if (!system.PollNextEvent(ref vrevent_t, num))
				{
					break;
				}
				EVREventType eventType = (EVREventType)vrevent_t.eventType;
				if (eventType != EVREventType.VREvent_InputFocusCaptured)
				{
					if (eventType != EVREventType.VREvent_InputFocusReleased)
					{
						if (eventType != EVREventType.VREvent_HideRenderModels)
						{
							if (eventType != EVREventType.VREvent_ShowRenderModels)
							{
								string name = Enum.GetName(typeof(EVREventType), vrevent_t.eventType);
								if (name != null)
								{
									SteamVR_Utils.Event.Send(name.Substring(8), new object[] { vrevent_t });
								}
							}
							else
							{
								SteamVR_Utils.Event.Send("hide_render_models", new object[] { false });
							}
						}
						else
						{
							SteamVR_Utils.Event.Send("hide_render_models", new object[] { true });
						}
					}
					else if (vrevent_t.data.process.pid == 0U)
					{
						SteamVR_Utils.Event.Send("input_focus", new object[] { true });
					}
				}
				else if (vrevent_t.data.process.oldPid == 0U)
				{
					SteamVR_Utils.Event.Send("input_focus", new object[] { false });
				}
			}
		}
		Application.targetFrameRate = -1;
		Application.runInBackground = true;
		QualitySettings.maxQueuedFrames = -1;
		QualitySettings.vSyncCount = 0;
		if (this.lockPhysicsUpdateRateToRenderFrequency && Time.timeScale > 0f)
		{
			SteamVR instance = SteamVR.instance;
			if (instance != null)
			{
				Compositor_FrameTiming compositor_FrameTiming = default(Compositor_FrameTiming);
				compositor_FrameTiming.m_nSize = (uint)Marshal.SizeOf(typeof(Compositor_FrameTiming));
				instance.compositor.GetFrameTiming(ref compositor_FrameTiming, 0U);
				Time.fixedDeltaTime = Time.timeScale / instance.hmd_DisplayFrequency;
			}
		}
	}

	// Token: 0x040017C5 RID: 6085
	public bool pauseGameWhenDashboardIsVisible = true;

	// Token: 0x040017C6 RID: 6086
	public bool lockPhysicsUpdateRateToRenderFrequency = true;

	// Token: 0x040017C7 RID: 6087
	public SteamVR_ExternalCamera externalCamera;

	// Token: 0x040017C8 RID: 6088
	public string externalCameraConfigPath = "externalcamera.cfg";

	// Token: 0x040017C9 RID: 6089
	public ETrackingUniverseOrigin trackingSpace = ETrackingUniverseOrigin.TrackingUniverseStanding;

	// Token: 0x040017CB RID: 6091
	private static SteamVR_Render _instance;

	// Token: 0x040017CC RID: 6092
	private static bool isQuitting;

	// Token: 0x040017CD RID: 6093
	private SteamVR_Camera[] cameras = new SteamVR_Camera[0];

	// Token: 0x040017CE RID: 6094
	public TrackedDevicePose_t[] poses = new TrackedDevicePose_t[16];

	// Token: 0x040017CF RID: 6095
	public TrackedDevicePose_t[] gamePoses = new TrackedDevicePose_t[0];

	// Token: 0x040017D0 RID: 6096
	private static bool _pauseRendering;

	// Token: 0x040017D1 RID: 6097
	private float sceneResolutionScale = 1f;

	// Token: 0x040017D2 RID: 6098
	private float timeScale = 1f;

	// Token: 0x040017D3 RID: 6099
	private SteamVR_UpdatePoses poseUpdater;
}
