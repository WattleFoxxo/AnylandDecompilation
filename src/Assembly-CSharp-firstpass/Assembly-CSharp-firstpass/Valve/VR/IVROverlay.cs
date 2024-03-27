using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x020000A1 RID: 161
	public struct IVROverlay
	{
		// Token: 0x04000099 RID: 153
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._FindOverlay FindOverlay;

		// Token: 0x0400009A RID: 154
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._CreateOverlay CreateOverlay;

		// Token: 0x0400009B RID: 155
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._DestroyOverlay DestroyOverlay;

		// Token: 0x0400009C RID: 156
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetHighQualityOverlay SetHighQualityOverlay;

		// Token: 0x0400009D RID: 157
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetHighQualityOverlay GetHighQualityOverlay;

		// Token: 0x0400009E RID: 158
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayKey GetOverlayKey;

		// Token: 0x0400009F RID: 159
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayName GetOverlayName;

		// Token: 0x040000A0 RID: 160
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayImageData GetOverlayImageData;

		// Token: 0x040000A1 RID: 161
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayErrorNameFromEnum GetOverlayErrorNameFromEnum;

		// Token: 0x040000A2 RID: 162
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayRenderingPid SetOverlayRenderingPid;

		// Token: 0x040000A3 RID: 163
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayRenderingPid GetOverlayRenderingPid;

		// Token: 0x040000A4 RID: 164
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayFlag SetOverlayFlag;

		// Token: 0x040000A5 RID: 165
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayFlag GetOverlayFlag;

		// Token: 0x040000A6 RID: 166
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayColor SetOverlayColor;

		// Token: 0x040000A7 RID: 167
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayColor GetOverlayColor;

		// Token: 0x040000A8 RID: 168
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayAlpha SetOverlayAlpha;

		// Token: 0x040000A9 RID: 169
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayAlpha GetOverlayAlpha;

		// Token: 0x040000AA RID: 170
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTexelAspect SetOverlayTexelAspect;

		// Token: 0x040000AB RID: 171
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTexelAspect GetOverlayTexelAspect;

		// Token: 0x040000AC RID: 172
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlaySortOrder SetOverlaySortOrder;

		// Token: 0x040000AD RID: 173
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlaySortOrder GetOverlaySortOrder;

		// Token: 0x040000AE RID: 174
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayWidthInMeters SetOverlayWidthInMeters;

		// Token: 0x040000AF RID: 175
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayWidthInMeters GetOverlayWidthInMeters;

		// Token: 0x040000B0 RID: 176
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayAutoCurveDistanceRangeInMeters SetOverlayAutoCurveDistanceRangeInMeters;

		// Token: 0x040000B1 RID: 177
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayAutoCurveDistanceRangeInMeters GetOverlayAutoCurveDistanceRangeInMeters;

		// Token: 0x040000B2 RID: 178
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTextureColorSpace SetOverlayTextureColorSpace;

		// Token: 0x040000B3 RID: 179
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTextureColorSpace GetOverlayTextureColorSpace;

		// Token: 0x040000B4 RID: 180
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTextureBounds SetOverlayTextureBounds;

		// Token: 0x040000B5 RID: 181
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTextureBounds GetOverlayTextureBounds;

		// Token: 0x040000B6 RID: 182
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTransformType GetOverlayTransformType;

		// Token: 0x040000B7 RID: 183
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTransformAbsolute SetOverlayTransformAbsolute;

		// Token: 0x040000B8 RID: 184
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTransformAbsolute GetOverlayTransformAbsolute;

		// Token: 0x040000B9 RID: 185
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTransformTrackedDeviceRelative SetOverlayTransformTrackedDeviceRelative;

		// Token: 0x040000BA RID: 186
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTransformTrackedDeviceRelative GetOverlayTransformTrackedDeviceRelative;

		// Token: 0x040000BB RID: 187
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTransformTrackedDeviceComponent SetOverlayTransformTrackedDeviceComponent;

		// Token: 0x040000BC RID: 188
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTransformTrackedDeviceComponent GetOverlayTransformTrackedDeviceComponent;

		// Token: 0x040000BD RID: 189
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ShowOverlay ShowOverlay;

		// Token: 0x040000BE RID: 190
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._HideOverlay HideOverlay;

		// Token: 0x040000BF RID: 191
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._IsOverlayVisible IsOverlayVisible;

		// Token: 0x040000C0 RID: 192
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetTransformForOverlayCoordinates GetTransformForOverlayCoordinates;

		// Token: 0x040000C1 RID: 193
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._PollNextOverlayEvent PollNextOverlayEvent;

		// Token: 0x040000C2 RID: 194
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayInputMethod GetOverlayInputMethod;

		// Token: 0x040000C3 RID: 195
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayInputMethod SetOverlayInputMethod;

		// Token: 0x040000C4 RID: 196
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayMouseScale GetOverlayMouseScale;

		// Token: 0x040000C5 RID: 197
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayMouseScale SetOverlayMouseScale;

		// Token: 0x040000C6 RID: 198
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ComputeOverlayIntersection ComputeOverlayIntersection;

		// Token: 0x040000C7 RID: 199
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._HandleControllerOverlayInteractionAsMouse HandleControllerOverlayInteractionAsMouse;

		// Token: 0x040000C8 RID: 200
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._IsHoverTargetOverlay IsHoverTargetOverlay;

		// Token: 0x040000C9 RID: 201
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetGamepadFocusOverlay GetGamepadFocusOverlay;

		// Token: 0x040000CA RID: 202
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetGamepadFocusOverlay SetGamepadFocusOverlay;

		// Token: 0x040000CB RID: 203
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayNeighbor SetOverlayNeighbor;

		// Token: 0x040000CC RID: 204
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._MoveGamepadFocusToNeighbor MoveGamepadFocusToNeighbor;

		// Token: 0x040000CD RID: 205
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayTexture SetOverlayTexture;

		// Token: 0x040000CE RID: 206
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ClearOverlayTexture ClearOverlayTexture;

		// Token: 0x040000CF RID: 207
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayRaw SetOverlayRaw;

		// Token: 0x040000D0 RID: 208
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetOverlayFromFile SetOverlayFromFile;

		// Token: 0x040000D1 RID: 209
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTexture GetOverlayTexture;

		// Token: 0x040000D2 RID: 210
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ReleaseNativeOverlayHandle ReleaseNativeOverlayHandle;

		// Token: 0x040000D3 RID: 211
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetOverlayTextureSize GetOverlayTextureSize;

		// Token: 0x040000D4 RID: 212
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._CreateDashboardOverlay CreateDashboardOverlay;

		// Token: 0x040000D5 RID: 213
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._IsDashboardVisible IsDashboardVisible;

		// Token: 0x040000D6 RID: 214
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._IsActiveDashboardOverlay IsActiveDashboardOverlay;

		// Token: 0x040000D7 RID: 215
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetDashboardOverlaySceneProcess SetDashboardOverlaySceneProcess;

		// Token: 0x040000D8 RID: 216
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetDashboardOverlaySceneProcess GetDashboardOverlaySceneProcess;

		// Token: 0x040000D9 RID: 217
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ShowDashboard ShowDashboard;

		// Token: 0x040000DA RID: 218
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetPrimaryDashboardDevice GetPrimaryDashboardDevice;

		// Token: 0x040000DB RID: 219
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ShowKeyboard ShowKeyboard;

		// Token: 0x040000DC RID: 220
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._ShowKeyboardForOverlay ShowKeyboardForOverlay;

		// Token: 0x040000DD RID: 221
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._GetKeyboardText GetKeyboardText;

		// Token: 0x040000DE RID: 222
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._HideKeyboard HideKeyboard;

		// Token: 0x040000DF RID: 223
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetKeyboardTransformAbsolute SetKeyboardTransformAbsolute;

		// Token: 0x040000E0 RID: 224
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVROverlay._SetKeyboardPositionForOverlay SetKeyboardPositionForOverlay;

		// Token: 0x020000A2 RID: 162
		// (Invoke) Token: 0x06000262 RID: 610
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _FindOverlay(string pchOverlayKey, ref ulong pOverlayHandle);

		// Token: 0x020000A3 RID: 163
		// (Invoke) Token: 0x06000266 RID: 614
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _CreateOverlay(string pchOverlayKey, string pchOverlayFriendlyName, ref ulong pOverlayHandle);

		// Token: 0x020000A4 RID: 164
		// (Invoke) Token: 0x0600026A RID: 618
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _DestroyOverlay(ulong ulOverlayHandle);

		// Token: 0x020000A5 RID: 165
		// (Invoke) Token: 0x0600026E RID: 622
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetHighQualityOverlay(ulong ulOverlayHandle);

		// Token: 0x020000A6 RID: 166
		// (Invoke) Token: 0x06000272 RID: 626
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _GetHighQualityOverlay();

		// Token: 0x020000A7 RID: 167
		// (Invoke) Token: 0x06000276 RID: 630
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetOverlayKey(ulong ulOverlayHandle, StringBuilder pchValue, uint unBufferSize, ref EVROverlayError pError);

		// Token: 0x020000A8 RID: 168
		// (Invoke) Token: 0x0600027A RID: 634
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetOverlayName(ulong ulOverlayHandle, StringBuilder pchValue, uint unBufferSize, ref EVROverlayError pError);

		// Token: 0x020000A9 RID: 169
		// (Invoke) Token: 0x0600027E RID: 638
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayImageData(ulong ulOverlayHandle, IntPtr pvBuffer, uint unBufferSize, ref uint punWidth, ref uint punHeight);

		// Token: 0x020000AA RID: 170
		// (Invoke) Token: 0x06000282 RID: 642
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetOverlayErrorNameFromEnum(EVROverlayError error);

		// Token: 0x020000AB RID: 171
		// (Invoke) Token: 0x06000286 RID: 646
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayRenderingPid(ulong ulOverlayHandle, uint unPID);

		// Token: 0x020000AC RID: 172
		// (Invoke) Token: 0x0600028A RID: 650
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetOverlayRenderingPid(ulong ulOverlayHandle);

		// Token: 0x020000AD RID: 173
		// (Invoke) Token: 0x0600028E RID: 654
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayFlag(ulong ulOverlayHandle, VROverlayFlags eOverlayFlag, bool bEnabled);

		// Token: 0x020000AE RID: 174
		// (Invoke) Token: 0x06000292 RID: 658
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayFlag(ulong ulOverlayHandle, VROverlayFlags eOverlayFlag, ref bool pbEnabled);

		// Token: 0x020000AF RID: 175
		// (Invoke) Token: 0x06000296 RID: 662
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayColor(ulong ulOverlayHandle, float fRed, float fGreen, float fBlue);

		// Token: 0x020000B0 RID: 176
		// (Invoke) Token: 0x0600029A RID: 666
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayColor(ulong ulOverlayHandle, ref float pfRed, ref float pfGreen, ref float pfBlue);

		// Token: 0x020000B1 RID: 177
		// (Invoke) Token: 0x0600029E RID: 670
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayAlpha(ulong ulOverlayHandle, float fAlpha);

		// Token: 0x020000B2 RID: 178
		// (Invoke) Token: 0x060002A2 RID: 674
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayAlpha(ulong ulOverlayHandle, ref float pfAlpha);

		// Token: 0x020000B3 RID: 179
		// (Invoke) Token: 0x060002A6 RID: 678
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTexelAspect(ulong ulOverlayHandle, float fTexelAspect);

		// Token: 0x020000B4 RID: 180
		// (Invoke) Token: 0x060002AA RID: 682
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTexelAspect(ulong ulOverlayHandle, ref float pfTexelAspect);

		// Token: 0x020000B5 RID: 181
		// (Invoke) Token: 0x060002AE RID: 686
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlaySortOrder(ulong ulOverlayHandle, uint unSortOrder);

		// Token: 0x020000B6 RID: 182
		// (Invoke) Token: 0x060002B2 RID: 690
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlaySortOrder(ulong ulOverlayHandle, ref uint punSortOrder);

		// Token: 0x020000B7 RID: 183
		// (Invoke) Token: 0x060002B6 RID: 694
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayWidthInMeters(ulong ulOverlayHandle, float fWidthInMeters);

		// Token: 0x020000B8 RID: 184
		// (Invoke) Token: 0x060002BA RID: 698
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayWidthInMeters(ulong ulOverlayHandle, ref float pfWidthInMeters);

		// Token: 0x020000B9 RID: 185
		// (Invoke) Token: 0x060002BE RID: 702
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayAutoCurveDistanceRangeInMeters(ulong ulOverlayHandle, float fMinDistanceInMeters, float fMaxDistanceInMeters);

		// Token: 0x020000BA RID: 186
		// (Invoke) Token: 0x060002C2 RID: 706
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayAutoCurveDistanceRangeInMeters(ulong ulOverlayHandle, ref float pfMinDistanceInMeters, ref float pfMaxDistanceInMeters);

		// Token: 0x020000BB RID: 187
		// (Invoke) Token: 0x060002C6 RID: 710
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTextureColorSpace(ulong ulOverlayHandle, EColorSpace eTextureColorSpace);

		// Token: 0x020000BC RID: 188
		// (Invoke) Token: 0x060002CA RID: 714
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTextureColorSpace(ulong ulOverlayHandle, ref EColorSpace peTextureColorSpace);

		// Token: 0x020000BD RID: 189
		// (Invoke) Token: 0x060002CE RID: 718
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTextureBounds(ulong ulOverlayHandle, ref VRTextureBounds_t pOverlayTextureBounds);

		// Token: 0x020000BE RID: 190
		// (Invoke) Token: 0x060002D2 RID: 722
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTextureBounds(ulong ulOverlayHandle, ref VRTextureBounds_t pOverlayTextureBounds);

		// Token: 0x020000BF RID: 191
		// (Invoke) Token: 0x060002D6 RID: 726
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTransformType(ulong ulOverlayHandle, ref VROverlayTransformType peTransformType);

		// Token: 0x020000C0 RID: 192
		// (Invoke) Token: 0x060002DA RID: 730
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTransformAbsolute(ulong ulOverlayHandle, ETrackingUniverseOrigin eTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToOverlayTransform);

		// Token: 0x020000C1 RID: 193
		// (Invoke) Token: 0x060002DE RID: 734
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTransformAbsolute(ulong ulOverlayHandle, ref ETrackingUniverseOrigin peTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToOverlayTransform);

		// Token: 0x020000C2 RID: 194
		// (Invoke) Token: 0x060002E2 RID: 738
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTransformTrackedDeviceRelative(ulong ulOverlayHandle, uint unTrackedDevice, ref HmdMatrix34_t pmatTrackedDeviceToOverlayTransform);

		// Token: 0x020000C3 RID: 195
		// (Invoke) Token: 0x060002E6 RID: 742
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTransformTrackedDeviceRelative(ulong ulOverlayHandle, ref uint punTrackedDevice, ref HmdMatrix34_t pmatTrackedDeviceToOverlayTransform);

		// Token: 0x020000C4 RID: 196
		// (Invoke) Token: 0x060002EA RID: 746
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTransformTrackedDeviceComponent(ulong ulOverlayHandle, uint unDeviceIndex, string pchComponentName);

		// Token: 0x020000C5 RID: 197
		// (Invoke) Token: 0x060002EE RID: 750
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTransformTrackedDeviceComponent(ulong ulOverlayHandle, ref uint punDeviceIndex, string pchComponentName, uint unComponentNameSize);

		// Token: 0x020000C6 RID: 198
		// (Invoke) Token: 0x060002F2 RID: 754
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _ShowOverlay(ulong ulOverlayHandle);

		// Token: 0x020000C7 RID: 199
		// (Invoke) Token: 0x060002F6 RID: 758
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _HideOverlay(ulong ulOverlayHandle);

		// Token: 0x020000C8 RID: 200
		// (Invoke) Token: 0x060002FA RID: 762
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsOverlayVisible(ulong ulOverlayHandle);

		// Token: 0x020000C9 RID: 201
		// (Invoke) Token: 0x060002FE RID: 766
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetTransformForOverlayCoordinates(ulong ulOverlayHandle, ETrackingUniverseOrigin eTrackingOrigin, HmdVector2_t coordinatesInOverlay, ref HmdMatrix34_t pmatTransform);

		// Token: 0x020000CA RID: 202
		// (Invoke) Token: 0x06000302 RID: 770
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _PollNextOverlayEvent(ulong ulOverlayHandle, ref VREvent_t pEvent, uint uncbVREvent);

		// Token: 0x020000CB RID: 203
		// (Invoke) Token: 0x06000306 RID: 774
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayInputMethod(ulong ulOverlayHandle, ref VROverlayInputMethod peInputMethod);

		// Token: 0x020000CC RID: 204
		// (Invoke) Token: 0x0600030A RID: 778
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayInputMethod(ulong ulOverlayHandle, VROverlayInputMethod eInputMethod);

		// Token: 0x020000CD RID: 205
		// (Invoke) Token: 0x0600030E RID: 782
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayMouseScale(ulong ulOverlayHandle, ref HmdVector2_t pvecMouseScale);

		// Token: 0x020000CE RID: 206
		// (Invoke) Token: 0x06000312 RID: 786
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayMouseScale(ulong ulOverlayHandle, ref HmdVector2_t pvecMouseScale);

		// Token: 0x020000CF RID: 207
		// (Invoke) Token: 0x06000316 RID: 790
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _ComputeOverlayIntersection(ulong ulOverlayHandle, ref VROverlayIntersectionParams_t pParams, ref VROverlayIntersectionResults_t pResults);

		// Token: 0x020000D0 RID: 208
		// (Invoke) Token: 0x0600031A RID: 794
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _HandleControllerOverlayInteractionAsMouse(ulong ulOverlayHandle, uint unControllerDeviceIndex);

		// Token: 0x020000D1 RID: 209
		// (Invoke) Token: 0x0600031E RID: 798
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsHoverTargetOverlay(ulong ulOverlayHandle);

		// Token: 0x020000D2 RID: 210
		// (Invoke) Token: 0x06000322 RID: 802
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _GetGamepadFocusOverlay();

		// Token: 0x020000D3 RID: 211
		// (Invoke) Token: 0x06000326 RID: 806
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetGamepadFocusOverlay(ulong ulNewFocusOverlay);

		// Token: 0x020000D4 RID: 212
		// (Invoke) Token: 0x0600032A RID: 810
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayNeighbor(EOverlayDirection eDirection, ulong ulFrom, ulong ulTo);

		// Token: 0x020000D5 RID: 213
		// (Invoke) Token: 0x0600032E RID: 814
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _MoveGamepadFocusToNeighbor(EOverlayDirection eDirection, ulong ulFrom);

		// Token: 0x020000D6 RID: 214
		// (Invoke) Token: 0x06000332 RID: 818
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayTexture(ulong ulOverlayHandle, ref Texture_t pTexture);

		// Token: 0x020000D7 RID: 215
		// (Invoke) Token: 0x06000336 RID: 822
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _ClearOverlayTexture(ulong ulOverlayHandle);

		// Token: 0x020000D8 RID: 216
		// (Invoke) Token: 0x0600033A RID: 826
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayRaw(ulong ulOverlayHandle, IntPtr pvBuffer, uint unWidth, uint unHeight, uint unDepth);

		// Token: 0x020000D9 RID: 217
		// (Invoke) Token: 0x0600033E RID: 830
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetOverlayFromFile(ulong ulOverlayHandle, string pchFilePath);

		// Token: 0x020000DA RID: 218
		// (Invoke) Token: 0x06000342 RID: 834
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTexture(ulong ulOverlayHandle, ref IntPtr pNativeTextureHandle, IntPtr pNativeTextureRef, ref uint pWidth, ref uint pHeight, ref uint pNativeFormat, ref EGraphicsAPIConvention pAPI, ref EColorSpace pColorSpace);

		// Token: 0x020000DB RID: 219
		// (Invoke) Token: 0x06000346 RID: 838
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _ReleaseNativeOverlayHandle(ulong ulOverlayHandle, IntPtr pNativeTextureHandle);

		// Token: 0x020000DC RID: 220
		// (Invoke) Token: 0x0600034A RID: 842
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetOverlayTextureSize(ulong ulOverlayHandle, ref uint pWidth, ref uint pHeight);

		// Token: 0x020000DD RID: 221
		// (Invoke) Token: 0x0600034E RID: 846
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _CreateDashboardOverlay(string pchOverlayKey, string pchOverlayFriendlyName, ref ulong pMainHandle, ref ulong pThumbnailHandle);

		// Token: 0x020000DE RID: 222
		// (Invoke) Token: 0x06000352 RID: 850
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsDashboardVisible();

		// Token: 0x020000DF RID: 223
		// (Invoke) Token: 0x06000356 RID: 854
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsActiveDashboardOverlay(ulong ulOverlayHandle);

		// Token: 0x020000E0 RID: 224
		// (Invoke) Token: 0x0600035A RID: 858
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _SetDashboardOverlaySceneProcess(ulong ulOverlayHandle, uint unProcessId);

		// Token: 0x020000E1 RID: 225
		// (Invoke) Token: 0x0600035E RID: 862
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _GetDashboardOverlaySceneProcess(ulong ulOverlayHandle, ref uint punProcessId);

		// Token: 0x020000E2 RID: 226
		// (Invoke) Token: 0x06000362 RID: 866
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ShowDashboard(string pchOverlayToShow);

		// Token: 0x020000E3 RID: 227
		// (Invoke) Token: 0x06000366 RID: 870
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetPrimaryDashboardDevice();

		// Token: 0x020000E4 RID: 228
		// (Invoke) Token: 0x0600036A RID: 874
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _ShowKeyboard(int eInputMode, int eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText, bool bUseMinimalMode, ulong uUserValue);

		// Token: 0x020000E5 RID: 229
		// (Invoke) Token: 0x0600036E RID: 878
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVROverlayError _ShowKeyboardForOverlay(ulong ulOverlayHandle, int eInputMode, int eLineInputMode, string pchDescription, uint unCharMax, string pchExistingText, bool bUseMinimalMode, ulong uUserValue);

		// Token: 0x020000E6 RID: 230
		// (Invoke) Token: 0x06000372 RID: 882
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetKeyboardText(StringBuilder pchText, uint cchText);

		// Token: 0x020000E7 RID: 231
		// (Invoke) Token: 0x06000376 RID: 886
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _HideKeyboard();

		// Token: 0x020000E8 RID: 232
		// (Invoke) Token: 0x0600037A RID: 890
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetKeyboardTransformAbsolute(ETrackingUniverseOrigin eTrackingOrigin, ref HmdMatrix34_t pmatTrackingOriginToKeyboardTransform);

		// Token: 0x020000E9 RID: 233
		// (Invoke) Token: 0x0600037E RID: 894
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _SetKeyboardPositionForOverlay(ulong ulOverlayHandle, HmdRect2_t avoidRect);
	}
}
