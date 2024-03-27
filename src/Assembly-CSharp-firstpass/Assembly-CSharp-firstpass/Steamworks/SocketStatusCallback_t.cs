using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	// Token: 0x02000216 RID: 534
	[CallbackIdentity(1201)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct SocketStatusCallback_t
	{
		// Token: 0x040006B6 RID: 1718
		public const int k_iCallback = 1201;

		// Token: 0x040006B7 RID: 1719
		public SNetSocket_t m_hSocket;

		// Token: 0x040006B8 RID: 1720
		public SNetListenSocket_t m_hListenSocket;

		// Token: 0x040006B9 RID: 1721
		public CSteamID m_steamIDRemote;

		// Token: 0x040006BA RID: 1722
		public int m_eSNetSocketState;
	}
}
