using System;
using System.Text;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

// Token: 0x0200028F RID: 655
public class SteamVR : IDisposable
{
	// Token: 0x060018AA RID: 6314 RVA: 0x000E2010 File Offset: 0x000E0410
	private SteamVR()
	{
		this.hmd = OpenVR.System;
		this.compositor = OpenVR.Compositor;
		this.overlay = OpenVR.Overlay;
		uint num = 0U;
		uint num2 = 0U;
		this.hmd.GetRecommendedRenderTargetSize(ref num, ref num2);
		this.sceneWidth = num;
		this.sceneHeight = num2;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		this.hmd.GetProjectionRaw(EVREye.Eye_Left, ref num3, ref num4, ref num5, ref num6);
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		this.hmd.GetProjectionRaw(EVREye.Eye_Right, ref num7, ref num8, ref num9, ref num10);
		this.tanHalfFov = new Vector2(Mathf.Max(new float[]
		{
			-num3,
			num4,
			-num7,
			num8
		}), Mathf.Max(new float[]
		{
			-num5,
			num6,
			-num9,
			num10
		}));
		this.textureBounds = new VRTextureBounds_t[2];
		this.textureBounds[0].uMin = 0.5f + 0.5f * num3 / this.tanHalfFov.x;
		this.textureBounds[0].uMax = 0.5f + 0.5f * num4 / this.tanHalfFov.x;
		this.textureBounds[0].vMin = 0.5f - 0.5f * num6 / this.tanHalfFov.y;
		this.textureBounds[0].vMax = 0.5f - 0.5f * num5 / this.tanHalfFov.y;
		this.textureBounds[1].uMin = 0.5f + 0.5f * num7 / this.tanHalfFov.x;
		this.textureBounds[1].uMax = 0.5f + 0.5f * num8 / this.tanHalfFov.x;
		this.textureBounds[1].vMin = 0.5f - 0.5f * num10 / this.tanHalfFov.y;
		this.textureBounds[1].vMax = 0.5f - 0.5f * num9 / this.tanHalfFov.y;
		this.sceneWidth /= Mathf.Max(this.textureBounds[0].uMax - this.textureBounds[0].uMin, this.textureBounds[1].uMax - this.textureBounds[1].uMin);
		this.sceneHeight /= Mathf.Max(this.textureBounds[0].vMax - this.textureBounds[0].vMin, this.textureBounds[1].vMax - this.textureBounds[1].vMin);
		this.aspect = this.tanHalfFov.x / this.tanHalfFov.y;
		this.fieldOfView = 2f * Mathf.Atan(this.tanHalfFov.y) * 57.29578f;
		this.eyes = new SteamVR_Utils.RigidTransform[]
		{
			new SteamVR_Utils.RigidTransform(this.hmd.GetEyeToHeadTransform(EVREye.Eye_Left)),
			new SteamVR_Utils.RigidTransform(this.hmd.GetEyeToHeadTransform(EVREye.Eye_Right))
		};
		if (SystemInfo.graphicsDeviceVersion.StartsWith("OpenGL"))
		{
			this.graphicsAPI = EGraphicsAPIConvention.API_OpenGL;
		}
		else
		{
			this.graphicsAPI = EGraphicsAPIConvention.API_DirectX;
		}
		SteamVR_Utils.Event.Listen("initializing", new SteamVR_Utils.Event.Handler(this.OnInitializing));
		SteamVR_Utils.Event.Listen("calibrating", new SteamVR_Utils.Event.Handler(this.OnCalibrating));
		SteamVR_Utils.Event.Listen("out_of_range", new SteamVR_Utils.Event.Handler(this.OnOutOfRange));
		SteamVR_Utils.Event.Listen("device_connected", new SteamVR_Utils.Event.Handler(this.OnDeviceConnected));
		SteamVR_Utils.Event.Listen("new_poses", new SteamVR_Utils.Event.Handler(this.OnNewPoses));
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x060018AB RID: 6315 RVA: 0x000E246A File Offset: 0x000E086A
	public static bool active
	{
		get
		{
			return SteamVR._instance != null;
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x060018AC RID: 6316 RVA: 0x000E2477 File Offset: 0x000E0877
	// (set) Token: 0x060018AD RID: 6317 RVA: 0x000E248E File Offset: 0x000E088E
	public static bool enabled
	{
		get
		{
			if (!XRSettings.enabled)
			{
				SteamVR.enabled = false;
			}
			return SteamVR._enabled;
		}
		set
		{
			SteamVR._enabled = value;
			if (!SteamVR._enabled)
			{
				SteamVR.SafeDispose();
			}
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x060018AE RID: 6318 RVA: 0x000E24A5 File Offset: 0x000E08A5
	public static SteamVR instance
	{
		get
		{
			if (!SteamVR.enabled)
			{
				return null;
			}
			if (SteamVR._instance == null)
			{
				SteamVR._instance = SteamVR.CreateInstance();
				if (SteamVR._instance == null)
				{
					SteamVR._enabled = false;
				}
			}
			return SteamVR._instance;
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x060018AF RID: 6319 RVA: 0x000E24DC File Offset: 0x000E08DC
	public static bool usingNativeSupport
	{
		get
		{
			return XRDevice.GetNativePtr() != IntPtr.Zero;
		}
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x000E24F0 File Offset: 0x000E08F0
	private static SteamVR CreateInstance()
	{
		try
		{
			EVRInitError evrinitError = EVRInitError.None;
			if (!SteamVR.usingNativeSupport)
			{
				Debug.Log("OpenVR initialization failed.  Ensure 'Virtual Reality Supported' is checked in Player Settings, and OpenVR is added to the list of Virtual Reality SDKs.");
				return null;
			}
			OpenVR.GetGenericInterface("IVRCompositor_016", ref evrinitError);
			if (evrinitError != EVRInitError.None)
			{
				SteamVR.ReportError(evrinitError);
				return null;
			}
			OpenVR.GetGenericInterface("IVROverlay_013", ref evrinitError);
			if (evrinitError != EVRInitError.None)
			{
				SteamVR.ReportError(evrinitError);
				return null;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError(ex);
			return null;
		}
		return new SteamVR();
	}

	// Token: 0x060018B1 RID: 6321 RVA: 0x000E2584 File Offset: 0x000E0984
	private static void ReportError(EVRInitError error)
	{
		if (error != EVRInitError.None)
		{
			if (error != EVRInitError.Init_VRClientDLLNotFound)
			{
				if (error != EVRInitError.Driver_RuntimeOutOfDate)
				{
					if (error != EVRInitError.VendorSpecific_UnableToConnectToOculusRuntime)
					{
						Debug.Log(OpenVR.GetStringForHmdError(error));
					}
					else
					{
						Debug.Log("SteamVR Initialization Failed!  Make sure device is on, Oculus runtime is installed, and OVRService_*.exe is running.");
					}
				}
				else
				{
					Debug.Log("SteamVR Initialization Failed!  Make sure device's runtime is up to date.");
				}
			}
			else
			{
				Debug.Log("SteamVR drivers not found!  They can be installed via Steam under Library > Tools.  Visit http://steampowered.com to install Steam.");
			}
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x060018B2 RID: 6322 RVA: 0x000E25FC File Offset: 0x000E09FC
	// (set) Token: 0x060018B3 RID: 6323 RVA: 0x000E2604 File Offset: 0x000E0A04
	public CVRSystem hmd { get; private set; }

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x060018B4 RID: 6324 RVA: 0x000E260D File Offset: 0x000E0A0D
	// (set) Token: 0x060018B5 RID: 6325 RVA: 0x000E2615 File Offset: 0x000E0A15
	public CVRCompositor compositor { get; private set; }

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x060018B6 RID: 6326 RVA: 0x000E261E File Offset: 0x000E0A1E
	// (set) Token: 0x060018B7 RID: 6327 RVA: 0x000E2626 File Offset: 0x000E0A26
	public CVROverlay overlay { get; private set; }

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x060018B8 RID: 6328 RVA: 0x000E262F File Offset: 0x000E0A2F
	// (set) Token: 0x060018B9 RID: 6329 RVA: 0x000E2636 File Offset: 0x000E0A36
	public static bool initializing { get; private set; }

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x060018BA RID: 6330 RVA: 0x000E263E File Offset: 0x000E0A3E
	// (set) Token: 0x060018BB RID: 6331 RVA: 0x000E2645 File Offset: 0x000E0A45
	public static bool calibrating { get; private set; }

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x060018BC RID: 6332 RVA: 0x000E264D File Offset: 0x000E0A4D
	// (set) Token: 0x060018BD RID: 6333 RVA: 0x000E2654 File Offset: 0x000E0A54
	public static bool outOfRange { get; private set; }

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x060018BE RID: 6334 RVA: 0x000E265C File Offset: 0x000E0A5C
	// (set) Token: 0x060018BF RID: 6335 RVA: 0x000E2664 File Offset: 0x000E0A64
	public float sceneWidth { get; private set; }

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x060018C0 RID: 6336 RVA: 0x000E266D File Offset: 0x000E0A6D
	// (set) Token: 0x060018C1 RID: 6337 RVA: 0x000E2675 File Offset: 0x000E0A75
	public float sceneHeight { get; private set; }

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x060018C2 RID: 6338 RVA: 0x000E267E File Offset: 0x000E0A7E
	// (set) Token: 0x060018C3 RID: 6339 RVA: 0x000E2686 File Offset: 0x000E0A86
	public float aspect { get; private set; }

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x060018C4 RID: 6340 RVA: 0x000E268F File Offset: 0x000E0A8F
	// (set) Token: 0x060018C5 RID: 6341 RVA: 0x000E2697 File Offset: 0x000E0A97
	public float fieldOfView { get; private set; }

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x060018C6 RID: 6342 RVA: 0x000E26A0 File Offset: 0x000E0AA0
	// (set) Token: 0x060018C7 RID: 6343 RVA: 0x000E26A8 File Offset: 0x000E0AA8
	public Vector2 tanHalfFov { get; private set; }

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x060018C8 RID: 6344 RVA: 0x000E26B1 File Offset: 0x000E0AB1
	// (set) Token: 0x060018C9 RID: 6345 RVA: 0x000E26B9 File Offset: 0x000E0AB9
	public VRTextureBounds_t[] textureBounds { get; private set; }

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x060018CA RID: 6346 RVA: 0x000E26C2 File Offset: 0x000E0AC2
	// (set) Token: 0x060018CB RID: 6347 RVA: 0x000E26CA File Offset: 0x000E0ACA
	public SteamVR_Utils.RigidTransform[] eyes { get; private set; }

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x060018CC RID: 6348 RVA: 0x000E26D3 File Offset: 0x000E0AD3
	public string hmd_TrackingSystemName
	{
		get
		{
			return this.GetStringProperty(ETrackedDeviceProperty.Prop_TrackingSystemName_String);
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x060018CD RID: 6349 RVA: 0x000E26E0 File Offset: 0x000E0AE0
	public string hmd_ModelNumber
	{
		get
		{
			return this.GetStringProperty(ETrackedDeviceProperty.Prop_ModelNumber_String);
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x060018CE RID: 6350 RVA: 0x000E26ED File Offset: 0x000E0AED
	public string hmd_SerialNumber
	{
		get
		{
			return this.GetStringProperty(ETrackedDeviceProperty.Prop_SerialNumber_String);
		}
	}

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x060018CF RID: 6351 RVA: 0x000E26FA File Offset: 0x000E0AFA
	public float hmd_SecondsFromVsyncToPhotons
	{
		get
		{
			return this.GetFloatProperty(ETrackedDeviceProperty.Prop_SecondsFromVsyncToPhotons_Float);
		}
	}

	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x060018D0 RID: 6352 RVA: 0x000E2707 File Offset: 0x000E0B07
	public float hmd_DisplayFrequency
	{
		get
		{
			return this.GetFloatProperty(ETrackedDeviceProperty.Prop_DisplayFrequency_Float);
		}
	}

	// Token: 0x060018D1 RID: 6353 RVA: 0x000E2714 File Offset: 0x000E0B14
	public string GetTrackedDeviceString(uint deviceId)
	{
		ETrackedPropertyError etrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;
		uint stringTrackedDeviceProperty = this.hmd.GetStringTrackedDeviceProperty(deviceId, ETrackedDeviceProperty.Prop_AttachedDeviceId_String, null, 0U, ref etrackedPropertyError);
		if (stringTrackedDeviceProperty > 1U)
		{
			StringBuilder stringBuilder = new StringBuilder((int)stringTrackedDeviceProperty);
			this.hmd.GetStringTrackedDeviceProperty(deviceId, ETrackedDeviceProperty.Prop_AttachedDeviceId_String, stringBuilder, stringTrackedDeviceProperty, ref etrackedPropertyError);
			return stringBuilder.ToString();
		}
		return null;
	}

	// Token: 0x060018D2 RID: 6354 RVA: 0x000E2768 File Offset: 0x000E0B68
	private string GetStringProperty(ETrackedDeviceProperty prop)
	{
		ETrackedPropertyError etrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;
		uint stringTrackedDeviceProperty = this.hmd.GetStringTrackedDeviceProperty(0U, prop, null, 0U, ref etrackedPropertyError);
		if (stringTrackedDeviceProperty > 1U)
		{
			StringBuilder stringBuilder = new StringBuilder((int)stringTrackedDeviceProperty);
			this.hmd.GetStringTrackedDeviceProperty(0U, prop, stringBuilder, stringTrackedDeviceProperty, ref etrackedPropertyError);
			return stringBuilder.ToString();
		}
		return (etrackedPropertyError == ETrackedPropertyError.TrackedProp_Success) ? "<unknown>" : etrackedPropertyError.ToString();
	}

	// Token: 0x060018D3 RID: 6355 RVA: 0x000E27D0 File Offset: 0x000E0BD0
	private float GetFloatProperty(ETrackedDeviceProperty prop)
	{
		ETrackedPropertyError etrackedPropertyError = ETrackedPropertyError.TrackedProp_Success;
		return this.hmd.GetFloatTrackedDeviceProperty(0U, prop, ref etrackedPropertyError);
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x000E27EE File Offset: 0x000E0BEE
	private void OnInitializing(params object[] args)
	{
		SteamVR.initializing = (bool)args[0];
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x000E27FD File Offset: 0x000E0BFD
	private void OnCalibrating(params object[] args)
	{
		SteamVR.calibrating = (bool)args[0];
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x000E280C File Offset: 0x000E0C0C
	private void OnOutOfRange(params object[] args)
	{
		SteamVR.outOfRange = (bool)args[0];
	}

	// Token: 0x060018D7 RID: 6359 RVA: 0x000E281C File Offset: 0x000E0C1C
	private void OnDeviceConnected(params object[] args)
	{
		int num = (int)args[0];
		SteamVR.connected[num] = (bool)args[1];
	}

	// Token: 0x060018D8 RID: 6360 RVA: 0x000E2844 File Offset: 0x000E0C44
	private void OnNewPoses(params object[] args)
	{
		TrackedDevicePose_t[] array = (TrackedDevicePose_t[])args[0];
		this.eyes[0] = new SteamVR_Utils.RigidTransform(this.hmd.GetEyeToHeadTransform(EVREye.Eye_Left));
		this.eyes[1] = new SteamVR_Utils.RigidTransform(this.hmd.GetEyeToHeadTransform(EVREye.Eye_Right));
		for (int i = 0; i < array.Length; i++)
		{
			bool bDeviceIsConnected = array[i].bDeviceIsConnected;
			if (bDeviceIsConnected != SteamVR.connected[i])
			{
				SteamVR_Utils.Event.Send("device_connected", new object[] { i, bDeviceIsConnected });
			}
		}
		if ((long)array.Length > 0L)
		{
			ETrackingResult eTrackingResult = array[(int)((UIntPtr)0)].eTrackingResult;
			bool flag = eTrackingResult == ETrackingResult.Uninitialized;
			if (flag != SteamVR.initializing)
			{
				SteamVR_Utils.Event.Send("initializing", new object[] { flag });
			}
			bool flag2 = eTrackingResult == ETrackingResult.Calibrating_InProgress || eTrackingResult == ETrackingResult.Calibrating_OutOfRange;
			if (flag2 != SteamVR.calibrating)
			{
				SteamVR_Utils.Event.Send("calibrating", new object[] { flag2 });
			}
			bool flag3 = eTrackingResult == ETrackingResult.Running_OutOfRange || eTrackingResult == ETrackingResult.Calibrating_OutOfRange;
			if (flag3 != SteamVR.outOfRange)
			{
				SteamVR_Utils.Event.Send("out_of_range", new object[] { flag3 });
			}
		}
	}

	// Token: 0x060018D9 RID: 6361 RVA: 0x000E29A8 File Offset: 0x000E0DA8
	~SteamVR()
	{
		this.Dispose(false);
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x000E29D8 File Offset: 0x000E0DD8
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x000E29E8 File Offset: 0x000E0DE8
	private void Dispose(bool disposing)
	{
		SteamVR_Utils.Event.Remove("initializing", new SteamVR_Utils.Event.Handler(this.OnInitializing));
		SteamVR_Utils.Event.Remove("calibrating", new SteamVR_Utils.Event.Handler(this.OnCalibrating));
		SteamVR_Utils.Event.Remove("out_of_range", new SteamVR_Utils.Event.Handler(this.OnOutOfRange));
		SteamVR_Utils.Event.Remove("device_connected", new SteamVR_Utils.Event.Handler(this.OnDeviceConnected));
		SteamVR_Utils.Event.Remove("new_poses", new SteamVR_Utils.Event.Handler(this.OnNewPoses));
		SteamVR._instance = null;
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x000E2A69 File Offset: 0x000E0E69
	public static void SafeDispose()
	{
		if (SteamVR._instance != null)
		{
			SteamVR._instance.Dispose();
		}
	}

	// Token: 0x04001705 RID: 5893
	private static bool _enabled = true;

	// Token: 0x04001706 RID: 5894
	private static SteamVR _instance;

	// Token: 0x0400170D RID: 5901
	public static bool[] connected = new bool[16];

	// Token: 0x04001715 RID: 5909
	public EGraphicsAPIConvention graphicsAPI;
}
