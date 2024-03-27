using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000250 RID: 592
	[CallbackIdentity(1104)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardFindResult_t
	{
		// Token: 0x040007A5 RID: 1957
		public const int k_iCallback = 1104;

		// Token: 0x040007A6 RID: 1958
		public SteamLeaderboard_t m_hSteamLeaderboard;

		// Token: 0x040007A7 RID: 1959
		public byte m_bLeaderboardFound;
	}
}
