using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000125 RID: 293
	public class OpenVRInterop
	{
		// Token: 0x0600053C RID: 1340
		[DllImport("openvr_api", EntryPoint = "VR_InitInternal")]
		internal static extern uint InitInternal(ref EVRInitError peError, EVRApplicationType eApplicationType);

		// Token: 0x0600053D RID: 1341
		[DllImport("openvr_api", EntryPoint = "VR_ShutdownInternal")]
		internal static extern void ShutdownInternal();

		// Token: 0x0600053E RID: 1342
		[DllImport("openvr_api", EntryPoint = "VR_IsHmdPresent")]
		internal static extern bool IsHmdPresent();

		// Token: 0x0600053F RID: 1343
		[DllImport("openvr_api", EntryPoint = "VR_IsRuntimeInstalled")]
		internal static extern bool IsRuntimeInstalled();

		// Token: 0x06000540 RID: 1344
		[DllImport("openvr_api", EntryPoint = "VR_GetStringForHmdError")]
		internal static extern IntPtr GetStringForHmdError(EVRInitError error);

		// Token: 0x06000541 RID: 1345
		[DllImport("openvr_api", EntryPoint = "VR_GetGenericInterface")]
		internal static extern IntPtr GetGenericInterface([MarshalAs(UnmanagedType.LPStr)] [In] string pchInterfaceVersion, ref EVRInitError peError);

		// Token: 0x06000542 RID: 1346
		[DllImport("openvr_api", EntryPoint = "VR_IsInterfaceVersionValid")]
		internal static extern bool IsInterfaceVersionValid([MarshalAs(UnmanagedType.LPStr)] [In] string pchInterfaceVersion);

		// Token: 0x06000543 RID: 1347
		[DllImport("openvr_api", EntryPoint = "VR_GetInitToken")]
		internal static extern uint GetInitToken();
	}
}
