using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000186 RID: 390
	public class OpenVR
	{
		// Token: 0x06000545 RID: 1349 RVA: 0x00004514 File Offset: 0x00002714
		public static uint InitInternal(ref EVRInitError peError, EVRApplicationType eApplicationType)
		{
			return OpenVRInterop.InitInternal(ref peError, eApplicationType);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000451D File Offset: 0x0000271D
		public static void ShutdownInternal()
		{
			OpenVRInterop.ShutdownInternal();
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00004524 File Offset: 0x00002724
		public static bool IsHmdPresent()
		{
			return OpenVRInterop.IsHmdPresent();
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0000452B File Offset: 0x0000272B
		public static bool IsRuntimeInstalled()
		{
			return OpenVRInterop.IsRuntimeInstalled();
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00004532 File Offset: 0x00002732
		public static string GetStringForHmdError(EVRInitError error)
		{
			return Marshal.PtrToStringAnsi(OpenVRInterop.GetStringForHmdError(error));
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0000453F File Offset: 0x0000273F
		public static IntPtr GetGenericInterface(string pchInterfaceVersion, ref EVRInitError peError)
		{
			return OpenVRInterop.GetGenericInterface(pchInterfaceVersion, ref peError);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00004548 File Offset: 0x00002748
		public static bool IsInterfaceVersionValid(string pchInterfaceVersion)
		{
			return OpenVRInterop.IsInterfaceVersionValid(pchInterfaceVersion);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00004550 File Offset: 0x00002750
		public static uint GetInitToken()
		{
			return OpenVRInterop.GetInitToken();
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x00004557 File Offset: 0x00002757
		// (set) Token: 0x0600054E RID: 1358 RVA: 0x0000455E File Offset: 0x0000275E
		private static uint VRToken { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x00004566 File Offset: 0x00002766
		private static OpenVR.COpenVRContext OpenVRInternal_ModuleContext
		{
			get
			{
				if (OpenVR._OpenVRInternal_ModuleContext == null)
				{
					OpenVR._OpenVRInternal_ModuleContext = new OpenVR.COpenVRContext();
				}
				return OpenVR._OpenVRInternal_ModuleContext;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00004581 File Offset: 0x00002781
		public static CVRSystem System
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRSystem();
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x0000458D File Offset: 0x0000278D
		public static CVRChaperone Chaperone
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRChaperone();
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x00004599 File Offset: 0x00002799
		public static CVRChaperoneSetup ChaperoneSetup
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRChaperoneSetup();
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x000045A5 File Offset: 0x000027A5
		public static CVRCompositor Compositor
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRCompositor();
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x000045B1 File Offset: 0x000027B1
		public static CVROverlay Overlay
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VROverlay();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000555 RID: 1365 RVA: 0x000045BD File Offset: 0x000027BD
		public static CVRRenderModels RenderModels
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRRenderModels();
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x000045C9 File Offset: 0x000027C9
		public static CVRExtendedDisplay ExtendedDisplay
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRExtendedDisplay();
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x000045D5 File Offset: 0x000027D5
		public static CVRSettings Settings
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRSettings();
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x000045E1 File Offset: 0x000027E1
		public static CVRApplications Applications
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRApplications();
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x000045ED File Offset: 0x000027ED
		public static CVRScreenshots Screenshots
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRScreenshots();
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x000045F9 File Offset: 0x000027F9
		public static CVRTrackedCamera TrackedCamera
		{
			get
			{
				return OpenVR.OpenVRInternal_ModuleContext.VRTrackedCamera();
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00004608 File Offset: 0x00002808
		public static CVRSystem Init(ref EVRInitError peError, EVRApplicationType eApplicationType = EVRApplicationType.VRApplication_Scene)
		{
			OpenVR.VRToken = OpenVR.InitInternal(ref peError, eApplicationType);
			OpenVR.OpenVRInternal_ModuleContext.Clear();
			if (peError != EVRInitError.None)
			{
				return null;
			}
			if (!OpenVR.IsInterfaceVersionValid("IVRSystem_012"))
			{
				OpenVR.ShutdownInternal();
				peError = EVRInitError.Init_InterfaceNotFound;
				return null;
			}
			return OpenVR.System;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00004655 File Offset: 0x00002855
		public static void Shutdown()
		{
			OpenVR.ShutdownInternal();
		}

		// Token: 0x04000455 RID: 1109
		public const uint k_unTrackingStringSize = 32U;

		// Token: 0x04000456 RID: 1110
		public const uint k_unMaxDriverDebugResponseSize = 32768U;

		// Token: 0x04000457 RID: 1111
		public const uint k_unTrackedDeviceIndex_Hmd = 0U;

		// Token: 0x04000458 RID: 1112
		public const uint k_unMaxTrackedDeviceCount = 16U;

		// Token: 0x04000459 RID: 1113
		public const uint k_unTrackedDeviceIndexOther = 4294967294U;

		// Token: 0x0400045A RID: 1114
		public const uint k_unTrackedDeviceIndexInvalid = 4294967295U;

		// Token: 0x0400045B RID: 1115
		public const uint k_unMaxPropertyStringSize = 32768U;

		// Token: 0x0400045C RID: 1116
		public const uint k_unControllerStateAxisCount = 5U;

		// Token: 0x0400045D RID: 1117
		public const ulong k_ulOverlayHandleInvalid = 0UL;

		// Token: 0x0400045E RID: 1118
		public const uint k_unScreenshotHandleInvalid = 0U;

		// Token: 0x0400045F RID: 1119
		public const string IVRSystem_Version = "IVRSystem_012";

		// Token: 0x04000460 RID: 1120
		public const string IVRExtendedDisplay_Version = "IVRExtendedDisplay_001";

		// Token: 0x04000461 RID: 1121
		public const string IVRTrackedCamera_Version = "IVRTrackedCamera_003";

		// Token: 0x04000462 RID: 1122
		public const uint k_unMaxApplicationKeyLength = 128U;

		// Token: 0x04000463 RID: 1123
		public const string IVRApplications_Version = "IVRApplications_006";

		// Token: 0x04000464 RID: 1124
		public const string IVRChaperone_Version = "IVRChaperone_003";

		// Token: 0x04000465 RID: 1125
		public const string IVRChaperoneSetup_Version = "IVRChaperoneSetup_005";

		// Token: 0x04000466 RID: 1126
		public const string IVRCompositor_Version = "IVRCompositor_016";

		// Token: 0x04000467 RID: 1127
		public const uint k_unVROverlayMaxKeyLength = 128U;

		// Token: 0x04000468 RID: 1128
		public const uint k_unVROverlayMaxNameLength = 128U;

		// Token: 0x04000469 RID: 1129
		public const uint k_unMaxOverlayCount = 64U;

		// Token: 0x0400046A RID: 1130
		public const string IVROverlay_Version = "IVROverlay_013";

		// Token: 0x0400046B RID: 1131
		public const string k_pch_Controller_Component_GDC2015 = "gdc2015";

		// Token: 0x0400046C RID: 1132
		public const string k_pch_Controller_Component_Base = "base";

		// Token: 0x0400046D RID: 1133
		public const string k_pch_Controller_Component_Tip = "tip";

		// Token: 0x0400046E RID: 1134
		public const string k_pch_Controller_Component_HandGrip = "handgrip";

		// Token: 0x0400046F RID: 1135
		public const string k_pch_Controller_Component_Status = "status";

		// Token: 0x04000470 RID: 1136
		public const string IVRRenderModels_Version = "IVRRenderModels_005";

		// Token: 0x04000471 RID: 1137
		public const uint k_unNotificationTextMaxSize = 256U;

		// Token: 0x04000472 RID: 1138
		public const string IVRNotifications_Version = "IVRNotifications_002";

		// Token: 0x04000473 RID: 1139
		public const uint k_unMaxSettingsKeyLength = 128U;

		// Token: 0x04000474 RID: 1140
		public const string IVRSettings_Version = "IVRSettings_001";

		// Token: 0x04000475 RID: 1141
		public const string k_pch_SteamVR_Section = "steamvr";

		// Token: 0x04000476 RID: 1142
		public const string k_pch_SteamVR_RequireHmd_String = "requireHmd";

		// Token: 0x04000477 RID: 1143
		public const string k_pch_SteamVR_ForcedDriverKey_String = "forcedDriver";

		// Token: 0x04000478 RID: 1144
		public const string k_pch_SteamVR_ForcedHmdKey_String = "forcedHmd";

		// Token: 0x04000479 RID: 1145
		public const string k_pch_SteamVR_DisplayDebug_Bool = "displayDebug";

		// Token: 0x0400047A RID: 1146
		public const string k_pch_SteamVR_DebugProcessPipe_String = "debugProcessPipe";

		// Token: 0x0400047B RID: 1147
		public const string k_pch_SteamVR_EnableDistortion_Bool = "enableDistortion";

		// Token: 0x0400047C RID: 1148
		public const string k_pch_SteamVR_DisplayDebugX_Int32 = "displayDebugX";

		// Token: 0x0400047D RID: 1149
		public const string k_pch_SteamVR_DisplayDebugY_Int32 = "displayDebugY";

		// Token: 0x0400047E RID: 1150
		public const string k_pch_SteamVR_SendSystemButtonToAllApps_Bool = "sendSystemButtonToAllApps";

		// Token: 0x0400047F RID: 1151
		public const string k_pch_SteamVR_LogLevel_Int32 = "loglevel";

		// Token: 0x04000480 RID: 1152
		public const string k_pch_SteamVR_IPD_Float = "ipd";

		// Token: 0x04000481 RID: 1153
		public const string k_pch_SteamVR_Background_String = "background";

		// Token: 0x04000482 RID: 1154
		public const string k_pch_SteamVR_BackgroundCameraHeight_Float = "backgroundCameraHeight";

		// Token: 0x04000483 RID: 1155
		public const string k_pch_SteamVR_BackgroundDomeRadius_Float = "backgroundDomeRadius";

		// Token: 0x04000484 RID: 1156
		public const string k_pch_SteamVR_Environment_String = "environment";

		// Token: 0x04000485 RID: 1157
		public const string k_pch_SteamVR_GridColor_String = "gridColor";

		// Token: 0x04000486 RID: 1158
		public const string k_pch_SteamVR_PlayAreaColor_String = "playAreaColor";

		// Token: 0x04000487 RID: 1159
		public const string k_pch_SteamVR_ShowStage_Bool = "showStage";

		// Token: 0x04000488 RID: 1160
		public const string k_pch_SteamVR_ActivateMultipleDrivers_Bool = "activateMultipleDrivers";

		// Token: 0x04000489 RID: 1161
		public const string k_pch_SteamVR_DirectMode_Bool = "directMode";

		// Token: 0x0400048A RID: 1162
		public const string k_pch_SteamVR_DirectModeEdidVid_Int32 = "directModeEdidVid";

		// Token: 0x0400048B RID: 1163
		public const string k_pch_SteamVR_DirectModeEdidPid_Int32 = "directModeEdidPid";

		// Token: 0x0400048C RID: 1164
		public const string k_pch_SteamVR_UsingSpeakers_Bool = "usingSpeakers";

		// Token: 0x0400048D RID: 1165
		public const string k_pch_SteamVR_SpeakersForwardYawOffsetDegrees_Float = "speakersForwardYawOffsetDegrees";

		// Token: 0x0400048E RID: 1166
		public const string k_pch_SteamVR_BaseStationPowerManagement_Bool = "basestationPowerManagement";

		// Token: 0x0400048F RID: 1167
		public const string k_pch_SteamVR_NeverKillProcesses_Bool = "neverKillProcesses";

		// Token: 0x04000490 RID: 1168
		public const string k_pch_SteamVR_RenderTargetMultiplier_Float = "renderTargetMultiplier";

		// Token: 0x04000491 RID: 1169
		public const string k_pch_SteamVR_AllowReprojection_Bool = "allowReprojection";

		// Token: 0x04000492 RID: 1170
		public const string k_pch_SteamVR_ForceReprojection_Bool = "forceReprojection";

		// Token: 0x04000493 RID: 1171
		public const string k_pch_SteamVR_ForceFadeOnBadTracking_Bool = "forceFadeOnBadTracking";

		// Token: 0x04000494 RID: 1172
		public const string k_pch_SteamVR_DefaultMirrorView_Int32 = "defaultMirrorView";

		// Token: 0x04000495 RID: 1173
		public const string k_pch_SteamVR_ShowMirrorView_Bool = "showMirrorView";

		// Token: 0x04000496 RID: 1174
		public const string k_pch_SteamVR_StartMonitorFromAppLaunch = "startMonitorFromAppLaunch";

		// Token: 0x04000497 RID: 1175
		public const string k_pch_SteamVR_UseGenericGraphicsDevice_Bool = "useGenericGraphicsDevice";

		// Token: 0x04000498 RID: 1176
		public const string k_pch_Lighthouse_Section = "driver_lighthouse";

		// Token: 0x04000499 RID: 1177
		public const string k_pch_Lighthouse_DisableIMU_Bool = "disableimu";

		// Token: 0x0400049A RID: 1178
		public const string k_pch_Lighthouse_UseDisambiguation_String = "usedisambiguation";

		// Token: 0x0400049B RID: 1179
		public const string k_pch_Lighthouse_DisambiguationDebug_Int32 = "disambiguationdebug";

		// Token: 0x0400049C RID: 1180
		public const string k_pch_Lighthouse_PrimaryBasestation_Int32 = "primarybasestation";

		// Token: 0x0400049D RID: 1181
		public const string k_pch_Lighthouse_LighthouseName_String = "lighthousename";

		// Token: 0x0400049E RID: 1182
		public const string k_pch_Lighthouse_MaxIncidenceAngleDegrees_Float = "maxincidenceangledegrees";

		// Token: 0x0400049F RID: 1183
		public const string k_pch_Lighthouse_UseLighthouseDirect_Bool = "uselighthousedirect";

		// Token: 0x040004A0 RID: 1184
		public const string k_pch_Lighthouse_DBHistory_Bool = "dbhistory";

		// Token: 0x040004A1 RID: 1185
		public const string k_pch_Null_Section = "driver_null";

		// Token: 0x040004A2 RID: 1186
		public const string k_pch_Null_EnableNullDriver_Bool = "enable";

		// Token: 0x040004A3 RID: 1187
		public const string k_pch_Null_SerialNumber_String = "serialNumber";

		// Token: 0x040004A4 RID: 1188
		public const string k_pch_Null_ModelNumber_String = "modelNumber";

		// Token: 0x040004A5 RID: 1189
		public const string k_pch_Null_WindowX_Int32 = "windowX";

		// Token: 0x040004A6 RID: 1190
		public const string k_pch_Null_WindowY_Int32 = "windowY";

		// Token: 0x040004A7 RID: 1191
		public const string k_pch_Null_WindowWidth_Int32 = "windowWidth";

		// Token: 0x040004A8 RID: 1192
		public const string k_pch_Null_WindowHeight_Int32 = "windowHeight";

		// Token: 0x040004A9 RID: 1193
		public const string k_pch_Null_RenderWidth_Int32 = "renderWidth";

		// Token: 0x040004AA RID: 1194
		public const string k_pch_Null_RenderHeight_Int32 = "renderHeight";

		// Token: 0x040004AB RID: 1195
		public const string k_pch_Null_SecondsFromVsyncToPhotons_Float = "secondsFromVsyncToPhotons";

		// Token: 0x040004AC RID: 1196
		public const string k_pch_Null_DisplayFrequency_Float = "displayFrequency";

		// Token: 0x040004AD RID: 1197
		public const string k_pch_UserInterface_Section = "userinterface";

		// Token: 0x040004AE RID: 1198
		public const string k_pch_UserInterface_StatusAlwaysOnTop_Bool = "StatusAlwaysOnTop";

		// Token: 0x040004AF RID: 1199
		public const string k_pch_UserInterface_Screenshots_Bool = "screenshots";

		// Token: 0x040004B0 RID: 1200
		public const string k_pch_UserInterface_ScreenshotType_Int = "screenshotType";

		// Token: 0x040004B1 RID: 1201
		public const string k_pch_Notifications_Section = "notifications";

		// Token: 0x040004B2 RID: 1202
		public const string k_pch_Notifications_DoNotDisturb_Bool = "DoNotDisturb";

		// Token: 0x040004B3 RID: 1203
		public const string k_pch_Keyboard_Section = "keyboard";

		// Token: 0x040004B4 RID: 1204
		public const string k_pch_Keyboard_TutorialCompletions = "TutorialCompletions";

		// Token: 0x040004B5 RID: 1205
		public const string k_pch_Keyboard_ScaleX = "ScaleX";

		// Token: 0x040004B6 RID: 1206
		public const string k_pch_Keyboard_ScaleY = "ScaleY";

		// Token: 0x040004B7 RID: 1207
		public const string k_pch_Keyboard_OffsetLeftX = "OffsetLeftX";

		// Token: 0x040004B8 RID: 1208
		public const string k_pch_Keyboard_OffsetRightX = "OffsetRightX";

		// Token: 0x040004B9 RID: 1209
		public const string k_pch_Keyboard_OffsetY = "OffsetY";

		// Token: 0x040004BA RID: 1210
		public const string k_pch_Keyboard_Smoothing = "Smoothing";

		// Token: 0x040004BB RID: 1211
		public const string k_pch_Perf_Section = "perfcheck";

		// Token: 0x040004BC RID: 1212
		public const string k_pch_Perf_HeuristicActive_Bool = "heuristicActive";

		// Token: 0x040004BD RID: 1213
		public const string k_pch_Perf_NotifyInHMD_Bool = "warnInHMD";

		// Token: 0x040004BE RID: 1214
		public const string k_pch_Perf_NotifyOnlyOnce_Bool = "warnOnlyOnce";

		// Token: 0x040004BF RID: 1215
		public const string k_pch_Perf_AllowTimingStore_Bool = "allowTimingStore";

		// Token: 0x040004C0 RID: 1216
		public const string k_pch_Perf_SaveTimingsOnExit_Bool = "saveTimingsOnExit";

		// Token: 0x040004C1 RID: 1217
		public const string k_pch_Perf_TestData_Float = "perfTestData";

		// Token: 0x040004C2 RID: 1218
		public const string k_pch_CollisionBounds_Section = "collisionBounds";

		// Token: 0x040004C3 RID: 1219
		public const string k_pch_CollisionBounds_Style_Int32 = "CollisionBoundsStyle";

		// Token: 0x040004C4 RID: 1220
		public const string k_pch_CollisionBounds_GroundPerimeterOn_Bool = "CollisionBoundsGroundPerimeterOn";

		// Token: 0x040004C5 RID: 1221
		public const string k_pch_CollisionBounds_CenterMarkerOn_Bool = "CollisionBoundsCenterMarkerOn";

		// Token: 0x040004C6 RID: 1222
		public const string k_pch_CollisionBounds_PlaySpaceOn_Bool = "CollisionBoundsPlaySpaceOn";

		// Token: 0x040004C7 RID: 1223
		public const string k_pch_CollisionBounds_FadeDistance_Float = "CollisionBoundsFadeDistance";

		// Token: 0x040004C8 RID: 1224
		public const string k_pch_CollisionBounds_ColorGammaR_Int32 = "CollisionBoundsColorGammaR";

		// Token: 0x040004C9 RID: 1225
		public const string k_pch_CollisionBounds_ColorGammaG_Int32 = "CollisionBoundsColorGammaG";

		// Token: 0x040004CA RID: 1226
		public const string k_pch_CollisionBounds_ColorGammaB_Int32 = "CollisionBoundsColorGammaB";

		// Token: 0x040004CB RID: 1227
		public const string k_pch_CollisionBounds_ColorGammaA_Int32 = "CollisionBoundsColorGammaA";

		// Token: 0x040004CC RID: 1228
		public const string k_pch_Camera_Section = "camera";

		// Token: 0x040004CD RID: 1229
		public const string k_pch_Camera_EnableCamera_Bool = "enableCamera";

		// Token: 0x040004CE RID: 1230
		public const string k_pch_Camera_EnableCameraInDashboard_Bool = "enableCameraInDashboard";

		// Token: 0x040004CF RID: 1231
		public const string k_pch_Camera_EnableCameraForCollisionBounds_Bool = "enableCameraForCollisionBounds";

		// Token: 0x040004D0 RID: 1232
		public const string k_pch_Camera_EnableCameraForRoomView_Bool = "enableCameraForRoomView";

		// Token: 0x040004D1 RID: 1233
		public const string k_pch_Camera_BoundsColorGammaR_Int32 = "cameraBoundsColorGammaR";

		// Token: 0x040004D2 RID: 1234
		public const string k_pch_Camera_BoundsColorGammaG_Int32 = "cameraBoundsColorGammaG";

		// Token: 0x040004D3 RID: 1235
		public const string k_pch_Camera_BoundsColorGammaB_Int32 = "cameraBoundsColorGammaB";

		// Token: 0x040004D4 RID: 1236
		public const string k_pch_Camera_BoundsColorGammaA_Int32 = "cameraBoundsColorGammaA";

		// Token: 0x040004D5 RID: 1237
		public const string k_pch_audio_Section = "audio";

		// Token: 0x040004D6 RID: 1238
		public const string k_pch_audio_OnPlaybackDevice_String = "onPlaybackDevice";

		// Token: 0x040004D7 RID: 1239
		public const string k_pch_audio_OnRecordDevice_String = "onRecordDevice";

		// Token: 0x040004D8 RID: 1240
		public const string k_pch_audio_OnPlaybackMirrorDevice_String = "onPlaybackMirrorDevice";

		// Token: 0x040004D9 RID: 1241
		public const string k_pch_audio_OffPlaybackDevice_String = "offPlaybackDevice";

		// Token: 0x040004DA RID: 1242
		public const string k_pch_audio_OffRecordDevice_String = "offRecordDevice";

		// Token: 0x040004DB RID: 1243
		public const string k_pch_audio_VIVEHDMIGain = "viveHDMIGain";

		// Token: 0x040004DC RID: 1244
		public const string k_pch_Power_Section = "power";

		// Token: 0x040004DD RID: 1245
		public const string k_pch_Power_PowerOffOnExit_Bool = "powerOffOnExit";

		// Token: 0x040004DE RID: 1246
		public const string k_pch_Power_TurnOffScreensTimeout_Float = "turnOffScreensTimeout";

		// Token: 0x040004DF RID: 1247
		public const string k_pch_Power_TurnOffControllersTimeout_Float = "turnOffControllersTimeout";

		// Token: 0x040004E0 RID: 1248
		public const string k_pch_Power_ReturnToWatchdogTimeout_Float = "returnToWatchdogTimeout";

		// Token: 0x040004E1 RID: 1249
		public const string k_pch_Power_AutoLaunchSteamVROnButtonPress = "autoLaunchSteamVROnButtonPress";

		// Token: 0x040004E2 RID: 1250
		public const string k_pch_Dashboard_Section = "dashboard";

		// Token: 0x040004E3 RID: 1251
		public const string k_pch_Dashboard_EnableDashboard_Bool = "enableDashboard";

		// Token: 0x040004E4 RID: 1252
		public const string k_pch_Dashboard_ArcadeMode_Bool = "arcadeMode";

		// Token: 0x040004E5 RID: 1253
		public const string k_pch_modelskin_Section = "modelskins";

		// Token: 0x040004E6 RID: 1254
		public const string IVRScreenshots_Version = "IVRScreenshots_001";

		// Token: 0x040004E7 RID: 1255
		public const string IVRResources_Version = "IVRResources_001";

		// Token: 0x040004E9 RID: 1257
		private const string FnTable_Prefix = "FnTable:";

		// Token: 0x040004EA RID: 1258
		private static OpenVR.COpenVRContext _OpenVRInternal_ModuleContext;

		// Token: 0x02000187 RID: 391
		private class COpenVRContext
		{
			// Token: 0x0600055E RID: 1374 RVA: 0x0000465E File Offset: 0x0000285E
			public COpenVRContext()
			{
				this.Clear();
			}

			// Token: 0x0600055F RID: 1375 RVA: 0x0000466C File Offset: 0x0000286C
			public void Clear()
			{
				this.m_pVRSystem = null;
				this.m_pVRChaperone = null;
				this.m_pVRChaperoneSetup = null;
				this.m_pVRCompositor = null;
				this.m_pVROverlay = null;
				this.m_pVRRenderModels = null;
				this.m_pVRExtendedDisplay = null;
				this.m_pVRSettings = null;
				this.m_pVRApplications = null;
				this.m_pVRScreenshots = null;
				this.m_pVRTrackedCamera = null;
			}

			// Token: 0x06000560 RID: 1376 RVA: 0x000046C6 File Offset: 0x000028C6
			private void CheckClear()
			{
				if (OpenVR.VRToken != OpenVR.GetInitToken())
				{
					this.Clear();
					OpenVR.VRToken = OpenVR.GetInitToken();
				}
			}

			// Token: 0x06000561 RID: 1377 RVA: 0x000046E8 File Offset: 0x000028E8
			public CVRSystem VRSystem()
			{
				this.CheckClear();
				if (this.m_pVRSystem == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRSystem_012", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRSystem = new CVRSystem(genericInterface);
					}
				}
				return this.m_pVRSystem;
			}

			// Token: 0x06000562 RID: 1378 RVA: 0x00004740 File Offset: 0x00002940
			public CVRChaperone VRChaperone()
			{
				this.CheckClear();
				if (this.m_pVRChaperone == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRChaperone_003", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRChaperone = new CVRChaperone(genericInterface);
					}
				}
				return this.m_pVRChaperone;
			}

			// Token: 0x06000563 RID: 1379 RVA: 0x00004798 File Offset: 0x00002998
			public CVRChaperoneSetup VRChaperoneSetup()
			{
				this.CheckClear();
				if (this.m_pVRChaperoneSetup == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRChaperoneSetup_005", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRChaperoneSetup = new CVRChaperoneSetup(genericInterface);
					}
				}
				return this.m_pVRChaperoneSetup;
			}

			// Token: 0x06000564 RID: 1380 RVA: 0x000047F0 File Offset: 0x000029F0
			public CVRCompositor VRCompositor()
			{
				this.CheckClear();
				if (this.m_pVRCompositor == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRCompositor_016", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRCompositor = new CVRCompositor(genericInterface);
					}
				}
				return this.m_pVRCompositor;
			}

			// Token: 0x06000565 RID: 1381 RVA: 0x00004848 File Offset: 0x00002A48
			public CVROverlay VROverlay()
			{
				this.CheckClear();
				if (this.m_pVROverlay == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVROverlay_013", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVROverlay = new CVROverlay(genericInterface);
					}
				}
				return this.m_pVROverlay;
			}

			// Token: 0x06000566 RID: 1382 RVA: 0x000048A0 File Offset: 0x00002AA0
			public CVRRenderModels VRRenderModels()
			{
				this.CheckClear();
				if (this.m_pVRRenderModels == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRRenderModels_005", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRRenderModels = new CVRRenderModels(genericInterface);
					}
				}
				return this.m_pVRRenderModels;
			}

			// Token: 0x06000567 RID: 1383 RVA: 0x000048F8 File Offset: 0x00002AF8
			public CVRExtendedDisplay VRExtendedDisplay()
			{
				this.CheckClear();
				if (this.m_pVRExtendedDisplay == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRExtendedDisplay_001", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRExtendedDisplay = new CVRExtendedDisplay(genericInterface);
					}
				}
				return this.m_pVRExtendedDisplay;
			}

			// Token: 0x06000568 RID: 1384 RVA: 0x00004950 File Offset: 0x00002B50
			public CVRSettings VRSettings()
			{
				this.CheckClear();
				if (this.m_pVRSettings == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRSettings_001", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRSettings = new CVRSettings(genericInterface);
					}
				}
				return this.m_pVRSettings;
			}

			// Token: 0x06000569 RID: 1385 RVA: 0x000049A8 File Offset: 0x00002BA8
			public CVRApplications VRApplications()
			{
				this.CheckClear();
				if (this.m_pVRApplications == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRApplications_006", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRApplications = new CVRApplications(genericInterface);
					}
				}
				return this.m_pVRApplications;
			}

			// Token: 0x0600056A RID: 1386 RVA: 0x00004A00 File Offset: 0x00002C00
			public CVRScreenshots VRScreenshots()
			{
				this.CheckClear();
				if (this.m_pVRScreenshots == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRScreenshots_001", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRScreenshots = new CVRScreenshots(genericInterface);
					}
				}
				return this.m_pVRScreenshots;
			}

			// Token: 0x0600056B RID: 1387 RVA: 0x00004A58 File Offset: 0x00002C58
			public CVRTrackedCamera VRTrackedCamera()
			{
				this.CheckClear();
				if (this.m_pVRTrackedCamera == null)
				{
					EVRInitError evrinitError = EVRInitError.None;
					IntPtr genericInterface = OpenVRInterop.GetGenericInterface("FnTable:IVRTrackedCamera_003", ref evrinitError);
					if (genericInterface != IntPtr.Zero && evrinitError == EVRInitError.None)
					{
						this.m_pVRTrackedCamera = new CVRTrackedCamera(genericInterface);
					}
				}
				return this.m_pVRTrackedCamera;
			}

			// Token: 0x040004EB RID: 1259
			private CVRSystem m_pVRSystem;

			// Token: 0x040004EC RID: 1260
			private CVRChaperone m_pVRChaperone;

			// Token: 0x040004ED RID: 1261
			private CVRChaperoneSetup m_pVRChaperoneSetup;

			// Token: 0x040004EE RID: 1262
			private CVRCompositor m_pVRCompositor;

			// Token: 0x040004EF RID: 1263
			private CVROverlay m_pVROverlay;

			// Token: 0x040004F0 RID: 1264
			private CVRRenderModels m_pVRRenderModels;

			// Token: 0x040004F1 RID: 1265
			private CVRExtendedDisplay m_pVRExtendedDisplay;

			// Token: 0x040004F2 RID: 1266
			private CVRSettings m_pVRSettings;

			// Token: 0x040004F3 RID: 1267
			private CVRApplications m_pVRApplications;

			// Token: 0x040004F4 RID: 1268
			private CVRScreenshots m_pVRScreenshots;

			// Token: 0x040004F5 RID: 1269
			private CVRTrackedCamera m_pVRTrackedCamera;
		}
	}
}
