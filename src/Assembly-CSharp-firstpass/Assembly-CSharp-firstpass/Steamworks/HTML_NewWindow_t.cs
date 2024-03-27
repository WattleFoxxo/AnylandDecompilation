using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001EC RID: 492
	[CallbackIdentity(4521)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_NewWindow_t
	{
		// Token: 0x0400063C RID: 1596
		public const int k_iCallback = 4521;

		// Token: 0x0400063D RID: 1597
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400063E RID: 1598
		public string pchURL;

		// Token: 0x0400063F RID: 1599
		public uint unX;

		// Token: 0x04000640 RID: 1600
		public uint unY;

		// Token: 0x04000641 RID: 1601
		public uint unWide;

		// Token: 0x04000642 RID: 1602
		public uint unTall;

		// Token: 0x04000643 RID: 1603
		public HHTMLBrowser unNewWindow_BrowserHandle;
	}
}
