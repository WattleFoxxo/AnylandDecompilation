using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000040 RID: 64
	public struct IVRApplications
	{
		// Token: 0x0400003C RID: 60
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._AddApplicationManifest AddApplicationManifest;

		// Token: 0x0400003D RID: 61
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._RemoveApplicationManifest RemoveApplicationManifest;

		// Token: 0x0400003E RID: 62
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._IsApplicationInstalled IsApplicationInstalled;

		// Token: 0x0400003F RID: 63
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationCount GetApplicationCount;

		// Token: 0x04000040 RID: 64
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationKeyByIndex GetApplicationKeyByIndex;

		// Token: 0x04000041 RID: 65
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationKeyByProcessId GetApplicationKeyByProcessId;

		// Token: 0x04000042 RID: 66
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._LaunchApplication LaunchApplication;

		// Token: 0x04000043 RID: 67
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._LaunchTemplateApplication LaunchTemplateApplication;

		// Token: 0x04000044 RID: 68
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._LaunchApplicationFromMimeType LaunchApplicationFromMimeType;

		// Token: 0x04000045 RID: 69
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._LaunchDashboardOverlay LaunchDashboardOverlay;

		// Token: 0x04000046 RID: 70
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._CancelApplicationLaunch CancelApplicationLaunch;

		// Token: 0x04000047 RID: 71
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._IdentifyApplication IdentifyApplication;

		// Token: 0x04000048 RID: 72
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationProcessId GetApplicationProcessId;

		// Token: 0x04000049 RID: 73
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationsErrorNameFromEnum GetApplicationsErrorNameFromEnum;

		// Token: 0x0400004A RID: 74
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationPropertyString GetApplicationPropertyString;

		// Token: 0x0400004B RID: 75
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationPropertyBool GetApplicationPropertyBool;

		// Token: 0x0400004C RID: 76
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationPropertyUint64 GetApplicationPropertyUint64;

		// Token: 0x0400004D RID: 77
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._SetApplicationAutoLaunch SetApplicationAutoLaunch;

		// Token: 0x0400004E RID: 78
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationAutoLaunch GetApplicationAutoLaunch;

		// Token: 0x0400004F RID: 79
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._SetDefaultApplicationForMimeType SetDefaultApplicationForMimeType;

		// Token: 0x04000050 RID: 80
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetDefaultApplicationForMimeType GetDefaultApplicationForMimeType;

		// Token: 0x04000051 RID: 81
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationSupportedMimeTypes GetApplicationSupportedMimeTypes;

		// Token: 0x04000052 RID: 82
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationsThatSupportMimeType GetApplicationsThatSupportMimeType;

		// Token: 0x04000053 RID: 83
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationLaunchArguments GetApplicationLaunchArguments;

		// Token: 0x04000054 RID: 84
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetStartingApplication GetStartingApplication;

		// Token: 0x04000055 RID: 85
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetTransitionState GetTransitionState;

		// Token: 0x04000056 RID: 86
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._PerformApplicationPrelaunchCheck PerformApplicationPrelaunchCheck;

		// Token: 0x04000057 RID: 87
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._GetApplicationsTransitionStateNameFromEnum GetApplicationsTransitionStateNameFromEnum;

		// Token: 0x04000058 RID: 88
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._IsQuitUserPromptRequested IsQuitUserPromptRequested;

		// Token: 0x04000059 RID: 89
		[MarshalAs(UnmanagedType.FunctionPtr)]
		internal IVRApplications._LaunchInternalProcess LaunchInternalProcess;

		// Token: 0x02000041 RID: 65
		// (Invoke) Token: 0x060000EE RID: 238
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _AddApplicationManifest(string pchApplicationManifestFullPath, bool bTemporary);

		// Token: 0x02000042 RID: 66
		// (Invoke) Token: 0x060000F2 RID: 242
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _RemoveApplicationManifest(string pchApplicationManifestFullPath);

		// Token: 0x02000043 RID: 67
		// (Invoke) Token: 0x060000F6 RID: 246
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsApplicationInstalled(string pchAppKey);

		// Token: 0x02000044 RID: 68
		// (Invoke) Token: 0x060000FA RID: 250
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetApplicationCount();

		// Token: 0x02000045 RID: 69
		// (Invoke) Token: 0x060000FE RID: 254
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _GetApplicationKeyByIndex(uint unApplicationIndex, string pchAppKeyBuffer, uint unAppKeyBufferLen);

		// Token: 0x02000046 RID: 70
		// (Invoke) Token: 0x06000102 RID: 258
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _GetApplicationKeyByProcessId(uint unProcessId, string pchAppKeyBuffer, uint unAppKeyBufferLen);

		// Token: 0x02000047 RID: 71
		// (Invoke) Token: 0x06000106 RID: 262
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _LaunchApplication(string pchAppKey);

		// Token: 0x02000048 RID: 72
		// (Invoke) Token: 0x0600010A RID: 266
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _LaunchTemplateApplication(string pchTemplateAppKey, string pchNewAppKey, [In] [Out] AppOverrideKeys_t[] pKeys, uint unKeys);

		// Token: 0x02000049 RID: 73
		// (Invoke) Token: 0x0600010E RID: 270
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _LaunchApplicationFromMimeType(string pchMimeType, string pchArgs);

		// Token: 0x0200004A RID: 74
		// (Invoke) Token: 0x06000112 RID: 274
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _LaunchDashboardOverlay(string pchAppKey);

		// Token: 0x0200004B RID: 75
		// (Invoke) Token: 0x06000116 RID: 278
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _CancelApplicationLaunch(string pchAppKey);

		// Token: 0x0200004C RID: 76
		// (Invoke) Token: 0x0600011A RID: 282
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _IdentifyApplication(uint unProcessId, string pchAppKey);

		// Token: 0x0200004D RID: 77
		// (Invoke) Token: 0x0600011E RID: 286
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetApplicationProcessId(string pchAppKey);

		// Token: 0x0200004E RID: 78
		// (Invoke) Token: 0x06000122 RID: 290
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetApplicationsErrorNameFromEnum(EVRApplicationError error);

		// Token: 0x0200004F RID: 79
		// (Invoke) Token: 0x06000126 RID: 294
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetApplicationPropertyString(string pchAppKey, EVRApplicationProperty eProperty, string pchPropertyValueBuffer, uint unPropertyValueBufferLen, ref EVRApplicationError peError);

		// Token: 0x02000050 RID: 80
		// (Invoke) Token: 0x0600012A RID: 298
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetApplicationPropertyBool(string pchAppKey, EVRApplicationProperty eProperty, ref EVRApplicationError peError);

		// Token: 0x02000051 RID: 81
		// (Invoke) Token: 0x0600012E RID: 302
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate ulong _GetApplicationPropertyUint64(string pchAppKey, EVRApplicationProperty eProperty, ref EVRApplicationError peError);

		// Token: 0x02000052 RID: 82
		// (Invoke) Token: 0x06000132 RID: 306
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _SetApplicationAutoLaunch(string pchAppKey, bool bAutoLaunch);

		// Token: 0x02000053 RID: 83
		// (Invoke) Token: 0x06000136 RID: 310
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetApplicationAutoLaunch(string pchAppKey);

		// Token: 0x02000054 RID: 84
		// (Invoke) Token: 0x0600013A RID: 314
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _SetDefaultApplicationForMimeType(string pchAppKey, string pchMimeType);

		// Token: 0x02000055 RID: 85
		// (Invoke) Token: 0x0600013E RID: 318
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetDefaultApplicationForMimeType(string pchMimeType, string pchAppKeyBuffer, uint unAppKeyBufferLen);

		// Token: 0x02000056 RID: 86
		// (Invoke) Token: 0x06000142 RID: 322
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _GetApplicationSupportedMimeTypes(string pchAppKey, string pchMimeTypesBuffer, uint unMimeTypesBuffer);

		// Token: 0x02000057 RID: 87
		// (Invoke) Token: 0x06000146 RID: 326
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetApplicationsThatSupportMimeType(string pchMimeType, string pchAppKeysThatSupportBuffer, uint unAppKeysThatSupportBuffer);

		// Token: 0x02000058 RID: 88
		// (Invoke) Token: 0x0600014A RID: 330
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate uint _GetApplicationLaunchArguments(uint unHandle, string pchArgs, uint unArgs);

		// Token: 0x02000059 RID: 89
		// (Invoke) Token: 0x0600014E RID: 334
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _GetStartingApplication(string pchAppKeyBuffer, uint unAppKeyBufferLen);

		// Token: 0x0200005A RID: 90
		// (Invoke) Token: 0x06000152 RID: 338
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationTransitionState _GetTransitionState();

		// Token: 0x0200005B RID: 91
		// (Invoke) Token: 0x06000156 RID: 342
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _PerformApplicationPrelaunchCheck(string pchAppKey);

		// Token: 0x0200005C RID: 92
		// (Invoke) Token: 0x0600015A RID: 346
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate IntPtr _GetApplicationsTransitionStateNameFromEnum(EVRApplicationTransitionState state);

		// Token: 0x0200005D RID: 93
		// (Invoke) Token: 0x0600015E RID: 350
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate bool _IsQuitUserPromptRequested();

		// Token: 0x0200005E RID: 94
		// (Invoke) Token: 0x06000162 RID: 354
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		internal delegate EVRApplicationError _LaunchInternalProcess(string pchBinaryPath, string pchArguments, string pchWorkingDirectory);
	}
}
