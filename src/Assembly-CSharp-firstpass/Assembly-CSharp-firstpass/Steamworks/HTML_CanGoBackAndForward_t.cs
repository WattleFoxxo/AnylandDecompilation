using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001E5 RID: 485
	[CallbackIdentity(4510)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_CanGoBackAndForward_t
	{
		// Token: 0x04000619 RID: 1561
		public const int k_iCallback = 4510;

		// Token: 0x0400061A RID: 1562
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400061B RID: 1563
		[MarshalAs(UnmanagedType.I1)]
		public bool bCanGoBack;

		// Token: 0x0400061C RID: 1564
		[MarshalAs(UnmanagedType.I1)]
		public bool bCanGoForward;
	}
}
