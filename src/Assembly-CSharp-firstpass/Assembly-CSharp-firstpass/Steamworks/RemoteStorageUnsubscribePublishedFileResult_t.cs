using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000221 RID: 545
	[CallbackIdentity(1315)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUnsubscribePublishedFileResult_t
	{
		// Token: 0x040006E5 RID: 1765
		public const int k_iCallback = 1315;

		// Token: 0x040006E6 RID: 1766
		public EResult m_eResult;

		// Token: 0x040006E7 RID: 1767
		public PublishedFileId_t m_nPublishedFileId;
	}
}
