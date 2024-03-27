using System;

namespace Valve.VR
{
	// Token: 0x02000174 RID: 372
	public struct VREvent_t
	{
		// Token: 0x040003E2 RID: 994
		public uint eventType;

		// Token: 0x040003E3 RID: 995
		public uint trackedDeviceIndex;

		// Token: 0x040003E4 RID: 996
		public float eventAgeSeconds;

		// Token: 0x040003E5 RID: 997
		public VREvent_Data_t data;
	}
}
