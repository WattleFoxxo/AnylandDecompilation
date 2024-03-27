using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000257 RID: 599
	[CallbackIdentity(1111)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardUGCSet_t
	{
		// Token: 0x040007C0 RID: 1984
		public const int k_iCallback = 1111;

		// Token: 0x040007C1 RID: 1985
		public EResult m_eResult;

		// Token: 0x040007C2 RID: 1986
		public SteamLeaderboard_t m_hSteamLeaderboard;
	}
}
