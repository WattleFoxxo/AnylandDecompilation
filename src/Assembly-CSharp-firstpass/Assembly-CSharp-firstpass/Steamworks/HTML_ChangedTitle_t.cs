using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001E3 RID: 483
	[CallbackIdentity(4508)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_ChangedTitle_t
	{
		// Token: 0x04000612 RID: 1554
		public const int k_iCallback = 4508;

		// Token: 0x04000613 RID: 1555
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000614 RID: 1556
		public string pchTitle;
	}
}
