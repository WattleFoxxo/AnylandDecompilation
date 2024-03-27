using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x0200011F RID: 287
	public class CVROverlay
	{
		// Token: 0x060004C4 RID: 1220 RVA: 0x000034B7 File Offset: 0x000016B7
		internal CVROverlay(IntPtr pInterface)
		{
			this.FnTable = (IVROverlay)Marshal.PtrToStructure(pInterface, typeof(IVROverlay));
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x000034DC File Offset: 0x000016DC
		public EVROverlayError FindOverlay(string pchOverlayKey, ref ulong pOverlayHandle)
		{
			pOverlayHandle = 0UL;
			return this.FnTable.FindOverlay(pchOverlayKey, ref pOverlayHandle);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00003504 File Offset: 0x00001704
		public EVROverlayError CreateOverlay(string pchOverlayKey, string pchOverlayFriendlyName, ref ulong pOverlayHandle)
		{
			pOverlayHandle = 0UL;
			return this.FnTable.CreateOverlay(pchOverlayKey, pchOverlayFriendlyName, ref pOverlayHandle);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0000352C File Offset: 0x0000172C
		public EVROverlayError DestroyOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.DestroyOverlay(ulOverlayHandle);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000354C File Offset: 0x0000174C
		public EVROverlayError SetHighQualityOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.SetHighQualityOverlay(ulOverlayHandle);
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000356C File Offset: 0x0000176C
		public ulong GetHighQualityOverlay()
		{
			return this.FnTable.GetHighQualityOverlay();
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0000358C File Offset: 0x0000178C
		public uint GetOverlayKey(ulong ulOverlayHandle, StringBuilder pchValue, uint unBufferSize, ref EVROverlayError pError)
		{
			return this.FnTable.GetOverlayKey(ulOverlayHandle, pchValue, unBufferSize, ref pError);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000035B0 File Offset: 0x000017B0
		public uint GetOverlayName(ulong ulOverlayHandle, StringBuilder pchValue, uint unBufferSize, ref EVROverlayError pError)
		{
			return this.FnTable.GetOverlayName(ulOverlayHandle, pchValue, unBufferSize, ref pError);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000035D4 File Offset: 0x000017D4
		public EVROverlayError GetOverlayImageData(ulong ulOverlayHandle, IntPtr pvBuffer, uint unBufferSize, ref uint punWidth, ref uint punHeight)
		{
			punWidth = 0U;
			punHeight = 0U;
			return this.FnTable.GetOverlayImageData(ulOverlayHandle, pvBuffer, unBufferSize, ref punWidth, ref punHeight);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00003604 File Offset: 0x00001804
		public string GetOverlayErrorNameFromEnum(EVROverlayError error)
		{
			IntPtr intPtr = this.FnTable.GetOverlayErrorNameFromEnum(error);
			return Marshal.PtrToStringAnsi(intPtr);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0000362C File Offset: 0x0000182C
		public EVROverlayError SetOverlayRenderingPid(ulong ulOverlayHandle, uint unPID)
		{
			return this.FnTable.SetOverlayRenderingPid(ulOverlayHandle, unPID);
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00003650 File Offset: 0x00001850
		public uint GetOverlayRenderingPid(ulong ulOverlayHandle)
		{
			return this.FnTable.GetOverlayRenderingPid(ulOverlayHandle);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00003670 File Offset: 0x00001870
		public EVROverlayError SetOverlayFlag(ulong ulOverlayHandle, VROverlayFlags eOverlayFlag, bool bEnabled)
		{
			return this.FnTable.SetOverlayFlag(ulOverlayHandle, eOverlayFlag, bEnabled);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00003694 File Offset: 0x00001894
		public EVROverlayError GetOverlayFlag(ulong ulOverlayHandle, VROverlayFlags eOverlayFlag, ref bool pbEnabled)
		{
			pbEnabled = false;
			return this.FnTable.GetOverlayFlag(ulOverlayHandle, eOverlayFlag, ref pbEnabled);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x000036BC File Offset: 0x000018BC
		public EVROverlayError SetOverlayColor(ulong ulOverlayHandle, float fRed, float fGreen, float fBlue)
		{
			return this.FnTable.SetOverlayColor(ulOverlayHandle, fRed, fGreen, fBlue);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x000036E0 File Offset: 0x000018E0
		public EVROverlayError GetOverlayColor(ulong ulOverlayHandle, ref float pfRed, ref float pfGreen, ref float pfBlue)
		{
			pfRed = 0f;
			pfGreen = 0f;
			pfBlue = 0f;
			return this.FnTable.GetOverlayColor(ulOverlayHandle, ref pfRed, ref pfGreen, ref pfBlue);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0000371C File Offset: 0x0000191C
		public EVROverlayError SetOverlayAlpha(ulong ulOverlayHandle, float fAlpha)
		{
			return this.FnTable.SetOverlayAlpha(ulOverlayHandle, fAlpha);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00003740 File Offset: 0x00001940
		public EVROverlayError GetOverlayAlpha(ulong ulOverlayHandle, ref float pfAlpha)
		{
			pfAlpha = 0f;
			return this.FnTable.GetOverlayAlpha(ulOverlayHandle, ref pfAlpha);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00003768 File Offset: 0x00001968
		public EVROverlayError SetOverlayTexelAspect(ulong ulOverlayHandle, float fTexelAspect)
		{
			return this.FnTable.SetOverlayTexelAspect(ulOverlayHandle, fTexelAspect);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0000378C File Offset: 0x0000198C
		public EVROverlayError GetOverlayTexelAspect(ulong ulOverlayHandle, ref float pfTexelAspect)
		{
			pfTexelAspect = 0f;
			return this.FnTable.GetOverlayTexelAspect(ulOverlayHandle, ref pfTexelAspect);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x000037B4 File Offset: 0x000019B4
		public EVROverlayError SetOverlaySortOrder(ulong ulOverlayHandle, uint unSortOrder)
		{
			return this.FnTable.SetOverlaySortOrder(ulOverlayHandle, unSortOrder);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000037D8 File Offset: 0x000019D8
		public EVROverlayError GetOverlaySortOrder(ulong ulOverlayHandle, ref uint punSortOrder)
		{
			punSortOrder = 0U;
			return this.FnTable.GetOverlaySortOrder(ulOverlayHandle, ref punSortOrder);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x000037FC File Offset: 0x000019FC
		public EVROverlayError SetOverlayWidthInMeters(ulong ulOverlayHandle, float fWidthInMeters)
		{
			return this.FnTable.SetOverlayWidthInMeters(ulOverlayHandle, fWidthInMeters);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00003820 File Offset: 0x00001A20
		public EVROverlayError GetOverlayWidthInMeters(ulong ulOverlayHandle, ref float pfWidthInMeters)
		{
			pfWidthInMeters = 0f;
			return this.FnTable.GetOverlayWidthInMeters(ulOverlayHandle, ref pfWidthInMeters);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00003848 File Offset: 0x00001A48
		public EVROverlayError SetOverlayAutoCurveDistanceRangeInMeters(ulong ulOverlayHandle, float fMinDistanceInMeters, float fMaxDistanceInMeters)
		{
			return this.FnTable.SetOverlayAutoCurveDistanceRangeInMeters(ulOverlayHandle, fMinDistanceInMeters, fMaxDistanceInMeters);
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0000386C File Offset: 0x00001A6C
		public EVROverlayError GetOverlayAutoCurveDistanceRangeInMeters(ulong ulOverlayHandle, ref float pfMinDistanceInMeters, ref float pfMaxDistanceInMeters)
		{
			pfMinDistanceInMeters = 0f;
			pfMaxDistanceInMeters = 0f;
			return this.FnTable.GetOverlayAutoCurveDistanceRangeInMeters(ulOverlayHandle, ref pfMinDistanceInMeters, ref pfMaxDistanceInMeters);
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0000389C File Offset: 0x00001A9C
		public EVROverlayError SetOverlayTextureColorSpace(ulong ulOverlayHandle, EColorSpace eTextureColorSpace)
		{
			return this.FnTable.SetOverlayTextureColorSpace(ulOverlayHandle, eTextureColorSpace);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x000038C0 File Offset: 0x00001AC0
		public EVROverlayError GetOverlayTextureColorSpace(ulong ulOverlayHandle, ref EColorSpace peTextureColorSpace)
		{
			return this.FnTable.GetOverlayTextureColorSpace(ulOverlayHandle, ref peTextureColorSpace);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000038E4 File Offset: 0x00001AE4
		public EVROverlayError SetOverlayTextureBounds(ulong ulOverlayHandle, ref VRTextureBounds_t pOverlayTextureBounds)
		{
			return this.FnTable.SetOverlayTextureBounds(ulOverlayHandle, ref pOverlayTextureBounds);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00003908 File Offset: 0x00001B08
		public EVROverlayError GetOverlayTextureBounds(ulong ulOverlayHandle, ref VRTextureBounds_t pOverlayTextureBounds)
		{
			return this.FnTable.GetOverlayTextureBounds(ulOverlayHandle, ref pOverlayTextureBounds);
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0000392C File Offset: 0x00001B2C
		public EVROverlayError GetOverlayTransformType(ulong ulOverlayHandle, ref VROverlayTransformType peTransformType)
		{
			return this.FnTable.GetOverlayTransformType(ulOverlayHandle, ref peTransformType);
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00003950 File Offset: 0x00001B50
		public EVROverlayError SetOverlayTransformAbsolute(ulong ulOverlayHandle, ETrackingUniverseOrigin eTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToOverlayTransform)
		{
			return this.FnTable.SetOverlayTransformAbsolute(ulOverlayHandle, eTrackingOrigin, ref pmatTrackingOriginToOverlayTransform);
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00003974 File Offset: 0x00001B74
		public EVROverlayError GetOverlayTransformAbsolute(ulong ulOverlayHandle, ref ETrackingUniverseOrigin peTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToOverlayTransform)
		{
			return this.FnTable.GetOverlayTransformAbsolute(ulOverlayHandle, ref peTrackingOrigin, ref pmatTrackingOriginToOverlayTransform);
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00003998 File Offset: 0x00001B98
		public EVROverlayError SetOverlayTransformTrackedDeviceRelative(ulong ulOverlayHandle, uint unTrackedDevice, ref HmdMatrix34_t pmatTrackedDeviceToOverlayTransform)
		{
			return this.FnTable.SetOverlayTransformTrackedDeviceRelative(ulOverlayHandle, unTrackedDevice, ref pmatTrackedDeviceToOverlayTransform);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000039BC File Offset: 0x00001BBC
		public EVROverlayError GetOverlayTransformTrackedDeviceRelative(ulong ulOverlayHandle, ref uint punTrackedDevice, ref HmdMatrix34_t pmatTrackedDeviceToOverlayTransform)
		{
			punTrackedDevice = 0U;
			return this.FnTable.GetOverlayTransformTrackedDeviceRelative(ulOverlayHandle, ref punTrackedDevice, ref pmatTrackedDeviceToOverlayTransform);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x000039E4 File Offset: 0x00001BE4
		public EVROverlayError SetOverlayTransformTrackedDeviceComponent(ulong ulOverlayHandle, uint unDeviceIndex, string pchComponentName)
		{
			return this.FnTable.SetOverlayTransformTrackedDeviceComponent(ulOverlayHandle, unDeviceIndex, pchComponentName);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00003A08 File Offset: 0x00001C08
		public EVROverlayError GetOverlayTransformTrackedDeviceComponent(ulong ulOverlayHandle, ref uint punDeviceIndex, string pchComponentName, uint unComponentNameSize)
		{
			punDeviceIndex = 0U;
			return this.FnTable.GetOverlayTransformTrackedDeviceComponent(ulOverlayHandle, ref punDeviceIndex, pchComponentName, unComponentNameSize);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00003A30 File Offset: 0x00001C30
		public EVROverlayError ShowOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.ShowOverlay(ulOverlayHandle);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00003A50 File Offset: 0x00001C50
		public EVROverlayError HideOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.HideOverlay(ulOverlayHandle);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00003A70 File Offset: 0x00001C70
		public bool IsOverlayVisible(ulong ulOverlayHandle)
		{
			return this.FnTable.IsOverlayVisible(ulOverlayHandle);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00003A90 File Offset: 0x00001C90
		public EVROverlayError GetTransformForOverlayCoordinates(ulong ulOverlayHandle, ETrackingUniverseOrigin eTrackingOrigin, HmdVector2_t coordinatesInOverlay, ref HmdMatrix34_t pmatTransform)
		{
			return this.FnTable.GetTransformForOverlayCoordinates(ulOverlayHandle, eTrackingOrigin, coordinatesInOverlay, ref pmatTransform);
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00003AB4 File Offset: 0x00001CB4
		public bool PollNextOverlayEvent(ulong ulOverlayHandle, ref VREvent_t pEvent, uint uncbVREvent)
		{
			return this.FnTable.PollNextOverlayEvent(ulOverlayHandle, ref pEvent, uncbVREvent);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00003AD8 File Offset: 0x00001CD8
		public EVROverlayError GetOverlayInputMethod(ulong ulOverlayHandle, ref VROverlayInputMethod peInputMethod)
		{
			return this.FnTable.GetOverlayInputMethod(ulOverlayHandle, ref peInputMethod);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00003AFC File Offset: 0x00001CFC
		public EVROverlayError SetOverlayInputMethod(ulong ulOverlayHandle, VROverlayInputMethod eInputMethod)
		{
			return this.FnTable.SetOverlayInputMethod(ulOverlayHandle, eInputMethod);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00003B20 File Offset: 0x00001D20
		public EVROverlayError GetOverlayMouseScale(ulong ulOverlayHandle, ref HmdVector2_t pvecMouseScale)
		{
			return this.FnTable.GetOverlayMouseScale(ulOverlayHandle, ref pvecMouseScale);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00003B44 File Offset: 0x00001D44
		public EVROverlayError SetOverlayMouseScale(ulong ulOverlayHandle, ref HmdVector2_t pvecMouseScale)
		{
			return this.FnTable.SetOverlayMouseScale(ulOverlayHandle, ref pvecMouseScale);
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00003B68 File Offset: 0x00001D68
		public bool ComputeOverlayIntersection(ulong ulOverlayHandle, ref VROverlayIntersectionParams_t pParams, ref VROverlayIntersectionResults_t pResults)
		{
			return this.FnTable.ComputeOverlayIntersection(ulOverlayHandle, ref pParams, ref pResults);
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00003B8C File Offset: 0x00001D8C
		public bool HandleControllerOverlayInteractionAsMouse(ulong ulOverlayHandle, uint unControllerDeviceIndex)
		{
			return this.FnTable.HandleControllerOverlayInteractionAsMouse(ulOverlayHandle, unControllerDeviceIndex);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00003BB0 File Offset: 0x00001DB0
		public bool IsHoverTargetOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.IsHoverTargetOverlay(ulOverlayHandle);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00003BD0 File Offset: 0x00001DD0
		public ulong GetGamepadFocusOverlay()
		{
			return this.FnTable.GetGamepadFocusOverlay();
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00003BF0 File Offset: 0x00001DF0
		public EVROverlayError SetGamepadFocusOverlay(ulong ulNewFocusOverlay)
		{
			return this.FnTable.SetGamepadFocusOverlay(ulNewFocusOverlay);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00003C10 File Offset: 0x00001E10
		public EVROverlayError SetOverlayNeighbor(EOverlayDirection eDirection, ulong ulFrom, ulong ulTo)
		{
			return this.FnTable.SetOverlayNeighbor(eDirection, ulFrom, ulTo);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00003C34 File Offset: 0x00001E34
		public EVROverlayError MoveGamepadFocusToNeighbor(EOverlayDirection eDirection, ulong ulFrom)
		{
			return this.FnTable.MoveGamepadFocusToNeighbor(eDirection, ulFrom);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00003C58 File Offset: 0x00001E58
		public EVROverlayError SetOverlayTexture(ulong ulOverlayHandle, ref Texture_t pTexture)
		{
			return this.FnTable.SetOverlayTexture(ulOverlayHandle, ref pTexture);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00003C7C File Offset: 0x00001E7C
		public EVROverlayError ClearOverlayTexture(ulong ulOverlayHandle)
		{
			return this.FnTable.ClearOverlayTexture(ulOverlayHandle);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00003C9C File Offset: 0x00001E9C
		public EVROverlayError SetOverlayRaw(ulong ulOverlayHandle, IntPtr pvBuffer, uint unWidth, uint unHeight, uint unDepth)
		{
			return this.FnTable.SetOverlayRaw(ulOverlayHandle, pvBuffer, unWidth, unHeight, unDepth);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00003CC4 File Offset: 0x00001EC4
		public EVROverlayError SetOverlayFromFile(ulong ulOverlayHandle, string pchFilePath)
		{
			return this.FnTable.SetOverlayFromFile(ulOverlayHandle, pchFilePath);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00003CE8 File Offset: 0x00001EE8
		public EVROverlayError GetOverlayTexture(ulong ulOverlayHandle, ref IntPtr pNativeTextureHandle, IntPtr pNativeTextureRef, ref uint pWidth, ref uint pHeight, ref uint pNativeFormat, ref EGraphicsAPIConvention pAPI, ref EColorSpace pColorSpace)
		{
			pWidth = 0U;
			pHeight = 0U;
			pNativeFormat = 0U;
			return this.FnTable.GetOverlayTexture(ulOverlayHandle, ref pNativeTextureHandle, pNativeTextureRef, ref pWidth, ref pHeight, ref pNativeFormat, ref pAPI, ref pColorSpace);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00003D20 File Offset: 0x00001F20
		public EVROverlayError ReleaseNativeOverlayHandle(ulong ulOverlayHandle, IntPtr pNativeTextureHandle)
		{
			return this.FnTable.ReleaseNativeOverlayHandle(ulOverlayHandle, pNativeTextureHandle);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00003D44 File Offset: 0x00001F44
		public EVROverlayError GetOverlayTextureSize(ulong ulOverlayHandle, ref uint pWidth, ref uint pHeight)
		{
			pWidth = 0U;
			pHeight = 0U;
			return this.FnTable.GetOverlayTextureSize(ulOverlayHandle, ref pWidth, ref pHeight);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00003D6C File Offset: 0x00001F6C
		public EVROverlayError CreateDashboardOverlay(string pchOverlayKey, string pchOverlayFriendlyName, ref ulong pMainHandle, ref ulong pThumbnailHandle)
		{
			pMainHandle = 0UL;
			pThumbnailHandle = 0UL;
			return this.FnTable.CreateDashboardOverlay(pchOverlayKey, pchOverlayFriendlyName, ref pMainHandle, ref pThumbnailHandle);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00003D9C File Offset: 0x00001F9C
		public bool IsDashboardVisible()
		{
			return this.FnTable.IsDashboardVisible();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00003DBC File Offset: 0x00001FBC
		public bool IsActiveDashboardOverlay(ulong ulOverlayHandle)
		{
			return this.FnTable.IsActiveDashboardOverlay(ulOverlayHandle);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00003DDC File Offset: 0x00001FDC
		public EVROverlayError SetDashboardOverlaySceneProcess(ulong ulOverlayHandle, uint unProcessId)
		{
			return this.FnTable.SetDashboardOverlaySceneProcess(ulOverlayHandle, unProcessId);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00003E00 File Offset: 0x00002000
		public EVROverlayError GetDashboardOverlaySceneProcess(ulong ulOverlayHandle, ref uint punProcessId)
		{
			punProcessId = 0U;
			return this.FnTable.GetDashboardOverlaySceneProcess(ulOverlayHandle, ref punProcessId);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00003E24 File Offset: 0x00002024
		public void ShowDashboard(string pchOverlayToShow)
		{
			this.FnTable.ShowDashboard(pchOverlayToShow);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00003E38 File Offset: 0x00002038
		public uint GetPrimaryDashboardDevice()
		{
			return this.FnTable.GetPrimaryDashboardDevice();
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00003E58 File Offset: 0x00002058
		public EVROverlayError ShowKeyboard(int eInputMode, int eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText, bool bUseMinimalMode, ulong uUserValue)
		{
			return this.FnTable.ShowKeyboard(eInputMode, eLineInputMode, pchDescription, unCharMax, pchExistingText, bUseMinimalMode, uUserValue);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x00003E84 File Offset: 0x00002084
		public EVROverlayError ShowKeyboardForOverlay(ulong ulOverlayHandle, int eInputMode, int eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText, bool bUseMinimalMode, ulong uUserValue)
		{
			return this.FnTable.ShowKeyboardForOverlay(ulOverlayHandle, eInputMode, eLineInputMode, pchDescription, unCharMax, pchExistingText, bUseMinimalMode, uUserValue);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00003EB0 File Offset: 0x000020B0
		public uint GetKeyboardText(StringBuilder pchText, uint cchText)
		{
			return this.FnTable.GetKeyboardText(pchText, cchText);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00003ED1 File Offset: 0x000020D1
		public void HideKeyboard()
		{
			this.FnTable.HideKeyboard();
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00003EE3 File Offset: 0x000020E3
		public void SetKeyboardTransformAbsolute(ETrackingUniverseOrigin eTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToKeyboardTransform)
		{
			this.FnTable.SetKeyboardTransformAbsolute(eTrackingOrigin, ref pmatTrackingOriginToKeyboardTransform);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00003EF7 File Offset: 0x000020F7
		public void SetKeyboardPositionForOverlay(ulong ulOverlayHandle, HmdRect2_t avoidRect)
		{
			this.FnTable.SetKeyboardPositionForOverlay(ulOverlayHandle, avoidRect);
		}

		// Token: 0x04000111 RID: 273
		private IVROverlay FnTable;
	}
}
