using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x020001C0 RID: 448
	[CallbackIdentity(335)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct ClanOfficerListResponse_t
	{
		// Token: 0x04000589 RID: 1417
		public const int k_iCallback = 335;

		// Token: 0x0400058A RID: 1418
		public CSteamID m_steamIDClan;

		// Token: 0x0400058B RID: 1419
		public int m_cOfficers;

		// Token: 0x0400058C RID: 1420
		public byte m_bSuccess;
	}
}
