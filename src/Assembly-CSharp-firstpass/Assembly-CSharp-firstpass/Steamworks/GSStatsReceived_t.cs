using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001D9 RID: 473
	[CallbackIdentity(1800)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSStatsReceived_t
	{
		// Token: 0x040005E5 RID: 1509
		public const int k_iCallback = 1800;

		// Token: 0x040005E6 RID: 1510
		public EResult m_eResult;

		// Token: 0x040005E7 RID: 1511
		public CSteamID m_steamIDUser;
	}
}
