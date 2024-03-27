using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000161 RID: 353
	public struct TrackedDevicePose_t
	{
		// Token: 0x040003AF RID: 943
		public HmdMatrix34_t mDeviceToAbsoluteTracking;

		// Token: 0x040003B0 RID: 944
		public HmdVector3_t vVelocity;

		// Token: 0x040003B1 RID: 945
		public HmdVector3_t vAngularVelocity;

		// Token: 0x040003B2 RID: 946
		public ETrackingResult eTrackingResult;

		// Token: 0x040003B3 RID: 947
		[MarshalAs(UnmanagedType.I1)]
		public bool bPoseIsValid;

		// Token: 0x040003B4 RID: 948
		[MarshalAs(UnmanagedType.I1)]
		public bool bDeviceIsConnected;
	}
}
