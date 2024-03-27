using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Valve.VR
{
	// Token: 0x02000118 RID: 280
	public class CVRSystem
	{
		// Token: 0x06000425 RID: 1061 RVA: 0x00002050 File Offset: 0x00000250
		internal CVRSystem(IntPtr pInterface)
		{
			this.FnTable = (IVRSystem)Marshal.PtrToStructure(pInterface, typeof(IVRSystem));
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00002073 File Offset: 0x00000273
		public void GetRecommendedRenderTargetSize(ref uint pnWidth, ref uint pnHeight)
		{
			pnWidth = 0U;
			pnHeight = 0U;
			this.FnTable.GetRecommendedRenderTargetSize(ref pnWidth, ref pnHeight);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00002090 File Offset: 0x00000290
		public HmdMatrix44_t GetProjectionMatrix(EVREye eEye, float fNearZ, float fFarZ, EGraphicsAPIConvention eProjType)
		{
			return this.FnTable.GetProjectionMatrix(eEye, fNearZ, fFarZ, eProjType);
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x000020B4 File Offset: 0x000002B4
		public void GetProjectionRaw(EVREye eEye, ref float pfLeft, ref float pfRight, ref float pfTop, ref float pfBottom)
		{
			pfLeft = 0f;
			pfRight = 0f;
			pfTop = 0f;
			pfBottom = 0f;
			this.FnTable.GetProjectionRaw(eEye, ref pfLeft, ref pfRight, ref pfTop, ref pfBottom);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x000020EC File Offset: 0x000002EC
		public DistortionCoordinates_t ComputeDistortion(EVREye eEye, float fU, float fV)
		{
			return this.FnTable.ComputeDistortion(eEye, fU, fV);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x00002110 File Offset: 0x00000310
		public HmdMatrix34_t GetEyeToHeadTransform(EVREye eEye)
		{
			return this.FnTable.GetEyeToHeadTransform(eEye);
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00002130 File Offset: 0x00000330
		public bool GetTimeSinceLastVsync(ref float pfSecondsSinceLastVsync, ref ulong pulFrameCounter)
		{
			pfSecondsSinceLastVsync = 0f;
			pulFrameCounter = 0UL;
			return this.FnTable.GetTimeSinceLastVsync(ref pfSecondsSinceLastVsync, ref pulFrameCounter);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000215C File Offset: 0x0000035C
		public int GetD3D9AdapterIndex()
		{
			return this.FnTable.GetD3D9AdapterIndex();
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000217B File Offset: 0x0000037B
		public void GetDXGIOutputInfo(ref int pnAdapterIndex)
		{
			pnAdapterIndex = 0;
			this.FnTable.GetDXGIOutputInfo(ref pnAdapterIndex);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00002194 File Offset: 0x00000394
		public bool IsDisplayOnDesktop()
		{
			return this.FnTable.IsDisplayOnDesktop();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x000021B4 File Offset: 0x000003B4
		public bool SetDisplayVisibility(bool bIsVisibleOnDesktop)
		{
			return this.FnTable.SetDisplayVisibility(bIsVisibleOnDesktop);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000021D4 File Offset: 0x000003D4
		public void GetDeviceToAbsoluteTrackingPose(ETrackingUniverseOrigin eOrigin, float fPredictedSecondsToPhotonsFromNow, TrackedDevicePose_t[] pTrackedDevicePoseArray)
		{
			this.FnTable.GetDeviceToAbsoluteTrackingPose(eOrigin, fPredictedSecondsToPhotonsFromNow, pTrackedDevicePoseArray, (uint)pTrackedDevicePoseArray.Length);
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x000021EC File Offset: 0x000003EC
		public void ResetSeatedZeroPose()
		{
			this.FnTable.ResetSeatedZeroPose();
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00002200 File Offset: 0x00000400
		public HmdMatrix34_t GetSeatedZeroPoseToStandingAbsoluteTrackingPose()
		{
			return this.FnTable.GetSeatedZeroPoseToStandingAbsoluteTrackingPose();
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00002220 File Offset: 0x00000420
		public HmdMatrix34_t GetRawZeroPoseToStandingAbsoluteTrackingPose()
		{
			return this.FnTable.GetRawZeroPoseToStandingAbsoluteTrackingPose();
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00002240 File Offset: 0x00000440
		public uint GetSortedTrackedDeviceIndicesOfClass(ETrackedDeviceClass eTrackedDeviceClass, uint[] punTrackedDeviceIndexArray, uint unRelativeToTrackedDeviceIndex)
		{
			return this.FnTable.GetSortedTrackedDeviceIndicesOfClass(eTrackedDeviceClass, punTrackedDeviceIndexArray, (uint)punTrackedDeviceIndexArray.Length, unRelativeToTrackedDeviceIndex);
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00002268 File Offset: 0x00000468
		public EDeviceActivityLevel GetTrackedDeviceActivityLevel(uint unDeviceId)
		{
			return this.FnTable.GetTrackedDeviceActivityLevel(unDeviceId);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00002288 File Offset: 0x00000488
		public void ApplyTransform(ref TrackedDevicePose_t pOutputPose, ref TrackedDevicePose_t pTrackedDevicePose, ref HmdMatrix34_t pTransform)
		{
			this.FnTable.ApplyTransform(ref pOutputPose, ref pTrackedDevicePose, ref pTransform);
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000022A0 File Offset: 0x000004A0
		public uint GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole unDeviceType)
		{
			return this.FnTable.GetTrackedDeviceIndexForControllerRole(unDeviceType);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000022C0 File Offset: 0x000004C0
		public ETrackedControllerRole GetControllerRoleForTrackedDeviceIndex(uint unDeviceIndex)
		{
			return this.FnTable.GetControllerRoleForTrackedDeviceIndex(unDeviceIndex);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000022E0 File Offset: 0x000004E0
		public ETrackedDeviceClass GetTrackedDeviceClass(uint unDeviceIndex)
		{
			return this.FnTable.GetTrackedDeviceClass(unDeviceIndex);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00002300 File Offset: 0x00000500
		public bool IsTrackedDeviceConnected(uint unDeviceIndex)
		{
			return this.FnTable.IsTrackedDeviceConnected(unDeviceIndex);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00002320 File Offset: 0x00000520
		public bool GetBoolTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetBoolTrackedDeviceProperty(unDeviceIndex, prop, ref pError);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00002344 File Offset: 0x00000544
		public float GetFloatTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetFloatTrackedDeviceProperty(unDeviceIndex, prop, ref pError);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00002368 File Offset: 0x00000568
		public int GetInt32TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetInt32TrackedDeviceProperty(unDeviceIndex, prop, ref pError);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000238C File Offset: 0x0000058C
		public ulong GetUint64TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetUint64TrackedDeviceProperty(unDeviceIndex, prop, ref pError);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000023B0 File Offset: 0x000005B0
		public HmdMatrix34_t GetMatrix34TrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetMatrix34TrackedDeviceProperty(unDeviceIndex, prop, ref pError);
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000023D4 File Offset: 0x000005D4
		public uint GetStringTrackedDeviceProperty(uint unDeviceIndex, ETrackedDeviceProperty prop, StringBuilder pchValue, uint unBufferSize, ref ETrackedPropertyError pError)
		{
			return this.FnTable.GetStringTrackedDeviceProperty(unDeviceIndex, prop, pchValue, unBufferSize, ref pError);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000023FC File Offset: 0x000005FC
		public string GetPropErrorNameFromEnum(ETrackedPropertyError error)
		{
			IntPtr intPtr = this.FnTable.GetPropErrorNameFromEnum(error);
			return Marshal.PtrToStringAnsi(intPtr);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00002424 File Offset: 0x00000624
		public bool PollNextEvent(ref VREvent_t pEvent, uint uncbVREvent)
		{
			return this.FnTable.PollNextEvent(ref pEvent, uncbVREvent);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00002448 File Offset: 0x00000648
		public bool PollNextEventWithPose(ETrackingUniverseOrigin eOrigin, ref VREvent_t pEvent, uint uncbVREvent, ref TrackedDevicePose_t pTrackedDevicePose)
		{
			return this.FnTable.PollNextEventWithPose(eOrigin, ref pEvent, uncbVREvent, ref pTrackedDevicePose);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000246C File Offset: 0x0000066C
		public string GetEventTypeNameFromEnum(EVREventType eType)
		{
			IntPtr intPtr = this.FnTable.GetEventTypeNameFromEnum(eType);
			return Marshal.PtrToStringAnsi(intPtr);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x00002494 File Offset: 0x00000694
		public HiddenAreaMesh_t GetHiddenAreaMesh(EVREye eEye)
		{
			return this.FnTable.GetHiddenAreaMesh(eEye);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x000024B4 File Offset: 0x000006B4
		public bool GetControllerState(uint unControllerDeviceIndex, ref VRControllerState_t pControllerState)
		{
			return this.FnTable.GetControllerState(unControllerDeviceIndex, ref pControllerState);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000024D8 File Offset: 0x000006D8
		public bool GetControllerStateWithPose(ETrackingUniverseOrigin eOrigin, uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, ref TrackedDevicePose_t pTrackedDevicePose)
		{
			return this.FnTable.GetControllerStateWithPose(eOrigin, unControllerDeviceIndex, ref pControllerState, ref pTrackedDevicePose);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x000024FC File Offset: 0x000006FC
		public void TriggerHapticPulse(uint unControllerDeviceIndex, uint unAxisId, char usDurationMicroSec)
		{
			this.FnTable.TriggerHapticPulse(unControllerDeviceIndex, unAxisId, usDurationMicroSec);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00002514 File Offset: 0x00000714
		public string GetButtonIdNameFromEnum(EVRButtonId eButtonId)
		{
			IntPtr intPtr = this.FnTable.GetButtonIdNameFromEnum(eButtonId);
			return Marshal.PtrToStringAnsi(intPtr);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x0000253C File Offset: 0x0000073C
		public string GetControllerAxisTypeNameFromEnum(EVRControllerAxisType eAxisType)
		{
			IntPtr intPtr = this.FnTable.GetControllerAxisTypeNameFromEnum(eAxisType);
			return Marshal.PtrToStringAnsi(intPtr);
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00002564 File Offset: 0x00000764
		public bool CaptureInputFocus()
		{
			return this.FnTable.CaptureInputFocus();
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00002583 File Offset: 0x00000783
		public void ReleaseInputFocus()
		{
			this.FnTable.ReleaseInputFocus();
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00002598 File Offset: 0x00000798
		public bool IsInputFocusCapturedByAnotherProcess()
		{
			return this.FnTable.IsInputFocusCapturedByAnotherProcess();
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x000025B8 File Offset: 0x000007B8
		public uint DriverDebugRequest(uint unDeviceIndex, string pchRequest, string pchResponseBuffer, uint unResponseBufferSize)
		{
			return this.FnTable.DriverDebugRequest(unDeviceIndex, pchRequest, pchResponseBuffer, unResponseBufferSize);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000025DC File Offset: 0x000007DC
		public EVRFirmwareError PerformFirmwareUpdate(uint unDeviceIndex)
		{
			return this.FnTable.PerformFirmwareUpdate(unDeviceIndex);
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x000025FC File Offset: 0x000007FC
		public void AcknowledgeQuit_Exiting()
		{
			this.FnTable.AcknowledgeQuit_Exiting();
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000260E File Offset: 0x0000080E
		public void AcknowledgeQuit_UserPrompt()
		{
			this.FnTable.AcknowledgeQuit_UserPrompt();
		}

		// Token: 0x0400010A RID: 266
		private IVRSystem FnTable;
	}
}
