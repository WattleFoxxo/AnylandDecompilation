using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001E9 RID: 489
	[CallbackIdentity(4514)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_JSAlert_t
	{
		// Token: 0x04000632 RID: 1586
		public const int k_iCallback = 4514;

		// Token: 0x04000633 RID: 1587
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x04000634 RID: 1588
		public string pchMessage;
	}
}
