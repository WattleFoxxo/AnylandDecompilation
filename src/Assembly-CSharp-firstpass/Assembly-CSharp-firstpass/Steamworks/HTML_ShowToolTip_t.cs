using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001EF RID: 495
	[CallbackIdentity(4524)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_ShowToolTip_t
	{
		// Token: 0x0400064A RID: 1610
		public const int k_iCallback = 4524;

		// Token: 0x0400064B RID: 1611
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x0400064C RID: 1612
		public string pchMsg;
	}
}
