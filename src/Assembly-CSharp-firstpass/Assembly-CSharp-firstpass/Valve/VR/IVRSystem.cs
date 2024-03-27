using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000002 RID: 2
	public struct IVRSystem
	{
		// Token: 0x04000001 RID: 1
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetRecommendedRenderTargetSize GetRecommendedRenderTargetSize;

		// Token: 0x04000002 RID: 2
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetProjectionMatrix GetProjectionMatrix;

		// Token: 0x04000003 RID: 3
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetProjectionRaw GetProjectionRaw;

		// Token: 0x04000004 RID: 4
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._ComputeDistortion ComputeDistortion;

		// Token: 0x04000005 RID: 5
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetEyeToHeadTransform GetEyeToHeadTransform;

		// Token: 0x04000006 RID: 6
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetTimeSinceLastVsync GetTimeSinceLastVsync;

		// Token: 0x04000007 RID: 7
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetD3D9AdapterIndex GetD3D9AdapterIndex;

		// Token: 0x04000008 RID: 8
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetDXGIOutputInfo GetDXGIOutputInfo;

		// Token: 0x04000009 RID: 9
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._IsDisplayOnDesktop IsDisplayOnDesktop;

		// Token: 0x0400000A RID: 10
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._SetDisplayVisibility SetDisplayVisibility;

		// Token: 0x0400000B RID: 11
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetDeviceToAbsoluteTrackingPose GetDeviceToAbsoluteTrackingPose;

		// Token: 0x0400000C RID: 12
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._ResetSeatedZeroPose ResetSeatedZeroPose;

		// Token: 0x0400000D RID: 13
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetSeatedZeroPoseToStandingAbsoluteTrackingPose GetSeatedZeroPoseToStandingAbsoluteTrackingPose;

		// Token: 0x0400000E RID: 14
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetRawZeroPoseToStandingAbsoluteTrackingPose GetRawZeroPoseToStandingAbsoluteTrackingPose;

		// Token: 0x0400000F RID: 15
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetSortedTrackedDeviceIndicesOfClass GetSortedTrackedDeviceIndicesOfClass;

		// Token: 0x04000010 RID: 16
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetTrackedDeviceActivityLevel GetTrackedDeviceActivityLevel;

		// Token: 0x04000011 RID: 17
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._ApplyTransform ApplyTransform;

		// Token: 0x04000012 RID: 18
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetTrackedDeviceIndexForControllerRole GetTrackedDeviceIndexForControllerRole;

		// Token: 0x04000013 RID: 19
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetControllerRoleForTrackedDeviceIndex GetControllerRoleForTrackedDeviceIndex;

		// Token: 0x04000014 RID: 20
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetTrackedDeviceClass GetTrackedDeviceClass;

		// Token: 0x04000015 RID: 21
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._IsTrackedDeviceConnected IsTrackedDeviceConnected;

		// Token: 0x04000016 RID: 22
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetBoolTrackedDeviceProperty GetBoolTrackedDeviceProperty;

		// Token: 0x04000017 RID: 23
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetFloatTrackedDeviceProperty GetFloatTrackedDeviceProperty;

		// Token: 0x04000018 RID: 24
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetInt32TrackedDeviceProperty GetInt32TrackedDeviceProperty;

		// Token: 0x04000019 RID: 25
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetUint64TrackedDeviceProperty GetUint64TrackedDeviceProperty;

		// Token: 0x0400001A RID: 26
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetMatrix34TrackedDeviceProperty GetMatrix34TrackedDeviceProperty;

		// Token: 0x0400001B RID: 27
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetStringTrackedDeviceProperty GetStringTrackedDeviceProperty;

		// Token: 0x0400001C RID: 28
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetPropErrorNameFromEnum GetPropErrorNameFromEnum;

		// Token: 0x0400001D RID: 29
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._PollNextEvent PollNextEvent;

		// Token: 0x0400001E RID: 30
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._PollNextEventWithPose PollNextEventWithPose;

		// Token: 0x0400001F RID: 31
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetEventTypeNameFromEnum GetEventTypeNameFromEnum;

		// Token: 0x04000020 RID: 32
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetHiddenAreaMesh GetHiddenAreaMesh;

		// Token: 0x04000021 RID: 33
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetControllerState GetControllerState;

		// Token: 0x04000022 RID: 34
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetControllerStateWithPose GetControllerStateWithPose;

		// Token: 0x04000023 RID: 35
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._TriggerHapticPulse TriggerHapticPulse;

		// Token: 0x04000024 RID: 36
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetButtonIdNameFromEnum GetButtonIdNameFromEnum;

		// Token: 0x04000025 RID: 37
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._GetControllerAxisTypeNameFromEnum GetControllerAxisTypeNameFromEnum;

		// Token: 0x04000026 RID: 38
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._CaptureInputFocus CaptureInputFocus;

		// Token: 0x04000027 RID: 39
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._ReleaseInputFocus ReleaseInputFocus;

		// Token: 0x04000028 RID: 40
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._IsInputFocusCapturedByAnotherProcess IsInputFocusCapturedByAnotherProcess;

		// Token: 0x04000029 RID: 41
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._DriverDebugRequest DriverDebugRequest;

		// Token: 0x0400002A RID: 42
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._PerformFirmwareUpdate PerformFirmwareUpdate;

		// Token: 0x0400002B RID: 43
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._AcknowledgeQuit_Exiting AcknowledgeQuit_Exiting;

		// Token: 0x0400002C RID: 44
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRSystem._AcknowledgeQuit_UserPrompt AcknowledgeQuit_UserPrompt;

		// Token: 0x02000003 RID: 3
		// (Invoke) Token: 0x06000002 RID: 2
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetRecommendedRenderTargetSize(ref uint pnWidth, ref uint pnHeight);

		// Token: 0x02000004 RID: 4
		// (Invoke) Token: 0x06000006 RID: 6
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HmdMatrix44_t _GetProjectionMatrix(EVREye eEye, float fNearZ, float fFarZ, EGraphicsAPIConvention eProjType);

		// Token: 0x02000005 RID: 5
		// (Invoke) Token: 0x0600000A RID: 10
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetProjectionRaw(EVREye eEye, ref float pfLeft, ref float pfRight, ref float pfTop, ref float pfBottom);

		// Token: 0x02000006 RID: 6
		// (Invoke) Token: 0x0600000E RID: 14
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate DistortionCoordinates_t _ComputeDistortion(EVREye eEye, float fU, float fV);

		// Token: 0x02000007 RID: 7
		// (Invoke) Token: 0x06000012 RID: 18
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HmdMatrix34_t _GetEyeToHeadTransform(EVREye eEye);

		// Token: 0x02000008 RID: 8
		// (Invoke) Token: 0x06000016 RID: 22
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetTimeSinceLastVsync(ref float pfSecondsSinceLastVsync, ref ulong pulFrameCounter);

		// Token: 0x02000009 RID: 9
		// (Invoke) Token: 0x0600001A RID: 26
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate int _GetD3D9AdapterIndex();

		// Token: 0x0200000A RID: 10
		// (Invoke) Token: 0x0600001E RID: 30
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetDXGIOutputInfo(ref int pnAdapterIndex);

		// Token: 0x0200000B RID: 11
		// (Invoke) Token: 0x06000022 RID: 34
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsDisplayOnDesktop();

		// Token: 0x0200000C RID: 12
		// (Invoke) Token: 0x06000026 RID: 38
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _SetDisplayVisibility(bool bIsVisibleOnDesktop);

		// Token: 0x0200000D RID: 13
		// (Invoke) Token: 0x0600002A RID: 42
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _GetDeviceToAbsoluteTrackingPose(ETrackingUniverseOrigin eOrigin, float fPredictedSecondsToPhotonsFromNow, [In] [Out] TrackedDevicePose_t[] pTrackedDevicePoseArray, uint unTrackedDevicePoseArrayCount);

		// Token: 0x0200000E RID: 14
		// (Invoke) Token: 0x0600002E RID: 46
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ResetSeatedZeroPose();

		// Token: 0x0200000F RID: 15
		// (Invoke) Token: 0x06000032 RID: 50
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HmdMatrix34_t _GetSeatedZeroPoseToStandingAbsoluteTrackingPose();

		// Token: 0x02000010 RID: 16
		// (Invoke) Token: 0x06000036 RID: 54
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HmdMatrix34_t _GetRawZeroPoseToStandingAbsoluteTrackingPose();

		// Token: 0x02000011 RID: 17
		// (Invoke) Token: 0x0600003A RID: 58
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetSortedTrackedDeviceIndicesOfClass(ETrackedDeviceClass eTrackedDeviceClass, [In] [Out] uint[] punTrackedDeviceIndexArray, uint unTrackedDeviceIndexArrayCount, uint unRelativeToTrackedDeviceIndex);

		// Token: 0x02000012 RID: 18
		// (Invoke) Token: 0x0600003E RID: 62
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EDeviceActivityLevel _GetTrackedDeviceActivityLevel(uint unDeviceId);

		// Token: 0x02000013 RID: 19
		// (Invoke) Token: 0x06000042 RID: 66
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ApplyTransform(ref TrackedDevicePose_t pOutputPose, ref TrackedDevicePose_t pTrackedDevicePose, ref HmdMatrix34_t pTransform);

		// Token: 0x02000014 RID: 20
		// (Invoke) Token: 0x06000046 RID: 70
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole unDeviceType);

		// Token: 0x02000015 RID: 21
		// (Invoke) Token: 0x0600004A RID: 74
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ETrackedControllerRole _GetControllerRoleForTrackedDeviceIndex(uint unDeviceIndex);

		// Token: 0x02000016 RID: 22
		// (Invoke) Token: 0x0600004E RID: 78
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ETrackedDeviceClass _GetTrackedDeviceClass(uint unDeviceIndex);

		// Token: 0x02000017 RID: 23
		// (Invoke) Token: 0x06000052 RID: 82
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsTrackedDeviceConnected(uint unDeviceIndex);

		// Token: 0x02000018 RID: 24
		// (Invoke) Token: 0x06000056 RID: 86
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetBoolTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError);

		// Token: 0x02000019 RID: 25
		// (Invoke) Token: 0x0600005A RID: 90
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate float _GetFloatTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError);

		// Token: 0x0200001A RID: 26
		// (Invoke) Token: 0x0600005E RID: 94
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate int _GetInt32TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError);

		// Token: 0x0200001B RID: 27
		// (Invoke) Token: 0x06000062 RID: 98
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _GetUint64TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError);

		// Token: 0x0200001C RID: 28
		// (Invoke) Token: 0x06000066 RID: 102
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HmdMatrix34_t _GetMatrix34TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError);

		// Token: 0x0200001D RID: 29
		// (Invoke) Token: 0x0600006A RID: 106
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetStringTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, StringBuilder pchValue, uint unBufferSize, ref ETrackedPropertyError pError);

		// Token: 0x0200001E RID: 30
		// (Invoke) Token: 0x0600006E RID: 110
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetPropErrorNameFromEnum(ETrackedPropertyError error);

		// Token: 0x0200001F RID: 31
		// (Invoke) Token: 0x06000072 RID: 114
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _PollNextEvent(ref VREvent_t pEvent, uint uncbVREvent);

		// Token: 0x02000020 RID: 32
		// (Invoke) Token: 0x06000076 RID: 118
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _PollNextEventWithPose(ETrackingUniverseOrigin eOrigin, ref VREvent_t pEvent, uint uncbVREvent, ref TrackedDevicePose_t pTrackedDevicePose);

		// Token: 0x02000021 RID: 33
		// (Invoke) Token: 0x0600007A RID: 122
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetEventTypeNameFromEnum(EVREventType eType);

		// Token: 0x02000022 RID: 34
		// (Invoke) Token: 0x0600007E RID: 126
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate HiddenAreaMesh_t _GetHiddenAreaMesh(EVREye eEye);

		// Token: 0x02000023 RID: 35
		// (Invoke) Token: 0x06000082 RID: 130
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetControllerState(uint unControllerDeviceIndex, ref VRControllerState_t pControllerState);

		// Token: 0x02000024 RID: 36
		// (Invoke) Token: 0x06000086 RID: 134
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetControllerStateWithPose(ETrackingUniverseOrigin eOrigin, uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, ref TrackedDevicePose_t pTrackedDevicePose);

		// Token: 0x02000025 RID: 37
		// (Invoke) Token: 0x0600008A RID: 138
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _TriggerHapticPulse(uint unControllerDeviceIndex, uint unAxisId, char usDurationMicroSec);

		// Token: 0x02000026 RID: 38
		// (Invoke) Token: 0x0600008E RID: 142
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetButtonIdNameFromEnum(EVRButtonId eButtonId);

		// Token: 0x02000027 RID: 39
		// (Invoke) Token: 0x06000092 RID: 146
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetControllerAxisTypeNameFromEnum(EVRControllerAxisType eAxisType);

		// Token: 0x02000028 RID: 40
		// (Invoke) Token: 0x06000096 RID: 150
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _CaptureInputFocus();

		// Token: 0x02000029 RID: 41
		// (Invoke) Token: 0x0600009A RID: 154
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _ReleaseInputFocus();

		// Token: 0x0200002A RID: 42
		// (Invoke) Token: 0x0600009E RID: 158
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsInputFocusCapturedByAnotherProcess();

		// Token: 0x0200002B RID: 43
		// (Invoke) Token: 0x060000A2 RID: 162
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _DriverDebugRequest(uint unDeviceIndex, string pchRequest, string pchResponseBuffer, uint unResponseBufferSize);

		// Token: 0x0200002C RID: 44
		// (Invoke) Token: 0x060000A6 RID: 166
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRFirmwareError _PerformFirmwareUpdate(uint unDeviceIndex);

		// Token: 0x0200002D RID: 45
		// (Invoke) Token: 0x060000AA RID: 170
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _AcknowledgeQuit_Exiting();

		// Token: 0x0200002E RID: 46
		// (Invoke) Token: 0x060000AE RID: 174
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate void _AcknowledgeQuit_UserPrompt();
	}
}
