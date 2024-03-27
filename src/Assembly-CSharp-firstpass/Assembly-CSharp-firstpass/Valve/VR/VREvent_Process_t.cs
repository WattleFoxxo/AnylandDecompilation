using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000168 RID: 360
	public struct VREvent_Process_t
	{
		// Token: 0x040003C8 RID: 968
		public uint pid;

		// Token: 0x040003C9 RID: 969
		public uint oldPid;

		// Token: 0x040003CA RID: 970
		[MarshalAs(UnmanagedType.I1)]
		public bool bForced;
	}
}
