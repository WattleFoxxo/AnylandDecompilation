using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001F0 RID: 496
	[CallbackIdentity(4525)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_UpdateToolTip_t
	{
		// Token: 0x0400064D RID: 1613
		public const int k_iCallback = 4525;

		// Token: 0x0400064E RID: 1614
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400064F RID: 1615
		public string pchMsg;
	}
}
