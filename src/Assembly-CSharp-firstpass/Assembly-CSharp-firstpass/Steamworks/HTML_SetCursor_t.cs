using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001ED RID: 493
	[CallbackIdentity(4522)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_SetCursor_t
	{
		// Token: 0x04000644 RID: 1604
		public const int k_iCallback = 4522;

		// Token: 0x04000645 RID: 1605
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000646 RID: 1606
		public uint eMouseCursor;
	}
}
