using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000254 RID: 596
	[CallbackIdentity(1108)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsUnloaded_t
	{
		// Token: 0x040007B6 RID: 1974
		public const int k_iCallback = 1108;

		// Token: 0x040007B7 RID: 1975
		public CSteamID m_steamIDUser;
	}
}
