using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000230 RID: 560
	[CallbackIdentity(1330)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileUpdated_t
	{
		// Token: 0x0400073B RID: 1851
		public const int k_iCallback = 1330;

		// Token: 0x0400073C RID: 1852
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400073D RID: 1853
		public AppId_t m_nAppID;

		// Token: 0x0400073E RID: 1854
		public ulong m_ulUnused;
	}
}
