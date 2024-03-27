using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000215 RID: 533
	[CallbackIdentity(1203)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct P2PSessionConnectFail_t
	{
		// Token: 0x040006B3 RID: 1715
		public const int k_iCallback = 1203;

		// Token: 0x040006B4 RID: 1716
		public CSteamID m_steamIDRemote;

		// Token: 0x040006B5 RID: 1717
		public byte m_eP2PSessionError;
	}
}
