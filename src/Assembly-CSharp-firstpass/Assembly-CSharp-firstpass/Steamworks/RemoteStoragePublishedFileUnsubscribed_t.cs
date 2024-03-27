using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000228 RID: 552
	[CallbackIdentity(1322)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileUnsubscribed_t
	{
		// Token: 0x0400071B RID: 1819
		public const int k_iCallback = 1322;

		// Token: 0x0400071C RID: 1820
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400071D RID: 1821
		public AppId_t m_nAppID;
	}
}
