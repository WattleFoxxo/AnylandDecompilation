using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001DD RID: 477
	[CallbackIdentity(4502)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_NeedsPaint_t
	{
		// Token: 0x040005EF RID: 1519
		public const int k_iCallback = 4502;

		// Token: 0x040005F0 RID: 1520
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040005F1 RID: 1521
		public IntPtr pBGRA;

		// Token: 0x040005F2 RID: 1522
		public uint unWide;

		// Token: 0x040005F3 RID: 1523
		public uint unTall;

		// Token: 0x040005F4 RID: 1524
		public uint unUpdateX;

		// Token: 0x040005F5 RID: 1525
		public uint unUpdateY;

		// Token: 0x040005F6 RID: 1526
		public uint unUpdateWide;

		// Token: 0x040005F7 RID: 1527
		public uint unUpdateTall;

		// Token: 0x040005F8 RID: 1528
		public uint unScrollX;

		// Token: 0x040005F9 RID: 1529
		public uint unScrollY;

		// Token: 0x040005FA RID: 1530
		public float flPageScale;

		// Token: 0x040005FB RID: 1531
		public uint unPageSerial;
	}
}
