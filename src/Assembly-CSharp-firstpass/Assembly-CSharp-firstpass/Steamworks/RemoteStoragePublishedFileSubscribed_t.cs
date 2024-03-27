using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000227 RID: 551
	[CallbackIdentity(1321)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStoragePublishedFileSubscribed_t
	{
		// Token: 0x04000718 RID: 1816
		public const int k_iCallback = 1321;

		// Token: 0x04000719 RID: 1817
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400071A RID: 1818
		public AppId_t m_nAppID;
	}
}
