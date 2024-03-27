using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001D8 RID: 472
	[CallbackIdentity(211)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ComputeNewPlayerCompatibilityResult_t
	{
		// Token: 0x040005DF RID: 1503
		public const int k_iCallback = 211;

		// Token: 0x040005E0 RID: 1504
		public EResult m_eResult;

		// Token: 0x040005E1 RID: 1505
		public int m_cPlayersThatDontLikeCandidate;

		// Token: 0x040005E2 RID: 1506
		public int m_cPlayersThatCandidateDoesntLike;

		// Token: 0x040005E3 RID: 1507
		public int m_cClanPlayersThatDontLikeCandidate;

		// Token: 0x040005E4 RID: 1508
		public CSteamID m_SteamIDCandidate;
	}
}
