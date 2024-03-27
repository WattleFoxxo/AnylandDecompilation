using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001E0 RID: 480
	[CallbackIdentity(4505)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_URLChanged_t
	{
		// Token: 0x04000604 RID: 1540
		public const int k_iCallback = 4505;

		// Token: 0x04000605 RID: 1541
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000606 RID: 1542
		public string pchURL;

		// Token: 0x04000607 RID: 1543
		public string pchPostData;

		// Token: 0x04000608 RID: 1544
		[MarshalAs(UnmanagedType.I1)]
		public bool bIsRedirect;

		// Token: 0x04000609 RID: 1545
		public string pchPageTitle;

		// Token: 0x0400060A RID: 1546
		[MarshalAs(UnmanagedType.I1)]
		public bool bNewNavigation;
	}
}
