using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001EE RID: 494
	[CallbackIdentity(4523)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_StatusText_t
	{
		// Token: 0x04000647 RID: 1607
		public const int k_iCallback = 4523;

		// Token: 0x04000648 RID: 1608
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000649 RID: 1609
		public string pchMsg;
	}
}
