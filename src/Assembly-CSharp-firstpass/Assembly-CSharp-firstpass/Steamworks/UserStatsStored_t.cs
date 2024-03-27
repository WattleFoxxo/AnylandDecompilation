using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x0200024E RID: 590
	[CallbackIdentity(1102)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserStatsStored_t
	{
		// Token: 0x0400079C RID: 1948
		public const int k_iCallback = 1102;

		// Token: 0x0400079D RID: 1949
		public ulong m_nGameID;

		// Token: 0x0400079E RID: 1950
		public EResult m_eResult;
	}
}
