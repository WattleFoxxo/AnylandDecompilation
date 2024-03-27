using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001DC RID: 476
	[CallbackIdentity(4501)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_BrowserReady_t
	{
		// Token: 0x040005ED RID: 1517
		public const int k_iCallback = 4501;

		// Token: 0x040005EE RID: 1518
		public HHTMLBrowser unBrowserHandle;
	}
}
