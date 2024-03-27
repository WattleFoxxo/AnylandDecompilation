using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000253 RID: 595
	[CallbackIdentity(1107)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct NumberOfCurrentPlayers_t
	{
		// Token: 0x040007B3 RID: 1971
		public const int k_iCallback = 1107;

		// Token: 0x040007B4 RID: 1972
		public byte m_bSuccess;

		// Token: 0x040007B5 RID: 1973
		public int m_cPlayers;
	}
}
