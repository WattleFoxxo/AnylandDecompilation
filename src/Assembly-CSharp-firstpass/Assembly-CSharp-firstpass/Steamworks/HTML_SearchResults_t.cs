using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001E4 RID: 484
	[CallbackIdentity(4509)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_SearchResults_t
	{
		// Token: 0x04000615 RID: 1557
		public const int k_iCallback = 4509;

		// Token: 0x04000616 RID: 1558
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000617 RID: 1559
		public uint unResults;

		// Token: 0x04000618 RID: 1560
		public uint unCurrentMatch;
	}
}
