using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001EA RID: 490
	[CallbackIdentity(4515)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_JSConfirm_t
	{
		// Token: 0x04000635 RID: 1589
		public const int k_iCallback = 4515;

		// Token: 0x04000636 RID: 1590
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000637 RID: 1591
		public string pchMessage;
	}
}
