using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200023D RID: 573
	[CallbackIdentity(3409)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetUserItemVoteResult_t
	{
		// Token: 0x04000769 RID: 1897
		public const int k_iCallback = 3409;

		// Token: 0x0400076A RID: 1898
		public PublishedFileId_t m_nPublishedFileId;

		// Token: 0x0400076B RID: 1899
		public EResult m_eResult;

		// Token: 0x0400076C RID: 1900
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVotedUp;

		// Token: 0x0400076D RID: 1901
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVotedDown;

		// Token: 0x0400076E RID: 1902
		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVoteSkipped;
	}
}
