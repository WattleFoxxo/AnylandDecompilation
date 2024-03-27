using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000226 RID: 550
	[CallbackIdentity(1320)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedItemVoteDetailsResult_t
	{
		// Token: 0x04000711 RID: 1809
		public const int k_iCallback = 1320;

		// Token: 0x04000712 RID: 1810
		public EResult m_eResult;

		// Token: 0x04000713 RID: 1811
		public PublishedFileId_t m_unPublishedFileId;

		// Token: 0x04000714 RID: 1812
		public int m_nVotesFor;

		// Token: 0x04000715 RID: 1813
		public int m_nVotesAgainst;

		// Token: 0x04000716 RID: 1814
		public int m_nReports;

		// Token: 0x04000717 RID: 1815
		public float m_fScore;
	}
}
