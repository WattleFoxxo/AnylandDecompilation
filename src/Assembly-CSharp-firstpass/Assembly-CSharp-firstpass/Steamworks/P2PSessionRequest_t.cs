using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000214 RID: 532
	[CallbackIdentity(1202)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct P2PSessionRequest_t
	{
		// Token: 0x040006B1 RID: 1713
		public const int k_iCallback = 1202;

		// Token: 0x040006B2 RID: 1714
		public CSteamID m_steamIDRemote;
	}
}
