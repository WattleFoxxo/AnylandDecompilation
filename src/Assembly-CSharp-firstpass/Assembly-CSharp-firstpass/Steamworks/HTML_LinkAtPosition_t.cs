using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001E8 RID: 488
	[CallbackIdentity(4513)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_LinkAtPosition_t
	{
		// Token: 0x0400062B RID: 1579
		public const int k_iCallback = 4513;

		// Token: 0x0400062C RID: 1580
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400062D RID: 1581
		public uint x;

		// Token: 0x0400062E RID: 1582
		public uint y;

		// Token: 0x0400062F RID: 1583
		public string pchURL;

		// Token: 0x04000630 RID: 1584
		[MarshalAs(UnmanagedType.I1)]
		public bool bInput;

		// Token: 0x04000631 RID: 1585
		[MarshalAs(UnmanagedType.I1)]
		public bool bLiveLink;
	}
}
