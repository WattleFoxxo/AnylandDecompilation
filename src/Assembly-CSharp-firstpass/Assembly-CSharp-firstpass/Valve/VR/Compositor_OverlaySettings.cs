using System;
using System.Runtime.InteropServices;

namespace Valve.VR
{
	// Token: 0x02000178 RID: 376
	public struct Compositor_OverlaySettings
	{
		// Token: 0x040003F2 RID: 1010
		public uint size;

		// Token: 0x040003F3 RID: 1011
		[MarshalAs(UnmanagedType.I1)]
		public bool curved;

		// Token: 0x040003F4 RID: 1012
		[MarshalAs(UnmanagedType.I1)]
		public bool antialias;

		// Token: 0x040003F5 RID: 1013
		public float scale;

		// Token: 0x040003F6 RID: 1014
		public float distance;

		// Token: 0x040003F7 RID: 1015
		public float alpha;

		// Token: 0x040003F8 RID: 1016
		public float uOffset;

		// Token: 0x040003F9 RID: 1017
		public float vOffset;

		// Token: 0x040003FA RID: 1018
		public float uScale;

		// Token: 0x040003FB RID: 1019
		public float vScale;

		// Token: 0x040003FC RID: 1020
		public float gridDivs;

		// Token: 0x040003FD RID: 1021
		public float gridWidth;

		// Token: 0x040003FE RID: 1022
		public float gridScale;

		// Token: 0x040003FF RID: 1023
		public HmdMatrix44_t transform;
	}
}
