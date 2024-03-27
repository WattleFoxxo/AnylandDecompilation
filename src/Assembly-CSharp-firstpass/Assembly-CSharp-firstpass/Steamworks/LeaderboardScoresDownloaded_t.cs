using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000251 RID: 593
	[CallbackIdentity(1105)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardScoresDownloaded_t
	{
		// Token: 0x040007A8 RID: 1960
		public const int k_iCallback = 1105;

		// Token: 0x040007A9 RID: 1961
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x040007AA RID: 1962
		public SteamLeaderboardEntries_t m_hSteamLeaderboardEntries;

		// Token: 0x040007AB RID: 1963
		public int m_cEntryCount;
	}
}
