using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001DF RID: 479
	[CallbackIdentity(4504)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_CloseBrowser_t
	{
		// Token: 0x04000602 RID: 1538
		public const int k_iCallback = 4504;

		// Token: 0x04000603 RID: 1539
		public HHTMLBrowser unBrowserHandle;
	}
}
