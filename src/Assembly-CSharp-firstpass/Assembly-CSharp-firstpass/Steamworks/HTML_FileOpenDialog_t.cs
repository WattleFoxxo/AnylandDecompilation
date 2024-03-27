using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001EB RID: 491
	[CallbackIdentity(4516)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_FileOpenDialog_t
	{
		// Token: 0x04000638 RID: 1592
		public const int k_iCallback = 4516;

		// Token: 0x04000639 RID: 1593
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400063A RID: 1594
		public string pchTitle;

		// Token: 0x0400063B RID: 1595
		public string pchInitialFile;
	}
}
