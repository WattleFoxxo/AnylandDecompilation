using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001DB RID: 475
	[CallbackIdentity(1108)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GSStatsUnloaded_t
	{
		// Token: 0x040005EB RID: 1515
		public const int k_iCallback = 1108;

		// Token: 0x040005EC RID: 1516
		public CSteamID m_steamIDUser;
	}
}
