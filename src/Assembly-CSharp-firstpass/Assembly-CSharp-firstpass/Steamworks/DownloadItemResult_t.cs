using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200023A RID: 570
	[CallbackIdentity(3406)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct DownloadItemResult_t
	{
		// Token: 0x0400075D RID: 1885
		public const int k_iCallback = 3406;

		// Token: 0x0400075E RID: 1886
		public AppId_t m_unAppID;

		// Token: 0x0400075F RID: 1887
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000760 RID: 1888
		public EResult m_eResult;
	}
}
