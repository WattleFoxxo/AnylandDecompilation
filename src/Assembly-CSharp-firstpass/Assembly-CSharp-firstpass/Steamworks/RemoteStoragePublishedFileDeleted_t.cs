using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000229 RID: 553
	[CallbackIdentity(1323)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileDeleted_t
	{
		// Token: 0x0400071E RID: 1822
		public const int k_iCallback = 1323;

		// Token: 0x0400071F RID: 1823
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000720 RID: 1824
		public AppId_t m_nAppID;
	}
}
