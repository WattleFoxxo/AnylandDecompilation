using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020002B9 RID: 697
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct LeaderboardEntry_t
	{
		// Token: 0x04000C7F RID: 3199
		public CSteamID m_steamIDUser;

		// Token: 0x04000C80 RID: 3200
		public int m_nGlobalRank;

		// Token: 0x04000C81 RID: 3201
		public int m_nScore;

		// Token: 0x04000C82 RID: 3202
		public int m_cDetails;

		// Token: 0x04000C83 RID: 3203
		public UGCHandle_t m_hUGC;
	}
}
