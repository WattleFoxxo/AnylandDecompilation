using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000252 RID: 594
	[CallbackIdentity(1106)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardScoreUploaded_t
	{
		// Token: 0x040007AC RID: 1964
		public const int k_iCallback = 1106;

		// Token: 0x040007AD RID: 1965
		public byte m_bSuccess;

		// Token: 0x040007AE RID: 1966
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x040007AF RID: 1967
		public int m_nScore;

		// Token: 0x040007B0 RID: 1968
		public byte m_bScoreChanged;

		// Token: 0x040007B1 RID: 1969
		public int m_nGlobalRankNew;

		// Token: 0x040007B2 RID: 1970
		public int m_nGlobalRankPrevious;
	}
}
