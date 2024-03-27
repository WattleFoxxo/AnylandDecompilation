using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001F1 RID: 497
	[CallbackIdentity(4526)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_HideToolTip_t
	{
		// Token: 0x04000650 RID: 1616
		public const int k_iCallback = 4526;

		// Token: 0x04000651 RID: 1617
		public HHTMLBrowser unBrowserHandle;
	}
}
