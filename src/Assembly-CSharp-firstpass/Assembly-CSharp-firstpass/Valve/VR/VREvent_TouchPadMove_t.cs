using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000166 RID: 358
	public struct VREvent_TouchPadMove_t
	{
		// Token: 0x040003C0 RID: 960
		[MarshalAs(UnmanagedType.I1)]
		public bool bFingerDown;

		// Token: 0x040003C1 RID: 961
		public float flSecondsFingerDown;

		// Token: 0x040003C2 RID: 962
		public float fValueXFirst;

		// Token: 0x040003C3 RID: 963
		public float fValueYFirst;

		// Token: 0x040003C4 RID: 964
		public float fValueXRaw;

		// Token: 0x040003C5 RID: 965
		public float fValueYRaw;
	}
}
