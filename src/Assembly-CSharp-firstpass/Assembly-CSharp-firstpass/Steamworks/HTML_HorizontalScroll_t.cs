using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001E6 RID: 486
	[CallbackIdentity(4511)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_HorizontalScroll_t
	{
		// Token: 0x0400061D RID: 1565
		public const int k_iCallback = 4511;

		// Token: 0x0400061E RID: 1566
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400061F RID: 1567
		public uint unScrollMax;

		// Token: 0x04000620 RID: 1568
		public uint unScrollCurrent;

		// Token: 0x04000621 RID: 1569
		public float flPageScale;

		// Token: 0x04000622 RID: 1570
		[MarshalAs(UnmanagedType.I1)]
		public bool bVisible;

		// Token: 0x04000623 RID: 1571
		public uint unPageSize;
	}
}
