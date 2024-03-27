using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001DA RID: 474
	[CallbackIdentity(1801)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct GSStatsStored_t
	{
		// Token: 0x040005E8 RID: 1512
		public const int k_iCallback = 1801;

		// Token: 0x040005E9 RID: 1513
		public EResult m_eResult;

		// Token: 0x040005EA RID: 1514
		public CSteamID m_steamIDUser;
	}
}
