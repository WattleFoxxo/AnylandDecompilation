using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200023C RID: 572
	[CallbackIdentity(3408)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SetUserItemVoteResult_t
	{
		// Token: 0x04000765 RID: 1893
		public const int k_iCallback = 3408;

		// Token: 0x04000766 RID: 1894
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x04000767 RID: 1895
		public EResult m_eResult;

		// Token: 0x04000768 RID: 1896
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVoteUp;
	}
}
