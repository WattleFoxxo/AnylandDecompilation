using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001E7 RID: 487
	[CallbackIdentity(4512)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_VerticalScroll_t
	{
		// Token: 0x04000624 RID: 1572
		public const int k_iCallback = 4512;

		// Token: 0x04000625 RID: 1573
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000626 RID: 1574
		public uint unScrollMax;

		// Token: 0x04000627 RID: 1575
		public uint unScrollCurrent;

		// Token: 0x04000628 RID: 1576
		public float flPageScale;

		// Token: 0x04000629 RID: 1577
		[MarshalAs(UnmanagedType.I1)]
		public bool bVisible;

		// Token: 0x0400062A RID: 1578
		public uint unPageSize;
	}
}
