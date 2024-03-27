using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000247 RID: 583
	[CallbackIdentity(143)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct ValidateAuthTicketResponse_t
	{
		// Token: 0x04000787 RID: 1927
		public const int k_iCallback = 143;

		// Token: 0x04000788 RID: 1928
		public CSteamID m_SteamID;

		// Token: 0x04000789 RID: 1929
		public EAuthSessionResponse m_eAuthSessionResponse;

		// Token: 0x0400078A RID: 1930
		public CSteamID m_OwnerSteamID;
	}
}
