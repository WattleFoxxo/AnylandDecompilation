using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001E1 RID: 481
	[CallbackIdentity(4506)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_FinishedRequest_t
	{
		// Token: 0x0400060B RID: 1547
		public const int k_iCallback = 4506;

		// Token: 0x0400060C RID: 1548
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400060D RID: 1549
		public string pchURL;

		// Token: 0x0400060E RID: 1550
		public string pchPageTitle;
	}
}
