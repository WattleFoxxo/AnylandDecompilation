using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001DE RID: 478
	[CallbackIdentity(4503)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_StartRequest_t
	{
		// Token: 0x040005FC RID: 1532
		public const int k_iCallback = 4503;

		// Token: 0x040005FD RID: 1533
		public HHTMLBrowser unBrowserHandle;

		// Token: 0x040005FE RID: 1534
		public string pchURL;

		// Token: 0x040005FF RID: 1535
		public string pchTarget;

		// Token: 0x04000600 RID: 1536
		public string pchPostData;

		// Token: 0x04000601 RID: 1537
		[MarshalAs(UnmanagedType.I1)]
		public bool bIsRedirect;
	}
}
