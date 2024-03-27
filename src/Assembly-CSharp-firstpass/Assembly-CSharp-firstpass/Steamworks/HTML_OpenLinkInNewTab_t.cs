using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001E2 RID: 482
	[CallbackIdentity(4507)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_OpenLinkInNewTab_t
	{
		// Token: 0x0400060F RID: 1551
		public const int k_iCallback = 4507;

		// Token: 0x04000610 RID: 1552
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000611 RID: 1553
		public string pchURL;
	}
}
