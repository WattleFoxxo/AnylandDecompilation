using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200022B RID: 555
	[CallbackIdentity(1325)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageUserVoteDetails_t
	{
		// Token: 0x04000724 RID: 1828
		public const int k_iCallback = 1325;

		// Token: 0x04000725 RID: 1829
		public EResult m_eResult;

		// Token: 0x04000726 RID: 1830
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000727 RID: 1831
		public EWorkshopVote m_eVote;
	}
}
