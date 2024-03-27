using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000258 RID: 600
	[CallbackIdentity(1112)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GlobalStatsReceived_t
	{
		// Token: 0x040007C3 RID: 1987
		public const int k_iCallback = 1112;

		// Token: 0x040007C4 RID: 1988
		public ulong m_nGameID;

		// Token: 0x040007C5 RID: 1989
		public EResult m_eResult;
	}
}
