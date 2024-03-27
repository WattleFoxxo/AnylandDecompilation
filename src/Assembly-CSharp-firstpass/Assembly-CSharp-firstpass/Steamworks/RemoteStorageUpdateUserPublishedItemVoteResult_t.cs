using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200022A RID: 554
	[CallbackIdentity(1324)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUpdateUserPublishedItemVoteResult_t
	{
		// Token: 0x04000721 RID: 1825
		public const int k_iCallback = 1324;

		// Token: 0x04000722 RID: 1826
		public EResult m_eResult;

		// Token: 0x04000723 RID: 1827
		public PublishedFileId_t m_nPublishedFileId;
	}
}
